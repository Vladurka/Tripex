namespace Auth.API.Services.Interfaces
{
    public interface ICookiesService
    {
        public void AddCookie(string name, string value, int hours);
        public string GetFromCookie(string name);
        public void UpdateCookie(string name, string value, int hours);
        public void DeleteCookie(string name);
        public bool CookieExists(string name);
    }
}