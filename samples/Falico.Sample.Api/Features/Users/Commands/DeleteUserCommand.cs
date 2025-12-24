using Falico;
using Falico.Sample.Api.Models;

namespace Falico.Sample.Api.Features.Users.Commands;

public class DeleteUserCommand : IRequest<bool>
{
    public int UserId { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly UserRepository _userRepository;
    private readonly ILogger<DeleteUserCommandHandler> _logger;

    public DeleteUserCommandHandler(UserRepository userRepository, ILogger<DeleteUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting user with ID: {UserId}", request.UserId);

        var result = await _userRepository.DeleteAsync(request.UserId);

        if (result)
        {
            _logger.LogInformation("User {UserId} deleted successfully", request.UserId);
        }
        else
        {
            _logger.LogWarning("User {UserId} not found for deletion", request.UserId);
        }

        return result;
    }
}
