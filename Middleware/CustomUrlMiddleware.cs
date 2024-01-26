using Horizontal.Domain;
using Horizontal.Domain.Repositories;
using System.Text.RegularExpressions;

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

            var path = context.Request.Path.Value;
            // If the path ends with '/number', it should be ignored as it represents a page automatically handled by this middleware
            string page = null;
            var pageRegex = Regex.Match(path, "\\/[1-9]\\d*$");
            if (pageRegex.Success)
            {
                page = pageRegex.Value.Substring(1);
                path = path.Substring(0, path.Length - pageRegex.Value.Length);
            }

            // If no such custom URL is mapped continue in middleware pipeline
            var urlMapping = customUrlsRepository.CustomUrls.FirstOrDefault(x => x.NewUrl == path);
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

            // Paging for custom URLs is handled automatically
            if (page != null && page != "1")
                context.Request.QueryString = context.Request.QueryString.Add("page", page);

            await _next(context);
        }
    }
}
