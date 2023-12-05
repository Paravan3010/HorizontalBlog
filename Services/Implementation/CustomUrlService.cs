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
            if (parameters != null)
            {
                for(int i = 0; i < parameters.Length; i++)
                {
                    if (i == 0)
                        url += $"?{parameters[i].key}={parameters[i].value}";
                    else
                        url += $"&{parameters[i].key}={parameters[i].value}";
                }
            }

            return (GetCustomUrlFromOriginalUrl(url));
        }

        /// <summary>
        /// Tries to find a mapped custom URL
        /// </summary>
        /// <param name="originalUrl">original URL</param>
        /// <returns>Custom URL. If custom URL is not mapped, original URL is returned.</returns>
        public string GetCustomUrlFromOriginalUrl(string originalUrl)
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
                    if (i == 0)
                        url += $"?{parameters[i].key}={parameters[i].value}";
                    else
                        url += $"&{parameters[i].key}={parameters[i].value}";
                }
            }

            return (HasCustomUrlFromOriginalUrl(url));
        }

        public bool HasCustomUrlFromOriginalUrl(string originalUrl)
        {
            return _customUrlRepository.CustomUrls.Any(x => x.OriginalUrl == originalUrl);
        }
    }
}
