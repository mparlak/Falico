namespace Falico.Sample.Api.Models;

public class UserRepository
{
    private readonly List<User> _users = new();
    private int _nextId = 1;

    public UserRepository()
    {
        // Seed some data
        _users.Add(new User
        {
            Id = _nextId++,
            Name = "John Doe",
            Email = "john@example.com",
            CreatedAt = DateTime.UtcNow.AddDays(-30)
        });

        _users.Add(new User
        {
            Id = _nextId++,
            Name = "Jane Smith",
            Email = "jane@example.com",
            CreatedAt = DateTime.UtcNow.AddDays(-15)
        });
    }

    public Task<User?> GetByIdAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<List<User>> GetAllAsync()
    {
        return Task.FromResult(_users.ToList());
    }

    public Task<User> CreateAsync(string name, string email)
    {
        var user = new User
        {
            Id = _nextId++,
            Name = name,
            Email = email,
            CreatedAt = DateTime.UtcNow
        };

        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return Task.FromResult(false);

        _users.Remove(user);
        return Task.FromResult(true);
    }
}
