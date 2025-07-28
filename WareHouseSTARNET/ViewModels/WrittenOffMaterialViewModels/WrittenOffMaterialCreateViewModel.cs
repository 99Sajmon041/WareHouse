using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WareHouseSTARNET.Models;

namespace WareHouseSTARNET.ViewModels.WrittenOffMaterialViewModels
{
    public class WrittenOffMaterialCreateViewModel
    {
        [Required(ErrorMessage = "Zvolte datum!")]
        [DataType(DataType.Date)]
        [Display(Name = "Datum")]
        public DateTime WrittenOffDate { get; set; }


        [Required(ErrorMessage = "Množství je požadované pole!")]
        [Display(Name = "Množství materiálu")]
        [Range(1, 20, ErrorMessage = "Zvolte množství materiálu, který je možné odepsat (max. 20)!")]
        public int Quantity { get; set; }


        [Required(ErrorMessage = "Zvolte materiál!")]
        [Display(Name = "Materiál")]
        public int MaterialId { get; set; }
        public IEnumerable<SelectListItem> Materials { get; set; } = Enumerable.Empty<SelectListItem>();


        [Required(ErrorMessage = "Zvolte technika!")]
        [Display(Name = "Technik")]
        public string ApplicationUserId { get; set; } = null!;

        public IEnumerable<SelectListItem> ApplicationUsers { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
