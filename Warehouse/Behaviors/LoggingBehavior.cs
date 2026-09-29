using MediatR;

namespace Warehouse.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string name = typeof(TRequest).Name;
        _logger.LogInformation("Handling {RequestName}", name);
        TResponse response = await next();
        _logger.LogInformation("Handled {RequestName}", name);
        return response;
    }
}
