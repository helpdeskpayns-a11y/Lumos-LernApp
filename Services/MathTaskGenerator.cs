using System;
using System.Collections.Generic;
using System.Linq;
using SpieleLernApp.Models;

namespace SpieleLernApp.Services;

public sealed class MathTaskGenerator : ILearningTaskGenerator
{
    public LearningSubject Subject => LearningSubject.Math;

    public IReadOnlyList<LearningTask> GenerateTasks(TaskGenerationRequest request)
    {
        Random random = request.Seed.HasValue
            ? new Random(request.Seed.Value)
            : new Random();

        List<LearningTask> pool = request.ClassLevel switch
        {
            1 => BuildClass1PortalTasks()
                .Concat(BuildClass1WorldTasks())
                .GroupBy(task => task.Prompt, StringComparer.Ordinal)
                .Select(group => group.First())
                .ToList(),
            2 => BuildClass2PortalTasks()
                .Concat(BuildClass2WorldTasks())
                .GroupBy(task => task.Prompt, StringComparer.Ordinal)
                .Select(group => group.First())
                .ToList(),
            3 => BuildClass3PortalTasks()
                .Concat(BuildClass3WorldTasks())
                .GroupBy(task => task.Prompt, StringComparer.Ordinal)
                .Select(group => group.First())
                .ToList(),
            4 => BuildClass4PortalTasks()
                .Concat(BuildClass4WorldTasks())
                .GroupBy(task => task.Prompt, StringComparer.Ordinal)
                .Select(group => group.First())
                .ToList(),
            _ => throw new NotSupportedException("Der Mathe-Generator ist aktuell für Klasse 1 bis Klasse 4 aufgebaut.")
        };

        return SelectTasks(pool, request.TaskCount, random);
    }

    private static List<LearningTask> BuildClass1PortalTasks()
    {
        return
        [
            CreateNumberTask("Wie viel ist 1 + 2?", 3, "Addition", "Richtig. Beim Addieren werden Mengen zusammengezählt.", 1),
            CreateNumberTask("Wie viel ist 4 + 3?", 7, "Addition", "Richtig. Beim Addieren werden Mengen zusammengezählt.", 1),
            CreateNumberTask("Wie viel ist 5 + 1?", 6, "Addition", "Richtig. Beim Addieren werden Mengen zusammengezählt.", 1),
            CreateNumberTask("Wie viel ist 7 - 2?", 5, "Subtraktion", "Super. Beim Subtrahieren wird etwas weggenommen.", 1),
            CreateNumberTask("Wie viel ist 9 - 4?", 5, "Subtraktion", "Super. Beim Subtrahieren wird etwas weggenommen.", 1),
            CreateNumberTask("Wie viel ist 6 - 3?", 3, "Subtraktion", "Super. Beim Subtrahieren wird etwas weggenommen.", 1),
            CreateNumberTask("Welche Zahl kommt nach 4?", 5, "Zahlenfolge", "Klasse. Du hast die Zahlenreihe richtig erkannt.", 1),
            CreateNumberTask("Welche Zahl kommt nach 8?", 9, "Zahlenfolge", "Klasse. Du hast die Zahlenreihe richtig erkannt.", 1),
            CreateNumberTask("Welche Zahl kommt nach 1?", 2, "Zahlenfolge", "Klasse. Du hast die Zahlenreihe richtig erkannt.", 1),
            CreateChoiceTask("Welche Zahl ist größer als 3?", "5", ["2", "1", "3"], "Vergleichen", "Gut gemacht. Du hast die größere Zahl gefunden.", 1),
            CreateChoiceTask("Welche Zahl ist größer als 6?", "8", ["5", "4", "6"], "Vergleichen", "Gut gemacht. Du hast die größere Zahl gefunden.", 1),
            CreateChoiceTask("Welche Zahl ist größer als 1?", "4", ["0", "1", "2"], "Vergleichen", "Gut gemacht. Du hast die größere Zahl gefunden.", 1)
        ];
    }

