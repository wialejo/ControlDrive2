using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ControlDrive.API.Models;
using System.Diagnostics;
using ControlDrive.CORE.Modelos;
using ControlDrive.CORE.Infraestructura;
using ControlDrive.CORE.Servicios;

namespace ControlDrive.API.Controllers
{
    public class ServiciosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ServiciosController()
        {
            //db.Database.Log = s => Debug.Write(s);
        }
        //GET: api/Servicios
        public IQueryable<Servicio> GetServicios()
        {
            var servicios = db.Servicios.OrderBy(s => s.Fecha);
            //var servicios = from s in db.Servicios
            //                join c in db.Conductores on s.ConductorId equals c.Id
            //                join a in db.Aseguradoras on s.AseguradoraId equals a.Id
            //                join ase in db.Asegurados on s.AseguradoId equals ase.Id
            //                join v in db.Vehiculos on s.VehiculoId equals v.Id
            //                select s;

            return servicios;
        }
        
        [Route("api/servicios/ServiciosByPeriodo")]
        [HttpPost]
        public IQueryable<Servicio> GetServiciosByPeriodo([FromBody]Periodo Fecha)
        {
            Periodo periodo = ObtenerPeriodo(Convert.ToDateTime(Fecha.Inicio));

            var servicios = (from s in db.Servicios
                            where s.Fecha > periodo.Inicio && s.Fecha < periodo.Fin && s.Estado.Codigo != "AN"
                            select s).OrderBy(s => s.Fecha);
            return servicios;
        }

        private Periodo ObtenerPeriodo(DateTime Fecha)
        {
            Periodo periodo = new Periodo();
            TimeSpan ti = new TimeSpan(18, 0, 0);
            periodo.Inicio = Fecha.Date + ti ;
            
            TimeSpan tf = new TimeSpan(17, 59, 0);
            periodo.Fin = Fecha.Date.AddDays(1) + tf;
            return periodo;
        }

        //GET: api/Servicios/5
        [ResponseType(typeof(Servicio))]
        public IHttpActionResult GetServicios(int id)
        {
            Servicio servicios = db.Servicios.Find(id);
            if (servicios == null)
            {
                return NotFound();
            }

            return Ok(servicios);
        }

        //PUT: api/Servicios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutServicios(int id, Servicio servicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != servicio.Id)
            {
                return BadRequest();
            }

            if (servicio.Conductor != null)
            {
                servicio.ConductorId = servicio.Conductor.Id;
                servicio.Conductor = null;
            }

            if (servicio.Ruta != null)
            {
                servicio.RutaId = servicio.Ruta.Id;
                servicio.Ruta = null;
            }

            servicio.Aseguradora = null;
            servicio.Seguimientos = null;


            servicio.DireccionInicio.CiudadId = servicio.DireccionInicio.Ciudad.Id;
            servicio.DireccionDestino.CiudadId = servicio.DireccionDestino.Ciudad.Id;
            servicio.DireccionInicio.Ciudad = null;
            servicio.DireccionDestino.Ciudad = null;
            servicio.Vehiculo.Placa = servicio.Vehiculo.Placa.ToUpper();
            servicio.FechaRegistro = DateTime.Now;

