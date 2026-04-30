# Planungsgrundlage: SpieleLernApp

## Ziel
Diese Datei sammelt die Informationen, die wir fuer die Planung und den Bau der Windows-SpieleLernApp benoetigen.

## Kerninformationen, die wir festlegen sollten

### 1. Zielgruppe
Wir sollten festlegen:
- Fuer welche Altersgruppe ist die App gedacht?
- Ist die App fuer Grundschule allgemein oder genau fuer Klasse 1 bis 4?
- Soll die Bedienung auch fuer Kinder funktionieren, die noch wenig lesen koennen?

### 2. Lernziele
Wir sollten festlegen:
- Welche Faecher sollen enthalten sein?
- Was soll zuerst gebaut werden: Mathe, Deutsch, Englisch oder Logik?
- Soll jede Klasse eigene Inhalte haben oder sollen Inhalte nach Schwierigkeitsgrad freigeschaltet werden?

### 3. Spielarten
Wir sollten festlegen:
- Welche Spieltypen soll die App haben?
- Welche Spielart soll zuerst umgesetzt werden?

Moegliche erste Spielarten:
- Quiz mit Antwortauswahl
- Zuordnungsaufgaben
- Bilder-Memory
- Rechenaufgaben
- Woerter lesen und richtig auswaehlen

### 4. App-Ablauf
Wir sollten festlegen, wie der Hauptfluss aussieht.

Empfohlener Ablauf:
1. Ladebildschirm
2. Startseite
3. Klassenwahl
4. Fachauswahl
5. Spielauswahl
6. Spielrunde
7. Ergebnisbildschirm mit Punkten oder Sternen

### 5. Design und Bilder
Wir sollten festlegen:
- Welche vorhandenen Bilder wofuer verwendet werden?
- Soll `Lumos.png` als feste Figur oder Helfer auftauchen?
- Soll `neutraler Ort.png` als allgemeiner Hintergrund dienen?
- Soll `Ladebild.png` immer beim App-Start gezeigt werden?

### 6. Motivation und Belohnung
Wir sollten festlegen:
- Sollen Kinder Sterne, Punkte oder Medaillen bekommen?
- Gibt es Levels oder nur Ergebnisse pro Runde?
- Sollen Inhalte freigeschaltet werden?

### 7. Inhalte und Daten
Wir sollten festlegen:
- Woher kommen Fragen und Aufgaben?
- Sollen die Inhalte direkt in Dateien gespeichert werden?
- Soll jede Klasse einen eigenen Fragenkatalog bekommen?
- Soll spaeter ein Bereich fuer eigene Aufgaben durch Eltern oder Lehrkraefte moeglich sein?

### 8. Sprache und Bedienung
Wir sollten festlegen:
- Soll die App komplett auf Deutsch sein?
- Sollen Symbole und Bilder wichtiger sein als Text?
- Brauchen wir Vorlese-Funktionen oder Sound-Hinweise?

### 9. Technik
Wir sollten festlegen:
- Wollen wir eine klassische Windows-Desktop-App bauen?
- Soll die App offline funktionieren?
- Sollen Fortschritte lokal auf dem PC gespeichert werden?

Empfehlung:
- Plattform: Windows Desktop
- Technik: WPF mit .NET 8
- Betrieb: offline faehig
- Speicherung: lokal in JSON-Dateien

### 10. Startumfang fuer Version 1
Wir sollten festlegen, was in die erste nutzbare Version kommt.

Empfohlener MVP:
- Startseite
- Klassenwahl mit Bildern
- Ein Fach
- Ein Spieltyp
- Punkte- oder Sternesystem
- Lokale Fortschrittsspeicherung

## Konkrete Entscheidungen, die wir als Naechstes brauchen

Bitte festlegen:
- Erstes Fach
- Erster Spieltyp
- Zielgruppe
- Belohnungssystem
- Offline ja oder nein

## Meine Empfehlung fuer einen guten Start
- Zielgruppe: Grundschule Klasse 1 bis 4
- Sprache: Deutsch
- Erstes Fach: Mathe
- Erster Spieltyp: Quiz
- Belohnung: Sterne und Punkte
- Technik: WPF mit .NET 8
- Speicherung: lokal auf dem Rechner
- Einsatz der Bilder:
  - `Ladebild.png` als Splash- oder Ladebildschirm
  - `Lumos.png` als Begleiter
  - `Klasse 1.png` bis `KLasse 4.png` fuer die Klassenwahl
  - `neutraler Ort.png` als Hintergrund fuer neutrale Lernseiten

## Ergebnis dieser Datei
Wenn diese Punkte festgelegt sind, koennen wir als Naechstes:
- die Seitenstruktur definieren
- das Datenmodell planen
- das Projektgeruest anlegen
- die erste lauffaehige Windows-App bauen
