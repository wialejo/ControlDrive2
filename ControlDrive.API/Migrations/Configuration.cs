namespace ControlDrive.API.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ControlDrive.API.Models.GestionServiciosContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ControlDrive.API.Models.GestionServiciosContext context)
        {
            var EstadoPendiente = new Estado() { Codigo = "PD", Descripcion = "Pendiente" };
            var EstadoEjecucion = new Estado() { Codigo = "EJ", Descripcion = "Ejecucion" };
            var EstadoRegistro = new Estado() { Codigo = "RG", Descripcion = "Registro" };

            var AseguradoraBolivar = new Aseguradora() { Id = 1, Nombre = "Bolivar" };
            var Asegurado1 = new Asegurado() { Id = 1, Nombre = "Asegurado 1" };
            var Conductor1 = new Conductor() { Id = 1, Nombre = "Conductor 2" };

            context.Conductores.AddOrUpdate(Conductor1);
            context.Asegurados.AddOrUpdate(Asegurado1);
            context.Aseguradoras.AddOrUpdate(AseguradoraBolivar);
            context.Estados.AddOrUpdate(EstadoPendiente);
            context.Estados.AddOrUpdate(EstadoRegistro);

            context.Estados.AddOrUpdate(EstadoEjecucion);
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
