using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareHouseSTARNET.Models
{
    public class WrittenOffMaterial
    {
        [Key]
        public int Id { get; set; }

        public DateTime WrittenOffDate { get; set; }

        public int Quantity { get; set; }


        [ForeignKey(nameof(MaterialId))]
        public Material Material { get; set; } = null!;

        public int MaterialId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser? ApplicationUser { get; set; }
        public string? ApplicationUserId { get; set; }
    }
}
