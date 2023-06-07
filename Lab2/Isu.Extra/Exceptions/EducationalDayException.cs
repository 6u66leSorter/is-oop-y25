using Isu.Extra.Entities;
using Isu.Extra.Models;

namespace Isu.Extra.Exceptions;

public class EducationalDayException : Exception
{
    private EducationalDayException(string message)
        : base(message) { }
    public static EducationalDayException BusyTimeSlot(LessonNumber number)
        => new EducationalDayException($"{number} lesson is already exist at this day");
    public static EducationalDayException FailedToDeleteLesson()
        => new EducationalDayException($"No such lesson in this day");
}