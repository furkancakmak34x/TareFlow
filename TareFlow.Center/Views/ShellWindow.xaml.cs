using System.Globalization;
using System.Windows;
using System.Windows.Threading;

namespace TareFlow.Center.Views;

public partial class ShellWindow : Window
{
    private static readonly CultureInfo Tr = new("tr-TR");

    public ShellWindow()
    {
        InitializeComponent();

        var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        timer.Tick += (_, _) => UpdateClock();
        timer.Start();
        UpdateClock();
    }

    private void UpdateClock()
    {
        var now = DateTime.Now;
        TxtDate.Text = now.ToString("dd MMMM yyyy, dddd", Tr);
        TxtTime.Text = now.ToString("HH:mm:ss", Tr);
    }

    private void BtnExit_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("Uygulamadan çıkmak istediğinizden emin misiniz?", "Çıkış",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            Application.Current.Shutdown();
        }
    }
}
