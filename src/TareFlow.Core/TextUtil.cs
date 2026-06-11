using System.Text;

namespace TareFlow.Core;

public static class TextUtil
{
    /// <summary>
    /// Türkçe karakterleri ASCII karşılıklarına çevirip büyük harfe dönüştürür.
    /// Termal yazıcı (ESC/POS) çıktısında karakter bozulmasını önlemek için kullanılır.
    /// </summary>
    public static string ConvertToAscii(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return "";

        return input
            .Replace("Ç", "C").Replace("ç", "C")
            .Replace("Ğ", "G").Replace("ğ", "G")
            .Replace("İ", "I").Replace("ı", "I").Replace("i", "I")
            .Replace("Ö", "O").Replace("ö", "O")
            .Replace("Ş", "S").Replace("ş", "S")
            .Replace("Ü", "U").Replace("ü", "U")
            .ToUpperInvariant();
    }
}
