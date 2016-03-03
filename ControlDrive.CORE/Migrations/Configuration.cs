namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using Core.Modelos;
    using CORE.Modelos;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ControlDrive.Core.Modelos.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

        }

        protected override void Seed(ControlDrive.Core.Modelos.ApplicationDbContext context)
        {
            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    Nombre = "Admininstrador del sistema",
                    UserName = "admin",
                    Email = "wi_alejo@hotmail.com",
                    PasswordHash = new PasswordHasher().HashPassword("admin"),
                    PhoneNumber = "3175104254",
                    SecurityStamp = Guid.NewGuid().ToString()

                });

            context.Estados.AddOrUpdate(
                new Estado() { Codigo = "RG", Descripcion = "Registrado", Orden = 1 },
                new Estado() { Codigo = "ES", Descripcion = "En sitio", Orden = 2 },
                new Estado() { Codigo = "EC", Descripcion = "En curso", Orden = 3 },
                new Estado() { Codigo = "TE", Descripcion = "Terminado", Orden = 4 },
                new Estado() { Codigo = "FL", Descripcion = "Fallido", Orden = 5 },
                new Estado() { Codigo = "CN", Descripcion = "Cancelado", Orden = 6 },
                new Estado() { Codigo = "AN", Descripcion = "Anulado", Orden = 7 }

            );
            context.Aseguradoras.AddOrUpdate(
                new Aseguradora() { Id = 1, Nombre = "Bolivar" },
                new Aseguradora() { Id = 2, Nombre = "Mapfre" },
                new Aseguradora() { Id = 3, Nombre = "Axa" }
            );
            //context.Conductores.AddOrUpdate(
            //    new Conductor()
            //    {
            //        Id = 1,
            //        Nombre = "Javier conductor martinez",
            //        Email = "wi_alejo@hotmail.com",
            //        TipoIdentificacion = "CC",
            //        Identificacion = 1018421359,
            //        Telefono1 = "3112150087",
            //        Direccion = "Cra 51 # 66a 22 Br San miguel",
            //        Activo = true
            //    });

            //context.Asegurados.AddOrUpdate(
            //    new Asegurado() { Id = 1, Nombre = "Asegurado 1" }
            //);

            //context.Cuentas.AddOrUpdate(new Cuenta
            //{
            //    Id = 1,
            //    Descripcion = "ControlDrive",
            //    CorreoSalida = "Notificaciones@controldrive.com.co",
            //    NombreMostrar = "ControlDrive",
            //    CorreoRespuesta = "Notificaciones@controldrive.com.co",
            //    NombreServidor = "controldrive.com.co",
            //    NombreServidorIMAP = "controldrive.com.co",
            //    NombreServidorPOP = "controldrive.com.co",
            //    NombreServidorSMPT = "controldrive.com.co",
            //    Puerto = 25,
            //    Ssl = false,
            //    Usuario = "Notificaciones@controldrive.com.co",
            //    Contrasena = "Loreka8812"
            //});

            context.Cuentas.AddOrUpdate(new Cuenta
            {
                Id = 1,
                Descripcion = "ControlDrive",
                CorreoSalida = "arhcontroldrive@gmail.com",
                NombreMostrar = "ARH - ControlDrive",
                CorreoRespuesta = "arhcontroldrive@gmail.com",
                NombreServidor = "smtp.gmail.com",
                NombreServidorIMAP = "smtp.gmail.com",
                NombreServidorPOP = "smtp.gmail.com",
                NombreServidorSMPT = "smtp.gmail.com",
                Puerto = 587,
                Ssl = true,
                Usuario = "arhcontroldrive@gmail.com",
                Contrasena = "Loreka8812"
            });
        }
    }
}