    private static List<LearningTask> BuildClass1WorldTasks()
    {
        return
        [
            CreateChoiceTask("Welche Rechnung passt zu 6?", "4 + 2", ["1 + 2", "5 - 2", "2 + 1"], "Rechenwege", "Stark. Du hast den passenden Rechenweg gefunden.", 1),
            CreateChoiceTask("Welche Rechnung passt zu 5?", "3 + 2", ["5 + 2", "6 - 0", "4 + 4"], "Rechenwege", "Stark. Du hast den passenden Rechenweg gefunden.", 1),
            CreateChoiceTask("Welche Rechnung passt zu 3?", "5 - 2", ["1 + 5", "4 + 2", "7 - 1"], "Rechenwege", "Stark. Du hast den passenden Rechenweg gefunden.", 1),
            CreateNumberTask("Welche Zahl fehlt? 2, 3, 4, __", 5, "Zahlenreihe", "Prima. Du hast die Zahlenreihe vervollständigt.", 1),
            CreateNumberTask("Welche Zahl fehlt? 6, 7, __", 8, "Zahlenreihe", "Prima. Du hast die Zahlenreihe vervollständigt.", 1),
            CreateNumberTask("Welche Zahl fehlt? 8, __, 10", 9, "Zahlenreihe", "Prima. Du hast die Zahlenreihe vervollständigt.", 1),
            CreateChoiceTask("Wo sind es am meisten? 2 ?pfel oder 5 ?pfel?", "5 ?pfel", ["2 ?pfel", "beide gleich", "kein Apfel"], "Mengen", "Super. Du hast die größere Menge erkannt.", 1),
            CreateChoiceTask("Wo sind es am meisten? 4 Sterne oder 1 Stern?", "4 Sterne", ["1 Stern", "beide gleich", "0 Sterne"], "Mengen", "Super. Du hast die größere Menge erkannt.", 1),
            CreateChoiceTask("Wo sind es weniger? 3 Bälle oder 6 Bälle?", "3 Bälle", ["6 Bälle", "beide gleich", "7 Bälle"], "Mengen", "Super. Du hast die kleinere Menge erkannt.", 1),
            CreateChoiceTask("Welche Zahl liegt zwischen 4 und 6?", "5", ["3", "6", "7"], "Zahlenraum bis 10", "Klasse. Du hast die Zahl in der Mitte gefunden.", 1),
            CreateChoiceTask("Welche Zahl liegt zwischen 7 und 9?", "8", ["6", "9", "10"], "Zahlenraum bis 10", "Klasse. Du hast die Zahl in der Mitte gefunden.", 1),
            CreateChoiceTask("Welche Zahl ist direkt vor 10?", "9", ["8", "10", "7"], "Zahlenraum bis 10", "Klasse. Du kennst die Zahlen vor und nach einer Zahl.", 1)
        ];
    }

    private static List<LearningTask> BuildClass2PortalTasks()
    {
        return
        [
            CreateNumberTask("Wie viel ist 24 + 15?", 39, "Addition bis 100", "Richtig. Du hast die Aufgabe im Zahlenraum bis 100 gelöst.", 2),
            CreateNumberTask("Wie viel ist 38 + 21?", 59, "Addition bis 100", "Richtig. Du hast die Aufgabe im Zahlenraum bis 100 gelöst.", 2),
            CreateNumberTask("Wie viel ist 46 + 13?", 59, "Addition bis 100", "Richtig. Du hast die Aufgabe im Zahlenraum bis 100 gelöst.", 2),
            CreateNumberTask("Wie viel ist 72 - 20?", 52, "Subtraktion bis 100", "Super. Du hast die Aufgabe im Zahlenraum bis 100 gelöst.", 2),
            CreateNumberTask("Wie viel ist 65 - 14?", 51, "Subtraktion bis 100", "Super. Du hast die Aufgabe im Zahlenraum bis 100 gelöst.", 2),
            CreateNumberTask("Wie viel ist 90 - 30?", 60, "Subtraktion bis 100", "Super. Du hast die Aufgabe im Zahlenraum bis 100 gelöst.", 2),
            CreateChoiceTask("Welche Zahl ist größer als 47?", "52", ["45", "47", "39"], "Vergleichen bis 100", "Gut gemacht. Du hast die größere Zahl erkannt.", 2),
            CreateChoiceTask("Welche Zahl ist kleiner als 63?", "58", ["64", "70", "63"], "Vergleichen bis 100", "Gut gemacht. Du hast die kleinere Zahl erkannt.", 2),
            CreateChoiceTask("Welche Zahl liegt zwischen 58 und 60?", "59", ["57", "60", "61"], "Nachbarzahlen", "Klasse. Du hast die passende Zahl gefunden.", 2),
            CreateNumberTask("Welche Zahl fehlt? 34, 35, __, 37", 36, "Zahlenreihe bis 100", "Prima. Du hast die Zahlenreihe vervollständigt.", 2),
            CreateNumberTask("Welche Zahl fehlt? 48, __, 50", 49, "Zahlenreihe bis 100", "Prima. Du hast die Zahlenreihe vervollständigt.", 2),
            CreateChoiceTask("Welche Zahl hat 5 Zehner und 3 Einer?", "53", ["35", "50", "58"], "Zehner und Einer", "Stark. Du kannst Zehner und Einer richtig zusammensetzen.", 2),
            CreateChoiceTask("Welche Zahl hat 7 Zehner und 1 Einer?", "71", ["17", "70", "81"], "Zehner und Einer", "Stark. Du kannst Zehner und Einer richtig zusammensetzen.", 2),
            CreateChoiceTask("Welche Rechnung passt zu 70?", "50 + 20", ["30 + 20", "80 - 5", "40 + 10"], "Rechenwege bis 100", "Stark. Du hast den passenden Rechenweg gefunden.", 2)
        ];
    }

