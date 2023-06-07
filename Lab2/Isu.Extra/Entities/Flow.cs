using Isu.Entities;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Entities;

public class Flow
{
    public const int MaxNumOfStudentsInFlow = 119;
    private readonly List<Student> _students;

    public Flow(Course course, string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(course);

        _students = new List<Student>();
        Course = course;
        Name = name;
        Schedule = new Schedule();
    }

    public IReadOnlyCollection<Student> Students => _students;

    public Schedule Schedule { get; }
    public string Name { get; }
    public Course Course { get; }

    public void AddStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(_students);
        ArgumentNullException.ThrowIfNull(student);
        ArgumentNullException.ThrowIfNull(student.Group);

        if (_students.Count == MaxNumOfStudentsInFlow)
        {
            throw FlowException.NotEnoughPlacesOnFlow(Name);
        }

        if (student.Group.GroupName[Group.FacultySymbol] == Course.Faculty.Letter)
        {
            throw FlowException.SameFaculty(Course.Faculty.Letter);
        }

        if (Course.Flows.Any(x => x.Students.Contains(student)))
        {
            throw FlowException.ReEnrollment(Course.Name);
        }

        _students.Add(student);
    }

    public void RemoveStudent(Student student)
    {
        ArgumentNullException.ThrowIfNull(_students);
        ArgumentNullException.ThrowIfNull(student);

        if (!_students.Remove(student))
        {
            throw FlowException.NoSuchStudent(student.Id);
        }
    }
}