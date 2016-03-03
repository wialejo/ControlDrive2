using ControlDrive.Core.Infraestructura;
using ControlDrive.Core.Modelos;
using ControlDrive.CORE.Modelos;
using ControlDrive.CORE.Repositorios;
using FileHelpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ControlDrive.CORE.Services
{
    public class ServicioService : ICommonInterface<Servicio>, IServicioService
    {

        private readonly IEntityBaseRepository<Servicio> _servicioRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommonInterface<Direccion> _direccionService;
        private readonly ICommonInterface<Asegurado> _aseguradoService;
        private readonly ICommonInterface<Vehiculo> _vehiculoService;
        private readonly ICorreoService _correoService;

        public ServicioService(IEntityBaseRepository<Servicio> servicioRepositorio, ICommonInterface<Direccion> direccionService,
            ICommonInterface<Asegurado> aseguradoService, ICommonInterface<Vehiculo> vehiculoService, ICorreoService correoService, IUnitOfWork unitOfWork)
        {
            _servicioRepositorio = servicioRepositorio;
            _direccionService = direccionService;
            _aseguradoService = aseguradoService;
            _vehiculoService = vehiculoService;
            _correoService = correoService;
            _unitOfWork = unitOfWork;
        }

        public Servicio Guardar(Servicio servicio)
        {
            var servicioRepo = new Servicio();




            if (servicio.Id == 0)
            {

                if (_servicioRepositorio.FindBy(s => s.Radicado == servicio.Radicado).Count() > 0 && !string.IsNullOrEmpty(servicio.Radicado))
                {
                    throw new Exception("El consecutivo ingresado ya se encuentra registrado.");
                }
                servicioRepo.EstadoCodigo = "RG";
                servicioRepo.Fecha = servicio.Fecha;
                servicioRepo.Hora = servicio.Hora;
                servicioRepo.Radicado = servicio.Radicado;
                servicioRepo.AsignadoPor = servicio.AsignadoPor;
                servicioRepo.AseguradoraId = servicio.Aseguradora.Id;

                _aseguradoService.Guardar(servicio.Asegurado);
                servicioRepo.AseguradoId = servicio.Asegurado.Id;

                _vehiculoService.Guardar(servicio.Vehiculo);
                servicioRepo.VehiculoId = servicio.Vehiculo.Id;

                _direccionService.Guardar(servicio.DireccionInicio);
                servicioRepo.DireccionInicioId = servicio.DireccionInicio.Id;

                _direccionService.Guardar(servicio.DireccionDestino);
                servicioRepo.DireccionDestinoId = servicio.DireccionDestino.Id;

                servicioRepo.FechaModificacion = DateTime.Now;
                servicioRepo.FechaRegistro = DateTime.Now;

                if (servicio.Conductor == null)
                    servicioRepo.ConductorId = null;
                else
                    servicioRepo.ConductorId = servicio.Conductor.Id;

                if (servicio.Ruta == null)
                    servicioRepo.RutaId = null;
                else
                    servicioRepo.RutaId = servicio.Ruta.Id;
                
                servicioRepo = _servicioRepositorio.Add(servicioRepo);
            }
            else {

                if (!string.IsNullOrEmpty(servicio.Radicado) && _servicioRepositorio.FindBy(s => s.Radicado == servicio.Radicado && s.Id != servicio.Id).Count() > 0)
                {
                    throw new Exception("El consecutivo ingresado ya se encuentra registrado.");
                }

                servicioRepo = _servicioRepositorio.FindBy(s => s.Id == servicio.Id).FirstOrDefault();
                servicioRepo.Fecha = servicio.Fecha;
                servicioRepo.Hora = servicio.Hora;
                servicioRepo.Radicado = servicio.Radicado;
                servicioRepo.AsignadoPor = servicio.AsignadoPor;
                servicioRepo.AseguradoraId = servicio.Aseguradora.Id;

                _aseguradoService.Guardar(servicio.Asegurado);
                servicioRepo.AseguradoId = servicio.Asegurado.Id;

                _vehiculoService.Guardar(servicio.Vehiculo);
                servicioRepo.VehiculoId = servicio.Vehiculo.Id;

                _direccionService.Guardar(servicio.DireccionInicio);
                servicioRepo.DireccionInicioId = servicio.DireccionInicio.Id;

                _direccionService.Guardar(servicio.DireccionDestino);
                servicioRepo.DireccionDestinoId = servicio.DireccionDestino.Id;

                servicioRepo.FechaModificacion = DateTime.Now;

                if(servicio.Conductor == null)
                    servicioRepo.ConductorId = null;
                else
                    servicioRepo.ConductorId = servicio.Conductor.Id;

                if (servicio.Ruta == null)
                    servicioRepo.RutaId = null;
                else
                    servicioRepo.RutaId = servicio.Ruta.Id;


                _servicioRepositorio.Edit(servicioRepo);
            }

            _unitOfWork.Commit();

            return servicioRepo;
        }

        public List<Servicio> Obtener()
        {
            var servicios = _servicioRepositorio.GetAll().ToList();
            return servicios;
        }

        public List<Servicio> ObtenerPorDescripcion(string descripcion)
        {
            throw new NotImplementedException();
        }

        public Servicio ObtenerPorId(int id)
        {
            var servicio = _servicioRepositorio.FindBy(s => s.Id == id).FirstOrDefault();
            return servicio;
        }
        
        public void Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public List<Servicio> Obtener(Periodo periodo)
        {
            var servicios = _servicioRepositorio
                .FindByIncluding(s => s.Fecha > periodo.Inicio && s.Fecha < periodo.Fin
                    //,s => s.DireccionInicioa => a.Ruta, s => s.Conductor, s => s.DireccionInicio, s => s.DireccionDestino, s => s.Asegurado, s => s.Aseguradora, s => s.Vehiculo
                    )
                .OrderBy(s => s.Fecha).ToList();
            return servicios;
        }

        public List<Servicio> ObtenerParaSeguimiento(Periodo periodo)
        {
            var periodoCompleto = new PeriodoService().Obtener(periodo.Inicio);
            var servicios = _servicioRepositorio
                .FindBy(s => s.Fecha > periodoCompleto.Inicio && s.Fecha < periodoCompleto.Fin && s.EstadoCodigo != "AN")
                .OrderBy(s => s.Fecha).ToList();

            return servicios;
        }

        public string ObtenerPorPeriodoCSV(Periodo periodo)
        {
            var servicios = _servicioRepositorio.FindBy(s => s.Fecha > periodo.Inicio && s.Fecha < periodo.Fin && s.Estado.Codigo != "AN").OrderBy(s => s.Fecha).ToList();
            var csv = ServiciosToCsvData(servicios);
            return csv;
        }

        private string ServiciosToCsvData(List<Servicio> servicios)
        {
            var serviciosExport = new List<serviciosModeloToExport>();
            servicios.ForEach(f =>
            {
                var servicio = new serviciosModeloToExport
                {
                    hora = f.Fecha.ToString("hh:mm"),
                    fecha = f.Fecha.ToString("dd/MM/yyyy"),
                    codigo = f.Radicado,
                    aseguradora = f.Aseguradora.Nombre,
                    asegurado = 
                            f.Asegurado.Nombre +
                            (!string.IsNullOrEmpty(f.Asegurado.Telefono1) ? " " + f.Asegurado.Telefono1 : "") +
                            (!string.IsNullOrEmpty(f.Asegurado.Telefono2) ? " " + f.Asegurado.Telefono2 : ""),

                    vehiculo = 
                            f.Vehiculo.Placa +
                            (!string.IsNullOrEmpty(f.Vehiculo.Marca) ? " " + f.Vehiculo.Marca : "") +
                            (!string.IsNullOrEmpty(f.Vehiculo.Referencia) ? " " + f.Vehiculo.Referencia : "") +
                            (!string.IsNullOrEmpty(f.Vehiculo.Observaciones) ? " " + f.Vehiculo.Observaciones : ""),

                    origen = 
                            f.DireccionInicio.Descripcion +
                            (!string.IsNullOrEmpty(f.DireccionInicio.Barrio) ? " " + f.DireccionInicio.Barrio : "") +
                            (!string.IsNullOrEmpty(f.DireccionInicio.Ciudad.Nombre) ? " " + f.DireccionInicio.Ciudad.Nombre : ""),

                    destino = 
                            f.DireccionDestino.Descripcion +
                            (!string.IsNullOrEmpty(f.DireccionDestino.Barrio) ? " " + f.DireccionDestino.Barrio : "") +
                            (!string.IsNullOrEmpty(f.DireccionDestino.Ciudad.Nombre) ? " " + f.DireccionDestino.Ciudad.Nombre : ""),

                    conductor = (f.Conductor != null) ?
                            (!string.IsNullOrEmpty(f.Conductor.Nombre) ? f.Conductor.Nombre : "") +
                            (!string.IsNullOrEmpty(f.Conductor.Telefono1) ? " " + f.Conductor.Telefono1 : "") : "",

                    ruta = (f.Ruta != null) ?
                            (!string.IsNullOrEmpty(f.Ruta.Nombre) ? f.Ruta.Nombre : "") +
                            (!string.IsNullOrEmpty(f.Ruta.Telefono1) ? " " + f.Ruta.Telefono1 : "") : "",

                    asignadoPor = f.AsignadoPor,
                    estado = f.Estado.Descripcion
                };

                serviciosExport.Add(servicio);
            });
            var engine = new FileHelperEngine<serviciosModeloToExport>();

            string strSeparadoPorComas = engine.WriteString(serviciosExport);

            return strSeparadoPorComas;
        }

        public void CambioEstado(int idServicio, Estado nuevoEstado)
        {
            var servicioRepo = _servicioRepositorio.FindBy(s => s.Id == idServicio).FirstOrDefault();
            servicioRepo.EstadoCodigo = nuevoEstado.Codigo;
            _servicioRepositorio.Edit(servicioRepo);
            _unitOfWork.Commit();
        }

        public string NotificarServiciosARuta(ICollection<Servicio> servicios)
        {
            IEnumerable<Conductor> Rutas = servicios.DistinctBy(s => s.RutaId).Select(c => c.Ruta);


            string respuesta = string.Empty;
            var rutasValidos = Rutas.Where(c => !string.IsNullOrEmpty(c.Email));

            if (Rutas.Where(c => string.IsNullOrEmpty(c.Email)).Count() > 0)
            {
                respuesta = "Existen conductores de ruta sin un email asignado";
            }

            if (rutasValidos.Count() > 0)
            {
                var correosDeServiciosPorRuta = CrearCorreosDeServiciosPorRuta(rutasValidos, servicios);
                _correoService.Enviar(correosDeServiciosPorRuta);
                respuesta = string.IsNullOrEmpty(respuesta) ? "" : ", " + "Notificacion enviada correctamente.";
            }

            return respuesta;
        }

        public string NotificarServiciosAConductor(ICollection<Servicio> servicios)
        {
            IEnumerable<Conductor> Conductores = servicios.DistinctBy(s => s.ConductorId).Select(c => c.Conductor);

            string respuesta = string.Empty;
            if (Conductores.Where(c => string.IsNullOrEmpty(c.Email)).Count()>0){
                respuesta = "Existen conductores sin un email asignado";
            }

            var conductoresValidos = Conductores.Where(c => !string.IsNullOrEmpty(c.Email));
            if (conductoresValidos.Count() > 0)
            {
                var correosDeServiciosPorConductor = CrearCorreosDeServiciosPorConductor(conductoresValidos, servicios);
                _correoService.Enviar(correosDeServiciosPorConductor);
                respuesta = string.IsNullOrEmpty(respuesta) ? "" : ", " + "Notificacion enviada correctamente.";
            }
            return respuesta;
        }

        private List<Correo> CrearCorreosDeServiciosPorConductor(IEnumerable<Conductor> conductores, ICollection<Servicio> servicios)
        {
            var correosDeServiciosPorConductor = new List<Correo>();
            foreach (var conductor in conductores)
            {
                string mensaje = "<p>Servicios asignados a: " + conductor.Nombre + "</p>";
                string destinatarios = string.Empty;
                var Servicios = servicios.Where(s => s.ConductorId == conductor.Id);
                destinatarios += string.Format("\"{0}\"<{1}>;", conductor.Nombre, conductor.Email);

                mensaje += ConstruirHtmlServicios(Servicios);

                var correo = new Correo()
                {
                    CORdestinatarios = destinatarios,
                    CORasunto = "Servicios de conductor elegido asignados",
                    CORmensajeHTML = mensaje
                };
                correosDeServiciosPorConductor.Add(correo);
            }
            return correosDeServiciosPorConductor;
        }

        private List<Correo> CrearCorreosDeServiciosPorRuta(IEnumerable<Conductor> rutas, ICollection<Servicio> servicios)
        {
            var correosDeServiciosPorConductor = new List<Correo>();
            foreach (var ruta in rutas)
            {
                string mensaje = "<p>Servicios de la ruta: " + ruta.Nombre + "</p>";
                string destinatarios = string.Empty;
                var Servicios = servicios.Where(s => s.RutaId == ruta.Id);
                destinatarios += string.Format("\"{0}\"<{1}>;", ruta.Nombre, ruta.Email);

                mensaje += ConstruirHtmlServicios(Servicios);

                var correo = new Correo()
                {
                    CORdestinatarios = destinatarios,
                    CORasunto = "Servicios asignados a su ruta",
                    CORmensajeHTML = mensaje
                };
                correosDeServiciosPorConductor.Add(correo);
            }
            return correosDeServiciosPorConductor;
        }

        private string ConstruirHtmlServicios(IEnumerable<Servicio> Servicios)
        {
            string mensaje = @"
                                <style>
                                    table {
                                        border-collapse: collapse;
                                        color:darkgray;
                                        font-size:12px;
                                        width:100%;
                                    }

                                    table, th, td {
                                        border: 1px solid lightgray;
                                        padding: 2px;
                                    }
                                </style>

                                <table>
                                    <thead>
                                        <tr>
                                            <th>Id</th>
                                            <th>Feha y hora</th>
                                            <th>Consecutivo</th>
                                            <th>Red</th>

                                            <th>Asegurado</th>
                                            <th>Vehiculo</th>

                                            <th>Origen</th>
                                            <th>Destino</th>
                                            <th>Conductor</th>
                                            <th>Ruta</th>
                                        </tr>
                                    </thead>
                                    <tbody>";

            foreach (var servicio in Servicios)
            {
                if (servicio.Ruta == null)
                    servicio.Ruta = new Conductor();
                if (servicio.Conductor == null)
                    servicio.Conductor = new Conductor();

                mensaje += string.Format(@"
                                        <tr>
                                            <td>{0}</td>
                                            <td>{1}</td>
                                            <td>{2}</td>
                                            <td>{3}</td>

                                            <td>{4}</td>
                                            <td>{5}</td>

                                            <td>{6}</td>
                                            <td>{7}</td>
                                            <td>{8}</td>
                                            <td>{9}</td>
                                        </tr>
                    ",
                    servicio.Id.ToString(),
                    servicio.Fecha.ToString("HH:mm dd/MM/yyyy"),
                    servicio.Radicado,
                    servicio.Aseguradora.Nombre,

                    servicio.Asegurado.Nombre +
                    (!string.IsNullOrEmpty(servicio.Asegurado.Telefono1) ? ", " + servicio.Asegurado.Telefono1 : "") +
                    (!string.IsNullOrEmpty(servicio.Asegurado.Telefono2) ? ", " + servicio.Asegurado.Telefono2 : ""),


                    servicio.Vehiculo.Placa +
                    (!string.IsNullOrEmpty(servicio.Vehiculo.Marca) ? ", " + servicio.Vehiculo.Marca : "") +
                    (!string.IsNullOrEmpty(servicio.Vehiculo.Referencia) ? ", " + servicio.Vehiculo.Referencia : "") +
                    (!string.IsNullOrEmpty(servicio.Vehiculo.Observaciones) ? ", " + servicio.Vehiculo.Observaciones : ""),


                    servicio.DireccionInicio.Descripcion +
                    (!string.IsNullOrEmpty(servicio.DireccionInicio.Barrio) ? ", " + servicio.DireccionInicio.Barrio : "") +
                    (!string.IsNullOrEmpty(servicio.DireccionInicio.Ciudad.Nombre) ? ", " + servicio.DireccionInicio.Ciudad.Nombre : ""),

                    servicio.DireccionDestino.Descripcion +
                    (!string.IsNullOrEmpty(servicio.DireccionDestino.Barrio) ? ", " + servicio.DireccionDestino.Barrio : "") +
                    (!string.IsNullOrEmpty(servicio.DireccionDestino.Ciudad.Nombre) ? ", " + servicio.DireccionDestino.Ciudad.Nombre : ""),

                    (servicio.Conductor != null) ? 
                    (!string.IsNullOrEmpty(servicio.Conductor.Nombre) ? servicio.Conductor.Nombre : "") +
                    (!string.IsNullOrEmpty(servicio.Conductor.Telefono1) ? ", " + servicio.Conductor.Telefono1 : ""):"",

                    (servicio.Ruta != null)?
                    (!string.IsNullOrEmpty(servicio.Ruta.Nombre) ? servicio.Ruta.Nombre : "") +
                    (!string.IsNullOrEmpty(servicio.Ruta.Telefono1) ? ", " + servicio.Ruta.Telefono1 : ""):""
                    );
            }
            return mensaje + "</tbody></table>";
        }

    }

    public partial interface IServicioService {
        List<Servicio> ObtenerParaSeguimiento(Periodo periodo);
        List<Servicio> Obtener(Periodo periodo);
        void CambioEstado(int idServicio, Estado nuevoEstado);
        string NotificarServiciosAConductor(ICollection<Servicio> servicios);
        string NotificarServiciosARuta(ICollection<Servicio> servicios);
        string ObtenerPorPeriodoCSV(Periodo periodo);
        
    }

    [DelimitedRecord(";")]
    public class serviciosModeloToExport
    {
        public string fecha { get; set; }
        public string hora { get; set; }
        public string aseguradora { get; set; }
        public string codigo { get; set; }
        public string asignadoPor { get; set; }
        public string asegurado { get; set; }
        public string vehiculo { get; set; }
        public string origen { get; set; }
        public string destino { get; set; }
        public string conductor { get; set; }
        public string ruta { get; set; }
        public string estado { get; set; }
    }
}
