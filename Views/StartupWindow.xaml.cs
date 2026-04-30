using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SpieleLernApp.Models;
using SpieleLernApp.Services;

namespace SpieleLernApp.Views;

public partial class StartupWindow : Window
{
    private readonly PlayerProfileStore _playerProfileStore = new();
    private readonly bool _skipLoadingSequence;
    private IReadOnlyList<PlayerProfile> _players = [];

    public StartupWindow()
        : this(skipLoadingSequence: false)
    {
    }

    public StartupWindow(bool skipLoadingSequence)
    {
        _skipLoadingSequence = skipLoadingSequence;
        InitializeComponent();
        ApplyFullscreenMode();
        ApplyBackgroundImage("Ladebild.png");
        Loaded += StartupWindow_Loaded;
        DataObject.AddPastingHandler(PlayerAgeTextBox, OnPlayerAgeTextBoxPaste);
    }

    private async void StartupWindow_Loaded(object sender, RoutedEventArgs e)
    {
        if (_skipLoadingSequence)
        {
            ShowPlayerEntryDirectly();
            return;
        }

        await RunLoadingSequenceAsync();
    }

    private void ShowPlayerEntryDirectly()
    {
        _players = _playerProfileStore.LoadPlayers();
        LoadingPanel.Visibility = Visibility.Collapsed;

        if (_players.Count > 0)
        {
            ShowPlayerSelection();
        }
        else
        {
            ShowPlayerForm();
        }
    }

    private async Task RunLoadingSequenceAsync()
    {
        string[] loadingSteps =
        {
            "Lade Bilder und Startbereich...",
            "Lade gespeicherte Spieler...",
            "Bereite das Startmenü vor..."
        };

        for (int progress = 0; progress <= 100; progress++)
        {
            LoadingProgressBar.Value = progress;
            LoadingPercentText.Text = $"{progress} %";

            if (progress < 35)
            {
                LoadingStatusText.Text = loadingSteps[0];
            }
            else if (progress < 70)
            {
                LoadingStatusText.Text = loadingSteps[1];
            }
            else
            {
                LoadingStatusText.Text = loadingSteps[2];
            }

            await Task.Delay(28);
        }

        LoadingStatusText.Text = "Ladevorgang abgeschlossen.";
        _players = _playerProfileStore.LoadPlayers();
        await Task.Delay(250);

        LoadingPanel.Visibility = Visibility.Collapsed;

        if (_players.Count > 0)
        {
            ShowPlayerSelection();
        }
        else
        {
            ShowPlayerForm();
        }
    }

    private void LoadPlayerButton_Click(object sender, RoutedEventArgs e)
    {
        if (PlayersListBox.SelectedItem is not PlayerProfile selectedPlayer)
        {
            SelectionMessageText.Text = "Bitte wähle zuerst einen gespeicherten Spieler aus.";
            SelectionMessageText.Visibility = Visibility.Visible;
            return;
        }

        selectedPlayer.LastPlayedAt = DateTime.Now;
        _playerProfileStore.SavePlayer(selectedPlayer);
        OpenMainMenu(selectedPlayer);
    }

    private void CreatePlayerButton_Click(object sender, RoutedEventArgs e)
    {
        ShowPlayerForm();
    }

    private void BackToSelectionButton_Click(object sender, RoutedEventArgs e)
    {
        ShowPlayerSelection();
    }

    private void ConfirmButton_Click(object sender, RoutedEventArgs e)
    {
        string playerName = PlayerNameTextBox.Text.Trim();
        string ageText = PlayerAgeTextBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(playerName))
        {
            ShowValidationMessage("Bitte gib einen Spielernamen ein.");
            PlayerNameTextBox.Focus();
            return;
        }

        if (!int.TryParse(ageText, out int age) || age < 5 || age > 12)
        {
            ShowValidationMessage("Bitte gib ein gültiges Alter zwischen 5 und 12 ein.");
            PlayerAgeTextBox.Focus();
            PlayerAgeTextBox.SelectAll();
            return;
        }

        ValidationMessageText.Visibility = Visibility.Collapsed;

        var playerProfile = new PlayerProfile
        {
            Name = playerName,
            Age = age,
            AvatarPath = Path.Combine(AppContext.BaseDirectory, "Bilder", "Lumos.png"),
            Trophies = 0,
            Stars = 0,
            LastPlayedAt = DateTime.Now
        };

        _playerProfileStore.SavePlayer(playerProfile);
        OpenMainMenu(playerProfile);
    }

    private void ShowPlayerSelection()
    {
        SelectionMessageText.Visibility = Visibility.Collapsed;
        ValidationMessageText.Visibility = Visibility.Collapsed;
        PlayerFormPanel.Visibility = Visibility.Collapsed;
        PlayerSelectionPanel.Visibility = Visibility.Visible;
        PlayersListBox.ItemsSource = _players;
        PlayersListBox.SelectedIndex = 0;
        BackToSelectionButton.Visibility = Visibility.Visible;
    }

    private void ShowPlayerForm()
    {
        SelectionMessageText.Visibility = Visibility.Collapsed;
        PlayerSelectionPanel.Visibility = Visibility.Collapsed;
        PlayerFormPanel.Visibility = Visibility.Visible;
        BackToSelectionButton.Visibility = _players.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        PlayerNameTextBox.Text = string.Empty;
        PlayerAgeTextBox.Text = string.Empty;
        ValidationMessageText.Visibility = Visibility.Collapsed;
        PlayerNameTextBox.Focus();
    }

    private void OpenMainMenu(PlayerProfile playerProfile)
    {
        var mainMenuWindow = new MainMenuWindow(playerProfile);
        Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        Application.Current.MainWindow = mainMenuWindow;
        mainMenuWindow.Show();
        Close();
    }

    private void ShowValidationMessage(string message)
    {
        ValidationMessageText.Text = message;
        ValidationMessageText.Visibility = Visibility.Visible;
    }

    private void PlayerAgeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !IsTextNumeric(e.Text);
    }

    private void OnPlayerAgeTextBoxPaste(object sender, DataObjectPastingEventArgs e)
    {
        if (!e.DataObject.GetDataPresent(typeof(string)))
        {
            e.CancelCommand();
            return;
        }

        string pastedText = (string)e.DataObject.GetData(typeof(string))!;
        if (!IsTextNumeric(pastedText))
        {
            e.CancelCommand();
        }
    }

    private static bool IsTextNumeric(string text)
    {
        foreach (char character in text)
        {
            if (!char.IsDigit(character))
            {
                return false;
            }
        }

        return true;
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

    private void ApplyFullscreenMode()
    {
        WindowStyle = WindowStyle.None;
        WindowState = WindowState.Maximized;
    }
}
