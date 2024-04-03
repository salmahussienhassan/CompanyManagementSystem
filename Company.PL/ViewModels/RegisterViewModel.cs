using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="First Name Is Required")]
        public string FName { get; set; }
        [Required(ErrorMessage = "Last Name Is Required")]
        public string LName { get; set; }

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage ="Invaild Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Pasword Doesnt Match")]
        public string ConfirmedPassword { get; set; }

        [Required(ErrorMessage ="Required")]
        public bool IsAgree { get; set; }
    }
}
