using MINIMALAPI.Domain.Entities;
using MINIMALAPI.Domain.DTOS;

namespace MINIMALAPI.Domain.Interfaces
{
    public interface IAdministratorService
    {
        Administrator? Login(LoginDTO loginDTO);

        Administrator Incluir(Administrator administrator);

        Administrator? BuscaPorId(int id);

        List<Administrator> Todos(int? pagina);
    }
}