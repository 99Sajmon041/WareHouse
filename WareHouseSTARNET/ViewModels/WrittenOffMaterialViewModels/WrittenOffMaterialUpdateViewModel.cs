using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WareHouseSTARNET.ViewModels.WrittenOffMaterialViewModels
{
    public class WrittenOffMaterialUpdateViewModel
    {
        public int Id { get; set; }

        public DateTime WritteOfDate { get; set; }


        [Required(ErrorMessage = "Množství je požadované pole!")]
        [Range(1, 20, ErrorMessage = "Zvolte množství materiálu, který chcete odepsat!")]
        public int Quantity { get; set; }


        [Required(ErrorMessage = "Zvolte materiál!")]
        public int MaterialId { get; set; }
        public IEnumerable<SelectListItem> Materials { get; set; } = Enumerable.Empty<SelectListItem>();


        [Required(ErrorMessage = "Zvolte technika!")]
        public string ApplicationUserId { get; set; } = null!;
        public IEnumerable<SelectListItem> ApplicationUsers { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
