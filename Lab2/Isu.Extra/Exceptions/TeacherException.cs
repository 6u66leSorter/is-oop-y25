namespace Isu.Extra.Exceptions;

public class TeacherException : Exception
{
    private TeacherException(string message)
        : base(message) { }
    public static TeacherException InvalidFirstName(string firstName)
        => new TeacherException($"First name has an invalid value : {firstName}");
    public static TeacherException InvalidLastName(string lastName)
        => new TeacherException($"Last name has an invalid value : {lastName}");
}