using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REstate1.Data.Entities
{
    [Table("Deal")]
    public class Deal
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Demand")]
        public int Demand_Id { get; set; }
        public virtual Demand Demand { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Supply")]
        public int Supply_Id { get; set; }
        public virtual Supply Supply { get; set; }
    }
}
