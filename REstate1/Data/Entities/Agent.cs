using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace REstate1.Data.Entities
{
    [Table("Agent")]
    public class Agent
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public int DealShare { get; set; }

        public virtual ICollection<Demand> Demands { get; set; }
        public virtual ICollection<Supply> Supplies { get; set; }
    }

}
