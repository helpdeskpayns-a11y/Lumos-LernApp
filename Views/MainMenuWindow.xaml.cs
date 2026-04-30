using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Ellipse = System.Windows.Shapes.Ellipse;
using Microsoft.Win32;
using SpieleLernApp.Models;
using SpieleLernApp.Services;

namespace SpieleLernApp.Views;

public partial class MainMenuWindow : Window
{
    private const int PortalQuestionsPerRound = 10;
    private const int WorldQuestionsPerLevel = 4;
    private const int FullTaskPoolRequestCount = 1000;
    private const int PortalRewardPerSubject = 10;
    private const int WorldRewardPerSubject = 1;
    private const int DefaultPortalUnlockRequirement = 40;
    private const int Class2PortalUnlockRequirement = 50;
    private const int DefaultWorldUnlockRequirement = 4;
    private const int Class2WorldUnlockRequirement = 5;
    private const double WorldWaypointMarkerSize = 56.61;
    private static readonly LearningSubject[] Class1PortalSubjects =
    [
        LearningSubject.Math,
        LearningSubject.German,
        LearningSubject.Sachunterricht,
        LearningSubject.Music
    ];
    private static readonly LearningSubject[] Class2PortalSubjects =
    [
        LearningSubject.German,
        LearningSubject.Math,
        LearningSubject.Sachunterricht,
        LearningSubject.Logic,
        LearningSubject.Music
    ];
    private static readonly LearningSubject[] Class3PortalSubjects =
    [
        LearningSubject.German,
        LearningSubject.Math,
        LearningSubject.Sachunterricht,
        LearningSubject.Logic,
        LearningSubject.Music
    ];
    private static readonly LearningSubject[] Class4PortalSubjects =
    [
        LearningSubject.German,
        LearningSubject.Math,
        LearningSubject.Sachunterricht,
        LearningSubject.Music,
        LearningSubject.Art,
        LearningSubject.English
    ];
    private static readonly LearningSubject[] Class3BonusMixSubjects =
    [
        LearningSubject.German,
        LearningSubject.Math,
        LearningSubject.Sachunterricht,
        LearningSubject.Logic,
        LearningSubject.Music
    ];
    private static readonly LearningSubject[] Class4BonusMixSubjects =
    [
        LearningSubject.German,
        LearningSubject.Math,
        LearningSubject.Sachunterricht,
        LearningSubject.Music,
        LearningSubject.Art,
        LearningSubject.English
    ];

    private readonly PlayerProfile _playerProfile;
    private readonly PlayerProfileStore _playerProfileStore = new();
    private readonly LearningTaskGeneratorFactory _taskGeneratorFactory = new();
    private readonly TaskSpeechService _taskSpeechService = new();
    private readonly List<DiaryEntry> _staticDiaryEntries;
    private readonly List<DailyGoal> _dailyGoals;
    private readonly List<Button> _taskAnswerButtons;
    private List<LearningTask> _currentTasks;
    private int _currentTaskIndex;
    private int _currentCorrectAnswers;
    private int _selectedClassLevel = 1;
    private int _activeClassLevel = 1;
    private int _pendingRewardGain;
    private int _pendingRewardFinalValue;
    private LearningSubject? _activeSubject;
    private LearningSubject? _selectedWorldIslandSubject;
    private int _activeWorldLevel = 1;
    private int? _pendingWorldMoveFromStep;
    private int? _pendingWorldMoveToStep;
    private int? _pendingWorldMoveClassLevel;
    private LearningSubject? _pendingWorldMoveSubject;
    private bool _animateWorldMoveOnNextMapOpen;
    private bool _shouldReturnToWorldMapAfterCollect;
    private bool _returnToWorldMapInsteadOfNewRound;
    private bool _worldStarCollectedThisRound;
    private RewardTrack _selectedRewardTrack = RewardTrack.PortalTrophies;
    private RewardTrack _activeRewardTrack = RewardTrack.PortalTrophies;
    private bool _hasPendingReward;

    public MainMenuWindow(PlayerProfile playerProfile)
    {
        _playerProfile = playerProfile;
        _playerProfile.AvatarPath ??= GetImagePath("Lumos.png");
        _playerProfile.RewardProgress ??= [];
        _playerProfile.DiaryEntries ??= [];
        _playerProfile.WorldMapProgress ??= [];
        NormalizeRewardTotals();

        _staticDiaryEntries =
        [
            new DiaryEntry
            {
                Title = "Startbereich freigeschaltet",
                Description = "Der Ladebildschirm, die Spielerabfrage und das Startmenü wurden freigeschaltet."
            },
            new DiaryEntry
            {
                Title = "Lernportal vorbereitet",
                Description = "Die Klassenauswahl ist erreichbar und kann als nächster Lernschritt geöffnet werden."
            },
            new DiaryEntry
            {
                Title = "Fächerbereich erweitert",
                Description = "Klasse 1 enthält jetzt Lernwege für Mathematik, Deutsch, Sachkunde und Musik."
            }
        ];

        _dailyGoals =
        [
            new DailyGoal
            {
                Title = "Tagesziel: Runde schaffen",
                Description = "Spiele heute eine Runde mit 10 Fragen und sammle danach deine Belohnung ein."
            },
            new DailyGoal
            {
                Title = "Tagesziel: Lernportal voranbringen",
                Description = "Sammle Pokale im Lernportal, damit neue Welten freigeschaltet werden."
            },
            new DailyGoal
            {
                Title = "Tagesziel: Welt öffnen",
                Description = "Sammle Sterne in den Welten, um die nächste Klasse im Lernportal freizuschalten."
            }
        ];

        _currentTasks = [];

        InitializeComponent();

        _taskAnswerButtons =
        [
            MathAnswerButton0,
            MathAnswerButton1,
            MathAnswerButton2,
            MathAnswerButton3
        ];

        ApplyFullscreenMode(true);
        ApplySceneImage("neutraler Ort.png");
        InitializeMenu();
        Closed += MainMenuWindow_Closed;
    }

    private void InitializeMenu()
    {
        WelcomeTextBlock.Text = $"Willkommen {_playerProfile.Name}! Das Bild ist jetzt interaktiv und führt dich durch dein Startmenü.";
        PlayerNameInfoText.Text = _playerProfile.Name;
        PlayerAgeInfoText.Text = _playerProfile.Age.ToString();
        LoadAvatarImage(_playerProfile.AvatarPath);
        RefreshDiaryItems();
        ShowDailyGoal();
        LoadClassPreviewImages();
        LoadSubjectPreviewImages();
        ReadTaskButtonImage.Source = CreateBitmap("Lautsprecher.png");
        MathClassImage.Source = CreateBitmap("Klasse 1.png");
        UpdateSettingsStatus();
        RefreshRewardDisplay();
        UpdateClassCardStates();
        HideAllPanels();
    }

    private void ProfileHotspot_Click(object sender, RoutedEventArgs e)
    {
        ShowProfilePanel();
    }

    private void RewardsHotspot_Click(object sender, RoutedEventArgs e)
    {
        ShowDetailPanel("Meine Belohnungen", "Im Lernportal sammelst du Pokale. In den Welten sammelst du Sterne.");
        RefreshRewardDisplay();
        RewardsPanel.Visibility = Visibility.Visible;
    }

    private void DiaryHotspot_Click(object sender, RoutedEventArgs e)
    {
        RefreshDiaryItems();
        ShowDetailPanel("Tagebuch", "Hier siehst du, welche Aufgaben oder Bereiche freigeschaltet und abgeschlossen wurden.");
        DiaryPanel.Visibility = Visibility.Visible;
    }

    private void DailyGoalHotspot_Click(object sender, RoutedEventArgs e)
    {
        ShowDetailPanel("Tagesziel", "Jeden Tag wird eine neue Aufgabe als Ziel angezeigt.");
        ShowDailyGoal();
        DailyGoalPanel.Visibility = Visibility.Visible;
    }

    private void LearningPortalHotspot_Click(object sender, RoutedEventArgs e)
    {
        _selectedRewardTrack = RewardTrack.PortalTrophies;
        ShowClassSelectionPanel();
    }

    private void WorldsHotspot_Click(object sender, RoutedEventArgs e)
    {
        _selectedRewardTrack = RewardTrack.WorldStars;
        ShowClassSelectionPanel();
    }

    private void TreasureHotspot_Click(object sender, RoutedEventArgs e)
    {
        ShowDetailPanel("Die Truhe", "Hier kannst du Sound und Vollbild einstellen.");
        UpdateSettingsStatus();
        SettingsPanel.Visibility = Visibility.Visible;
    }

    private void SwitchPlayerButton_Click(object sender, RoutedEventArgs e)
    {
        SavePlayerProfile();

        var startupWindow = new StartupWindow(skipLoadingSequence: true);
        Application.Current.MainWindow = startupWindow;
        startupWindow.Show();
        Close();
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        SavePlayerProfile();
        Application.Current.Shutdown();
    }

