using Valedourado.Shared.Dtos;
namespace Valedourado.Services
{
    public interface IAuthService
    {
        UsuarioDto CurrentUser { get; }
        bool IsUserLoggedIn { get; }
        void Login(UsuarioDto user);
        void Logout();
    }
}