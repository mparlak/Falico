using System.Threading.Tasks;

namespace Falico
{
    public interface IMediator
    {
        Task<TResponse> Request<TRequest, TResponse>(IRequest<TRequest, TResponse> request)
            where TRequest : IRequest<TRequest, TResponse>
            where TResponse : IResponse;

        Task Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
