namespace SpieleLernApp.Models;

public sealed class TaskGenerationRequest
{
    public LearningSubject Subject { get; init; }

    public int ClassLevel { get; init; }

    public int TaskCount { get; init; } = 5;

    public RewardTrack Track { get; init; } = RewardTrack.PortalTrophies;

    public int? Seed { get; init; }
}
