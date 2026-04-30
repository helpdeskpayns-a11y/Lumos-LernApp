using System;
using System.Collections.Generic;
using System.Linq;
using SpieleLernApp.Models;

namespace SpieleLernApp.Services;

public sealed class MediaTaskGenerator : ILearningTaskGenerator
{
    public LearningSubject Subject => LearningSubject.Media;

    public IReadOnlyList<LearningTask> GenerateTasks(TaskGenerationRequest request)
    {
        if (request.ClassLevel != 3)
        {
            throw new NotSupportedException("Der Medien-Generator ist aktuell für Klasse 3 aufgebaut.");
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
            CreateChoiceTask("Welches Gerät nutzt du zum Tippen von Texten?", "Computer", ["Zahnbürste", "Fahrradhelm", "Brotdose"], "Digitale Geräte", "Richtig. Ein Computer ist ein digitales Gerät.", "Computer"),
            CreateChoiceTask("Welches Gerät kannst du zum Fotografieren nutzen?", "Tablet", ["Turnbeutel", "Bleistift", "Lineal"], "Digitale Geräte", "Richtig. Ein Tablet kann Fotos machen.", "Tablet"),
            CreateChoiceTask("Was ist ein sicheres Passwort?", "eine geheime Mischung aus Zeichen", ["1234", "meinname", "passwort"], "Sicher im Netz", "Prima. Ein gutes Passwort ist schwer zu erraten.", "eine geheime Mischung aus Zeichen"),
            CreateChoiceTask("Was solltest du im Internet nicht einfach weitergeben?", "deine Adresse", ["dein Lieblingsessen", "deine Lieblingsfarbe", "dein Pausenspiel"], "Sicher im Netz", "Richtig. Private Daten bleiben geschützt.", "deine Adresse"),
            CreateChoiceTask("Was tust du, wenn dir online etwas komisch vorkommt?", "einem Erwachsenen Bescheid sagen", ["sofort anklicken", "weiterleiten", "geheim halten"], "Sicher im Netz", "Super. Hilfe holen ist die richtige Entscheidung.", "einem Erwachsenen Bescheid sagen"),
            CreateChoiceTask("Wofür ist ein Browser da?", "zum öffnen von Internetseiten", ["zum Kochen", "zum Schuheputzen", "zum Rechnen im Kopf"], "Internet", "Richtig. Ein Browser zeigt Internetseiten an.", "zum öffnen von Internetseiten"),
            CreateChoiceTask("Was ist eine Suchmaschine?", "eine Hilfe zum Finden von Infos", ["ein Spielzeugauto", "eine Kamera", "ein Fahrradlicht"], "Internet", "Prima. Eine Suchmaschine hilft beim Finden von Informationen.", "eine Hilfe zum Finden von Infos"),
            CreateChoiceTask("Welches Verhalten ist im Klassenchat freundlich?", "höflich schreiben", ["andere auslachen", "dauernd schreien", "private Fotos schicken"], "Medienregeln", "Richtig. Freundliche Sprache ist wichtig.", "höflich schreiben"),
            CreateChoiceTask("Was ist wichtig, bevor du ein Foto von jemandem verschickst?", "um Erlaubnis fragen", ["einfach senden", "laut lachen", "das Passwort verraten"], "Medienregeln", "Prima. Man fragt vorher um Erlaubnis.", "um Erlaubnis fragen"),
            CreateChoiceTask("Wozu kann ein Tablet in der Schule helfen?", "zum Lernen und üben", ["zum Verstecken", "zum Ausradieren von Heften", "zum Turnen"], "Digitale Welt", "Richtig. Ein Tablet kann beim Lernen helfen.", "zum Lernen und üben"),
            CreateChoiceTask("Welches Zeichen zeigt oft, dass du etwas speichern kannst?", "eine Diskette oder ein Speichersymbol", ["ein Schuh", "eine Wolke aus Stein", "ein Löffel"], "Digitale Welt", "Gut gemacht. Das Speichersymbol steht oft für Speichern.", "eine Diskette oder ein Speichersymbol"),
            CreateChoiceTask("Was ist besser für die Augen bei Bildschirmarbeit?", "Pausen machen", ["stundenlang ohne Pause schauen", "sehr nah rangehen", "im Dunkeln lesen"], "Gesund am Gerät", "Richtig. Pausen sind wichtig.", "Pausen machen")
        ];
    }

