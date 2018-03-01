using System.Threading.Tasks;
using Autofac;

namespace Falico
{
    public class Mediator : IMediator
    {
        private readonly ILifetimeScope _lifetimeScope;

        public Mediator(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public async Task<TResponse> Request<TRequest, TResponse>(IRequest<TRequest, TResponse> request)
            where TRequest : IRequest<TRequest, TResponse>
            where TResponse : IResponse
        {
            var handler = _lifetimeScope.Resolve<IRequestHandler<TRequest, TResponse>>();

            return await handler.Handle((TRequest)request); ;
        }

        public async Task Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = _lifetimeScope.Resolve<ICommandHandler<TCommand>>();

            await handler.Handle(command);
        }
    }
}
