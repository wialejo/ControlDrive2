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
using System.Web;
using Microsoft.AspNet.Identity;

namespace ControlDrive.API.Controllers
{
    public class SeguimientosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Seguimientos
        public IQueryable<Seguimiento> GetSeguimientos()
        {
            return db.Seguimientos;
        }

        // GET: api/Seguimientos/5
        [ResponseType(typeof(Seguimiento))]
        public IQueryable<Seguimiento> GetSeguimiento(int idServicio)
        {
            var seguimiento = db.Seguimientos.Where(s => s.ServicioId == idServicio);
            return seguimiento;
        }

        // PUT: api/Seguimientos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSeguimiento(int id, Seguimiento seguimiento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != seguimiento.Id)
            {
                return BadRequest();
            }

            db.Entry(seguimiento).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeguimientoExists(id))
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

        // POST: api/Seguimientos
        [ResponseType(typeof(Seguimiento))]
        public IHttpActionResult PostSeguimiento(Seguimiento seguimiento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            seguimiento.Fecha = DateTime.Now;

            seguimiento.UsuarioRegistroId = HttpContext.Current.User.Identity.GetUserId();

            if (db.Servicios.Find(seguimiento.ServicioId).EstadoCodigo != seguimiento.NuevoEstado)
            {
                seguimiento.Observacion = seguimiento.Observacion + "  -- Nuevo estado: " + db.Estados.Find(seguimiento.NuevoEstado).Descripcion;
                db.Servicios.Find(seguimiento.ServicioId).EstadoCodigo = seguimiento.NuevoEstado;
            }

            db.Seguimientos.Add(seguimiento);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = seguimiento.Id }, seguimiento);
        }

        // DELETE: api/Seguimientos/5
        [ResponseType(typeof(Seguimiento))]
        public IHttpActionResult DeleteSeguimiento(int id)
        {
            Seguimiento seguimiento = db.Seguimientos.Find(id);
            if (seguimiento == null)
            {
                return NotFound();
            }

            db.Seguimientos.Remove(seguimiento);
            db.SaveChanges();

            return Ok(seguimiento);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SeguimientoExists(int id)
        {
            return db.Seguimientos.Count(e => e.Id == id) > 0;
        }
    }
}