using REstate1.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

public class RealEstateContext : DbContext
{
    public RealEstateContext() : base("name=RealEstate")
    {
    }


    public DbSet<Agent> Agent { get; set; }

        public DbSet<Client> Client { get; set; }
}