            db.Entry(servicio.Vehiculo).State = servicio.Vehiculo.Id == 0 ? EntityState.Added : EntityState.Modified;
            db.Entry(servicio.Asegurado).State = servicio.Asegurado.Id == 0 ? EntityState.Added : EntityState.Modified;
            db.Entry(servicio.DireccionInicio).State = EntityState.Modified;
            db.Entry(servicio.DireccionDestino).State = EntityState.Modified;
            db.Entry(servicio).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiciosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        //POST: api/Servicios
        [ResponseType(typeof(Servicio))]
        public IHttpActionResult PostServicios(Servicio servicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (servicio.AseguradoraId == 0) {
            //   var NuevaAseguradora = db.Aseguradoras.Add(servicio.Aseguradora);
            //   servicio.AseguradoraId = NuevaAseguradora.Id;
            //}

            if (servicio.Conductor != null)
            {
                servicio.ConductorId = servicio.Conductor.Id;
                servicio.Conductor = null;
            }

            if (servicio.Ruta != null)
            {
                servicio.RutaId = servicio.Ruta.Id;
                servicio.Ruta = null;
            }


            if (servicio.AseguradoId == 0)
            {
                var NuevoAsegurado = db.Asegurados.Add(servicio.Asegurado);
                servicio.AseguradoId = NuevoAsegurado.Id;
            }
            servicio.Vehiculo.Placa = servicio.Vehiculo.Placa.ToUpper();
            servicio.FechaRegistro = DateTime.Now;

            servicio.DireccionInicio.CiudadId = servicio.DireccionInicio.Ciudad.Id;
            servicio.DireccionDestino.CiudadId = servicio.DireccionDestino.Ciudad.Id;
            servicio.DireccionInicio.Ciudad = null;
            servicio.DireccionDestino.Ciudad = null;
            db.Servicios.Add(servicio);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = servicio.Id }, servicio);
        }

        //POST: api/Servicios
        //[ResponseType(typeof(Servicio))]
        [Route("api/servicios/EnviarCorreoSeguimiento")]
        [HttpPost]
        public IHttpActionResult EnviarCorreoSeguimiento([FromBody]ICollection<Servicio> servicios)
        {
            IEnumerable<Conductor> Conductores = servicios.DistinctBy(s => s.ConductorId).Select(c => c.Conductor);
            var correosDeServiciosPorConductor = CrearCorreosDeServiciosPorConductor(Conductores, servicios);
            EnviarCorreos(correosDeServiciosPorConductor);

            return Ok();
        }

        //POST: api/Servicios
        //[ResponseType(typeof(Servicio))]
        [Route("api/servicios/EnviarCorreoRutaSeguimiento")]
        [HttpPost]
        public IHttpActionResult EnviarCorreoRutaSeguimiento([FromBody]ICollection<Servicio> servicios)
        {
            IEnumerable<Conductor> Rutas = servicios.DistinctBy(s => s.RutaId).Select(c => c.Ruta);

            var correosDeServiciosPorRuta = CrearCorreosDeServiciosPorRuta(Rutas, servicios);
            EnviarCorreos(correosDeServiciosPorRuta);

            return Ok();
        }


        private void EnviarCorreos(List<Correo> correos)
        {
            var cuenta = db.Cuentas.FirstOrDefault();
            var SmtpClient = Smtp.IniciarSmtpClient(cuenta);
            Smtp.EnviarMensajesAClienteSMTP(correos, cuenta, SmtpClient);
        }

        private List<Correo> CrearCorreosDeServiciosPorConductor(IEnumerable<Conductor> conductores, ICollection<Servicio> servicios)
        {
            var correosDeServiciosPorConductor = new List<Correo>();
            foreach (var conductor in conductores)
            {
                string mensaje = "<p>Servicios asignados a: "+ conductor.Nombre +"</p>";
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
                string mensaje = "<p>Servicios de la ruta: "+ ruta.Nombre +"</p>";
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
                                            <th>Aseguradora</th>

                                            <th>Asegurado</th>
                                            <th>Vehiculo</th>

                                            <th>Origen/destino</th>
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


                    "<strong>Inicio: </strong>" + servicio.DireccionInicio.Descripcion +
                    (!string.IsNullOrEmpty(servicio.DireccionInicio.Barrio) ? ", " + servicio.DireccionInicio.Barrio: "") +
                    (!string.IsNullOrEmpty(servicio.DireccionInicio.Ciudad.Nombre) ? ", " + servicio.DireccionInicio.Ciudad.Nombre : "") +

                    "<strong><br/>Destino: </strong>" + servicio.DireccionDestino.Descripcion +
                    (!string.IsNullOrEmpty(servicio.DireccionDestino.Barrio) ? ", " + servicio.DireccionDestino.Barrio : "") +
                    (!string.IsNullOrEmpty(servicio.DireccionDestino.Ciudad.Nombre) ? ", " + servicio.DireccionDestino.Ciudad.Nombre : ""),

                    (!string.IsNullOrEmpty(servicio.Conductor.Nombre) ? servicio.Conductor.Nombre : "") +  
                    (!string.IsNullOrEmpty(servicio.Conductor.Telefono1) ? ", " + servicio.Conductor.Telefono1 : ""),
                    (!string.IsNullOrEmpty(servicio.Ruta.Nombre) ? servicio.Ruta.Nombre : "")+
                    (!string.IsNullOrEmpty(servicio.Ruta.Telefono1) ? ", " + servicio.Ruta.Telefono1 : "")
                    );
            }
            return mensaje + "</tbody></table>";
        }




        //DELETE: api/Servicios/5
        [ResponseType(typeof(Servicio))]
        public IHttpActionResult DeleteServicios(int id)
        {
            Servicio servicios = db.Servicios.Find(id);
            if (servicios == null)
            {
                return NotFound();
            }

            db.Servicios.Remove(servicios);
            db.SaveChanges();

            return Ok(servicios);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ServiciosExists(int id)
        {
            return db.Servicios.Count(e => e.Id == id)>0;
        }
    }
}