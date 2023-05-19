using System.ComponentModel.DataAnnotations;

namespace Magical_Villa.Models.Dto
{
    public class stringResponse
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string? Content { get; set; }
    }
}
