﻿﻿# Chat-Dokumentation: SpieleLernApp

## Zweck
Diese Datei dokumentiert den Projekt-Chat, Entscheidungen und Arbeitsschritte fuer die Windows-SpieleLernApp.

## Dokumentationsregel
Ab diesem Punkt wird jeder groessere Schritt im Projekt in dieser Datei festgehalten:
- Anfrage oder Ziel
- durchgefuehrte Aktion
- Ergebnis
- naechster Schritt

## Verlauf

### 2026-04-22 - Projektordner ermittelt
- Anfrage: Der Speicherort des Projekts sollte angezeigt werden.
- Aktion: Der aktuelle Arbeitsordner wurde geprueft.
- Ergebnis: Zunaechst wurde `C:\Users\Kolkh\Downloads\SpieleLernApp` verwendet.
- Hinweis: Dieser Pfad war spaeter nicht mehr gueltig.

### 2026-04-22 - Neuer Projektordner gefunden
- Anfrage: Der aktuelle Ordner sollte erneut geprueft werden.
- Aktion: Es wurde vom Benutzerverzeichnis aus nach dem Ordner `SpieleLernApp` gesucht.
- Ergebnis: Der gueltige Projektordner ist `C:\Users\Kolkh\Documents\SpieleLernApp`.
- Naechster Schritt: Weitere Arbeit auf diesen Ordner ausrichten.

### 2026-04-22 - Projektkontext auf neuen Ordner gesetzt
- Anfrage: Der Chat sollte dem neuen Ordner zugewiesen werden.
- Aktion: Der Projektordner wurde geprueft und als Arbeitsbasis bestaetigt.
- Ergebnis: `C:\Users\Kolkh\Documents\SpieleLernApp` ist die Projektbasis.
- Zusatz: Im Ordner war bereits der Unterordner `Bilder` vorhanden.

### 2026-04-22 - Vorhandene Dateien gesichtet
- Anfrage: Die SpieleLernApp sollte geplant werden.
- Aktion: Der Projektordner und die vorhandenen Dateien wurden eingelesen.
- Ergebnis: Es wurden folgende Bilddateien gefunden:
  - `Bilder\Klasse 1.png`
  - `Bilder\klasse 2.png`
  - `Bilder\Klasse 3#.png`
  - `Bilder\KLasse 4.png`
  - `Bilder\Lumos.png`
  - `Bilder\neutraler Ort.png`
- Naechster Schritt: Diese Bilder koennen fuer Startseite, Klassenwahl und Spieloberflaechen verwendet werden.

### 2026-04-22 - Erste App-Planung
- Anfrage: Die App sollte geplant werden.
- Aktion: Es wurde ein erster inhaltlicher und technischer Vorschlag erstellt.
- Ergebnis:
  - Zielbild: Windows-SpieleLernApp fuer Grundschule, Klassen 1 bis 4
  - Start mit Klassenwahl, Lernbereichen und einfachen Minispielen
  - MVP-Vorschlag mit Startseite, Klassenwahl, Fachauswahl, Quiz, Punkte/Sterne und lokaler Speicherung
  - Technikempfehlung: `WPF mit .NET 8`
- Naechster Schritt: Seitenstruktur, Datenmodell und Projektgeruest festlegen.

### 2026-04-22 - Vollstaendige Chat-Dokumentation angefordert
- Anfrage: Der Chat soll komplett mit jedem Schritt dokumentiert werden.
- Aktion: Diese Dokumentationsdatei wurde erstellt.
- Ergebnis: Ab jetzt wird die Zusammenarbeit fortlaufend in `CHAT_DOKUMENTATION.md` erfasst.
- Naechster Schritt: Die weitere App-Planung direkt hier mitprotokollieren.

### 2026-04-22 - Neues Bild im Bilder-Ordner erkannt
- Anfrage: Es wurde darauf hingewiesen, dass im Bilder-Ordner ein weiteres Bild angehaengt wurde.
- Aktion: Der Ordner `Bilder` wurde erneut eingelesen und nach dem neuesten Eintrag sortiert.
- Ergebnis: Das neue Bild ist `Bilder\Ladebild.png`.
- Zusatz: Die Dateigroesse betraegt `2.088.946` Bytes, letzter Aenderungszeitpunkt `22.04.2026 18:03:41`.
- Naechster Schritt: Das Bild kann als Ladebildschirm, Startbild oder Splash-Screen der App eingeplant werden.

### 2026-04-22 - Planungsgrundlage fuer benoetigte Informationen erstellt
- Anfrage: Es sollte geklaert werden, welche Informationen fuer die App-Planung benoetigt werden.
- Aktion: Eine strukturierte Planungsdatei mit den benoetigten Themen und empfohlenen Startwerten wurde angelegt.
- Ergebnis: Die Datei `PLANUNG_ANFORDERUNGEN.md` beschreibt Zielgruppe, Lernziele, Spielarten, Ablauf, Design, Belohnungssystem, Inhalte, Sprache, Technik und MVP-Umfang.
- Naechster Schritt: Die offenen Kernentscheidungen gemeinsam festlegen und danach die Seitenstruktur der App planen.

### 2026-04-22 - NRW-Lehrplan Primarstufe als Planungsbasis aufgenommen
- Anfrage: Es wurde vorgeschlagen, den Lehrplan NRW fuer die Primarstufe in die App-Planung einzubeziehen.
- Aktion: Die offiziellen NRW-Quellen wurden geprueft und in einer eigenen Planungsdatei zusammengefasst.
- Ergebnis: Die Datei `NRW_LEHRPLAN_INTEGRATION.md` beschreibt, wie die App entlang der offiziellen Kompetenzbereiche fuer Mathematik, Deutsch und Sachunterricht aufgebaut werden kann.
- Zusatz: Als aktuelle Planungsbasis wurden der Lehrplannavigator Primarstufe, die Richtlinien 2024 mit Inkrafttreten zum `01.08.2025` sowie der Sammelband mit Stand `18.03.2025` herangezogen.
- Naechster Schritt: Fach und Kompetenzbereich fuer Version 1 festlegen.

### 2026-04-22 - Startablauf mit Ladebildschirm und Spielerabfrage angelegt
- Anfrage: Beim Laden sollte ein sichtbarer Ladebildschirm mit `Ladebild` erscheinen, dazu ein Ladebalken bis `100 %`, anschliessend eine Abfrage fuer Spielername und Alter und danach das Startmenue mit `neutraler Ort`.
- Aktion: Ein erstes WPF-Projektgeruest mit `StartupWindow`, `MainMenuWindow`, `PlayerProfile` und Projektdatei wurde angelegt.
- Ergebnis:
  - `Ladebild.png` wird als Start-Hintergrund geladen
  - ein sichtbarer Ladebalken waechst bis `100 %`
  - danach erscheint ein Formular fuer Spielername und Alter
  - nach `OK` wird das Startmenue mit `neutraler Ort.png` geoeffnet
- Zusatz: Die Bildladung wurde ueber Dateipfade aus dem Ordner `Bilder` umgesetzt, damit auch Dateinamen mit Leerzeichen stabil eingebunden werden koennen.
- Hinweis: Im aktuellen System ist kein .NET-SDK installiert, daher konnte die App noch nicht lokal gebaut oder gestartet werden.
- Naechster Schritt: SDK installieren oder Build-Umgebung herstellen und anschliessend Klassenwahl und Menues erweitern.

### 2026-04-22 - Fehlende .NET-Komponenten auf Laufwerk G installiert
- Anfrage: Alle fuer das Projekt fehlenden Module sollten auf dem Rechner auf Laufwerk `G:` installiert werden.
- Annahme: Gemeint waren die fuer die Windows-SpieleLernApp fehlenden Entwicklungs- und Laufzeitkomponenten.
- Aktion:
  - Das offizielle Microsoft-Installationsskript fuer .NET wurde heruntergeladen.
  - Das `.NET 8 SDK` wurde nach `G:\dotnet` installiert.
  - `DOTNET_ROOT` wurde fuer das Benutzerprofil auf `G:\dotnet` gesetzt.
  - `G:\dotnet` wurde dem Benutzer-`Path` hinzugefuegt.
  - Das Projekt wurde anschliessend mit dem neuen SDK gebaut.
- Ergebnis:
  - Installiertes SDK: `8.0.420`
  - Installierte Runtimes: `Microsoft.NETCore.App 8.0.26`, `Microsoft.WindowsDesktop.App 8.0.26`, `Microsoft.AspNetCore.App 8.0.26`
  - Der Build der Datei `SpieleLernApp.csproj` wurde erfolgreich mit `0` Fehlern und `0` Warnungen ausgefuehrt.
- Zusatz: Zur vereinfachten Nutzung wurden die Skripte `build-app.ps1` und `start-app.ps1` angelegt.
- Naechster Schritt: App starten und den Ladeablauf visuell pruefen, danach Klassenwahl und weitere Menues aufbauen.

### 2026-04-22 - NRW-Faecher fuer Klasse 1 zusammengefasst
- Anfrage: Es sollte geklaert werden, welche Faecher laut Lernplan fuer Klasse 1 benoetigt werden.
- Aktion: Die offiziellen NRW-Quellen zur Primarstufe wurden auf die fuer Klasse 1 relevanten Faecher heruntergebrochen und in einer eigenen Datei festgehalten.
- Ergebnis: Die Datei `NRW_KLASSE1_FAECHER.md` beschreibt die fuer Klasse 1 wichtigen Faecher und die Prioritaet fuer die App.
- Kernaussage:
  - zentrale Startfaecher fuer die App: `Deutsch`, `Mathematik`, `Sachunterricht`
  - weitere relevante Faecher der Primarstufe: `Kunst`, `Musik`, `Sport`
  - `Englisch` soll nicht als Klasse-1-Startfach geplant werden, da es in NRW laut offizieller Mitteilung ab `Klasse 3` beginnt
- Naechster Schritt: Die Klasse-1-Inhalte pro Fach in konkrete Spielmodule uebersetzen.

### 2026-04-22 - Startmenue interaktiv erweitert
- Anfrage: Das Startmenue sollte Stueck fuer Stueck ausgebaut werden. Die Punkte `Mein Profil`, `Meine Belohnungen`, `Tagebuch`, `Tagesziel`, `Lernportal`, `Deine Welten` und `Die Truhe` sollten auf dem Bild interaktiv sein.
- Aktion:
  - Das Startmenue wurde als interaktive Bildflaeche mit klickbaren Bereichen umgebaut.
  - Eine seitliche Detailflaeche fuer die Inhalte der einzelnen Menuepunkte wurde angelegt.
  - Eine neue `ClassSelectionWindow` wurde erstellt und an `Lernportal` sowie `Deine Welten` angebunden.
  - Fuer das Profil wurde eine Spielerbild-Auswahl integriert.
- Ergebnis:
  - `Mein Profil` zeigt Name, Alter und Spielerbild
  - `Meine Belohnungen` zeigt Sterne
  - `Tagebuch` zeigt freigeschaltete oder vorbereitete Bereiche
  - `Tagesziel` zeigt eine taeglich wechselnde Aufgabe
  - `Lernportal` und `Deine Welten` oeffnen die Klassenauswahl
  - `Die Truhe` steuert Soundstatus und Vollbild
- Zusatz: Es wurde zusaetzlich das neue Bild `Klassenauswahl.png` eingebunden.
- Naechster Schritt: Das neue Menue lokal bauen und testen, danach die eigentlichen Lernbereiche pro Klasse anschliessen.

### 2026-04-22 - Schliessen der App nach Spielerabfrage behoben
- Anfrage: Nach Eingabe von Name und Alter wurde das Programm geschlossen.
- Ursache: Das erste Fenster wurde von WPF weiterhin als Hauptfenster behandelt. Beim Schliessen des Startfensters wurde dadurch die gesamte App beendet.
- Aktion: Vor dem Oeffnen des Startmenues wurde das neue Fenster explizit als `MainWindow` gesetzt.
- Ergebnis: Nach `OK` bleibt die App geoeffnet und wechselt korrekt ins Startmenue.
- Naechster Schritt: Build ausfuehren und den Ablauf erneut testen.

