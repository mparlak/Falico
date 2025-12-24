# Falico

Lightweight mediator pattern implementation for .NET 10+.

## Features

- **Request/Response Pattern**: Send requests and receive responses
- **Notification Pattern**: Publish notifications to multiple handlers
- **Dependency Injection**: Built-in support for Microsoft.Extensions.DependencyInjection
- **Async/Await**: Fully async API with cancellation token support
- **Lightweight**: Minimal dependencies and overhead
- **.NET 10 & C# 14**: Built with the latest .NET technologies

## Installation

```bash
dotnet add package Falico
```

## Usage

### 1. Define a Request and Handler

```csharp
using Falico;

// Define a request with response
public class GetUserQuery : IRequest<User>
{
    public int UserId { get; set; }
}

// Define the handler
public class GetUserQueryHandler : IRequestHandler<GetUserQuery, User>
{
    public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken = default)
    {
        // Your logic here
        return new User { Id = request.UserId, Name = "John Doe" };
    }
}
```

### 2. Define a Request without Response (Command)

```csharp
// Define a command (request with no response)
public class CreateUserCommand : IRequest
{
    public string Name { get; set; }
}

// Define the handler
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken = default)
    {
        // Your logic here
        Console.WriteLine($"Creating user: {request.Name}");
        return Unit.Value;
    }
}
```

### 3. Define a Notification and Handlers

```csharp
// Define a notification
public class UserCreatedNotification : INotification
{
    public int UserId { get; set; }
    public string Name { get; set; }
}

// Multiple handlers can handle the same notification
public class SendEmailHandler : INotificationHandler<UserCreatedNotification>
{
    public async Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Sending email for user: {notification.Name}");
    }
}

public class LogUserCreatedHandler : INotificationHandler<UserCreatedNotification>
{
    public async Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Logging user creation: {notification.Name}");
    }
}
```

### 4. Register Services

```csharp
using Falico;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Register Falico and scan assemblies for handlers
services.AddFalico(typeof(Program).Assembly);

// Or scan multiple assemblies
// services.AddFalico(Assembly.GetExecutingAssembly(), typeof(SomeType).Assembly);

var serviceProvider = services.BuildServiceProvider();
```

### 5. Use the Mediator

```csharp
var mediator = serviceProvider.GetRequiredService<IMediator>();

// Send a request
var user = await mediator.Send(new GetUserQuery { UserId = 1 });

// Send a command
await mediator.Send(new CreateUserCommand { Name = "Jane Doe" });

// Publish a notification
await mediator.Publish(new UserCreatedNotification
{
    UserId = 1,
    Name = "Jane Doe"
});
```

## API Reference

### Interfaces

- **`IRequest<TResponse>`**: Marker interface for requests with a response
- **`IRequest`**: Marker interface for requests without a response (void)
- **`INotification`**: Marker interface for notifications
- **`IRequestHandler<TRequest, TResponse>`**: Handler for requests
- **`INotificationHandler<TNotification>`**: Handler for notifications
- **`IMediator`**: Main mediator interface

### Methods

- **`Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)`**: Send a request
- **`Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)`**: Publish a notification

## License

MIT
