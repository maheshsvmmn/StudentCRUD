namespace Students_API.Services.Authorization
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string username, string password);

    }
}
