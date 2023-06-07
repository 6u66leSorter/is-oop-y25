namespace Isu.Extra.Exceptions;

public class StudentException : Exception
{
    private StudentException(string message)
        : base(message) { }
    public static StudentException MaxAmountOfCoursesReached(string firstName, string lastName)
        => new StudentException($"{firstName} {lastName} is already enrolled in 2 courses");
}