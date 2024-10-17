
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

        public Administrator? BuscaPorId(int id)
        {
            return _contexto.Administrators.Where(a => a.Id == id).FirstOrDefault();
        }

        public Administrator Incluir(Administrator administrator)
        {
            _contexto.Add(administrator);
            _contexto.SaveChanges();

            return administrator;
        }

        public Administrator? Login(LoginDTO loginDTO)
        {
            var adm = _contexto.Administrators.Where( a => a.Email == loginDTO.Email).FirstOrDefault();
            return adm;
        }

        public List<Administrator> Todos(int? pagina)
        {
            var query = _contexto.Administrators.AsQueryable();
    
            int itensPorPagina = 10;
            if(pagina != null)
                query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);


            return query.ToList();
        }
    }
}