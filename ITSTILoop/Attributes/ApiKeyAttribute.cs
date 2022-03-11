using ITSTILoop.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ITSTILoop.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string APIKEYNAME = "ApiKey";
        private const string APIIDNAME = "ApiId";
        private const string APIADMIN = "ApiAdmin";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api Key was not provided"
                };
                return;
            }

            if (!context.HttpContext.Request.Headers.TryGetValue(APIIDNAME, out var extractedApiId))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api Id was not provided"
                };
                return;
            }

            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

            var apiKey = appSettings.GetValue<string>(APIKEYNAME);

            bool isAdmin = apiKey.Equals(extractedApiKey) && APIADMIN.Equals(extractedApiId);

            bool isParticipant = dbContext.Participants.Count(k => k.ApiKey == extractedApiKey.First() && k.Name == extractedApiId.First()) > 0;

            if (isAdmin || isParticipant)
            {
                await next();
            }   
            else
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api Key/Api Id is not valid"
                };
                return;
            }                       
        }
    }
}
