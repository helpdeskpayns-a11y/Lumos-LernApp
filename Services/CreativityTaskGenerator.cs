using System;
using System.Collections.Generic;
using System.Linq;
using SpieleLernApp.Models;

namespace SpieleLernApp.Services;

public sealed class CreativityTaskGenerator : ILearningTaskGenerator
{
    public LearningSubject Subject => LearningSubject.Creativity;

    public IReadOnlyList<LearningTask> GenerateTasks(TaskGenerationRequest request)
    {
        if (request.ClassLevel != 3)
        {
            throw new NotSupportedException("Der Kreativitäts-Generator ist aktuell für Klasse 3 aufgebaut.");
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
            CreateChoiceTask("Welche Farbe entsteht aus Blau und Gelb?", "Grün", ["Rot", "Lila", "Schwarz"], "Farben", "Richtig. Blau und Gelb ergeben Grün.", "Grün"),
            CreateChoiceTask("Welche Farbe entsteht aus Rot und Blau?", "Lila", ["Orange", "Grün", "Braun"], "Farben", "Richtig. Rot und Blau ergeben Lila.", "Lila"),
            CreateChoiceTask("Was brauchst du oft zum Zeichnen?", "Buntstifte", ["Schneeschaufel", "Fahrradklingel", "Topflappen"], "Gestalten", "Prima. Buntstifte helfen beim Gestalten.", "Buntstifte"),
            CreateChoiceTask("Was gehört oft zu einem Lied?", "eine Melodie", ["ein Türschloss", "eine Schaufel", "eine Klammer"], "Musik", "Richtig. Eine Melodie gehört zu einem Lied.", "eine Melodie"),
            CreateChoiceTask("Wie wirkt Musik oft, wenn sie ruhig und leise ist?", "sanft", ["hektisch", "kantig", "laut"], "Musik fühlen", "Super. Ruhige Musik wirkt oft sanft.", "sanft"),
            CreateChoiceTask("Was ist beim Basteln wichtig?", "sorgfältig arbeiten", ["alles zerreißen", "niemals hinsehen", "nur rennen"], "Gestalten", "Richtig. Sorgfältiges Arbeiten hilft beim Basteln.", "sorgfältig arbeiten"),
            CreateChoiceTask("Womit kannst du kleben?", "mit Klebstoff", ["mit Sand", "mit Wasser allein", "mit Luft"], "Gestalten", "Richtig. Klebstoff hilft beim Basteln.", "mit Klebstoff"),
            CreateChoiceTask("Wie oft kannst du bei Regenbogen klatschen?", "3", ["2", "4", "5"], "Rhythmus", "Klasse. Du hast den Rhythmus richtig gezählt.", "3"),
            CreateChoiceTask("Wie oft kannst du bei Fantasie klatschen?", "3", ["2", "4", "5"], "Rhythmus", "Klasse. Du hast den Rhythmus richtig gezählt.", "3"),
            CreateChoiceTask("Was gehört zu einer Bühne?", "Publikum", ["Schneesturm", "Wasserhahn", "Kochtopf"], "Darstellen", "Prima. Auf einer Bühne gibt es oft ein Publikum.", "Publikum"),
            CreateChoiceTask("Welches Material ist gut zum Modellieren?", "Knete", ["Papierkorb", "Zahnpasta", "Radiergummi"], "Gestalten", "Richtig. Knete eignet sich gut zum Formen.", "Knete"),
            CreateChoiceTask("Was hilft dir, eine Idee kreativ umzusetzen?", "Fantasie", ["Langeweile", "Stille im Schrank", "Angst vor Farben"], "Kreativität", "Super. Fantasie hilft beim Gestalten.", "Fantasie")
        ];
    }

    private static List<LearningTask> BuildWorldTasks()
    {
        return
        [
            CreateChoiceTask("Welche Farbe passt oft zu Gras?", "Grün", ["Blau", "Lila", "Silber"], "Farben", "Richtig. Gras ist oft grün.", "Grün"),
            CreateChoiceTask("Welche Farbe passt oft zur Sonne?", "Gelb", ["Schwarz", "Grau", "Braun"], "Farben", "Richtig. Die Sonne wird oft gelb dargestellt.", "Gelb"),
            CreateChoiceTask("Was ist bei einem gemeinsamen Lied wichtig?", "zusammen anfangen", ["jeder zufällig", "gar nicht zuhören", "dauernd dazwischenrufen"], "Musik", "Prima. Gemeinsamer Einsatz ist wichtig.", "zusammen anfangen"),
            CreateChoiceTask("Was kannst du zu Musik mit dem Körper machen?", "tanzen", ["löschen", "abwiegen", "schweigen"], "Darstellen", "Richtig. Tanzen passt gut zu Musik.", "tanzen"),
            CreateChoiceTask("Wie klingt eine Triangel oft?", "hell", ["dumpf", "stumm", "schwer"], "Musik", "Richtig. Eine Triangel klingt oft hell.", "hell"),
            CreateChoiceTask("Was hilft beim Malen genauer Linien?", "ruhige Hand", ["geschlossene Augen", "nasse Schuhe", "wildes Schütteln"], "Gestalten", "Prima. Eine ruhige Hand hilft beim Malen.", "ruhige Hand"),
            CreateChoiceTask("Welches Material ist weich und bunt zum Basteln?", "Filz", ["Stein", "Besteck", "Fensterglas"], "Materialien", "Richtig. Filz eignet sich gut zum Basteln.", "Filz"),
            CreateChoiceTask("Was gehört oft zu einem Theaterstück?", "Rollen", ["Autoreifen", "Schneeflocken im Zimmer", "Wasserkocher"], "Darstellen", "Prima. In einem Theaterstück gibt es Rollen.", "Rollen"),
            CreateChoiceTask("Was ist eine gute Idee für ein Kunstprojekt?", "verschiedene Materialien ausprobieren", ["immer alles gleich malen", "nichts anfassen", "nur den Tisch anstarren"], "Kreativität", "Super. Ausprobieren gehört zur Kreativität.", "verschiedene Materialien ausprobieren"),
            CreateChoiceTask("Welche Musik lädt oft zum Mitschwingen ein?", "rhythmische Musik", ["stumme Musik", "kaputte Musik", "eingeschlafene Musik"], "Musik fühlen", "Richtig. Rhythmische Musik lädt oft zum Mitschwingen ein.", "rhythmische Musik"),
            CreateChoiceTask("Wofür ist eine Skizze gut?", "für eine erste Idee", ["zum Schlafen", "zum Löschen", "zum Wiegen"], "Gestalten", "Richtig. Eine Skizze ist ein erster Entwurf.", "für eine erste Idee"),
            CreateChoiceTask("Was macht Kunst oft besonders?", "eigene Ideen", ["nur Kopieren", "gar nichts ausprobieren", "alles immer gleich"], "Kreativität", "Prima. Eigene Ideen machen Kunst spannend.", "eigene Ideen")
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

    private static LearningTask CreateChoiceTask(string prompt, string correctAnswer, string[] wrongAnswers, string topic, string successText, string _)
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
            Subject = LearningSubject.Creativity,
            ClassLevel = 3
        };
    }
}
