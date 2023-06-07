namespace Isu.Exceptions;

public class InvalidGroupNameException : Exception
{
    public InvalidGroupNameException(string groupName)
        : base(message: $"Invalid name for group : {groupName}")
    { }
}