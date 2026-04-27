namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(string email, string password);
        Task<string> RegisterAsync(string name, string email, string password);
    }
}
