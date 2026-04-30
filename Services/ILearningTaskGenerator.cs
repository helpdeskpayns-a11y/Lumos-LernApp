using System.Collections.Generic;
using SpieleLernApp.Models;

namespace SpieleLernApp.Services;

public interface ILearningTaskGenerator
{
    LearningSubject Subject { get; }

    IReadOnlyList<LearningTask> GenerateTasks(TaskGenerationRequest request);
}
