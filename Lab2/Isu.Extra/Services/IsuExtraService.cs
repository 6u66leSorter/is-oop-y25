using System.Threading.Tasks.Dataflow;
using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Models;
using Isu.Services;

namespace Isu.Extra.Services;

public class IsuExtraService : IIsuExtraService
{
    private const int MaxNumOfStudentCourses = 2;

    private readonly List<FacultyLetter> _validLetters;
    private readonly List<Course> _courses;
    private readonly List<StudyGroup> _studyGroups;
    private readonly List<Teacher> _teachers;
    private readonly List<Student> _students;

    private readonly IsuService _isuService;

    public IsuExtraService()
    {
        _students = new List<Student>();
        _isuService = new IsuService();
        _courses = new List<Course>();
        _studyGroups = new List<StudyGroup>();
        _teachers = new List<Teacher>();
        _validLetters = new List<FacultyLetter>();
    }

    public FacultyLetter AddFacultyLetter(char letter)
    {
        var symbol = new FacultyLetter(letter);

        _validLetters.Add(symbol);
        return symbol;
    }

    public StudyGroup AddStudyGroup(string groupName)
    {
        if (!_validLetters.Select(l => l.Letter).Contains(groupName[Group.FacultySymbol]))
        {
            throw FacultyLetterException.NoSuchLetter(groupName[Group.FacultySymbol]);
        }

        Group group = _isuService.AddGroup(groupName);
        var studyGroup = new StudyGroup(group, Guid.NewGuid());

        _studyGroups.Add(studyGroup);

        return studyGroup;
    }

    public Student AddStudent(StudyGroup studyGroup, string firstName, string lastname)
    {
        Student student = _isuService.AddStudent(studyGroup.Group, firstName, lastname);

        _students.Add(student);
        return student;
    }

    public Course AddCourse(FacultyLetter facultyName, string name)
    {
        var course = new Course(facultyName, name);

        _courses.Add(course);
        return course;
    }

    public Flow AddFlow(Course course, string name)
    {
        var flow = new Flow(course, name);

        course.AddFlow(flow);
        return flow;
    }

    public Teacher AddTeacher(string firstName, string lastName)
    {
        var teacher = new Teacher(firstName, lastName, Guid.NewGuid());

        _teachers.Add(teacher);
        return teacher;
    }

    public Lesson AddLesson(Teacher teacher, LessonNumber number, string room)
    {
        ArgumentNullException.ThrowIfNull(teacher);
        ArgumentNullException.ThrowIfNull(room);

        var lesson = new Lesson(teacher, Guid.NewGuid(), number, room);
        return lesson;
    }

    public void EnrollStudentInCourse(Student student, Flow flow)
    {
        ArgumentNullException.ThrowIfNull(student);

        StudyGroup studyGroup = _studyGroups
            .Find(x => x.Group.Students.Contains(student)) ?? throw new InvalidOperationException();

        if (studyGroup.Schedule.EducationalDays.Zip(flow.Schedule.EducationalDays)
            .Select(days => days.First.Lessons.Select(l1 => l1.Number)
                .IntersectBy(days.Second.Lessons.Select(l2 => l2.Number), lNumber => lNumber)
                .ToList())
            .Any(res => res.Count != 0))
        {
            throw ScheduleException.IntersectionOfLessons();
        }

        if (_courses.Where(c => c.Flows.Any(f => f.Students.Contains(student))).ToList().Count == MaxNumOfStudentCourses)
        {
            throw StudentException.MaxAmountOfCoursesReached(student.FirstName, student.LastName);
        }

        flow.AddStudent(student);
    }

    public void UnsubscribeStudentFromCourse(Student student, Flow flow)
    {
        ArgumentNullException.ThrowIfNull(student);
        ArgumentNullException.ThrowIfNull(flow);

        flow.RemoveStudent(student);
    }

    public IReadOnlyCollection<Flow> GetCourseFlows(Course course)
    {
        ArgumentNullException.ThrowIfNull(course);

        return course.Flows;
    }

    public IReadOnlyCollection<Student> GetStudentsFromFlow(Flow flow)
    {
        ArgumentNullException.ThrowIfNull(flow);

        return flow.Students;
    }

    public IReadOnlyCollection<Student> GetStudentsWithoutCourseFromGroup(StudyGroup studyGroup)
    {
        return studyGroup.Group.Students
            .Where(s => !_courses
                .SelectMany(c => c.Flows)
                .SelectMany(f => f.Students).Contains(s))
            .ToList();
    }
}