using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REstate1.Data.Entities
{
    [Table("Supply")]
    public class Supply
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        [ForeignKey("Agent")]
        public int AgentId { get; set; }
        public virtual Agent Agent { get; set; }
        [ForeignKey("RealEstate")]
        public int RealEstateId { get; set; }
        public virtual RealEstate RealEstate { get; set; }
        public long Price { get; set; }
        public virtual ICollection<Deal> Deals { get; set; }
    }

}
