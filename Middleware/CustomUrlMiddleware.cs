using Horizontal.Domain;
using Horizontal.Domain.Repositories;

namespace Horizontal.Middleware
{
    /// <summary>
    /// This middleware handles custom URLs defined in the CustomUrl and CustomUrlQueryValues domain classes
    /// </summary>
    public class CustomUrlMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomUrlMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
             var customUrlsRepository = context.RequestServices.GetService<ICustomUrlRepository>();

            var path = context.Request.Path;
            // If no such custom URL is mapped continue in middleware pipeline
            var urlMapping = customUrlsRepository.CustomUrls.FirstOrDefault(x => x.NewUrl == path.Value);
            if (urlMapping == null)
            {
                await _next(context);
                return;
            }

            if (urlMapping.OriginalUrl.Contains("?"))
            {
                context.Request.Path = $"{urlMapping.OriginalUrl.Split("?").First()}";
                context.Request.QueryString = new QueryString($"?{urlMapping.OriginalUrl.Split("?").Skip(1).First()}");
            }
            else
            {
                context.Request.Path = $"{urlMapping.OriginalUrl}";
            }

            await _next(context);
        }
    }
}
