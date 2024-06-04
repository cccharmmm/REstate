using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REstate1.Data.Entities
{
    [Table("Apartment")]
    public class Apartment
    {
        [Key]

        [ForeignKey("RealEstate")]
        public int Id { get; set; }
        public int Floor { get; set; }
        public int Rooms { get; set; }
        public float TotalArea { get; set; }

        public RealEstate RealEstate { get; set; }
    }
}
