using System.Net.Sockets;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class Schedule
{
    private readonly List<EducationalDay> _educationalDays;

    public Schedule()
    {
        _educationalDays = new List<EducationalDay>(7);
        for (int i = 0; i < _educationalDays.Capacity; i++)
        {
            _educationalDays.Add(new EducationalDay());
        }
    }

    public IReadOnlyCollection<EducationalDay> EducationalDays => _educationalDays;

    public void AddLessonToDay(Lesson lesson, DayOfTheWeek day)
    {
        _educationalDays[(int)day].AddLesson(lesson);
    }

    public void RemoveLessonFromDay(Lesson lesson, DayOfWeek day)
    {
        _educationalDays[(int)day].RemoveLesson(lesson);
    }
}