namespace ThomasWright.ServiceTemplate.Entities;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
    
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}

public interface IEntity : IEntity<Guid>;

public abstract class Entity<TKey> : IEntity<TKey>
{
    public TKey Id { get; set; }
    
    protected Entity()
    {
    }
    
    protected Entity(TKey id) : this(id, DateTime.UtcNow, DateTime.UtcNow)
    {
    }
    
    protected Entity(TKey id, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public abstract class Entity : Entity<Guid>, IEntity
{
    protected Entity()
    {
    }
    
    protected Entity(Guid id) : base(id)
    {
    }
    
    protected Entity(Guid id, DateTime createdAt, DateTime updatedAt) : base(id, createdAt, updatedAt)
    {
    }
}