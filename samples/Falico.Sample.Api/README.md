# Falico Sample API

This is a sample ASP.NET Core Web API demonstrating the usage of **Falico** mediator pattern library.

## Features Demonstrated

- **CQRS Pattern**: Separate Commands and Queries
- **Request/Response**: GET user(s) with response
- **Commands**: Create and Delete users
- **Notifications**: Multiple handlers for a single event (UserCreated)
- **Dependency Injection**: Full DI integration with ASP.NET Core

## Project Structure

```
Features/
└── Users/
    ├── Commands/
    │   ├── CreateUserCommand.cs       # Create user command + handler
    │   └── DeleteUserCommand.cs       # Delete user command + handler
    ├── Queries/
    │   ├── GetUserByIdQuery.cs        # Get single user query + handler
    │   └── GetAllUsersQuery.cs        # Get all users query + handler
    └── Notifications/
        └── UserCreatedNotification.cs # User created notification + 3 handlers
Models/
├── User.cs                            # User model
└── UserRepository.cs                  # In-memory repository
Controllers/
└── UsersController.cs                 # REST API controller using IMediator
```

## Running the Application

```bash
cd samples/Falico.Sample.Api
dotnet run
```

The API will start on `http://localhost:5231` (or the port shown in console).

## API Documentation

Once running, visit:
```
http://localhost:5231/scalar/v1
```

This provides a modern, interactive API documentation powered by **Scalar**.

## API Endpoints

### Get All Users
```http
GET /api/users
```

### Get User by ID
```http
GET /api/users/{id}
```

### Create User
```http
POST /api/users
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@example.com"
}
```

When a user is created, **3 notification handlers** are triggered automatically:
- `SendWelcomeEmailHandler` - Sends welcome email
- `LogUserCreatedHandler` - Logs the creation
- `UpdateAnalyticsHandler` - Updates analytics

### Delete User
```http
DELETE /api/users/{id}
```


## How It Works

### 1. Registration (Program.cs)

```csharp
// Register Falico and scan assembly for handlers
builder.Services.AddFalico(typeof(Program));

// Register your services
builder.Services.AddSingleton<UserRepository>();
```

### 2. Controller Usage

```csharp
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _mediator.Send(new GetUserByIdQuery { UserId = id });
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var user = await _mediator.Send(new CreateUserCommand
        {
            Name = request.Name,
            Email = request.Email
        });
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }
}
```

### 3. Query Example

```csharp
// Query - returns data
public class GetUserByIdQuery : IRequest<User?>
{
    public int UserId { get; set; }
}

// Handler
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User?>
{
    private readonly UserRepository _repository;

    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.UserId);
    }
}
```

### 4. Command Example

```csharp
// Command - modifies state and publishes notification
public class CreateUserCommand : IRequest<User>
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly UserRepository _repository;
    private readonly IMediator _mediator;

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.CreateAsync(request.Name, request.Email);

        // Publish notification to multiple handlers
        await _mediator.Publish(new UserCreatedNotification
        {
            UserId = user.Id,
            Name = user.Name,
            Email = user.Email
        });

        return user;
    }
}
```

### 5. Notification Example

```csharp
// Notification
public class UserCreatedNotification : INotification
{
    public int UserId { get; set; }
    public string Name { get; set; }
}

// Handler 1 - Send Email
public class SendWelcomeEmailHandler : INotificationHandler<UserCreatedNotification>
{
    public Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        // Send email logic
        return Task.CompletedTask;
    }
}

// Handler 2 - Log Event
public class LogUserCreatedHandler : INotificationHandler<UserCreatedNotification>
{
    public Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        // Logging logic
        return Task.CompletedTask;
    }
}

// Handler 3 - Update Analytics
public class UpdateAnalyticsHandler : INotificationHandler<UserCreatedNotification>
{
    public Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        // Analytics logic
        return Task.CompletedTask;
    }
}
```

## Testing with curl

### Get all users
```bash
curl http://localhost:5231/api/users
```

### Get user by ID
```bash
curl http://localhost:5231/api/users/1
```

### Create a user (triggers 3 notification handlers!)
```bash
curl -X POST http://localhost:5231/api/users \
  -H "Content-Type: application/json" \
  -d '{"name":"Alice Johnson","email":"alice@example.com"}'
```

Check the console output to see all 3 notification handlers being triggered:
- 📧 SendWelcomeEmailHandler
- 📝 LogUserCreatedHandler
- 📊 UpdateAnalyticsHandler

### Delete a user
```bash
curl -X DELETE http://localhost:5231/api/users/1
```

## Key Takeaways

1. **Separation of Concerns**: Business logic is isolated in handlers
2. **Single Responsibility**: Each handler does one thing
3. **Testability**: Handlers can be easily unit tested
4. **Extensibility**: Add new handlers without modifying existing code
5. **Decoupling**: Controllers don't know about implementation details
6. **Event-Driven**: Notifications enable pub/sub pattern

## Learn More

- Check the [main Falico README](../../src/Falico/README.md) for more details about the library
- Explore the source code to see how everything connects together
