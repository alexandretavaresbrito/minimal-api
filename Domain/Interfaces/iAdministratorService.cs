using MINIMALAPI.Domain.Entities;
using MINIMALAPI.Domain.DTOS;

namespace MINIMALAPI.Domain.Interfaces
{
    public interface IAdministratorService
    {
        Administrator? Login(LoginDTO loginDTO);
    }
}