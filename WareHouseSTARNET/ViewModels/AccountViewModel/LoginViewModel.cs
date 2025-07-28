using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace WareHouseSTARNET.ViewModels.AccountViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email je povinný údaj!")]
        [EmailAddress(ErrorMessage = "Zadejte platnou emailovou adresu!")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Heslo je povinný údaj!")]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = null!;

        [Display(Name = "Zapamatovat si mě")]
        public bool RememberMe { get; set; }
    }
}
