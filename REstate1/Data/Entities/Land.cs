using System.ComponentModel.DataAnnotations.Schema;

namespace REstate1.Data.Entities
{
    [Table("Land")]
    public class Land
    {
        [ForeignKey("RealEstate")]
        public int Id { get; set; }
        public float TotalArea { get; set; }

        public RealEstate RealEstate { get; set; }
    }
}
