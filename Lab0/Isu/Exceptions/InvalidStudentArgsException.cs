namespace Isu.Exceptions
{
    public class InvalidStudentArgsException : Exception
    {
        public InvalidStudentArgsException(string firstName, string lastName)
        : base(message: $"Invalid firstName ({firstName}) or lastName ({lastName})")
        { }
    }
}
