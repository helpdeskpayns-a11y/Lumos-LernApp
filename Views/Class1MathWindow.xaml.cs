using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SpieleLernApp.Models;
using SpieleLernApp.Services;

namespace SpieleLernApp.Views;

public partial class Class1MathWindow : Window
{
    private readonly PlayerProfile _playerProfile;
    private readonly PlayerProfileStore _playerProfileStore = new();
    private readonly LearningTaskGeneratorFactory _taskGeneratorFactory = new();
    private readonly List<Button> _answerButtons;
    private List<LearningTask> _tasks;
    private int _currentTaskIndex;
    private int _correctAnswers;

    public Class1MathWindow(PlayerProfile playerProfile)
    {
        _playerProfile = playerProfile;
        _tasks = [];

        InitializeComponent();

        _answerButtons =
        [
            AnswerButton0,
            AnswerButton1,
            AnswerButton2,
            AnswerButton3
        ];

        ApplyBackgroundImage("Klasse 1.png");
        ClassImage.Source = CreateImage("Klasse 1.png");
        StartNewRound();
        UpdateHeader();
    }

    private void AnswerButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button || button.Tag is not string tagText || !int.TryParse(tagText, out int selectedIndex))
        {
            return;
        }

        LearningTask task = _tasks[_currentTaskIndex];
        bool isCorrect = selectedIndex == task.CorrectAnswerIndex;

        foreach (Button answerButton in _answerButtons)
        {
            answerButton.IsEnabled = false;
            answerButton.Background = new SolidColorBrush(Color.FromRgb(232, 255, 247));
        }

        if (isCorrect)
        {
            _correctAnswers++;
            button.Background = new SolidColorBrush(Color.FromRgb(176, 233, 151));
            FeedbackTextBlock.Text = $"{task.SuccessText} Du bekommst einen Stern.";
        }
        else
        {
            button.Background = new SolidColorBrush(Color.FromRgb(246, 180, 180));
            _answerButtons[task.CorrectAnswerIndex].Background = new SolidColorBrush(Color.FromRgb(176, 233, 151));
            FeedbackTextBlock.Text = $"Fast. Die richtige Antwort ist {_answerButtons[task.CorrectAnswerIndex].Content}. {task.SuccessText}";
        }

        SessionStarsTextBlock.Text = $"Sterne in dieser Runde: {_correctAnswers}";
        NextButton.Visibility = Visibility.Visible;
        NextButton.Content = _currentTaskIndex == _tasks.Count - 1 ? "Abschließen" : "Weiter";
    }

    private void NextButton_Click(object sender, RoutedEventArgs e)
    {
        _currentTaskIndex++;

        if (_currentTaskIndex >= _tasks.Count)
        {
            FinishSession();
            return;
        }

        ShowTask();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void NewRoundButton_Click(object sender, RoutedEventArgs e)
    {
        StartNewRound();
    }

    private void ShowTask()
    {
        LearningTask task = _tasks[_currentTaskIndex];

        TaskTitleTextBlock.Text = task.Title;
        TaskPromptTextBlock.Text = task.Prompt;
        FeedbackTextBlock.Text = "Wähle die richtige Antwort aus.";
        ProgressTextBlock.Text = $"{_currentTaskIndex + 1} / {_tasks.Count}";
        SessionStarsTextBlock.Text = $"Sterne in dieser Runde: {_correctAnswers}";
        NextButton.Visibility = Visibility.Collapsed;
        NewRoundButton.Visibility = Visibility.Collapsed;

        foreach (Button answerButton in _answerButtons)
        {
            answerButton.Visibility = Visibility.Visible;
            answerButton.IsEnabled = true;
            answerButton.Background = new SolidColorBrush(Color.FromRgb(232, 255, 247));
        }

        for (int index = 0; index < _answerButtons.Count; index++)
        {
            _answerButtons[index].Content = task.Answers[index];
        }
    }

    private void FinishSession()
    {
        _playerProfile.Stars += _correctAnswers;
        _playerProfile.LastPlayedAt = DateTime.Now;
        _playerProfileStore.SavePlayer(_playerProfile);
        UpdateHeader();

        TaskTitleTextBlock.Text = "Geschafft";
        TaskPromptTextBlock.Text = "Du hast den ersten Mathebereich für Klasse 1 beendet.";
        FeedbackTextBlock.Text = $"Du hast {_correctAnswers} von {_tasks.Count} Aufgaben richtig gelöst und {_correctAnswers} Sterne gesammelt.";
        ProgressTextBlock.Text = $"{_tasks.Count} / {_tasks.Count}";

        foreach (Button answerButton in _answerButtons)
        {
            answerButton.Visibility = Visibility.Collapsed;
        }

        NextButton.Visibility = Visibility.Collapsed;
        NewRoundButton.Visibility = Visibility.Visible;
    }

    private void StartNewRound()
    {
        _tasks = _taskGeneratorFactory.GenerateTasks(new TaskGenerationRequest
        {
            Subject = LearningSubject.Math,
            ClassLevel = 1,
            TaskCount = 5
        }).ToList();

        _currentTaskIndex = 0;
        _correctAnswers = 0;
        ShowTask();
    }

    private void UpdateHeader()
    {
        TotalStarsTextBlock.Text = _playerProfile.Stars.ToString();
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
        string imagePath = GetImagePath(imageName);
        return new BitmapImage(new Uri(imagePath, UriKind.Absolute));
    }

    private static string GetImagePath(string imageName)
    {
        return Path.Combine(AppContext.BaseDirectory, "Bilder", imageName);
    }
}
