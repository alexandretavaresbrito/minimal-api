using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MINIMALAPI.Domain.DTOS;
using MINIMALAPI.Domain.Entities;
using MINIMALAPI.Domain.Interfaces;

namespace Teste.Mocks
{
    public class AdministratorServiceMock : IAdministratorService
    {
        private List<Administrator> administradores = new List<Administrator>(){
                        new Administrator{
                            Id = 1,
                            Email = "admin@admin.com",
                            Senha = "admin",
                            Perfil = "Adm"
                        },
                        new Administrator{
                            Id = 2,
                            Email = "editor@editor.com",
                            Senha = "editor",
                            Perfil = "Editor"
                        }
                    };
        public Administrator? BuscaPorId(int id)
        {
            return administradores.Find(c => c.Id == id);
        }

        public Administrator Incluir(Administrator administrator)
        {
            administrator.Id = administradores.Count() + 1;
            administradores.Add(administrator);
            return administrator;
        }

        public Administrator? Login(LoginDTO loginDTO)
        {
            return administradores.Find(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha);
        }

        public List<Administrator> Todos(int? pagina)
        {
            return administradores;
        }
    }
}