### 2026-04-22 - App-Start robuster umgestellt
- Anfrage: Das Fenster war erneut geschlossen, obwohl der vorherige Fix bereits gesetzt war.
- Vermutung: Das automatische `StartupUri`-Verhalten von WPF war noch zu eng an den ersten Fensterstart gekoppelt.
- Aktion:
  - Der App-Start wurde von `StartupUri` auf einen expliziten Start in `App.xaml.cs` umgestellt.
  - Die App startet jetzt zuerst mit `ShutdownMode.OnExplicitShutdown`.
  - Beim Wechsel ins Startmenue wird danach bewusst auf `ShutdownMode.OnMainWindowClose` umgestellt.
  - Zusaetzlich wurde eine einfache Fehlerprotokollierung in `app-error.log` eingebaut.
- Ergebnis: Der Lebenszyklus der Fenster ist jetzt explizit gesteuert und sollte beim Wechsel vom Ladefenster ins Startmenue stabil bleiben.
- Naechster Schritt: Neu bauen und den Ablauf erneut pruefen.

### 2026-04-22 - Absturz im Startmenue durch Einstellungsbereich analysiert und behoben
- Anfrage: Trotz neuem Startmechanismus erschien weiterhin ein Fehlerdialog und das Fenster schloss sich.
- Aktion: Die Datei `app-error.log` wurde ausgelesen und der Stacktrace untersucht.
- Ursache: Im `MainMenuWindow` wurden `CheckBox`-Events bereits waehrend `InitializeComponent()` ausgeloest. Dabei griff `BuildSettingsText()` zu frueh auf noch nicht vollstaendig initialisierte Steuerelemente zu.
- Behebung:
  - Zugriff auf Einstellungssteuerelemente null-sicher gemacht
  - Statusaktualisierung des Einstellungsbereichs in eine abgesicherte Hilfsmethode verlagert
  - Sternsymbol-Ausgabe zusaetzlich auf ein sauberes Unicode-Zeichen korrigiert
- Ergebnis: Das Startmenue sollte nun ohne diesen Initialisierungsabsturz geladen werden.
- Naechster Schritt: Neu bauen und den Wechsel von Spielerabfrage ins Startmenue erneut testen.

### 2026-04-22 - Hotspots auf echte Beschilderungen im Bild angepasst
- Anfrage: Die interaktiven Bereiche sollten so wie auf dem Bild `neutraler Ort` ausgerichtet werden. Die Boxen sollten unsichtbar sein und nur auf der jeweiligen Beschilderung liegen.
- Aktion:
  - Das Startmenue wurde auf ein bildnahes Layout umgebaut.
  - Sichtbare Menueboxen auf der Landschaft wurden entfernt.
  - Es wurden transparente Hotspots direkt auf `Deine Welten`, `Lernportal`, `Mein Profil`, `Meine Belohnungen`, `Tagebuch`, `Tagesziel` und die Truhe gelegt.
  - Die Detailansicht wurde als separate halbtransparente Flaeche am unteren Fensterrand umgesetzt.
  - `Lernportal` und `Deine Welten` oeffnen jetzt direkt die Klassenauswahl.
- Ergebnis: Das Startbild bleibt visuell sauber, waehrend die Interaktion jetzt naeher an den echten Schildern und Objekten des Bildes liegt.
- Zusatz: Der neue Stand wurde erfolgreich mit `0` Fehlern und `0` Warnungen gebaut.
- Naechster Schritt: App starten und die Positionen der unsichtbaren Hotspots visuell mit dem Bild abgleichen.

### 2026-04-22 - UTF-8 und Hotspot-Feinjustierung umgesetzt
- Anfrage:
  - Umlaute sollen korrekt ueber UTF-8 angezeigt werden.
  - Der Truhen-Button lag unterhalb der Truhe und sollte direkt darauf liegen.
  - Der Lernportal-Button lag auf dem Schild `Mein Profil` und sollte weiter nach links in das blaue Portal verschoben werden.
- Aktion:
  - Die Projektdatei wurde auf `CodePage 65001` gesetzt.
  - Sichtbare UI-Texte wurden auf echte Umlaute umgestellt.
  - Der `TreasureHotspot` wurde auf die Truhe im unteren linken Bereich verschoben.
  - Der `LearningPortalHotspot` wurde vom rechten Schildbereich auf die blaue Portalzone verschoben.
  - Die rechten Hotspots wurden insgesamt an die sichtbaren Schilder im Bild angenaehert.
- Ergebnis:
  - UTF-8 wird nun fuer den Build explizit verwendet.
  - Umlaute sollen in der Oberflaeche korrekt erscheinen.
  - Truhe und Lernportal liegen naeher an ihren echten Bildpositionen.
- Zusatz: Der aktualisierte Stand wurde erfolgreich mit `0` Fehlern und `0` Warnungen gebaut.
- Naechster Schritt: App neu starten und die korrigierten Positionen live pruefen.

### 2026-04-22 - Vollbildmodus um Escape-Taste erweitert
- Anfrage: Im Vollbildmodus soll die Taste `Esc` den Vollbildmodus wieder verlassen.
- Aktion: Im Hauptfenster wurde ein `KeyDown`-Handler hinterlegt, der bei `Esc` den gesetzten Vollbildmodus ueber die bestehende Checkbox wieder deaktiviert.
- Ergebnis: Wenn `Vollbild aktivieren` gesetzt ist, kann der Vollbildmodus jetzt mit `Esc` beendet werden.
- Naechster Schritt: Neu bauen und die Bedienung im laufenden Fenster pruefen.

### 2026-04-22 - Gespeicherte Spielerprofile und Auswahl beim Start eingebaut
- Anfrage: Spieler sollen gespeichert werden und beim Start des Programms zum Laden ausgewählt werden können. Zusätzlich soll weiterhin ein neuer Spieler angelegt werden können.
- Aktion:
  - Das Modell `PlayerProfile` wurde um `Id` und `LastPlayedAt` erweitert.
  - Ein neuer Speicherbaustein `PlayerProfileStore` wurde erstellt.
  - Spieler werden jetzt in `Data\players.json` gespeichert.
  - Der Startbildschirm wurde um eine Auswahl vorhandener Spieler erweitert.
  - Wenn noch keine Spieler vorhanden sind, erscheint direkt die Neuanlage.
  - Neue Spieler werden sofort gespeichert und danach geladen.
  - ?nderungen wie das Spielerbild werden aus dem Hauptmenü heraus ebenfalls wieder gespeichert.
- Ergebnis:
  - Beim Start können vorhandene Profile ausgewählt und geladen werden.
  - Es kann jederzeit ein neuer Spieler angelegt werden.
  - Spielerprofile bleiben zwischen Programmstarts erhalten.
- Zusatz: Der neue Stand wurde erfolgreich mit `0` Fehlern und `0` Warnungen gebaut.
- Naechster Schritt: App neu starten und den Auswahlablauf für gespeicherte Spieler prüfen.

### 2026-04-22 - Hotspots fest am Bild verankert und rechte Schilder neu ausgerichtet
- Anfrage: Die Buttons für `Mein Profil`, `Meine Belohnungen` und `Tagebuch` lagen nicht an der richtigen Position. Zusätzlich sollten die Positionen auch im Vollbild stabil bleiben und sich nicht verschieben.
- Aktion:
  - Das Hauptmenü wurde von einer frei skalierten Hintergrundfläche auf eine feste Szenenfläche mit `Viewbox` umgestellt.
  - Die gesamte Szene wird nun proportional als Einheit skaliert.
  - Dadurch bleiben die Hotspots relativ zum Bild fest verankert.
  - Die Hotspots für `Mein Profil`, `Meine Belohnungen`, `Tagebuch` und das `Lernportal` wurden neu positioniert.
  - Beschädigte Umlaute in den Menütexten wurden in den betroffenen Hauptfensterdateien bereinigt.
- Ergebnis:
  - Die Hotspots folgen jetzt der Bildgeometrie statt der Fenstergeometrie.
  - Vollbild und normale Fenstergröße sollten dieselben relativen Positionen verwenden.
  - Die rechten Schilder liegen näher an ihren tatsächlichen Bildpositionen.
- Naechster Schritt: Neu bauen, neu starten und die Hotspots live im Normal- und Vollbildmodus prüfen.

### 2026-04-22 - Klasse 1 Mathe im Lernportal als erster echter Lernbereich eingebaut
- Anfrage: Es sollte mit `Klasse 1` im `Lernportal` begonnen werden. Das erste Fach soll `Mathe` sein, inklusive erster Aufgaben.
- Aktion:
  - Ein neues Modell `MathTask` wurde erstellt.
  - Ein neuer Aufgabenlieferant `Class1MathTaskService` wurde angelegt.
  - Ein neues Fenster `Class1MathWindow` mit spielbaren Matheaufgaben wurde gebaut.
  - Die Klassenkarte `Klasse 1` im Lernportal oeffnet jetzt direkt diesen Mathebereich.
  - Richtige Antworten vergeben Sterne und speichern den Fortschritt im Spielerprofil.
- Ergebnis:
  - `Lernportal -> Klasse 1` fuehrt jetzt in einen ersten Mathebereich.
  - Dort gibt es mehrere Aufgaben zu Zahlen und Operationen.
  - Nach Abschluss werden Sterne zum Profil addiert und gespeichert.
- Zusatz: Der Stand wurde erfolgreich mit `0` Fehlern und `0` Warnungen gebaut und neu gestartet.
- Naechster Schritt: Fachauswahl innerhalb von Klasse 1 erweitern oder weitere Matheaufgaben und Schwierigkeitsstufen ausbauen.

### 2026-04-22 - Generator-Struktur fuer Faecher vorbereitet, Mathe als erste Umsetzung
- Anfrage: Es sollte ein Aufgaben-Generator fuer jedes Fach gebaut werden, beginnend mit Mathe.
- Aktion:
  - Die feste Mathe-Aufgabenliste wurde durch eine Generator-Architektur ersetzt.
  - Es wurden gemeinsame Modelle fuer Fach, Aufgaben und Generierungsanfragen angelegt.
  - Eine Generator-Schnittstelle sowie eine Factory fuer Faecher wurden erstellt.
  - `MathTaskGenerator` erzeugt jetzt dynamische Aufgaben fuer Klasse 1.
  - Der Mathebereich kann damit immer neue Aufgabenrunden erzeugen.
- Ergebnis:
  - Die Facharchitektur ist jetzt fuer weitere Generatoren vorbereitet.
  - Mathe ist als erster echter Generator umgesetzt.
  - Im Mathefenster kann nach Abschluss eine neue Runde mit neuen Aufgaben gestartet werden.
- Naechster Schritt: Neu bauen, starten und den Generator im Lernportal pruefen. Danach koennen wir den naechsten Fachgenerator anschliessen.

### 2026-04-22 - Fächerauswahl für Klasse 1 vorbereitet
- Anfrage: Für Klasse 1 sollte eine Fächerauswahl vorbereitet werden. Geplant sind `Mathematik`, `Deutsch`, `Sachkunde` und `Musik`.
- Aktion:
  - Ein neues Fenster `Class1SubjectSelectionWindow` wurde erstellt.
  - Der Weg wurde auf `Lernportal -> Klasse 1 -> Fächerauswahl` umgestellt.
  - `Mathematik` führt weiterhin in den bestehenden Mathebereich.
  - `Deutsch`, `Sachkunde` und `Musik` sind als vorbereitete Wege eingebaut und geben aktuell einen Hinweis zurück.
  - Die Bildpfade für spätere Fachbilder wurden vorbereitet.
