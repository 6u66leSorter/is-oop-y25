using Isu.Extra.Exceptions;

namespace Isu.Extra.Entities;

public class FacultyLetter
{
    public FacultyLetter(char letter)
    {
        if (!char.IsUpper(letter) || !char.IsLetter(letter))
        {
            throw FacultyLetterException.InvalidSymbol(letter);
        }

        Letter = letter;
    }

    public char Letter { get; }
}