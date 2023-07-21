using System.ComponentModel.DataAnnotations;

namespace RomaF5.Models.Dtos
{
    public class CrearUsuarioDto
    {
        public string? Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]        
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
