using System.ComponentModel.DataAnnotations;

namespace InquirerAPI.Website.ViewModels
{
    public class EditKeyViewModel
    {
        [Required(ErrorMessage = "Поле не може бути пустим.")]
        public string Name { get; set; }

        public int TypeId { get; set; }
    }
}
