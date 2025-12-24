using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Falico;

/// <summary>
/// Extension methods for IServiceCollection
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Falico services with the service collection
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="assemblies">Assemblies to scan for handlers</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddFalico(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        // Register mediator
        services.AddTransient<IMediator, Mediator>();

        // If no assemblies provided, use calling assembly
        if (assemblies.Length == 0)
        {
            assemblies = new[] { Assembly.GetCallingAssembly() };
        }

        // Register all handlers
        foreach (var assembly in assemblies)
        {
            RegisterHandlers(services, assembly);
        }

        return services;
    }

    /// <summary>
    /// Registers Falico services with the service collection, scanning the specified types' assemblies
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="handlerAssemblyMarkerTypes">Types from assemblies to scan</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddFalico(this IServiceCollection services, params Type[] handlerAssemblyMarkerTypes)
    {
        if (handlerAssemblyMarkerTypes == null || handlerAssemblyMarkerTypes.Length == 0)
            throw new ArgumentException("At least one marker type must be provided", nameof(handlerAssemblyMarkerTypes));

        var assemblies = handlerAssemblyMarkerTypes.Select(t => t.Assembly).Distinct().ToArray();
        return AddFalico(services, assemblies);
    }

    private static void RegisterHandlers(IServiceCollection services, Assembly assembly)
    {
        var types = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericTypeDefinition)
            .ToList();

        // Register request handlers
        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();

            foreach (var @interface in interfaces)
            {
                if (!@interface.IsGenericType)
                    continue;

                var genericTypeDefinition = @interface.GetGenericTypeDefinition();

                // Register IRequestHandler<TRequest, TResponse>
                if (genericTypeDefinition == typeof(IRequestHandler<,>))
                {
                    services.AddTransient(@interface, type);
                }
                // Register INotificationHandler<TNotification>
                else if (genericTypeDefinition == typeof(INotificationHandler<>))
                {
                    services.AddTransient(@interface, type);
                }
            }
        }
    }
}
