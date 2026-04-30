using System;
using System.Collections.Generic;
using System.Linq;
using SpieleLernApp.Models;

namespace SpieleLernApp.Services;

public sealed class MusicTaskGenerator : ILearningTaskGenerator
{
    public LearningSubject Subject => LearningSubject.Music;

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
            _ => throw new NotSupportedException("Der Musik-Generator ist aktuell für Klasse 1 bis Klasse 4 aufgebaut.")
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
            CreateChoiceTask("Womit kannst du trommeln?", "Trommel", ["Flasche", "Kissen", "Stift"], "Instrumente", "Super. Das passt gut zum Musik-Thema.", 1),
            CreateChoiceTask("Worauf hat ein Klavier Tasten?", "Klavier", ["Ball", "Buch", "Tasse"], "Instrumente", "Super. Das passt gut zum Musik-Thema.", 1),
            CreateChoiceTask("Womit kannst du pusten und Musik machen?", "Flöte", ["Löffel", "Stuhl", "Mütze"], "Instrumente", "Super. Das passt gut zum Musik-Thema.", 1),
            CreateChoiceTask("Wie bewegst du dich zu schneller Musik?", "schnell", ["leise", "langsam", "ruhig"], "Bewegung zur Musik", "Super. Das passt gut zum Musik-Thema.", 1),
            CreateChoiceTask("Wie bewegst du dich zu langsamer Musik?", "langsam", ["schnell", "laut", "hoch"], "Bewegung zur Musik", "Super. Das passt gut zum Musik-Thema.", 1),
            CreateChoiceTask("Wie singst du ein Schlaflied meist?", "leise", ["wild", "sehr schnell", "hüpfend"], "Bewegung zur Musik", "Super. Das passt gut zum Musik-Thema.", 1),
            CreateChoiceTask("Wie oft kannst du bei Banane klatschen?", "3", ["2", "4", "5"], "Rhythmus", "Klasse. Du hast den Rhythmus richtig gezählt.", 1),
            CreateChoiceTask("Wie oft kannst du bei Auto klatschen?", "2", ["1", "3", "4"], "Rhythmus", "Klasse. Du hast den Rhythmus richtig gezählt.", 1),
            CreateChoiceTask("Wie oft kannst du bei Lokomotive klatschen?", "5", ["3", "4", "6"], "Rhythmus", "Klasse. Du hast den Rhythmus richtig gezählt.", 1),
            CreateChoiceTask("Wie wirkt fröhliche Musik oft?", "heiter", ["müde", "still", "traurig"], "Musik fühlen", "Super. Das passt gut zum Musik-Thema.", 1),
            CreateChoiceTask("Wie wirkt ein Schlaflied oft?", "ruhig", ["laut", "wild", "hektisch"], "Musik fühlen", "Super. Das passt gut zum Musik-Thema.", 1),
            CreateChoiceTask("Wie fühlt sich ein leises Lied oft an?", "sanft", ["polternd", "schreiend", "rasant"], "Musik fühlen", "Super. Das passt gut zum Musik-Thema.", 1)
        ];
    }

    private static List<LearningTask> BuildClass1WorldTasks()
    {
        return
        [
            CreateChoiceTask("Was machst du oft mit den Händen bei Musik?", "klatschen", ["schlafen", "malen", "zählen"], "Mitmachen", "Klasse. Das passt gut zum Musikmachen.", 1),
            CreateChoiceTask("Was kannst du mit den Füßen zur Musik machen?", "stampfen", ["flüstern", "lesen", "trinken"], "Mitmachen", "Klasse. Das passt gut zum Musikmachen.", 1),
            CreateChoiceTask("Was kannst du zur Musik mit dem Körper machen?", "tanzen", ["schweigen", "rechnen", "zeichnen"], "Mitmachen", "Klasse. Das passt gut zum Musikmachen.", 1),
            CreateChoiceTask("Wie klingt eine Trommel oft?", "dumpf", ["glitzernd", "trocken wie Papier", "stumm"], "Klänge", "Super. Du hast den Klang gut erkannt.", 1),
            CreateChoiceTask("Wie klingt eine Glocke oft?", "hell", ["matschig", "unsichtbar", "stumm"], "Klänge", "Super. Du hast den Klang gut erkannt.", 1),
            CreateChoiceTask("Wie klingt eine Rassel oft?", "raschelnd", ["weich", "dunkel wie Nacht", "reglos"], "Klänge", "Super. Du hast den Klang gut erkannt.", 1),
            CreateChoiceTask("Was kommt zuerst in einem Lied oft vor?", "der Anfang", ["das Ende", "die Pause am Schluss", "gar nichts"], "Liedaufbau", "Prima. Ein Lied hat einen Anfang.", 1),
            CreateChoiceTask("Was kommt nach dem Anfang eines Liedes?", "die Mitte", ["gleich der Schluss", "nur Stille", "gar nichts"], "Liedaufbau", "Prima. Ein Lied hat auch eine Mitte.", 1),
            CreateChoiceTask("Was kommt am Schluss eines Liedes?", "das Ende", ["der Anfang", "die Mitte", "die erste Pause"], "Liedaufbau", "Prima. Ein Lied hat auch ein Ende.", 1),
            CreateChoiceTask("Wie ist Musik, wenn sie sehr laut ist?", "kräftig", ["unsichtbar", "still", "schlafend"], "Laut und leise", "Richtig. Du unterscheidest laut und leise.", 1),
            CreateChoiceTask("Wie ist Musik, wenn sie ganz leise ist?", "sanft", ["polternd", "rasend", "grell"], "Laut und leise", "Richtig. Du unterscheidest laut und leise.", 1),
            CreateChoiceTask("Wie ist Musik, wenn sie immer gleich mächtig klopft?", "gleichmäßig", ["chaotisch", "wortlos", "unscharf"], "Laut und leise", "Richtig. Du erkennst ein gleichmäßiges Muster.", 1)
        ];
    }

    private static List<LearningTask> BuildClass2PortalTasks()
    {
        return
        [
            CreateChoiceTask("Welches Instrument hat Saiten?", "Gitarre", ["Flöte", "Trommel", "Becken"], "Instrumente", "Richtig. Eine Gitarre hat Saiten.", 2),
            CreateChoiceTask("Welches Instrument gehört zu den Blasinstrumenten?", "Flöte", ["Trommel", "Klavier", "Triangel"], "Instrumente", "Super. Eine Flöte ist ein Blasinstrument.", 2),
            CreateChoiceTask("Welches Instrument wird mit Sticks gespielt?", "Trommel", ["Gitarre", "Blockflöte", "Geige"], "Instrumente", "Prima. Eine Trommel spielt man oft mit Sticks.", 2),
            CreateChoiceTask("Wie klingt Musik meist bei einem Schlaflied?", "ruhig", ["hektisch", "grell", "wild"], "Musik fühlen", "Richtig. Ein Schlaflied klingt meist ruhig.", 2),
            CreateChoiceTask("Wie klingt Musik meist bei einem Tanzlied?", "lebendig", ["stumm", "schlafend", "müde"], "Musik fühlen", "Gut gemacht. Ein Tanzlied klingt oft lebendig.", 2),
            CreateChoiceTask("Wie oft kannst du bei Trommel klatschen?", "2", ["1", "3", "4"], "Rhythmus", "Klasse. Du hast den Rhythmus richtig gezählt.", 2),
            CreateChoiceTask("Wie oft kannst du bei Gitarre klatschen?", "3", ["2", "4", "5"], "Rhythmus", "Klasse. Du hast den Rhythmus richtig gezählt.", 2),
            CreateChoiceTask("Wie viele Silben hat das Wort Melodie?", "3", ["2", "4", "5"], "Rhythmus", "Klasse. Melodie hat drei Silben: Me-lo-die.", 2),
            CreateChoiceTask("Was wird in der Musik oft mit Klatschen gezeigt?", "der Rhythmus", ["die Farbe", "die Schrift", "die Pause im Flur"], "Rhythmus", "Richtig. Klatschen hilft beim Rhythmus.", 2),
            CreateChoiceTask("Wie ist Musik, wenn sie sehr leise klingt?", "sanft", ["polternd", "scharf", "stumm"], "Laut und leise", "Richtig. Leise Musik wirkt oft sanft.", 2),
            CreateChoiceTask("Wie ist Musik, wenn sie sehr laut klingt?", "kräftig", ["schlafend", "still", "weich wie Watte"], "Laut und leise", "Richtig. Laute Musik wirkt oft kräftig.", 2),
            CreateChoiceTask("Was gehört oft zu einem Lied?", "eine Melodie", ["eine Zahnbürste", "ein Fenstergriff", "ein Radiergummi"], "Lieder", "Prima. Eine Melodie gehört zu einem Lied.", 2),
            CreateChoiceTask("Was kommt in einem Lied nach dem Anfang?", "die Mitte", ["das Ende zuerst", "nur Stille", "gar nichts"], "Liedaufbau", "Prima. Ein Lied hat auch eine Mitte.", 2),
            CreateChoiceTask("Was kommt am Schluss eines Liedes?", "das Ende", ["der Anfang", "nur die Pause", "die erste Zeile"], "Liedaufbau", "Prima. Ein Lied hat auch ein Ende.", 2)
        ];
    }

    private static List<LearningTask> BuildClass2WorldTasks()
    {
        return
        [
            CreateChoiceTask("Was kannst du zur Musik mit den Händen machen?", "klatschen", ["flüstern", "lesen", "schlafen"], "Mitmachen", "Klasse. Klatschen passt gut zu Musik.", 2),
            CreateChoiceTask("Was kannst du zur Musik mit den Füßen machen?", "stampfen", ["malen", "essen", "zählen"], "Mitmachen", "Klasse. Stampfen passt gut zu Musik.", 2),
            CreateChoiceTask("Welche Musik lädt oft zum Tanzen ein?", "schnelle Musik", ["gar keine Musik", "stille Musik", "eingeschlafene Musik"], "Bewegung zur Musik", "Richtig. Schnelle Musik lädt oft zum Tanzen ein.", 2),
            CreateChoiceTask("Welche Musik lädt oft zum Ausruhen ein?", "ruhige Musik", ["rasende Musik", "hämmernde Musik", "blinkende Musik"], "Bewegung zur Musik", "Richtig. Ruhige Musik lädt oft zum Ausruhen ein.", 2),
            CreateChoiceTask("Wie klingt eine Glocke oft?", "hell", ["muffig", "stumm", "weich wie ein Kissen"], "Klänge", "Super. Eine Glocke klingt oft hell.", 2),
            CreateChoiceTask("Wie klingt eine Trommel oft?", "dumpf", ["gläsern", "stumm", "unsichtbar"], "Klänge", "Super. Eine Trommel klingt oft dumpf.", 2),
            CreateChoiceTask("Wie klingt eine Rassel oft?", "raschelnd", ["schlafend", "leuchtend", "kalt"], "Klänge", "Super. Eine Rassel klingt oft raschelnd.", 2),
            CreateChoiceTask("Was zeigt an, dass Musik gleich wieder beginnt?", "eine Pause endet", ["ein Fenster geht auf", "ein Stift rollt", "der Tisch wird kleiner"], "Musik hören", "Prima. Nach einer Pause geht die Musik weiter.", 2),
            CreateChoiceTask("Welches Instrument kann Töne hoch und tief spielen?", "Klavier", ["Ball", "Besen", "Ranzen"], "Instrumente", "Richtig. Auf einem Klavier gibt es hohe und tiefe Töne.", 2),
            CreateChoiceTask("Welches Wort passt zu gleichmäßigem Klatschen?", "regelmäßig", ["chaotisch", "lautlos", "schlafend"], "Rhythmus", "Gut gemacht. Gleichm??iges Klatschen ist regelmäßig.", 2),
            CreateChoiceTask("Wozu hilft dir Zählen in der Musik?", "beim Rhythmus", ["beim Farbenmischen", "beim Schuheputzen", "beim Fensteröffnen"], "Rhythmus", "Richtig. Zählen hilft beim Rhythmus.", 2),
            CreateChoiceTask("Welche Musik klingt oft fröhlich?", "ein heiteres Lied", ["ein stiller Wecker", "ein kaputter Stuhl", "ein leerer Flur"], "Musik fühlen", "Prima. Ein heiteres Lied klingt oft fröhlich.", 2),
            CreateChoiceTask("Was braucht eine Liedgruppe beim gemeinsamen Singen?", "gemeinsamen Einsatz", ["nur Turnschuhe", "eine Tafelkreide", "einen Regenschirm"], "Gemeinsames Musizieren", "Richtig. Beim gemeinsamen Singen ist ein gemeinsamer Start wichtig.", 2),
            CreateChoiceTask("Was hilft dir, wenn du ein Lied wiederholen möchtest?", "genau zuhören", ["wegsehen", "laut husten", "den Tisch drehen"], "Musik hören", "Richtig. Genaues Zuhören hilft beim Wiederholen.", 2)
        ];
    }

    private static List<LearningTask> BuildClass3PortalTasks()
    {
        return
        [
            CreateChoiceTask("Welches Instrument gehört zu den Saiteninstrumenten?", "Gitarre", ["Trommel", "Triangel", "Rassel"], "Instrumente", "Richtig. Eine Gitarre hat Saiten.", 3),
            CreateChoiceTask("Welches Instrument gehört zu den Blasinstrumenten?", "Querflöte", ["Becken", "Trommel", "Xylophon"], "Instrumente", "Prima. Eine Querflöte ist ein Blasinstrument.", 3),
            CreateChoiceTask("Wie viele Silben hat das Wort Trompete?", "3", ["2", "4", "5"], "Rhythmus", "Richtig. Trompete hat drei Silben: Trom-pe-te.", 3),
            CreateChoiceTask("Was zeigt dir in der Musik den gleichmäßigen Puls?", "der Takt", ["die Farbe", "die Lampe", "der Stuhl"], "Rhythmus", "Richtig. Der Takt gibt den gleichmäßigen Puls vor.", 3),
            CreateChoiceTask("Was gehört zu einem Lied?", "Melodie", ["Fensterbrett", "Schneeschaufel", "Wasserhahn"], "Lieder", "Prima. Eine Melodie gehört zu einem Lied.", 3),
            CreateChoiceTask("Wie klingt Musik oft bei einem ruhigen Lied?", "sanft", ["hektisch", "grell", "kantig"], "Musik fühlen", "Richtig. Ruhige Musik wirkt oft sanft.", 3),
            CreateChoiceTask("Wie klingt Musik oft bei einem Festlied?", "kräftig", ["müde", "stumm", "unsicher"], "Musik fühlen", "Super. Festliche Musik klingt oft kräftig.", 3),
            CreateChoiceTask("Was hilft beim gemeinsamen Singen?", "zusammen einsetzen", ["jeder startet irgendwann", "gar nicht zuhören", "dauernd dazwischenrufen"], "Gemeinsames Musizieren", "Richtig. Gemeinsam einsetzen ist wichtig.", 3),
            CreateChoiceTask("Was kann in einem Lied immer wiederkehren?", "der Refrain", ["das Pausenbrot", "der Stundenplan", "die Schuhgröße"], "Liedaufbau", "Richtig. Ein Refrain kehrt oft wieder.", 3),
            CreateChoiceTask("Was kommt in einem Lied oft zwischen zwei Refrains?", "eine Strophe", ["nur Stille", "eine Schublade", "ein Fenster"], "Liedaufbau", "Prima. Strophen und Refrains wechseln sich oft ab.", 3),
            CreateChoiceTask("Welches Wort passt zu genauem Hinhören in der Musik?", "zuhören", ["weglaufen", "ablenken", "durcheinanderrufen"], "Musik hören", "Richtig. Genaues Zuhören hilft beim Musikmachen.", 3),
            CreateChoiceTask("Was kannst du tun, um einen Rhythmus mitzumachen?", "mitklatschen", ["wegsehen", "still schlafen", "den Stuhl drehen"], "Rhythmus", "Prima. Mitklatschen hilft beim Rhythmus.", 3),
            CreateChoiceTask("Welches Instrument klingt oft hell?", "Triangel", ["Trommel", "Bass", "Kartoffel"], "Klänge", "Richtig. Eine Triangel klingt oft hell.", 3),
            CreateChoiceTask("Welches Instrument klingt oft tief und kräftig?", "Trommel", ["Glöckchen", "Flöte", "Pfeife"], "Klänge", "Richtig. Eine Trommel klingt oft tief und kräftig.", 3)
        ];
    }

    private static List<LearningTask> BuildClass3WorldTasks()
    {
        return
        [
            CreateChoiceTask("Wie viele Silben hat das Wort Melodie?", "3", ["2", "4", "5"], "Rhythmus", "Richtig. Melodie hat drei Silben: Me-lo-die.", 3),
            CreateChoiceTask("Wie viele Silben hat das Wort Xylophon?", "3", ["2", "4", "5"], "Rhythmus", "Richtig. Xylophon hat drei Silben.", 3),
            CreateChoiceTask("Welche Musik lädt oft zum Mitschwingen ein?", "rhythmische Musik", ["stumme Musik", "kaputte Musik", "eingeschlafene Musik"], "Musik fühlen", "Richtig. Rhythmische Musik lädt oft zum Mitschwingen ein.", 3),
            CreateChoiceTask("Welche Musik lädt oft zum Ausruhen ein?", "ruhige Musik", ["rasende Musik", "lärmende Musik", "blinkende Musik"], "Musik fühlen", "Prima. Ruhige Musik lädt eher zum Ausruhen ein.", 3),
            CreateChoiceTask("Was hilft dir, wenn du einen Rhythmus behalten willst?", "mitzählen", ["wegdrehen", "husten", "alles vergessen"], "Rhythmus", "Richtig. Mitzählen hilft beim Rhythmus.", 3),
            CreateChoiceTask("Was ist bei einem Kanon wichtig?", "aufeinander hören", ["jeder singt extra laut", "niemand hört zu", "alle singen etwas anderes"], "Gemeinsames Musizieren", "Prima. Beim Kanon muss man gut aufeinander hören.", 3),
            CreateChoiceTask("Welches Instrument passt zu Schlagen mit Sticks?", "Snare", ["Flöte", "Geige", "Block"], "Instrumente", "Richtig. Eine Snare wird mit Sticks gespielt.", 3),
            CreateChoiceTask("Welches Instrument passt zu Zupfen?", "Harfe", ["Trommel", "Triangel", "Pauke"], "Instrumente", "Richtig. Eine Harfe wird gezupft.", 3),
            CreateChoiceTask("Wie heißt der Teil eines Liedes, der oft wiederkommt?", "Refrain", ["Titelblatt", "Stuhlkreis", "Schlussgong"], "Liedaufbau", "Richtig. Der Refrain wiederholt sich oft.", 3),
            CreateChoiceTask("Was brauchst du, um beim Singen den Ton besser zu treffen?", "genau hinhören", ["wegschauen", "immer schneller werden", "laut stampfen"], "Musik hören", "Prima. Genaues Hinhören hilft dir beim Singen.", 3),
            CreateChoiceTask("Wie klingt eine Glocke oft?", "hell", ["stumpf", "unsichtbar", "weich wie Watte"], "Klänge", "Richtig. Eine Glocke klingt oft hell.", 3),
            CreateChoiceTask("Wie klingt ein Bass oft?", "tief", ["spitz", "durchsichtig", "klebrig"], "Klänge", "Richtig. Ein Bass klingt oft tief.", 3),
            CreateChoiceTask("Was hilft einer Gruppe beim gemeinsamen Musizieren?", "gemeinsamer Einsatz", ["durcheinanderrufen", "gar nicht zählen", "nur auf sich schauen"], "Gemeinsames Musizieren", "Richtig. Ein gemeinsamer Einsatz hält die Gruppe zusammen.", 3),
            CreateChoiceTask("Was kannst du zu Musik mit deinem Körper machen?", "tanzen", ["vergessen", "löschen", "schweigen"], "Bewegung zur Musik", "Prima. Tanzen passt gut zu Musik.", 3)
        ];
    }

    private static List<LearningTask> BuildClass4PortalTasks()
    {
        return
        [
            CreateChoiceTask("Welches Instrument gehört zu den Streichinstrumenten?", "Geige", ["Trommel", "Triangel", "Becken"], "Instrumente", "Richtig. Die Geige ist ein Streichinstrument.", 4),
            CreateChoiceTask("Welches Instrument gehört zu den Holzblasinstrumenten?", "Klarinette", ["Pauke", "Harfe", "Becken"], "Instrumente", "Prima. Die Klarinette ist ein Holzblasinstrument.", 4),
            CreateChoiceTask("Was zeigt in der Musik meist das Tempo?", "wie schnell oder langsam ein Stück ist", ["welche Farbe das Heft hat", "wie schwer das Instrument ist", "wie gro? die Bühne ist"], "Tempo", "Richtig. Das Tempo beschreibt die Geschwindigkeit.", 4),
            CreateChoiceTask("Was kommt in einem Lied oft zwischen zwei Strophen?", "der Refrain", ["das Pausenbrot", "der Stundenplan", "die Schultasche"], "Liedaufbau", "Super. Der Refrain kehrt oft wieder.", 4),
            CreateChoiceTask("Wie wirkt Musik mit einem schnellen Tempo oft?", "lebendig", ["müde", "starr", "lautlos"], "Musik fühlen", "Prima. Schnelles Tempo wirkt oft lebendig.", 4),
            CreateChoiceTask("Wie wirkt Musik mit einem langsamen Tempo oft?", "ruhig", ["hektisch", "zackig", "stumm"], "Musik fühlen", "Richtig. Langsames Tempo klingt oft ruhiger.", 4),
            CreateChoiceTask("Was ist bei einem gemeinsamen Einsatz wichtig?", "aufeinander hören", ["durcheinander starten", "gar nicht schauen", "immer schneller rufen"], "Ensemble", "Gut gemacht. Gemeinsam hören ist wichtig.", 4),
            CreateChoiceTask("Welche Angabe passt zu einem lauten Vortrag?", "forte", ["piano", "pause", "tauschen"], "Dynamik", "Richtig. Forte steht für laut.", 4),
            CreateChoiceTask("Welche Angabe passt zu einem leisen Vortrag?", "piano", ["forte", "trommel", "chor"], "Dynamik", "Prima. Piano steht für leise.", 4),
            CreateChoiceTask("Was hilft dir, einen Rhythmus sicher zu halten?", "regelmäßig mitzählen", ["die Augen schließen und raten", "immer schneller werden", "einfach aufhören"], "Rhythmus", "Richtig. Mitzählen hält den Puls stabil.", 4),
            CreateChoiceTask("Welche Aussage passt zu einem Kanon?", "mehrere Gruppen setzen zeitversetzt ein", ["alle schweigen gleichzeitig", "nur eine Person darf singen", "niemand achtet aufeinander"], "Gemeinsames Musizieren", "Super. So funktioniert ein Kanon.", 4),
            CreateChoiceTask("Welcher Begriff passt zu einem wiederkehrenden Schlagmuster?", "Takt", ["Fenster", "Pausenbrot", "Schatten"], "Rhythmus", "Richtig. Der Takt ordnet die Schläge.", 4)
        ];
    }

    private static List<LearningTask> BuildClass4WorldTasks()
    {
        return
        [
            CreateChoiceTask("Welches Instrument klingt oft tief und warm?", "Cello", ["Triangel", "Pfeife", "Becken"], "Klänge", "Richtig. Ein Cello klingt oft tief und warm.", 4),
            CreateChoiceTask("Was bedeutet crescendo?", "die Musik wird nach und nach lauter", ["die Musik wird sofort leiser", "alles stoppt plötzlich", "nur die Trommel spielt"], "Dynamik", "Prima. Crescendo bedeutet lauter werden.", 4),
            CreateChoiceTask("Was bedeutet decrescendo?", "die Musik wird nach und nach leiser", ["die Musik wird immer schneller", "alle klatschen wild", "nur hohe Töne bleiben"], "Dynamik", "Richtig. Decrescendo bedeutet leiser werden.", 4),
            CreateChoiceTask("Was hilft einer Musikgruppe beim Zusammenspiel?", "gemeinsamer Puls", ["jeder spielt zufällig", "niemand hört zu", "alle starten an verschiedenen Stellen"], "Ensemble", "Super. Ein gemeinsamer Puls hält die Gruppe zusammen.", 4),
            CreateChoiceTask("Welche Aussage passt zu einer Melodie?", "Sie besteht aus aufeinanderfolgenden Tönen", ["Sie ist nur ein Bildrahmen", "Sie ist immer lautlos", "Sie bedeutet automatisch Pause"], "Melodie", "Richtig. Eine Melodie führt Töne nacheinander.", 4),
            CreateChoiceTask("Wozu dienen Pausen in der Musik?", "sie strukturieren das Stück", ["sie löschen alle Töne für immer", "sie machen Instrumente schwerer", "sie ersetzen den Rhythmus"], "Liedaufbau", "Prima. Pausen gehören zur Struktur.", 4),
            CreateChoiceTask("Welche Musik lädt oft zum Marschieren ein?", "klarer gleichmäßiger Rhythmus", ["ungeordnete Geräusche", "dauernde Stille", "unsaubere Einsätze"], "Rhythmus", "Richtig. Ein gleichmäßiger Rhythmus passt gut dazu.", 4),
            CreateChoiceTask("Was ist beim Chor wichtig?", "auf die anderen Stimmen achten", ["nur sich selbst hören", "immer schneller werden", "beliebig spät einsetzen"], "Gemeinsames Musizieren", "Gut gemacht. Gemeinsames Hören ist wichtig.", 4),
            CreateChoiceTask("Welcher Teil eines Liedes erklärt oft eine Geschichte weiter?", "die Strophe", ["der Refrain allein", "der Titel nur", "das Ende zuerst"], "Liedaufbau", "Richtig. In Strophen entwickelt sich der Inhalt weiter.", 4),
            CreateChoiceTask("Was beschreibt die Dynamik in einem Musikstück?", "wie laut oder leise gespielt wird", ["wie schwer ein Instrument ist", "wie lang das Kabel ist", "wie viele Stühle im Raum stehen"], "Dynamik", "Prima. Dynamik betrifft Lautstärke.", 4),
            CreateChoiceTask("Wozu hilft genaues Zuhören beim Singen?", "den richtigen Einsatz und Ton zu treffen", ["den Text zu vergessen", "die Gruppe zu stören", "immer lauter zu rufen"], "Musik hören", "Richtig. Genaues Hören hilft beim Singen.", 4),
            CreateChoiceTask("Welche Aussage passt zu einem Orchester?", "Viele Instrumente spielen gemeinsam", ["Es gibt nur eine Trommel", "Niemand folgt einer Leitung", "Alle spielen dieselbe Stimme gleichzeitig"], "Musikformen", "Super. Ein Orchester verbindet viele Instrumente.", 4)
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
            Subject = LearningSubject.Music,
            ClassLevel = classLevel
        };
    }
}
