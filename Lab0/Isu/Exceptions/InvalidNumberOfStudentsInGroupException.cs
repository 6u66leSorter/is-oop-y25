namespace Isu.Exceptions
{
    public class InvalidNumberOfStudentsInGroupException : Exception
    {
        public InvalidNumberOfStudentsInGroupException(string groupName)
            : base(message: $"The limit of students in the group {groupName} has been exceeded")
        { }
    }
}