using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REstate1.Data.Entities
{
    [Table("LandDemand")]
    public class LandDemand
    {
        [Key]
        public int Id { get; set; }
        public double MinArea { get; set; }
        public double MaxArea { get; set; }

        public virtual ICollection<Demand> Demands { get; set; }

    }
}
