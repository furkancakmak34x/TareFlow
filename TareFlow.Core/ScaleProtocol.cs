using System.Globalization;
using System.Text.Json;

namespace TareFlow.Core;

/// <summary>
/// Kantar Agent (sunucu) ile Merkez (istemci) arasındaki satır-bazlı TCP protokolü.
/// Her ağırlık okuması tek satır JSON olarak gönderilir: {"w":12345,"s":true,"t":"..."}\n
/// </summary>
public static class ScaleProtocol
{
    public const int DefaultPort = 9100;

    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>Okumayı tek satırlık JSON'a (sonunda \n) çevirir.</summary>
    public static string Serialize(ScaleReading reading)
    {
        var dto = new Dto
        {
            W = reading.Weight,
            S = reading.Stable,
            T = reading.Timestamp.ToString("o", CultureInfo.InvariantCulture)
        };
        return JsonSerializer.Serialize(dto, Options) + "\n";
    }

    /// <summary>Gelen JSON satırını okumaya çevirir. Geçersizse null.</summary>
    public static ScaleReading? Deserialize(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return null;
        try
        {
            var dto = JsonSerializer.Deserialize<Dto>(line, Options);
            if (dto is null)
                return null;
            return new ScaleReading
            {
                Weight = dto.W,
                Stable = dto.S,
                Timestamp = DateTime.TryParse(
                    dto.T, CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind, out var ts) ? ts : DateTime.Now
            };
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private sealed class Dto
    {
        public int W { get; set; }
        public bool S { get; set; }
        public string T { get; set; } = "";
    }
}