    private static List<LearningTask> BuildWorldTasks()
    {
        return
        [
            CreateChoiceTask("Was solltest du tun, bevor du etwas im Internet herunterlädst?", "prüfen, ob es erlaubt und sicher ist", ["einfach alles öffnen", "den Namen ändern", "das Gerät ausschalten"], "Sicher im Netz", "Prima. Vor dem Herunterladen sollte man prüfen, ob etwas sicher ist.", "prüfen, ob es erlaubt und sicher ist"),
            CreateChoiceTask("Welches Gerät kann Texte ausdrucken?", "Drucker", ["Toaster", "Fußball", "Wasserflasche"], "Digitale Geräte", "Richtig. Ein Drucker druckt Texte oder Bilder.", "Drucker"),
            CreateChoiceTask("Was ist in einer Videokonferenz freundlich?", "zuhören und ausreden lassen", ["dazwischenrufen", "dauernd stummschalten anderer", "absichtlich stören"], "Medienregeln", "Richtig. Zuhören gehört zu einem fairen Miteinander.", "zuhören und ausreden lassen"),
            CreateChoiceTask("Warum sollst du dein Passwort geheim halten?", "damit niemand in dein Konto kommt", ["damit es schöner klingt", "damit der Bildschirm heller wird", "damit der Stuhl nicht rutscht"], "Sicher im Netz", "Super. Ein Passwort schützt dein Konto.", "damit niemand in dein Konto kommt"),
            CreateChoiceTask("Was hilft dir, im Internet gute Informationen zu finden?", "mehrere Quellen vergleichen", ["nur die erste Überschrift lesen", "alles glauben", "nie nachfragen"], "Informationen prüfen", "Prima. Mehrere Quellen helfen beim Prüfen.", "mehrere Quellen vergleichen"),
            CreateChoiceTask("Welcher Knopf schaltet ein Gerät oft aus?", "der Ein-/Aus-Knopf", ["die Leertaste", "die Entertaste", "die Shift-Taste"], "Digitale Geräte", "Richtig. Der Ein-/Aus-Knopf schaltet ein Gerät aus.", "der Ein-/Aus-Knopf"),
            CreateChoiceTask("Was ist klug, wenn ein fremdes Fenster aufploppt?", "nicht anklicken und Hilfe holen", ["alles bestätigen", "das Passwort eingeben", "den Namen verraten"], "Sicher im Netz", "Richtig. Fremde Fenster sollte man vorsichtig behandeln.", "nicht anklicken und Hilfe holen"),
            CreateChoiceTask("Was ist eine gute Bildschirmhaltung?", "mit etwas Abstand sitzen", ["ganz nah am Bildschirm kleben", "schief liegen", "mit dem Rücken zum Bildschirm sitzen"], "Gesund am Gerät", "Prima. Abstand und Haltung sind wichtig.", "mit etwas Abstand sitzen"),
            CreateChoiceTask("Woran erkennst du oft ein sicheres Gespräch online?", "an bekannten Personen und klaren Regeln", ["an wilden Kettenbriefen", "an fremden Passwortfragen", "an Drohungen"], "Sicher im Netz", "Richtig. Bekannte Kontakte und Regeln machen Gespräche sicherer.", "an bekannten Personen und klaren Regeln"),
            CreateChoiceTask("Wozu dient ein WLAN?", "zur kabellosen Verbindung mit dem Internet", ["zum Pflanzen gießen", "zum Messen von Schuhen", "zum Schneiden von Papier"], "Internet", "Richtig. WLAN verbindet Geräte kabellos mit dem Netz.", "zur kabellosen Verbindung mit dem Internet"),
            CreateChoiceTask("Was machst du, wenn du aus Versehen auf etwas Falsches geklickt hast?", "einem Erwachsenen Bescheid sagen", ["heimlich weitermachen", "das Passwort überall eingeben", "das Gerät verstecken"], "Sicher im Netz", "Super. Hilfe holen ist dann genau richtig.", "einem Erwachsenen Bescheid sagen"),
            CreateChoiceTask("Was ist im Internet eine gute Regel?", "erst denken, dann klicken", ["immer sofort klicken", "alles weiterschicken", "nie nachfragen"], "Medienregeln", "Prima. Erst denken, dann klicken ist eine gute Regel.", "erst denken, dann klicken")
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
            Subject = LearningSubject.Media,
            ClassLevel = 3
        };
    }
}
