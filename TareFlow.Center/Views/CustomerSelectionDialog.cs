using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TareFlow.Core;
using TareFlow.Center.Services;

namespace TareFlow.Center.Views;

public class CustomerSelectionDialog : Window
{
    public string SelectedCustomer { get; private set; } = "";

    public CustomerSelectionDialog(Window? owner, List<string> existingCustomers, string defaultCustomer = "")
    {
        Title = "Müşteri Seçimi";
        Width = 420;
        Height = 230;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        if (owner != null) Owner = owner;
        ResizeMode = ResizeMode.NoResize;
        WindowStyle = WindowStyle.ToolWindow;
        
        SetResourceReference(BackgroundProperty, "BackgroundBrush");
        SetResourceReference(ForegroundProperty, "TextPrimaryBrush");

        var mainGrid = new Grid { Margin = new Thickness(24) };
        mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var lbl = new TextBlock
        {
            Text = "Kantar ücreti ödenmedi. Lütfen borcun kaydedileceği müşteriyi seçin veya yeni bir müşteri adı yazın:",
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(0, 0, 0, 14),
            FontFamily = new FontFamily("Google Sans"),
            FontSize = 13
        };
        lbl.SetResourceReference(TextBlock.ForegroundProperty, "TextSecondaryBrush");
        Grid.SetRow(lbl, 0);
        mainGrid.Children.Add(lbl);

        var cb = new ComboBox
        {
            IsEditable = true,
            ItemsSource = existingCustomers,
            Height = 36,
            VerticalContentAlignment = VerticalAlignment.Center,
            FontFamily = new FontFamily("Google Sans"),
            FontSize = 14
        };
        if (!string.IsNullOrWhiteSpace(defaultCustomer))
        {
            cb.Text = defaultCustomer;
        }
        
        Grid.SetRow(cb, 1);
        mainGrid.Children.Add(cb);

        var btnStack = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(0, 16, 0, 0)
        };
        Grid.SetRow(btnStack, 3);

        var btnOk = new Button
        {
            Content = "Kaydet",
            Width = 100,
            Height = 36,
            IsDefault = true,
            Margin = new Thickness(0, 0, 12, 0),
            Cursor = System.Windows.Input.Cursors.Hand
        };
        btnOk.SetResourceReference(StyleProperty, "PrimaryButton");
        btnOk.Click += (s, e) =>
        {
            string txt = cb.Text.Trim();
            if (string.IsNullOrWhiteSpace(txt))
            {
                MessageBox.Show("Müşteri adı boş olamaz.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SelectedCustomer = txt;
            DialogResult = true;
            Close();
        };

        var btnCancel = new Button
        {
            Content = "İptal",
            Width = 100,
            Height = 36,
            IsCancel = true,
            Cursor = System.Windows.Input.Cursors.Hand
        };
        btnCancel.SetResourceReference(StyleProperty, "SecondaryButton");
        btnCancel.Click += (s, e) =>
        {
            DialogResult = false;
            Close();
        };

        btnStack.Children.Add(btnOk);
        btnStack.Children.Add(btnCancel);
        mainGrid.Children.Add(btnStack);

        Content = mainGrid;
    }

    public static string? Show(WeighRepository repo, string defaultCustomer = "")
    {
        var owner = Application.Current.Windows.Cast<Window>().FirstOrDefault(x => x.IsActive) 
                    ?? Application.Current.MainWindow;
        
        var customerDebts = repo.ListCustomerDebts();
        var existingCustomers = customerDebts
            .Select(x => x.Customer)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        var dialog = new CustomerSelectionDialog(owner, existingCustomers, defaultCustomer);
        if (dialog.ShowDialog() == true)
        {
            return dialog.SelectedCustomer;
        }
        return null;
    }
}
