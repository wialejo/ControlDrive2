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
    //[Authorize]
    public class CiudadesController : ApiController
    {
        private readonly ICommonInterface<Ciudad> _ciudadSevice;

        public CiudadesController(ICommonInterface<Ciudad> ciudadSevice)
        {
            _ciudadSevice = ciudadSevice;
        }

        [HttpGet]
        public IHttpActionResult Obtener()
        {
            var ciudades  = _ciudadSevice.Obtener();
            return Ok(ciudades);
        }

        [HttpGet]
        public IHttpActionResult Obtener(int id)
        {
            var ciudad  = _ciudadSevice.ObtenerPorId(id);
            return Ok(ciudad);
        }
        
        // POST: api/Ciudades
        [HttpPost]
        public IHttpActionResult Guardar(Ciudad ciudad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ciudadRepo = _ciudadSevice.Guardar(ciudad);

            return Ok(ciudadRepo);
        }

        // DELETE: api/Ciudades/5
        [HttpDelete]
        public IHttpActionResult DeleteCiudad(int id)
        {
            _ciudadSevice.Eliminar(id);

            return Ok();
        }
    }
}