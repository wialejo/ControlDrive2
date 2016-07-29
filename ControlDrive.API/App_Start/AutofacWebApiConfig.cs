using Autofac;
using Autofac.Core;
using Autofac.Integration.WebApi;
using ControlDrive.Core.Infraestructura;
using ControlDrive.Core.Modelos;
using ControlDrive.CORE.Modelos;
using ControlDrive.CORE.Repositorios;
using ControlDrive.CORE.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace ControlDrive.Core.App_Start
{
    public class AutofacWebApiConfig
    {
        public static IContainer Container;
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder()));
        }

        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<ApplicationDbContext>()
                   .As<IdentityDbContext<ApplicationUser>>()
                   .InstancePerRequest();

            builder.RegisterType<DbFactory>()
                .As<IDbFactory>()
                .InstancePerRequest();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest();

            builder.RegisterGeneric(typeof(EntityBaseRepository<>))
                   .As(typeof(IEntityBaseRepository<>))
                   .InstancePerRequest();

            //// Services
            builder.RegisterType<ServicioService>()
                .As<ICommonInterface<Servicio>, IServicioService>()
                .InstancePerRequest();

            builder.RegisterType<SeguimientoService>()
                .As<ICommonInterface<Seguimiento>, ISeguimientoService>()
                .InstancePerRequest();

            builder.RegisterType<EstadoService>()
                .As<ICommonInterface<Estado>, IEstadoService>()
                .InstancePerRequest();

            builder.RegisterType<DireccionService>()
                .As<ICommonInterface<Direccion>>()
                .InstancePerRequest();
            
            builder.RegisterType<ConductorService>()
                .As<IConductorService>()
                .InstancePerRequest();

            builder.RegisterType<AseguradoraService>()
                .As<ICommonInterface<Aseguradora>>()
                .InstancePerRequest();

            builder.RegisterType<AseguradoService>()
                .As<ICommonInterface<Asegurado>>()
                .InstancePerRequest();

            builder.RegisterType<VehiculoService>()
                .As<ICommonInterface<Vehiculo>>()
                .InstancePerRequest();

            builder.RegisterType<CiudadService>()
                .As<ICommonInterface<Ciudad>>()
                .InstancePerRequest();

            builder.RegisterType<SucursalService>()
                .As<ICommonInterface<Sucursal>, ISucursalService>()
                .InstancePerRequest();

            builder.RegisterType<CorreoService>()
                .As<ICorreoService>()
                .InstancePerRequest();

            builder.RegisterType<MovimientosService>()
                .As<IMovimientosService>()
                .InstancePerRequest();

            builder.RegisterType<UsuarioService>()
                .As<IUsuarioService>()
                .InstancePerRequest();

            builder.RegisterType<DocumentosService>()
                .As<IDocumentosService>()
                .InstancePerRequest();

            builder.RegisterType<TiposServicioService>()
                .As<ITiposServiciosService>()
                .InstancePerRequest();
            Container = builder.Build();

            return Container;
        }
    }
}