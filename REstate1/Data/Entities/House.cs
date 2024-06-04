using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REstate1.Data.Entities
{
    [Table("House")]
    public class House
    {
        [Key]

        [ForeignKey("RealEstate")]
        public int Id { get; set; }
        public int TotalFloors { get; set; }
        public int Rooms { get; set; }
        public float TotalArea { get; set; }

        public RealEstate RealEstate { get; set; }
    }

}
