using MINIMALAPI.Domain.Enums;

namespace MINIMALAPI.Domain.ModelViews
{
    public struct AdministradorModelView
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Perfil { get; set; }  

    }
}