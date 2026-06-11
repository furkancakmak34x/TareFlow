using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TareFlow
{
    internal static class Program
    {
        static readonly Mutex mutex = new Mutex(true, "{AF8185D7-AD63-40EC-9E38-443A7AC9CE49}");

        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Home());
                mutex.ReleaseMutex();
            }

            else
            {
                MessageBox.Show("Uygulama zaten çalışıyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}