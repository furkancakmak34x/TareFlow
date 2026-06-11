using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
using static TareFlow.Processes;
using static TareFlow.Properties.Resources;

public class LPTPrinter
{
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern SafeFileHandle CreateFile(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        IntPtr lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        IntPtr hTemplateFile);

    const uint GENERIC_WRITE = 0x40000000;
    const uint OPEN_EXISTING = 3;

    public void Print(string plate, string date, string secdate, string customer, string vendor, string product, string weight, string secweight, string total)
    {
        string template = Template;

        DateTime dt = DateTime.Parse(date);
        DateTime dtsec = DateTime.Parse(secdate);
        string newDate = dt.ToString("dd MMMM yyyy HH:mm", new System.Globalization.CultureInfo("tr-TR"));
        string newSecDate = dtsec.ToString("dd MMMM yyyy HH:mm", new System.Globalization.CultureInfo("tr-TR"));

        template = template.Replace("@PLATE@", ConvertToASCII(plate))
                           .Replace("@DATE@", ConvertToASCII(newDate))
                           .Replace("@SECDATE@", ConvertToASCII(newSecDate))
                           .Replace("@CUSTOMER@", ConvertToASCII(customer))
                           .Replace("@VENDOR@", ConvertToASCII(vendor))
                           .Replace("@PRODUCT@", ConvertToASCII(product))
                           .Replace("@WEIGHT@", ConvertToASCII(weight))
                           .Replace("@SECWEIGHT@", ConvertToASCII(secweight))
                           .Replace("@TOTAL@", ConvertToASCII(total));


        string text =   "\x1B\x40" +              // Reset
                        "\x1B\x4D\x03" +          // Font
                        " \x1B\x45\x01" +         // Bold
                        "\x1B\x21\x20" +          // 2x Wide - High
                        template +                // Template
                        new string('\n', 20) +    // Indent
                        "\x1B\x21\x00" +          // Normal Dimensions
                        "\x0A" +                  // Line Feed
                        "\x1B\x40";               // Reset
                        
        PrintOperation(text);
    }
    public void PrintOperation(string text)
    {
        SafeFileHandle handle = CreateFile(@"\\.\LPT1", GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

        if (handle.IsInvalid)
        {
            Console.WriteLine("Port açılamadı");
            return;
        }

        using (var fs = new FileStream(handle, FileAccess.Write))
        {
            byte[] buffer = Encoding.ASCII.GetBytes("\x1B\x40" + text);
            fs.Write(buffer, 0, buffer.Length);
            fs.Flush();
        }
    }
}