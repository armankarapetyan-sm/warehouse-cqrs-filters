using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Warehouse.Filters;

public class PositiveIdAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        object? raw;
        if (!context.ActionArguments.TryGetValue("id", out raw))
        {
            return;
        }

        int id;
        if (raw is int)
        {
            id = (int)raw;
        }
        else if (!int.TryParse(raw?.ToString(), out id))
        {
            context.Result = new BadRequestObjectResult("id must be a number");
            return;
        }

        if (id <= 0)
        {
            context.Result = new BadRequestObjectResult("id must be > 0");
        }
    }
}
