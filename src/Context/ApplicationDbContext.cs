using IWantApp.Domain.Models.Orders;
using IWantApp.Domain.Models.Products;

namespace IWantApp.Context;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Chamando o OnModelCreating da classe pai que é o IdentityDbContext <Importante>
        base.OnModelCreating(builder);

        builder.Ignore<Notification>();

        builder.Entity<Product>()
            .Property(p => p.Description).HasMaxLength(255);

        builder.Entity<Product>()
           .Property(p => p.Name).IsRequired();

        builder.Entity<Product>()
           .Property(p => p.Price).HasColumnType("decimal(10,2)").IsRequired();

        builder.Entity<Category>()
           .Property(c => c.Name).IsRequired();

        builder.Entity<Order>()
           .Property(o => o.ClientId).IsRequired();

        builder.Entity<Order>()
           .Property(o => o.DeliveryAddress).IsRequired();

        // Criando relacionamento de muitos para muitos
        builder.Entity<Order>()
           .HasMany(o => o.Products)
           .WithMany(o => o.Orders)
           .UsingEntity(x => x.ToTable("OrderProducts"));
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configuration)
    {
        configuration.Properties<string>()
            .HaveMaxLength(100);
    }
}
