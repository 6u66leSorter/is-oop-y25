using Isu.Entities;

namespace Isu.Extra.Entities;

public class StudyGroup
{
    public StudyGroup(Group group, Guid id)
    {
        ArgumentNullException.ThrowIfNull(group);

        Group = group;
        Id = id;
        Schedule = new Schedule();
    }

    public Schedule Schedule { get; }

    public Group Group { get; }

    public Guid Id { get; }
}