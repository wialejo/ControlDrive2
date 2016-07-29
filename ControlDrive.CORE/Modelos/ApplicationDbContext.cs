using ControlDrive.API.Migrations.ApplicationDbContext;
using ControlDrive.CORE.Modelos;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ControlDrive.Core.Modelos
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public ApplicationDbContext() : base("name=ControlDriveDBConnectionString")
        {
            Database.SetInitializer<ApplicationDbContext>(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public virtual void Commit()
        {
            try
            {
                base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Servicio> Servicios { get; set; }

        public DbSet<Estado> Estados { get; set; }

        public DbSet<Aseguradora> Aseguradoras { get; set; }

        public DbSet<Asegurado> Asegurados { get; set; }

        public DbSet<Conductor> Conductores { get; set; }

        public DbSet<Vehiculo> Vehiculos { get; set; }

        public DbSet<Seguimiento> Seguimientos { get; set; }

        public DbSet<Ciudad> Ciudades { get; set; }

        public DbSet<Correo> Correos { get; set; }

        public DbSet<Cuenta> Cuentas { get; set; }

        public DbSet<Movimiento> Movimientos { get; set; }

        public DbSet<Documento> Documentos { get; set; }

        public DbSet<Empresa> Empresas { get; set; }

        public DbSet<ConceptoPago> ConceptosPagos { get; set; }

        public DbSet<TipoServicio> TiposServicios { get; set; }

        public DbSet<Sucursal> Sucursales { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });


            modelBuilder.Entity<ConceptoPago>()
                .HasMany<TipoServicio>(p => p.TiposServicios)
                .WithMany(c => c.ConceptosPagos);

            //modelBuilder.Entity<Servicio>()
            //    .HasOptional(s => s.Movimientos)
            //    .WithRequired(s => s.servicio)
            //    .WillCascadeOnDelete(false);


            //modelBuilder.Entity<Servicio>()
            //    .HasOptional(A => A.Asegurado);

            //modelBuilder.Entity<Servicio>()
            //    .HasOptional(C => C.Conductor);



            //modelBuilder.Entity<Direccion>()
            //    .HasRequired(C => C.Ciudad)
            //    .WithMany(d => d.Direcciones)
            //    .HasForeignKey(d => d.CiudadId);


            // modelBuilder.Entity<Servicio>()
            //.Property(l => l.Id)
            //.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }

    }
}
