using System.ComponentModel.DataAnnotations;

namespace WebShopApi.Dtos
{
    public class SignInDto
    {
        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
