using Falico;
using Falico.Sample.Api.Models;

namespace Falico.Sample.Api.Features.Users.Queries;

public class GetUserByIdQuery : IRequest<User?>
{
    public int UserId { get; set; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User?>
{
    private readonly UserRepository _userRepository;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(UserRepository userRepository, ILogger<GetUserByIdQueryHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting user with ID: {UserId}", request.UserId);

        var user = await _userRepository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found", request.UserId);
        }

        return user;
    }
}
