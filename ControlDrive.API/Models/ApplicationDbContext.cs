using ControlDrive.API.Migrations.ApplicationDbContext;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ControlDrive.API.Models
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

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<ControlDrive.API.Models.Servicio> Servicios { get; set; }

        public System.Data.Entity.DbSet<ControlDrive.API.Models.Estado> Estados { get; set; }

        public System.Data.Entity.DbSet<ControlDrive.API.Models.Aseguradora> Aseguradoras { get; set; }

        public System.Data.Entity.DbSet<ControlDrive.API.Models.Asegurado> Asegurados { get; set; }

        public System.Data.Entity.DbSet<ControlDrive.API.Models.Conductor> Conductores { get; set; }

        public System.Data.Entity.DbSet<ControlDrive.API.Models.Vehiculo> Vehiculos { get; set; }

        public System.Data.Entity.DbSet<ControlDrive.API.Models.Seguimiento> Seguimientos { get; set; }

        public System.Data.Entity.DbSet<ControlDrive.API.Models.Ciudad> Ciudades { get; set; }

        public System.Data.Entity.DbSet<ControlDrive.CORE.Modelos.Correo> Correos { get; set; }

        public System.Data.Entity.DbSet<ControlDrive.CORE.Modelos.Cuenta> Cuentas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });



            modelBuilder.Entity<Servicio>()
                .HasOptional(A => A.Asegurado);

            modelBuilder.Entity<Servicio>()
                .HasOptional(C => C.Conductor);

            modelBuilder.Entity<Servicio>()
                .HasOptional(DI => DI.DireccionInicio);

            modelBuilder.Entity<Servicio>()
                .HasOptional(DF => DF.DireccionDestino);
            

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
