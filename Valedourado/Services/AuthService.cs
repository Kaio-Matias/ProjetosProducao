using Valedourado.Shared.Dtos;
namespace Valedourado.Services
{
    public class AuthService : IAuthService
    {
        public UsuarioDto CurrentUser { get; private set; }
        public bool IsUserLoggedIn => CurrentUser != null;

        public void Login(UsuarioDto user)
        {
            CurrentUser = user;
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}