    private void ChooseAvatarButton_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "Spielerbild auswählen",
            Filter = "Bilddateien|*.png;*.jpg;*.jpeg;*.bmp"
        };

        if (openFileDialog.ShowDialog(this) == true)
        {
            _playerProfile.AvatarPath = openFileDialog.FileName;
            LoadAvatarImage(_playerProfile.AvatarPath);
            SavePlayerProfile();
        }
    }

    private void ClassCardButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement element || element.Tag is not string className)
        {
            return;
        }

        int classLevel = ParseClassLevel(className);
        _selectedClassLevel = classLevel;

        if (_selectedRewardTrack == RewardTrack.PortalTrophies && !IsPortalClassUnlocked(classLevel))
        {
            int currentStars = GetClassRewardTotal(classLevel - 1, RewardTrack.WorldStars);
            ShowPlaceholderPanel(
                className,
                $"{className} ist noch gesperrt",
                $"Für {className} im Lernportal brauchst du zuerst {GetWorldUnlockRequirement(classLevel - 1)} Sterne in Welt Klasse {classLevel - 1}. Aktuell hast du dort {currentStars} von {GetWorldUnlockRequirement(classLevel - 1)} Sternen.",
                $"Klasse {Math.Max(1, classLevel - 1)}.png",
                backTarget: "Classes");
            return;
        }

        if (_selectedRewardTrack == RewardTrack.WorldStars && !IsWorldClassUnlocked(classLevel))
        {
            int currentTrophies = GetClassRewardTotal(classLevel, RewardTrack.PortalTrophies);
            ShowPlaceholderPanel(
                $"Welt {className}",
                $"Welt {className} ist noch gesperrt",
                $"Für Welt Klasse {classLevel} brauchst du zuerst {GetPortalUnlockRequirement(classLevel)} Pokale im Lernportal Klasse {classLevel}. Aktuell hast du dort {currentTrophies} von {GetPortalUnlockRequirement(classLevel)} Pokalen.",
                className == "Klasse 1" ? "Klasse 1.png" : $"{className}.png",
                backTarget: "Classes");
            return;
        }

        if (classLevel == 1)
        {
            ShowClass1SubjectPanel();
            return;
        }

        if (classLevel == 2)
        {
            ShowClass2SubjectPanel();
            return;
        }

        if (classLevel == 3)
        {
            ShowClass3SubjectPanel();
            return;
        }

        if (classLevel == 4)
        {
            ShowClass4SubjectPanel();
            return;
        }

        ShowPlaceholderPanel(
            className,
            $"{className} ist freigeschaltet",
            $"{className} ist bereits freigeschaltet. Die Inhalte für diese Klasse werden als Nächstes aufgebaut.",
            $"{className}.png",
            backTarget: "Classes");
    }

    private void Class1SubjectButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement element || element.Tag is not string subjectName)
        {
            return;
        }

        LearningSubject subject = GetLearningSubject(subjectName);

        if (_selectedRewardTrack == RewardTrack.WorldStars)
        {
            if (subject == LearningSubject.Bonus && !IsClass3BonusWorldUnlocked())
            {
                ShowPlaceholderPanel(
                    "Welt Klasse 3 - Bonus",
                    "Bonus-Insel ist noch gesperrt",
                    $"Die Bonus-Insel wird erst freigeschaltet, wenn alle fünf Fach-Inseln in Welt Klasse 3 abgeschlossen sind. Aktuell hast du {GetClassRewardTotal(3, RewardTrack.WorldStars)} von {Class3PortalSubjects.Length} Fach-Sternen gesammelt.",
                    "Klasse 3.png",
                    backTarget: "Subjects");
                return;
            }

            OpenWorldSubjectSafely(subject);
            return;
        }

        OpenPortalSubjectSafely(subject);
    }

    private void Class2SubjectButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement element || element.Tag is not string subjectName)
        {
            return;
        }

        LearningSubject subject = GetLearningSubject(subjectName);

        if (_selectedRewardTrack == RewardTrack.WorldStars)
        {
            OpenWorldSubjectSafely(subject);
            return;
        }

        OpenPortalSubjectSafely(subject);
    }

    private void Class3SubjectButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement element || element.Tag is not string subjectName)
        {
            return;
        }

        LearningSubject subject = GetLearningSubject(subjectName);

        if (_selectedRewardTrack == RewardTrack.WorldStars)
        {
            OpenWorldSubjectSafely(subject);
            return;
        }

        OpenPortalSubjectSafely(subject);
    }

    private void Class4SubjectButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement element || element.Tag is not string subjectName)
        {
            return;
        }

        LearningSubject subject = GetLearningSubject(subjectName);

        if (_selectedRewardTrack == RewardTrack.WorldStars)
        {
            if (subject == LearningSubject.Bonus && !IsClass4BonusWorldUnlocked())
            {
                ShowPlaceholderPanel(
                    "Welt Klasse 4 - Meisterprüfung",
                    "Meisterinsel ist noch gesperrt",
                    $"Die Meisterprüfung wird erst freigeschaltet, wenn alle sechs Fach-Inseln in Welt Klasse 4 abgeschlossen sind. Aktuell hast du {GetClassRewardTotal(4, RewardTrack.WorldStars)} von {Class4PortalSubjects.Length} Fach-Sternen gesammelt.",
                    "KLasse 4.png",
                    backTarget: "Subjects");
                return;
            }

            OpenWorldSubjectSafely(subject);
            return;
        }

        OpenPortalSubjectSafely(subject);
    }

    private void OpenPortalSubjectSafely(LearningSubject subject)
    {
        try
        {
            ShowSubjectPanel(subject);
        }
        catch (Exception exception)
        {
            HandleSubjectOpenFailure(subject, isWorldSubject: false, exception);
        }
    }

    private void OpenWorldSubjectSafely(LearningSubject subject)
    {
        try
        {
            ShowWorldIslandPanel(subject);
        }
        catch (Exception exception)
        {
            HandleSubjectOpenFailure(subject, isWorldSubject: true, exception);
        }
    }

    private void HandleSubjectOpenFailure(LearningSubject subject, bool isWorldSubject, Exception exception)
    {
        string subjectName = isWorldSubject
            ? GetWorldSubjectDisplayName(_selectedClassLevel, subject)
            : GetSubjectDisplayName(subject);
        string previewImage = _selectedClassLevel == 4 ? "KLasse 4.png" : $"Klasse {_selectedClassLevel}.png";
        string titlePrefix = isWorldSubject ? "Welt Klasse" : "Klasse";
        string description = isWorldSubject
            ? $"Beim Laden der Insel {subjectName} ist ein Fehler aufgetreten. Bitte versuche es erneut. Details stehen in app-error.log."
            : $"Beim Laden des Fachs {subjectName} ist ein Fehler aufgetreten. Bitte versuche es erneut. Details stehen in app-error.log.";

        WriteMenuErrorLog(
            isWorldSubject ? "OpenWorldSubject" : "OpenPortalSubject",
            exception,
            $"Klasse {_selectedClassLevel} | Fach {subjectName} | Track {_selectedRewardTrack}");

        ShowPlaceholderPanel(
            $"{titlePrefix} {_selectedClassLevel} - {subjectName}",
            "Fach konnte nicht geöffnet werden",
            description,
            FindPreviewImage(previewImage) ?? previewImage,
            backTarget: "Subjects");
    }

    private void BackToClassSelectionButton_Click(object sender, RoutedEventArgs e)
    {
        ClearPendingWorldMove();
        ShowClassSelectionPanel();
    }

    private void ContentPlaceholderBackButton_Click(object sender, RoutedEventArgs e)
    {
        if (ContentPlaceholderBackButton.Tag as string == "Subjects")
        {
            ShowSelectedClassSubjectPanel();
            return;
        }

        ShowClassSelectionPanel();
    }

    private void BackToSubjectsFromMathButton_Click(object sender, RoutedEventArgs e)
    {
        SavePlayerProfile();
        ClearPendingWorldMove();
        ShowSelectedClassSubjectPanel();
    }

    private void BackToSubjectsFromIslandButton_Click(object sender, RoutedEventArgs e)
    {
        ClearPendingWorldMove();
        ShowSelectedClassSubjectPanel();
    }

    private void StartIslandRoundButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedWorldIslandSubject is null)
        {
            return;
        }

        WorldMapProgress progress = GetWorldMapProgress(_selectedClassLevel, _selectedWorldIslandSubject.Value);
        if (progress.StarCollected)
        {
            if (HasPendingWorldStarToCollect(_selectedClassLevel, _selectedWorldIslandSubject.Value))
            {
                _activeSubject = _selectedWorldIslandSubject.Value;
                _activeRewardTrack = RewardTrack.WorldStars;
                _activeClassLevel = _selectedClassLevel;
                CollectPendingWorldStar(_selectedWorldIslandSubject.Value);
                return;
            }

            WorldIslandProgressTextBlock.Text = "Der Stern dieser Insel wurde bereits eingesammelt. Du kannst ein anderes Fach wählen.";
            return;
        }

        _activeWorldLevel = GetCurrentWorldStep(progress, _selectedClassLevel, _selectedWorldIslandSubject.Value);
        ClearPendingWorldMove();
        ShowSubjectPanel(_selectedWorldIslandSubject.Value);
    }

    private void MathAnswerButton_Click(object sender, RoutedEventArgs e)
    {
        _taskSpeechService.Stop();

        if (_activeSubject is null)
        {
            return;
        }

        if (sender is not Button button || button.Tag is not string tagText || !int.TryParse(tagText, out int selectedIndex))
        {
            return;
        }

        LearningTask task = _currentTasks[_currentTaskIndex];
        bool isCorrect = selectedIndex == task.CorrectAnswerIndex;

        foreach (Button answerButton in _taskAnswerButtons)
        {
            answerButton.IsEnabled = false;
            answerButton.Background = CreateBrush(232, 255, 247);
        }

        if (isCorrect)
        {
            _currentCorrectAnswers++;
            button.Background = CreateBrush(176, 233, 151);
            FeedbackTextBlock.Text = $"{task.SuccessText} Jede richtige Aufgabe zählt für den Fachfortschritt.";
        }
        else
        {
            button.Background = CreateBrush(246, 180, 180);
            _taskAnswerButtons[task.CorrectAnswerIndex].Background = CreateBrush(176, 233, 151);
            FeedbackTextBlock.Text = $"Fast. Die richtige Antwort ist {_taskAnswerButtons[task.CorrectAnswerIndex].Content}. Du kannst in dieser Runde trotzdem weitere Punkte sammeln.";
        }

        SessionRewardTextBlock.Text = _activeRewardTrack == RewardTrack.WorldStars
            ? $"Level-Fortschritt: {_currentCorrectAnswers} / {GetQuestionCountForTrack(_activeRewardTrack)} richtig. Sterne gibt es erst am Insel-Stern."
            : $"{GetRewardPluralLabel(_activeRewardTrack)} in dieser Runde: {GetCurrentRoundRewardPreview()}";
        MathNextButton.Visibility = Visibility.Visible;
        MathNextButton.Content = _currentTaskIndex == _currentTasks.Count - 1 ? "Abschließen" : "Weiter";
    }

    private void MathNextButton_Click(object sender, RoutedEventArgs e)
    {
        _currentTaskIndex++;

        if (_currentTaskIndex >= _currentTasks.Count)
        {
            FinishSubjectSession();
            return;
        }

        ShowCurrentTask();
    }

    private void MathNewRoundButton_Click(object sender, RoutedEventArgs e)
    {
        if (_returnToWorldMapInsteadOfNewRound && _activeSubject is not null)
        {
            _returnToWorldMapInsteadOfNewRound = false;
            bool animateMoveOnOpen = _animateWorldMoveOnNextMapOpen;
            _animateWorldMoveOnNextMapOpen = false;
            ShowWorldIslandPanel(_activeSubject.Value, animateMoveOnOpen);
            return;
        }

        if (_activeSubject is null)
        {
            return;
        }

        StartNewRound(_activeSubject.Value, _activeRewardTrack, _activeClassLevel);
    }

    private void CollectRewardButton_Click(object sender, RoutedEventArgs e)
    {
        if (!_hasPendingReward || _activeSubject is null || _pendingRewardGain <= 0)
        {
            return;
        }

        bool unlockedWorldBefore = IsWorldClassUnlocked(_activeClassLevel);
        bool unlockedNextPortalBefore = IsPortalClassUnlocked(_activeClassLevel + 1);

        SetSubjectRewardValue(_activeClassLevel, _activeSubject.Value, _activeRewardTrack, _pendingRewardFinalValue);
        SavePlayerProfile();
        RefreshRewardDisplay();
        UpdateClassCardStates();

        string rewardLabel = GetRewardPluralLabel(_activeRewardTrack).ToLowerInvariant();
        string unlockMessage = BuildUnlockMessage(unlockedWorldBefore, unlockedNextPortalBefore);

        if (!string.IsNullOrWhiteSpace(unlockMessage))
        {
            AddDiaryEntryIfMissing(
                $"Erfolg: {unlockMessage.Trim()}",
                $"Durch eingesammelte {rewardLabel} wurde {unlockMessage.Trim().TrimEnd('.')}.");
        }

        if (_activeRewardTrack == RewardTrack.WorldStars)
        {
            AddWorldStarDiaryEntry(_activeSubject.Value);
        }

        TaskTitleTextBlock.Text = "Belohnung eingesammelt";
        FeedbackTextBlock.Text = $"Du hast {_pendingRewardGain} {rewardLabel} eingesammelt.{unlockMessage}";
        SessionRewardTextBlock.Text = $"{GetRewardPluralLabel(_activeRewardTrack)} in dieser Runde: {_pendingRewardGain}";
        CollectRewardButton.Visibility = Visibility.Collapsed;
        MathNewRoundButton.Visibility = Visibility.Visible;
        _hasPendingReward = false;
        _pendingRewardGain = 0;

        if (_shouldReturnToWorldMapAfterCollect && _activeSubject is not null)
        {
            _shouldReturnToWorldMapAfterCollect = false;
            bool animateMoveOnOpen = _animateWorldMoveOnNextMapOpen;
            _animateWorldMoveOnNextMapOpen = false;
            ShowWorldIslandPanel(_activeSubject.Value, animateMoveOnOpen);
        }
    }

    private void SoundEnabledCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        if (SoundEnabledCheckBox?.IsChecked != true)
        {
            _taskSpeechService.Stop();
        }

        UpdateSettingsStatus();
    }

    private void ReadTaskButton_Click(object sender, RoutedEventArgs e)
    {
        if (SoundEnabledCheckBox?.IsChecked != true || _currentTasks.Count == 0 || _currentTaskIndex >= _currentTasks.Count)
        {
            return;
        }

        _taskSpeechService.SpeakTask(_currentTasks[_currentTaskIndex], _currentTaskIndex + 1, _currentTasks.Count);
    }

    private void FullscreenCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        if (FullscreenCheckBox is null)
        {
            return;
        }

        ApplyFullscreenMode(FullscreenCheckBox.IsChecked == true);
        UpdateSettingsStatus();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Escape || FullscreenCheckBox is null || FullscreenCheckBox.IsChecked != true)
        {
            return;
        }

        FullscreenCheckBox.IsChecked = false;
        e.Handled = true;
    }

    private void ShowProfilePanel()
    {
        ShowDetailPanel("Mein Profil", "Hier sind Spielername, Alter, Pokale, Sterne und das Bild des Spielers hinterlegt.");
        ProfilePanel.Visibility = Visibility.Visible;
    }

    private void ShowClassSelectionPanel()
    {
        ClearPendingWorldMove();
        UpdateClassCardStates();

        if (_selectedRewardTrack == RewardTrack.WorldStars)
        {
            ShowDetailPanel(
                "Deine Welten",
                "Hier kannst du direkt eine Klasse auswählen. In den Welten sammelst du Sterne und schaltest die nächste Portal-Klasse frei.");
            WorldsPanel.Visibility = Visibility.Visible;
            return;
        }

        ShowDetailPanel(
            "Lernportal",
            "Hier kannst du direkt eine Klasse auswählen. Im Lernportal sammelst du Pokale und schaltest die passende Welt frei.");
        PortalPanel.Visibility = Visibility.Visible;
    }

    private void ShowClass1SubjectPanel()
    {
        string rewardText = _selectedRewardTrack == RewardTrack.PortalTrophies
            ? "Wähle ein Fach für Klasse 1 aus. Nach jeder Runde kannst du die verdienten Pokale einsammeln."
            : "Wähle ein Fach für Klasse 1 aus. Nach jeder Runde kannst du die verdienten Sterne einsammeln.";

        ShowDetailPanel("Klasse 1", rewardText);
        Class1SubjectsPanel.Visibility = Visibility.Visible;
    }

    private void ShowClass2SubjectPanel()
    {
        string rewardText = _selectedRewardTrack == RewardTrack.PortalTrophies
            ? "Wähle ein Fach für Klasse 2 aus. Die fünf Fächer stammen direkt aus dem Bild klasse 2.png und sind im Lernportal jetzt testbar."
            : "Wähle ein Fach für Welt Klasse 2 aus. Die fünf Fächer stammen direkt aus dem Bild klasse 2.png.";

        ShowDetailPanel("Klasse 2", rewardText);
        Class2SubjectsPanel.Visibility = Visibility.Visible;
    }

    private void ShowClass3SubjectPanel()
    {
        string rewardText = _selectedRewardTrack == RewardTrack.PortalTrophies
            ? "Wähle ein Fach für Klasse 3 aus. Die fünf Fächer stammen direkt aus dem Bild Klasse 3.png und sind im Lernportal jetzt testbar."
            : "Wähle ein Fach für Welt Klasse 3 aus. Zusätzlich gibt es eine Bonus-Insel mit 3 Leveln und einem Extra-Stern.";

        ShowDetailPanel("Klasse 3", rewardText);
        bool showBonus = _selectedRewardTrack == RewardTrack.WorldStars;
        Class3BonusButton.Visibility = showBonus ? Visibility.Visible : Visibility.Collapsed;
        Class3BonusButton.IsEnabled = !showBonus || IsClass3BonusWorldUnlocked();
        Class3BonusButton.Opacity = !showBonus || IsClass3BonusWorldUnlocked() ? 1.0 : 0.6;
        Class3SubjectsInfoTextBlock.Text = showBonus
            ? "Die fünf Fach-Inseln stammen aus dem Bild Klasse 3.png. Die Bonus-Insel wird erst nach allen fünf Fach-Sternen freigeschaltet."
            : "Die fünf Fächer wurden direkt aus dem Bild Klasse 3.png übernommen.";
        Class3SubjectsPanel.Visibility = Visibility.Visible;
    }

    private void ShowClass4SubjectPanel()
    {
        string rewardText = _selectedRewardTrack == RewardTrack.PortalTrophies
            ? "Wähle ein Fach für Klasse 4 aus. Die sechs Fächer sind jetzt auch im Lernportal spielbar."
            : "Wähle ein Fach für Welt Klasse 4 aus. Zusätzlich gibt es eine Meisterinsel mit 3 Bonus-Leveln und einem Extra-Stern.";

        ShowDetailPanel("Klasse 4", rewardText);
        bool showBonus = _selectedRewardTrack == RewardTrack.WorldStars;
        Class4BonusButton.Visibility = showBonus ? Visibility.Visible : Visibility.Collapsed;
        Class4BonusButton.IsEnabled = !showBonus || IsClass4BonusWorldUnlocked();
        Class4BonusButton.Opacity = !showBonus || IsClass4BonusWorldUnlocked() ? 1.0 : 0.6;
        Class4SubjectsInfoTextBlock.Text = _selectedRewardTrack == RewardTrack.WorldStars
            ? "Die Klasse-4-Welt nutzt sechs Fachinseln. Die Meisterprüfung wird erst nach allen sechs Fach-Sternen freigeschaltet."
            : "Die Klasse-4-Fächer folgen jetzt den sichtbaren Fachinseln der Meisterwelt.";
        Class4SubjectsPanel.Visibility = Visibility.Visible;
    }

    private void ShowSelectedClassSubjectPanel()
    {
        ClearPendingWorldMove();

        if (_selectedClassLevel == 2)
        {
            ShowClass2SubjectPanel();
            return;
        }

        if (_selectedClassLevel == 3)
        {
            ShowClass3SubjectPanel();
            return;
        }

        if (_selectedClassLevel == 4)
        {
            ShowClass4SubjectPanel();
            return;
        }

        ShowClass1SubjectPanel();
    }

    private void ShowSubjectPanel(LearningSubject subject)
    {
        _activeSubject = subject;
        _activeRewardTrack = _selectedRewardTrack;
        _activeClassLevel = _selectedClassLevel;

        ShowDetailPanel(
            $"Klasse {_activeClassLevel} - {GetSubjectDisplayName(subject)}",
            GetSubjectIntroText(subject, _activeRewardTrack, _activeClassLevel));
        if (_activeRewardTrack == RewardTrack.WorldStars)
        {
            ApplyFullscreenDetailPanelLayout();
        }

        MathPanel.Visibility = Visibility.Visible;

        string previewImage = FindPreviewImage(GetSubjectPreviewName(_activeClassLevel, subject)) ?? "Klasse 1.png";
        MathClassImage.Source = CreateBitmap(previewImage);
        StartNewRound(subject, _activeRewardTrack, _activeClassLevel);
    }

    private void ShowWorldIslandPanel(LearningSubject subject, bool animateMoveOnOpen = false)
    {
        _selectedWorldIslandSubject = subject;
        _activeSubject = subject;
        _activeRewardTrack = RewardTrack.WorldStars;
        _activeClassLevel = _selectedClassLevel;

        string subjectName = GetWorldSubjectDisplayName(_activeClassLevel, subject);
        WorldMapProgress progress = GetWorldMapProgress(_activeClassLevel, subject);
        _activeWorldLevel = GetCurrentWorldStep(progress, _activeClassLevel, subject);

        ShowDetailPanel(
            $"Welt Klasse {_activeClassLevel} - {subjectName}",
            $"Lumos ist auf der {subjectName}-Insel angekommen. Von hier aus startet Level {_activeWorldLevel} mit {GetQuestionCountForTrack(RewardTrack.WorldStars)} Aufgaben.");
        ApplyFullscreenDetailPanelLayout();

        WorldIslandTitleTextBlock.Text = subject == LearningSubject.Bonus && _activeClassLevel == 4
            ? "Meisterprüfung"
            : $"{subjectName}-Insel";
        WorldIslandDescriptionTextBlock.Text = GetWorldIslandDescription(_activeClassLevel, subject);
        WorldIslandMapImage.Source = CreateBitmap(GetWorldIslandImage(_activeClassLevel));
        LumosIslandImage.Source = CreateBitmap("Lumos.png");
        UpdateWorldIslandProgressDisplay(subject, progress);
        PositionWorldLevelMarkers(subject, progress);
        PositionLumosOnIsland(subject, GetCurrentWorldStep(progress, _activeClassLevel, subject), animatePendingMove: animateMoveOnOpen);
        WorldIslandPanel.Visibility = Visibility.Visible;
    }

    private void ShowPlaceholderPanel(
        string title,
        string heading,
        string description,
        string previewImageName,
        string backTarget)
    {
        ShowDetailPanel(title, "Dieser Bereich bleibt innerhalb desselben Fensters.");
        ContentPlaceholderHeadingTextBlock.Text = heading;
        ContentPlaceholderDescriptionTextBlock.Text = description;
        ContentPlaceholderBackButton.Tag = backTarget;
        ContentPlaceholderBackButton.Content = backTarget == "Subjects"
            ? "Zurück zur Fächerauswahl"
            : "Zurück zur Klassenwahl";
        ContentPreviewImage.Source = CreateBitmap(FindPreviewImage(previewImageName) ?? "Klasse 1.png");
        ContentPlaceholderPanel.Visibility = Visibility.Visible;
    }

    private void ShowGrandMasteryPanel()
    {
        ShowDetailPanel("Meisterwelt", "Dieser Erfolg fällt das ganze Fenster.");
        ApplyFullscreenDetailPanelLayout();
        ContentPlaceholderHeadingTextBlock.Text = "Du hast die Grundschule gemeistert.";
        ContentPlaceholderDescriptionTextBlock.Text = "Die Meisterprüfung ist geschafft. Lumos hat den Extra-Stern eingesammelt und damit die Grundschulwelt vollständig abgeschlossen.";
        ContentPlaceholderBackButton.Tag = "Classes";
        ContentPlaceholderBackButton.Content = "Zurück zur Klassenwahl";
        ContentPreviewImage.Source = CreateBitmap(FindPreviewImage("KLasse 4") ?? "KLasse 4.png");
        ContentPlaceholderPanel.Visibility = Visibility.Visible;
    }

    private void HideAllPanels()
    {
        _taskSpeechService.Stop();
        DetailPanelBorder.Visibility = Visibility.Collapsed;
        ProfilePanel.Visibility = Visibility.Collapsed;
        RewardsPanel.Visibility = Visibility.Collapsed;
        DiaryPanel.Visibility = Visibility.Collapsed;
        DailyGoalPanel.Visibility = Visibility.Collapsed;
        PortalPanel.Visibility = Visibility.Collapsed;
        WorldsPanel.Visibility = Visibility.Collapsed;
        SettingsPanel.Visibility = Visibility.Collapsed;
        Class1SubjectsPanel.Visibility = Visibility.Collapsed;
        Class2SubjectsPanel.Visibility = Visibility.Collapsed;
        Class3SubjectsPanel.Visibility = Visibility.Collapsed;
        Class4SubjectsPanel.Visibility = Visibility.Collapsed;
        ContentPlaceholderPanel.Visibility = Visibility.Collapsed;
        WorldIslandPanel.Visibility = Visibility.Collapsed;
        MathPanel.Visibility = Visibility.Collapsed;
    }

    private void ShowDetailPanel(string title, string subtitle)
    {
        HideAllPanels();
        ApplyStandardDetailPanelLayout();
        DetailPanelBorder.Visibility = Visibility.Visible;
        PanelTitleTextBlock.Text = title;
        PanelSubtitleTextBlock.Text = subtitle;
    }

    private void ApplyStandardDetailPanelLayout()
    {
        DetailPanelBorder.HorizontalAlignment = HorizontalAlignment.Center;
        DetailPanelBorder.VerticalAlignment = VerticalAlignment.Bottom;
        DetailPanelBorder.Margin = new Thickness(0, 0, 0, 26);
        DetailPanelBorder.MaxWidth = 1120;
        DetailPanelBorder.MaxHeight = 620;
    }

    private void ApplyFullscreenDetailPanelLayout()
    {
        DetailPanelBorder.HorizontalAlignment = HorizontalAlignment.Stretch;
        DetailPanelBorder.VerticalAlignment = VerticalAlignment.Stretch;
        DetailPanelBorder.Margin = new Thickness(24);
        DetailPanelBorder.MaxWidth = double.PositiveInfinity;
        DetailPanelBorder.MaxHeight = double.PositiveInfinity;
    }

    private void ShowDailyGoal()
    {
        DailyGoal dailyGoal = _dailyGoals[DateTime.Today.DayOfYear % _dailyGoals.Count];
        DailyGoalTitleTextBlock.Text = dailyGoal.Title;
        DailyGoalDescriptionTextBlock.Text = dailyGoal.Description;
    }

    private void RefreshDiaryItems()
    {
        DiaryItemsControl.ItemsSource = _playerProfile.DiaryEntries.Concat(_staticDiaryEntries).ToList();
    }

    private void AddDiaryEntryIfMissing(string title, string description)
    {
        bool exists = _playerProfile.DiaryEntries.Any(entry => entry.Title == title && entry.Description == description);
        if (exists)
        {
            return;
        }

        _playerProfile.DiaryEntries.Insert(0, new DiaryEntry
        {
            Title = title,
            Description = description
        });

        RefreshDiaryItems();
    }

    private void StartNewRound(LearningSubject subject, RewardTrack rewardTrack, int classLevel)
    {
        _activeSubject = subject;
        _activeRewardTrack = rewardTrack;
        _activeClassLevel = classLevel;
        _currentTasks = GenerateRoundTasks(subject, rewardTrack, classLevel);

        if (_currentTasks.Count == 0)
        {
            throw new InvalidOperationException(
                $"Für Klasse {classLevel}, Fach {GetSubjectDisplayName(subject)} und Track {rewardTrack} konnten keine gültigen Aufgaben geladen werden.");
        }

        _currentTaskIndex = 0;
        _currentCorrectAnswers = 0;
        _hasPendingReward = false;
        _pendingRewardGain = 0;
        _pendingRewardFinalValue = 0;
        _shouldReturnToWorldMapAfterCollect = false;
        _returnToWorldMapInsteadOfNewRound = false;
        _worldStarCollectedThisRound = false;
        _animateWorldMoveOnNextMapOpen = false;
        MathNewRoundButton.Content = "Neue Runde";
        RefreshRewardDisplay();
        ShowCurrentTask();
    }

    private List<LearningTask> GenerateRoundTasks(LearningSubject subject, RewardTrack rewardTrack, int classLevel)
    {
        if (rewardTrack == RewardTrack.WorldStars && classLevel == 3 && subject == LearningSubject.Bonus)
        {
            return GenerateClass3BonusRoundTasks();
        }

        if (rewardTrack == RewardTrack.WorldStars && classLevel == 4 && subject == LearningSubject.Bonus)
        {
            return GenerateClass4BonusRoundTasks();
        }

        if (rewardTrack == RewardTrack.WorldStars)
        {
            return GenerateWorldTasksWithoutRepeats(subject, classLevel);
        }

        return _taskGeneratorFactory.GenerateTasks(new TaskGenerationRequest
        {
            Subject = subject,
            ClassLevel = classLevel,
            TaskCount = GetQuestionCountForTrack(rewardTrack),
            Track = rewardTrack
        }).ToList();
    }

    private List<LearningTask> GenerateWorldTasksWithoutRepeats(LearningSubject subject, int classLevel)
    {
        WorldMapProgress progress = GetWorldMapProgress(classLevel, subject);
        progress.UsedTaskPrompts ??= [];

        List<LearningTask> uniquePool = _taskGeneratorFactory.GenerateTasks(new TaskGenerationRequest
        {
            Subject = subject,
            ClassLevel = classLevel,
            TaskCount = FullTaskPoolRequestCount,
            Track = RewardTrack.WorldStars
        })
            .GroupBy(task => task.Prompt, StringComparer.Ordinal)
            .Select(group => group.First())
            .ToList();

        int requiredTaskCount = GetQuestionCountForTrack(RewardTrack.WorldStars);
        List<LearningTask> freshTasks = uniquePool
            .Where(task => !progress.UsedTaskPrompts.Contains(task.Prompt, StringComparer.Ordinal))
            .Take(requiredTaskCount)
            .ToList();

        if (freshTasks.Count >= requiredTaskCount)
        {
            return freshTasks;
        }

        progress.UsedTaskPrompts.Clear();
        return uniquePool
            .Take(requiredTaskCount)
            .ToList();
    }

    private List<LearningTask> GenerateClass3BonusRoundTasks()
    {
        Random random = new();
        List<LearningTask> mixedPool = [];
        WorldMapProgress progress = GetWorldMapProgress(3, LearningSubject.Bonus);
        progress.UsedTaskPrompts ??= [];
        int requiredTaskCount = GetQuestionCountForTrack(RewardTrack.WorldStars);

        foreach (LearningSubject subject in Class3BonusMixSubjects)
        {
            mixedPool.AddRange(_taskGeneratorFactory.GenerateTasks(new TaskGenerationRequest
            {
                Subject = subject,
                ClassLevel = 3,
                TaskCount = FullTaskPoolRequestCount,
                Track = RewardTrack.WorldStars
            }));
        }

        List<LearningTask> uniquePool = mixedPool
            .GroupBy(task => task.Prompt, StringComparer.Ordinal)
            .Select(group => group.First())
            .ToList();

        List<LearningTask> freshTasks = uniquePool
            .OrderBy(_ => random.Next())
            .Where(task => !progress.UsedTaskPrompts.Contains(task.Prompt, StringComparer.Ordinal))
            .Take(requiredTaskCount)
            .Select((task, index) => new LearningTask
            {
                Title = $"Aufgabe {index + 1}",
                Prompt = task.Prompt,
                Answers = task.Answers.ToArray(),
                CorrectAnswerIndex = task.CorrectAnswerIndex,
                SuccessText = task.SuccessText,
                Topic = task.Topic,
                Subject = task.Subject,
                ClassLevel = task.ClassLevel
            })
            .ToList();

        if (freshTasks.Count >= requiredTaskCount)
        {
            return freshTasks;
        }

        progress.UsedTaskPrompts.Clear();
        return uniquePool
            .OrderBy(_ => random.Next())
            .Take(requiredTaskCount)
            .Select((task, index) => new LearningTask
            {
                Title = $"Aufgabe {index + 1}",
                Prompt = task.Prompt,
                Answers = task.Answers.ToArray(),
                CorrectAnswerIndex = task.CorrectAnswerIndex,
                SuccessText = task.SuccessText,
                Topic = task.Topic,
                Subject = task.Subject,
                ClassLevel = task.ClassLevel
            })
            .ToList();
    }

    private List<LearningTask> GenerateClass4BonusRoundTasks()
    {
        Random random = new();
        List<LearningTask> mixedPool = [];
        WorldMapProgress progress = GetWorldMapProgress(4, LearningSubject.Bonus);
        progress.UsedTaskPrompts ??= [];
        int requiredTaskCount = GetQuestionCountForTrack(RewardTrack.WorldStars);

        foreach (LearningSubject subject in Class4BonusMixSubjects)
        {
            mixedPool.AddRange(_taskGeneratorFactory.GenerateTasks(new TaskGenerationRequest
            {
                Subject = subject,
                ClassLevel = 4,
                TaskCount = FullTaskPoolRequestCount,
                Track = RewardTrack.WorldStars
            }));
        }

        List<LearningTask> uniquePool = mixedPool
            .GroupBy(task => task.Prompt, StringComparer.Ordinal)
            .Select(group => group.First())
            .ToList();

        List<LearningTask> freshTasks = uniquePool
            .OrderBy(_ => random.Next())
            .Where(task => !progress.UsedTaskPrompts.Contains(task.Prompt, StringComparer.Ordinal))
            .Take(requiredTaskCount)
            .Select((task, index) => new LearningTask
            {
                Title = $"Aufgabe {index + 1}",
                Prompt = task.Prompt,
                Answers = task.Answers.ToArray(),
                CorrectAnswerIndex = task.CorrectAnswerIndex,
                SuccessText = task.SuccessText,
                Topic = task.Topic,
                Subject = task.Subject,
                ClassLevel = task.ClassLevel
            })
            .ToList();

        if (freshTasks.Count >= requiredTaskCount)
        {
            return freshTasks;
        }

        progress.UsedTaskPrompts.Clear();
        return uniquePool
            .OrderBy(_ => random.Next())
            .Take(requiredTaskCount)
            .Select((task, index) => new LearningTask
            {
                Title = $"Aufgabe {index + 1}",
                Prompt = task.Prompt,
                Answers = task.Answers.ToArray(),
                CorrectAnswerIndex = task.CorrectAnswerIndex,
                SuccessText = task.SuccessText,
                Topic = task.Topic,
                Subject = task.Subject,
                ClassLevel = task.ClassLevel
            })
            .ToList();
    }

    private void ShowCurrentTask()
    {
        LearningTask task = _currentTasks[_currentTaskIndex];
        if (task.Answers.Length < _taskAnswerButtons.Count)
        {
            throw new InvalidOperationException(
                $"Die Aufgabe \"{task.Prompt}\" enthält nur {task.Answers.Length} Antworten, benötigt werden aber {_taskAnswerButtons.Count}.");
        }

        ApplyTaskDensity(task);

        TaskTitleTextBlock.Text = $"{GetSubjectDisplayName(task.Subject)} - {task.Topic}";
        TaskPromptTextBlock.Text = task.Prompt;
        FeedbackTextBlock.Text = _activeRewardTrack == RewardTrack.WorldStars
            ? $"Level {_activeWorldLevel}: Löse alle {GetQuestionCountForTrack(_activeRewardTrack)} Aufgaben richtig, damit Lumos zum nächsten Punkt der Map läuft."
            : $"Wähle die richtige Antwort aus. Nach der Runde kannst du neue {GetRewardPluralLabel(_activeRewardTrack).ToLowerInvariant()} einsammeln.";
        ProgressTextBlock.Text = $"{_currentTaskIndex + 1} / {_currentTasks.Count}";
        SessionRewardTextBlock.Text = _activeRewardTrack == RewardTrack.WorldStars
            ? $"Level-Fortschritt: {_currentCorrectAnswers} / {GetQuestionCountForTrack(_activeRewardTrack)} richtig. Keine Sterne im Level."
            : $"{GetRewardPluralLabel(_activeRewardTrack)} in dieser Runde: {GetCurrentRoundRewardPreview()}";
        TotalRewardLabelTextBlock.Text = _activeRewardTrack == RewardTrack.WorldStars
            ? "Sterne durch Insel-Sterne"
            : $"{GetRewardPluralLabel(_activeRewardTrack)} gesamt";
        MathNextButton.Visibility = Visibility.Collapsed;
        MathNewRoundButton.Visibility = Visibility.Collapsed;
        CollectRewardButton.Visibility = Visibility.Collapsed;

        for (int index = 0; index < _taskAnswerButtons.Count; index++)
        {
            Button answerButton = _taskAnswerButtons[index];
            answerButton.Visibility = Visibility.Visible;
            answerButton.IsEnabled = true;
            answerButton.Background = CreateBrush(232, 255, 247);
            answerButton.Content = task.Answers[index];
        }

    }

    private void ApplyTaskDensity(LearningTask task)
    {
        int promptLength = task.Prompt.Length;
        int longestAnswerLength = task.Answers.Max(answer => answer.Length);

        TaskTitleTextBlock.FontSize = promptLength > 75 ? 20 : 22;
        TaskPromptTextBlock.FontSize = promptLength switch
        {
            > 95 => 20,
            > 70 => 22,
            > 50 => 23,
            _ => 24
        };

        FeedbackTextBlock.FontSize = promptLength > 85 ? 15 : 16;

        double answerFontSize = longestAnswerLength switch
        {
            > 34 => 17,
            > 26 => 18,
            > 18 => 20,
            _ => 22
        };

        Thickness answerPadding = longestAnswerLength > 26
            ? new Thickness(12)
            : new Thickness(14);

        foreach (Button answerButton in _taskAnswerButtons)
        {
            answerButton.FontSize = answerFontSize;
            answerButton.Padding = answerPadding;
        }
    }

    private void FinishSubjectSession()
    {
        string subjectName = GetSubjectDisplayName(_activeSubject ?? LearningSubject.Math);
        string rewardLabel = GetRewardPluralLabel(_activeRewardTrack);

        if (_activeRewardTrack == RewardTrack.WorldStars && _activeSubject is not null)
        {
            FinishWorldLevelSession(_activeSubject.Value, subjectName);
            return;
        }

        int previousPoints = _activeSubject is null ? 0 : GetSubjectRewardValue(_activeClassLevel, _activeSubject.Value, _activeRewardTrack);
        int newBestPoints = Math.Max(previousPoints, _currentCorrectAnswers);
        int gainedPoints = Math.Max(0, newBestPoints - previousPoints);
        string mapProgressMessage = string.Empty;

        if (_activeRewardTrack == RewardTrack.WorldStars && _activeSubject is not null)
        {
            if (_currentCorrectAnswers == GetQuestionCountForTrack(_activeRewardTrack))
            {
                mapProgressMessage = AdvanceWorldMapProgress(_activeClassLevel, _activeSubject.Value);
                _shouldReturnToWorldMapAfterCollect = true;
                _returnToWorldMapInsteadOfNewRound = true;
            }
            else
            {
                int requiredTaskCount = GetQuestionCountForTrack(_activeRewardTrack);
                mapProgressMessage = $" Level {_activeWorldLevel} ist noch nicht abgeschlossen. Für den nächsten Schritt auf der Map brauchst du diesmal {requiredTaskCount} von {requiredTaskCount} richtigen Antworten.";
            }
        }

        _pendingRewardGain = gainedPoints;
        _pendingRewardFinalValue = newBestPoints;
        _hasPendingReward = gainedPoints > 0;

        TaskTitleTextBlock.Text = "Rundenende";
        TaskPromptTextBlock.Text = $"Du hast den Bereich {subjectName} für Klasse {_activeClassLevel} beendet.";
        ProgressTextBlock.Text = $"{_currentTasks.Count} / {_currentTasks.Count}";
        SessionRewardTextBlock.Text = $"{rewardLabel} in dieser Runde: {gainedPoints}";

        if (gainedPoints > 0)
        {
            FeedbackTextBlock.Text = $"Du hast {_currentCorrectAnswers} von {GetQuestionCountForTrack(_activeRewardTrack)} Fragen richtig gelöst.{mapProgressMessage} Jetzt kannst du {gainedPoints} {rewardLabel.ToLowerInvariant()} einsammeln.";
            CollectRewardButton.Content = $"{rewardLabel} einsammeln";
            CollectRewardButton.Visibility = Visibility.Visible;
        }
        else
        {
            FeedbackTextBlock.Text = $"Du hast {_currentCorrectAnswers} von {GetQuestionCountForTrack(_activeRewardTrack)} Fragen richtig gelöst.{mapProgressMessage} Dieses Fach steht bereits bei {previousPoints} von {PortalRewardPerSubject}, daher gibt es diesmal keine neuen {rewardLabel.ToLowerInvariant()}.";
            MathNewRoundButton.Content = _returnToWorldMapInsteadOfNewRound ? "Zurück zur Map" : "Neue Runde";
            MathNewRoundButton.Visibility = Visibility.Visible;
        }

        foreach (Button answerButton in _taskAnswerButtons)
        {
            answerButton.Visibility = Visibility.Collapsed;
        }

        MathNextButton.Visibility = Visibility.Collapsed;
    }

    private void FinishWorldLevelSession(LearningSubject subject, string subjectName)
    {
        int requiredTaskCount = GetQuestionCountForTrack(RewardTrack.WorldStars);
        bool levelSolved = _currentCorrectAnswers == requiredTaskCount;
        RememberUsedWorldTaskPrompts(subject);

        TaskTitleTextBlock.Text = "Level beendet";
        TaskPromptTextBlock.Text = $"Du hast Level {_activeWorldLevel} der {subjectName}-Insel gespielt.";
        ProgressTextBlock.Text = $"{_currentTasks.Count} / {_currentTasks.Count}";
        SessionRewardTextBlock.Text = "In Welt-Leveln werden keine Sterne gesammelt.";

        foreach (Button answerButton in _taskAnswerButtons)
        {
            answerButton.Visibility = Visibility.Collapsed;
        }

        MathNextButton.Visibility = Visibility.Collapsed;
        CollectRewardButton.Visibility = Visibility.Collapsed;

        if (!levelSolved)
        {
            FeedbackTextBlock.Text = $"Du hast {_currentCorrectAnswers} von {requiredTaskCount} Fragen richtig gelöst. Level {_activeWorldLevel} bleibt aktiv, bis alle {requiredTaskCount} Aufgaben richtig sind.";
            MathNewRoundButton.Content = "Level wiederholen";
            MathNewRoundButton.Visibility = Visibility.Visible;
            return;
        }

        string mapProgressMessage = AdvanceWorldMapProgress(_activeClassLevel, subject);

        if (_worldStarCollectedThisRound)
        {
            int previousPoints = GetSubjectRewardValue(_activeClassLevel, subject, RewardTrack.WorldStars);
            _pendingRewardGain = Math.Max(0, WorldRewardPerSubject - previousPoints);
            _pendingRewardFinalValue = WorldRewardPerSubject;
            _hasPendingReward = _pendingRewardGain > 0;
            _shouldReturnToWorldMapAfterCollect = true;
            _returnToWorldMapInsteadOfNewRound = true;
            _animateWorldMoveOnNextMapOpen = true;

            FeedbackTextBlock.Text = $"Perfekt: Du hast alle {requiredTaskCount} Aufgaben richtig gelöst.{mapProgressMessage} Jetzt kannst du den Stern einsammeln.";
            SessionRewardTextBlock.Text = $"Sterne in dieser Runde: {_pendingRewardGain}";
            CollectRewardButton.Content = "Stern einsammeln";
            CollectRewardButton.Visibility = _hasPendingReward ? Visibility.Visible : Visibility.Collapsed;
            MathNewRoundButton.Content = "Zurück zur Map";
            MathNewRoundButton.Visibility = _hasPendingReward ? Visibility.Collapsed : Visibility.Visible;
            return;
        }

        FeedbackTextBlock.Text = $"Perfekt: Du hast alle {requiredTaskCount} Aufgaben richtig gelöst.{mapProgressMessage}";
        MathNewRoundButton.Content = "Zurück zur Map";
        MathNewRoundButton.Visibility = Visibility.Visible;
        _returnToWorldMapInsteadOfNewRound = true;
        _animateWorldMoveOnNextMapOpen = true;
    }

    private string BuildUnlockMessage(bool unlockedWorldBefore, bool unlockedNextPortalBefore)
    {
        if (_activeRewardTrack == RewardTrack.PortalTrophies)
        {
            if (!unlockedWorldBefore && IsWorldClassUnlocked(_activeClassLevel))
            {
                return $" Erfolg: Welt Klasse {_activeClassLevel} ist freigeschaltet.";
            }

            return string.Empty;
        }

        int nextPortalClassLevel = _activeClassLevel + 1;
        if (nextPortalClassLevel <= 4 && !unlockedNextPortalBefore && IsPortalClassUnlocked(nextPortalClassLevel))
        {
            return $" Erfolg: Klasse {nextPortalClassLevel} im Lernportal ist freigeschaltet.";
        }

        return string.Empty;
    }

    private void RefreshRewardDisplay()
    {
        NormalizeRewardTotals();
        TrophiesCountTextBlock.Text = _playerProfile.Trophies.ToString();
        TrophiesInfoTextBlock.Text = $"Klasse 1 Lernportal: {GetClassRewardTotal(1, RewardTrack.PortalTrophies)} / {GetPortalUnlockRequirement(1)} Pokale";
        StarsCountTextBlock.Text = _playerProfile.Stars.ToString();
        StarsInfoTextBlock.Text = $"Welt Klasse 1: {GetClassRewardTotal(1, RewardTrack.WorldStars)} / {GetWorldUnlockRequirement(1)} Sterne";
        TotalStarsTextBlock.Text = _activeRewardTrack == RewardTrack.PortalTrophies
            ? _playerProfile.Trophies.ToString()
            : _playerProfile.Stars.ToString();
        TotalRewardLabelTextBlock.Text = $"{GetRewardPluralLabel(_activeRewardTrack)} gesamt";
        UpdateMissingPortalRewardsOverview();
    }

    private bool HasPendingWorldStarToCollect(int classLevel, LearningSubject subject)
    {
        return GetWorldMapProgress(classLevel, subject).StarCollected &&
               GetSubjectRewardValue(classLevel, subject, RewardTrack.WorldStars) < WorldRewardPerSubject;
    }

    private void CollectPendingWorldStar(LearningSubject subject)
    {
        int previousPoints = GetSubjectRewardValue(_activeClassLevel, subject, RewardTrack.WorldStars);
        _pendingRewardGain = Math.Max(0, WorldRewardPerSubject - previousPoints);
        _pendingRewardFinalValue = WorldRewardPerSubject;
        _hasPendingReward = _pendingRewardGain > 0;

        if (!_hasPendingReward)
        {
            ShowWorldIslandPanel(subject);
            return;
        }

        bool unlockedNextPortalBefore = IsPortalClassUnlocked(_activeClassLevel + 1);
        SetSubjectRewardValue(_activeClassLevel, subject, RewardTrack.WorldStars, _pendingRewardFinalValue);
        SavePlayerProfile();
        RefreshRewardDisplay();
        UpdateClassCardStates();

        string unlockMessage = BuildUnlockMessage(unlockedWorldBefore: false, unlockedNextPortalBefore: unlockedNextPortalBefore);
        AddWorldStarDiaryEntry(subject);

        if (_activeClassLevel == 4 && subject == LearningSubject.Bonus)
        {
            _hasPendingReward = false;
            _pendingRewardGain = 0;
            _pendingRewardFinalValue = 0;
            ShowGrandMasteryPanel();
            return;
        }

        WorldIslandProgressTextBlock.Text = $"Der Stern wurde eingesammelt.{unlockMessage}";
        StartIslandRoundButton.Content = "Stern eingesammelt";
        StartIslandRoundButton.IsEnabled = false;
        StartIslandRoundButton.Opacity = 0.55;
        _hasPendingReward = false;
        _pendingRewardGain = 0;
        _pendingRewardFinalValue = 0;
    }

    private void UpdateMissingPortalRewardsOverview()
    {
        List<string> missingItems = [];

        foreach ((int classLevel, IReadOnlyList<LearningSubject> subjects) in GetPlayablePortalSubjectGroups())
        {
            foreach (LearningSubject subject in subjects)
            {
                int currentPoints = GetSubjectRewardValue(classLevel, subject, RewardTrack.PortalTrophies);
                int missingPoints = Math.Max(0, PortalRewardPerSubject - currentPoints);
                if (missingPoints <= 0)
                {
                    continue;
                }

                missingItems.Add($"Klasse {classLevel} - {GetSubjectDisplayName(subject)}: {currentPoints} / {PortalRewardPerSubject} Pokale gesammelt, es fehlen noch {missingPoints}.");
            }
        }

        if (missingItems.Count == 0)
        {
            MissingPortalRewardsSummaryTextBlock.Text = "In den aktuell spielbaren Lernportal-Fächern fehlen keine Pokale mehr.";
            MissingPortalRewardsItemsControl.ItemsSource = new[] { "Alle Pokale in den aktuell freigeschalteten Klassen sind vollständig gesammelt." };
            return;
        }

        MissingPortalRewardsSummaryTextBlock.Text = "Hier siehst du sofort, in welchen aktuell spielbaren Lernportal-Fächern noch Pokale fehlen.";
        MissingPortalRewardsItemsControl.ItemsSource = missingItems;
    }

    private IEnumerable<(int ClassLevel, IReadOnlyList<LearningSubject> Subjects)> GetPlayablePortalSubjectGroups()
    {
        yield return (1, Class1PortalSubjects);

        if (IsPortalClassUnlocked(2))
        {
            yield return (2, Class2PortalSubjects);
        }

        if (IsPortalClassUnlocked(3))
        {
            yield return (3, Class3PortalSubjects);
        }

        if (IsPortalClassUnlocked(4))
        {
            yield return (4, Class4PortalSubjects);
        }
    }

    private void AddWorldStarDiaryEntry(LearningSubject subject)
    {
        string subjectName = GetWorldSubjectDisplayName(_activeClassLevel, subject);
        AddDiaryEntryIfMissing(
            subject == LearningSubject.Bonus
                ? "Erfolg: Bonus-Stern eingesammelt"
                : $"Erfolg: Stern auf der {subjectName}-Insel eingesammelt",
            subject == LearningSubject.Bonus
                ? $"Lumos hat den Bonus-Stern in Welt Klasse {_activeClassLevel} erfolgreich eingesammelt."
                : $"Lumos hat den Stern der {subjectName}-Insel in Welt Klasse {_activeClassLevel} erfolgreich eingesammelt.");
    }

    private bool IsClass3BonusWorldUnlocked()
    {
        return GetClassRewardTotal(3, RewardTrack.WorldStars) >= Class3PortalSubjects.Length;
    }

    private bool IsClass4BonusWorldUnlocked()
    {
        return GetClassRewardTotal(4, RewardTrack.WorldStars) >= Class4PortalSubjects.Length;
    }

    private void UpdateClassCardStates()
    {
        UpdatePortalClassCard(1, PortalClass1Button, PortalClass1StatusTextBlock);
        UpdatePortalClassCard(2, PortalClass2Button, PortalClass2StatusTextBlock);
        UpdatePortalClassCard(3, PortalClass3Button, PortalClass3StatusTextBlock);
        UpdatePortalClassCard(4, PortalClass4Button, PortalClass4StatusTextBlock);

        UpdateWorldClassCard(1, WorldClass1Button, WorldClass1StatusTextBlock);
        UpdateWorldClassCard(2, WorldClass2Button, WorldClass2StatusTextBlock);
        UpdateWorldClassCard(3, WorldClass3Button, WorldClass3StatusTextBlock);
        UpdateWorldClassCard(4, WorldClass4Button, WorldClass4StatusTextBlock);
    }

    private void UpdatePortalClassCard(int classLevel, Button button, TextBlock statusTextBlock)
    {
        bool unlocked = IsPortalClassUnlocked(classLevel);
        button.Opacity = unlocked ? 1.0 : 0.6;

        if (!unlocked)
        {
            int currentStars = GetClassRewardTotal(classLevel - 1, RewardTrack.WorldStars);
            statusTextBlock.Text = $"{currentStars} / {GetWorldUnlockRequirement(classLevel - 1)} Sterne";
            return;
        }

        int currentTrophies = GetClassRewardTotal(classLevel, RewardTrack.PortalTrophies);
        statusTextBlock.Text = $"{currentTrophies} / {GetPortalUnlockRequirement(classLevel)} Pokale";
    }

    private void UpdateWorldClassCard(int classLevel, Button button, TextBlock statusTextBlock)
    {
        bool unlocked = IsWorldClassUnlocked(classLevel);
        button.Opacity = unlocked ? 1.0 : 0.6;

        if (!unlocked)
        {
            int currentTrophies = GetClassRewardTotal(classLevel, RewardTrack.PortalTrophies);
            statusTextBlock.Text = $"{currentTrophies} / {GetPortalUnlockRequirement(classLevel)} Pokale";
            return;
        }

        statusTextBlock.Text = $"{GetClassRewardTotal(classLevel, RewardTrack.WorldStars)} / {GetWorldUnlockRequirement(classLevel)} Sterne";
    }

    private string BuildSettingsText()
    {
        string soundText = SoundEnabledCheckBox?.IsChecked == true ? "Sound ist aktiv." : "Sound ist ausgeschaltet.";
        string fullscreenText = FullscreenCheckBox?.IsChecked == true
            ? " Vollbild ist aktiv. Mit Esc verlässt du den Vollbildmodus."
            : " Vollbild ist ausgeschaltet.";
        return soundText + fullscreenText;
    }

    private int GetCurrentRoundRewardPreview()
    {
        if (_activeSubject is null)
        {
            return 0;
        }

        int previousPoints = GetSubjectRewardValue(_activeClassLevel, _activeSubject.Value, _activeRewardTrack);
        int newBestPoints = Math.Max(previousPoints, _currentCorrectAnswers);
        return Math.Max(0, newBestPoints - previousPoints);
    }

    private bool IsPortalClassUnlocked(int classLevel)
    {
        if (classLevel <= 1)
        {
            return true;
        }

        return GetClassRewardTotal(classLevel - 1, RewardTrack.WorldStars) >= GetWorldUnlockRequirement(classLevel - 1);
    }

    private static int GetWorldUnlockRequirement(int classLevel)
    {
        return classLevel switch
        {
            1 => DefaultWorldUnlockRequirement,
            2 or 3 => Class2WorldUnlockRequirement,
            4 => Class2WorldUnlockRequirement + WorldRewardPerSubject,
            _ => DefaultWorldUnlockRequirement
        };
    }

    private bool IsWorldClassUnlocked(int classLevel)
    {
        return GetClassRewardTotal(classLevel, RewardTrack.PortalTrophies) >= GetPortalUnlockRequirement(classLevel);
    }

    private static int GetPortalUnlockRequirement(int classLevel)
    {
        return classLevel switch
        {
            1 => DefaultPortalUnlockRequirement,
            2 or 3 => Class2PortalUnlockRequirement,
            4 => Class2PortalUnlockRequirement + PortalRewardPerSubject,
            _ => DefaultPortalUnlockRequirement
        };
    }

    private int GetClassRewardTotal(int classLevel, RewardTrack track)
    {
        if (classLevel <= 0)
        {
            return 0;
        }

        IReadOnlyCollection<LearningSubject>? supportedSubjects = GetSupportedSubjectsForClass(classLevel);

        return _playerProfile.RewardProgress
            .Where(progress => progress.ClassLevel == classLevel && progress.Track == track)
            .Where(progress => supportedSubjects is null || supportedSubjects.Contains(progress.Subject))
            .Sum(progress => progress.EarnedPoints);
    }

    private static IReadOnlyCollection<LearningSubject>? GetSupportedSubjectsForClass(int classLevel)
    {
        return classLevel switch
        {
            1 => Class1PortalSubjects,
            2 => Class2PortalSubjects,
            3 => Class3PortalSubjects,
            4 => Class4PortalSubjects,
            _ => null
        };
    }

    private int GetSubjectRewardValue(int classLevel, LearningSubject subject, RewardTrack track)
    {
        SubjectRewardProgress? progress = _playerProfile.RewardProgress.FirstOrDefault(existing =>
            existing.ClassLevel == classLevel &&
            existing.Subject == subject &&
            existing.Track == track);

        return progress?.EarnedPoints ?? 0;
    }

    private void SetSubjectRewardValue(int classLevel, LearningSubject subject, RewardTrack track, int earnedPoints)
    {
        int safePoints = Math.Max(0, Math.Min(GetRewardCap(track), earnedPoints));
        SubjectRewardProgress? existingProgress = _playerProfile.RewardProgress.FirstOrDefault(progress =>
            progress.ClassLevel == classLevel &&
            progress.Subject == subject &&
            progress.Track == track);

        if (existingProgress is null)
        {
            _playerProfile.RewardProgress.Add(new SubjectRewardProgress
            {
                ClassLevel = classLevel,
                Subject = subject,
                Track = track,
                EarnedPoints = safePoints,
                CompletedAt = DateTime.Now
            });
            return;
        }

        existingProgress.EarnedPoints = safePoints;
        existingProgress.CompletedAt = DateTime.Now;
    }

    private void NormalizeRewardTotals()
    {
        _playerProfile.RewardProgress ??= [];

        foreach (SubjectRewardProgress progress in _playerProfile.RewardProgress)
        {
            progress.EarnedPoints = Math.Max(0, Math.Min(GetRewardCap(progress.Track), progress.EarnedPoints));
        }

        _playerProfile.Trophies = _playerProfile.RewardProgress
            .Where(progress => progress.Track == RewardTrack.PortalTrophies)
            .Sum(progress => progress.EarnedPoints);
        _playerProfile.Stars = _playerProfile.RewardProgress
            .Where(progress => progress.Track == RewardTrack.WorldStars)
            .Sum(progress => progress.EarnedPoints);
    }

    private static int GetRewardCap(RewardTrack track)
    {
        return track == RewardTrack.WorldStars ? WorldRewardPerSubject : PortalRewardPerSubject;
    }

    private void UpdateSettingsStatus()
    {
        if (SettingsStatusTextBlock is null)
        {
            return;
        }

        SettingsStatusTextBlock.Text = BuildSettingsText();
    }

    private void CloseDetailButton_Click(object sender, RoutedEventArgs e)
    {
        HideAllPanels();
    }

    private void MainMenuWindow_Closed(object? sender, EventArgs e)
    {
        _taskSpeechService.Dispose();
        SavePlayerProfile();
    }

    private void LoadAvatarImage(string? avatarPath)
    {
        if (string.IsNullOrWhiteSpace(avatarPath) || !File.Exists(avatarPath))
        {
            avatarPath = GetImagePath("Lumos.png");
        }

        AvatarImage.Source = new BitmapImage(new Uri(avatarPath, UriKind.Absolute));
    }

    private void ApplySceneImage(string imageName)
    {
        SceneImage.Source = CreateBitmap(imageName);
    }

    private void LoadClassPreviewImages()
    {
        SetImageSource(PortalClass1Image, "Klasse 1.png");
        SetImageSource(PortalClass2Image, "klasse 2.png");
        SetImageSource(PortalClass3Image, "Klasse 3.png");
        SetImageSource(PortalClass4Image, "KLasse 4.png");
        SetImageSource(WorldClass1Image, "Klasse 1.png");
        SetImageSource(WorldClass2Image, "klasse 2.png");
        SetImageSource(WorldClass3Image, "Klasse 3.png");
        SetImageSource(WorldClass4Image, "KLasse 4.png");
    }

    private void LoadSubjectPreviewImages()
    {
        TrySetSubjectImage(MathImage, MathPlaceholderText, "Mathematik");
        TrySetSubjectImage(GermanImage, GermanPlaceholderText, "Deutsch");
        TrySetSubjectImage(SachkundeImage, SachkundePlaceholderText, "Sachkunde");
        TrySetSubjectImage(MusicImage, MusicPlaceholderText, "Musik");
        TrySetSubjectImage(Class2GermanImage, Class2GermanPlaceholderText, "Deutsch");
        TrySetSubjectImage(Class2MathImage, Class2MathPlaceholderText, "Mathematik");
        TrySetSubjectImage(Class2SachkundeImage, Class2SachkundePlaceholderText, "Sachkunde");
        TrySetSubjectImage(Class2LogicImage, Class2LogicPlaceholderText, "Logik");
        TrySetSubjectImage(Class2MusicImage, Class2MusicPlaceholderText, "Musik");
        TrySetSubjectImage(Class3LanguageImage, Class3LanguagePlaceholderText, "Deutsch");
        TrySetSubjectImage(Class3MathImage, Class3MathPlaceholderText, "Mathematik");
        TrySetSubjectImage(Class3SachkundeImage, Class3SachkundePlaceholderText, "Sachkunde");
        TrySetSubjectImage(Class3MediaImage, Class3MediaPlaceholderText, "Logik");
        TrySetSubjectImage(Class3CreativityImage, Class3CreativityPlaceholderText, "Musik");
        TrySetSubjectImage(Class3BonusImage, Class3BonusPlaceholderText, "Bonus");
        TrySetSubjectImage(Class4GermanImage, Class4GermanPlaceholderText, "Deutsch");
        TrySetSubjectImage(Class4MathImage, Class4MathPlaceholderText, "Mathematik");
        TrySetSubjectImage(Class4SachkundeImage, Class4SachkundePlaceholderText, "Sachkunde");
        TrySetSubjectImage(Class4MusicImage, Class4MusicPlaceholderText, "Musik");
        TrySetSubjectImage(Class4ArtImage, Class4ArtPlaceholderText, "Kunst");
        TrySetSubjectImage(Class4EnglishImage, Class4EnglishPlaceholderText, "Englisch");
        TrySetSubjectImage(Class4BonusImage, Class4BonusPlaceholderText, "Bonus");
    }

    private void TrySetSubjectImage(Image imageControl, TextBlock placeholderText, string subjectName)
    {
        string? previewImage = FindPreviewImage(subjectName);
        if (previewImage is null)
        {
            placeholderText.Visibility = Visibility.Visible;
            return;
        }

        imageControl.Source = CreateBitmap(previewImage);
        placeholderText.Visibility = Visibility.Collapsed;
    }

    private static string? FindPreviewImage(string imageName)
    {
        string[] candidateFolders =
        [
            Path.Combine(AppContext.BaseDirectory, "Bilder", "Welten", "Klasse1"),
            Path.Combine(AppContext.BaseDirectory, "Bilder", "Inseln", "Klasse1"),
            Path.Combine(AppContext.BaseDirectory, "Bilder", "Faecher", "Klasse1"),
            Path.Combine(AppContext.BaseDirectory, "Bilder")
        ];

        string[] extensions = [".png", ".jpg", ".jpeg", ".webp", ".bmp"];

        foreach (string folder in candidateFolders)
        {
            foreach (string extension in extensions)
            {
                string candidate = Path.Combine(folder, imageName.EndsWith(extension, StringComparison.OrdinalIgnoreCase)
                    ? imageName
                    : $"{imageName}{extension}");

                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }

            string directCandidate = Path.Combine(folder, imageName);
            if (File.Exists(directCandidate))
            {
                return directCandidate;
            }
        }

        return null;
    }

    private static string GetWorldIslandImage(int classLevel)
    {
        return classLevel switch
        {
            4 => FindPreviewImage("KLasse 4") ?? "KLasse 4.png",
            3 => FindPreviewImage("Klasse 3") ?? "Klasse 3.png",
            2 => FindPreviewImage("klasse 2") ?? "klasse 2.png",
            _ => FindPreviewImage("Klasse 1") ?? "Klasse 1.png"
        };
    }

    private static string GetWorldIslandDescription(int classLevel, LearningSubject subject)
    {
        if (classLevel == 3)
        {
            return subject switch
            {
                LearningSubject.German => $"Lumos steht auf der Deutsch-Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst, wenn Lumos den Ziel-Stern erreicht.",
                LearningSubject.Math => $"Lumos steht auf der Mathe-Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst, wenn Lumos den Ziel-Stern erreicht.",
                LearningSubject.Sachunterricht => $"Lumos steht auf der Sachkunde-Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst, wenn Lumos den Ziel-Stern erreicht.",
                LearningSubject.Logic => $"Lumos steht auf der Logik-Insel. Auf der Klasse-3-Map nutzt Logik die orange Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst, wenn Lumos den Ziel-Stern erreicht.",
                LearningSubject.Music => $"Lumos steht auf der Musik-Insel. Auf der Klasse-3-Map nutzt Musik die pinke Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst, wenn Lumos den Ziel-Stern erreicht.",
                LearningSubject.Bonus => $"Lumos steht auf der Bonus-Insel. Hier gibt es 3 Bonus-Level mit je {WorldQuestionsPerLevel} gemischten Fragen aus allen Klasse-3-Fächern. Danach wartet ein Extra-Stern.",
                _ => $"Lumos steht auf der passenden Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben."
            };
        }

        if (classLevel == 4)
        {
            return subject switch
            {
                LearningSubject.German => $"Lumos steht auf der Deutsch-Insel von Klasse 4. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst am Ziel-Stern der Insel.",
                LearningSubject.Math => $"Lumos steht auf der Mathe-Insel von Klasse 4. Auf der Karte ist das die blaue Mathe-Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst am Ziel-Stern der Insel.",
                LearningSubject.Sachunterricht => $"Lumos steht auf der Sachkunde-Insel von Klasse 4. Auf der Karte ist das die grüne Mittelinsel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst am Ziel-Stern der Insel.",
                LearningSubject.Music => $"Lumos steht auf der Musik-Insel von Klasse 4. Auf der Karte nutzt Musik die Medien-Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst am Ziel-Stern der Insel.",
                LearningSubject.Art => $"Lumos steht auf der Kunst-Insel von Klasse 4. Auf der Karte nutzt Kunst die Kreativitäts-Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst am Ziel-Stern der Insel.",
                LearningSubject.English => $"Lumos steht auf der Englisch-Insel von Klasse 4. Auf der Karte nutzt Englisch die Projekt-Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst am Ziel-Stern der Insel.",
                LearningSubject.Bonus => $"Lumos steht auf der Meisterinsel. Hier gibt es 3 Bonus-Level mit je {WorldQuestionsPerLevel} gemischten Fragen aus allen Klasse-4-Fächern. Danach wartet ein Extra-Stern.",
                _ => $"Lumos steht auf der passenden Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben."
            };
        }

        if (classLevel == 2)
        {
            return subject switch
            {
                LearningSubject.German => $"Lumos steht auf der Deutsch-/Sprach-Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst, wenn Lumos den Insel-Stern erreicht.",
                LearningSubject.Math => $"Lumos steht auf der Mathe-Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst, wenn Lumos den Insel-Stern erreicht.",
                LearningSubject.Sachunterricht => $"Lumos steht auf der Sachkunde-Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst, wenn Lumos den Insel-Stern erreicht.",
                LearningSubject.Logic => $"Lumos steht auf der Logik-Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst, wenn Lumos den Insel-Stern erreicht.",
                LearningSubject.Music => $"Lumos steht auf der Musik-Insel. Auf der Klasse-2-Map nutzt sie die blaue Medien-Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst, wenn Lumos den Insel-Stern erreicht.",
                _ => $"Lumos steht auf der passenden Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben."
            };
        }

        return subject switch
        {
            LearningSubject.Math => $"Lumos steht auf der Mathematik-Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst, wenn Lumos den Insel-Stern erreicht.",
            LearningSubject.German => $"Lumos steht auf der Deutsch-Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst, wenn Lumos den Insel-Stern erreicht.",
            LearningSubject.Sachunterricht => $"Lumos steht auf der Sachkunde-Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst, wenn Lumos den Insel-Stern erreicht.",
            LearningSubject.Music => $"Lumos steht auf der Kreativ-/Musik-Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst, wenn Lumos den Insel-Stern erreicht.",
            _ => $"Lumos steht auf der passenden Insel. Jedes Level hat {WorldQuestionsPerLevel} Aufgaben."
        };
    }

    private void UpdateWorldIslandProgressDisplay(LearningSubject subject, WorldMapProgress progress)
    {
        string subjectName = GetSubjectDisplayName(subject);

        if (progress.StarCollected)
        {
            if (HasPendingWorldStarToCollect(_activeClassLevel, subject))
            {
                WorldIslandProgressTextBlock.Text = $"Lumos hat den Stern der {subjectName}-Insel erreicht. Drücke auf Stern einsammeln, damit er gutgeschrieben wird.";
                StartIslandRoundButton.Content = "Stern einsammeln";
                StartIslandRoundButton.IsEnabled = true;
                StartIslandRoundButton.Opacity = 1;
                return;
            }

            WorldIslandProgressTextBlock.Text = $"Der Stern der {subjectName}-Insel wurde eingesammelt. Wähle eine andere Insel oder wiederhole später Aufgaben.";
            StartIslandRoundButton.Content = "Stern eingesammelt";
            StartIslandRoundButton.IsEnabled = false;
            StartIslandRoundButton.Opacity = 0.55;
            return;
        }

        int currentLevel = GetCurrentWorldStep(progress, _activeClassLevel, subject);
        WorldIslandProgressTextBlock.Text = $"Aktueller Wegpunkt: Level {currentLevel}. Löse die Runde fehlerfrei, damit Lumos automatisch weiterläuft.";
        StartIslandRoundButton.Content = $"Level {currentLevel} starten";
        StartIslandRoundButton.IsEnabled = true;
        StartIslandRoundButton.Opacity = 1;
    }

    private void PositionWorldLevelMarkers(LearningSubject subject, WorldMapProgress progress)
    {
        Ellipse[] levelMarkers =
        [
            WorldStep1Marker,
            WorldStep2Marker,
            WorldStep3Marker,
            WorldStep4Marker,
            WorldStep5Marker
        ];

        int levelCount = GetWorldLevelCount(_activeClassLevel, subject);

        for (int step = 1; step <= levelMarkers.Length; step++)
        {
            if (step > levelCount)
            {
                levelMarkers[step - 1].Visibility = Visibility.Collapsed;
                continue;
            }

            levelMarkers[step - 1].Visibility = Visibility.Visible;
            Point point = GetWorldPathPoint(_activeClassLevel, subject, step);
            PositionMarker(levelMarkers[step - 1], point, WorldWaypointMarkerSize);

            bool completed = progress.CompletedLevels >= step;
            bool current = !progress.StarCollected && GetCurrentWorldStep(progress, _activeClassLevel, subject) == step;
            levelMarkers[step - 1].Fill = completed ? CreateBrushWithAlpha(88, 176, 233, 151) : CreateBrushWithAlpha(34, 255, 255, 255);
            levelMarkers[step - 1].Stroke = current ? CreateBrush(255, 224, 138) : CreateBrush(255, 255, 255);
            levelMarkers[step - 1].StrokeThickness = current ? 8 : 4;
        }

        Point starPoint = GetWorldPathPoint(_activeClassLevel, subject, levelCount + 1);
        PositionMarker(WorldStarMarker, starPoint, 96);
        WorldStarMarker.Fill = progress.StarCollected ? CreateBrushWithAlpha(105, 255, 224, 138) : CreateBrushWithAlpha(34, 255, 255, 255);
        WorldStarMarker.Stroke = progress.StarCollected ? CreateBrush(255, 224, 138) : CreateBrush(255, 255, 255);
    }

    private void PositionLumosOnIsland(LearningSubject subject, int step, bool animatePendingMove)
    {
        LumosIslandImage.Width = 130;
        LumosIslandImage.Height = 130;
        StopCanvasMoveAnimations(LumosIslandImage);
        StopCanvasMoveAnimations(LumosIslandGlow);

        Point targetPoint = GetWorldPathPoint(_activeClassLevel, subject, step);
        double targetLeft = targetPoint.X - 65;
        double targetTop = targetPoint.Y - 100;
        double targetGlowLeft = targetPoint.X - 85;
        double targetGlowTop = targetPoint.Y - 105;

        if (animatePendingMove &&
            _pendingWorldMoveFromStep is not null &&
            _pendingWorldMoveToStep is not null &&
            _pendingWorldMoveClassLevel == _activeClassLevel &&
            _pendingWorldMoveSubject == subject &&
            _pendingWorldMoveToStep.Value == step)
        {
            Point startPoint = GetWorldPathPoint(_activeClassLevel, subject, _pendingWorldMoveFromStep.Value);
            Canvas.SetLeft(LumosIslandImage, startPoint.X - 65);
            Canvas.SetTop(LumosIslandImage, startPoint.Y - 100);
            Canvas.SetLeft(LumosIslandGlow, startPoint.X - 85);
            Canvas.SetTop(LumosIslandGlow, startPoint.Y - 105);
            AnimateCanvasMove(LumosIslandImage, targetLeft, targetTop);
            AnimateCanvasMove(LumosIslandGlow, targetGlowLeft, targetGlowTop);
            ClearPendingWorldMove();
            return;
        }

        if (animatePendingMove)
        {
            int starStep = GetWorldLevelCount(_activeClassLevel, subject) + 1;
            int fallbackStartStep = step switch
            {
                <= 1 => 1,
                _ when step == starStep => starStep - 1,
                _ => step - 1
            };

            if (fallbackStartStep != step)
            {
                Point startPoint = GetWorldPathPoint(_activeClassLevel, subject, fallbackStartStep);
                Canvas.SetLeft(LumosIslandImage, startPoint.X - 65);
                Canvas.SetTop(LumosIslandImage, startPoint.Y - 100);
                Canvas.SetLeft(LumosIslandGlow, startPoint.X - 85);
                Canvas.SetTop(LumosIslandGlow, startPoint.Y - 105);
                AnimateCanvasMove(LumosIslandImage, targetLeft, targetTop);
                AnimateCanvasMove(LumosIslandGlow, targetGlowLeft, targetGlowTop);
                return;
            }
        }

        Canvas.SetLeft(LumosIslandGlow, targetGlowLeft);
        Canvas.SetTop(LumosIslandGlow, targetGlowTop);
        Canvas.SetLeft(LumosIslandImage, targetLeft);
        Canvas.SetTop(LumosIslandImage, targetTop);
    }

    private WorldMapProgress GetWorldMapProgress(int classLevel, LearningSubject subject)
    {
        _playerProfile.WorldMapProgress ??= [];

        WorldMapProgress? progress = _playerProfile.WorldMapProgress.FirstOrDefault(existing =>
            existing.ClassLevel == classLevel &&
            existing.Subject == subject);

        if (progress is not null)
        {
            int maxLevels = GetWorldLevelCount(classLevel, subject);
            progress.CompletedLevels = Math.Max(0, Math.Min(maxLevels, progress.CompletedLevels));
            progress.StarCollected = progress.StarCollected || progress.CompletedLevels >= maxLevels;
            progress.UsedTaskPrompts ??= [];
            return progress;
        }

        progress = new WorldMapProgress
        {
            ClassLevel = classLevel,
            Subject = subject
        };
        _playerProfile.WorldMapProgress.Add(progress);
        return progress;
    }

    private void RememberUsedWorldTaskPrompts(LearningSubject subject)
    {
        WorldMapProgress progress = GetWorldMapProgress(_activeClassLevel, subject);
        progress.UsedTaskPrompts ??= [];

        foreach (string prompt in _currentTasks.Select(task => task.Prompt).Distinct(StringComparer.Ordinal))
        {
            if (!progress.UsedTaskPrompts.Contains(prompt, StringComparer.Ordinal))
            {
                progress.UsedTaskPrompts.Add(prompt);
            }
        }

        progress.UpdatedAt = DateTime.Now;
    }

    private static int GetWorldLevelCount(int classLevel, LearningSubject subject)
    {
        return (classLevel == 3 || classLevel == 4) && subject == LearningSubject.Bonus ? 3 : 5;
    }

    private int GetCurrentWorldStep(WorldMapProgress progress, int classLevel, LearningSubject subject)
    {
        if (progress.StarCollected)
        {
            return GetWorldLevelCount(classLevel, subject) + 1;
        }

        int levelCount = GetWorldLevelCount(classLevel, subject);
        return Math.Max(1, Math.Min(levelCount, progress.CompletedLevels + 1));
    }

    private string AdvanceWorldMapProgress(int classLevel, LearningSubject subject)
    {
        WorldMapProgress progress = GetWorldMapProgress(classLevel, subject);

        if (progress.StarCollected)
        {
            ClearPendingWorldMove();
            return " Der Insel-Stern wurde bereits eingesammelt.";
        }

        int levelCount = GetWorldLevelCount(classLevel, subject);
        int fromStep = GetCurrentWorldStep(progress, classLevel, subject);
        progress.CompletedLevels = Math.Max(progress.CompletedLevels, fromStep);

        if (progress.CompletedLevels >= levelCount)
        {
            progress.CompletedLevels = levelCount;
            progress.StarCollected = true;
        }

        progress.UpdatedAt = DateTime.Now;
        int toStep = GetCurrentWorldStep(progress, classLevel, subject);
        _pendingWorldMoveFromStep = fromStep;
        _pendingWorldMoveToStep = toStep;
        _pendingWorldMoveClassLevel = classLevel;
        _pendingWorldMoveSubject = subject;

        string subjectName = GetSubjectDisplayName(subject);
        if (progress.StarCollected)
        {
            _worldStarCollectedThisRound = true;
            SavePlayerProfile();
            return $" Level {fromStep} geschafft. Lumos läuft jetzt zum Stern der {subjectName}-Insel. Der Stern wartet danach auf das Einsammeln.";
        }

        _worldStarCollectedThisRound = false;
        SavePlayerProfile();
        return $" Level {fromStep} geschafft. Level {toStep} ist freigeschaltet und Lumos läuft automatisch weiter.";
    }

    private void ClearPendingWorldMove()
    {
        _pendingWorldMoveFromStep = null;
        _pendingWorldMoveToStep = null;
        _pendingWorldMoveClassLevel = null;
        _pendingWorldMoveSubject = null;
        _animateWorldMoveOnNextMapOpen = false;
    }

    private static Point GetWorldPathPoint(int classLevel, LearningSubject subject, int step)
    {
        if (classLevel == 4)
        {
            return subject switch
            {
                LearningSubject.German => step switch
                {
                    1 => new Point(270, 430),
                    2 => new Point(330, 360),
                    3 => new Point(420, 330),
                    4 => new Point(500, 355),
                    5 => new Point(560, 420),
                    _ => new Point(610, 360)
                },
                LearningSubject.Math => step switch
                {
                    1 => new Point(1088, 445),
                    2 => new Point(1088, 515),
                    3 => new Point(1186, 444),
                    4 => new Point(1291, 515),
                    5 => new Point(1392, 442),
                    _ => new Point(1222, 292)
                },
                LearningSubject.Sachunterricht => step switch
                {
                    1 => new Point(978, 532),
                    2 => new Point(908, 468),
                    3 => new Point(815, 542),
                    4 => new Point(700, 482),
                    5 => new Point(760, 398),
                    _ => new Point(831, 334)
                },
                LearningSubject.Music => step switch
                {
                    1 => new Point(300, 730),
                    2 => new Point(380, 675),
                    3 => new Point(470, 640),
                    4 => new Point(555, 665),
                    5 => new Point(615, 735),
                    _ => new Point(665, 675)
                },
                LearningSubject.Art => step switch
                {
                    1 => new Point(720, 760),
                    2 => new Point(780, 700),
                    3 => new Point(860, 665),
                    4 => new Point(940, 685),
                    5 => new Point(1000, 755),
                    _ => new Point(1065, 700)
                },
                LearningSubject.English => step switch
                {
                    1 => new Point(1197, 695),
                    2 => new Point(1240, 778),
                    3 => new Point(1370, 693),
                    4 => new Point(1265, 667),
                    5 => new Point(1326, 612),
                    _ => new Point(1432, 611)
                },
                LearningSubject.Bonus => step switch
                {
                    1 => new Point(842, 235),
                    2 => new Point(875, 170),
                    3 => new Point(930, 105),
                    _ => new Point(1037, 215)
                },
                _ => new Point(1075, 350)
            };
        }

        if (classLevel == 3)
        {
            return subject switch
            {
                LearningSubject.German => step switch
                {
                    1 => new Point(452, 474),
                    2 => new Point(417, 412),
                    3 => new Point(474, 379),
                    4 => new Point(509, 316),
                    5 => new Point(604, 379),
                    _ => new Point(586, 458)
                },
                LearningSubject.Math => step switch
                {
                    1 => new Point(753, 459),
                    2 => new Point(752, 391),
                    3 => new Point(813, 358),
                    4 => new Point(913, 307),
                    5 => new Point(994, 385),
                    _ => new Point(919, 465)
                },
                LearningSubject.Sachunterricht => step switch
                {
                    1 => new Point(536, 644),
                    2 => new Point(538, 732),
                    3 => new Point(483, 783),
                    4 => new Point(365, 772),
                    5 => new Point(446, 720),
                    _ => new Point(585, 640)
                },
                LearningSubject.Logic => step switch
                {
                    1 => new Point(703, 790),
                    2 => new Point(705, 706),
                    3 => new Point(742, 666),
                    4 => new Point(824, 683),
                    5 => new Point(820, 787),
                    _ => new Point(882, 734)
                },
                LearningSubject.Music => step switch
                {
                    1 => new Point(1197, 695),
                    2 => new Point(1240, 778),
                    3 => new Point(1370, 693),
                    4 => new Point(1265, 667),
                    5 => new Point(1326, 612),
                    _ => new Point(1432, 611)
                },
                LearningSubject.Bonus => step switch
                {
                    1 => new Point(1109, 437),
                    2 => new Point(1124, 369),
                    3 => new Point(1196, 301),
                    _ => new Point(1211, 270)
                },
                _ => new Point(919, 465)
            };
        }

        if (classLevel == 2)
        {
            return subject switch
            {
                LearningSubject.German => step switch
                {
                    1 => new Point(113, 386),
                    2 => new Point(207, 389),
                    3 => new Point(280, 344),
                    4 => new Point(340, 293),
                    5 => new Point(456, 292),
                    _ => new Point(528, 344)
                },
                LearningSubject.Math => step switch
                {
                    1 => new Point(646, 394),
                    2 => new Point(746, 388),
                    3 => new Point(792, 407),
                    4 => new Point(887, 366),
                    5 => new Point(910, 306),
                    _ => new Point(968, 225)
                },
                LearningSubject.Sachunterricht => step switch
                {
                    1 => new Point(1091, 393),
                    2 => new Point(1160, 385),
                    3 => new Point(1217, 360),
                    4 => new Point(1269, 311),
                    5 => new Point(1361, 305),
                    _ => new Point(1420, 349)
                },
                LearningSubject.Logic => step switch
                {
                    1 => new Point(291, 742),
                    2 => new Point(404, 749),
                    3 => new Point(492, 704),
                    4 => new Point(564, 615),
                    5 => new Point(618, 593),
                    _ => new Point(533, 573)
                },
                LearningSubject.Music => step switch
                {
                    1 => new Point(765, 747),
                    2 => new Point(847, 777),
                    3 => new Point(946, 769),
                    4 => new Point(1061, 721),
                    5 => new Point(1110, 687),
                    _ => new Point(1137, 675)
                },
                _ => new Point(146, 778)
            };
        }

        return subject switch
        {
            LearningSubject.Math => step switch
            {
                1 => new Point(870, 360),
                2 => new Point(960, 407),
                3 => new Point(1084, 414),
                4 => new Point(1132, 398),
                5 => new Point(1174, 379),
                _ => new Point(1265, 405)
            },
            LearningSubject.German => step switch
            {
                1 => new Point(352, 432),
                2 => new Point(446, 390),
                3 => new Point(524, 330),
                4 => new Point(615, 371),
                5 => new Point(668, 420),
                _ => new Point(714, 352)
            },
            LearningSubject.Sachunterricht => step switch
            {
                1 => new Point(332, 724),
                2 => new Point(431, 735),
                3 => new Point(493, 703),
                4 => new Point(521, 648),
                5 => new Point(571, 668),
                _ => new Point(624, 681)
            },
            LearningSubject.Music => step switch
            {
                1 => new Point(860, 657),
                2 => new Point(942, 703),
                3 => new Point(1036, 737),
                4 => new Point(1119, 737),
                5 => new Point(1205, 684),
                _ => new Point(1335, 690)
            },
            _ => new Point(146, 778)
        };
    }

    private static void PositionMarker(Ellipse marker, Point centerPoint, double size)
    {
        Canvas.SetLeft(marker, centerPoint.X - (size / 2));
        Canvas.SetTop(marker, centerPoint.Y - (size / 2));
    }

    private static SolidColorBrush CreateBrushWithAlpha(byte alpha, byte red, byte green, byte blue)
    {
        return new SolidColorBrush(Color.FromArgb(alpha, red, green, blue));
    }

    private static void AnimateCanvasMove(UIElement element, double targetLeft, double targetTop)
    {
        Duration duration = new(TimeSpan.FromMilliseconds(850));
        IEasingFunction easing = new QuadraticEase { EasingMode = EasingMode.EaseInOut };

        element.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(targetLeft, duration)
        {
            EasingFunction = easing
        });
        element.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(targetTop, duration)
        {
            EasingFunction = easing
        });
    }

    private static void StopCanvasMoveAnimations(UIElement element)
    {
        element.BeginAnimation(Canvas.LeftProperty, null);
        element.BeginAnimation(Canvas.TopProperty, null);
    }

    private void SetImageSource(Image imageControl, string imageName)
    {
        string? imagePath = FindPreviewImage(imageName);
        imageControl.Source = imagePath is null ? null : CreateBitmap(imagePath);
    }

    private static BitmapImage CreateBitmap(string imageNameOrPath)
    {
        string imagePath = File.Exists(imageNameOrPath)
            ? imageNameOrPath
            : GetImagePath(imageNameOrPath);

        if (!File.Exists(imagePath))
        {
            throw new FileNotFoundException($"Die Bilddatei wurde nicht gefunden: {imagePath}", imagePath);
        }

        return new BitmapImage(new Uri(imagePath, UriKind.Absolute));
    }

    private static SolidColorBrush CreateBrush(byte red, byte green, byte blue)
    {
        return new SolidColorBrush(Color.FromRgb(red, green, blue));
    }

    private void ApplyFullscreenMode(bool isFullscreen)
    {
        if (isFullscreen)
        {
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
            return;
        }

        WindowStyle = WindowStyle.SingleBorderWindow;
        WindowState = WindowState.Normal;
    }

    private static string GetImagePath(string imageName)
    {
        return Path.Combine(AppContext.BaseDirectory, "Bilder", imageName);
    }

    private static int ParseClassLevel(string className)
    {
        string digits = new string(className.Where(char.IsDigit).ToArray());
        return int.TryParse(digits, out int classLevel) ? classLevel : 1;
    }

    private static LearningSubject GetLearningSubject(string subjectName)
    {
        return subjectName switch
        {
            "Mathematik" => LearningSubject.Math,
            "Mathe" => LearningSubject.Math,
            "Sprache" => LearningSubject.German,
            "Deutsch" => LearningSubject.German,
            "Englisch" => LearningSubject.English,
            "Kunst" => LearningSubject.Art,
            "Logik" => LearningSubject.Logic,
            "Sachkunde" => LearningSubject.Sachunterricht,
            "Musik" => LearningSubject.Music,
            "Bonus" => LearningSubject.Bonus,
            "Medien" => LearningSubject.Media,
            "Kreativität" => LearningSubject.Creativity,
            _ => throw new NotSupportedException($"Das Fach {subjectName} ist nicht bekannt.")
        };
    }

    private static string GetSubjectDisplayName(LearningSubject subject)
    {
        return subject switch
        {
            LearningSubject.Math => "Mathematik",
            LearningSubject.German => "Deutsch",
            LearningSubject.English => "Englisch",
            LearningSubject.Art => "Kunst",
            LearningSubject.Logic => "Logik",
            LearningSubject.Sachunterricht => "Sachkunde",
            LearningSubject.Music => "Musik",
            LearningSubject.Bonus => "Bonus",
            LearningSubject.Media => "Medien",
            LearningSubject.Creativity => "Kreativität",
            _ => subject.ToString()
        };
    }

    private static string GetWorldSubjectDisplayName(int classLevel, LearningSubject subject)
    {
        if (classLevel == 4 && subject == LearningSubject.Bonus)
        {
            return "Meister";
        }

        return GetSubjectDisplayName(subject);
    }

    private static string GetSubjectPreviewName(int classLevel, LearningSubject subject)
    {
        if (subject == LearningSubject.Bonus)
        {
            return classLevel == 4 ? "KLasse 4" : "Klasse 3";
        }

        return subject switch
        {
            LearningSubject.Creativity => "Musik",
            LearningSubject.Art => "Kunst",
            LearningSubject.English => "Englisch",
            _ => GetSubjectDisplayName(subject)
        };
    }

    private static string GetSubjectIntroText(LearningSubject subject, RewardTrack rewardTrack, int classLevel)
    {
        string rewardText = rewardTrack == RewardTrack.PortalTrophies
            ? "Hier sammelst du pro richtiger Aufgabe Pokale und holst sie nach der Runde ab."
            : $"Hier spielst du ein Map-Level mit {WorldQuestionsPerLevel} Aufgaben. Sterne gibt es erst am Insel-Stern.";

        if (classLevel == 2)
        {
            return subject switch
            {
                LearningSubject.Math => $"Hier übst du in Klasse 2 Rechnen bis 100, Zehner und Einer und Zahlenreihen. {rewardText}",
                LearningSubject.German => $"Hier übst du in Klasse 2 Wörter, Wortarten, Silben, Reime und kleine Satzbausteine. {rewardText}",
                LearningSubject.Logic => $"Hier trainierst du in Klasse 2 Reihen, Muster, Kategorien und logische Abläufe. {rewardText}",
                LearningSubject.Sachunterricht => $"Hier entdeckst du in Klasse 2 Verkehr, Pflanzen, Tiere, Wetter und Alltag. {rewardText}",
                LearningSubject.Music => $"Hier übst du in Klasse 2 Rhythmus, Instrumente, Liedaufbau und musikalisches Hören. {rewardText}",
                _ => $"Hier lernst du direkt im Hauptfenster. {rewardText}"
            };
        }

        if (classLevel == 3)
        {
            return subject switch
            {
                LearningSubject.German => $"Hier übst du in Klasse 3 Texte lesen, Grammatik und Sätze bilden. {rewardText}",
                LearningSubject.Math => $"Hier übst du in Klasse 3 Multiplikation, Division und erste Sachaufgaben. {rewardText}",
                LearningSubject.Sachunterricht => $"Hier entdeckst du in Klasse 3 Natur, Technik und Umwelt. {rewardText}",
                LearningSubject.Logic => $"Hier übst du in Klasse 3 Muster, Regeln, Reihen und logische Denkwege. {rewardText}",
                LearningSubject.Music => $"Hier übst du in Klasse 3 Rhythmus, Instrumente, Melodien und gemeinsames Musizieren. {rewardText}",
                LearningSubject.Bonus => $"Hier spielst du in Klasse 3 Bonus-Level mit gemischten Fragen aus Deutsch, Mathematik, Sachkunde, Logik und Musik. {rewardText}",
                _ => $"Hier lernst du direkt im Hauptfenster. {rewardText}"
            };
        }

        if (classLevel == 4)
        {
            return subject switch
            {
                LearningSubject.German => $"Hier übst du in Klasse 4 Rechtschreibung, Zeitformen, wörtliche Rede und Textverständnis. {rewardText}",
                LearningSubject.Math => $"Hier übst du in Klasse 4 Rechnen bis 1000, Bruchteile, Umfang und anspruchsvollere Sachaufgaben. {rewardText}",
                LearningSubject.Sachunterricht => $"Hier entdeckst du in Klasse 4 Umwelt, Energie, Sicherheit, Orientierung und Zusammenleben. {rewardText}",
                LearningSubject.Music => $"Hier übst du in Klasse 4 Instrumentenkunde, Dynamik, Tempo, Rhythmus und gemeinsames Musizieren. Auf der Karte nutzt dieses Fach die Medien-Insel. {rewardText}",
                LearningSubject.Art => $"Hier gestaltest du in Klasse 4 mit Farben, Materialien, Bildaufbau und Präsentation. Auf der Karte nutzt dieses Fach die Kreativitäts-Insel. {rewardText}",
                LearningSubject.English => $"Hier übst du in Klasse 4 Wortschatz, einfache Sätze, Lesen und Sprechen auf Englisch. Auf der Karte nutzt dieses Fach die Projekt-Insel. {rewardText}",
                LearningSubject.Bonus => $"Hier spielst du in Klasse 4 Meister-Level mit gemischten Fragen aus Deutsch, Mathematik, Sachkunde, Musik, Kunst und Englisch. {rewardText}",
                _ => $"Hier lernst du direkt im Hauptfenster. {rewardText}"
            };
        }

        return subject switch
        {
            LearningSubject.Math => $"Hier übst du Zahlen, Rechnen und Vergleichen direkt im Hauptfenster. {rewardText}",
            LearningSubject.German => $"Hier übst du Buchstaben, Reime, Silben und erste Wörter direkt im Hauptfenster. {rewardText}",
            LearningSubject.Logic => $"Hier trainierst du Muster, Reihen und logisches Denken direkt im Hauptfenster. {rewardText}",
            LearningSubject.Sachunterricht => $"Hier entdeckst du Tiere, Jahreszeiten, Gesundheit und Tag und Nacht direkt im Hauptfenster. {rewardText}",
            LearningSubject.Music => $"Hier übst du Instrumente, Rhythmus und die Wirkung von Musik direkt im Hauptfenster. {rewardText}",
            _ => $"Hier lernst du direkt im Hauptfenster. {rewardText}"
        };
    }

    private static string GetRewardPluralLabel(RewardTrack rewardTrack)
    {
        return rewardTrack == RewardTrack.PortalTrophies ? "Pokale" : "Sterne";
    }

    private static int GetQuestionCountForTrack(RewardTrack rewardTrack)
    {
        return rewardTrack == RewardTrack.WorldStars ? WorldQuestionsPerLevel : PortalQuestionsPerRound;
    }

    private static void WriteMenuErrorLog(string source, Exception exception, string context)
    {
        var logBuilder = new StringBuilder();
        logBuilder.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {source}");
        logBuilder.AppendLine(context);
        logBuilder.AppendLine(exception.ToString());
        logBuilder.AppendLine(new string('-', 60));
        AppStoragePaths.AppendErrorLog(logBuilder.ToString());
    }

    private void SavePlayerProfile()
    {
        NormalizeRewardTotals();
        _playerProfile.LastPlayedAt = DateTime.Now;
        _playerProfileStore.SavePlayer(_playerProfile);
    }
}