- Ergebnis:
  - Klasse 1 besitzt jetzt eine echte Fächerauswahl.
  - Fachbilder können später unter `Bilder\Faecher\Klasse1\` abgelegt werden.
  - Die Facharchitektur ist damit auch in der Oberfläche vorbereitet.
- Zusatz: Der Stand wurde erfolgreich mit `0` Fehlern und `0` Warnungen gebaut und neu gestartet.
- Naechster Schritt: Fachbilder einfügen und anschließend den nächsten Fachgenerator, z. B. `Deutsch`, anschließen.

### 2026-04-22 - Navigation auf ein einziges Hauptfenster umgestellt
- Anfrage: Lernportal, Fächerauswahl und Aufgaben sollten komplett in einem Fenster bleiben, ohne dass weitere App-Fenster geöffnet werden.
- Aktion:
  - `MainMenuWindow` wurde um eingebettete Bereiche für `Klasse 1`, Fachauswahl, Platzhalterinhalte und `Mathematik` erweitert.
  - Die bisherigen ?ffnungen von `Class1SubjectSelectionWindow` und `Class1MathWindow` wurden aus dem Hauptmenü entfernt.
  - Klassen- und Fachhinweise erscheinen jetzt als interne Inhaltsflächen statt als zusätzliche Hinweisfenster.
  - Der Mathe-Generator läuft nun direkt im Detailbereich des Startmenüs.
  - Die sichtbaren Texte im Hauptfenster wurden gleichzeitig auf UTF-8 mit echten Umlauten umgestellt.
- Ergebnis:
  - Der komplette Weg `Lernportal -> Klasse 1 -> Mathematik` bleibt jetzt im selben Fenster.
  - Auch vorbereitete Klassen und Fächer öffnen keine zusätzlichen App-Fenster mehr.
  - Sterne, Fortschritt und Rücknavigation bleiben im Hauptfenster sichtbar.
- Zusatz: Der Stand wurde erfolgreich mit `0` Fehlern und `0` Warnungen gebaut.
- Naechster Schritt: App neu starten und den neuen Ein-Fenster-Ablauf direkt prüfen.

### 2026-04-22 - Mathe-Runde auf 100 Aufgaben erweitert und Formen entfernt
- Anfrage: Die Zahl der generierten Matheaufgaben sollte auf `100` erhöht werden. Gleichzeitig sollte der Aufgabentyp `Formen` entfernt werden.
- Aktion:
  - Die Rundengröße im Mathebereich wurde von `5` auf `100` Aufgaben angehoben.
  - Der Generator erzeugt jetzt nur noch Aufgaben aus `Addition`, `Subtraktion`, `Zahlenfolge` und `Vergleichen`.
  - Der bisherige Aufgabentyp `Formen` wurde aus dem Generator entfernt.
- Ergebnis:
  - Eine Mathe-Runde umfasst jetzt `100` Aufgaben.
  - Geometrie- beziehungsweise Formenfragen erscheinen nicht mehr.
  - Der Mathebereich konzentriert sich damit voll auf Zahlen und Rechenlogik.
- Naechster Schritt: Neu bauen, App neu starten und den neuen Umfang im Lernbereich testen.

### 2026-04-22 - Startmenü um Beenden und Spieler wechseln erweitert
- Anfrage: Im Startmenü sollten noch zwei sichtbare Buttons ergänzt werden: `Beenden` zum Schließen des Programms und `Spieler wechseln`, um erneut zur Spielerauswahl zurückzukehren.
- Aktion:
  - Im oberen Startmenü-Bereich wurden zwei neue Buttons eingebaut.
  - `Beenden` speichert das aktuelle Profil und schließt danach die App sauber.
  - `Spieler wechseln` speichert das aktuelle Profil, verlässt bei Bedarf den Vollbildmodus und öffnet anschließend wieder die Spielerauswahl.
  - Der Startbildschirm kann dafür jetzt auch direkt ohne erneuten Ladeablauf in die Spielerauswahl oder Neuanlage wechseln.
- Ergebnis:
  - Das Hauptmenü besitzt jetzt direkte Steuerungen zum Beenden und zum Wechseln des Spielers.
  - Beim Spielerwechsel kann sofort ein anderes Profil geladen oder ein neuer Spieler angelegt werden.
  - Der Rückweg in die Spielerwahl bleibt innerhalb des bestehenden App-Flusses.
- Naechster Schritt: Neu bauen, neu starten und beide Buttons im laufenden Startmenü testen.

### 2026-04-22 - Szenen-Buttons an die Stufen im unteren Bildbereich versetzt
- Anfrage: Die Buttons `Beenden` und `Spieler wechseln` sollten nicht oben bleiben, sondern unten im Bild bei den Stufen platziert werden.
- Aktion:
  - Die beiden Buttons wurden aus dem Kopfbereich entfernt.
  - Stattdessen wurden sie als fest verankerte Szenen-Buttons direkt auf dem Startbild positioniert.
  - Die neuen Positionen liegen im unteren Bereich links und rechts neben den Stufen, damit sie mit der Bildszene mitskalieren.
- Ergebnis:
  - `Spieler wechseln` und `Beenden` sitzen jetzt unten auf dem Bild bei den Stufen.
  - Die Positionen bleiben auch bei Größenänderung des Fensters an der Szene ausgerichtet.
- Naechster Schritt: Neu bauen, neu starten und die neue Position direkt im Hauptmenü prüfen.

### 2026-04-22 - App startet jetzt direkt im Vollbildmodus
- Anfrage: Das Spiel sollte direkt im Vollbildmodus starten.
- Aktion:
  - Der Startbildschirm wurde so angepasst, dass er sofort randlos maximiert geöffnet wird.
  - Das Hauptmenü startet nun ebenfalls direkt im Vollbildmodus.
  - Die vorhandene Vollbild-Umschaltung im Hauptmenü wurde dafür auf eine gemeinsame Logik umgestellt.
  - Beim Spielerwechsel wird nicht mehr erst sichtbar in den Fenstermodus zurückgesprungen.
- Ergebnis:
  - Die App öffnet jetzt direkt bildschirmfüllend.
  - Startbildschirm, Spielerauswahl und Hauptmenü folgen demselben Vollbild-Startverhalten.
  - `Esc` kann weiterhin den Vollbildmodus im Hauptmenü verlassen.
- Naechster Schritt: Neu bauen, neu starten und den Vollbild-Start im laufenden Programm prüfen.

### 2026-04-22 - Neue Spieler starten ohne Sterne, bestehende Profile entfernt
- Anfrage: Neue Spieler sollten ohne Sterne starten. Zusätzlich sollten alle bereits bestehenden Spieler gelöscht werden.
- Aktion:
  - Die automatische Anhebung auf mindestens `6` Sterne im Hauptmenü wurde entfernt.
  - Die vorhandene Datei mit gespeicherten Spielerprofilen wurde geleert.
- Ergebnis:
  - Neue Spieler beginnen jetzt wirklich mit `0` Sternen.
  - Die bisher gespeicherten Spielerprofile sind entfernt.
  - Beim nächsten Start erscheint wieder eine leere Spielerauswahl beziehungsweise direkt die Neuanlage.
- Naechster Schritt: Neu bauen, App neu starten und die Neuanlage mit `0` Sternen prüfen.

### 2026-04-22 - Deutsch, Sachkunde und Musik als spielbare Fächer ergänzt
- Anfrage: Die Fächer `Deutsch`, `Sachkunde` und `Musik` sollten genauso wie `Mathematik` als echte Lernbereiche aufgebaut werden.
- Aktion:
  - Drei neue Aufgaben-Generatoren für `Deutsch`, `Sachkunde` und `Musik` wurden erstellt.
  - Die Generator-Factory wurde erweitert, sodass jetzt vier Fächer für Klasse 1 unterstützt werden.
  - Das bisher fest verdrahtete Mathefenster im Hauptmenü wurde zu einer gemeinsamen Fach-Lernansicht umgebaut.
  - Alle vier Fächer laufen jetzt mit Aufgabenrunden, Fortschritt, Sternen und Rückweg im selben Fenster.
- Ergebnis:
  - `Mathematik`, `Deutsch`, `Sachkunde` und `Musik` sind in Klasse 1 direkt spielbar.
  - Jedes Fach erzeugt eigene Aufgabenarten, bleibt aber im selben Bedienablauf wie Mathe.
  - Sterne werden fachübergreifend gesammelt und im Spielerprofil gespeichert.
- Naechster Schritt: Neu bauen, App neu starten und alle vier Fächer in Klasse 1 einmal durchtesten.

### 2026-04-22 - Lernportal auf Pokale und Welten auf Sterne getrennt
- Anfrage: Im `Lernportal` sollte eine Pokallogik gelten, in den `Welten` weiterhin die Sternelogik.
- Aktion:
  - Das Spielerprofil wurde um eigene `Pokale` erweitert.
  - Die Spielerauswahl zeigt jetzt sowohl `Pokale` als auch `Sterne` an.
  - Das Hauptmenü merkt sich nun, ob ein Fach über `Lernportal` oder `Welten` gestartet wurde.
  - Aufgabenrunden aus dem Lernportal vergeben jetzt `Pokale`, Aufgabenrunden aus den Welten vergeben `Sterne`.
  - Die Belohnungsansicht wurde in zwei getrennte Bereiche für Pokale und Sterne umgebaut.
- Ergebnis:
  - Lernportal und Welten verwenden jetzt zwei getrennte Belohnungssysteme.
  - Die aktuelle Lernansicht zeigt passend zur Herkunft entweder Pokale oder Sterne an.
  - Beide Werte werden getrennt im Spielerprofil gespeichert.
- Naechster Schritt: Neu bauen, neu starten und die Belohnungstrennung über beide Wege prüfen.

### 2026-04-22 - Klassenfreischaltung mit perfekten 10er-Runden aufgebaut
- Anfrage: Eine Runde sollte aus `10` Fragen bestehen. Eine perfekte Fachrunde soll im Lernportal `10 Pokale` geben. Die damalige Welt-Sternelogik wurde später angepasst und ist weiter unten dokumentiert.
- Aktion:
  - Die Rundengröße wurde auf `10` Fragen festgelegt.
  - Belohnungen werden jetzt nur noch bei einer perfekten Runde vergeben.
  - Perfekte Runden zählen pro Fach, Klasse und Bereich nur einmal.
  - Das Spielerprofil speichert dafür jetzt Fach-Fortschritte für Lernportal und Welten getrennt.
  - Die Klassenkarten im Lernportal und in den Welten zeigen jetzt ihren aktuellen Freischaltstatus an.
- Ergebnis:
  - Eine perfekte Runde in einem Fach gibt im Lernportal genau `10 Pokale`.
  - Klasse 1 im Lernportal kann über die vier Fächer genau `40 Pokale` erreichen und damit `Welt Klasse 1` freischalten.
  - Die Freischaltlogik ist für die weiteren Klassenketten vorbereitet und die spätere Welt-Sternelogik ist separat dokumentiert.
- Naechster Schritt: Neu bauen, neu starten und die perfekte-Runden- sowie Klassenfreischaltung im laufenden Spiel prüfen.

### 2026-04-22 - Fachfortschritt auf richtige Aufgaben statt nur perfekte Runde umgestellt
- Anfrage: Die Pokale wurden nicht pro richtig gelöster Aufgabe gezählt.
- Aktion:
  - Der Fachfortschritt wurde von einer reinen Perfekt-Runden-Wertung auf eine Aufgabenwertung umgestellt.
  - Jede richtige Aufgabe erhöht jetzt den Wert eines Fachs bis maximal `10` Punkte.
  - Ein Fach kann damit im Lernportal bis zu `10 Pokale` und in den Welten bis zu `10 Sterne` liefern.
  - Wiederholte Runden erhöhen ein Fach nur noch bis zum besten erreichten Stand und nicht unbegrenzt weiter.
- Ergebnis:
  - Richtige Aufgaben zählen jetzt direkt in den Fortschritt des Fachs.
  - Die Freischaltlogik mit `40` Punkten pro Klasse bleibt erhalten.
  - Portal und Welten können weiterhin sauber über vier Fächer freigeschaltet werden.
- Naechster Schritt: Neu bauen, neu starten und die neue Zählweise im laufenden Spiel prüfen.

### 2026-04-22 - Belohnungen müssen nach jeder Runde eingesammelt werden
- Anfrage: Pokale sollten nach jeder Runde erst eingesammelt werden. Sobald im Lernportal `40` Pokale erreicht sind, soll die Meldung erscheinen, dass `Welt Klasse 1` freigeschaltet wurde, und der Erfolg soll ins Tagebuch eingetragen werden.
- Aktion:
  - Das Rundenende wurde so umgestellt, dass neue Pokale oder Sterne erst nach Klick auf `Belohnung einsammeln` gutgeschrieben werden.
  - Pro Fach wird weiterhin nur der beste erreichte Stand gespeichert, damit sich Aufgaben nicht unbegrenzt auszahlen.
  - Nach dem Einsammeln wird jetzt sofort geprüft, ob dadurch eine Welt oder eine neue Portal-Klasse freigeschaltet wurde.
  - Freischaltungen werden als Erfolgstext in der Lernansicht angezeigt und zusätzlich dauerhaft im Tagebuch des Spielers gespeichert.
- Ergebnis:
  - Nach jeder Runde werden neue Belohnungen erst bewusst eingesammelt.
  - Bei `40` Pokalen in Klasse 1 erscheint die Erfolgsmeldung für `Welt Klasse 1`.
  - Die Freischaltung bleibt im Tagebuch des jeweiligen Spielers dokumentiert.
- Naechster Schritt: Im Spiel eine Klasse-1-Runde abschließen, Pokale einsammeln und die Freischaltmeldung mit Tagebucheintrag prüfen.

### 2026-04-23 - Welt Klasse 1 erhält Fach-Inseln mit Lumos
- Anfrage: Nach der Fachauswahl in `Welt Klasse 1` sollte zuerst die entsprechende Insel erscheinen. Lumos soll dann auf dieser Insel beziehungsweise Map stehen.
- Aktion:
  - Der Klick auf ein Fach in `Deine Welten` führt jetzt zuerst zu einer neuen Inselansicht im selben Hauptfenster.
  - Die Inselansicht zeigt eine Fachkarte und legt `Lumos.png` sichtbar darüber.
  - Lumos bekommt je Fach eine eigene feste Position auf der Karte.
  - über `Aufgaben starten` beginnt anschließend die normale 10-Fragen-Runde für Sterne.
  - Die Bildsuche wurde vorbereitet für spätere Inselbilder in `Bilder\Welten\Klasse1` oder `Bilder\Inseln\Klasse1`.
- Ergebnis:
  - Das Lernportal startet Fächer weiterhin direkt mit Aufgaben und Pokalen.
  - Die Welten haben jetzt einen zusätzlichen Map-/Insel-Zwischenschritt mit Lumos.
  - Falls später echte Inselbilder erstellt werden, können sie unter passenden Namen ergänzt werden, ohne die Navigation neu zu bauen.
- Naechster Schritt: App starten, `Deine Welten` öffnen, `Welt Klasse 1` wählen und jedes Fach einmal bis zur Inselansicht prüfen.

### 2026-04-23 - Klasse-1-Bild als gemeinsames Spielbrett gesetzt
- Anfrage: Das Bild `Klasse 1.png` soll das eigentliche Spielbrett beziehungsweise die Map sein. Die unterschiedlichen Fächer sind dort bereits als Inseln dargestellt.
- Aktion:
  - Die Welt-Klasse-1-Ansicht verwendet jetzt immer `Bilder\Klasse 1.png` als Map.
  - Die Darstellung wurde auf ein festes Koordinatensystem der Map umgestellt, damit Lumos im Vollbild stabil auf derselben Inselposition bleibt.
  - Lumos wird je nach Fach auf die passende Insel gesetzt: Mathe oben rechts, Deutsch/Sprache oben links, Sachkunde unten links.
  - Musik wird vorerst auf die Kreativ-Insel unten rechts gesetzt, weil auf der aktuellen Map keine eigene Musik-Insel eingezeichnet ist.
- Ergebnis:
  - Die Fachauswahl in den Welten führt nun auf das gemeinsame Klasse-1-Spielbrett.
  - Lumos steht nicht mehr auf einzelnen Fachbildern, sondern direkt auf der großen Map.
- Naechster Schritt: Prüfen, ob für Musik später eine eigene Musik-Insel auf der Map ergänzt oder die Kreativ-Insel umbenannt werden soll.

### 2026-04-23 - Welt Klasse 1 als echtes Level-Spielbrett aufgebaut
- Anfrage: Lumos soll auf jeder Insel bei `Level 1` starten, nach richtig gelöstem Level automatisch zum nächsten Zahlenfeld laufen und am Ende den Stern einsammeln. Insel und Aufgaben sollen im Vollbild laufen. Pro Level gibt es `10 Aufgaben`; in den Leveln selbst werden keine Sterne gesammelt.
- Aktion:
  - Für jede Insel wurden feste Wegpunkte für `Level 1` bis `Level 5` und den Stern auf der `Klasse 1.png`-Map hinterlegt.
  - Der Spielerfortschritt wird pro Klasse und Fach gespeichert, inklusive abgeschlossener Level und eingesammeltem Insel-Stern.
  - Die Inselansicht und die Aufgabenansicht werden im Vollbild-Detailbereich geöffnet.
  - Ein Level besteht aus `10 Aufgaben` und gilt nur dann als geschafft, wenn alle `10` Antworten richtig sind.
  - Während der Level werden keine Sterne vergeben; Sterne werden erst beim Erreichen des Insel-Sterns gutgeschrieben.
  - Nach einem perfekten Level läuft Lumos automatisch zum nächsten Wegpunkt beziehungsweise nach Level 5 zum Stern.
- Ergebnis:
  - `Welt Klasse 1` ist jetzt als Map-Spielbrett vorbereitet.
  - Lumos läuft pro Fach-Insel von `1` über `2`, `3`, `4`, `5` bis zum Stern.
  - Der Stern einer Insel zählt anschließend als `10 Sterne` für diese Insel.
- Naechster Schritt: Im Spiel eine Insel öffnen, Level 1 spielen und prüfen, ob Lumos nach einer perfekten Runde sichtbar zu Level 2 läuft.

### 2026-04-23 - Belohnungsstände und Weltfortschritt zurückgesetzt
- Anfrage: Alle gesammelten Pokale und Sterne sollten auf `0` gesetzt werden.
- Aktion:
  - Die gespeicherten Belohnungsstände in der Spielerdatei wurden zurückgesetzt.
  - Dazu wurden `RewardProgress`, `WorldMapProgress`, `Trophies` und `Stars` auf einen leeren Startzustand gebracht.
- Ergebnis:
  - Gesammelte Pokale und Sterne stehen wieder auf `0`.
  - Bereits angelaufene Weltpfade wurden ebenfalls zurückgesetzt, damit keine alten Sterne oder Wege sichtbar bleiben.
- Naechster Schritt: App starten und prüfen, ob die Spielerprofile wieder ohne Belohnungsstand geladen werden.

### 2026-04-23 - Lumos-Grafik ohne weißen Kasten vorbereitet
- Anfrage: Der Held sollte auf der Karte ohne weißen Kasten beziehungsweise Hintergrund angezeigt werden.
- Aktion:
  - Es wurde geprüft, ob `Lumos.png` bereits transparent gespeichert ist.
  - Der helle Hintergrund im Bild wurde technisch freigestellt und die Laufzeitkopie aktualisiert.
  - Zusätzlich wurde die helle Markierung hinter Lumos in der Kartenansicht entfernt.
- Ergebnis:
  - Lumos soll jetzt ohne weißen Kasten direkt auf der Map stehen.
  - Sichtbare helle Flächen hinter der Figur kommen nicht mehr aus der Markierung des Interfaces.
- Naechster Schritt: In der laufenden App direkt auf der Weltkarte prüfen, ob Lumos sauber freigestellt dargestellt wird.

### 2026-04-23 - Weltkarte vergrößert und Textbereich kompakter gemacht
- Anfrage: Die Map sollte größer dargestellt werden. Der Textbereich sollte kleiner werden, aber die Buttons sollten sichtbar bleiben und die Ansicht sollte ohne Scrollen funktionieren.
- Aktion:
  - Die Kartenfläche der Weltansicht wurde mehrfach vergrößert.
  - Der untere Textbereich wurde kompakter gemacht: kleinere Abstände, kleinere Schrift und geringere Buttonhöhe.
  - Die Weltansicht wurde so angepasst, dass die Map mehr Platz bekommt und die Buttons weiterhin direkt sichtbar bleiben.
- Ergebnis:
  - Die Karte nutzt jetzt deutlich mehr sichtbare Fläche.
  - Der Textbereich nimmt weniger Höhe ein.
  - Die Bedienung in der Weltansicht soll ohne unnötiges Scrollen möglich sein.
- Naechster Schritt: In der laufenden Weltansicht prüfen, ob die Karte gro? genug ist und alle Buttons ohne Scrollen sichtbar bleiben.

### 2026-04-23 - Doppelte Fragen innerhalb einer Runde verhindert
- Anfrage: Es sollte nicht dieselbe Frage zweimal in einer Runde vorkommen, egal ob die Runde im Lernportal oder in der Welt gespielt wird.
- Aktion:
  - Die Fachgeneratoren wurden auf einen gemeinsamen Fragenpool pro Fach gestellt.
  - Innerhalb einer erzeugten Runde werden Fragen jetzt nach dem Fragetext gefiltert, sodass keine doppelte Aufgabe in derselben Runde erscheint.
  - Die Antwortreihenfolge bleibt weiterhin zufaellig gemischt.
- Ergebnis:
  - In einer 10er-Runde sollen keine identischen Fragen mehr doppelt auftauchen.
  - Lernportal und Welt duerfen weiterhin aus demselben Fachpool ziehen, aber die aktuelle Runde bleibt ohne Wiederholungen.
- Naechster Schritt: Ein Fach mehrfach starten und pruefen, ob innerhalb einer Runde keine Frage doppelt erscheint.

### 2026-04-23 - Weltsterne auf 1 pro Fach umgestellt
- Anfrage: Am Ende eines vollständig abgeschlossenen Fachwegs in der Welt soll genau ein Stern eingesammelt werden. In Klasse 1 sollen insgesamt `4 Sterne` aus den `4 Fächern` benötigt werden, damit `Klasse 2` im Lernportal freigeschaltet wird.
- Aktion:
  - Die Weltbelohnung pro abgeschlossenem Fach wurde auf genau `1 Stern` gesetzt.
  - Die Freischaltlogik für das Lernportal prüft jetzt bei Welt Klasse 1 insgesamt `4 Sterne`.
  - Die Anzeigen in Belohnungen, Klassenkarten und Sperrhinweisen wurden auf die neue Sternelogik angepasst.
  - Die Speicherung der Spielerstände begrenzt Weltsterne jetzt sauber auf maximal `1` pro Fach.
- Ergebnis:
  - Jede vollständig abgeschlossene Insel in `Welt Klasse 1` zählt genau `1 Stern`.
  - Mit `4 Sternen` aus Mathe, Deutsch, Sachkunde und Musik wird `Klasse 2` im Lernportal freigeschaltet.
  - Die Pokallogik im Lernportal bleibt unverändert bei `40 Pokalen`, um eine Welt freizuschalten.
- Naechster Schritt: In der App ein Fach in `Welt Klasse 1` bis zum Stern abschließen und prüfen, ob genau `1 Stern` gutgeschrieben wird.

### 2026-04-23 - Klasse-2-Fächer aus klasse 2.png übernommen
- Anfrage: Das Bild `klasse 2.png` sollte ausgelesen werden. Die dort sichtbaren Fächer sollten für Klasse 2 in der App angelegt und anschließend teilweise umbenannt werden.
- Aktion:
  - Das Bild `klasse 2.png` wurde visuell geprüft.
  - Die fünf sichtbaren Fächer wurden übernommen und für die App benannt als: `Deutsch`, `Mathe`, `Sachkunde`, `Logik`, `Musik`.
  - Für `Klasse 2` wurde eine eigene Fächerauswahl im selben Fenster angelegt.
  - Die neuen Fachbuttons sind sowohl vom Lernportal als auch von den Welten aus erreichbar.
  - Die neuen Klasse-2-Fächer führen vorerst in Platzhalteransichten, damit die Fachstruktur schon steht und die Inhalte danach einzeln ausgebaut werden können.
- Ergebnis:
  - `Klasse 2` zeigt jetzt eine eigene Fachauswahl statt nur einer allgemeinen Platzhalterkarte.
  - Die App verwendet für Klasse 2 jetzt die fünf Fächer `Deutsch`, `Mathe`, `Sachkunde`, `Logik` und `Musik`.
  - Zurück-Navigation und Klassenwechsel bleiben dabei im selben Fenster erhalten.
- Naechster Schritt: Für jedes der fünf Klasse-2-Fächer nacheinander Aufgaben, Inselpfade und Belohnungslogik aufbauen.

### 2026-04-23 - Sachkunde-Position auf Welt-Klasse-1-Map korrigiert
- Anfrage: In `Welt Klasse 1` stand Lumos bei `Sachkunde` nicht auf der passenden Insel.
- Aktion:
  - Die Wegpunkte der Sachkunde-Insel auf `Klasse 1.png` wurden neu an die blaue Insel unten links angepasst.
  - Dabei wurden die Positionen für `Level 1` bis `Level 5` und den Stern gemeinsam korrigiert, damit Lumos auf dem ganzen Fachweg auf derselben Insel bleibt.
- Ergebnis:
  - Lumos startet bei `Sachkunde` jetzt auf der richtigen Insel.
  - Auch die weiteren automatischen Schritte auf der Map bleiben für Sachkunde auf dem blauen Inselpfad.
- Naechster Schritt: In der laufenden App `Welt Klasse 1 > Sachkunde` öffnen und prüfen, ob Startpunkt und Laufweg jetzt sauber auf der Insel liegen.

### 2026-04-23 - Falsche Lumos-Position nach Fachwechsel in der Welt behoben
- Anfrage: Nach Abschluss eines Fachs in der Welt stand Lumos bei der Auswahl eines neuen Fachs nicht auf dem richtigen Punkt der neuen Insel.
- Aktion:
  - Der temporäre Bewegungszustand von Lumos wurde an Fach und Klasse gebunden.
  - Beim Verlassen einer Welt-Aufgabe zurück zur Fach- oder Klassenwahl wird die offene Bewegung jetzt gezielt gelöscht.
  - Eine automatische Lumos-Bewegung wird dadurch nur noch auf genau der Insel abgespielt, zu der sie gehört.
- Ergebnis:
  - Ein abgeschlossener Laufweg aus einem Fach springt nicht mehr in ein anderes Fach über.
  - Beim Wechsel auf eine neue Insel soll Lumos jetzt direkt auf dem korrekten Start- oder Fortschrittspunkt erscheinen.
- Naechster Schritt: In `Welt Klasse 1` erst ein Fach beenden, dann zurück zur Fächerauswahl gehen und anschließend ein anderes Fach öffnen.

### 2026-04-23 - Klasse-2-Fachbilder aus dem Ordner eingebunden
- Anfrage: Für `Deutsch`, `Mathe`, `Sachkunde` und `Musik` sollten die vorhandenen Bilder aus dem Ordner verwendet werden. `Logik` sollte vorerst ohne Bild bleiben.
- Aktion:
  - Die Klasse-2-Fachkarten wurden von reinen Farbflächen auf Bildkarten umgestellt.
  - Verwendet werden jetzt die vorhandenen Dateien `Deutsch.png`, `Mathematik.png`, `Sachkunde.png` und `Musik.png`.
  - `Logik` bleibt absichtlich als Platzhalterkarte ohne Bild, bis ein eigenes Logik-Bild erstellt ist.
- Ergebnis:
  - Klasse 2 zeigt für vier Fächer jetzt direkt die passenden Fachbilder aus dem Bilder-Ordner.
  - Die Logik-Karte bleibt weiterhin klar erkennbar und kann später ohne Umbau mit einem eigenen Bild ergänzt werden.
- Naechster Schritt: In der laufenden App `Klasse 2` öffnen und prüfen, ob die vier Bildkarten korrekt geladen werden.

### 2026-04-23 - Logik-Bild für Klasse 2 nachgezogen
- Anfrage: Das Bild `Logik.png` wurde nachträglich im Ordner hinterlegt.
- Aktion:
  - Die Klasse-2-Logik-Karte wurde ebenfalls auf eine Bildkarte umgestellt.
  - `Logik.png` wird jetzt wie die anderen Fachbilder über die bestehende Bildsuche geladen.
  - Zusätzlich wurde geprüft, dass `Logik.png` nun auch im eigentlichen Projektordner `Bilder` liegt und nicht nur in der Debug-Laufzeitkopie.
- Ergebnis:
  - Auch `Logik` kann in Klasse 2 jetzt mit eigenem Fachbild angezeigt werden.
  - Die fünf Klasse-2-Fächer sind damit bildseitig vollständig vorbereitet.
- Zusatz: Der Stand wurde erfolgreich neu gestartet.
- Naechster Schritt: Klasse 2 öffnen und prüfen, ob auch die Logik-Karte nun das richtige Bild zeigt.

### 2026-04-23 - übersicht für fehlende Lernportal-Pokale ergänzt
- Anfrage: Wenn in den Lernaufgaben noch nicht alle Pokale gesammelt wurden, sollte sichtbar sein, in welchem Fach die fehlenden Pokale liegen.
- Aktion:
  - Die Struktur der App wurde geprüft und die übersicht im Bereich `Meine Belohnungen` ergänzt.
  - Unter den Gesamtwerten für Pokale und Sterne gibt es jetzt einen eigenen Abschnitt `Fehlende Pokale im Lernportal`.
  - Die übersicht listet für die aktuell spielbaren Lernportal-Fächer von Klasse 1 pro Fach den aktuellen Pokalstand und die noch fehlenden Pokale auf.
- Ergebnis:
  - Spieler sehen jetzt direkt, in welchem Fach im Lernportal noch Pokale fehlen.
  - Die übersicht aktualisiert sich zusammen mit der normalen Belohnungsanzeige.
- Naechster Schritt: In der App `Meine Belohnungen` öffnen und prüfen, ob unvollständige Fächer mit Rest-Pokalen angezeigt werden.

### 2026-04-23 - Fehler beim Laden durch Klassenbild behoben
- Anfrage: Beim Laden des Spielstands trat ein unerwarteter Fehler auf.
- Aktion:
  - Das Fehlerprotokoll wurde geprüft.
  - Ursache war ein falscher Dateiname in der Klassen-Vorschau: Im Code stand `Klasse 3#.png`, im Ordner liegt jedoch `Klasse 3.png`.
  - Zusätzlich wurde das Laden der Klassenbilder robuster gemacht, damit ein fehlendes Vorschau-Bild nicht mehr die ganze App schließt.
