# SpieleLernApp

## Aktueller Stand
Dieses Projekt enthaelt jetzt ein erstes WPF-Grundgeruest fuer eine Windows-SpieleLernApp mit:
- sichtbarem Ladebildschirm auf Basis von `Bilder\Ladebild.png`
- Ladebalken von `0 %` bis `100 %`
- anschliessender Eingabe von Spielername und Alter
- Startmenue auf Basis von `Bilder\neutraler Ort.png`
- robuster Bildladung ueber Dateipfade aus dem Ordner `Bilder`
- interaktivem Startmenue mit klickbaren Bereichen fuer Profil, Belohnungen, Tagebuch, Tagesziel, Lernportal, Welten und Truhe
- Klassenauswahl mit `Klassenauswahl.png` und den Bildern fuer Klasse 1 bis 4
- expliziter Steuerung des App-Starts, damit der Wechsel vom Ladefenster ins Startmenue stabil bleibt
- gespeicherter Spielerverwaltung mit Auswahl vorhandener Profile oder Neuanlage beim Start

## Voraussetzung zum Starten
Zum Bauen und Starten wird ein .NET-SDK benoetigt.

Empfohlen:
- .NET 8 SDK

## Installierter Stand auf diesem Rechner
Am `22.04.2026` wurde das .NET 8 SDK lokal auf `G:\dotnet` installiert.

Installiert:
- .NET SDK `8.0.420`
- Microsoft.NETCore.App `8.0.26`
- Microsoft.WindowsDesktop.App `8.0.26`
- Microsoft.AspNetCore.App `8.0.26`

Zusaetzlich wurde `DOTNET_ROOT=G:\dotnet` fuer das Benutzerprofil gesetzt und `G:\dotnet` zum Benutzer-`Path` hinzugefuegt.

## Geplanter Ablauf
1. App startet mit Ladebildschirm
2. Ladebalken laeuft sichtbar bis `100 %`
3. Vorhandene Spieler koennen geladen oder ein neuer Spieler kann angelegt werden
4. Nach Auswahl oder Neuanlage oeffnet sich das Startmenue

## Hinweis zur lokalen Ausfuehrung
Das Projekt wurde nach der Installation erfolgreich gebaut.

Direkte Hilfsskripte:
- `build-app.ps1`
- `start-app.ps1`

Beispiel:
```powershell
powershell -ExecutionPolicy Bypass -File C:\Users\Kolkh\Documents\SpieleLernApp\build-app.ps1
```

## Projektstruktur
- `SpieleLernApp.csproj` - WPF-Projektdatei
- `App.xaml` - App-Einstieg
- `Views\StartupWindow.xaml` - Ladebildschirm und Spielerabfrage
- `Views\MainMenuWindow.xaml` - Startmenue
- `Models\PlayerProfile.cs` - Datenmodell fuer den Spieler
- `Bilder\` - vorhandene Bilddateien
