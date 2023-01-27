using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Models.Tipos;

namespace Models
{
    public class Ctx : DbContext
    {
        public DbSet<StatusFactura> StatusFactura { get; set; }
        public DbSet<Persona> Persona { get; set; }
        public DbSet<TipoPersona> TipoPersona { get; set; }
        public DbSet<XmlNs> XmlNs { get; set; }

        private string dbFile;
        private string DbPath;

        public Ctx()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            this.dbFile = String.Format(@"{0}", "File.db");
            this.DbPath = System.IO.Path.Join(path, this.dbFile);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source=./Dts/{dbFile}");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Persona
            builder.Entity<Persona>(p =>
            {
                p.ToTable("Persona");
                p.HasKey(x => new { x.ID, x.Tipo });
                p.Property(x => x.ID).HasColumnName("ID");
                p.Property(x => x.Tipo).HasColumnName("Tipo");
                p.Property(x => x.Nombre).HasColumnName("Descripcion");
                p.Property(x => x.statusID).HasColumnName("statusID");
                p.Property(x => x.creado).HasColumnName("creado");
                p.Property(x => x.actualizado).HasColumnName("actualizado");

                #region TipoPersona
                p.HasOne(x => x.TipoPersona)
                .WithMany(y => y.Personas)
                .HasForeignKey(z => z.Tipo)
                .HasPrincipalKey(x => x.ID);
                #endregion

                p.Navigation(x => x.TipoPersona);
            });
            #endregion

            #region TipoPersona
            builder.Entity<TipoPersona>()
            .HasMany(t => t.Personas)
            .WithOne(p => p.TipoPersona);
            #endregion
        }
    }
}
