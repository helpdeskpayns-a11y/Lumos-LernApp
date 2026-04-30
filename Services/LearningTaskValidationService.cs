using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SpieleLernApp.Models;

namespace SpieleLernApp.Services;

public static class LearningTaskValidationService
{
    private static readonly HashSet<string> LoggedMessages = [];

    public static IReadOnlyList<LearningTask> FilterValidTasks(IEnumerable<LearningTask> tasks)
    {
        List<LearningTask> validTasks = [];

        foreach (LearningTask task in tasks)
        {
            if (TryValidateTask(task, out string? errorMessage))
            {
                validTasks.Add(task);
                continue;
            }

            LogValidationError(task, errorMessage ?? "Unbekannter Validierungsfehler.");
        }

        return validTasks;
    }

    private static bool TryValidateTask(LearningTask task, out string? errorMessage)
    {
        string correctAnswer = GetCorrectAnswer(task);

        if (TryMatch(task.Prompt, @"^Wie viele Buchstaben hat (?<word>.+)\?$", out Match letterMatch))
        {
            string word = letterMatch.Groups["word"].Value.Trim();
            int actualCount = word.Count(char.IsLetter);
            return ValidateExpectedNumber(correctAnswer, actualCount, task.Prompt, out errorMessage);
        }

        if (TryMatch(task.Prompt, @"^Wie viele Silben hat (?<word>.+)\?$", out Match syllableMatch))
        {
            string word = syllableMatch.Groups["word"].Value.Trim();
            int actualCount = CountSyllables(word);
            return ValidateExpectedNumber(correctAnswer, actualCount, task.Prompt, out errorMessage);
        }

        if (TryMatch(task.Prompt, @"^Welches Wort hat (?<count>\d+) Silben\?$", out Match wordSyllableMatch))
        {
            int expectedCount = int.Parse(wordSyllableMatch.Groups["count"].Value, CultureInfo.InvariantCulture);
            int actualCount = CountSyllables(correctAnswer);
            return ValidateExpectedNumber(actualCount, expectedCount, task.Prompt, out errorMessage);
        }

        if (TryMatch(task.Prompt, @"^Mit welchem Buchstaben beginnt (?<word>.+)\?$", out Match startMatch))
        {
            char? actualLetter = GetFirstLetter(startMatch.Groups["word"].Value);
            return ValidateExpectedLetter(correctAnswer, actualLetter, task.Prompt, out errorMessage);
        }

        if (TryMatch(task.Prompt, @"^Mit welchem Buchstaben endet (?<word>.+)\?$", out Match endMatch))
        {
            char? actualLetter = GetLastLetter(endMatch.Groups["word"].Value);
            return ValidateExpectedLetter(correctAnswer, actualLetter, task.Prompt, out errorMessage);
        }

        if (TryMatch(task.Prompt, @"^Welche Zahl kommt nach (?<value>\d+)\?$", out Match nextMatch))
        {
            int expected = int.Parse(nextMatch.Groups["value"].Value, CultureInfo.InvariantCulture) + 1;
            return ValidateExpectedNumber(correctAnswer, expected, task.Prompt, out errorMessage);
        }

        if (TryMatch(task.Prompt, @"^Wie viel ist (?<left>\d+)\s*(?<op>[+-])\s*(?<right>\d+)\?$", out Match arithmeticMatch))
        {
            int left = int.Parse(arithmeticMatch.Groups["left"].Value, CultureInfo.InvariantCulture);
            int right = int.Parse(arithmeticMatch.Groups["right"].Value, CultureInfo.InvariantCulture);
            int expected = arithmeticMatch.Groups["op"].Value == "+" ? left + right : left - right;
            return ValidateExpectedNumber(correctAnswer, expected, task.Prompt, out errorMessage);
        }

        if (TryMatch(task.Prompt, @"^Welche Zahl liegt zwischen (?<left>\d+) und (?<right>\d+)\?$", out Match betweenMatch))
        {
            int left = int.Parse(betweenMatch.Groups["left"].Value, CultureInfo.InvariantCulture);
            int right = int.Parse(betweenMatch.Groups["right"].Value, CultureInfo.InvariantCulture);
            int expected = left + 1;
            return ValidateExpectedNumber(correctAnswer, expected, task.Prompt, out errorMessage);
        }

        if (TryMatch(task.Prompt, @"^Welche Zahl fehlt\?\s*(?<sequence>.+)$", out Match missingNumberMatch))
        {
            string[] parts = missingNumberMatch.Groups["sequence"].Value
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            List<int> knownNumbers = parts
                .Where(part => !part.Contains('_'))
                .Select(part => int.Parse(part, CultureInfo.InvariantCulture))
                .ToList();

            if (knownNumbers.Count >= 2)
            {
                int expected = FindMissingNumber(parts, knownNumbers);
                return ValidateExpectedNumber(correctAnswer, expected, task.Prompt, out errorMessage);
            }
        }

        if (TryMatch(task.Prompt, @"^Welche Rechnung passt zu (?<target>\d+)\?$", out Match equationMatch))
        {
            int target = int.Parse(equationMatch.Groups["target"].Value, CultureInfo.InvariantCulture);
            if (TryEvaluateExpression(correctAnswer, out int result))
            {
                return ValidateExpectedNumber(result, target, task.Prompt, out errorMessage);
            }
        }

        if (TryMatch(task.Prompt, @"^Wo sind es am meisten\?\s*(?<left>\d+).+ oder (?<right>\d+).+\?$", out Match mostMatch))
        {
            int left = int.Parse(mostMatch.Groups["left"].Value, CultureInfo.InvariantCulture);
            int right = int.Parse(mostMatch.Groups["right"].Value, CultureInfo.InvariantCulture);
            int expected = Math.Max(left, right);
            return ValidateAnswerContainsNumber(correctAnswer, expected, task.Prompt, out errorMessage);
        }

        if (TryMatch(task.Prompt, @"^Wo sind es weniger\?\s*(?<left>\d+).+ oder (?<right>\d+).+\?$", out Match lessMatch))
        {
            int left = int.Parse(lessMatch.Groups["left"].Value, CultureInfo.InvariantCulture);
            int right = int.Parse(lessMatch.Groups["right"].Value, CultureInfo.InvariantCulture);
            int expected = Math.Min(left, right);
            return ValidateAnswerContainsNumber(correctAnswer, expected, task.Prompt, out errorMessage);
        }

        errorMessage = null;
        return true;
    }