    private static List<LearningTask> BuildClass2WorldTasks()
    {
        return
        [
            CreateChoiceTask("Welche Rechnung passt zu 44?", "24 + 20", ["10 + 10", "60 - 10", "30 + 3"], "Rechenwege bis 100", "Stark. Du hast den passenden Rechenweg gefunden.", 2),
            CreateChoiceTask("Welche Rechnung passt zu 63?", "70 - 7", ["30 + 20", "50 - 4", "80 - 20"], "Rechenwege bis 100", "Stark. Du hast den passenden Rechenweg gefunden.", 2),
            CreateChoiceTask("Welche Rechnung passt zu 81?", "60 + 21", ["40 + 10", "90 - 20", "70 + 5"], "Rechenwege bis 100", "Stark. Du hast den passenden Rechenweg gefunden.", 2),
            CreateNumberTask("Welche Zahl fehlt? 61, 62, __, 64", 63, "Zahlenreihe bis 100", "Prima. Du hast die Zahlenreihe vervollständigt.", 2),
            CreateNumberTask("Welche Zahl fehlt? 79, __, 81", 80, "Zahlenreihe bis 100", "Prima. Du hast die Zahlenreihe vervollständigt.", 2),
            CreateNumberTask("Welche Zahl fehlt? __, 56, 57", 55, "Zahlenreihe bis 100", "Prima. Du hast die Zahlenreihe vervollständigt.", 2),
            CreateChoiceTask("Wo sind es am meisten? 18 Murmeln oder 24 Murmeln?", "24 Murmeln", ["18 Murmeln", "beide gleich", "20 Murmeln"], "Mengen bis 100", "Super. Du hast die größere Menge erkannt.", 2),
            CreateChoiceTask("Wo sind es weniger? 32 Punkte oder 27 Punkte?", "27 Punkte", ["32 Punkte", "beide gleich", "40 Punkte"], "Mengen bis 100", "Super. Du hast die kleinere Menge erkannt.", 2),
            CreateChoiceTask("Welche Zahl liegt zwischen 82 und 84?", "83", ["81", "84", "85"], "Nachbarzahlen", "Klasse. Du hast die Zahl in der Mitte gefunden.", 2),
            CreateChoiceTask("Welche Zahl liegt zwischen 69 und 71?", "70", ["68", "71", "72"], "Nachbarzahlen", "Klasse. Du hast die Zahl in der Mitte gefunden.", 2),
            CreateChoiceTask("Welche Zahl ist direkt vor 50?", "49", ["48", "50", "51"], "Vorgänger", "Prima. Du kennst den Vorgänger.", 2),
            CreateChoiceTask("Welche Zahl ist direkt nach 89?", "90", ["88", "89", "91"], "Nachfolger", "Prima. Du kennst den Nachfolger.", 2),
            CreateChoiceTask("Welche Zahl ist größer als 76?", "84", ["69", "76", "70"], "Vergleichen bis 100", "Gut gemacht. Du hast die größere Zahl erkannt.", 2),
            CreateChoiceTask("Welche Zahl ist kleiner als 41?", "39", ["42", "41", "45"], "Vergleichen bis 100", "Gut gemacht. Du hast die kleinere Zahl erkannt.", 2)
        ];
    }

