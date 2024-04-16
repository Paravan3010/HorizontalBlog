namespace Horizontal.Services
{
    public interface ICustomUrlProviderService
    {
        public bool HasCustomUrl(string controller, string action = null, params (string key, string value)[] parameters);
        public string GetCustomUrl(string controller, string action = null, params (string key, string value)[] parameters);
    }
}
