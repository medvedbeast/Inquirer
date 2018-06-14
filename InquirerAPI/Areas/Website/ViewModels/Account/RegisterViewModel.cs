using System.ComponentModel.DataAnnotations;

namespace InquirerAPI.Website.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле не може бути пустим.")]
        [Display(Name = "Електронна пошта")]
        [EmailAddress(ErrorMessage = "Поле має неправильний формат.")]
        [UIHint("email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле не може бути пустим.")]
        [Display(Name = "Пароль")]
        [UIHint("password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле не може бути пустим.")]
        [Display(Name = "І'мя")]
        public string Name { get; set; }
    }
}
