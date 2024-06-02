using REstate1.Data.Entities;
using System.Data.Entity;

public class RealEstateContext : DbContext
{
    public RealEstateContext() : base("name=RealEstate")
    {
    }

    public DbSet<Agent> Agent { get; set; }
}
