using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NotRealCoverWeb.Models
{
    public partial class NotRealCoverWebContext : DbContext
    {
        public NotRealCoverWebContext()
        {
        }

        public NotRealCoverWebContext(DbContextOptions<NotRealCoverWebContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DetFacturaVentum> DetFacturaVenta { get; set; } = null!;
        public virtual DbSet<FacturaVentum> FacturaVenta { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-EC207QH;Initial Catalog=NotRealCoverWeb;Integrated Security=True;Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetFacturaVentum>(entity =>
            {
                entity.Property(e => e.Album)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.IdFacturaVentaNavigation)
                    .WithMany(p => p.DetFacturaVenta)
                    .HasForeignKey(d => d.IdFacturaVenta)
                    .HasConstraintName("FK__DetFactur__IdFac__286302EC");
            });

            modelBuilder.Entity<FacturaVentum>(entity =>
            {
                entity.Property(e => e.Cliente)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Correlativo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FechaVenta).HasColumnType("date");

                entity.Property(e => e.TotalVenta).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuario");

                entity.Property(e => e.Apellido)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Rol)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
