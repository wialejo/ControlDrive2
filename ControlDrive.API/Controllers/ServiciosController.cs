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
            var servicios = db.Servicios;
            //var servicios = from s in db.Servicios
            //                join c in db.Conductores on s.ConductorId equals c.Id
            //                join a in db.Aseguradoras on s.AseguradoraId equals a.Id
            //                join ase in db.Asegurados on s.AseguradoId equals ase.Id
            //                join v in db.Vehiculos on s.VehiculoId equals v.Id
            //                select s;

            return servicios;
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

            servicio.ConductorId = servicio.Conductor.Id;
            db.Entry(servicio.Conductor).State = servicio.Conductor.Id == 0 ? EntityState.Added : EntityState.Modified;
            
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
            servicio.ConductorId = servicio.Conductor.Id;
            db.Entry(servicio.Conductor).State = servicio.Conductor.Id == 0 ? EntityState.Added : EntityState.Modified;

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
            var correos = new List<Correo>();
            var cuenta = db.Cuentas.FirstOrDefault();

            foreach (var conductor in Conductores)
            {
                string mensaje = "<p>Servicios asignados:</p>";
                string destinatarios = string.Empty;
                var Servicios = servicios.Where(s => s.ConductorId == conductor.Id);
                destinatarios += string.Format("\"{0}\"<{1}>;", conductor.Nombre, conductor.Email);

                foreach (var servicio in Servicios)
                {
                    mensaje += string.Format(@"
                            <div style='margin:10px'>
                            <hr>
                            <ul>
                                <li><span style='font-weight:bold;'>Id: </span><span>{15}</span></li>
                                <li><span style='font-weight:bold;'>Fecha y Hora: </span><span>{0}</span></li>
                                <li><span style='font-weight:bold;'>Consecutivo: </span><span>{1}</span></li>
                                <li><span style='font-weight:bold;'>Aseguradora: </span><span>{2}</span></li>
                                <br>                                                     
                                <li><span style='font-weight:bold;'>Asegurado: </span><span>{3} Tel: {4} - {5}</span></li>
                                <li><span style='font-weight:bold;'>Vehiculo: </span><span>{6} - {7} - {8}</span></li>
                                <br>
                                <li><span style='font-weight:bold;'>Dirección inicio: </span><span>{9}, {10}, {11}</span></li>
                                <li><span style='font-weight:bold;'>Dirección destino: </span><span>{12}, {13}, {14}</span></li>
                            </ul>
                            </div>
                    ", servicio.Fecha.ToString(), servicio.Radicado, servicio.Aseguradora.Nombre,
                        servicio.Asegurado.Nombre, servicio.Asegurado.Telefono1, servicio.Asegurado.Telefono2,
                        servicio.Vehiculo.Placa, servicio.Vehiculo.Marca, servicio.Vehiculo.Observaciones,
                        servicio.DireccionInicio.Descripcion, servicio.DireccionInicio.Barrio, servicio.DireccionInicio.Ciudad.Nombre,
                        servicio.DireccionDestino.Descripcion, servicio.DireccionDestino.Barrio, servicio.DireccionDestino.Ciudad.Nombre,
                        servicio.Id.ToString()
                        );
                }
                var correo = new Correo()
                {
                    CORdestinatarios = destinatarios,
                    CuentaId = cuenta.Id,
                    CORasunto = "Servicios de conductor elegido asignados",
                    CORmensajeHTML = mensaje
                };
                correos.Add(correo);
            }
            var SmtpClient = Smtp.IniciarSmtpClient(cuenta);
            Smtp.EnviarMensajesAClienteSMTP(correos, cuenta, SmtpClient);
            return Ok();
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