    private static List<LearningTask> BuildClass3PortalTasks()
    {
        return
        [
            CreateNumberTask("Wie viel ist 6 x 4?", 24, "Multiplikation", "Richtig. Du hast die Multiplikationsaufgabe gelöst.", 3),
            CreateNumberTask("Wie viel ist 8 x 3?", 24, "Multiplikation", "Richtig. Du hast die Multiplikationsaufgabe gelöst.", 3),
            CreateNumberTask("Wie viel ist 9 x 5?", 45, "Multiplikation", "Richtig. Du hast die Multiplikationsaufgabe gelöst.", 3),
            CreateNumberTask("Wie viel ist 24 : 6?", 4, "Division", "Super. Du hast die Divisionsaufgabe gelöst.", 3),
            CreateNumberTask("Wie viel ist 35 : 5?", 7, "Division", "Super. Du hast die Divisionsaufgabe gelöst.", 3),
            CreateNumberTask("Wie viel ist 42 : 7?", 6, "Division", "Super. Du hast die Divisionsaufgabe gelöst.", 3),
            CreateChoiceTask("Welche Rechnung passt zu 36?", "6 x 6", ["5 x 5", "8 x 3", "4 x 7"], "Einmaleins", "Stark. Du hast die passende Rechnung gefunden.", 3),
            CreateChoiceTask("Welche Rechnung passt zu 28?", "4 x 7", ["3 x 7", "5 x 5", "6 x 6"], "Einmaleins", "Stark. Du hast die passende Rechnung gefunden.", 3),
            CreateChoiceTask("Welche Zahl ist das Ergebnis von 7 x 8?", "56", ["48", "54", "64"], "Einmaleins", "Gut gemacht. Du kennst die Malaufgabe.", 3),
            CreateChoiceTask("Welches Ergebnis passt zu 63 : 9?", "7", ["6", "8", "9"], "Teilen", "Prima. Du hast das Ergebnis richtig gefunden.", 3),
            CreateChoiceTask("Tom hat 3 Tüten mit je 4 Bonbons. Wie viele Bonbons sind das?", "12", ["7", "9", "16"], "Sachaufgaben", "Richtig. Du hast die Sachaufgabe gelöst.", 3),
            CreateChoiceTask("Eine Kiste enthält 5 Reihen mit je 6 ?pfeln. Wie viele ?pfel sind es?", "30", ["11", "25", "36"], "Sachaufgaben", "Richtig. Du hast die Sachaufgabe gelöst.", 3)
        ];
    }

