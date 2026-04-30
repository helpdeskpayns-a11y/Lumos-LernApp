using System;
using System.Collections.Generic;
using System.Linq;
using SpieleLernApp.Models;

namespace SpieleLernApp.Services;

public sealed class ArtTaskGenerator : ILearningTaskGenerator
{
    public LearningSubject Subject => LearningSubject.Art;

    public IReadOnlyList<LearningTask> GenerateTasks(TaskGenerationRequest request)
    {
        if (request.ClassLevel != 4)
        {
            throw new NotSupportedException("Der Kunst-Generator ist aktuell für Klasse 4 aufgebaut.");
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
            CreateChoiceTask("Welche Farben sind Grundfarben?", "Rot, Blau und Gelb", ["Grün, Braun und Schwarz", "Pink, Grau und Weiß", "Orange, Lila und Türkis"], "Farben", "Richtig. Rot, Blau und Gelb sind Grundfarben.", 4),
            CreateChoiceTask("Womit kannst du eine Skizze anfertigen?", "mit einem Bleistift", ["mit einem Toaster", "mit einer Gabel", "mit einem Ball"], "Zeichnen", "Prima. Ein Bleistift eignet sich gut für Skizzen.", 4),
            CreateChoiceTask("Was entsteht oft aus vielen aufgeklebten Teilen?", "eine Collage", ["ein Wetterbericht", "eine Uhrzeit", "ein Taschenrechner"], "Gestalten", "Richtig. Das nennt man Collage.", 4),
            CreateChoiceTask("Welche Farbe entsteht aus Blau und Gelb?", "Grün", ["Rot", "Schwarz", "Pink"], "Farben mischen", "Super. Blau und Gelb ergeben Grün.", 4),
            CreateChoiceTask("Was ist bei einer Ausstellung wichtig?", "Bilder ordentlich präsentieren", ["alles verstecken", "Bilder umdrehen", "Rahmen zerreißen"], "Präsentieren", "Prima. Gute Präsentation gehört zur Kunst dazu.", 4),
            CreateChoiceTask("Welches Material eignet sich gut zum Modellieren?", "Ton", ["Wasser", "Sandwichpapier", "Luft"], "Materialien", "Richtig. Ton lässt sich gut formen.", 4),
            CreateChoiceTask("Was hilft dir bei einer Bildidee?", "eine Vorzeichnung", ["gar nichts planen", "alles sofort wegwerfen", "nur den Tisch ansehen"], "Planen", "Prima. Eine Vorzeichnung hilft beim Start.", 4),
            CreateChoiceTask("Welche Wirkung haben warme Farben oft?", "lebendig und nah", ["kalt und weit weg", "lautlos und schwer", "immer unsichtbar"], "Bildwirkung", "Richtig. Warme Farben wirken oft lebendig und nah.", 4),
            CreateChoiceTask("Was ist ein Muster?", "eine wiederkehrende Form oder Farbe", ["ein einzelner Fehler", "nur ein Radiergummi", "eine leere Seite"], "Gestalten", "Super. Muster wiederholen sich.", 4),
            CreateChoiceTask("Wozu dient ein Radiergummi beim Zeichnen?", "zum Verbessern von Linien", ["zum Kleben von Papier", "zum Mischen von Wasserfarben", "zum Schneiden von Karton"], "Zeichnen", "Richtig. Ein Radiergummi hilft beim überarbeiten.", 4),
            CreateChoiceTask("Was passt gut zu einer Landschaft in der Ferne?", "kleinere Formen", ["immer nur riesige Häuser", "keine Farben", "nur schwarze Kreise"], "Räumlichkeit", "Prima. Entfernte Dinge wirken kleiner.", 4),
            CreateChoiceTask("Was gehört zu einem bewussten Bildaufbau?", "Vordergrund und Hintergrund planen", ["alles zufällig verteilen", "nur den Rand bemalen", "nie auf die Fläche achten"], "Bildaufbau", "Richtig. Bildaufbau hilft beim Gestalten.", 4)
        ];
    }

    private static List<LearningTask> BuildWorldTasks()
    {
        return
        [
            CreateChoiceTask("Welche Farbe entsteht aus Rot und Gelb?", "Orange", ["Blau", "Grau", "Schwarz"], "Farben mischen", "Richtig. Rot und Gelb ergeben Orange.", 4),
            CreateChoiceTask("Was ist bei Wasserfarben wichtig?", "mit wenig Wasser starten", ["den Pinsel nie auswaschen", "nur auf den Tisch malen", "alles trocken lassen"], "Malen", "Prima. So lässt sich die Farbe besser steuern.", 4),
            CreateChoiceTask("Was kann ein Hintergrund in einem Bild zeigen?", "den Raum hinter dem Hauptmotiv", ["nur die Unterschrift", "nur das Datum", "nur den Bilderrahmen"], "Bildaufbau", "Richtig. Der Hintergrund zeigt, was hinten im Bild liegt.", 4),
            CreateChoiceTask("Welche Linie wirkt oft ruhig?", "eine waagerechte Linie", ["eine zackige Linie", "eine zerbrochene Linie", "eine wirbelnde Kritzelei"], "Linien", "Super. Waagerechte Linien wirken oft ruhig.", 4),
            CreateChoiceTask("Wozu eignet sich ein Pinsel?", "zum Malen von Farbe", ["zum Lochen von Papier", "zum Messen von Zeit", "zum Rechnen"], "Werkzeuge", "Richtig. Mit einem Pinsel malt man.", 4),
            CreateChoiceTask("Was ist eine Skulptur?", "ein geformtes Kunstwerk aus Material", ["eine Rechenaufgabe", "ein Stundenplan", "eine Wetterwolke"], "Kunstformen", "Prima. Eine Skulptur wird geformt.", 4),
            CreateChoiceTask("Welche Wirkung haben kalte Farben oft?", "ruhig und fern", ["laut und hei?", "immer gefährlich", "nur lustig"], "Bildwirkung", "Richtig. Kalte Farben wirken oft ruhig und fern.", 4),
            CreateChoiceTask("Was hilft, wenn du eine Figur genauer zeichnen willst?", "genau beobachten", ["nur schnell raten", "wegschauen", "den Stift verstecken"], "Zeichnen", "Super. Beobachten hilft beim genauen Zeichnen.", 4),
            CreateChoiceTask("Was gehört zu einer Collage?", "verschiedene Materialien zusammenfügen", ["nur laut vorlesen", "immer nur rechnen", "nichts anfassen"], "Gestalten", "Richtig. Eine Collage verbindet Materialien.", 4),
            CreateChoiceTask("Was kann Licht in einem Bild bewirken?", "hell und dunkel sichtbar machen", ["alle Farben löschen", "das Papier unsichtbar machen", "nur den Rand vergrößern"], "Bildwirkung", "Prima. Licht zeigt Helligkeit und Schatten.", 4),
            CreateChoiceTask("Was hilft bei einem Plakat in Kunst?", "eine klare Überschrift", ["nur winzige Schrift", "gar keine Ordnung", "alles auf eine Ecke"], "Präsentieren", "Richtig. Eine klare Überschrift hilft beim Verstehen.", 4),
            CreateChoiceTask("Warum probiert man in Kunst verschiedene Materialien aus?", "um neue Wirkungen zu entdecken", ["damit alles gleich aussieht", "damit nichts entsteht", "damit man nie beginnt"], "Experimentieren", "Super. Ausprobieren gehört zur Kunst.", 4)
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
            Subject = LearningSubject.Art,
            ClassLevel = classLevel
        };
    }
}
