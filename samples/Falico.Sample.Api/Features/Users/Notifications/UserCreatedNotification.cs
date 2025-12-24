using Falico;

namespace Falico.Sample.Api.Features.Users.Notifications;

public class UserCreatedNotification : INotification
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class SendWelcomeEmailHandler : INotificationHandler<UserCreatedNotification>
{
    private readonly ILogger<SendWelcomeEmailHandler> _logger;

    public SendWelcomeEmailHandler(ILogger<SendWelcomeEmailHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("📧 Sending welcome email to {Email} for user {Name}",
            notification.Email, notification.Name);

        // Simulate sending email
        return Task.CompletedTask;
    }
}

public class LogUserCreatedHandler : INotificationHandler<UserCreatedNotification>
{
    private readonly ILogger<LogUserCreatedHandler> _logger;

    public LogUserCreatedHandler(ILogger<LogUserCreatedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("📝 Logging user creation - UserID: {UserId}, Name: {Name}, Email: {Email}",
            notification.UserId, notification.Name, notification.Email);

        return Task.CompletedTask;
    }
}

public class UpdateAnalyticsHandler : INotificationHandler<UserCreatedNotification>
{
    private readonly ILogger<UpdateAnalyticsHandler> _logger;

    public UpdateAnalyticsHandler(ILogger<UpdateAnalyticsHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("📊 Updating analytics for new user: {UserId}", notification.UserId);

        return Task.CompletedTask;
    }
}
