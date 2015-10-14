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

namespace ControlDrive.API.Controllers
{
    public class ServiciosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ServiciosController()
        {
            db.Database.Log = s => Debug.Write(s);
        }
        // GET: api/Servicios
        public IQueryable<Servicio> GetServicios()
        {
            var servicios = db.Servicios.Include(c => c.Conductor);
            return servicios;
        }

        // GET: api/Servicios/5
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

        // PUT: api/Servicios/5
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

        // POST: api/Servicios
        [ResponseType(typeof(Servicio))]
        public IHttpActionResult PostServicios(Servicio servicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (servicio.AseguradoraId == 0) {
            //    var NuevaAseguradora = db.Aseguradoras.Add(servicio.Aseguradora);
            //    servicio.AseguradoraId = NuevaAseguradora.Id;
            //}
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

        // DELETE: api/Servicios/5
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
            return db.Servicios.Count(e => e.Id == id) > 0;
        }
    }
}