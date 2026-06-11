namespace TareFlow.Core;

/// <summary>
/// Bassan BX-1 indikatöründen gelen ASCII seri verisini ayrıştırır.
/// Eski WinForms davranışı: satır boşluklarla bölünür, 3. parça (index 2) ağırlıktır.
/// </summary>
public static class ScaleParser
{
    /// <summary>
    /// Ham seri metinden ağırlık değerini çıkarır. Başarılıysa true döner.
    /// </summary>
    public static bool TryParse(string? raw, out int weight)
    {
        weight = 0;
        if (string.IsNullOrWhiteSpace(raw))
            return false;

        string[] parts = raw.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 3)
            return false;

        return int.TryParse(parts[2].Trim(), out weight);
    }
}
