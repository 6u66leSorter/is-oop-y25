using Isu.Entities;
using Isu.Models;

namespace Isu.Services;

public interface IIsuService
{
    Student AddStudent(Group group, string firstName, string lastName);
    Group? FindGroup(string groupName);
    Student GetStudent(int id);
    Student? FindStudent(int id);
    IReadOnlyCollection<Student> FindStudents(string groupName);
    IReadOnlyCollection<Student> FindStudents(CourseNumber courseNumber);
    IReadOnlyCollection<Group> FindGroups(CourseNumber courseNumber);

    void ChangeStudentGroup(Student student, Group newGroup);
}