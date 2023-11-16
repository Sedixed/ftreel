namespace ftreel.Services;

public interface IAuthenticationService
{
    void Login();
    void Logout();
    void AuthenticatedUser();
}