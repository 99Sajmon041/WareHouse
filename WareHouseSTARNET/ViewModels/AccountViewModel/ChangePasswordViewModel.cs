using System.ComponentModel.DataAnnotations;

namespace WareHouseSTARNET.ViewModels.AccountViewModel
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Zadejte aktuální heslo!")]
        [Display(Name = "Aktuální heslo")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = null!;


        [Required(ErrorMessage = "Zadejte nové heslo!")]
        [Display(Name = "Nové heslo")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;


        [Required(ErrorMessage = "Zadejte nové heslo!")]
        [Compare(nameof(NewPassword), ErrorMessage = "Hesla se neshodují!")]
        [Display(Name = "Potvrdit nové heslo")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; } = null!;
    }
}