- Ergebnis:
  - Das Hauptmenü soll jetzt auch dann starten, wenn ein einzelnes Klassenbild fehlt oder umbenannt wurde.
  - Der Spielstand `Astrid` wird nicht mehr durch diesen Bildfehler blockiert.
- Naechster Schritt: Spielstand erneut laden und prüfen, ob das Men? jetzt ohne Fehlermeldung geöffnet wird.

### 2026-04-23 - Sterne in den Welten auf manuelles Einsammeln umgestellt
- Anfrage: In den Welten sollten Sterne nicht automatisch gutgeschrieben werden, sondern erst nach einem bewussten Klick auf einen Button eingesammelt werden.
- Aktion:
  - Der Welt-Fortschritt wurde in `Stern erreicht` und `Stern eingesammelt` getrennt.
  - Nach dem letzten Level wartet der Stern jetzt erst auf den Sammel-Klick.
  - Beim direkten Abschluss erscheint ein eigener Sammel-Button.
  - Wenn die App vorher verlassen wird, bleibt der Stern auf der Insel als einsammelbar sichtbar und kann später über den Insel-Button eingesammelt werden.
  - Der Tagebuch-Eintrag für den Stern wird jetzt erst beim tatsächlichen Einsammeln angelegt.
- Ergebnis:
  - Welt-Sterne werden nicht mehr automatisch beim Erreichen gutgeschrieben.
  - Sterne zählen erst nach einem bewussten Klick als eingesammelt.
  - Bereits erreichte, aber noch nicht eingesammelte Sterne gehen nicht verloren.
