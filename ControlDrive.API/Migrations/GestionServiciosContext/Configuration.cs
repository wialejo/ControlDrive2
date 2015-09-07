namespace ControlDrive.API.Migrations.GestionServiciosContext
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
            MigrationsDirectory = @"Migrations\GestionServiciosContext";
        }

        protected override void Seed(ControlDrive.API.Models.GestionServiciosContext context)
        {

            context.Estados.AddOrUpdate(
                new Estado() { Codigo = "PD", Descripcion = "Pendiente" },
                new Estado() { Codigo = "EJ", Descripcion = "Ejecucion" },
                new Estado() { Codigo = "RG", Descripcion = "Registro" },
                new Estado() { Codigo = "AS", Descripcion = "Asignado" }

            );
            context.Aseguradoras.AddOrUpdate(
                new Aseguradora() { Id = 1, Nombre = "Bolivar" },
                new Aseguradora() { Id = 2, Nombre = "Mapfre" },
                new Aseguradora() { Id = 3, Nombre = "Axa" }
                );

            context.Conductores.AddOrUpdate(
                new Conductor()
                {
                    Id = 1,
                    Nombre = "Javier conductor martinez",
                    Email = "javier@controldrive.com",
                    TipoIdentificacion = "CC",
                    Identificacion = 1018421359,
                    Telefono1 = "3112150087",
                    Direccion = "Cra 51 # 66a 22 Br San miguel",
                    Sexo = Enums.Sexo.Masculino,
                    Activo = true
                });

            context.Asegurados.AddOrUpdate(
                new Asegurado() { Id = 1, Nombre = "Asegurado 1" }
            );
        }
    }
}
