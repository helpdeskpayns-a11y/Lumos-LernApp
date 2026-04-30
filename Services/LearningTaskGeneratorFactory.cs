using System;
using System.Collections.Generic;
using System.Linq;
using SpieleLernApp.Models;

namespace SpieleLernApp.Services;

public sealed class LearningTaskGeneratorFactory
{
    private readonly Dictionary<LearningSubject, ILearningTaskGenerator> _generators;

    public LearningTaskGeneratorFactory()
    {
        _generators = new Dictionary<LearningSubject, ILearningTaskGenerator>
        {
            [LearningSubject.Math] = new MathTaskGenerator(),
            [LearningSubject.German] = new GermanTaskGenerator(),
            [LearningSubject.Logic] = new LogicTaskGenerator(),
            [LearningSubject.English] = new EnglishTaskGenerator(),
            [LearningSubject.Art] = new ArtTaskGenerator(),
            [LearningSubject.Media] = new MediaTaskGenerator(),
            [LearningSubject.Creativity] = new CreativityTaskGenerator(),
            [LearningSubject.Sachunterricht] = new SachkundeTaskGenerator(),
            [LearningSubject.Music] = new MusicTaskGenerator()
        };
    }

    public IReadOnlyList<LearningTask> GenerateTasks(TaskGenerationRequest request)
    {
        if (!_generators.TryGetValue(request.Subject, out ILearningTaskGenerator? generator))
        {
            throw new NotSupportedException($"Für das Fach {request.Subject} gibt es noch keinen Aufgaben-Generator.");
        }

        TaskGenerationRequest validationRequest = new()
        {
            Subject = request.Subject,
            ClassLevel = request.ClassLevel,
            Track = request.Track,
            Seed = request.Seed,
            TaskCount = 1000
        };

        IReadOnlyList<LearningTask> generatedTasks = generator.GenerateTasks(validationRequest);
        IReadOnlyList<LearningTask> validTasks = LearningTaskValidationService.FilterValidTasks(generatedTasks);

        return validTasks
            .Take(request.TaskCount)
            .ToList();
    }
}
