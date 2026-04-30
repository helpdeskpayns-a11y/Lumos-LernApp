using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SpieleLernApp.Models;

namespace SpieleLernApp.Views;

public partial class Class1SubjectSelectionWindow : Window
{
    private readonly PlayerProfile _playerProfile;

    public Class1SubjectSelectionWindow(PlayerProfile playerProfile)
    {
        _playerProfile = playerProfile;

        InitializeComponent();
        ApplyBackgroundImage("Klasse 1.png");
        ClassImage.Source = CreateImage("Klasse 1.png");
        SubtitleTextBlock.Text = $"Wähle für {_playerProfile.Name} ein Fach aus.";
        LoadSubjectPreviewImages();
    }

    private void SubjectButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement element || element.Tag is not string subjectName)
        {
            return;
        }

        if (subjectName == "Mathematik")
        {
            var class1MathWindow = new Class1MathWindow(_playerProfile)
            {
                Owner = this
            };

            class1MathWindow.ShowDialog();
            return;
        }

        MessageBox.Show(
            $"{subjectName} ist vorbereitet. Der Aufgaben-Generator wird als nächstes angeschlossen.",
            "Fächerwahl",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void LoadSubjectPreviewImages()
    {
        TrySetSubjectImage(MathImage, MathPlaceholderText, "Mathematik");
        TrySetSubjectImage(GermanImage, GermanPlaceholderText, "Deutsch");
        TrySetSubjectImage(SachkundeImage, SachkundePlaceholderText, "Sachkunde");
        TrySetSubjectImage(MusicImage, MusicPlaceholderText, "Musik");
    }

    private void TrySetSubjectImage(Image imageControl, TextBlock placeholderText, string subjectName)
    {
        string? imagePath = FindSubjectImagePath(subjectName);
        if (imagePath is null)
        {
            placeholderText.Visibility = Visibility.Visible;
            return;
        }

        imageControl.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
        placeholderText.Visibility = Visibility.Collapsed;
    }

    private static string? FindSubjectImagePath(string subjectName)
    {
        string basePath = Path.Combine(AppContext.BaseDirectory, "Bilder", "Faecher", "Klasse1");
        string[] candidates =
        [
            Path.Combine(basePath, $"{subjectName}.png"),
            Path.Combine(basePath, $"{subjectName}.jpg"),
            Path.Combine(basePath, $"{subjectName}.jpeg"),
            Path.Combine(basePath, $"{subjectName}.webp")
        ];

        foreach (string candidate in candidates)
        {
            if (File.Exists(candidate))
            {
                return candidate;
            }
        }

        return null;
    }

    private void ApplyBackgroundImage(string imageName)
    {
        RootGrid.Background = new ImageBrush(CreateImage(imageName))
        {
            Stretch = Stretch.UniformToFill
        };
    }

    private static BitmapImage CreateImage(string imageName)
    {
        string imagePath = Path.Combine(AppContext.BaseDirectory, "Bilder", imageName);
        return new BitmapImage(new Uri(imagePath, UriKind.Absolute));
    }
}
