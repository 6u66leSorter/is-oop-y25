using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Services
{
    public class IsuService : IIsuService
    {
        private readonly Dictionary<string, Group> _groups;

        private readonly Dictionary<long, Student> _students;

        private int _increment;

        public IsuService()
        {
            _groups = new Dictionary<string, Group>();
            _students = new Dictionary<long, Student>();
            _increment = 0;
        }

        public Group AddGroup(string groupName)
        {
            if (!Group.TryCreate(groupName, out Group? group))
            {
                throw new InvalidGroupNameException(groupName);
            }

            _groups.Add(groupName, group!);

            return group!;
        }

        public Student AddStudent(Group group, string firstName, string lastName)
        {
            _increment += 1;

            if (!Student.TryCreate(_increment, firstName, lastName, group, out Student? student))
            {
                throw new InvalidStudentArgsException(firstName, lastName);
            }

            if (group.Students.Count >= Group.MaxNumOfStudentsInGroup)
            {
                throw new InvalidNumberOfStudentsInGroupException(group.GroupName);
            }

            _students.Add(student!.Id, student);
            group.Add(student);

            return student;
        }

        public Student GetStudent(int id)
        {
            if (!_students.ContainsKey(id))
            {
                throw new StudentNotFoundException(id);
            }

            return _students[id];
        }

        public Student? FindStudent(int id)
        {
            return !_students.ContainsKey(id) ? null : _students[id];
        }

        public IReadOnlyCollection<Student> FindStudents(CourseNumber courseNumber)
        {
            var res = new List<Student>();
            foreach (KeyValuePair<long, Student> elem in _students)
            {
                if (elem.Value.Group?.CourseNumber == courseNumber)
                {
                    res.Add(elem.Value);
                }
            }

            return res;
        }

        public IReadOnlyCollection<Student> FindStudents(string groupName)
        {
            var res = new List<Student>();
            if (!_groups.ContainsKey(groupName))
            {
                return res;
            }

            return _groups[groupName].Students;
        }

        public Group? FindGroup(string groupName)
        {
            if (!_groups.ContainsKey(groupName))
            {
                return null;
            }

            return _groups[groupName];
        }

        public IReadOnlyCollection<Group> FindGroups(CourseNumber courseNumber)
        {
            var res = new List<Group>();
            foreach (KeyValuePair<string, Group> elem in _groups)
            {
                if (elem.Value.CourseNumber == courseNumber)
                {
                    res.Add(elem.Value);
                }
            }

            return res;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            Group? group = student.Group;
            group?.Remove(student);
            student.Group = newGroup;
            newGroup.Add(student);
        }
    }
}