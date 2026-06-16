using System.Globalization;
using System.IO;
using System.Text;
using TareFlow.Core;

namespace TareFlow.Center.Services;

/// <summary>
/// 80mm USB termal yazıcı (ESC/POS) için kantar fişi üretir ve basar.
/// Font A'da satır genişliği 48 sütundur.
/// </summary>
public sealed class ReceiptPrinter
{
    private const int Width = 48;
    private static readonly CultureInfo Tr = new("tr-TR");

    // ESC/POS komutları
    private static readonly byte[] Init = { 0x1B, 0x40 };            // ESC @  (reset)
    private static readonly byte[] AlignCenter = { 0x1B, 0x61, 0x01 };
    private static readonly byte[] AlignLeft = { 0x1B, 0x61, 0x00 };
    private static readonly byte[] BoldOn = { 0x1B, 0x45, 0x01 };
    private static readonly byte[] BoldOff = { 0x1B, 0x45, 0x00 };
    private static readonly byte[] DoubleSize = { 0x1D, 0x21, 0x11 }; // GS ! (2x en/boy)
    private static readonly byte[] NormalSize = { 0x1D, 0x21, 0x00 };
    private static readonly byte[] FeedCut = { 0x1D, 0x56, 0x42, 0x00 }; // GS V B (besle+kes)

    private readonly CenterSettings _settings;
    public ReceiptPrinter(CenterSettings settings) => _settings = settings;

    public void Print(SecWeightRecord rec)
    {
        if (string.IsNullOrWhiteSpace(_settings.PrinterName))
            throw new InvalidOperationException("Yazıcı seçilmedi. Ayarlar'dan termal yazıcıyı seçin.");

        var bytes = Build(rec);
        RawPrinterHelper.SendBytes(_settings.PrinterName, bytes);
    }

    /// <summary>Fişin metin önizlemesini döndürür (UI'da göstermek için).</summary>
    public static string Preview(SecWeightRecord r) => BuildText(r);

    private static byte[] Build(SecWeightRecord r)
    {
        var ms = new MemoryStream();
        void W(byte[] b) => ms.Write(b, 0, b.Length);
        void T(string s) => W(Encoding.ASCII.GetBytes(TextUtil.ConvertToAscii(s)));
        void Line(string s = "") { T(s); W(new byte[] { 0x0A }); }

        W(Init);

        // Başlık
        W(AlignCenter); W(BoldOn); W(DoubleSize);
        Line("KANTAR FISI");
        W(NormalSize); W(BoldOff);
        Line();
        W(AlignLeft);

        Line(Sep());
        Line(Field("PLAKA", r.Plate));
        Line(Field("GIRIS", FormatDate(r.Date)));
        Line(Field("CIKIS", FormatDate(r.SecDate)));
        if (!string.IsNullOrWhiteSpace(r.Customer)) Line(Field("ALICI", r.Customer!));
        if (!string.IsNullOrWhiteSpace(r.Vendor)) Line(Field("SATICI", r.Vendor!));
        if (!string.IsNullOrWhiteSpace(r.Product)) Line(Field("URUN", r.Product!));
        Line(Sep());

        Line(Amount("1. TARTIM", r.Weight));
        Line(Amount("2. TARTIM", r.SecWeight));
        Line(Sep());

        // Net (vurgulu, çift boy)
        W(BoldOn); W(DoubleSize);
        Line(NetLine(r.Total));
        W(NormalSize); W(BoldOff);
        Line(Sep());

        W(AlignCenter);
        Line(DateTime.Now.ToString("dd MMMM yyyy HH:mm", Tr));
        Line();

        W(FeedCut);
        return ms.ToArray();
    }

    // --- Metin yardımcıları ---

    private static string BuildText(SecWeightRecord r)
    {
        var sb = new StringBuilder();
        sb.AppendLine("KANTAR FISI".PadLeft((Width + 11) / 2));
        sb.AppendLine(Sep());
        sb.AppendLine(Field("PLAKA", r.Plate));
        sb.AppendLine(Field("GIRIS", FormatDate(r.Date)));
        sb.AppendLine(Field("CIKIS", FormatDate(r.SecDate)));
        if (!string.IsNullOrWhiteSpace(r.Customer)) sb.AppendLine(Field("ALICI", r.Customer!));
        if (!string.IsNullOrWhiteSpace(r.Vendor)) sb.AppendLine(Field("SATICI", r.Vendor!));
        if (!string.IsNullOrWhiteSpace(r.Product)) sb.AppendLine(Field("URUN", r.Product!));
        sb.AppendLine(Sep());
        sb.AppendLine(Amount("1. TARTIM", r.Weight));
        sb.AppendLine(Amount("2. TARTIM", r.SecWeight));
        sb.AppendLine(Sep());
        sb.AppendLine(NetLine(r.Total));
        sb.AppendLine(Sep());
        return TextUtil.ConvertToAscii(sb.ToString());
    }

    private static string Sep() => new('-', Width);

    private static string Field(string label, string value)
        => $"{label,-7}: {value}";

    private static string Amount(string label, int kg)
    {
        string right = $"{kg.ToString("N0", Tr)} KG";
        string left = $"{label,-10}: ";
        return left + right.PadLeft(Width - left.Length);
    }

    private static string NetLine(int kg)
    {
        // Çift boy olduğu için görünür genişlik yarıdır (~24 sütun).
        string right = $"{kg.ToString("N0", Tr)} KG";
        string left = "NET: ";
        int half = Width / 2;
        return left + right.PadLeft(Math.Max(1, half - left.Length));
    }

    private static string FormatDate(string raw)
        => DateTime.TryParse(raw, out var dt)
            ? dt.ToString("dd MMMM yyyy HH:mm", Tr)
            : raw;
}
