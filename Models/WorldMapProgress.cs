namespace SpieleLernApp.Models;

public sealed class WorldMapProgress
{
    public int ClassLevel { get; set; }

    public LearningSubject Subject { get; set; }

    public int CompletedLevels { get; set; }

    public bool StarCollected { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
