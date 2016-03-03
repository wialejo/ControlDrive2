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
    public class AseguradoraController : ApiController
    {
        private readonly ICommonInterface<Aseguradora> _aseguradoraSevice;

        public AseguradoraController(ICommonInterface<Aseguradora> aseguradoraSevice)
        {
            _aseguradoraSevice = aseguradoraSevice;
        }

        [HttpGet]
        public IHttpActionResult Obtener()
        {
            var aseguradoras  = _aseguradoraSevice.Obtener();
            return Ok(aseguradoras);
        }

        [HttpGet]
        public IHttpActionResult Obtener(int id)
        {
            var aseguradora  = _aseguradoraSevice.ObtenerPorId(id);
            return Ok(aseguradora);
        }
        
        // POST: api/Aseguradoraes
        [HttpPost]
        public IHttpActionResult Guardar(Aseguradora aseguradora)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var aseguradoraRepo = _aseguradoraSevice.Guardar(aseguradora);

            return Ok(aseguradoraRepo);
        }

        // DELETE: api/Aseguradoraes/5
        [HttpDelete]
        public IHttpActionResult Eliminar(int id)
        {
            _aseguradoraSevice.Eliminar(id);

            return Ok();
        }
    }
}