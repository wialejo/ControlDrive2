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
    public class ConductoresController : ApiController
    {
        private GestionServiciosContext db = new GestionServiciosContext();

        // GET: api/Conductores

        [Authorize]
        public IQueryable<Conductor> GetConductores()
        {
            return db.Conductores;
        }

        // GET: api/Conductores/5
        [ResponseType(typeof(Conductor))]
        public IHttpActionResult GetConductor(int id)
        {
            Conductor conductor = db.Conductores.Find(id);
            if (conductor == null)
            {
                return NotFound();
            }

            return Ok(conductor);
        }

        // PUT: api/Conductores/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutConductor(int id, Conductor conductor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != conductor.Id)
            {
                return BadRequest();
            }

            db.Entry(conductor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConductorExists(id))
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

        // POST: api/Conductores
        [ResponseType(typeof(Conductor))]
        public IHttpActionResult PostConductor(Conductor conductor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Conductores.Add(conductor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = conductor.Id }, conductor);
        }

        // DELETE: api/Conductores/5
        [ResponseType(typeof(Conductor))]
        public IHttpActionResult DeleteConductor(int id)
        {
            Conductor conductor = db.Conductores.Find(id);
            if (conductor == null)
            {
                return NotFound();
            }

            db.Conductores.Remove(conductor);
            db.SaveChanges();

            return Ok(conductor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ConductorExists(int id)
        {
            return db.Conductores.Count(e => e.Id == id) > 0;
        }
    }
}