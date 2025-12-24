using Falico;
using Falico.Sample.Api.Models;

namespace Falico.Sample.Api.Features.Users.Queries;

public class GetAllUsersQuery : IRequest<List<User>>
{
}

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<User>>
{
    private readonly UserRepository _userRepository;
    private readonly ILogger<GetAllUsersQueryHandler> _logger;

    public GetAllUsersQueryHandler(UserRepository userRepository, ILogger<GetAllUsersQueryHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting all users");

        var users = await _userRepository.GetAllAsync();

        _logger.LogInformation("Found {Count} users", users.Count);

        return users;
    }
}
