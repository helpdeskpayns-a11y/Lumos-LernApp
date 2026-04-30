using System;
using System.Collections.Generic;
using System.Linq;
using SpieleLernApp.Models;

namespace SpieleLernApp.Services;

public sealed class LogicTaskGenerator : ILearningTaskGenerator
{
    public LearningSubject Subject => LearningSubject.Logic;

    public IReadOnlyList<LearningTask> GenerateTasks(TaskGenerationRequest request)
    {
        if (request.ClassLevel is < 2 or > 4)
        {
            throw new NotSupportedException("Der Logik-Generator ist aktuell für Klasse 2 bis Klasse 4 aufgebaut.");
        }

        Random random = request.Seed.HasValue
            ? new Random(request.Seed.Value)
            : new Random();

        List<LearningTask> pool = request.ClassLevel switch
        {
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
            _ => BuildClass4PortalTasks()
                .Concat(BuildClass4WorldTasks())
                .GroupBy(task => task.Prompt, StringComparer.Ordinal)
                .Select(group => group.First())
                .ToList()
        };

        return pool
            .OrderBy(_ => random.Next())
            .Take(request.TaskCount)
            .Select((task, index) => CloneWithNumber(task, index + 1, random))
            .ToList();
    }

    private static List<LearningTask> BuildClass2PortalTasks()
    {
        return
        [
            CreateChoiceTask("Welche Zahl setzt die Reihe fort? 2, 4, 6, __", "8", ["7", "9", "10"], "Zahlenmuster", "Richtig. Du hast das Muster erkannt.", 2),
            CreateChoiceTask("Welche Zahl setzt die Reihe fort? 5, 10, 15, __", "20", ["18", "19", "25"], "Zahlenmuster", "Richtig. Du hast das Muster erkannt.", 2),
            CreateChoiceTask("Welche Form kommt als Nächstes? Kreis, Quadrat, Kreis, Quadrat, __", "Kreis", ["Dreieck", "Quadrat", "Stern"], "Formenmuster", "Super. Das Muster wiederholt sich.", 2),
            CreateChoiceTask("Welche Form kommt als Nächstes? Dreieck, Dreieck, Kreis, Dreieck, Dreieck, Kreis, __", "Dreieck", ["Kreis", "Rechteck", "Stern"], "Formenmuster", "Super. Das Muster wiederholt sich.", 2),
            CreateChoiceTask("Welcher Tag kommt nach Dienstag?", "Mittwoch", ["Montag", "Freitag", "Sonntag"], "Reihenfolgen", "Klasse. Du kennst die Reihenfolge.", 2),
            CreateChoiceTask("Welcher Monat kommt nach März?", "April", ["Juni", "Februar", "Mai"], "Reihenfolgen", "Klasse. Du kennst die Reihenfolge.", 2),
            CreateChoiceTask("Welches Wort passt nicht dazu? Apfel, Birne, Auto, Banane", "Auto", ["Apfel", "Birne", "Banane"], "Kategorien", "Prima. Das Wort gehört nicht zur Obstgruppe.", 2),
            CreateChoiceTask("Welches Wort passt nicht dazu? Hund, Katze, Tisch, Pferd", "Tisch", ["Hund", "Katze", "Pferd"], "Kategorien", "Prima. Das Wort gehört nicht zur Tiergruppe.", 2),
            CreateChoiceTask("Welche Zahl fehlt? 12, 14, __, 18", "16", ["15", "17", "19"], "Zahlenmuster", "Richtig. Die Reihe geht in Zweierschritten weiter.", 2),
            CreateChoiceTask("Welche Zahl fehlt? 30, __, 50", "40", ["35", "45", "60"], "Zahlenmuster", "Richtig. Die Reihe geht in Zehnerschritten weiter.", 2),
            CreateChoiceTask("Welche Richtung ist das Gegenteil von links?", "rechts", ["oben", "unten", "geradeaus"], "Richtungen", "Richtig. Links und rechts sind Gegensätze.", 2),
            CreateChoiceTask("Was kommt im Tagesablauf meist zuerst?", "aufstehen", ["schlafen gehen", "Zähne putzen nach der Schule", "Abendbrot"], "Abläufe", "Prima. Du hast den Tagesablauf richtig geordnet.", 2),
            CreateChoiceTask("Welches Paar passt zusammen?", "Schlüssel - Schloss", ["Schuhe - Fenster", "Mond - Suppe", "Besen - Wolke"], "Beziehungen", "Richtig. Diese Dinge gehören zusammen.", 2),
            CreateChoiceTask("Welches Paar passt zusammen?", "Zahnbürste - Zähne", ["Tasse - Hose", "Heft - Regen", "Schrank - Sonne"], "Beziehungen", "Richtig. Diese Dinge gehören zusammen.", 2)
        ];
    }

