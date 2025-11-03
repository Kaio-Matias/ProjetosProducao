using Valedourado.Shared.Dtos;
namespace Valedourado.Supervisor.Services;
public interface IAuthService 
{ UsuarioDto? CurrentUser { get; } 
    void Login(UsuarioDto user); void Logout(); 
}