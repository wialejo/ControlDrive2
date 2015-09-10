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

namespace ControlDrive.API.Controllers
{
    public class EstadosController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Estados
        public IQueryable<Estado> GetEstados()
        {
            return db.Estados;
        }

        // GET: api/Estados/5
        [ResponseType(typeof(Estado))]
        public IHttpActionResult GetEstado(string id)
        {
            Estado estado = db.Estados.Find(id);
            if (estado == null)
            {
                return NotFound();
            }

            return Ok(estado);
        }

        // PUT: api/Estados/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEstado(string id, Estado estado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != estado.Codigo)
            {
                return BadRequest();
            }

            db.Entry(estado).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoExists(id))
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

        // POST: api/Estados
        [ResponseType(typeof(Estado))]
        public IHttpActionResult PostEstado(Estado estado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Estados.Add(estado);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EstadoExists(estado.Codigo))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = estado.Codigo }, estado);
        }

        // DELETE: api/Estados/5
        [ResponseType(typeof(Estado))]
        public IHttpActionResult DeleteEstado(string id)
        {
            Estado estado = db.Estados.Find(id);
            if (estado == null)
            {
                return NotFound();
            }

            db.Estados.Remove(estado);
            db.SaveChanges();

            return Ok(estado);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EstadoExists(string id)
        {
            return db.Estados.Count(e => e.Codigo == id) > 0;
        }
    }
}