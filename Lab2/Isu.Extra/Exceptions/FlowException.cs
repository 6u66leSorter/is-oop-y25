using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Models;

namespace Isu.Extra.Exceptions;

public class FlowException : Exception
{
    private FlowException(string message)
        : base(message) { }
    public static FlowException NotEnoughPlacesOnFlow(string name)
        => new FlowException($"all places are filled on course : {name}");
    public static FlowException SameFaculty(char faculty)
        => new FlowException($"You can't enroll course of your faculty : {faculty}");
    public static FlowException ReEnrollment(string courseName)
        => new FlowException($"You have already enrolled this course : {courseName}");

    public static FlowException NoSuchStudent(int id)
        => new FlowException("Student with id ({id}) isn't in flow");
}