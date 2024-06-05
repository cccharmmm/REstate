using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REstate1.Data.Entities
{
    [Table("HouseDemand")]
    public class HouseDemand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public float ?MinArea { get; set; }
        public float ?MaxArea { get; set; }
        public int ?MinRooms { get; set; }
        public int ?MaxRooms { get; set; }
        public int ?MinFloors { get; set; }
        public int ?MaxFloors { get; set; }

        public virtual ICollection<Demand> Demands { get; set; }

    }
}
