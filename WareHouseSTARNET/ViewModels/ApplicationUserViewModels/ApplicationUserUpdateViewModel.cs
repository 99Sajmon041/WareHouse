using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WareHouseSTARNET.ViewModels.UserViewModels
{
    public class ApplicationUserUpdateViewModel
    {
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "Křestí jméno je povinný údaj!")]
        [Display(Name = "Křeštní jméno")]
        public string FirstName { get; set; } = null!;


        [Required(ErrorMessage = "Příjmení je povinný údaj!")]
        [Display(Name = "Příjmení")]
        public string LastName { get; set; } = null!;


        [Required(ErrorMessage = "E-mail je povinný údaj!"), EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Telefonní číslo je povinný údaj!"), Phone]
        [Display(Name = "Telefonní číslo")]
        public string PhoneNumber { get; set; } = null!;


        [Required(ErrorMessage = "Datum narození je povinný údaj!"), DataType(DataType.Date)]
        [Display(Name = "Datum narození")]
        public DateTime DateOfBirth { get; set; }


        [Required(ErrorMessage = "Role je povinný údaj!")]
        [Display(Name = "Role")]
        public string Role { get; set; } = null!;

        public IEnumerable<SelectListItem> Roles { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
