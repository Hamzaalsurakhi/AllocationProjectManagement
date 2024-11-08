using Microsoft.AspNetCore.Mvc.Filters;

namespace PresentationLayer.Attributes
{
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            response.Headers["Pragma"] = "no-cache";
            response.Headers["Expires"] = "-1";

            base.OnResultExecuting(filterContext);
        }
    }
}
