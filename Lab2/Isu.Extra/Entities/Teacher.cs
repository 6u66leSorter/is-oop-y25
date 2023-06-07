using Isu.Extra.Exceptions;

namespace Isu.Extra.Entities;

public class Teacher
{
    public Teacher(string firstName, string lastName, Guid id)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw TeacherException.InvalidFirstName(firstName);

        if (string.IsNullOrWhiteSpace(lastName)) throw TeacherException.InvalidLastName(lastName);

        FirstName = firstName;
        LastName = lastName;
        Id = id;
    }

    public string FirstName { get; }

    public string LastName { get; }

    public Guid Id { get; }
}