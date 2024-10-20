using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MINIMALAPI.Domain.Enums;
namespace MINIMALAPI.Domain.Entities
{
    public class Administrator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = default!;
        [Required]
        [StringLength(50)]
        public string Senha { get; set; } = default!;
        [Required]
        [StringLength(255)]
        public string Email { get; set; } = default!;
        [Required]
        [StringLength(15)]
        public string Perfil { get; set; } = default!;
    }
}