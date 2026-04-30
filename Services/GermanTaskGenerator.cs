using System;
using System.Collections.Generic;
using System.Linq;
using SpieleLernApp.Models;

namespace SpieleLernApp.Services;

public sealed class GermanTaskGenerator : ILearningTaskGenerator
{
    public LearningSubject Subject => LearningSubject.German;

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
            _ => throw new NotSupportedException("Der Deutsch-Generator ist aktuell für Klasse 1 bis Klasse 4 aufgebaut.")
        };

        return pool
            .OrderBy(_ => random.Next())
            .Take(request.TaskCount)
            .Select((task, index) => CloneWithNumber(task, index + 1, random))
            .ToList();
    }

    private static List<LearningTask> BuildClass1PortalTasks()
    {
        return
        [
            CreateChoiceTask("Mit welchem Buchstaben beginnt Maus?", "M", ["S", "B", "L"], "Anfangsbuchstaben", "Super. Du hast den Anfangsbuchstaben richtig erkannt.", 1),
            CreateChoiceTask("Mit welchem Buchstaben beginnt Sonne?", "S", ["M", "B", "L"], "Anfangsbuchstaben", "Super. Du hast den Anfangsbuchstaben richtig erkannt.", 1),
            CreateChoiceTask("Mit welchem Buchstaben beginnt Fisch?", "F", ["H", "K", "T"], "Anfangsbuchstaben", "Super. Du hast den Anfangsbuchstaben richtig erkannt.", 1),
            CreateChoiceTask("Welches Wort reimt sich auf Haus?", "Maus", ["Baum", "Brot", "Hand"], "Reime", "Klasse. Das Reimwort passt.", 1),
            CreateChoiceTask("Welches Wort reimt sich auf Ball?", "Fall", ["Mond", "Berg", "Nase"], "Reime", "Klasse. Das Reimwort passt.", 1),
            CreateChoiceTask("Welches Wort reimt sich auf Hand?", "Wand", ["Auto", "Blatt", "Mond"], "Reime", "Klasse. Das Reimwort passt.", 1),
            CreateChoiceTask("Wie viele Buchstaben hat Ball?", "4", ["3", "5", "6"], "Buchstaben zählen", "Richtig. Du hast die Buchstaben gut gezählt.", 1),
            CreateChoiceTask("Wie viele Buchstaben hat Katze?", "5", ["4", "6", "7"], "Buchstaben zählen", "Richtig. Du hast die Buchstaben gut gezählt.", 1),
            CreateChoiceTask("Wie viele Buchstaben hat Rakete?", "6", ["5", "7", "8"], "Buchstaben zählen", "Richtig. Du hast die Buchstaben gut gezählt.", 1),
            CreateChoiceTask("Wie viele Silben hat Auto?", "2", ["1", "3", "4"], "Silben", "Gut gemacht. Du hast die Silben richtig erkannt.", 1),
            CreateChoiceTask("Wie viele Silben hat Banane?", "3", ["2", "4", "5"], "Silben", "Gut gemacht. Du hast die Silben richtig erkannt.", 1),
            CreateChoiceTask("Wie viele Silben hat Blume?", "2", ["1", "3", "4"], "Silben", "Gut gemacht. Du hast die Silben richtig erkannt.", 1)
        ];
    }

    private static List<LearningTask> BuildClass1WorldTasks()
    {
        return
        [
            CreateChoiceTask("Mit welchem Buchstaben endet Ball?", "L", ["B", "A", "T"], "Endbuchstaben", "Stark. Du hast den Endbuchstaben gefunden.", 1),
            CreateChoiceTask("Mit welchem Buchstaben endet Maus?", "S", ["M", "A", "R"], "Endbuchstaben", "Stark. Du hast den Endbuchstaben gefunden.", 1),
            CreateChoiceTask("Mit welchem Buchstaben endet Fisch?", "H", ["F", "I", "C"], "Endbuchstaben", "Stark. Du hast den Endbuchstaben gefunden.", 1),
            CreateChoiceTask("Welches Wort passt zu dem Artikel die?", "Blume", ["Ball", "Hase", "Tisch"], "Artikel", "Prima. Du hast den passenden Artikel erkannt.", 1),
            CreateChoiceTask("Welches Wort passt zu dem Artikel der?", "Baum", ["Sonne", "Blume", "Maus"], "Artikel", "Prima. Du hast den passenden Artikel erkannt.", 1),
            CreateChoiceTask("Welches Wort passt zu dem Artikel das?", "Haus", ["Katze", "Nase", "Blume"], "Artikel", "Prima. Du hast den passenden Artikel erkannt.", 1),
            CreateChoiceTask("Welches Wort hat 2 Silben?", "Lampe", ["Maus", "Ball", "Brot"], "Silben hören", "Gut. Du hast das Wort mit 2 Silben gefunden.", 1),
            CreateChoiceTask("Welches Wort hat 3 Silben?", "Tomate", ["Brot", "Ball", "Fisch"], "Silben hören", "Gut. Du hast das Wort mit 3 Silben gefunden.", 1),
            CreateChoiceTask("Welches Wort hat 2 Silben?", "Rose", ["Hand", "Mond", "Zug"], "Silben hören", "Gut. Du hast das Wort mit 2 Silben gefunden.", 1),
            CreateChoiceTask("Welches Wort beginnt wie Maus?", "Milch", ["Sonne", "Lampe", "Ball"], "Anlaute vergleichen", "Klasse. Beide Wörter beginnen gleich.", 1),
            CreateChoiceTask("Welches Wort beginnt wie Fisch?", "Fenster", ["Sonne", "Katze", "Lampe"], "Anlaute vergleichen", "Klasse. Beide Wörter beginnen gleich.", 1),
            CreateChoiceTask("Welches Wort beginnt wie Ball?", "Blume", ["Hase", "Tisch", "Rakete"], "Anlaute vergleichen", "Klasse. Beide Wörter beginnen gleich.", 1)
        ];
    }

    private static List<LearningTask> BuildClass2PortalTasks()
    {
        return
        [
            CreateChoiceTask("Mit welchem Buchstaben beginnt Schule?", "S", ["T", "L", "H"], "Anfangsbuchstaben", "Super. Du hast den Anfangsbuchstaben erkannt.", 2),
            CreateChoiceTask("Mit welchem Buchstaben beginnt Zebra?", "Z", ["B", "S", "M"], "Anfangsbuchstaben", "Super. Du hast den Anfangsbuchstaben erkannt.", 2),
            CreateChoiceTask("Wie viele Buchstaben hat Schule?", "6", ["5", "7", "8"], "Buchstaben zählen", "Richtig. Du hast die Buchstaben gut gezählt.", 2),
            CreateChoiceTask("Wie viele Buchstaben hat Fenster?", "7", ["6", "8", "9"], "Buchstaben zählen", "Richtig. Du hast die Buchstaben gut gezählt.", 2),
            CreateChoiceTask("Wie viele Silben hat Laterne?", "3", ["2", "4", "5"], "Silben", "Gut gemacht. Du hast die Silben richtig erkannt.", 2),
            CreateChoiceTask("Wie viele Silben hat Schule?", "2", ["1", "3", "4"], "Silben", "Gut gemacht. Du hast die Silben richtig erkannt.", 2),
            CreateChoiceTask("Welches Wort reimt sich auf Regen?", "Segen", ["Löffel", "Teller", "Baum"], "Reime", "Klasse. Das Reimwort passt.", 2),
            CreateChoiceTask("Welches Wort reimt sich auf Maus?", "Haus", ["Stuhl", "Blatt", "Hand"], "Reime", "Klasse. Das Reimwort passt.", 2),
            CreateChoiceTask("Welches Wort passt zu dem Artikel die?", "Tafel", ["Stuhl", "Ranzen", "Ball"], "Artikel", "Prima. Du hast den passenden Artikel erkannt.", 2),
            CreateChoiceTask("Welches Wort passt zu dem Artikel das?", "Heft", ["Lehrerin", "Schere", "Tasche"], "Artikel", "Prima. Du hast den passenden Artikel erkannt.", 2),
            CreateChoiceTask("Welches Wort ist ein Tunwort?", "laufen", ["Hund", "schnell", "Haus"], "Wortarten", "Klasse. Ein Tunwort beschreibt eine Handlung.", 2),
            CreateChoiceTask("Welches Wort ist ein Wiewort?", "fröhlich", ["springen", "Tisch", "Auto"], "Wortarten", "Klasse. Ein Wiewort beschreibt etwas genauer.", 2),
            CreateChoiceTask("Welches Wort hat 3 Silben?", "Ananas", ["Ball", "Schuh", "Hand"], "Silben hören", "Gut. Du hast das Wort mit 3 Silben gefunden.", 2),
            CreateChoiceTask("Welches Wort endet auf -en?", "malen", ["Haus", "Hund", "Stuhl"], "Wörter erkennen", "Stark. Du hast das passende Wort erkannt.", 2)
        ];
    }

    private static List<LearningTask> BuildClass2WorldTasks()
    {
        return
        [
            CreateChoiceTask("Mit welchem Buchstaben endet Regen?", "N", ["R", "E", "G"], "Endbuchstaben", "Stark. Du hast den Endbuchstaben gefunden.", 2),
            CreateChoiceTask("Mit welchem Buchstaben endet Tafel?", "L", ["T", "A", "E"], "Endbuchstaben", "Stark. Du hast den Endbuchstaben gefunden.", 2),
            CreateChoiceTask("Welches Wort hat 2 Silben?", "Tafel", ["Ball", "Schrank", "Stift"], "Silben hören", "Gut. Du hast das Wort mit 2 Silben gefunden.", 2),
            CreateChoiceTask("Welches Wort hat 3 Silben?", "Tomate", ["Brot", "Ball", "Schuh"], "Silben hören", "Gut. Du hast das Wort mit 3 Silben gefunden.", 2),
            CreateChoiceTask("Welches Wort beginnt wie Tisch?", "Tiger", ["Maus", "Lampe", "Rose"], "Anlaute vergleichen", "Klasse. Beide Wörter beginnen gleich.", 2),
            CreateChoiceTask("Welches Wort beginnt wie Schule?", "Schuh", ["Tafel", "Maus", "Brot"], "Anlaute vergleichen", "Klasse. Beide Wörter beginnen gleich.", 2),
            CreateChoiceTask("Welches Wort ist die Mehrzahl von Hund?", "Hunde", ["Hund", "Hundes", "Hände"], "Mehrzahl", "Super. Du hast die passende Mehrzahl gefunden.", 2),
            CreateChoiceTask("Welches Wort ist die Mehrzahl von Kind?", "Kinder", ["Kinds", "Kinde", "Kind"], "Mehrzahl", "Super. Du hast die passende Mehrzahl gefunden.", 2),
            CreateChoiceTask("Welches Wort ist das Gegenteil von kalt?", "warm", ["nass", "kurz", "laut"], "Gegenteile", "Prima. Du hast das Gegenteil gefunden.", 2),
            CreateChoiceTask("Welches Wort ist das Gegenteil von hell?", "dunkel", ["schnell", "stark", "offen"], "Gegenteile", "Prima. Du hast das Gegenteil gefunden.", 2),
            CreateChoiceTask("Welches Wort passt in den Satz: Der Vogel kann ...", "fliegen", ["Teller", "freundlich", "Lampe"], "Sätze ergänzen", "Richtig. Das Verb passt in den Satz.", 2),
            CreateChoiceTask("Welches Wort passt in den Satz: Wir schreiben mit dem ...", "Stift", ["laufen", "hell", "springen"], "Sätze ergänzen", "Richtig. Das Nomen passt in den Satz.", 2),
            CreateChoiceTask("Welches Wort ist ein Nomen?", "Schule", ["rennen", "lustig", "schnell"], "Wortarten", "Klasse. Ein Nomen ist ein Namenwort.", 2),
            CreateChoiceTask("Welches Wort ist ein Tunwort?", "springen", ["Tasche", "blau", "Fenster"], "Wortarten", "Klasse. Ein Tunwort beschreibt eine Handlung.", 2)
        ];
    }

    private static List<LearningTask> BuildClass3PortalTasks()
    {
        return
        [
            CreateChoiceTask("Welches Wort ist ein Nomen?", "Freundschaft", ["laufen", "schnell", "denken"], "Wortarten", "Richtig. Ein Nomen ist ein Namenwort.", 3),
            CreateChoiceTask("Welches Wort ist ein Verb?", "springen", ["Tisch", "grün", "heiter"], "Wortarten", "Richtig. Ein Verb beschreibt eine Handlung.", 3),
            CreateChoiceTask("Welches Wort ist ein Adjektiv?", "fröhlich", ["rennen", "Auto", "Schule"], "Wortarten", "Richtig. Ein Adjektiv beschreibt etwas genauer.", 3),
            CreateChoiceTask("Welcher Satz ist richtig geschrieben?", "Der Hund läuft schnell.", ["der Hund läuft schnell", "Der hund läuft schnell.", "Der Hund läuft schnell"], "Satzanfänge", "Super. Satzanfang und Punkt stimmen.", 3),
            CreateChoiceTask("Welches Wort ist die Mehrzahl von Maus?", "Mäuse", ["Mausen", "Mause", "Mäusen"], "Mehrzahl", "Prima. Du hast die richtige Mehrzahl gefunden.", 3),
            CreateChoiceTask("Welches Wort ist die Mehrzahl von Haus?", "Häuser", ["Hausen", "Hause", "Häus"], "Mehrzahl", "Prima. Du hast die richtige Mehrzahl gefunden.", 3),
            CreateChoiceTask("Welches Wort passt in den Satz: Wir ... jeden Morgen zur Schule.", "gehen", ["Haus", "schnell", "Freude"], "Sätze ergänzen", "Richtig. Das Verb passt in den Satz.", 3),
            CreateChoiceTask("Welches Wort passt in den Satz: Das Mädchen liest einen spannenden ...", "Text", ["springen", "laut", "gehen"], "Texte verstehen", "Richtig. Das Wort passt in den Satz.", 3),
            CreateChoiceTask("Welches Wort reimt sich auf Regen?", "Segen", ["Teller", "Lampe", "Besen"], "Reime", "Klasse. Das Reimwort passt.", 3),
            CreateChoiceTask("Welches Wort hat 3 Silben?", "Abenteuer", ["Haus", "Ball", "Brot"], "Silben", "Gut gemacht. Du hast das passende Wort gefunden.", 3),
            CreateChoiceTask("Welches Wort beginnt mit einem großgeschriebenen Nomen?", "Die Blume blüht.", ["die Blume blüht.", "Die blume blüht.", "die blume blüht."], "Rechtschreibung", "Super. Das Nomen ist richtig großgeschrieben.", 3),
            CreateChoiceTask("Welcher Satz erzählt etwas in der richtigen Reihenfolge?", "Zuerst frühstücke ich, dann gehe ich zur Schule.", ["Dann frühstücke ich zuerst.", "Ich Schule gehe, dann frühstücke.", "Gehe Schule dann zuerst."], "Sätze bilden", "Prima. Der Satz ist logisch und gut gebaut.", 3)
        ];
    }

    private static List<LearningTask> BuildClass3WorldTasks()
    {
        return
        [
            CreateChoiceTask("Welches Wort ist das Gegenteil von mutig?", "?ngstlich", ["tapfer", "stark", "laut"], "Gegenteile", "Prima. Du hast das Gegenteil gefunden.", 3),
            CreateChoiceTask("Welches Wort ist das Gegenteil von dunkel?", "hell", ["nass", "warm", "weich"], "Gegenteile", "Prima. Du hast das Gegenteil gefunden.", 3),
            CreateChoiceTask("Welches Wort passt zu dem Artikel das?", "Abenteuer", ["Freundin", "Blume", "Schule"], "Artikel", "Richtig. Der Artikel passt zum Wort.", 3),
            CreateChoiceTask("Welches Wort passt zu dem Artikel die?", "Geschichte", ["Ball", "Hund", "Regen"], "Artikel", "Richtig. Der Artikel passt zum Wort.", 3),
            CreateChoiceTask("Welches Wort endet auf -ung?", "Zeitung", ["laufen", "Tisch", "hell"], "Wörter erkennen", "Klasse. Du hast das passende Wort erkannt.", 3),
            CreateChoiceTask("Welches Wort beginnt wie Schule?", "Schlüssel", ["Teller", "Hase", "Maus"], "Anlaute vergleichen", "Klasse. Beide Wörter beginnen gleich.", 3),
            CreateChoiceTask("Welches Wort hat 4 Silben?", "Elefanten", ["Haus", "Himmel", "Ball"], "Silben", "Gut gemacht. Du hast das passende Wort gefunden.", 3),
            CreateChoiceTask("Welches Wort ist ein Verb?", "denken", ["Sommer", "klug", "Bleistift"], "Wortarten", "Richtig. Ein Verb beschreibt eine Handlung.", 3),
            CreateChoiceTask("Welches Wort ist ein Adjektiv?", "spannend", ["lesen", "Heft", "Wald"], "Wortarten", "Richtig. Ein Adjektiv beschreibt etwas genauer.", 3),
            CreateChoiceTask("Welches Wort ist ein Nomen?", "Wetter", ["rennen", "laut", "frisch"], "Wortarten", "Richtig. Ein Nomen ist ein Namenwort.", 3),
            CreateChoiceTask("Welcher Satz ist richtig?", "Mia schreibt heute einen Brief.", ["mia schreibt heute einen Brief.", "Mia schreibt heute einen brief.", "Mia schreibt heute einen Brief"], "Satzbau", "Super. Satzanfang, Nomen und Punkt stimmen.", 3),
            CreateChoiceTask("Welcher Satz passt logisch?", "Nach dem Aufstehen ziehe ich mich an.", ["Nach dem Schlafen gehe ich zuerst wieder ins Bett.", "Anziehen kommt vor dem Aufstehen.", "Ich esse nachts in der Schule Frühstück."], "Satzlogik", "Prima. Der Ablauf passt logisch zusammen.", 3)
        ];
    }

    private static List<LearningTask> BuildClass4PortalTasks()
    {
        return
        [
            CreateChoiceTask("Welches Wort ist ein Adjektiv?", "mutig", ["laufen", "Baum", "Schule"], "Wortarten", "Richtig. Ein Adjektiv beschreibt etwas genauer.", 4),
            CreateChoiceTask("Welches Wort ist ein Verb?", "entdecken", ["Abenteuer", "leise", "Sommer"], "Wortarten", "Prima. Ein Verb beschreibt eine Handlung.", 4),
            CreateChoiceTask("Welcher Satz ist richtig geschrieben?", "Morgen fahren wir mit dem Zug nach Köln.", ["morgen fahren wir mit dem Zug nach Köln.", "Morgen fahren wir mit dem zug nach Köln.", "Morgen fahren wir mit dem Zug nach köln."], "Rechtschreibung", "Super. Satzanfang und Nomen sind richtig geschrieben.", 4),
            CreateChoiceTask("Welches Wort ist die Mehrzahl von Garten?", "Gärten", ["Gartens", "Gartene", "Gärtenen"], "Mehrzahl", "Richtig. Du hast die passende Mehrzahl erkannt.", 4),
            CreateChoiceTask("Welches Wort passt in den Satz: Die Kinder ... leise zu.", "hören", ["Fenster", "schnell", "bunt"], "Sätze ergänzen", "Prima. Das Verb passt in den Satz.", 4),
            CreateChoiceTask("Welches Wort ist das Gegenteil von freundlich?", "unfreundlich", ["heiter", "nett", "ruhig"], "Gegenteile", "Richtig. Du hast das Gegenteil erkannt.", 4),
            CreateChoiceTask("Welcher Satz steht in der Vergangenheit?", "Gestern spielte Lea im Garten.", ["Heute spielt Lea im Garten.", "Lea spielt morgen im Garten.", "Spiele Lea Garten heute."], "Zeitformen", "Gut gemacht. Das ist eine Vergangenheitsform.", 4),
            CreateChoiceTask("Welches Wort passt am besten zu einer Überschrift für eine Tiergeschichte?", "Der mutige Fuchs", ["Rechnen mit Rest", "Der rote Schraubenzieher", "Busfahrplan im Winter"], "Texte verstehen", "Stark. Die Überschrift passt gut zum Thema.", 4),
            CreateChoiceTask("Welcher Satz enthält wörtliche Rede?", "\"Komm bitte herein\", sagte Amir.", ["Amir ging heute schnell nach Hause.", "Der Regen fiel auf das Dach.", "Morgen ist die Schule geschlossen."], "Wörtliche Rede", "Prima. Das ist wörtliche Rede.", 4),
            CreateChoiceTask("Welches Wort ist ein Nomen?", "Entdeckung", ["rennen", "traurig", "flüstern"], "Wortarten", "Richtig. Ein Nomen ist ein Namenwort.", 4),
            CreateChoiceTask("Welcher Satz ist logisch aufgebaut?", "Zuerst packe ich die Tasche, dann gehe ich los.", ["Dann gehe ich los zuerst Tasche.", "Ich gehe los und packe danach zuerst.", "Zuerst los, dann die Tasche gestern."], "Sätze bilden", "Super. Der Satz ist klar und logisch aufgebaut.", 4),
            CreateChoiceTask("Welches Wort reimt sich auf Wind?", "Kind", ["Wald", "Haus", "Boot"], "Reime", "Richtig. Diese Wörter reimen sich.", 4)
        ];
    }

    private static List<LearningTask> BuildClass4WorldTasks()
    {
        return
        [
            CreateChoiceTask("Welches Wort ist das Gegenteil von laut?", "leise", ["stark", "heiss", "hell"], "Gegenteile", "Prima. Du hast das passende Gegenteil gefunden.", 4),
            CreateChoiceTask("Welcher Satz ist richtig?", "Im Museum sahen wir alte Bilder.", ["im Museum sahen wir alte Bilder.", "Im museum sahen wir alte Bilder.", "Im Museum sahen wir alte bilder."], "Rechtschreibung", "Richtig. Satzanfang und Nomen stimmen.", 4),
            CreateChoiceTask("Welches Wort passt in den Satz: Der Hund hat einen langen ...", "Schwanz", ["springen", "freundlich", "morgen"], "Sätze ergänzen", "Gut gemacht. Das Nomen passt in den Satz.", 4),
            CreateChoiceTask("Welcher Satz steht in der Zukunft?", "Morgen werde ich früh aufstehen.", ["Gestern stand ich früh auf.", "Ich stehe jetzt auf.", "Früh aufstehen gestern morgen."], "Zeitformen", "Richtig. Das ist eine Zukunftsform.", 4),
            CreateChoiceTask("Welches Wort ist die Mehrzahl von Schloss?", "Schlösser", ["Schlosse", "Schlossen", "Schloss"], "Mehrzahl", "Prima. Du hast die Mehrzahl erkannt.", 4),
            CreateChoiceTask("Welche Aussage passt zu einem Sachtext?", "Er erklärt ein Thema mit Fakten.", ["Er ist nur ein Ratespiel.", "Er singt von selbst.", "Er besteht nur aus Reimen."], "Textsorten", "Stark. Das beschreibt einen Sachtext gut.", 4),
            CreateChoiceTask("Welches Wort ist ein Verb?", "beobachten", ["Wolke", "ruhig", "Freundschaft"], "Wortarten", "Richtig. Beobachten ist ein Tunwort.", 4),
            CreateChoiceTask("Welche Überschrift passt zu einem Text über Wasser sparen?", "Jeder Tropfen zählt", ["Die schnellste Rakete", "Mein Lieblingskuchen", "Turnen im Schnee"], "Texte verstehen", "Super. Diese Überschrift passt gut zum Thema.", 4),
            CreateChoiceTask("Welcher Satz enthält ein Adjektiv?", "Das kleine Boot schwamm ruhig.", ["Das Boot schwimmt.", "Boote und Häfen.", "Schwimmen und rudern."], "Satzanalyse", "Richtig. Klein ist hier das Adjektiv.", 4),
            CreateChoiceTask("Welcher Satz zeigt eine klare Reihenfolge?", "Erst lese ich den Text, danach beantworte ich die Fragen.", ["Fragen danach Text lese ich.", "Text danach ich zuerst Fragen.", "Beantworte ich erst danach Text."], "Lesestrategien", "Prima. Die Reihenfolge ist klar formuliert.", 4),
            CreateChoiceTask("Welches Wort passt zu dem Artikel die?", "Erfindung", ["Sommer", "Baum", "Fluss"], "Artikel", "Richtig. Der Artikel passt zum Wort.", 4),
            CreateChoiceTask("Welcher Satz ist wörtliche Rede?", "\"Wir starten jetzt\", rief die Trainerin.", ["Die Trainerin startete den Bus.", "Wir starteten gestern leise.", "Der Start war schnell vorbei."], "Wörtliche Rede", "Klasse. Du hast die wörtliche Rede erkannt.", 4)
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
            Subject = LearningSubject.German,
            ClassLevel = classLevel
        };
    }
}
