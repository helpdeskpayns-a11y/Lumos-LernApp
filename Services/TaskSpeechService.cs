using System;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Security;
using System.Text;
using SpieleLernApp.Models;

namespace SpieleLernApp.Services;

public sealed class TaskSpeechService : IDisposable
{
    private readonly SpeechSynthesizer _speechSynthesizer = new();
    private string? _selectedVoiceName;

    public TaskSpeechService()
    {
        _speechSynthesizer.SetOutputToDefaultAudioDevice();
        _speechSynthesizer.Volume = 95;
        _speechSynthesizer.Rate = -3;
        ConfigureVoice();
    }

    public void SpeakTask(LearningTask task, int taskNumber, int totalTasks)
    {
        string ssml = BuildWarmNarratorSsml(task);
        SpeakSsml(ssml);
    }

    public void Stop()
    {
        _speechSynthesizer.SpeakAsyncCancelAll();
    }

    public void Dispose()
    {
        Stop();
        _speechSynthesizer.Dispose();
    }

    private void SpeakSsml(string ssml)
    {
        Stop();
        if (string.IsNullOrWhiteSpace(ssml))
        {
            return;
        }

        _speechSynthesizer.SpeakSsmlAsync(ssml);
    }

    private void ConfigureVoice()
    {
        InstalledVoice? preferredVoice = _speechSynthesizer.GetInstalledVoices()
            .Where(voice => voice.Enabled)
            .OrderBy(voice => GetVoicePriority(voice.VoiceInfo))
            .FirstOrDefault();

        if (preferredVoice is null)
        {
            try
            {
                _speechSynthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult, 0, new CultureInfo("de-DE"));
            }
            catch
            {
                // Fallback bleibt auf der Standardstimme.
            }

            return;
        }

        _selectedVoiceName = preferredVoice.VoiceInfo.Name;
        _speechSynthesizer.SelectVoice(_selectedVoiceName);
    }

    private string BuildWarmNarratorSsml(LearningTask task)
    {
        string prompt = EscapeForSsml(task.Prompt);
        string[] answers = task.Answers
            .Select(answer => EscapeForSsml(answer))
            .ToArray();

        var builder = new StringBuilder();
        builder.AppendLine("""<speak version="1.0" xml:lang="de-DE">""");

        if (!string.IsNullOrWhiteSpace(_selectedVoiceName))
        {
            builder.AppendLine($"""<voice name="{EscapeForSsml(_selectedVoiceName)}">""");
        }

        builder.AppendLine("""<prosody rate="-12%" pitch="-2%">""");
        builder.AppendLine($"""<p><s>{prompt}</s></p>""");
        builder.AppendLine("""<break time="450ms"/>""");

        for (int index = 0; index < answers.Length; index++)
        {
            builder.AppendLine($"""<p><s>Antwort {index + 1}. {answers[index]}.</s></p>""");
            if (index < answers.Length - 1)
            {
                builder.AppendLine("""<break time="250ms"/>""");
            }
        }

        builder.AppendLine("""</prosody>""");

        if (!string.IsNullOrWhiteSpace(_selectedVoiceName))
        {
            builder.AppendLine("""</voice>""");
        }

        builder.AppendLine("""</speak>""");
        return builder.ToString();
    }

    private static string EscapeForSsml(string text)
    {
        return SecurityElement.Escape(text) ?? string.Empty;
    }

    private static int GetVoicePriority(VoiceInfo voiceInfo)
    {
        string name = voiceInfo.Name;
        bool german = voiceInfo.Culture.Name.StartsWith("de", StringComparison.OrdinalIgnoreCase);
        bool female = voiceInfo.Gender == VoiceGender.Female;
        bool likelyWarmFemale = name.Contains("Hedda", StringComparison.OrdinalIgnoreCase) ||
                                name.Contains("Katja", StringComparison.OrdinalIgnoreCase) ||
                                name.Contains("Steffi", StringComparison.OrdinalIgnoreCase);

        if (german && female && likelyWarmFemale)
        {
            return 0;
        }

        if (german && female)
        {
            return 1;
        }

        if (german)
        {
            return 2;
        }

        if (female)
        {
            return 3;
        }

        return 4;
    }
}
