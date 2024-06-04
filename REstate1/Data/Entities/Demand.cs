using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REstate1.Data.Entities
{
    [Table("Demand")]
    public class Demand
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        [ForeignKey("Agent")]
        public int AgentId { get; set; }
        public virtual Agent Agent { get; set; }
        [ForeignKey("TypeRealEstate")]
        public int Id_type { get; set; }
        public virtual TypeRealEstate TypeRealEstate { get; set; }
        public string Address_City { get; set; }
        public string Address_Street { get; set; }
        public string Address_House { get; set; }
        public string Address_Number { get; set; }
        public long? MinPrice { get; set; }
        public long? MaxPrice { get; set; }
        [ForeignKey("HouseDemand")]
        public int? Id_HouseDemand { get; set; }
        public virtual HouseDemand HouseDemand { get; set; }
        [ForeignKey("LandDemand")]
        public int? Id_LandDemand { get; set; }
        public virtual LandDemand LandDemand { get; set; }
        [ForeignKey("ApartmentDemand")]
        public int? Id_ApartmentDemand { get; set; }
        public virtual ApartmentDemand ApartmentDemand { get; set; }
        public virtual ICollection<Deal> Deals { get; set; }
    }
}
