using REstate1.Data.Entities;
using System.Data.Entity;

public class RealEstateContext : DbContext
{
    public RealEstateContext() : base("name=RealEstate")
    {
    }

    public DbSet<Agent> Agent { get; set; }
    public DbSet<Client> Client { get; set; }
    public DbSet<RealEstate> RealEstate { get; set; }
    public DbSet<Apartment> Apartment { get; set; }
    public DbSet<District> District { get; set; }
    public DbSet<Land> Land { get; set; }
    public DbSet<TypeRealEstate> TypeRealEstate { get; set; }
    public DbSet<House> House { get; set; }
    public DbSet<Supply> Supply { get; set; }
    public DbSet<Deal> Deals { get; set; }
    public DbSet<Demand> Demands { get; set; }

}
