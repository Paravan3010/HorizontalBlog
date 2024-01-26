using Horizontal.Domain.Repositories;

namespace Horizontal.Services.Implementation
{
    public class CustomUrlProviderService : ICustomUrlProviderService
    {
        private ICustomUrlRepository _customUrlRepository;

        public CustomUrlProviderService(ICustomUrlRepository customUrlRepository)
        {
            _customUrlRepository = customUrlRepository;
        }

        /// <summary>
        /// Tries to find a mapped custom URL
        /// </summary>
        /// <param name="controller">Constroller</param>
        /// <param name="action">Action</param>
        /// <param name="parameters">List of query parameter keys and values</param>
        /// <returns>Custom URL. If custom URL is not mapped, original URL is returned.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GetCustomUrl(string controller, string action = null, params (string key, string value)[] parameters)
        {
            var url = $"/{controller}";
            if (action != null)
                url += $"/{action}";

            string pageParam = null;
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    // In the event that a page parameter exists, it will be skipped and added to the end of the generated URL
                    if (String.Equals(parameters[i].key, "page", StringComparison.OrdinalIgnoreCase))
                    {
                        pageParam = parameters[i].value;
                        continue;
                    }

                    if (i == 0)
                        url += $"?{parameters[i].key}={parameters[i].value}";
                    else
                        url += $"&{parameters[i].key}={parameters[i].value}";
                }
            }

            var customUrl = GetCustomUrlFromOriginalUrl(url);

            // For non-custom URLs, the page parameter is not customized either
            if (customUrl == url && pageParam != null && pageParam != "1")
            {
                if (customUrl.Contains("?")) // not the first paramter
                    return $"{customUrl}&page={pageParam}";
                else
                    return $"{customUrl}?page={pageParam}";
            }

            if (pageParam == null || pageParam == "1")
                return customUrl;
            else
                return $"{customUrl}/{pageParam}";
        }

        /// <summary>
        /// Tries to find a mapped custom URL
        /// </summary>
        /// <param name="originalUrl">original URL</param>
        /// <returns>Custom URL. If custom URL is not mapped, original URL is returned.</returns>
        private string GetCustomUrlFromOriginalUrl(string originalUrl)
        {
            var mappingRecord = _customUrlRepository.CustomUrls.Where(x => x.OriginalUrl == originalUrl).FirstOrDefault();
            if (mappingRecord == null || mappingRecord.NewUrl == null)
                return originalUrl;
            return mappingRecord.NewUrl;
        }

        public bool HasCustomUrl(string controller, string action = null, params (string key, string value)[] parameters)
        {
            var url = $"/{controller}";
            if (action != null)
                url += $"/{action}";
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    // In the event that a page parameter exists, it will be skipped, as URLs differing
                    // from existing custom URLs are handled automatically in the CustomUrlMiddleware
                    if (String.Equals(parameters[i].key, "page", StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (i == 0)
                        url += $"?{parameters[i].key}={parameters[i].value}";
                    else
                        url += $"&{parameters[i].key}={parameters[i].value}";
                }
            }

            return (HasCustomUrlFromOriginalUrl(url));
        }

        private bool HasCustomUrlFromOriginalUrl(string originalUrl)
        {
            return _customUrlRepository.CustomUrls.Any(x => x.OriginalUrl == originalUrl);
        }
    }
}