- Naechster Schritt: In `Welt Klasse 1` ein Fach bis zum Stern spielen und prüfen, ob erst der Sammel-Klick den Stern wirklich gutschreibt.

### 2026-04-23 - Stern-Erfolg im Tagebuch vereinheitlicht
- Anfrage: Wenn der Stern eingesammelt ist, soll dies im Tagebuch als Erfolg erscheinen.
- Aktion:
  - Der Tagebuch-Eintrag für eingesammelte Welt-Sterne wurde auf einen einheitlichen Erfolgstext umgestellt.
  - Beide Einsammelwege verwenden jetzt denselben Erfolgseintrag, egal ob der Stern direkt nach dem Level oder später über die Insel eingesammelt wird.
- Ergebnis:
  - Nach dem Einsammeln eines Welt-Sterns erscheint im Tagebuch ein klarer Erfolgseintrag.
  - Der Eintrag ist jetzt an beiden Stellen gleich formuliert.
- Naechster Schritt: Einen Stern einsammeln und prüfen, ob im Tagebuch ein Eintrag mit `Erfolg` erscheint.

### 2026-04-23 - Aufgabengeneratoren auf echte Umlaute umgestellt
- Anfrage: In den Aufgaben aus dem Generator wurden keine Umlaute angezeigt. Es sollte geprüft und bei Bedarf auf UTF-8 umgestellt werden.
- Aktion:
  - Die Projekteinstellung wurde geprüft: Die App arbeitet bereits mit UTF-8.
  - Die eigentliche Ursache lag in den Generator-Dateien, in denen viele Aufgabentexte bewusst als `ae/oe/ue` hinterlegt waren.
  - Die Generator-Texte für Mathematik, Deutsch, Sachkunde und Musik wurden auf echte Umlaute umgestellt.
- Ergebnis:
  - Neue Aufgabenrunden zeigen jetzt Wörter wie `größer`, `?pfel`, `Frühling`, `Höhle`, `Flöte`, `fühlt` und `gleichmäßig` mit echten Umlauten.
  - Die Generatoren sind jetzt inhaltlich auf UTF-8-Darstellung abgestimmt.
- Naechster Schritt: Mehrere Aufgabenrunden in den vier Fächern starten und prüfen, ob die Umlaute überall sauber angezeigt werden.

### 2026-04-23 - Automatische Generator-Prüfung gegen Datenfehler ergänzt
- Anfrage: Es sollte eine Prüfung eingebaut werden, damit in den Aufgabengeneratoren offensichtliche Fachfehler wie falsche Buchstabenanzahlen nicht mehr unbemerkt bleiben.
- Aktion:
  - Die fehlerhafte Aufgabe `Wie viele Buchstaben hat Rakete?` wurde auf `6` korrigiert.
  - Eine zentrale Validierungsschicht für generierte Aufgaben wurde ergänzt.
  - Die Prüfung kontrolliert jetzt unter anderem:
    - Buchstabenanzahl
    - Silbenanzahl
    - Anfangs- und Endbuchstaben
    - einfache Plus- und Minusaufgaben
    - Zahlenfolgen und fehlende Zahlen
    - Zwischenzahlen
    - passende Rechenwege
    - Mengenvergleiche wie `am meisten` oder `weniger`
  - Fehlerhafte Aufgaben werden automatisch aus dem Generator-Ergebnis entfernt und zusätzlich in `app-error.log` protokolliert.
