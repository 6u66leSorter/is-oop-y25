using Isu.Models;

namespace Isu.Extra.Entities;

public class Course
{
    private readonly List<Flow> _flows;
    public Course(FacultyLetter faculty, string name)
    {
        ArgumentNullException.ThrowIfNull(faculty);
        ArgumentNullException.ThrowIfNull(name);

        _flows = new List<Flow>();
        Faculty = faculty;
        Name = name;
    }

    public FacultyLetter Faculty { get; }
    public string Name { get; }

    public IReadOnlyCollection<Flow> Flows => _flows;

    public void AddFlow(Flow flow)
    {
        ArgumentNullException.ThrowIfNull(flow);

        _flows.Add(flow);
    }
}