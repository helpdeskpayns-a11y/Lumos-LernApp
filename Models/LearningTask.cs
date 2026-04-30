namespace SpieleLernApp.Models;

public sealed class LearningTask
{
    public string Title { get; init; } = string.Empty;

    public string Prompt { get; init; } = string.Empty;

    public string[] Answers { get; init; } = [];

    public int CorrectAnswerIndex { get; init; }

    public string SuccessText { get; init; } = string.Empty;

    public string Topic { get; init; } = string.Empty;

    public LearningSubject Subject { get; init; }

    public int ClassLevel { get; init; }
}
