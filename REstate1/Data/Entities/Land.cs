using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REstate1.Data.Entities
{
    [Table("Land")]
    public class Land
    {
        [Key]

        [ForeignKey("RealEstate")]
        public int Id { get; set; }
        public float? TotalArea { get; set; }

        public RealEstate RealEstate { get; set; }
    }
}