- Ergebnis:
  - Offensichtliche inhaltliche Fehler aus den Generatoren sollen nicht mehr bis in die Spielrunde durchrutschen.
  - Künftige Datenfehler werden beim Generieren abgefangen und protokolliert.
- Naechster Schritt: Mehrere Runden starten und prüfen, ob Aufgaben wie Buchstaben- und Silbenzählen jetzt fachlich korrekt bleiben.

### 2026-04-23 - Sprachausgabe für Aufgaben ergänzt
- Anfrage: Aufgaben im Lernportal und in den Welten sollten vorgelesen werden. Die Stimme sollte freundlich, warm, weiblich und ruhig wirken statt nach KI zu klingen.
- Aktion:
  - Eine Windows-Sprachausgabe wurde für die Aufgabenansicht ergänzt.
  - Beim Anzeigen einer Aufgabe werden jetzt Fragetext und Antworten automatisch vorgelesen, solange `Sound aktiv` eingeschaltet ist.
  - Die Stimmenauswahl bevorzugt deutsche weibliche Stimmen und versucht dabei zuerst typische warme Windows-Stimmen zu wählen.
  - Die Sprechgeschwindigkeit wurde bewusst etwas ruhiger eingestellt.
  - Beim Verlassen von Ansichten oder beim Ausschalten des Sounds wird laufende Sprachausgabe gestoppt.
- Ergebnis:
  - Aufgaben in Lernportal und Welten werden jetzt automatisch vorgelesen.
  - Die Sprachausgabe hängt direkt am vorhandenen Sound-Schalter.
- Naechster Schritt: Eine Aufgabenrunde starten und prüfen, ob die Stimme sauber vorliest und sich beim Ausschalten von `Sound aktiv` sofort abschaltet.

### 2026-04-23 - Vorlesefunktion auf Lautsprecher-Bildbutton umgestellt
- Anfrage: Die Vorlesefunktion sollte über das Bild `Lautsprecher.png` aktiviert werden. Der schwarze Rand um den Lautsprecherbereich sollte entfallen.
- Aktion:
  - Im Aufgabenbereich wurde ein eigener Bildbutton mit `Lautsprecher.png` ergänzt.
  - Der Button sitzt direkt an der Aufgabe und verwendet keinen schwarzen Zusatzrahmen.
  - Das automatische Vorlesen beim Anzeigen einer Aufgabe wurde entfernt.
  - Vorlesen startet jetzt bewusst erst beim Klick auf den Lautsprecher-Button.
- Ergebnis:
  - Die Sprachausgabe wird jetzt gezielt über den Lautsprecher ausgelöst.
  - Der Lautsprecherbereich wirkt leichter, weil der zusätzliche dunkle Rahmen entfällt.
- Naechster Schritt: Eine Aufgabe öffnen und prüfen, ob der Lautsprecher-Button das Vorlesen zuverlässig startet.

### 2026-04-23 - Vorlesetext auf Frage und Antworten begrenzt
- Anfrage: Die Vorlesefunktion sollte bei Aktivierung nur die Frage und die Antworten vorlesen, nichts anderes.
- Aktion:
  - Der Vorlesetext wurde gekürzt.
  - Die bisherige Ansage der Aufgabennummer wurde entfernt.
- Ergebnis:
  - Der Lautsprecher liest jetzt nur noch den Fragetext und die Antwortmöglichkeiten vor.
- Naechster Schritt: Eine Aufgabe öffnen und prüfen, ob kein zusätzlicher Vorspann mehr gesprochen wird.

### 2026-04-23 - Vorlesestimme als warme Erzählerinnen-Stimme verfeinert
- Anfrage: Statt einer neutralen Standardstimme sollte die Vorlesefunktion eher wie eine freundliche, ruhige und warme Erzählerinnen-Stimme wirken.
- Aktion:
  - Die lokale Windows-Stimme wurde beibehalten, aber die Ausgabe über SSML weicher gestaltet.
  - Sprechtempo und Rhythmus wurden ruhiger gesetzt.
  - Zwischen Frage und Antworten sowie zwischen den Antworten wurden bewusst kleine Pausen ergänzt.
  - Die Stimmwahl bevorzugt weiterhin eine deutsche weibliche Stimme und nutzt diese jetzt mit weicherem Vorleseprofil.
- Ergebnis:
  - Die Vorlesefunktion klingt ruhiger und erzählerischer als die reine Standardausgabe.
  - Frage und Antworten werden mit natürlicheren Pausen gesprochen.
- Naechster Schritt: Eine Aufgabe öffnen und prüfen, ob die Stimme jetzt angenehmer und ruhiger klingt.

### 2026-04-23 - Sichtbare Silbentrennungen in Aufgaben entfernt
- Anfrage: In Fragen und Antworten sollten Wörter nicht als `Blu-me` oder `Ba-na-ne` erscheinen, weil das für Kinder nicht hilfreich ist.
- Aktion:
  - Die sichtbaren Trennstriche in den betroffenen Deutsch- und Musikaufgaben wurden entfernt.
  - Die Aufgaben wurden auf normale Wortschreibweise wie `Blume`, `Banane`, `Auto`, `Tomate`, `Rose`, `Lampe` und `Lokomotive` umgestellt.
- Ergebnis:
  - Kinder sehen in Frage und Antworten jetzt normale Wörter ohne künstliche Trennstriche.
  - Die Silbenaufgaben bleiben fachlich erhalten, aber wirken lesefreundlicher.
- Naechster Schritt: Aufgaben in Deutsch und Musik öffnen und prüfen, ob keine künstlich getrennten Wörter mehr sichtbar sind.

### 2026-04-23 - Klasse 2 im Lernportal spielbar gemacht
- Anfrage: Klasse 2 sollte nicht nur vorbereitet sein, sondern mit echten Aufgaben so eingebunden werden, dass der Spieler-Test direkt weiterlaufen kann.
- Aktion:
  - Die Klasse-2-Fächerauswahl wurde mit echten Lernrunden verbunden statt nur auf Platzhalter zu zeigen.
  - Für `Deutsch`, `Mathematik`, `Sachkunde` und `Musik` wurden eigene Klasse-2-Aufgabenpools ergänzt.
  - Das neue Fach `Logik` wurde als eigenes Fachmodell und mit eigenem Aufgaben-Generator eingebunden.
  - Die Fachtexte in der Aufgabenansicht unterscheiden jetzt zwischen Klasse 1 und Klasse 2.
  - Die Pokal-übersicht in `Meine Belohnungen` zeigt jetzt auch fehlende Pokale aus Klasse 2, sobald Klasse 2 im Lernportal freigeschaltet ist.
- Ergebnis:
  - Klasse 2 ist im Lernportal jetzt direkt testbar, sobald sie über die Sternelogik freigeschaltet wurde.
  - Alle fünf Fächer aus `klasse 2.png` sind fachlich angebunden: `Deutsch`, `Mathematik`, `Sachkunde`, `Logik`, `Musik`.
  - Die Welt von Klasse 2 bleibt vorerst vorbereitet, aber noch nicht als Map-Level-System ausgebaut.
- Naechster Schritt: Mit einem freigeschalteten Spieler Klasse 2 im Lernportal durchspielen und auffällige Fach-, Text- oder Belohnungsfehler nachziehen.

### 2026-04-23 - Freischaltung fuer Welt Klasse 2 auf 50 Pokale gesetzt
- Anfrage: Im Lernportal von Klasse 2 sollen `10 Pokale pro Fach` gelten, also bei `5 Fächern` insgesamt `50 Pokale`, um `Welt Klasse 2` freizuschalten.
- Aktion:
  - Die bisher globale Pokalgrenze für Welt-Freischaltungen wurde in eine klassenabhängige Freischaltlogik umgestellt.
  - Klasse 1 bleibt bei `40 Pokalen`.
  - Klasse 2 nutzt jetzt `50 Pokale` als Freischaltgrenze für `Welt Klasse 2`.
  - Anzeigen, Statuskarten und Sperrtexte greifen jetzt ebenfalls auf die neue klassenabhängige Grenze zu.
- Ergebnis:
  - `Welt Klasse 2` wird jetzt erst bei `50 Pokalen` im Lernportal von Klasse 2 freigeschaltet.
  - Die Regel `10 Pokale pro Fach` passt damit exakt zu den `5` Klasse-2-Fächern.
- Naechster Schritt: Mit einem freigeschalteten Klasse-2-Spieler sammeln und prüfen, ob die Welt genau bei `50 / 50` freigeschaltet wird.

### 2026-04-23 - Alte Klasse-1-Fachzuordnung nach Enum-Aenderung korrigiert
- Anfrage: Die Gesamtanzeige meldete `Klasse 1 Lernportal: 40 von 40 Pokale`, obwohl in `Fehlende Pokale im Lernportal` noch `Sachkunde` und `Musik` als offen angezeigt wurden.
- Ursache:
  - Nach dem späteren Einfügen des neuen Fachs `Logik` hatte sich die interne Fachreihenfolge verschoben.
  - Alte Klasse-1-Spielstände wurden dadurch beim Laden teilweise als falsche Fächer gelesen.
- Aktion:
  - Beim Laden der Spielerprofile wurde eine Migration für alte Klasse-1-Daten ergänzt.
  - Alte Einträge, die durch die Verschiebung fälschlich als `Logik` oder `Englisch` erschienen, werden jetzt wieder korrekt auf `Sachkunde` und `Musik` zurückgesetzt.
  - Zusätzlich summiert die Klassen-Gesamtanzeige jetzt nur noch Fächer, die für die jeweilige Klasse wirklich unterstützt werden.
- Ergebnis:
  - Die Gesamtanzeige und die Detailübersicht greifen wieder auf dieselben korrekten Fächer zu.
  - Alte Klasse-1-Spielstände wie `Astrid` oder `Luca` sollen damit wieder sauber `Mathematik`, `Deutsch`, `Sachkunde` und `Musik` anzeigen.
- Naechster Schritt: Betroffenen Spieler erneut laden und prüfen, ob die fehlenden Pokale jetzt nicht mehr fälschlich angezeigt werden.

### 2026-04-23 - Aufgabenansicht fuer Klasse 2 ohne Scrollen verdichtet
- Anfrage: In einigen Aufgaben im Lernportal von Klasse 2 erschien Scrollen. Die Aufgabenansicht sollte so angepasst werden, dass kein Scrollen mehr nötig ist.
- Aktion:
  - Das Aufgabenlayout wurde insgesamt kompakter gemacht.
  - Linke Vorschau- und Fortschrittsbereiche wurden etwas verkleinert.
  - Fragebereich, Lautsprecherbutton, Antwortfelder, Feedbackbereich und Aktionsbuttons wurden enger gesetzt.
  - Zusätzlich passt die App bei längeren Fragen und Antworten die Schriftgrößen jetzt automatisch an.
- Ergebnis:
  - Längere Klasse-2-Aufgaben sollen jetzt ohne Scrollen in die Aufgabenansicht passen.
  - Die Ansicht bleibt dabei im gleichen Fenster und lesbar.
- Naechster Schritt: Klasse 2 im Lernportal öffnen und lange Fragen prüfen, ob alles ohne Scrollen sichtbar bleibt.

### 2026-04-23 - Welt Klasse 2 nach Vorlage von Welt Klasse 1 eingebaut
- Anfrage: Welt Klasse 2 sollte auf Basis von Welt Klasse 1 aufgebaut und um die Fächer aus dem Lernportal Klasse 2 erweitert werden.
- Aktion:
  - Die bisherige Platzhalteransicht von `Welt Klasse 2` wurde auf die echte Insel-/Map-Ansicht umgestellt.
  - Die Klasse-2-Map `klasse 2.png` wird jetzt direkt als Weltkarte verwendet.
  - Für die fünf Klasse-2-Fächer wurden eigene Inselpfade mit Level `1` bis `5` und Zielmarker hinterlegt:
    - `Deutsch` auf der Sprach-Insel oben links
    - `Mathematik` auf der Mathe-Insel oben mittig
    - `Sachkunde` auf der Schnee-/Natur-Insel oben rechts
    - `Logik` auf der pinken Logik-Insel unten links
    - `Musik` auf der blauen unteren Insel
  - Die Welt-Freischaltlogik für den übergang zur nächsten Portal-Klasse wurde passend zur Anzahl der Klasse-2-Fächer auf `5 Sterne` angehoben.
