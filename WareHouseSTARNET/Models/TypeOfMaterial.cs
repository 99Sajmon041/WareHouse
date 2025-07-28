using System.ComponentModel.DataAnnotations;

namespace WareHouseSTARNET.Models
{
    public class TypeOfMaterial
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; } = null!;
    }
}