    private static int FindMissingNumber(string[] parts, List<int> knownNumbers)
    {
        int step = knownNumbers.Count >= 2 ? knownNumbers[1] - knownNumbers[0] : 1;
        for (int index = 0; index < parts.Length; index++)
        {
            if (!parts[index].Contains('_'))
            {
                continue;
            }

            return knownNumbers[0] + (step * index);
        }

        return knownNumbers[0];
    }

    private static bool TryEvaluateExpression(string expression, out int result)
    {
        Match match = Regex.Match(expression.Trim(), @"^(?<left>\d+)\s*(?<op>[+-])\s*(?<right>\d+)$");
        if (!match.Success)
        {
            result = 0;
            return false;
        }

        int left = int.Parse(match.Groups["left"].Value, CultureInfo.InvariantCulture);
        int right = int.Parse(match.Groups["right"].Value, CultureInfo.InvariantCulture);
        result = match.Groups["op"].Value == "+" ? left + right : left - right;
        return true;
    }

    private static int CountSyllables(string word)
    {
        MatchCollection matches = Regex.Matches(word, "[aeiouyäöüAEIOUYÄÖÜ]+", RegexOptions.CultureInvariant);
        return Math.Max(1, matches.Count);
    }

    private static bool ValidateExpectedNumber(string actualText, int expected, string prompt, out string? errorMessage)
    {
        if (int.TryParse(actualText, NumberStyles.Integer, CultureInfo.InvariantCulture, out int actual) && actual == expected)
        {
            errorMessage = null;
            return true;
        }

        errorMessage = $"Falsche Zahl in Aufgabe \"{prompt}\". Erwartet: {expected}, hinterlegt: {actualText}.";
        return false;
    }

    private static bool ValidateExpectedNumber(int actual, int expected, string prompt, out string? errorMessage)
    {
        if (actual == expected)
        {
            errorMessage = null;
            return true;
        }

        errorMessage = $"Falsche Zahl in Aufgabe \"{prompt}\". Erwartet: {expected}, berechnet: {actual}.";
        return false;
    }

    private static bool ValidateAnswerContainsNumber(string answer, int expected, string prompt, out string? errorMessage)
    {
        Match match = Regex.Match(answer, @"\d+");
        if (match.Success &&
            int.TryParse(match.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int actual) &&
            actual == expected)
        {
            errorMessage = null;
            return true;
        }

        errorMessage = $"Falsche Mengenantwort in Aufgabe \"{prompt}\". Erwartet Zahl {expected}, hinterlegt: {answer}.";
        return false;
    }

    private static bool ValidateExpectedLetter(string actualText, char? expected, string prompt, out string? errorMessage)
    {
        char? actual = GetFirstLetter(actualText);
        if (actual is not null && expected is not null && char.ToUpperInvariant(actual.Value) == char.ToUpperInvariant(expected.Value))
        {
            errorMessage = null;
            return true;
        }

        errorMessage = $"Falscher Buchstabe in Aufgabe \"{prompt}\". Erwartet: {expected}, hinterlegt: {actualText}.";
        return false;
    }

    private static char? GetFirstLetter(string text)
    {
        return text.FirstOrDefault(char.IsLetter);
    }

    private static char? GetLastLetter(string text)
    {
        return text.LastOrDefault(char.IsLetter);
    }

    private static string GetCorrectAnswer(LearningTask task)
    {
        if (task.CorrectAnswerIndex < 0 || task.CorrectAnswerIndex >= task.Answers.Length)
        {
            return string.Empty;
        }

        return task.Answers[task.CorrectAnswerIndex];
    }

    private static bool TryMatch(string input, string pattern, out Match match)
    {
        match = Regex.Match(input, pattern, RegexOptions.CultureInvariant);
        return match.Success;
    }

    private static void LogValidationError(LearningTask task, string message)
    {
        string key = $"{task.Subject}|{task.ClassLevel}|{task.Prompt}|{message}";
        if (!LoggedMessages.Add(key))
        {
            return;
        }

        var logBuilder = new StringBuilder();
        logBuilder.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] TaskValidation");
        logBuilder.AppendLine($"{task.Subject} | Klasse {task.ClassLevel} | {task.Prompt}");
        logBuilder.AppendLine(message);
        logBuilder.AppendLine(new string('-', 60));
        AppStoragePaths.AppendErrorLog(logBuilder.ToString());
    }
}
