using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using SpieleLernApp.Models;

namespace SpieleLernApp.Services;

public sealed class PlayerProfileStore
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };
    private readonly string _filePath;

    public PlayerProfileStore()
    {
        _filePath = Path.Combine(AppStoragePaths.DataDirectory, "players.json");
        MigrateLegacyPlayersFileIfNeeded();
    }

    private void MigrateLegacyPlayersFileIfNeeded()
    {
        if (File.Exists(_filePath))
        {
            return;
        }

        string legacyFilePath = Path.Combine(AppContext.BaseDirectory, "Data", "players.json");
        if (!File.Exists(legacyFilePath))
        {
            return;
        }

        File.Copy(legacyFilePath, _filePath);
    }

    public IReadOnlyList<PlayerProfile> LoadPlayers()
    {
        if (!File.Exists(_filePath))
        {
            return [];
        }

        string json = File.ReadAllText(_filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return [];
        }

        List<PlayerProfile>? players = JsonSerializer.Deserialize<List<PlayerProfile>>(json, SerializerOptions);
        if (players is null)
        {
            return [];
        }

        foreach (PlayerProfile player in players)
        {
            if (player.Id == Guid.Empty)
            {
                player.Id = Guid.NewGuid();
            }

            player.RewardProgress ??= [];
            player.WorldMapProgress ??= [];
            MigrateLegacyClass1Subjects(player);
            player.RewardProgress = player.RewardProgress
                .GroupBy(progress => new { progress.ClassLevel, progress.Subject, progress.Track })
                .Select(group =>
                {
                    SubjectRewardProgress bestProgress = group
                        .OrderByDescending(progress => progress.EarnedPoints)
                        .ThenBy(progress => progress.CompletedAt)
                        .First();

                    int rewardCap = bestProgress.Track == RewardTrack.WorldStars ? 1 : 10;
                    bestProgress.EarnedPoints = Math.Max(0, Math.Min(rewardCap, bestProgress.EarnedPoints));
                    return bestProgress;
                })
                .ToList();

            player.Trophies = player.RewardProgress
                .Where(progress => progress.Track == RewardTrack.PortalTrophies)
                .Sum(progress => progress.EarnedPoints);
            player.Stars = player.RewardProgress
                .Where(progress => progress.Track == RewardTrack.WorldStars)
                .Sum(progress => progress.EarnedPoints);

            player.WorldMapProgress = player.WorldMapProgress
                .GroupBy(progress => new { progress.ClassLevel, progress.Subject })
                .Select(group =>
                {
                    WorldMapProgress bestProgress = group
                        .OrderByDescending(progress => progress.CompletedLevels)
                        .ThenByDescending(progress => progress.StarCollected)
                        .ThenByDescending(progress => progress.UpdatedAt)
                        .First();

                    bestProgress.UsedTaskPrompts = group
                        .SelectMany(progress => progress.UsedTaskPrompts ?? [])
                        .Distinct(StringComparer.Ordinal)
                        .ToList();

                    int maxLevels = (bestProgress.ClassLevel == 3 || bestProgress.ClassLevel == 4) &&
                        bestProgress.Subject == LearningSubject.Bonus ? 3 : 5;
                    bestProgress.CompletedLevels = Math.Max(0, Math.Min(maxLevels, bestProgress.CompletedLevels));
                    bestProgress.StarCollected = bestProgress.StarCollected || bestProgress.CompletedLevels >= maxLevels;
                    return bestProgress;
                })
                .ToList();
        }

        return players
            .OrderByDescending(player => player.LastPlayedAt)
            .ThenBy(player => player.Name, StringComparer.CurrentCultureIgnoreCase)
            .ToList();
    }

    private static void MigrateLegacyClass1Subjects(PlayerProfile player)
    {
        foreach (SubjectRewardProgress progress in player.RewardProgress)
        {
            if (progress.ClassLevel != 1)
            {
                continue;
            }

            if (progress.Subject == LearningSubject.Logic)
            {
                progress.Subject = LearningSubject.Sachunterricht;
                continue;
            }

            if (progress.Subject == LearningSubject.English)
            {
                progress.Subject = LearningSubject.Music;
            }
        }

        foreach (WorldMapProgress progress in player.WorldMapProgress)
        {
            if (progress.ClassLevel != 1)
            {
                continue;
            }

            if (progress.Subject == LearningSubject.Logic)
            {
                progress.Subject = LearningSubject.Sachunterricht;
                continue;
            }

            if (progress.Subject == LearningSubject.English)
            {
                progress.Subject = LearningSubject.Music;
            }
        }
    }

    public void SavePlayer(PlayerProfile playerProfile)
    {
        List<PlayerProfile> players = LoadPlayers().ToList();
        int existingIndex = players.FindIndex(player => player.Id == playerProfile.Id);

        if (existingIndex >= 0)
        {
            players[existingIndex] = playerProfile;
        }
        else
        {
            players.Add(playerProfile);
        }

        SavePlayers(players);
    }

    private void SavePlayers(IEnumerable<PlayerProfile> playerProfiles)
    {
        string json = JsonSerializer.Serialize(playerProfiles, SerializerOptions);
        File.WriteAllText(_filePath, json);
    }
}
