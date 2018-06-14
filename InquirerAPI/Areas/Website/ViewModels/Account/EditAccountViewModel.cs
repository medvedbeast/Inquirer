using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InquirerAPI.Website.ViewModels
{
    public class EditAccountViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Поле не може бути пустим.")]
        public string Name { get; set; }

        [UIHint("email")]
        [Required(ErrorMessage = "Поле не може бути пустим.")]
        [EmailAddress(ErrorMessage = "Значення не є валідною адресою.")]
        public string Email { get; set; }

        [UIHint("password")]
        public string Password { get; set; }

        [UIHint("password")]
        public string PasswordConfirmation { get; set; }
    }
}
