namespace Isu.Entities;

public class Student
{
    private Student(int id, string firstName, string lastName, Group group)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Group = group;
    }

    public int Id { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public Group? Group { get; set; }

    public static bool TryCreate(int id, string firstName, string lastName, Group group, out Student? student)
    {
        student = null;
        ArgumentNullException.ThrowIfNull(firstName);
        ArgumentNullException.ThrowIfNull(lastName);
        ArgumentNullException.ThrowIfNull(group);

        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            return false;
        }

        student = new Student(id, firstName, lastName, group);
        return true;
    }
}