    private static List<LearningTask> BuildClass2WorldTasks()
    {
        return
        [
            CreateChoiceTask("Welche Zahl setzt die Reihe fort? 3, 6, 9, __", "12", ["10", "11", "13"], "Zahlenmuster", "Richtig. Du hast das Muster erkannt.", 2),
            CreateChoiceTask("Welche Zahl setzt die Reihe fort? 20, 18, 16, __", "14", ["12", "15", "17"], "Zahlenmuster", "Richtig. Die Reihe zählt zurück.", 2),
            CreateChoiceTask("Welche Form kommt als Nächstes? Stern, Kreis, Stern, Kreis, __", "Stern", ["Kreis", "Quadrat", "Dreieck"], "Formenmuster", "Super. Das Muster wiederholt sich.", 2),
            CreateChoiceTask("Welcher Tag kommt vor Freitag?", "Donnerstag", ["Mittwoch", "Samstag", "Montag"], "Reihenfolgen", "Klasse. Du kennst die Reihenfolge.", 2),
            CreateChoiceTask("Welcher Monat kommt vor Oktober?", "September", ["August", "November", "Juli"], "Reihenfolgen", "Klasse. Du kennst die Reihenfolge.", 2),
            CreateChoiceTask("Welches Wort passt nicht dazu? rot, blau, grün, Löffel", "Löffel", ["rot", "blau", "grün"], "Kategorien", "Prima. Das Wort gehört nicht zu den Farben.", 2),
            CreateChoiceTask("Welches Wort passt nicht dazu? Sonne, Mond, Stern, Stuhl", "Stuhl", ["Sonne", "Mond", "Stern"], "Kategorien", "Prima. Das Wort gehört nicht zum Himmel.", 2),
            CreateChoiceTask("Welche Zahl fehlt? 4, 8, __, 16", "12", ["10", "14", "18"], "Zahlenmuster", "Richtig. Die Reihe geht in Viererschritten weiter.", 2),
            CreateChoiceTask("Welche Zahl fehlt? 50, 45, __, 35", "40", ["38", "42", "44"], "Zahlenmuster", "Richtig. Die Reihe zählt in Fünferschritten zurück.", 2),
            CreateChoiceTask("Welche Richtung ist das Gegenteil von oben?", "unten", ["links", "rechts", "geradeaus"], "Richtungen", "Richtig. Oben und unten sind Gegensätze.", 2),
            CreateChoiceTask("Was kommt im Tagesablauf meist nach dem Aufstehen?", "frühstücken", ["einschlafen", "Abendbrot", "Pause auf dem Schulhof"], "Abläufe", "Prima. Du hast den Tagesablauf richtig geordnet.", 2),
            CreateChoiceTask("Welches Paar passt zusammen?", "Regenschirm - Regen", ["Gabel - Kissen", "Sonne - Schal", "Maus - Schuh"], "Beziehungen", "Richtig. Diese Dinge gehören zusammen.", 2),
            CreateChoiceTask("Welches Paar passt zusammen?", "Schere - schneiden", ["Besen - trinken", "Ball - lesen", "Tafel - schlafen"], "Beziehungen", "Richtig. Diese Dinge gehören zusammen.", 2),
            CreateChoiceTask("Welcher Weg ist logisch? Schule -> nach Hause -> Abendessen -> __", "schlafen", ["Frühstück", "zur Schule gehen", "Pause"], "Abläufe", "Gut gemacht. Der Ablauf passt.", 2)
        ];
    }

