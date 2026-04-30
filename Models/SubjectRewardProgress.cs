namespace SpieleLernApp.Models;

public sealed class SubjectRewardProgress
{
    public int ClassLevel { get; set; }

    public LearningSubject Subject { get; set; }

    public RewardTrack Track { get; set; }

    public int EarnedPoints { get; set; }

    public DateTime CompletedAt { get; set; } = DateTime.Now;
}