    private static List<LearningTask> BuildClass3WorldTasks()
    {
        return
        [
            CreateChoiceTask("Welche Rechnung passt zu 54?", "6 x 9", ["7 x 7", "8 x 5", "9 x 5"], "Multiplikation", "Stark. Du hast den passenden Rechenweg gefunden.", 3),
            CreateChoiceTask("Welche Rechnung passt zu 18?", "54 : 3", ["16 : 2", "40 : 5", "63 : 9"], "Division", "Stark. Du hast den passenden Rechenweg gefunden.", 3),
            CreateChoiceTask("Welche Zahl fehlt? 7, 14, 21, __", "28", ["24", "27", "35"], "Zahlenmuster", "Prima. Du hast die Zahlenreihe vervollständigt.", 3),
            CreateChoiceTask("Welche Zahl fehlt? 45, 40, 35, __", "30", ["25", "32", "34"], "Zahlenmuster", "Prima. Du hast die Zahlenreihe vervollständigt.", 3),
            CreateChoiceTask("Welche Zahl liegt zwischen 299 und 301?", "300", ["298", "301", "302"], "Zahlenraum", "Klasse. Du hast die Zahl in der Mitte gefunden.", 3),
            CreateChoiceTask("Welche Zahl ist größer als 478?", "482", ["467", "478", "471"], "Vergleichen", "Gut gemacht. Du hast die größere Zahl erkannt.", 3),
            CreateChoiceTask("Welche Zahl ist kleiner als 610?", "599", ["611", "620", "610"], "Vergleichen", "Gut gemacht. Du hast die kleinere Zahl erkannt.", 3),
            CreateChoiceTask("Wie viel ist 12 x 4?", "48", ["36", "44", "52"], "Einmaleins", "Richtig. Du hast die Malaufgabe gelöst.", 3),
            CreateChoiceTask("Wie viel ist 72 : 8?", "9", ["7", "8", "10"], "Teilen", "Richtig. Du hast die Divisionsaufgabe gelöst.", 3),
            CreateChoiceTask("Eine Lehrerin verteilt 24 Stifte gleich auf 6 Tische. Wie viele Stifte liegen auf jedem Tisch?", "4", ["3", "5", "6"], "Sachaufgaben", "Prima. Du hast die Sachaufgabe gelöst.", 3),
            CreateChoiceTask("4 Kinder bekommen jeweils 7 Murmeln. Wie viele Murmeln werden verteilt?", "28", ["11", "21", "32"], "Sachaufgaben", "Prima. Du hast die Sachaufgabe gelöst.", 3),
            CreateChoiceTask("Welche Zahl ist das Ergebnis von 9 x 9?", "81", ["72", "79", "91"], "Einmaleins", "Klasse. Du kennst die Malaufgabe.", 3)
        ];
    }

    private static List<LearningTask> BuildClass4PortalTasks()
    {
        return
        [
            CreateNumberTask("Wie viel ist 125 + 238?", 363, "Addition bis 1000", "Richtig. Du hast die Summe bis 1000 sicher berechnet.", 4),
            CreateNumberTask("Wie viel ist 640 - 275?", 365, "Subtraktion bis 1000", "Super. Du hast die Differenz bis 1000 richtig berechnet.", 4),
            CreateNumberTask("Wie viel ist 48 : 6?", 8, "Division", "Prima. Du hast die Divisionsaufgabe gelöst.", 4),
            CreateChoiceTask("Welche Rechnung passt zu 96?", "12 x 8", ["9 x 9", "14 x 6", "16 x 5"], "Multiplikation", "Stark. Du hast den passenden Rechenweg erkannt.", 4),
            CreateChoiceTask("Welche Zahl ist größer als 799?", "803", ["790", "799", "698"], "Vergleichen", "Gut gemacht. Du hast die größere Zahl gefunden.", 4),
            CreateChoiceTask("Welche Zahl liegt zwischen 459 und 461?", "460", ["458", "461", "470"], "Zahlenraum", "Klasse. Du hast die Zahl in der Mitte gefunden.", 4),
            CreateChoiceTask("Welches Ergebnis passt zu 9 x 7?", "63", ["56", "72", "79"], "Einmaleins", "Richtig. Du kennst das Einmaleins sicher.", 4),
            CreateChoiceTask("Ein Buch kostet 7 Euro. Du kaufst 6 Bücher. Wie viel bezahlst du?", "42", ["36", "48", "56"], "Sachaufgaben", "Prima. Du hast die Sachaufgabe richtig gelöst.", 4),
            CreateChoiceTask("Welche Zahl hat 4 Hunderter, 3 Zehner und 8 Einer?", "438", ["348", "430", "483"], "Stellenwert", "Richtig. Du kannst Zahlen nach Stellenwert zusammensetzen.", 4),
            CreateChoiceTask("Welche Zahl ist auf 100 gerundet gleich 500?", "472", ["438", "451", "549"], "Runden", "Super. Du hast die Zahl passend zum Runden erkannt.", 4),
            CreateChoiceTask("Welche Zahl ist ein Viertel von 20?", "5", ["4", "10", "15"], "Bruchteile", "Stark. Du hast den Bruch richtig bestimmt.", 4),
            CreateChoiceTask("Welche Zahl fehlt? 125, 225, 325, __", "425", ["375", "400", "525"], "Zahlenmuster", "Prima. Du hast das Muster erkannt.", 4)
        ];
    }

