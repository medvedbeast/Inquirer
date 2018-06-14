using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InquirerAPI.Website.ViewModels
{
    public class CreateKeyViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Поле не може бути пустим.")]
        public string Name { get; set; }

        public int TypeId { get; set; }
    }
}
