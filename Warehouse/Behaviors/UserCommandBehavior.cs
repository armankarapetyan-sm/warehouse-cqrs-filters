using MediatR;
using Warehouse.Features.Products.Application;

namespace Warehouse.Behaviors;

public class UserCommandBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        IUserCommand? command = request as IUserCommand;
        if (command != null && string.IsNullOrEmpty(command.UserId))
        {
            throw new UnauthorizedAccessException("User id missing on command.");
        }

        return next();
    }
}
