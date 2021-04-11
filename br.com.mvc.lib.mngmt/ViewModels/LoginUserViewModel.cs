using System.ComponentModel.DataAnnotations;

namespace br.com.mvc.lib.mngmt.ViewModels
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }

    }
}