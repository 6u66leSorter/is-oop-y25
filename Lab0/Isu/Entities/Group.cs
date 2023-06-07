using System.Net.Sockets;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities;

public class Group
{
    public const int MaxNumOfStudentsInGroup = 30;
    public const int FacultySymbol = 0;
    private const int DegreeSymbol = 1;
    private const int CourseSymbol = 2;
    private const int MinLenght = 5;
    private const int MaxLenght = 6;
    private const char Bachelor = '3';

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
        if (string.IsNullOrWhiteSpace(groupName))
        {
            throw new InvalidGroupNameException(groupName);
        }

        if (groupName[DegreeSymbol] != Bachelor
            || !char.IsLetter(groupName[FacultySymbol])
            || groupName.Length is not(MinLenght or MaxLenght)
            || !int.TryParse(groupName[CourseSymbol].ToString(), out int course)
            || course is not(>= (int)CourseNumber.First and <= (int)CourseNumber.Fourth))
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