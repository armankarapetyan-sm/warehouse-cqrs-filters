using Microsoft.AspNetCore.Mvc.Filters;

namespace Warehouse.Filters;

public class AddApiHeaderAttribute : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.Headers["X-Api"] = "warehouse";
    }
}
