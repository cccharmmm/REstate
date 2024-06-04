using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REstate1.Data.Entities
{
    [Table("ApartmentDemand")]
    public class ApartmentDemand
    {
        [Key]
        public int Id { get; set; }
        public float MinArea { get; set; }
        public float MaxArea { get; set; }
        public int MinRooms { get; set; }
        public int MaxRooms { get; set; }
        public int MinFloor { get; set; }
        public int MaxFloor { get; set; }

        public virtual ICollection<Demand> Demands { get; set; }

    }
}
