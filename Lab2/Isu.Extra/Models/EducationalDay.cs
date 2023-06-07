using System.Runtime.ExceptionServices;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;

namespace Isu.Extra.Models;

public class EducationalDay
{
    private readonly List<Lesson> _lessons;

    public EducationalDay()
    {
        _lessons = new List<Lesson>();
    }

    public IReadOnlyCollection<Lesson> Lessons => _lessons;

    public void AddLesson(Lesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);

        if (_lessons.Any(x => x.Number == lesson.Number))
        {
            throw EducationalDayException.BusyTimeSlot(lesson.Number);
        }

        _lessons.Add(lesson);
    }

    public void RemoveLesson(Lesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);

        if (!_lessons.Remove(lesson))
        {
            throw EducationalDayException.FailedToDeleteLesson();
        }
    }
}