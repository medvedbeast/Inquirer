using System.ComponentModel.DataAnnotations;

namespace InquirerAPI.Website.ViewModels
{
    public class LoginViewModel
    {
        [UIHint("email")]
        [Display(Name = "Електронна пошта")]
        [Required(ErrorMessage = "Поле не може бути пустим.")]
        [EmailAddress(ErrorMessage = "Поле має неправильний формат.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле не може бути пустим.")]
        [UIHint("password")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
