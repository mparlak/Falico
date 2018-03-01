using System.Threading.Tasks;

namespace Falico
{
    public interface IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TRequest, TResponse>
        where TResponse : IResponse
    {
        Task<TResponse> Handle(TRequest request);
    }
}
