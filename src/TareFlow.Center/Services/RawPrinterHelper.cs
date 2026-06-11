using System.Runtime.InteropServices;

namespace TareFlow.Center.Services;

/// <summary>
/// Windows yazıcı kuyruğuna ham (RAW) byte gönderir. ESC/POS termal yazıcılar için
/// sürücü işlemeden doğrudan komut göndermeyi sağlar (winspool.drv).
/// </summary>
internal static class RawPrinterHelper
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct DOCINFO
    {
        [MarshalAs(UnmanagedType.LPWStr)] public string pDocName;
        [MarshalAs(UnmanagedType.LPWStr)] public string? pOutputFile;
        [MarshalAs(UnmanagedType.LPWStr)] public string pDataType;
    }

    [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool OpenPrinter(string src, out IntPtr hPrinter, IntPtr pd);

    [DllImport("winspool.drv", SetLastError = true)]
    private static extern bool ClosePrinter(IntPtr hPrinter);

    [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool StartDocPrinter(IntPtr hPrinter, int level, ref DOCINFO di);

    [DllImport("winspool.drv", SetLastError = true)]
    private static extern bool EndDocPrinter(IntPtr hPrinter);

    [DllImport("winspool.drv", SetLastError = true)]
    private static extern bool StartPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.drv", SetLastError = true)]
    private static extern bool EndPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.drv", SetLastError = true)]
    private static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

    public static void SendBytes(string printerName, byte[] bytes)
    {
        if (!OpenPrinter(printerName, out IntPtr hPrinter, IntPtr.Zero))
            throw new InvalidOperationException($"Yazıcı açılamadı: {printerName}");

        IntPtr pUnmanaged = Marshal.AllocCoTaskMem(bytes.Length);
        try
        {
            Marshal.Copy(bytes, 0, pUnmanaged, bytes.Length);
            var di = new DOCINFO
            {
                pDocName = "TareFlow Kantar Fişi",
                pDataType = "RAW"
            };

            if (!StartDocPrinter(hPrinter, 1, ref di))
                throw new InvalidOperationException("StartDocPrinter başarısız.");
            try
            {
                StartPagePrinter(hPrinter);
                WritePrinter(hPrinter, pUnmanaged, bytes.Length, out _);
                EndPagePrinter(hPrinter);
            }
            finally
            {
                EndDocPrinter(hPrinter);
            }
        }
        finally
        {
            Marshal.FreeCoTaskMem(pUnmanaged);
            ClosePrinter(hPrinter);
        }
    }
}