- Ergebnis:
  - Welt Klasse 2 nutzt jetzt dieselbe spielbare Map- und Levelstruktur wie Welt Klasse 1.
  - Alle fünf Klasse-2-Fächer können in der Welt direkt angewählt und über ihre Inselpfade gespielt werden.
  - Portal Klasse 3 kann dadurch erst freigeschaltet werden, wenn in Welt Klasse 2 alle fünf Fach-Sterne gesammelt wurden.
- Naechster Schritt: Welt Klasse 2 mit einem freigeschalteten Spieler durchspielen und die Positionen der fünf Inselpfade auf der Map feinprüfen.

### 2026-04-23 - Wegpunkt-Kreise in allen Welten um 10 Prozent verkleinert
- Anfrage: Die Kreise der Wegpunkte sollten auf allen Welten um `10 %` verkleinert werden.
- Aktion:
  - Die gemeinsame Markergröße der Welt-Wegpunkte wurde zentral reduziert.
  - Dadurch werden die Level-Kreise in allen Weltkarten gleichmäßig kleiner dargestellt.
- Ergebnis:
  - Die Wegpunkte in Welt Klasse 1 und Welt Klasse 2 wirken jetzt etwas kompakter und verdecken weniger von der Karte.
- Naechster Schritt: Beide Welten öffnen und prüfen, ob die kleineren Kreise auf allen Inseln weiterhin gut lesbar und sauber positioniert sind.

### 2026-04-23 - Laufweg-Kreise von Lumos um weitere 15 Prozent verkleinert
- Anfrage: Die Kreise, auf denen Lumos läuft, sollten noch einmal um `15 %` kleiner werden.
- Aktion:
  - Die Markergröße der Welt-Wegpunkte wurde erneut reduziert.
  - Die Sternmarkierung blieb dabei unverändert, damit das Ziel weiterhin klar hervorsticht.
- Ergebnis:
  - Die Laufweg-Kreise sind jetzt nochmals kompakter und wirken auf allen Welten dezenter.
- Naechster Schritt: Welt Klasse 1 und Welt Klasse 2 öffnen und prüfen, ob Lumos weiterhin sauber über die kleineren Kreise geführt wird.
### 2026-04-24 - Lernportal Klasse 3 mit 5 Faechern spielbar gemacht
- Anfrage: Im Lernportal von Klasse 3 soll es `5 Faecher` geben. Dort muessen `50 Pokale` gesammelt werden, um `Welt Klasse 3` freizuschalten.
- Aktion:
  - Die Fachauswahl fuer `Klasse 3` wurde im Lernportal vervollstaendigt.
  - Eingebunden sind die fuenf Faecher aus `Klasse 3.png`:
    - `Sprache` als `Deutsch`
    - `Mathe` als `Mathematik`
    - `Sachkunde`
    - `Medien`
    - `Kreativitaet`
  - Fuer `Mathematik`, `Deutsch` und `Sachkunde` wurden eigene Klasse-3-Aufgabenstufen in die bestehenden Generatoren eingebaut.
  - Fuer `Medien` und `Kreativitaet` wurden neue Aufgaben-Generatoren angelegt.
  - Die Pokalgrenze fuer die Freischaltung von `Welt Klasse 3` wurde fuer `Klasse 3` auf `50 Pokale` gesetzt, also `10 Pokale pro Fach`.
- Ergebnis:
  - `Klasse 3` ist im Lernportal jetzt direkt spielbar, sobald sie ueber die Sternelogik freigeschaltet wurde.
  - Alle fuenf Faecher koennen im selben Fenster getestet werden.
  - `Welt Klasse 3` bleibt vorerst noch ein vorbereiteter Folgeschritt und wird bei `50 / 50` Pokalen in `Klasse 3` freigeschaltet.
- Naechster Schritt: Mit einem freigeschalteten Klasse-3-Spieler jede Lernportal-Rubrik einmal anspielen und danach die Weltkarte fuer Klasse 3 als echtes Map-Level-System ausbauen.

### 2026-04-24 - Lumos-Bewegung in Welt Klasse 2 nach Levelabschluss robuster gemacht
- Anfrage: In `Welt Klasse 2` wurde `Level 2` erfolgreich abgeschlossen, aber Lumos lief danach nicht sichtbar zum naechsten Wegpunkt weiter.
- Aktion:
  - Der Ruecksprung von der Aufgabenansicht zur Inselkarte wurde fuer Welt-Level robuster gemacht.
  - Die App merkt sich jetzt gezielt, dass beim naechsten Oeffnen der Map eine Bewegungsanimation gezeigt werden soll.
  - Falls der direkte Zwischenspeicher der Bewegung einmal fehlt, wird der Startpunkt der Animation aus dem aktuellen Levelstand abgeleitet und Lumos trotzdem sauber zum naechsten Punkt gefuehrt.
- Ergebnis:
  - Nach einem erfolgreich geloesten Welt-Level soll Lumos beim Zurueckkehren auf die Karte jetzt wieder sichtbar zur naechsten Markierung weiterlaufen.
  - Der Fix gilt nicht nur fuer Welt Klasse 2, sondern fuer die Weltbewegung allgemein.
- Naechster Schritt: In `Welt Klasse 2` ein Fach erneut bis zum naechsten Levelpunkt spielen und pruefen, ob Lumos den Schritt jetzt sichtbar von `Level 2` zu `Level 3` animiert.

### 2026-04-24 - Uebergaenge der Hauptnavigation und Weltlogik geprueft
- Anfrage: Alle Uebergaenge sollten geprueft werden.
- Aktion:
  - Geprueft wurden die Wechsel zwischen Klassenwahl, Fachwahl, Aufgabenansicht, Weltkarte und Rueckspruengen.
  - Besonders betrachtet wurden:
    - `Klassenwahl -> Fachwahl`
    - `Fachwahl -> Lernrunde`
    - `Fachwahl -> Weltkarte`
    - `Weltkarte -> Level`
    - `Levelende -> Zurueck zur Map`
    - Rueckspruenge ueber `Zurueck zur Faecherauswahl` und `Zurueck zur Klassenwahl`
  - Dabei wurde der Welt-Uebergang nach abgeschlossenem Level als kritische Stelle identifiziert und abgesichert.
- Ergebnis:
  - Die Hauptnavigation bleibt im selben Fenster und die Rueckspruenge setzen offene Welt-Bewegungszustaende jetzt sauber zurueck.
  - Die automatische Weiterbewegung von Lumos zur naechsten Markierung wurde fuer den Karten-Ruecksprung stabilisiert.
- Naechster Schritt: Die geprueften Wege im laufenden Spiel einmal praktisch durchklicken und einzelne Inselpositionen nur noch dann nachziehen, wenn beim Testen ein konkreter Ausreisser sichtbar wird.

### 2026-04-24 - Fachwechsel in Welt Klasse 2 auf saubere Lumos-Position korrigiert
- Anfrage: Nach einem abgeschlossenen Deutsch-Level in `Welt Klasse 2` sollte anschliessend `Mathematik` geoeffnet werden, aber Lumos stand dort nicht auf `Level 1`.
- Ursache:
  - Die vorherige Bewegungsanimation von Lumos konnte beim Inselwechsel noch aktiv bleiben und die neue Zielposition ueberlagern.
- Aktion:
  - Vor jeder neuen Positionierung auf einer Insel werden laufende Canvas-Animationen von Lumos und dem Leuchteffekt jetzt zuerst gestoppt.
- Ergebnis:
  - Beim Wechsel von einer Welt-Insel zur naechsten soll Lumos jetzt wieder exakt auf dem aktuellen Startpunkt des neu gewaehlten Fachs erscheinen.
  - Der Fix wirkt fuer alle Welten, nicht nur fuer Klasse 2.
- Naechster Schritt: In `Welt Klasse 2` nacheinander `Deutsch` und danach `Mathematik` oeffnen und pruefen, ob Lumos auf der Mathe-Insel wieder sauber auf `Level 1` startet.

### 2026-04-24 - Klasse 3 von Medien auf Logik umgestellt und Melodie-Aufgabe korrigiert
- Anfrage: In Klasse 3 soll statt `Medien` das Fach `Logik` verwendet werden, sowohl im Lernportal als auch in der Welt. Ausserdem war die Aufgabe zu `Melodie` im Aufgabenpool falsch.
- Aktion:
  - Die Klasse-3-Faecherliste wurde von `Medien` auf `Logik` umgestellt.
  - Die Klasse-3-Fachkarte zeigt jetzt `Logik` und nutzt das passende Vorschaubild.
  - Der Logik-Generator wurde so erweitert, dass er jetzt auch `Klasse 3` mit eigenem Aufgabenpool unterstuetzt.
  - Die fehlerhafte Musik-Aufgabe wurde von einer unklaren Klatschfrage auf eine klare Silbenfrage korrigiert:
    - `Wie viele Silben hat das Wort Melodie?`
    - richtige Antwort: `3`
- Ergebnis:
  - Klasse 3 nutzt jetzt `Deutsch`, `Mathematik`, `Sachkunde`, `Logik` und `Kreativitaet`.
  - Die falsche `Melodie`-Antwort taucht im Aufgabenpool nicht mehr in dieser Form auf.
- Naechster Schritt: Klasse 3 im Lernportal oeffnen und pruefen, ob statt `Medien` jetzt ueberall `Logik` erscheint und die neuen Aufgaben sauber laufen.

### 2026-04-24 - Klasse 3 Kreativ auf Musik umgestellt und Musik-Generator erweitert
- Anfrage: Die Fragen in Klasse 3 passten nicht zum Fachbild. Aus `Kreativ` sollte `Musik` werden und der Aufgaben-Generator sollte dazu angepasst werden.
- Aktion:
  - Das fuenfte Fach in Klasse 3 wurde von `Kreativitaet` auf `Musik` umgestellt.
  - Die Fachkarte in Klasse 3 zeigt jetzt `Musik`.
  - Die Klasse-3-Faecherliste verwendet jetzt `Deutsch`, `Mathematik`, `Sachkunde`, `Logik` und `Musik`.
  - Der Musik-Generator wurde auf `Klasse 3` erweitert und mit einem eigenen Aufgabenpool zu Rhythmus, Instrumenten, Melodien, Liedaufbau und gemeinsamem Musizieren ausgestattet.
- Ergebnis:
  - Bild und Fragen passen in Klasse 3 jetzt wieder fachlich zusammen.
  - Klasse 3 verwendet kein Kreativ-Fach mehr im spielbaren Lernportal-Ablauf.
- Naechster Schritt: Klasse 3 im Lernportal oeffnen und `Musik` direkt anspielen, um zu pruefen, ob die neuen Musik-Aufgaben sauber laufen.

### 2026-04-24 - Welt Klasse 3 nach Vorlage von Welt Klasse 2 spielbar gemacht
- Anfrage: Fuer `Welt Klasse 3` sollte `Welt Klasse 2` als Vorlage genutzt werden.
- Aktion:
  - Die bisherige Platzhalterlogik von `Welt Klasse 3` wurde entfernt.
  - Die Fachwahl von Klasse 3 fuehrt in der Welt jetzt direkt in die Inselansicht.
  - `Klasse 3.png` wird als Weltkarte verwendet.
  - Fuer die fuenf Klasse-3-Faecher wurden eigene Wegpunkte auf der Karte hinterlegt:
    - `Deutsch`
    - `Mathematik`
    - `Sachkunde`
    - `Logik` auf der orangefarbenen Insel
    - `Musik` auf der pinken Insel
  - Die Welt nutzt damit dieselbe Struktur wie Klasse 2: `Level 1 bis 5`, danach Stern zum Einsammeln.
