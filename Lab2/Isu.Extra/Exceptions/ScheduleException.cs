namespace Isu.Extra.Exceptions;

public class ScheduleException : Exception
{
    private ScheduleException(string message)
        : base(message) { }
    public static ScheduleException IntersectionOfLessons()
        => new ScheduleException($"There is an intersection of lessons");
}