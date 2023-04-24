using System.Net.Sockets;
using Isu.Models;

namespace Isu.Entities;

public class Group
{
    public const int MaxNumOfStudentsInGroup = 30;

    private readonly List<Student> _students;

    private Group(string groupName, CourseNumber courseNumber)
    {
        GroupName = groupName;
        CourseNumber = courseNumber;
        _students = new List<Student>();
    }

    public string GroupName { get; }

    public CourseNumber CourseNumber { get; }

    public IReadOnlyCollection<Student> Students => _students;

    public static bool TryCreate(string groupName, out Group? group)
    {
        group = null;
        if (groupName == null)
        {
            throw new ArgumentNullException();
        }

        if (!groupName.StartsWith("M") || groupName.Length is not(5 or 6) || groupName[1] != '3' ||
            !int.TryParse(groupName[2].ToString(), out int course) || course is not(>= 1 and <= 4))
        {
            return false;
        }

        group = new Group(groupName, (CourseNumber)course);

        return true;
    }

    public void Add(Student student)
    {
        _students.Add(student);
    }

    public void Remove(Student student)
    {
        _students.Remove(student);
    }
}