using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Extra.Services;

public interface IIsuExtraService
{
    public StudyGroup AddStudyGroup(string groupName);
    public Student AddStudent(StudyGroup studyGroup, string firstName, string lastname);
    public Course AddCourse(FacultyLetter facultyName, string name);
    public Flow AddFlow(Course course, string name);
    public Teacher AddTeacher(string firstName, string lastName);
    public Lesson AddLesson(Teacher teacher, LessonNumber number, string room);
    public IReadOnlyCollection<Student> GetStudentsFromFlow(Flow flow);
    public IReadOnlyCollection<Student> GetStudentsWithoutCourseFromGroup(StudyGroup studyGroup);
    public IReadOnlyCollection<Flow> GetCourseFlows(Course course);
    public FacultyLetter AddFacultyLetter(char letter);
}