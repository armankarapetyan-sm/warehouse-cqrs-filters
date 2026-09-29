using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Warehouse.Filters;

public class MapErrorsAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is KeyNotFoundException)
        {
            context.Result = new NotFoundObjectResult(new { error = "not found" });
            context.ExceptionHandled = true;
            return;
        }

        if (context.Exception is InvalidOperationException)
        {
            context.Result = new BadRequestObjectResult(new { error = context.Exception.Message });
            context.ExceptionHandled = true;
        }
    }
}
