namespace Isu.Extra.Exceptions;

public class FacultyLetterException : Exception
{
    private FacultyLetterException(string message)
        : base(message) { }
    public static FacultyLetterException InvalidSymbol(char symbol)
        => new FacultyLetterException($"Symbol ({symbol}) is not a upper case letter");
    public static FacultyLetterException NoSuchLetter(char symbol)
        => new FacultyLetterException($"Invalid letter");
}