    private static List<LearningTask> BuildClass3PortalTasks()
    {
        return
        [
            CreateChoiceTask("Welche Zahl setzt die Reihe fort? 4, 8, 12, __", "16", ["14", "18", "20"], "Zahlenmuster", "Richtig. Die Reihe wächst immer um vier.", 3),
            CreateChoiceTask("Welche Zahl setzt die Reihe fort? 27, 24, 21, __", "18", ["17", "19", "20"], "Zahlenmuster", "Richtig. Die Reihe zählt in Dreierschritten zurück.", 3),
            CreateChoiceTask("Welches Muster passt weiter? rot, blau, grün, rot, blau, __", "grün", ["rot", "gelb", "blau"], "Muster", "Super. Das Farb-Muster wiederholt sich.", 3),
            CreateChoiceTask("Welches Wort passt nicht in die Gruppe? Dreieck, Kreis, Quadrat, Banane", "Banane", ["Dreieck", "Kreis", "Quadrat"], "Kategorien", "Prima. Banane ist keine Form.", 3),
            CreateChoiceTask("Was kommt logisch nach Frühling?", "Sommer", ["Winter", "Herbst", "Montag"], "Reihenfolgen", "Richtig. Nach dem Frühling kommt der Sommer.", 3),
            CreateChoiceTask("Welche Zahl fehlt? 120, 130, __, 150", "140", ["135", "145", "160"], "Zahlenmuster", "Richtig. Die Reihe geht in Zehnerschritten weiter.", 3),
            CreateChoiceTask("Welches Paar gehört zusammen?", "Brief - Briefkasten", ["Milch - Schuh", "Besen - Fernseher", "Fenster - Löffel"], "Beziehungen", "Richtig. Diese Dinge gehören zusammen.", 3),
            CreateChoiceTask("Welche Richtung ist das Gegenteil von Norden?", "Süden", ["Westen", "Osten", "oben"], "Richtungen", "Richtig. Norden und Süden sind Gegensätze.", 3),
            CreateChoiceTask("Was kommt in einem Ablauf zuerst?", "Teig machen", ["Kuchen essen", "Ofen ausschalten", "Tisch abräumen"], "Abläufe", "Prima. Vor dem Backen braucht man zuerst den Teig.", 3),
            CreateChoiceTask("Welche Regel passt? Wenn heute Mittwoch ist, ist morgen __", "Donnerstag", ["Dienstag", "Freitag", "Montag"], "Reihenfolgen", "Richtig. Auf Mittwoch folgt Donnerstag.", 3),
            CreateChoiceTask("Was passt logisch? 2, 6, 10, __", "14", ["12", "15", "16"], "Zahlenmuster", "Super. Die Reihe wächst immer um vier.", 3),
            CreateChoiceTask("Welcher Weg ist logisch? aufstehen -> anziehen -> frühstücken -> __", "zur Schule gehen", ["einschlafen", "Mitternacht", "Zähne verlieren"], "Abläufe", "Gut gemacht. Der Ablauf passt logisch zusammen.", 3),
            CreateChoiceTask("Welche Form hat vier gleich lange Seiten?", "Quadrat", ["Kreis", "Dreieck", "Oval"], "Formenwissen", "Richtig. Ein Quadrat hat vier gleich lange Seiten.", 3),
            CreateChoiceTask("Welches Wort gehört nicht dazu? rechnen, knobeln, lösen, schlafen", "schlafen", ["rechnen", "knobeln", "lösen"], "Kategorien", "Prima. Schlafen passt nicht zu den Denkaufgaben.", 3)
        ];
    }

