using Falico;
using Falico.Sample.Api.Features.Users.Notifications;
using Falico.Sample.Api.Models;

namespace Falico.Sample.Api.Features.Users.Commands;

public class CreateUserCommand : IRequest<User>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly UserRepository _userRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(
        UserRepository userRepository,
        IMediator mediator,
        ILogger<CreateUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating user: {Name}", request.Name);

        var user = await _userRepository.CreateAsync(request.Name, request.Email);
    
        _logger.LogInformation("User created with ID: {UserId}", user.Id);

        // Publish notification
        await _mediator.Publish(new UserCreatedNotification
        {
            UserId = user.Id,
            Name = user.Name,
            Email = user.Email
        }, cancellationToken);

        return user;
    }
}
