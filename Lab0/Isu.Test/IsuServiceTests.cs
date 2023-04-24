using Isu.Entities;
using Isu.Exceptions;
using Isu.Services;
using Xunit;

namespace Isu.Test;

public class IsuServiceTests
{
    private readonly IsuService _service;

    public IsuServiceTests()
    {
        _service = new IsuService();
    }

    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        Group group = _service.AddGroup("M32091");
        Student student = _service.AddStudent(group, "Иван", "Алейников");

        Assert.Contains(student, group.Students);
        Assert.Equal(group.GroupName, student.Group?.GroupName);
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        Group group = _service.AddGroup("M32091");
        for (int i = 0; i < Group.MaxNumOfStudentsInGroup; i++)
        {
            Student student = _service.AddStudent(group, $"Иван {i}", "Алейников");
        }

        Assert.Throws<InvalidNumberOfStudentsInGroupException>(() => _service.AddStudent(group, "Лжеиван", "Алейников"));
    }

    [Theory]
    [InlineData("a123")]
    [InlineData("M3209111")]
    [InlineData("A32091")]
    public void CreateGroupWithInvalidName_ThrowException(string invalidName)
    {
        Assert.Throws<InvalidGroupNameException>(() => _service.AddGroup(invalidName));
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        Group group1 = _service.AddGroup("M3109");
        Group group2 = _service.AddGroup("M32091");
        Student student = _service.AddStudent(group1, "Иван", "Алейников");

        _service.ChangeStudentGroup(student, group2);

        Assert.Equal(group2.GroupName, student.Group?.GroupName);
        Assert.Contains(student, group2.Students);
        Assert.DoesNotContain(student, group1.Students);
    }
}