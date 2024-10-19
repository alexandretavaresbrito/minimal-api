using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MINIMALAPI.Domain.DTOS
{
    public class LoginDTO
    {
        public string? Email { get; set; }
        public string? Senha { get; set; }

        public LoginDTO(string email, string senha)
        {
            Email = email;
            Senha = senha;
        }     
    }
}