    private static List<LearningTask> BuildClass4WorldTasks()
    {
        return
        [
            CreateChoiceTask("Welche Rechnung passt zu 144?", "12 x 12", ["15 x 8", "11 x 11", "18 x 7"], "Multiplikation", "Richtig. Das ist die passende Rechnung.", 4),
            CreateChoiceTask("Welche Zahl fehlt? 900, 850, 800, __", "750", ["700", "760", "780"], "Zahlenmuster", "Prima. Die Reihe zählt in Fünfzigerschritten zurück.", 4),
            CreateChoiceTask("Welche Zahl ist kleiner als 1 000?", "998", ["1 002", "1 050", "1 000"], "Vergleichen", "Gut gemacht. Du hast die kleinere Zahl erkannt.", 4),
            CreateChoiceTask("Wie viel ist 84 : 7?", "12", ["10", "11", "14"], "Division", "Richtig. Du hast die Aufgabe gelöst.", 4),
            CreateChoiceTask("Ein Rechteck hat die Seiten 6 cm und 4 cm. Wie gross ist der Umfang?", "20 cm", ["10 cm", "24 cm", "18 cm"], "Geometrie", "Super. Du hast den Umfang richtig berechnet.", 4),
            CreateChoiceTask("Welche Zahl ist die Hälfte von 96?", "48", ["44", "52", "46"], "Halbieren", "Prima. Du kannst sicher halbieren.", 4),
            CreateChoiceTask("Welche Zahl ist doppelt so gross wie 37?", "74", ["67", "70", "84"], "Verdoppeln", "Stark. Du hast richtig verdoppelt.", 4),
            CreateChoiceTask("Welche Zahl ist auf 10 gerundet gleich 370?", "367", ["362", "374", "381"], "Runden", "Richtig. Diese Zahl rundet sich zu 370.", 4),
            CreateChoiceTask("Welche Zahl ist ein Drittel von 27?", "9", ["6", "12", "18"], "Bruchteile", "Klasse. Du hast den Anteil richtig erkannt.", 4),
            CreateChoiceTask("Vier Kinder teilen 32 Sticker gerecht. Wie viele bekommt jedes Kind?", "8", ["6", "7", "9"], "Sachaufgaben", "Richtig. Gerechtes Teilen führt hier zu 8.", 4),
            CreateChoiceTask("Welche Zahl liegt zwischen 6 499 und 6 501?", "6 500", ["6 490", "6 501", "6 510"], "Zahlenraum", "Prima. Du hast die Zahl in der Mitte gefunden.", 4),
            CreateChoiceTask("Welche Aussage stimmt?", "3 Viertel von 20 sind 15", ["die Hälfte von 20 ist 15", "ein Viertel von 20 ist 10", "doppelt von 20 ist 30"], "Bruchteile", "Super. Du hast die passende Aussage erkannt.", 4)
        ];
    }

    private static List<LearningTask> SelectTasks(IReadOnlyList<LearningTask> pool, int taskCount, Random random)
    {
        return pool
            .OrderBy(_ => random.Next())
            .Take(taskCount)
            .Select((task, index) => CloneWithNumber(task, index + 1, random))
            .ToList();
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

    private static LearningTask CreateNumberTask(string prompt, int correctAnswer, string topic, string successText, int classLevel)
    {
        List<int> answers = [correctAnswer];

        for (int offset = -2; offset <= 2 && answers.Count < 4; offset++)
        {
            int candidate = correctAnswer + offset;
            if (candidate >= 0 && !answers.Contains(candidate))
            {
                answers.Add(candidate);
            }
        }

        while (answers.Count < 4)
        {
            answers.Add(answers[^1] + 1);
        }

        return new LearningTask
        {
            Title = "Aufgabe",
            Prompt = prompt,
            Answers = answers.Select(value => value.ToString()).ToArray(),
            CorrectAnswerIndex = answers.IndexOf(correctAnswer),
            SuccessText = successText,
            Topic = topic,
            Subject = LearningSubject.Math,
            ClassLevel = classLevel
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
            Subject = LearningSubject.Math,
            ClassLevel = classLevel
        };
    }
}
