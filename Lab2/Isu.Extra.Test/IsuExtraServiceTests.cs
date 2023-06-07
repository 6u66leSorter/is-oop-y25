using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Services;
using Isu.Models;
using Xunit;

namespace Isu.Extra.Test;

public class IsuExtraServiceTests
{
    private readonly IsuExtraService _service;

    public IsuExtraServiceTests()
    {
        _service = new IsuExtraService();
    }

    [Fact]
    public void AddStudentToCourse_StudentAdded()
    {
        FacultyLetter faculty = _service.AddFacultyLetter('U');
        FacultyLetter faculty1 = _service.AddFacultyLetter('M');
        StudyGroup studyGroup = _service.AddStudyGroup("M32091");
        Student student = _service.AddStudent(studyGroup, "Иван", "Алейников");
        Course course = _service.AddCourse(faculty, "cringe");
        var flow = new Flow(course, "mem");

        _service.EnrollStudentInCourse(student, flow);

        Assert.Contains(student, flow.Students);
    }

    [Fact]
    public void AddStudentToCourse_ThrowFlowException()
    {
        FacultyLetter faculty1 = _service.AddFacultyLetter('M');
        FacultyLetter faculty = _service.AddFacultyLetter('U');
        StudyGroup studyGroup1 = _service.AddStudyGroup("M32091");
        StudyGroup studyGroup2 = _service.AddStudyGroup("M32071");
        StudyGroup studyGroup3 = _service.AddStudyGroup("M32021");
        StudyGroup studyGroup4 = _service.AddStudyGroup("M32001");
        Course course = _service.AddCourse(faculty, "cringe");
        var flow = new Flow(course, "mem");
        var students = new List<Student>();
        for (int i = 0; i < Group.MaxNumOfStudentsInGroup; i++)
        {
            Student student1 = _service.AddStudent(studyGroup1, $"Иван{i}", "Алейников");
            Student student2 = _service.AddStudent(studyGroup2, $"Иван{i}", "Алейников");
            Student student3 = _service.AddStudent(studyGroup3, $"Иван{i}", "Алейников");
            Student student4 = _service.AddStudent(studyGroup4, $"Иван{i}", "Алейников");

            students.Add(student1);
            students.Add(student2);
            students.Add(student3);
            students.Add(student4);
        }

        for (int i = 0; i < students.Count - 1; i++)
        {
            _service.EnrollStudentInCourse(students[i], flow);
        }

        Assert.Throws<FlowException>(() => _service.EnrollStudentInCourse(students[119], flow));
    }

    [Fact]
    public void RemoveStudentFromCourse_StudentRemoved()
    {
        FacultyLetter faculty1 = _service.AddFacultyLetter('M');
        FacultyLetter faculty = _service.AddFacultyLetter('U');
        StudyGroup studyGroup = _service.AddStudyGroup("M32091");
        Student student = _service.AddStudent(studyGroup, "Иван", "Алейников");
        Course course = _service.AddCourse(faculty, "cringe");
        var flow = new Flow(course, "mem");

        _service.EnrollStudentInCourse(student, flow);
        _service.UnsubscribeStudentFromCourse(student, flow);

        Assert.DoesNotContain(student, flow.Students);
    }

    [Fact]
    public void GetFlowsFromCourse_FlowsGot()
    {
        FacultyLetter faculty1 = _service.AddFacultyLetter('M');
        FacultyLetter faculty = _service.AddFacultyLetter('U');
        Course course = _service.AddCourse(faculty, "cringe");
        for (int i = 0; i < 10; i++)
        {
            var flow = new Flow(course, $"mem{i}");
        }

        var flows = _service.GetCourseFlows(course).ToList();

        Assert.Equal(flows, course.Flows.ToList());
    }

    [Fact]
    public void GetStudentsWithoutCourse_StudentsGot()
    {
        FacultyLetter faculty1 = _service.AddFacultyLetter('M');
        FacultyLetter faculty = _service.AddFacultyLetter('U');
        StudyGroup studyGroup1 = _service.AddStudyGroup("M32091");
        Course course = _service.AddCourse(faculty, "cringe");
        var flow = new Flow(course, "mem");
        var students = new List<Student>();
        for (int i = 0; i < Group.MaxNumOfStudentsInGroup; i++)
        {
            Student student1 = _service.AddStudent(studyGroup1, $"Иван{i}", "Алейников");

            students.Add(student1);
        }

        for (int i = 0; i < Group.MaxNumOfStudentsInGroup - 1; i++)
        {
            _service.EnrollStudentInCourse(students[i], flow);
        }

        var studentsWithoutCourse = _service.GetStudentsWithoutCourseFromGroup(studyGroup1).ToList();

        Assert.Contains(students[29], studyGroup1.Group.Students);
        Assert.DoesNotContain(students[29], flow.Students);
        Assert.Contains(students[29], studentsWithoutCourse);
    }

    [Fact]
    public void AddStudentToCourse_ThrowScheduleException()
    {
        FacultyLetter faculty1 = _service.AddFacultyLetter('M');
        FacultyLetter faculty = _service.AddFacultyLetter('U');
        StudyGroup studyGroup = _service.AddStudyGroup("M32091");
        Student student = _service.AddStudent(studyGroup, "Иван", "Алейников");
        Course course = _service.AddCourse(faculty, "cringe");
        var flow = new Flow(course, "mem");
        Teacher teacher1 = _service.AddTeacher("abuga", "gaga");
        Teacher teacher2 = _service.AddTeacher("a", "g");
        Lesson lesson1 = _service.AddLesson(teacher1, LessonNumber.First, "228332");
        Lesson lesson2 = _service.AddLesson(teacher2, LessonNumber.First, "23947");

        studyGroup.Schedule.AddLessonToDay(lesson1, DayOfTheWeek.Monday);
        flow.Schedule.AddLessonToDay(lesson2, DayOfTheWeek.Monday);
        Assert.Throws<ScheduleException>(() => _service.EnrollStudentInCourse(student, flow));

        Assert.DoesNotContain(student, flow.Students);
    }
}