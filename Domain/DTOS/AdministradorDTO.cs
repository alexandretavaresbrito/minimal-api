using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MINIMALAPI.Domain.Enums;

namespace MINIMALAPI.Domain.DTOS
{
    public class AdministradorDTO
    {
        public required string? Email { get; set; }
        public required string Senha { get; set; }
        public required PerfilEnum? Perfil { get; set; }    
    }
}