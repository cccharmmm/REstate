using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REstate1.Data.Entities
{
    [Table("Deal")]
    public class Deal
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Demand")]
        public int DemandId { get; set; }
        public virtual Demand Demand { get; set; }
        [ForeignKey("Supply")]
        public int SupplyId { get; set; }
        public virtual Supply Supply { get; set; }
    }
}
