using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;



public partial class PosContext : DbContext
{
    /**
    public PosContext()
    {
    }
    

    public PosContext(DbContextOptions<PosContext> options)
        : base(options)
    {
    }
    */
    public bool esNumero(string numero)
{
    try
    {
        int.Parse(numero);
        return true;
    }
    catch (Exception e)
    {
        return false;
    }
}

    public  DbSet<Producto> Productos { get; set; }

    public  DbSet<VentaProducto> VentaProductos { get; set; }

    public  DbSet<Venta> Ventas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){

        optionsBuilder.UseMySQL("server = localhost; port = 3308; database=POS; user = root; password =root123");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Idproducto).HasName("PRIMARY");

            entity.ToTable("producto", "POS");

            entity.HasIndex(e => e.Codigo, "codigo_UNIQUE").IsUnique();

            entity.Property(e => e.Idproducto).HasColumnName("idproducto");
            entity.Property(e => e.Codigo).HasColumnName("codigo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio).HasColumnName("precio");
        });

        modelBuilder.Entity<VentaProducto>(entity =>
        {
            entity.HasKey(e => e.IdventaProducto).HasName("PRIMARY");

            entity.ToTable("venta_producto", "POS");

            entity.HasIndex(e => e.Idproducto, "idproducto_idx");

            entity.HasIndex(e => e.Idventa, "idventa_idx");

            entity.Property(e => e.IdventaProducto).HasColumnName("idventa_producto");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Idproducto).HasColumnName("idproducto");
            entity.Property(e => e.Idventa).HasColumnName("idventa");
            entity.Property(e => e.Precio).HasColumnName("precio");

            entity.HasOne(d => d.IdproductoNavigation).WithMany(p => p.VentaProductos)
                .HasForeignKey(d => d.Idproducto)
                .HasConstraintName("idproducto");

            entity.HasOne(d => d.IdventaNavigation).WithMany(p => p.VentaProductos)
                .HasForeignKey(d => d.Idventa)
                .HasConstraintName("idventa");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.Idventa).HasName("PRIMARY");

            entity.ToTable("venta", "POS");

            entity.HasIndex(e => e.Correlativo, "correlativo_UNIQUE").IsUnique();

            entity.Property(e => e.Idventa).HasColumnName("idventa");
            entity.Property(e => e.Correlativo).HasColumnName("correlativo");
            entity.Property(e => e.Fecha)
                .HasColumnType("date")
                .HasColumnName("fecha");
            entity.Property(e => e.Hora)
                .HasColumnType("time")
                .HasColumnName("hora");
            entity.Property(e => e.RutCliente)
                .HasMaxLength(255)
                .HasColumnName("rutCliente");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
