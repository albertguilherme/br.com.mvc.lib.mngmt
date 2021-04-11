using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using br.com.mvc.lib.mngmt.model;

namespace br.com.mvc.lib.mngmt.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Username must be more than 5 caracters.")]
        [MaxLength(10, ErrorMessage = "Username must be less than 10 caracters.")]
        public string Username { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password must be more than 8 caracters.")]
        [MaxLength(16, ErrorMessage = "Passwrod must be less than 16 caracters.")]
        public string Password { get; set; }
        public string[] Roles { get; set; }

        public User ToModel()
        {
            return new ()
            {
                Id = Id,
                Name = Name,
                Password = BCrypt.Net.BCrypt.HashPassword(Password),
                Username = Username,
                Roles = string.Join('|', Roles)
            };
        }

        public UserViewModel ToViewModel(User u)
        {
            Id = u.Id;
            Name = u.Name;
            Password = "";
            Username = u.Username;
            Roles = u.Roles?.Split(',');
            return this;

        }
    }
}
