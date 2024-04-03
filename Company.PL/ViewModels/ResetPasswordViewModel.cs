using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="New Password Is Required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "New Confirm Password Is Required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage ="Password Doesnt Match")]
        public string NewConfirmPassword { get; set;}
    }
}
