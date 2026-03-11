using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Domain.Entities;

namespace SistemaPedidos.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<PrecioProducto> PreciosProducto => Set<PrecioProducto>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<PedidoItem> PedidoItems => Set<PedidoItem>();
    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<Pago> Pagos => Set<Pago>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // PrecioProducto relación
        modelBuilder.Entity<PrecioProducto>()
            .HasOne(p => p.Producto)
            .WithMany(p => p.Precios)
            .HasForeignKey(p => p.ProductoId);

        // PedidoItem relación
        modelBuilder.Entity<PedidoItem>()
            .HasOne(p => p.Pedido)
            .WithMany(p => p.Items)
            .HasForeignKey(p => p.PedidoId);

        // Venta relación
        modelBuilder.Entity<Venta>()
            .HasOne(v => v.Pedido)
            .WithOne()
            .HasForeignKey<Venta>(v => v.PedidoId);

        // Pago relación
        modelBuilder.Entity<Venta>()
        .Property(v => v.TotalProductos)
        .HasPrecision(18, 2);

        modelBuilder.Entity<Venta>()
            .Property(v => v.TotalEnvio)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Venta>()
            .Property(v => v.TotalGeneral)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Pedido>()
            .Property(p => p.PrecioEnvio)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Pedido>()
            .Property(p => p.TotalProductos)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Pedido>()
            .Property(p => p.TotalGeneral)
            .HasPrecision(18, 2);


        modelBuilder.Entity<PedidoItem>()
            .Property(p => p.PrecioUnitario)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PedidoItem>()
            .Property(p => p.Subtotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PrecioProducto>()
            .Property(p => p.Importe)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Pago>()
            .Property(p => p.Monto)
            .HasPrecision(18, 2);

    }
}
