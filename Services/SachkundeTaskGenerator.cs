using System;
using System.Collections.Generic;
using System.Linq;
using SpieleLernApp.Models;

namespace SpieleLernApp.Services;

public sealed class SachkundeTaskGenerator : ILearningTaskGenerator
{
    public LearningSubject Subject => LearningSubject.Sachunterricht;

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
            _ => throw new NotSupportedException("Der Sachkunde-Generator ist aktuell für Klasse 1 bis Klasse 4 aufgebaut.")
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
            CreateChoiceTask("Im Winter fällt Schnee. Welche Jahreszeit ist das?", "Winter", ["Sommer", "Herbst", "Frühling"], "Jahreszeiten", "Richtig. Das passt gut zum Sachkunde-Thema.", 1),
            CreateChoiceTask("Im Sommer ist es oft warm und sonnig. Welche Jahreszeit ist das?", "Sommer", ["Winter", "Herbst", "Frühling"], "Jahreszeiten", "Richtig. Das passt gut zum Sachkunde-Thema.", 1),
            CreateChoiceTask("Im Herbst fallen viele Blätter. Welche Jahreszeit ist das?", "Herbst", ["Winter", "Sommer", "Frühling"], "Jahreszeiten", "Richtig. Das passt gut zum Sachkunde-Thema.", 1),
            CreateChoiceTask("Wo lebt der Fisch?", "im Wasser", ["im Nest", "im Stall", "auf dem Baum"], "Tiere", "Richtig. Das passt gut zum Sachkunde-Thema.", 1),
            CreateChoiceTask("Wo lebt das Huhn?", "im Stall", ["im Wasser", "im Teich", "in der Höhle"], "Tiere", "Richtig. Das passt gut zum Sachkunde-Thema.", 1),
            CreateChoiceTask("Wo lebt der Vogel?", "im Nest", ["im Stall", "im Wasser", "im Keller"], "Tiere", "Richtig. Das passt gut zum Sachkunde-Thema.", 1),
            CreateChoiceTask("Was ist ein gesundes Essen für die Pause?", "Apfel", ["Bonbon", "Limo", "Kuchen"], "Gesund leben", "Richtig. Das passt gut zum Sachkunde-Thema.", 1),
            CreateChoiceTask("Was trinkst du am besten bei Durst?", "Wasser", ["Cola", "Brause", "Limo"], "Gesund leben", "Richtig. Das passt gut zum Sachkunde-Thema.", 1),
            CreateChoiceTask("Was gehört zu einem gesunden Frühstück?", "Brot", ["Bonbon", "Lolli", "Chips"], "Gesund leben", "Richtig. Das passt gut zum Sachkunde-Thema.", 1),
            CreateChoiceTask("Wann scheint oft der Mond?", "in der Nacht", ["am Mittag", "am Morgen", "in der Pause"], "Tag und Nacht", "Richtig. Das passt gut zum Sachkunde-Thema.", 1),
            CreateChoiceTask("Wann geht die Sonne auf?", "am Morgen", ["in der Nacht", "am Abend", "um Mitternacht"], "Tag und Nacht", "Richtig. Das passt gut zum Sachkunde-Thema.", 1),
            CreateChoiceTask("Wann wird es draußen dunkel?", "am Abend", ["am Morgen", "am Mittag", "in der Pause"], "Tag und Nacht", "Richtig. Das passt gut zum Sachkunde-Thema.", 1)
        ];
    }

    private static List<LearningTask> BuildClass1WorldTasks()
    {
        return
        [
            CreateChoiceTask("Was brauchst du bei Regen?", "Regenschirm", ["Sonnenbrille", "Badesachen", "Schlitten"], "Wetter", "Prima. Das passt zum Wetter.", 1),
            CreateChoiceTask("Was brauchst du im Winter draußen?", "Mütze", ["Sandalen", "Badehose", "Sonnencreme"], "Wetter", "Prima. Das passt zum Wetter.", 1),
            CreateChoiceTask("Was passt zu einem heißen Sommertag?", "kurze Kleidung", ["dicker Schal", "Handschuhe", "Winterjacke"], "Wetter", "Prima. Das passt zum Wetter.", 1),
            CreateChoiceTask("Womit kannst du riechen?", "mit der Nase", ["mit dem Knie", "mit der Hand", "mit dem Fuß"], "Sinne", "Super. Du kennst den passenden Sinn.", 1),
            CreateChoiceTask("Womit kannst du hören?", "mit den Ohren", ["mit der Schulter", "mit dem Mund", "mit dem Bauch"], "Sinne", "Super. Du kennst den passenden Sinn.", 1),
            CreateChoiceTask("Womit kannst du sehen?", "mit den Augen", ["mit den Zehen", "mit dem Ellenbogen", "mit den Knien"], "Sinne", "Super. Du kennst den passenden Sinn.", 1),
            CreateChoiceTask("Was braucht eine Pflanze zum Wachsen?", "Wasser", ["Limo", "Kakao", "Saft"], "Pflanzen", "Gut gemacht. Pflanzen brauchen Wasser.", 1),
            CreateChoiceTask("Was braucht eine Pflanze außer Wasser?", "Licht", ["Fernseher", "Kissen", "Schuhe"], "Pflanzen", "Gut gemacht. Pflanzen brauchen Licht.", 1),
            CreateChoiceTask("Was wächst aus einem Samen?", "eine Pflanze", ["ein Schuh", "ein Teller", "ein Auto"], "Pflanzen", "Gut gemacht. Aus Samen wachsen Pflanzen.", 1),
            CreateChoiceTask("Was machst du vor dem Essen?", "Hände waschen", ["im Schlamm spielen", "Schuhe verstecken", "Fernsehen"], "Alltag", "Richtig. Das ist gesund im Alltag.", 1),
            CreateChoiceTask("Was tust du, wenn du müde bist?", "schlafen", ["rennen", "schwimmen", "hüpfen"], "Alltag", "Richtig. Das passt gut zum Alltag.", 1),
            CreateChoiceTask("Was tust du, wenn du traurig bist?", "mit jemandem sprechen", ["alles verstecken", "nie mehr trinken", "die Jacke essen"], "Alltag", "Richtig. Das ist ein guter Weg im Alltag.", 1)
        ];
    }

    private static List<LearningTask> BuildClass2PortalTasks()
    {
        return
        [
            CreateChoiceTask("Welche Ampelfarbe bedeutet Stopp?", "Rot", ["Grün", "Blau", "Gelb"], "Verkehr", "Richtig. Rot bedeutet Stopp.", 2),
            CreateChoiceTask("Womit schützt du deinen Kopf beim Fahrradfahren?", "mit einem Helm", ["mit einer Mütze", "mit einer Brille", "mit einem Schal"], "Verkehr", "Super. Beim Fahrradfahren schützt ein Helm.", 2),
            CreateChoiceTask("Was gehört in die Papiertonne?", "Zeitung", ["Apfelschale", "Plastikflasche", "Banane"], "Mülltrennung", "Prima. Papier gehört zur Papiertonne.", 2),
            CreateChoiceTask("Welches Tier hält Winterschlaf?", "Igel", ["Huhn", "Schaf", "Ente"], "Tiere", "Richtig. Der Igel hält Winterschlaf.", 2),
            CreateChoiceTask("In welcher Jahreszeit werden viele Früchte reif?", "im Herbst", ["im Winter", "im Frühling", "in der Nacht"], "Jahreszeiten", "Richtig. Viele Früchte werden im Herbst reif.", 2),
            CreateChoiceTask("Was braucht eine Pflanze außer Wasser besonders?", "Licht", ["Kissen", "Fernseher", "Schuhe"], "Pflanzen", "Gut gemacht. Pflanzen brauchen Licht.", 2),
            CreateChoiceTask("Welches Körperteil benutzt du zum Hören?", "die Ohren", ["die Knie", "die Füße", "die Ellbogen"], "Körper", "Super. Mit den Ohren hörst du.", 2),
            CreateChoiceTask("Was zeigt dir auf der Uhr, dass die Schule beginnt?", "eine feste Uhrzeit", ["die Farbe der Tafel", "die Länge des Flurs", "das Wetter"], "Zeit", "Richtig. Eine Uhrzeit hilft dir beim Tagesablauf.", 2),
            CreateChoiceTask("Was ist gesund für das Frühstück?", "Vollkornbrot", ["Lutscher", "Limo", "Chips"], "Gesund leben", "Prima. Ein gesundes Frühstück gibt Kraft.", 2),
            CreateChoiceTask("Welches Wetter passt zu einer Regenjacke?", "Regen", ["Sonnenschein", "Sternenhimmel", "Nebel im Zimmer"], "Wetter", "Richtig. Bei Regen hilft eine Regenjacke.", 2),
            CreateChoiceTask("Wo wächst eine Bohne zuerst?", "in der Erde", ["auf dem Tisch", "im Schuh", "an der Lampe"], "Pflanzen", "Richtig. Pflanzen wachsen in Erde.", 2),
            CreateChoiceTask("Wer hilft dir beim sicheren überqueren der Straße?", "die Ampel", ["der Mülleimer", "die Schultasche", "die Pausenglocke"], "Verkehr", "Gut gemacht. Die Ampel hilft im Straßenverkehr.", 2),
            CreateChoiceTask("Welches Tier lebt auf dem Bauernhof?", "Kuh", ["Hai", "Pinguin", "Wal"], "Tiere", "Richtig. Eine Kuh lebt auf dem Bauernhof.", 2),
            CreateChoiceTask("Was machst du vor dem Essen?", "Hände waschen", ["Schuhe ausziehen", "Bleistift spitzen", "Tafel wischen"], "Alltag", "Richtig. Das ist gesund im Alltag.", 2)
        ];
    }

    private static List<LearningTask> BuildClass2WorldTasks()
    {
        return
        [
            CreateChoiceTask("Welches Wetter passt zu einem Schlitten?", "Schnee", ["Sommerregen", "Sonnenschein im Zimmer", "Nebel in der Küche"], "Wetter", "Prima. Ein Schlitten passt zu Schnee.", 2),
            CreateChoiceTask("Welches Tier legt Eier?", "Huhn", ["Katze", "Hund", "Pferd"], "Tiere", "Richtig. Ein Huhn legt Eier.", 2),
            CreateChoiceTask("Was braucht eine Pflanze zum Wachsen?", "Wasser", ["Saft", "Limonade", "Kakao"], "Pflanzen", "Gut gemacht. Pflanzen brauchen Wasser.", 2),
            CreateChoiceTask("Welches Signal zeigt an der Ampel, dass du gehen darfst?", "Grün", ["Rot", "Braun", "Schwarz"], "Verkehr", "Richtig. Grün bedeutet, dass du gehen darfst.", 2),
            CreateChoiceTask("Was schützt dich bei starkem Regen?", "Regenschirm", ["Sonnenbrille", "Trommel", "Fächer"], "Wetter", "Prima. Ein Regenschirm schützt bei Regen.", 2),
            CreateChoiceTask("Welcher Müll gehört in die Biotonne?", "Apfelschale", ["Konservendose", "Glasflasche", "Zeitung"], "Mülltrennung", "Prima. Eine Apfelschale gehört in die Biotonne.", 2),
            CreateChoiceTask("Womit riechst du?", "mit der Nase", ["mit dem Knie", "mit der Schulter", "mit dem Fuß"], "Sinne", "Super. Die Nase hilft beim Riechen.", 2),
            CreateChoiceTask("Womit siehst du?", "mit den Augen", ["mit den Händen", "mit den Füßen", "mit dem Rücken"], "Sinne", "Super. Die Augen helfen beim Sehen.", 2),
            CreateChoiceTask("Was passiert oft im Frühling?", "Blumen blühen", ["Blätter fallen", "Schneemänner wachsen", "Eis friert im Zimmer"], "Jahreszeiten", "Richtig. Im Frühling blühen viele Pflanzen.", 2),
            CreateChoiceTask("Wann ist meist Schlafenszeit?", "am Abend", ["am Frühstück", "in der Pause", "zur Hofpause"], "Tagesablauf", "Richtig. Viele Kinder gehen am Abend schlafen.", 2),
            CreateChoiceTask("Was ist gut für die Zähne?", "Zähne putzen", ["nur Limo trinken", "nie Wasser trinken", "Zucker naschen"], "Gesund leben", "Prima. Zähneputzen hält die Zähne gesund.", 2),
            CreateChoiceTask("Was hilft dir auf dem Schulweg sichtbar zu sein?", "helle Kleidung", ["versteckte Schuhe", "dunkler Sack", "geschlossene Augen"], "Verkehr", "Gut gemacht. Helle Kleidung macht dich sichtbarer.", 2),
            CreateChoiceTask("Welcher Beruf hilft bei Feuer?", "Feuerwehr", ["Bäckerei", "Bibliothek", "Musikschule"], "Berufe", "Richtig. Die Feuerwehr hilft bei Feuer.", 2),
            CreateChoiceTask("Wer hilft dir oft bei Fragen in der Schule?", "die Lehrkraft", ["der Mülleimer", "die Schere", "der Schuh"], "Schule", "Richtig. Die Lehrkraft unterstützt dich in der Schule.", 2)
        ];
    }

    private static List<LearningTask> BuildClass3PortalTasks()
    {
        return
        [
            CreateChoiceTask("Welche Energiequelle ist erneuerbar?", "Sonne", ["Kohle", "?l", "Benzin"], "Energie", "Richtig. Sonnenenergie ist erneuerbar.", 3),
            CreateChoiceTask("Welches Gerät nutzt Wind, um Strom zu erzeugen?", "Windrad", ["Toaster", "Lampe", "Wasserhahn"], "Technik", "Prima. Ein Windrad nutzt den Wind.", 3),
            CreateChoiceTask("Was brauchen Pflanzen außer Wasser besonders?", "Licht", ["Fernbedienung", "Schuhe", "Murmeln"], "Natur", "Richtig. Pflanzen brauchen Licht.", 3),
            CreateChoiceTask("Welches Tier gehört zu den Insekten?", "Biene", ["Katze", "Amsel", "Schaf"], "Tiere", "Super. Die Biene ist ein Insekt.", 3),
            CreateChoiceTask("Welcher Stoff zieht einen Magneten an?", "Eisen", ["Holz", "Papier", "Stoff"], "Technik", "Richtig. Eisen wird von Magneten angezogen.", 3),
            CreateChoiceTask("Was ist wichtig, wenn du im Internet mit anderen schreibst?", "keine privaten Daten verraten", ["jeden Fremden einladen", "Passwörter weitergeben", "alles sofort anklicken"], "Sicher unterwegs", "Prima. Private Daten schützt man im Netz.", 3),
            CreateChoiceTask("Welches Wetter entsteht oft vor einem Gewitter?", "dunkle Wolken", ["Schnee im Sommer", "trockene Zimmerluft", "Lichter an der Decke"], "Wetter", "Richtig. Dunkle Wolken kündigen oft ein Gewitter an.", 3),
            CreateChoiceTask("Wofür brauchst du eine Lupe?", "zum Vergrößern", ["zum Kühlen", "zum Löschen", "zum Hören"], "Forschen", "Gut gemacht. Eine Lupe vergrößert Dinge.", 3),
            CreateChoiceTask("Was ist gut für die Umwelt?", "Müll trennen", ["Licht unnötig anlassen", "Wasser laufen lassen", "Batterien in den Wald werfen"], "Umwelt", "Richtig. Müll trennen hilft der Umwelt.", 3),
            CreateChoiceTask("Welches Organ pumpt Blut durch den Körper?", "Herz", ["Lunge", "Magen", "Fuß"], "Körper", "Richtig. Das Herz pumpt das Blut.", 3),
            CreateChoiceTask("Welches Tier hält Winterruhe?", "Bär", ["Huhn", "Fisch", "Schaf"], "Tiere", "Prima. Ein Bär hält Winterruhe.", 3),
            CreateChoiceTask("Was zeigt dir eine Wetterkarte?", "wie das Wetter wird", ["wie lang ein Text ist", "wie schwer ein Stein ist", "wie ein Lied klingt"], "Wetter", "Richtig. Eine Wetterkarte zeigt Wetterdaten.", 3)
        ];
    }

    private static List<LearningTask> BuildClass3WorldTasks()
    {
        return
        [
            CreateChoiceTask("Welches Material schwimmt oft auf Wasser?", "Holz", ["Stein", "Metallkugel", "Glasplatte"], "Forschen", "Prima. Holz schwimmt oft auf Wasser.", 3),
            CreateChoiceTask("Was passiert mit Eis, wenn es warm wird?", "es schmilzt", ["es wächst", "es brennt", "es bellt"], "Natur", "Richtig. Eis schmilzt bei Wärme.", 3),
            CreateChoiceTask("Welches Gerät misst die Temperatur?", "Thermometer", ["Lineal", "Schere", "Kompass"], "Technik", "Richtig. Ein Thermometer misst die Temperatur.", 3),
            CreateChoiceTask("Welche Mülltonne ist oft für Papier?", "die blaue Tonne", ["die Badewanne", "die Brotdose", "die rote Socke"], "Umwelt", "Prima. Papier kommt meist in die blaue Tonne.", 3),
            CreateChoiceTask("Wofür ist ein Stromkreis nötig?", "damit eine Lampe leuchtet", ["damit Wasser friert", "damit Pflanzen schlafen", "damit ein Heft singt"], "Technik", "Richtig. Eine Lampe braucht einen geschlossenen Stromkreis.", 3),
            CreateChoiceTask("Was ist ein Raubtier?", "Fuchs", ["Schaf", "Kuh", "Kaninchen"], "Tiere", "Richtig. Ein Fuchs jagt andere Tiere.", 3),
            CreateChoiceTask("Was schützt deine Haut an sonnigen Tagen?", "Sonnencreme", ["Schneeschaufel", "Wintermütze", "Kopfhörer"], "Gesundheit", "Prima. Sonnencreme schützt die Haut.", 3),
            CreateChoiceTask("Warum spart man Wasser?", "damit genug Wasser da bleibt", ["damit Tische trockener werden", "damit Bücher singen", "damit Wolken verschwinden"], "Umwelt", "Richtig. Wasser ist wertvoll und soll nicht verschwendet werden.", 3),
            CreateChoiceTask("Was macht ein Kompass?", "er zeigt die Himmelsrichtung", ["er misst Zeit", "er zählt Wörter", "er macht Musik"], "Orientierung", "Super. Ein Kompass hilft bei der Orientierung.", 3),
            CreateChoiceTask("Welche Technik hilft bei einem Notruf?", "Telefon", ["Buntstift", "Radiergummi", "Turnschuh"], "Technik", "Richtig. Ein Telefon hilft bei einem Notruf.", 3),
            CreateChoiceTask("Was braucht eine Pflanze für die Fotosynthese?", "Licht", ["Kekse", "Sandspielzeug", "Fernseher"], "Natur", "Prima. Pflanzen brauchen Licht für die Fotosynthese.", 3),
            CreateChoiceTask("Was ist im Straßenverkehr sicher?", "bei Grün gehen", ["bei Rot rennen", "ohne zu schauen fahren", "zwischen Autos spielen"], "Sicherheit", "Richtig. Bei Grün und mit Blick nach links und rechts ist es sicherer.", 3)
        ];
    }

    private static List<LearningTask> BuildClass4PortalTasks()
    {
        return
        [
            CreateChoiceTask("Welche Energiequelle ist erneuerbar?", "Wind", ["Kohle", "Diesel", "Benzin"], "Energie", "Richtig. Wind ist erneuerbar.", 4),
            CreateChoiceTask("Was hilft der Umwelt im Alltag?", "Müll vermeiden", ["Licht immer brennen lassen", "Papier wegwerfen", "Wasser unnutz laufen lassen"], "Umwelt", "Prima. Müll vermeiden hilft der Umwelt.", 4),
            CreateChoiceTask("Welches Organ hilft dir beim Atmen?", "Lunge", ["Herz", "Knie", "Lebertran"], "Körper", "Richtig. Die Lunge gehört zur Atmung.", 4),
            CreateChoiceTask("Was ist bei starkem Gewitter am sichersten?", "im Haus bleiben", ["unter einen einzelnen Baum gehen", "ins offene Feld laufen", "einen Metallzaun anfassen"], "Wetter", "Super. Im Haus bist du bei Gewitter besser geschuetzt.", 4),
            CreateChoiceTask("Wofür nutzt man einen Kompass?", "zum Finden der Himmelsrichtung", ["zum Wiegen von Obst", "zum Messen der Lautstärke", "zum Zählen von Schritten"], "Orientierung", "Richtig. Ein Kompass zeigt die Himmelsrichtung.", 4),
            CreateChoiceTask("Warum trennt man Müll?", "damit Stoffe wiederverwendet werden können", ["damit der Müll lauter klingt", "damit alles gleich schwer wird", "damit Tonnen unsichtbar werden"], "Umwelt", "Prima. Recycling braucht getrennte Stoffe.", 4),
            CreateChoiceTask("Was gehört zu einer sicheren Internetnutzung?", "keine Passwörter weitergeben", ["jedem Fremden schreiben", "alles ungeprüft anklicken", "den echten Namen immer öffentlich posten"], "Medien", "Richtig. Passwörter bleiben privat.", 4),
            CreateChoiceTask("Welcher Beruf hilft bei einem kaputten Stromkreis?", "Elektrikerin oder Elektriker", ["Bäckerin oder Bäcker", "Gärtnerin oder Gärtner", "Pilotin oder Pilot"], "Berufe", "Gut gemacht. Dieser Beruf arbeitet mit Strom.", 4),
            CreateChoiceTask("Was passiert mit Wasser bei 0 Grad?", "es kann gefrieren", ["es wird zu Holz", "es verschwindet sofort", "es wird immer zu Dampf"], "Natur", "Richtig. Bei 0 Grad kann Wasser gefrieren.", 4),
            CreateChoiceTask("Womit misst man die Temperatur?", "mit einem Thermometer", ["mit einem Lineal", "mit einem Löffel", "mit einer Schere"], "Forschen", "Prima. Ein Thermometer misst Temperatur.", 4),
            CreateChoiceTask("Was ist wichtig, wenn du eine Pflanze beobachtest?", "regelmäßig schauen und notieren", ["nur einmal kurz ansehen", "sie im Dunkeln vergessen", "sie ohne Wasser lassen"], "Forschen", "Stark. Beobachten braucht Regelm??igkeit.", 4),
            CreateChoiceTask("Warum spart man Strom?", "um Energie und Ressourcen zu schonen", ["damit Lampen schneller tanzen", "damit Heizungen singen", "damit Stecker wachsen"], "Energie", "Richtig. Stromsparen schont Ressourcen.", 4)
        ];
    }

    private static List<LearningTask> BuildClass4WorldTasks()
    {
        return
        [
            CreateChoiceTask("Welche Aussage zu Sonnenenergie stimmt?", "Solarzellen können Strom aus Sonnenlicht gewinnen", ["Solarzellen machen nur Wind", "Solarzellen funktionieren nur nachts", "Solarzellen bestehen aus Papier"], "Energie", "Richtig. Solarzellen nutzen Sonnenlicht.", 4),
            CreateChoiceTask("Was ist für Tiere im Winter hilfreich?", "ruhige Rückzugsorte", ["laute Musik im Wald", "Plastik im Gebüsch", "offene Feuerstellen"], "Tiere", "Prima. Viele Tiere brauchen Ruhe und Schutz.", 4),
            CreateChoiceTask("Welche Tonne ist oft für Glas?", "der Altglascontainer", ["die Biotonne", "der Schuhschrank", "die Brotdose"], "Recycling", "Richtig. Glas wird meist extra gesammelt.", 4),
            CreateChoiceTask("Warum lüftet man Klassenräume regelmäßig?", "damit frische Luft hineinkommt", ["damit Tafeln kleiner werden", "damit Stühle rollen", "damit Bücher summen"], "Gesundheit", "Gut gemacht. Frische Luft ist wichtig.", 4),
            CreateChoiceTask("Was zeigt eine Wetterkarte?", "wie sich das Wetter entwickelt", ["wie lang ein Lied ist", "wie schwer ein Buch ist", "wie hoch ein Baum singt"], "Wetter", "Richtig. Eine Wetterkarte gibt Wetterinformationen.", 4),
            CreateChoiceTask("Was ist bei einem Notruf wichtig?", "klar sagen, was passiert ist", ["sofort auflegen", "nur lachen", "den Ort verschweigen"], "Sicherheit", "Prima. Ein Notruf braucht klare Informationen.", 4),
            CreateChoiceTask("Wozu dient ein Stromkreis?", "damit Strom fließen kann", ["damit Wasser friert", "damit Schuhe wachsen", "damit Wolken malen"], "Technik", "Richtig. Ein geschlossener Stromkreis lässt Strom fließen.", 4),
            CreateChoiceTask("Was ist eine nachhaltige Handlung?", "eine Trinkflasche mehrfach benutzen", ["alles nach einmaligem Nutzen wegwerfen", "bei Licht schlafen", "Batterien in den Wald werfen"], "Nachhaltigkeit", "Super. Mehrfach nutzen ist nachhaltiger.", 4),
            CreateChoiceTask("Wie kann man Wasser sparen?", "beim Zähneputzen den Hahn zudrehen", ["länger laufen lassen", "mit Wasser spielen während man wartet", "immer zwei Hähne öffnen"], "Umwelt", "Richtig. So sparst du Wasser.", 4),
            CreateChoiceTask("Welche Beobachtung passt zu einem Schatten?", "Er verändert sich mit dem Sonnenstand", ["Er bleibt immer gleich lang", "Er leuchtet nachts selbst", "Er riecht nach Regen"], "Natur", "Prima. Schatten verändern sich mit dem Licht.", 4),
            CreateChoiceTask("Was hilft bei einem Schulweg im Dunkeln?", "helle oder reflektierende Kleidung", ["geschlossene Augen", "leise Schuhe", "ein leerer Rucksack"], "Verkehr", "Richtig. Helle Kleidung macht sichtbarer.", 4),
            CreateChoiceTask("Was bedeutet demokratisch abstimmen?", "gemeinsam entscheiden und Stimmen zählen", ["eine Person bestimmt immer alles", "niemand darf etwas sagen", "es wird gewürfelt statt abgestimmt"], "Zusammenleben", "Gut gemacht. So funktioniert eine Abstimmung.", 4)
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
            Subject = LearningSubject.Sachunterricht,
            ClassLevel = classLevel
        };
    }
}