    private static List<LearningTask> BuildClass3WorldTasks()
    {
        return
        [
            CreateChoiceTask("Welche Zahl setzt die Reihe fort? 9, 18, 27, __", "36", ["30", "35", "45"], "Zahlenmuster", "Richtig. Die Reihe wächst immer um neun.", 3),
            CreateChoiceTask("Welche Zahl setzt die Reihe fort? 64, 56, 48, __", "40", ["42", "44", "38"], "Zahlenmuster", "Richtig. Die Reihe zählt in Achterschritten zurück.", 3),
            CreateChoiceTask("Was gehört nicht dazu? Würfel, Kugel, Zylinder, Erdbeere", "Erdbeere", ["Würfel", "Kugel", "Zylinder"], "Kategorien", "Prima. Erdbeere ist kein Körper.", 3),
            CreateChoiceTask("Welcher Monat kommt vor August?", "Juli", ["Juni", "September", "Mai"], "Reihenfolgen", "Richtig. Vor August kommt Juli.", 3),
            CreateChoiceTask("Welche Zahl fehlt? 45, __, 55", "50", ["48", "52", "60"], "Zahlenmuster", "Richtig. Die Reihe geht in Fünferschritten weiter.", 3),
            CreateChoiceTask("Welche Richtung ist das Gegenteil von Westen?", "Osten", ["Norden", "Süden", "unten"], "Richtungen", "Richtig. Westen und Osten sind Gegensätze.", 3),
            CreateChoiceTask("Was ist logisch? lesen -> verstehen -> antworten -> __", "überprüfen", ["einschlafen", "wegrennen", "vergessen"], "Abläufe", "Gut gemacht. So geht man Schritt für Schritt vor.", 3),
            CreateChoiceTask("Welches Paar passt zusammen?", "Landkarte - Orientierung", ["Teller - fliegen", "Wolke - schreiben", "Kissen - rechnen"], "Beziehungen", "Richtig. Mit einer Landkarte orientiert man sich.", 3),
            CreateChoiceTask("Welches Muster geht weiter? A, B, C, A, B, __", "C", ["A", "B", "D"], "Muster", "Super. Das Buchstabenmuster wiederholt sich.", 3),
            CreateChoiceTask("Welcher Tag kommt zwei Tage nach Montag?", "Mittwoch", ["Dienstag", "Donnerstag", "Freitag"], "Reihenfolgen", "Richtig. Zwei Tage nach Montag ist Mittwoch.", 3),
            CreateChoiceTask("Welche Zahl fehlt? 200, 180, __, 140", "160", ["150", "170", "155"], "Zahlenmuster", "Richtig. Die Reihe geht in Zwanzigerschritten zurück.", 3),
            CreateChoiceTask("Was passt logisch zu Schloss?", "Schlüssel", ["Kissen", "Stuhl", "Wolke"], "Beziehungen", "Prima. Schlüssel und Schloss gehören zusammen.", 3),
            CreateChoiceTask("Welche Aussage ist logisch?", "Erst denken, dann handeln", ["Erst rennen, dann aufwachen", "Immer rückwärts frühstücken", "Nie auf Regeln achten"], "Logik im Alltag", "Richtig. Nachdenken hilft bei guten Entscheidungen.", 3),
            CreateChoiceTask("Welche Form hat keine Ecken?", "Kreis", ["Rechteck", "Dreieck", "Quadrat"], "Formenwissen", "Richtig. Ein Kreis hat keine Ecken.", 3)
        ];
    }

    private static List<LearningTask> BuildClass4PortalTasks()
    {
        return
        [
            CreateChoiceTask("Welche Zahl setzt die Reihe fort? 15, 30, 45, __", "60", ["55", "65", "75"], "Zahlenmuster", "Richtig. Die Reihe wächst immer um 15.", 4),
            CreateChoiceTask("Welche Zahl setzt die Reihe fort? 200, 180, 160, __", "140", ["150", "145", "130"], "Zahlenmuster", "Prima. Die Reihe zählt in Zwanzigerschritten zurück.", 4),
            CreateChoiceTask("Welches Wort passt nicht dazu? Dreieck, Quadrat, Kreis, Giraffe", "Giraffe", ["Dreieck", "Quadrat", "Kreis"], "Kategorien", "Richtig. Giraffe ist keine Form.", 4),
            CreateChoiceTask("Welche Richtung ist das Gegenteil von Süden?", "Norden", ["Osten", "Westen", "unten"], "Richtungen", "Gut gemacht. Norden und Süden sind Gegensätze.", 4),
            CreateChoiceTask("Welche Regel passt? Wenn heute Freitag ist, ist übermorgen __", "Sonntag", ["Samstag", "Montag", "Donnerstag"], "Reihenfolgen", "Richtig. Nach Freitag kommen Samstag und Sonntag.", 4),
            CreateChoiceTask("Welche Zahl fehlt? 125, 150, __, 200", "175", ["165", "170", "180"], "Zahlenmuster", "Prima. Du hast die Reihe erkannt.", 4),
            CreateChoiceTask("Welches Paar passt zusammen?", "Kompass - Orientierung", ["Fenster - Suppe", "Besen - Schlafen", "Schuh - Wolke"], "Beziehungen", "Richtig. Diese Begriffe gehören zusammen.", 4),
            CreateChoiceTask("Was kommt logisch zuerst?", "den Plan lesen", ["das Ergebnis feiern", "alles wegräumen", "den Schluss schreiben"], "Abläufe", "Stark. Vor dem Start hilft der Plan.", 4),
            CreateChoiceTask("Welches Muster geht weiter? A, C, E, G, __", "I", ["H", "J", "K"], "Buchstabenmuster", "Richtig. Das Muster springt immer einen Buchstaben weiter.", 4),
            CreateChoiceTask("Welche Aussage ist logisch?", "Erst vergleichen, dann entscheiden", ["Erst raten, dann nie prüfen", "Immer alles gleichzeitig tun", "Nie auf Regeln achten"], "Logik im Alltag", "Prima. Vergleichen hilft beim Entscheiden.", 4),
            CreateChoiceTask("Welche Form hat genau drei Ecken?", "Dreieck", ["Kreis", "Quadrat", "Oval"], "Formenwissen", "Richtig. Ein Dreieck hat drei Ecken.", 4),
            CreateChoiceTask("Welche Zahl passt? 3, 9, 27, __", "81", ["54", "72", "90"], "Zahlenmuster", "Super. Die Zahlen werden jeweils mit 3 multipliziert.", 4)
        ];
    }

