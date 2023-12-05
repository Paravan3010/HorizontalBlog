namespace Horizontal.Services
{
    public interface ICustomUrlProviderService
    {
        public bool HasCustomUrl(string controller, string action = null, params (string key, string value)[] parameters);
        public bool HasCustomUrlFromOriginalUrl(string originalUrl);
        public string GetCustomUrl(string controller, string action = null, params (string key, string value)[] parameters);
        public string GetCustomUrlFromOriginalUrl(string originalUrl);
    }
}
