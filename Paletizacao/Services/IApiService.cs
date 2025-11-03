using Valedourado.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Paletizacao.Services
{
    public interface IApiService
    {
        Task<UsuarioDto> LoginAsync(LoginRequestDto loginRequest);
        Task<UsuarioDto> RegisterAsync(CreateUsuarioDto novoUsuario);
        Task<List<ProducaoDto>> GetOpenProducoesAsync();
        Task<CadastroDto> GetCadastroPorCodBarraAsync(string codBarra);
        Task<List<PaleteDto>> GetPaletesPorOPAsync(int ordemProducao);
        Task<PaleteDto> SalvarPaleteAsync(CreatePaleteDto palete);
        Task<PaleteDto> UpdateQtdePaleteAsync(int paleteId, UpdateQtdePaleteDto updateDto);
    }
}