    private static List<LearningTask> BuildClass4WorldTasks()
    {
        return
        [
            CreateChoiceTask("Welche Zahl setzt die Reihe fort? 96, 84, 72, __", "60", ["58", "62", "64"], "Zahlenmuster", "Richtig. Die Reihe sinkt immer um 12.", 4),
            CreateChoiceTask("Welcher Monat kommt zwei Monate nach April?", "Juni", ["Mai", "Juli", "August"], "Reihenfolgen", "Prima. Zwei Monate nach April ist Juni.", 4),
            CreateChoiceTask("Was passt logisch? sammeln -> ordnen -> auswerten -> __", "erklären", ["vergessen", "verstecken", "wegwerfen"], "Abläufe", "Richtig. Nach dem Auswerten kann man Ergebnisse erklären.", 4),
            CreateChoiceTask("Welches Muster geht weiter? rot, rot, blau, rot, rot, blau, __", "rot", ["blau", "gruen", "gelb"], "Muster", "Super. Das Farb-Muster wiederholt sich.", 4),
            CreateChoiceTask("Welche Zahl fehlt? 1000, 900, __, 700", "800", ["750", "820", "850"], "Zahlenmuster", "Prima. Die Reihe zählt in Hunderterschritten zurück.", 4),
            CreateChoiceTask("Welche Richtung liegt zwischen Norden und Osten?", "Nordosten", ["Südwesten", "Westen", "unten"], "Richtungen", "Richtig. Nordosten liegt dazwischen.", 4),
            CreateChoiceTask("Welches Paar gehört zusammen?", "Thermometer - Temperatur", ["Buch - Regen", "Kissen - Rennen", "Fenster - Mittag"], "Beziehungen", "Stark. Diese Begriffe passen zusammen.", 4),
            CreateChoiceTask("Welche Aussage ist logisch?", "Ein Plan hilft bei schwierigen Aufgaben", ["Pläne stören immer", "Nachdenken bringt nie etwas", "Regeln gelten nur nachts"], "Logik im Alltag", "Richtig. Planung hilft beim Lösen.", 4),
            CreateChoiceTask("Welcher Tag kommt drei Tage nach Dienstag?", "Freitag", ["Donnerstag", "Samstag", "Montag"], "Reihenfolgen", "Prima. Nach Dienstag folgen Mittwoch, Donnerstag und Freitag.", 4),
            CreateChoiceTask("Welche Form passt nicht dazu? Würfel, Kugel, Zylinder, Banane", "Banane", ["Würfel", "Kugel", "Zylinder"], "Kategorien", "Richtig. Banane ist kein geometrischer Körper.", 4),
            CreateChoiceTask("Welche Zahl passt? 7, 14, 28, 56, __", "112", ["98", "104", "120"], "Zahlenmuster", "Super. Die Zahlen werden jeweils verdoppelt.", 4),
            CreateChoiceTask("Was ist logisch? fragen -> prüfen -> lösen -> __", "erklären", ["vergessen", "verstecken", "nichts tun"], "Denkwege", "Richtig. Nach dem Lösen kann man den Weg erklären.", 4)
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
            Subject = LearningSubject.Logic,
            ClassLevel = classLevel
        };
    }
}
