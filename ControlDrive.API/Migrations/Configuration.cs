namespace ControlDrive.API.Migrations.ApplicationDbContext
{
    using CORE.Modelos;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ControlDrive.API.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ControlDrive.API.Models.ApplicationDbContext context)
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
                new Estado() { Codigo = "RG", Descripcion = "Registrado", Orden=1 },
                new Estado() { Codigo = "ES", Descripcion = "En sitio", Orden = 2 },
                new Estado() { Codigo = "EC", Descripcion = "En curso", Orden = 3 },
                new Estado() { Codigo = "TE", Descripcion = "Terminado", Orden = 4 },
                new Estado() { Codigo = "FL", Descripcion = "Fallido", Orden = 5 },
                new Estado() { Codigo = "CN", Descripcion = "Cancelado", Orden = 6 }
                
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
                    Email = "wi_alejo@hotmail.com",
                    TipoIdentificacion = "CC",
                    Identificacion = 1018421359,
                    Telefono1 = "3112150087",
                    Direccion = "Cra 51 # 66a 22 Br San miguel",
                    Activo = true
                });

            context.Asegurados.AddOrUpdate(
                new Asegurado() { Id = 1, Nombre = "Asegurado 1" }
            );

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
                CorreoSalida = "wi_alejo@hotmail.com",
                NombreMostrar = "ControlDrive",
                CorreoRespuesta = "wi_alejo@hotmail.com",
                NombreServidor = "smtp.live.com",
                NombreServidorIMAP = "controldrive.com.co",
                NombreServidorPOP = "controldrive.com.co",
                NombreServidorSMPT = "smtp.live.com",
                Puerto = 25,
                Ssl = true,
                Usuario = "wi_alejo@hotmail.com",
                Contrasena = "Alejo123"
            });
        }
    }
}
