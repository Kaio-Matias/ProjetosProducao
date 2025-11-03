// Valedourado.Supervisor/Services/AuthService.cs
using Valedourado.Shared.Dtos;

namespace Valedourado.Supervisor.Services
{
    public class AuthService : IAuthService
    {
        public UsuarioDto? CurrentUser { get; private set; }
        public void Login(UsuarioDto user) => CurrentUser = user;
        public void Logout() => CurrentUser = null;
    }
}