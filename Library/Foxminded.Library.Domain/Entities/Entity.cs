namespace Foxminded.Library.Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; set; }
    public string? Hash { get; set; }
}
