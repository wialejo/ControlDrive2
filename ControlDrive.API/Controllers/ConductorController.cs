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
using ControlDrive.Core.Modelos;
using ControlDrive.CORE.Modelos;
using ControlDrive.CORE.Services;

namespace ControlDrive.Core.Controllers
{
    public class ConductorController : ApiController
    {
        private readonly ICommonInterface<Conductor> _conductorSevice;

        public ConductorController(ICommonInterface<Conductor> conductorSevice)
        {
            _conductorSevice = conductorSevice;
        }

        [HttpGet]
        public IHttpActionResult Obtener()
        {
            var conductores  = _conductorSevice.Obtener();
            return Ok(conductores);
        }

        [HttpGet]
        public IHttpActionResult Obtener(int id)
        {
            var conductor  = _conductorSevice.ObtenerPorId(id);
            return Ok(conductor);
        }
        
        // POST: api/Conductores
        [HttpPost]
        public IHttpActionResult Guardar(Conductor conductor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var conductorRepo = _conductorSevice.Guardar(conductor);

            return Ok(conductorRepo);
        }

        // DELETE: api/Conductores/5
        [HttpDelete]
        public IHttpActionResult Eliminar(int id)
        {
            _conductorSevice.Eliminar(id);

            return Ok();
        }
    }
}