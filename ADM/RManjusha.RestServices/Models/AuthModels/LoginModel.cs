using System.ComponentModel.DataAnnotations;

namespace RManjusha.RestServices.Models.AuthModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "login type is required")]
        public bool IsCorporate { get; set; }

        public LoginModel(string username, string password, bool isCorporate)
        {
            Username = username;
            Password = password;
            IsCorporate = isCorporate;
        }
        public LoginModel()
        {

        }
    }
}
