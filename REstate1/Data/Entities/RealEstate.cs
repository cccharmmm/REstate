    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace REstate1.Data.Entities
    {
        [Table("RealEstate")]
        public class RealEstate
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            public string Address_City { get; set; }
            public string Address_Street { get; set; }
            public string Address_House { get; set; }
            public string Address_Number { get; set; }
            [ForeignKey("District")]

            public int? District_Id { get; set; }
            public virtual District District { get; set; }

            [ForeignKey("TypeRealEstate")]

            public int Id_type { get; set; }

            public virtual TypeRealEstate TypeRealEstate { get; set; }
            public virtual Apartment Apartment { get; set; }
            public virtual House House { get; set; }
            public virtual Land Land { get; set; }
            public virtual ICollection<Supply> Supplies { get; set; }
        }

    }
