using System.ComponentModel.DataAnnotations;

namespace bancoDeImagens.Models
{
    public class Images
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Size { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
