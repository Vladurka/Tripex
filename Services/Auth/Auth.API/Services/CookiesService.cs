namespace Auth.API.Services
{
    public class CookiesService(IHttpContextAccessor httpContextAccessor) : ICookiesService
    {
        public void AddCookie(string name, string value, int hours)
        {
            ValidateCookieParams(name, value, hours);   

            if (CookieExists(name))
                throw new InvalidOperationException("Cookie with this name already exists");

            else
            {
                httpContextAccessor?.HttpContext?.Response.Cookies.Append(name, value, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddHours(hours)
                });
            }
        }

        public string GetFromCookie(string name)
        {
            if (httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(name, out string? value))
                return value;

            else
                return string.Empty;
        }

        public void DeleteCookie(string name)
        {
            if (!CookieExists(name))
                throw new InvalidOperationException("Cookie is not found");

            httpContextAccessor?.HttpContext?.Response.Cookies.Delete(name);
        }

        public void UpdateCookie(string name, string newValue, int hours)
        {
            ValidateCookieParams(name, newValue, hours);

            if (!CookieExists(name))
                throw new InvalidOperationException("Cookie is not found");

            httpContextAccessor?.HttpContext?.Response.Cookies.Append(name, newValue, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddHours(hours)
            });
        }

        private void ValidateCookieParams(string name, string value, int hours)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Cookie name cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value), "Cookie value cannot be null or empty.");

            if (hours <= 0)
                throw new ArgumentOutOfRangeException(nameof(hours), "Expiration time must be greater than zero.");
        }

        public bool CookieExists(string name) =>
            httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(name);
    }
}

