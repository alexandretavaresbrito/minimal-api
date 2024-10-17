using MINIMALAPI.Domain.Entities;
using MINIMALAPI.Domain.DTOS;

namespace MINIMALAPI.Domain.Interfaces
{
    public interface IVeiculoService
    {
        List<Veiculo> Todos(int? pagina = 1, string? nome=null, string? marca = null);
        Veiculo? BuscaPorId(int id);
        void Incluir(Veiculo veiculo);
        void Atualizar(Veiculo veiculo);
        void Apagar(Veiculo veiculo);
        
    }
}