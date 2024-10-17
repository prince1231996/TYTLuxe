using System.ComponentModel.DataAnnotations;

namespace OleHotels.Models
{
    public class ClientLoginViewModel
    {
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        
        [Required]
        [StringLength(30)]
        public string Password { get; set; }
    }
}