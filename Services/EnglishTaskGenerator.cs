using System;
using System.Collections.Generic;
using System.Linq;
using SpieleLernApp.Models;

namespace SpieleLernApp.Services;

public sealed class EnglishTaskGenerator : ILearningTaskGenerator
{
    public LearningSubject Subject => LearningSubject.English;

    public IReadOnlyList<LearningTask> GenerateTasks(TaskGenerationRequest request)
    {
        if (request.ClassLevel != 4)
        {
            throw new NotSupportedException("Der Englisch-Generator ist aktuell für Klasse 4 aufgebaut.");
        }

        Random random = request.Seed.HasValue
            ? new Random(request.Seed.Value)
            : new Random();

        List<LearningTask> pool = BuildPortalTasks()
            .Concat(BuildWorldTasks())
            .GroupBy(task => task.Prompt, StringComparer.Ordinal)
            .Select(group => group.First())
            .ToList();

        return pool
            .OrderBy(_ => random.Next())
            .Take(request.TaskCount)
            .Select((task, index) => CloneWithNumber(task, index + 1, random))
            .ToList();
    }

    private static List<LearningTask> BuildPortalTasks()
    {
        return
        [
            CreateChoiceTask("Was heißt dog auf Deutsch?", "Hund", ["Katze", "Haus", "Ball"], "Wortschatz", "Richtig. Dog heißt Hund.", 4),
            CreateChoiceTask("Was heißt school auf Deutsch?", "Schule", ["Küche", "Straße", "Blume"], "Wortschatz", "Prima. School heißt Schule.", 4),
            CreateChoiceTask("Welches Wort passt zu blue?", "blau", ["grün", "rot", "gelb"], "Farben", "Richtig. Blue bedeutet blau.", 4),
            CreateChoiceTask("Wie heißt Monday auf Deutsch?", "Montag", ["Mittwoch", "Freitag", "Sonntag"], "Tage", "Prima. Monday ist Montag.", 4),
            CreateChoiceTask("Welches Wort bedeutet Buch?", "book", ["bag", "chair", "window"], "Wortschatz", "Richtig. Book heißt Buch.", 4),
            CreateChoiceTask("Was heißt I am ten auf Deutsch?", "Ich bin zehn", ["Ich habe zehn", "Ich laufe zehn", "Ich male zehn"], "Sätze verstehen", "Super. I am ten bedeutet Ich bin zehn.", 4),
            CreateChoiceTask("Welches Wort passt zu apple?", "Apfel", ["Birne", "Banane", "Traube"], "Wortschatz", "Richtig. Apple heißt Apfel.", 4),
            CreateChoiceTask("Wie fragst du auf Englisch nach dem Namen?", "What is your name?", ["Where is your shoe?", "How old is red?", "When is the chair?"], "Sprechen", "Prima. So fragst du nach dem Namen.", 4),
            CreateChoiceTask("Welches Wort bedeutet Freund?", "friend", ["bread", "green", "river"], "Wortschatz", "Richtig. Friend heißt Freund.", 4),
            CreateChoiceTask("Was heißt My bag is red auf Deutsch?", "Meine Tasche ist rot", ["Mein Buch ist rot", "Meine Hose ist gro?", "Mein Stuhl ist neu"], "Sätze verstehen", "Super. So wird der Satz übersetzt.", 4),
            CreateChoiceTask("Welches Wort gehört zur Schule?", "pencil", ["banana", "rabbit", "cloud"], "Schule", "Richtig. Pencil gehört zur Schule.", 4),
            CreateChoiceTask("Was bedeutet hello?", "Hallo", ["Tschüss", "Danke", "Bitte"], "Sprechen", "Prima. Hello heißt Hallo.", 4)
        ];
    }

    private static List<LearningTask> BuildWorldTasks()
    {
        return
        [
            CreateChoiceTask("Welches Wort bedeutet Fenster?", "window", ["table", "garden", "milk"], "Wortschatz", "Richtig. Window heißt Fenster.", 4),
            CreateChoiceTask("Was heißt cat auf Deutsch?", "Katze", ["Hund", "Maus", "Pferd"], "Wortschatz", "Prima. Cat heißt Katze.", 4),
            CreateChoiceTask("Wie heißt Friday auf Deutsch?", "Freitag", ["Dienstag", "Samstag", "Montag"], "Tage", "Richtig. Friday ist Freitag.", 4),
            CreateChoiceTask("Welcher Satz passt zu einem Kind in der Schule?", "I like English.", ["The chair sings.", "Blue is seven.", "My window runs."], "Sätze", "Super. Das ist ein sinnvoller englischer Satz.", 4),
            CreateChoiceTask("Was bedeutet yellow?", "gelb", ["weiß", "braun", "pink"], "Farben", "Richtig. Yellow bedeutet gelb.", 4),
            CreateChoiceTask("Welches Wort bedeutet Wasser?", "water", ["winter", "writer", "window"], "Wortschatz", "Prima. Water heißt Wasser.", 4),
            CreateChoiceTask("Wie sagst du auf Englisch Ich habe ein Buch?", "I have a book.", ["I am a book.", "I like a chair.", "I run a school."], "Sätze bilden", "Richtig. So bildet man den Satz.", 4),
            CreateChoiceTask("Welches Wort passt zu family?", "Familie", ["Ferien", "Fenster", "Feuer"], "Wortschatz", "Prima. Family heißt Familie.", 4),
            CreateChoiceTask("Was heißt teacher auf Deutsch?", "Lehrerin oder Lehrer", ["Tafel", "Tasche", "Treppe"], "Schule", "Richtig. Teacher bedeutet Lehrerin oder Lehrer.", 4),
            CreateChoiceTask("Welcher Gru? passt zum Morgen?", "Good morning", ["Good banana", "Blue morning", "Morning chair"], "Sprechen", "Super. Good morning ist der passende Gru?.", 4),
            CreateChoiceTask("Was bedeutet We play together?", "Wir spielen zusammen", ["Wir schreiben heute", "Wir schlafen leise", "Wir trinken schnell"], "Sätze verstehen", "Richtig. So wird der Satz übersetzt.", 4),
            CreateChoiceTask("Welches Wort heißt Haus?", "house", ["horse", "mouse", "flower"], "Wortschatz", "Prima. House heißt Haus.", 4)
        ];
    }

    private static LearningTask CloneWithNumber(LearningTask task, int number, Random random)
    {
        string correctAnswer = task.Answers[task.CorrectAnswerIndex];
        string[] answers = task.Answers.OrderBy(_ => random.Next()).ToArray();

        return new LearningTask
        {
            Title = $"Aufgabe {number}",
            Prompt = task.Prompt,
            Answers = answers,
            CorrectAnswerIndex = Array.IndexOf(answers, correctAnswer),
            SuccessText = task.SuccessText,
            Topic = task.Topic,
            Subject = task.Subject,
            ClassLevel = task.ClassLevel
        };
    }

    private static LearningTask CreateChoiceTask(string prompt, string correctAnswer, string[] wrongAnswers, string topic, string successText, int classLevel)
    {
        List<string> answers = [correctAnswer, .. wrongAnswers];

        return new LearningTask
        {
            Title = "Aufgabe",
            Prompt = prompt,
            Answers = answers.ToArray(),
            CorrectAnswerIndex = 0,
            SuccessText = successText,
            Topic = topic,
            Subject = LearningSubject.English,
            ClassLevel = classLevel
        };
    }
}
