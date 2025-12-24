using Microsoft.Extensions.DependencyInjection;

namespace Falico;

/// <summary>
/// Default implementation of IMediator
/// </summary>
public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <summary>
    /// Send a request to a single handler
    /// </summary>
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        var requestType = request.GetType();
        var responseType = typeof(TResponse);
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

        var handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
            throw new InvalidOperationException($"Handler not found for request type {requestType.Name}");

        var handleMethod = handlerType.GetMethod("Handle");
        if (handleMethod == null)
            throw new InvalidOperationException($"Handle method not found on handler for {requestType.Name}");

        var result = handleMethod.Invoke(handler, new object[] { request, cancellationToken });

        if (result is Task<TResponse> task)
            return await task;

        throw new InvalidOperationException($"Handler for {requestType.Name} did not return a Task<{responseType.Name}>");
    }

    /// <summary>
    /// Publish a notification to multiple handlers
    /// </summary>
    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        if (notification == null)
            throw new ArgumentNullException(nameof(notification));

        var notificationType = notification.GetType();
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notificationType);

        var handlers = _serviceProvider.GetServices(handlerType);

        var tasks = new List<Task>();

        foreach (var handler in handlers)
        {
            var handleMethod = handlerType.GetMethod("Handle");
            if (handleMethod == null)
                continue;

            var result = handleMethod.Invoke(handler, new object[] { notification, cancellationToken });

            if (result is Task task)
                tasks.Add(task);
        }

        await Task.WhenAll(tasks);
    }
}
