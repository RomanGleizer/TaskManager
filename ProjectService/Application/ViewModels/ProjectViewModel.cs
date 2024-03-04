namespace Application.ViewModels;

public class ProjectViewModel
{
    public ProjectViewModel(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public int Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }
}
