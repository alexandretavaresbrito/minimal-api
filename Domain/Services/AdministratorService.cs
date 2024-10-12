
using MINIMALAPI.Domain.Services;
using MINIMALAPI.Domain.DTOS;
using MINIMALAPI.Domain.Entities;
using MINIMALAPI.Infrastructure.Db;
using MINIMALAPI.Domain.Interfaces;

namespace MINIMALAPI.Domain.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly ContextDb _contexto;
        public AdministratorService(ContextDb contexto)
        {
            _contexto = contexto;
        }

        public Administrator? Login(LoginDTO loginDTO)
        
        {
            var adm = _contexto.Administrators.Where( a => a.Email == loginDTO.Email).FirstOrDefault();
            return adm;
        }
    }
}