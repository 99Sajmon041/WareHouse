using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouseSTARNET.Models
{
    public class Material
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Quantity { get; set; }
        public int CriticalQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ImageUrl { get; set; } = null!;

        [ForeignKey(nameof(TypeOfMaterialId))]
        public TypeOfMaterial TypeOfMaterial { get; set; } = null!;
        public int TypeOfMaterialId { get; set; }
    }
}
