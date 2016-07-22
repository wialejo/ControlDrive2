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
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;

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
                new Estado() { Codigo = "RG", Descripcion = "Registrado", Orden = 2, EnOperacion = true },
                new Estado() { Codigo = "RE", Descripcion = "Recibido", Orden = 6, EnOperacion = true },
                new Estado() { Codigo = "ES", Descripcion = "En sitio", Orden = 8, EnOperacion = true },
                new Estado() { Codigo = "EC", Descripcion = "En curso", Orden = 10, EnOperacion = true },
                new Estado() { Codigo = "FL", Descripcion = "Fallido", Orden = 12, EnOperacion = true },
                new Estado() { Codigo = "CN", Descripcion = "Cancelado", Orden = 12, EnOperacion = true },
                new Estado() { Codigo = "AN", Descripcion = "Anulado", Orden = 14, EnOperacion = true },
                new Estado() { Codigo = "TE", Descripcion = "Finalizado", Orden = 16, EnOperacion = true },
                new Estado() { Codigo = "CR", Descripcion = "Cierre finalizado", Orden = 18, EnOperacion = true },
                new Estado() { Codigo = "CF", Descripcion = "Cierre fallido", Orden = 19, EnOperacion = true },
                new Estado() { Codigo = "FA", Descripcion = "Facturado", Orden = 20, EnOperacion = false }
            );
            context.Aseguradoras.AddOrUpdate(
                new Aseguradora() { Id = 1, Nombre = "Bolivar" },
                new Aseguradora() { Id = 2, Nombre = "Mapfre" },
                new Aseguradora() { Id = 3, Nombre = "Axa-colpatria" },
                new Aseguradora() { Id = 4, Nombre = "Andi" },
                new Aseguradora() { Id = 5, Nombre = "Particular" },
                new Aseguradora() { Id = 6, Nombre = "ARH" },
                new Aseguradora() { Id = 7, Nombre = "Axa-previsora" },
                new Aseguradora() { Id = 8, Nombre = "Axa-helm" }
            );

            //context.TiposServicios.AddOrUpdate(
            //    new TipoServicio { Id = 1, Descripcion = "Conductor elegido", RequiereSeguimiento = true },
            //    new TipoServicio { Id = 2, Descripcion = "Coordinación", RequiereSeguimiento = false },
            //    new TipoServicio { Id = 3, Descripcion = "Valet", RequiereSeguimiento = true },
            //    new TipoServicio { Id = 4, Descripcion = "Mensajeria", RequiereSeguimiento = true },
            //    new TipoServicio { Id = 5, Descripcion = "Ejecutivo", RequiereSeguimiento = true },
            //    new TipoServicio { Id = 6, Descripcion = "Valet conductor elegido", RequiereSeguimiento = true }
            //    );
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
