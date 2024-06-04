using System.ComponentModel.DataAnnotations.Schema;

namespace REstate1.Data.Entities
{
    [Table("Supply")]
    public class Supply
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int AgentId { get; set; }
        public int RealEstateId { get; set; }
        public double Price { get; set; }
        public RealEstate RealEstate { get; set; }
    }
}
