using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MINIMALAPI.Domain.Entities
{
    public class Administrator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(50)]
        public string Senha { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(15)]
        public string Perfil { get; set; }
    }
}