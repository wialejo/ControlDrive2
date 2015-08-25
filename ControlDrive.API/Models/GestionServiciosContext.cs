using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ControlDrive.API.Models
{
    public class GestionServiciosContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public GestionServiciosContext() : base("name=GestionServiciosContext")
        {
        }

        public System.Data.Entity.DbSet<ControlDrive.API.Models.Servicio> Servicios { get; set; }

        public System.Data.Entity.DbSet<ControlDrive.API.Models.Estado> Estados { get; set; }

        public System.Data.Entity.DbSet<ControlDrive.API.Models.Aseguradora> Aseguradoras { get; set; }

        public System.Data.Entity.DbSet<ControlDrive.API.Models.Asegurado> Asegurados { get; set; }

        public System.Data.Entity.DbSet<ControlDrive.API.Models.Conductor> Conductores { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();


            // modelBuilder.Entity<Servicio>()
            //.Property(l => l.Id)
            //.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
