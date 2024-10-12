using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MINIMALAPI.Domain.Entities
{
    public record Veiculo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public required string Nome { get; set; }
        [Required]
        [StringLength(100)]
        public required string Marca { get; set; }
        [Required]
        public int Ano { get; set; }
    }
}