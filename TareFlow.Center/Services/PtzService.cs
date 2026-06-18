using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using System.Net.Sockets;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace TareFlow.Center.Services;

/// <summary>
/// ONVIF üzerinden PTZ (pan/tilt/zoom) kontrolü. Sürekli hareket (ContinuousMove)
/// ve durdurma (Stop) destekler. Profil token'ı ve servis adresleri ilk kullanımda
/// keşfedilip önbelleğe alınır. Tüm çağrılar hatayı yutar (kamera yoksa uygulama akışı bozulmaz).
/// </summary>
public sealed class PtzService
{
    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(4) };
    private readonly Dictionary<string, Endpoints> _cache = new();

    private sealed record Endpoints(string Media, string Ptz, string Token);

    private static string Host(CameraConfig c)
    {
        try { return new Uri(c.RtspUrl).Host; } catch { return c.RtspUrl; }
    }

    private static string SecurityHeader(string user, string pass)
    {
        var nonceBytes = RandomNumberGenerator.GetBytes(16);
        string nonce = Convert.ToBase64String(nonceBytes);
        string created = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
        byte[] createdB = Encoding.UTF8.GetBytes(created);
        byte[] passB = Encoding.UTF8.GetBytes(pass ?? "");
        byte[] combined = new byte[nonceBytes.Length + createdB.Length + passB.Length];
        Buffer.BlockCopy(nonceBytes, 0, combined, 0, nonceBytes.Length);
        Buffer.BlockCopy(createdB, 0, combined, nonceBytes.Length, createdB.Length);
        Buffer.BlockCopy(passB, 0, combined, nonceBytes.Length + createdB.Length, passB.Length);
        string digest = Convert.ToBase64String(SHA1.HashData(combined));
        return
            "<s:Header><Security s:mustUnderstand=\"1\" xmlns=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">" +
            "<UsernameToken><Username>" + user + "</Username>" +
            "<Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest\">" + digest + "</Password>" +
            "<Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\">" + nonce + "</Nonce>" +
            "<Created xmlns=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">" + created + "</Created>" +
            "</UsernameToken></Security></s:Header>";
    }

    private static async Task<string> PostAsync(string url, string user, string pass, string bodyInner)
    {
        string env =
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
            "<s:Envelope xmlns:s=\"http://www.w3.org/2003/05/soap-envelope\">" +
            SecurityHeader(user, pass) +
            "<s:Body>" + bodyInner + "</s:Body></s:Envelope>";
        using var content = new StringContent(env, Encoding.UTF8, "application/soap+xml");
        using var resp = await Http.PostAsync(url, content).ConfigureAwait(false);
        return await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
    }

    private async Task<Endpoints> GetEndpointsAsync(CameraConfig cam)
    {
        string host = Host(cam);
        if (_cache.TryGetValue(host, out var ep))
            return ep;

        int port = cam.OnvifPort > 0 ? cam.OnvifPort : 80;
        string baseUrl = $"http://{host}:{port}";
        string media = $"{baseUrl}/onvif/Media";
        string ptz = $"{baseUrl}/onvif/PTZ";

        string user = cam.User ?? "";
        string password = cam.Password ?? "";

        // 1) GetServices ile gerçek Media/PTZ adreslerini bul.
        try
        {
            string r = await PostAsync($"{baseUrl}/onvif/device_service", user, password,
                "<GetServices xmlns=\"http://www.onvif.org/ver10/device/wsdl\"><IncludeCapability>false</IncludeCapability></GetServices>");
            var doc = XDocument.Parse(r);
            foreach (var svc in doc.Descendants().Where(e => e.Name.LocalName == "Service"))
            {
                string ns = svc.Elements().FirstOrDefault(e => e.Name.LocalName == "Namespace")?.Value ?? "";
                string xaddr = svc.Elements().FirstOrDefault(e => e.Name.LocalName == "XAddr")?.Value ?? "";
                if (xaddr.Length == 0) continue;
                if (ns.Contains("/media")) media = xaddr;
                else if (ns.Contains("/ptz")) ptz = xaddr;
            }
        }
        catch { /* keşif başarısızsa varsayılan adresler denenir */ }

        // 2) Profil token'ı (elle verilmemişse GetProfiles ile).
        string token = cam.PtzProfileToken ?? "";
        if (string.IsNullOrWhiteSpace(token))
        {
            try
            {
                string r = await PostAsync(media, user, password,
                    "<GetProfiles xmlns=\"http://www.onvif.org/ver10/media/wsdl\"/>");
                var doc = XDocument.Parse(r);
                var prof = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "Profiles");
                token = prof?.Attribute("token")?.Value ?? "";
            }
            catch { /* token alınamadı */ }
        }

        ep = new Endpoints(media, ptz, token);
        _cache[host] = ep;
        return ep;
    }

    private static string Inv(double v) => v.ToString("0.###", CultureInfo.InvariantCulture);

    /// <summary>Sürekli hareket başlatır (pan/tilt/zoom hızları -1..1).</summary>
    public async Task MoveAsync(CameraConfig cam, double pan, double tilt, double zoom)
    {
        try
        {
            var ep = await GetEndpointsAsync(cam);
            if (string.IsNullOrWhiteSpace(ep.Token))
                return;
            string body =
                "<ContinuousMove xmlns=\"http://www.onvif.org/ver20/ptz/wsdl\">" +
                "<ProfileToken>" + ep.Token + "</ProfileToken><Velocity>" +
                "<PanTilt x=\"" + Inv(pan) + "\" y=\"" + Inv(tilt) + "\" xmlns=\"http://www.onvif.org/ver10/schema\"/>" +
                "<Zoom x=\"" + Inv(zoom) + "\" xmlns=\"http://www.onvif.org/ver10/schema\"/>" +
                "</Velocity></ContinuousMove>";
            await PostAsync(ep.Ptz, cam.User, cam.Password, body);
        }
        catch { /* yut */ }
    }

    /// <summary>Hareketi durdurur.</summary>
    public async Task StopAsync(CameraConfig cam)
    {
        try
        {
            var ep = await GetEndpointsAsync(cam);
            if (string.IsNullOrWhiteSpace(ep.Token))
                return;
            string body =
                "<Stop xmlns=\"http://www.onvif.org/ver20/ptz/wsdl\">" +
                "<ProfileToken>" + ep.Token + "</ProfileToken><PanTilt>true</PanTilt><Zoom>true</Zoom></Stop>";
            await PostAsync(ep.Ptz, cam.User, cam.Password, body);
        }
        catch { /* yut */ }
    }

    /// <summary>
    /// Kameranın gerçek RTSP yayın adresini ONVIF (GetStreamUri) ile sorar.
    /// Doğru RTSP yolunu bilmediğimiz kameralarda adresi otomatik doldurmak için kullanılır.
    /// </summary>
    public async Task<string?> GetStreamUriAsync(CameraConfig cam)
    {
        try
        {
            var ep = await GetEndpointsAsync(cam);
            if (string.IsNullOrWhiteSpace(ep.Token))
                return null;
            string body =
                "<GetStreamUri xmlns=\"http://www.onvif.org/ver10/media/wsdl\">" +
                "<StreamSetup>" +
                "<Stream xmlns=\"http://www.onvif.org/ver10/schema\">RTP-Unicast</Stream>" +
                "<Transport xmlns=\"http://www.onvif.org/ver10/schema\"><Protocol>RTSP</Protocol></Transport>" +
                "</StreamSetup>" +
                "<ProfileToken>" + ep.Token + "</ProfileToken></GetStreamUri>";
            string r = await PostAsync(ep.Media, cam.User, cam.Password, body);
            var doc = XDocument.Parse(r);
            string? uri = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "Uri")?.Value;
            return string.IsNullOrWhiteSpace(uri) ? null : uri.Trim();
        }
        catch { return null; }
    }
}
