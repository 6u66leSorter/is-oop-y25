using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class Lesson
{
    public Lesson(Teacher teacher, Guid id, LessonNumber number, string room)
    {
        ArgumentNullException.ThrowIfNull(teacher);
        ArgumentNullException.ThrowIfNull(room);

        Teacher = teacher;
        Team = id;
        Number = number;
        Room = room;
    }

    public Teacher Teacher { get; }

    public Guid Team { get; }

    public LessonNumber Number { get; }

    public string Room { get; }
}