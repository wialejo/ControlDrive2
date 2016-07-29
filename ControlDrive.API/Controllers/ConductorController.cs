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
    [Authorize]
    public class ConductoresController : BaseController
    {
        private readonly IConductorService _conductorSevice;
        private readonly IMovimientosService _movimientosService;

        public ConductoresController(IConductorService conductorSevice, IMovimientosService movimientosService)
        {
            _movimientosService = movimientosService;
            _conductorSevice = conductorSevice;
        }

        [HttpGet]
        public IHttpActionResult Obtener()
        {
            var conductores = _conductorSevice.Obtener().Where(c => c.SucursalId == IdSucursal);
            return Ok(conductores);
        }

        [HttpGet]
        [Route("api/conductores/movimientos")]
        public IHttpActionResult ObtenerMovimientos([FromUri]DateTime inicio, [FromUri]DateTime fin, int proveedorId)
        {
            var servicios = _movimientosService
                .Obtener(m => m.Servicio.ConductorId == proveedorId && 
                                    DbFunctions.TruncateTime(m.Servicio.Fecha) >= DbFunctions.TruncateTime(inicio) 
                                    && DbFunctions.TruncateTime(m.Servicio.Fecha) <= DbFunctions.TruncateTime(fin)
                        );

            return Ok(servicios);
        }

        [HttpGet]
        public IHttpActionResult Obtener(int id)
        {
            var conductor = _conductorSevice.Obtener()
                .Where(c => c.Id == id && c.SucursalId == IdSucursal);
            return Ok(conductor);
        }

        [HttpGet]
        public IHttpActionResult ObtenerPorDescripcion(string id)
        {
            var conductores = _conductorSevice.Obtener()
                .Where(c => c.Nombre.ToLower().Contains(id.ToLower()) && c.SucursalId == IdSucursal)
                .Select(c => new { c.Id, c.Nombre })
                .Take(10);

            return Ok(conductores);
        }

        // POST: api/Conductores
        [HttpPost]
        public IHttpActionResult Guardar(Conductor conductor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            conductor.SucursalId = IdSucursal;
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