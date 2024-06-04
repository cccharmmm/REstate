using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REstate1.Data.Entities
{
    [Table("District")]
    public class District
    {
        [Key]

        public int ID { get; set; }
        public string Name { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string Area { get; set; }

        public ICollection<RealEstate> RealEstates { get; set; }
    }
}
