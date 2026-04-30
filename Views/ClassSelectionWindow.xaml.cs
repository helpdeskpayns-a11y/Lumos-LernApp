using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpieleLernApp.Views;

public partial class ClassSelectionWindow : Window
{
    public ClassSelectionWindow()
    {
        InitializeComponent();
        ApplyBackgroundImage("Klassenauswahl.png");
        LoadClassImages();
    }

    private void ClassButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string className)
        {
            MessageBox.Show(
                $"{className} ist als nächster Bereich vorgesehen.",
                "Klassenauswahl",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }

    private void ApplyBackgroundImage(string imageName)
    {
        string imagePath = Path.Combine(AppContext.BaseDirectory, "Bilder", imageName);
        var image = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
        RootGrid.Background = new ImageBrush(image)
        {
            Stretch = Stretch.UniformToFill
        };
    }

    private void LoadClassImages()
    {
        Class1Image.Source = CreateImage("Klasse 1.png");
        Class2Image.Source = CreateImage("klasse 2.png");
        Class3Image.Source = CreateImage("Klasse 3#.png");
        Class4Image.Source = CreateImage("KLasse 4.png");
    }

    private static BitmapImage CreateImage(string imageName)
    {
        string imagePath = Path.Combine(AppContext.BaseDirectory, "Bilder", imageName);
        return new BitmapImage(new Uri(imagePath, UriKind.Absolute));
    }
}