- Ergebnis:
  - `Welt Klasse 3` ist jetzt direkt spielbar.
  - Lumos kann in Klasse 3 nun fuer jedes der fuenf Faecher ueber die Map gefuehrt werden.
- Naechster Schritt: In `Welt Klasse 3` jedes Fach einmal oeffnen und pruefen, ob Lumos auf der richtigen Insel und am richtigen Startpunkt steht.

### 2026-04-24 - Bonus-Insel in Welt Klasse 3 mit 3 Leveln und Extra-Stern ergänzt
- Anfrage: `Welt Klasse 3` soll `6 Inseln` haben. Auf der `6. Insel` soll es einen Bonus-Level mit `3 Leveln` und einem `Extra-Stern` geben. Die Fragen sollen aus allen Klasse-3-Fächern gemischt werden.
- Zusatzregel:
  - Die Bonus-Insel wird erst freigeschaltet, wenn alle `5` normalen Fach-Inseln in `Welt Klasse 3` abgeschlossen wurden.
  - Für die Freischaltung von `Lernportal Klasse 4` bleiben weiterhin nur die `5 Sterne` aus den normalen Fach-Inseln relevant.
- Aktion:
  - In der Klasse-3-Welt wurde ein eigenes Bonus-Fach ergänzt.
  - Die Bonus-Insel erscheint als `6.` Auswahlpunkt nur in `Welt Klasse 3`.
  - Die Bonus-Insel ist im Fachpanel zunächst gesperrt und wird erst bei `5 / 5` Fach-Sternen freigeschaltet.
  - Für die Bonus-Insel gilt eine eigene Weltlogik mit `3` Leveln statt `5`.
  - Der Aufgabenpool der Bonus-Insel mischt Fragen aus:
    - `Deutsch`
    - `Mathematik`
    - `Sachkunde`
    - `Logik`
    - `Musik`
  - Der Extra-Stern der Bonus-Insel wird separat gesammelt und schreibt einen eigenen Erfolg ins Tagebuch.
- Ergebnis:
  - `Welt Klasse 3` hat jetzt `5` normale Fach-Inseln plus `1` Bonus-Insel.
  - `Klasse 4` im Lernportal bleibt an die `5` normalen Fach-Sterne gebunden.
  - Der Bonus-Stern ist ein zusätzlicher Welt-Stern und blockiert die Klassenfreischaltung nicht.
- Naechster Schritt: In `Welt Klasse 3` erst alle `5` Fach-Inseln abschliessen, dann die Bonus-Insel öffnen und pruefen, ob dort `3` Bonus-Level mit gemischten Fragen sauber laufen.

### 2026-04-24 - Bonus-Bild eingebunden
- Anfrage: Fuer den Bonus-Level wurde ein eigenes Bild erstellt und gespeichert.
- Aktion:
  - Das neue Bild `Bonus.png` aus dem Bilderordner wurde an die Bonus-Karte in Klasse 3 angebunden.
- Ergebnis:
  - Die Bonus-Insel nutzt jetzt in der Fachauswahl ihr eigenes Vorschaubild.
- Naechster Schritt: Klasse 3 in den Welten oeffnen und pruefen, ob die Bonus-Karte das richtige Bild anzeigt.

### 2026-04-28 - Klasse 4 spielbar gemacht
- Anfrage: Am Programm sollte weitergebaut werden. Als naechster grosser Ausbau sollte `Klasse 4` nicht nur sichtbar, sondern wirklich spielbar werden.
- Aktion:
  - Fuer `Klasse 4` wurde im Hauptfenster eine eigene Fachauswahl mit `Deutsch`, `Mathematik`, `Sachkunde`, `Logik` und `Musik` eingebaut.
  - Die Portal- und Weltlogik wurde auf diese fuenf Klasse-4-Faecher erweitert.
  - In allen fuenf Aufgaben-Generatoren wurden neue Klasse-4-Aufgabenpools fuer Lernportal und Welt ergaenzt.
  - Die Freischaltgrenze fuer `Welt Klasse 4` wurde auf `50 Pokale` aus Klasse 4 abgestimmt.
  - Die Weltkarte von Klasse 4 erhielt eigene Inselpfade, Beschreibungen und Fachvorschauen.
  - Ein Generator-Selbsttest wurde ausgefuehrt; dabei wurde eine fehlerhafte Zahlenformatierung in einer Logik-Aufgabe gefunden und direkt korrigiert.
- Ergebnis:
  - `Klasse 4` ist jetzt im Lernportal und in den Welten durchgaengig anwählbar und spielbar.
  - Der Build laeuft erfolgreich mit `0` Warnungen und `0` Fehlern.
  - Alle fuenf Klasse-4-Faecher liefern im Test jeweils `10` gueltige Aufgaben.
- Naechster Schritt: Klasse 4 in der laufenden App oeffnen und die neuen Inselwege, Aufgaben und Freischaltungen visuell durchspielen.

### 2026-04-29 - Klasse 4 an Bildinseln und NRW-Faecher angepasst
- Anfrage: Die Fachinseln der Klasse-4-Meisterwelt sollten mit dem NRW-Lernplan der Primarstufe abgeglichen und an das sichtbare Kartenbild angepasst werden.
- Entscheidung:
  - `Kreativitaet` wird in `Klasse 4` als `Kunst` verwendet.
  - `Projekt` wird in `Klasse 4` als `Englisch` verwendet.
  - `Medien` wird in `Klasse 4` als `Musik` verwendet.
  - `Logik` wird in `Klasse 4` nicht weiter als Fachinsel genutzt.
  - Aktion:
    - Die Klasse-4-Fachauswahl wurde auf `Deutsch`, `Mathematik`, `Sachkunde`, `Musik`, `Kunst` und `Englisch` umgestellt.
    - Es wurden neue Aufgaben-Generatoren fuer `Englisch` und `Kunst` angelegt.
    - Die Klasse-4-Weltpfade wurden auf die sechs sichtbaren Fachinseln des Bildes neu zugeordnet.
    - Freischaltungen und Statusanzeigen wurden fuer `6` Faecher aktualisiert.
    - Fuer `Welt Klasse 4` wurde zusaetzlich eine `Meisterpruefung` als Bonus-Insel ergaenzt.
    - Die Meisterinsel wird erst bei `6 / 6` Fach-Sternen freigeschaltet, spielt `3` Bonus-Level mit gemischten Fragen aus allen Klasse-4-Faechern und vergibt danach einen Extra-Stern.
    - Nach dem Einsammeln des Extra-Sterns erscheint eine fenstergrosse Abschlussmeldung mit `Du hast die Grundschule gemeistert.`
  - Ergebnis:
  - `Welt Klasse 4` folgt jetzt der sichtbaren Meisterwelt-Karte und nutzt sechs Fachinseln plus die spaetere `Meisterpruefung` als getrennten Weltbereich.
  - Die Freischaltgrenze fuer `Welt Klasse 4` liegt jetzt bei `60 Pokalen`, passend zu `6` Klasse-4-Faechern.
- Naechster Schritt: Die Klasse-4-Welt direkt in der App oeffnen und pruefen, ob alle sechs Inseln, Wege und Aufgaben im Ablauf sauber zusammenspielen.

### 2026-04-30 - App in Lumos - LernApp umbenannt und Installer vorbereitet
- Anfrage: Fuer die Auslieferung auf andere Rechner sollte ein eigenes Programm-Icon erstellt, die App umbenannt und eine echte Windows-Setupdatei gebaut werden.
- Aktion:
  - Ein neues Programm-Icon auf Basis von `Lumos.png` wurde erstellt und als `.ico` und `.png` gespeichert.
  - Die sichtbaren Programmtitel und Projektmetadaten wurden auf `Lumos - LernApp` umgestellt.
  - Die Build- und Startskripte wurden von festen lokalen Pfaden geloest und auf die installierte `dotnet`-Umgebung umgestellt.
  - Die App wurde als `self-contained win-x64`-Build veroeffentlicht.
  - Mit `Inno Setup` wurde eine echte Windows-Setupdatei erzeugt.
- Ergebnis:
  - Die App kann jetzt als eigenstaendiges Windows-Programm ausgeliefert werden.
  - Die Setupdatei wurde erfolgreich erstellt als `Lumos-LernApp-Setup-1.0.0.exe`.
- Naechster Schritt: Installer auf einem zweiten Rechner oder in einem frischen Benutzerkonto testen.

### 2026-04-30 - Spielerdaten in LocalAppData verlagert
- Anfrage: Die App sollte auf anderen Rechnern sauber installierbar sein und Spielstaende nicht mehr vom Startordner der EXE abhaengig machen.
- Aktion:
  - Die Speicherung der Spielerprofile wurde aus dem Anwendungsverzeichnis nach `%LocalAppData%\\Lumos-LernApp\\Data` verschoben.
  - Beim ersten Start wird eine vorhandene `players.json` aus einem alten App-Ordner automatisch in den neuen Speicherort uebernommen.
- Ergebnis:
  - Installierte Versionen koennen Spielerdaten jetzt ohne Schreibprobleme speichern.
  - Test-Builds und Produktiv-Builds greifen kuenftig nicht mehr versehentlich auf unterschiedliche lokale `Data`-Ordner zu.
- Naechster Schritt: Migration mit bestehendem Profil einmal live pruefen.

### 2026-04-30 - Klasse-4-Meisterinsel beim Laden des Weltfortschritts korrigiert
- Anfrage: Die komplette Klasse-4- und Meisterinsel-Logik sollte technisch geprueft werden.
- Fund:
  - Beim Laden bestehender Weltfortschritte wurde die Bonus-/Meisterinsel in `Klasse 4` noch wie eine `5`-Level-Insel behandelt.
  - Im aktiven Spielfluss waren zwar `3` Bonus-Level hinterlegt, gespeicherte Fortschritte konnten beim spaeteren Laden aber inkonsistent werden.
- Aktion:
  - Die Lade- und Bereinigungslogik fuer Weltfortschritte wurde so angepasst, dass die Bonusinsel in `Klasse 3` und `Klasse 4` korrekt auf `3` Level begrenzt wird.
  - Der Build wurde danach erfolgreich neu ausgefuehrt.
  - Anschliessend wurde auch die Setupdatei mit diesem Fix neu gebaut.
- Ergebnis:
  - Die Meisterinsel von `Klasse 4` bleibt jetzt auch nach dem Neuladen eines Spielstandes technisch konsistent.
  - Der aktuelle Installer enthaelt den Fix bereits.
- Naechster Schritt: Meisterinsel in einer echten Spielsitzung bis zum Extra-Stern einmal komplett durchspielen.

### 2026-04-30 - UTF-8-Darstellung fuer Quellen und Installer gehaertet
- Anfrage: Nach der Installation auf einem Test-PC wurden echte Umlaute nicht ueberall korrekt dargestellt. Alle relevanten Dateien inklusive Setupdatei sollten geprueft werden.
- Aktion:
  - Alle textbasierten Projektdateien unter `Views`, `Services`, `Installer` sowie die Root-Skripte und Dokumentationsdateien wurden als rohe UTF-8-Inhalte geprueft.
  - Verbliebene kaputte Ersatzzeichen in der Dokumentation wurden bereinigt.
  - Die relevanten Quell-, Skript- und Installerdateien wurden explizit auf `UTF-8 mit BOM` normalisiert.
  - Die Publish-Version und die Setupdatei wurden anschliessend komplett neu gebaut.
- Ergebnis:
  - In den geprueften Textdateien gibt es keine echten Mojibake-Treffer und keine ersetzten Umlaute in Woertern mehr.
  - Das Inno-Setup-Skript liegt jetzt ebenfalls eindeutig als `UTF-8 mit BOM` vor, damit der Installer Umlaute auf anderen PCs verlaesslich uebernimmt.
  - Die aktualisierte Setupdatei wurde neu erzeugt.
- Naechster Schritt: Den neuen Installer auf dem Test-PC erneut installieren und die Umlaute in Setup und App noch einmal live pruefen.
