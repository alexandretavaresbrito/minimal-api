using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MINIMALAPI.Domain.Enums;

namespace MINIMALAPI.Domain.DTOS
{
    public class AdministradorDTO
    {
        public string Email { get; set; } = default!;
        public string Senha { get; set; } = default!;
        public PerfilEnum? Perfil { get; set; } = default!;    
    }
}