# NRW-Lehrplan als Grundlage fuer die SpieleLernApp

## Entscheidung
Ja, der Lehrplan NRW fuer die Primarstufe sollte als zentrale Planungsgrundlage in die App einfliessen.

## Warum das sinnvoll ist
- Die Inhalte werden fachlich sauber an die Grundschule in Nordrhein-Westfalen angebunden.
- Die App kann Aufgaben nicht nur nach Klasse, sondern nach Kompetenzen aufbauen.
- Eltern und Lehrkraefte verstehen besser, warum bestimmte Inhalte in Klasse 1 bis 4 auftauchen.
- Spaetere Erweiterungen koennen geordnet nach Fach, Kompetenzbereich und Lernstand erfolgen.

## Offizielle Grundlage
Stand unserer Planung:
- Lehrplannavigator Primarstufe NRW mit den aktuellen Lehrplaenen
- Richtlinien 2024, laut Lehrplannavigator in Kraft seit `01.08.2025`
- Lehrplan-Sammelband Primarstufe mit Stand `18.03.2025` auf dem Lehrplannavigator

Quellen:
- https://lehrplannavigator.nrw.de/lehrplannavigator-primarstufe-richtlinien-und-lehrplaene
- https://lehrplannavigator.nrw.de/system/files/media/document/file/ps_lp_sammelband_2025_03_18_0.pdf
- https://www.schulministerium.nrw/schule-bildung/schulorganisation/richtlinien-und-lehrplaene

## Was fuer die App besonders wichtig ist

### 1. Inhalte nicht nur nach Klassen, sondern nach Kompetenzstufen planen
Der Lehrplannavigator beschreibt Kompetenzerwartungen fuer:
- das Ende der Schuleingangsphase
- das Ende der Klasse 4

Das passt sehr gut zu einer Lern-App. Wir koennen Aufgaben deshalb doppelt ordnen:
- nach Klasse
- nach Kompetenzbereich

## 2. Empfehlenswerte Startfaecher
Fuer eine erste Version sind besonders geeignet:
- Mathematik
- Deutsch
- Sachunterricht

Diese Faecher sind im Primarstufen-Lehrplannavigator besonders gut mit Material, Beispielen und Kompetenzbereichen beschrieben.

## 3. Sinnvolle Struktur fuer Mathematik
Aus dem offiziellen Lehrplan ergeben sich fuer Mathematik diese zentralen Bereiche:
- Prozesse: Problemlosen, Modellieren, Kommunizieren, Argumentieren, Darstellen
- Inhalte: Zahlen und Operationen, Raum und Form, Groessen und Messen, Daten, Haufigkeiten, Wahrscheinlichkeiten

Folgerung fuer die App:
- Wir sollten nicht nur Rechenaufgaben bauen.
- Die App sollte auch Denken, Erkunden, Darstellen und Begruenden foerdern.

## 4. Sinnvolle Struktur fuer Deutsch
Aus dem offiziellen Lehrplan ergeben sich fuer Deutsch diese grossen Bereiche:
- Sprechen und Zuhoeren
- Schreiben
- Lesen - mit Texten und Medien umgehen
- Sprache und Sprachgebrauch untersuchen

Folgerung fuer die App:
- Wir koennen spaeter nicht nur Lesequizze bauen.
- Auch Zuhoeraufgaben, Wortschatz, Rechtschreibung, Sprachmuster und kleine Schreibanlaesse passen gut.

## 5. Sinnvolle Struktur fuer Sachunterricht
Aus dem offiziellen Lehrplan ergeben sich fuer Sachunterricht u. a. diese Bereiche:
- Demokratie und Gesellschaft
- Natur und Umwelt
- Raum und Mobilitaet

Folgerung fuer die App:
- Sachunterricht kann spaeter sehr spielerisch umgesetzt werden.
- Besonders geeignet sind Bildaufgaben, Zuordnung, Reihenfolgen, Beobachtungsfragen und Alltagswissen.

## Empfehlung fuer unser Projekt
Wir sollten die App von Anfang an lehrplanorientiert aufbauen, aber fuer Version 1 den Umfang klein halten.

Empfohlener Start:
- Fach 1: Mathematik
- Zielbereich zuerst: Schuleingangsphase und Klasse 3/4 getrennt denken
- Erster Inhaltsbereich: Zahlen und Operationen
- Erster Spieltyp: Quiz mit Bildunterstuetzung
- Spaeter erweiterbar um Deutsch und Sachunterricht

## Vorschlag fuer das Datenmodell
Jede Aufgabe sollte spaeter mindestens diese Angaben haben:
- Fach
- Klassenstufe oder Lernstufe
- Lehrplanbereich
- Teilkompetenz
- Aufgabentyp
- Schwierigkeitsgrad
- richtige Antwort
- Feedbacktext

Beispiel:
- Fach: Mathematik
- Lernstufe: Schuleingangsphase
- Lehrplanbereich: Zahlen und Operationen
- Teilkompetenz: Additions- und Subtraktionsaufgaben im Zahlenraum bis 100
- Aufgabentyp: Quiz

## Konkrete Auswirkung auf die Planung
Wenn wir den Lehrplan einbeziehen, brauchen wir als Naechstes:
1. Auswahl der Startfaecher fuer Version 1
2. Auswahl der Kompetenzbereiche fuer Version 1
3. Definition eines Aufgabenrasters pro Klasse oder Lernstufe
4. Festlegung, wie die App Lernfortschritt pro Kompetenz speichert

## Meine Empfehlung
Fuer die erste lauffaehige Version:
- NRW-Lehrplan als feste Inhaltsbasis
- Start nur mit Mathematik
- Inhalte zuerst entlang von `Zahlen und Operationen`
- spaeter Ausbau mit Deutsch und Sachunterricht
- Darstellung in der App kinderfreundlich, intern aber sauber nach Lehrplan strukturiert
