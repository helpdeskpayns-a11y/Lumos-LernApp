namespace SpieleLernApp.Models;

public sealed class PlayerProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public string? AvatarPath { get; set; }

    public List<DiaryEntry> DiaryEntries { get; set; } = [];

    public List<SubjectRewardProgress> RewardProgress { get; set; } = [];

    public List<WorldMapProgress> WorldMapProgress { get; set; } = [];

    public int Trophies { get; set; }

    public int Stars { get; set; }

    public DateTime LastPlayedAt { get; set; } = DateTime.Now;
}
