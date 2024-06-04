using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REstate1.Data.Entities
{
    [Table("TypeRealEstate")]
    public class TypeRealEstate
    {
        [Key]
        public int Id_type { get; set; }

        public string Name { get; set; }

        public ICollection<RealEstate> RealEstates { get; set; }
    }
}
