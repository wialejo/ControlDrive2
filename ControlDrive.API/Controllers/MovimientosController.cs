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
using Microsoft.AspNet.Identity;
using System.Web;
using ControlDrive.CORE.Enums;

namespace ControlDrive.Core.Controllers
{
    [Authorize]
    public class MovimientosController : ApiController
    {
        private readonly IMovimientosService _movimientosService;

        public MovimientosController(IMovimientosService movimientosService)
        {
            _movimientosService = movimientosService;
        }

        [HttpGet]
        [Route("api/movimientos/cliente")]
        public IHttpActionResult ObtenerMovimientosCliente([FromUri]DateTime inicio, [FromUri]DateTime fin, int clienteId)
        {
            try
            {
                var movimientos = _movimientosService
                    .Obtener(
                        m => DbFunctions.TruncateTime(m.Servicio.Fecha) >= DbFunctions.TruncateTime(inicio)
                        && DbFunctions.TruncateTime(m.Servicio.Fecha) <= DbFunctions.TruncateTime(fin)
                        && (m.Servicio.EstadoCodigo == "CF" || m.Servicio.EstadoCodigo == "CR")
                        && m.Servicio.AseguradoraId == clienteId
                        && m.Concepto.TipoConcepto == TipoConcepto.Cliente
                        && m.DocumentoId == null)
                    .ToList();
                return Ok(movimientos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/movimientos/cliente")]
        public IHttpActionResult ObtenerMovimientosCliente([FromUri]DateTime inicio, [FromUri]DateTime fin, int clienteId, bool aprobado)
        {
            try
            {
                var movimientos = _movimientosService
                    .Obtener(
                        m => DbFunctions.TruncateTime(m.Servicio.Fecha) >= DbFunctions.TruncateTime(inicio)
                        && DbFunctions.TruncateTime(m.Servicio.Fecha) <= DbFunctions.TruncateTime(fin)
                        && (m.Servicio.EstadoCodigo == "CF" || m.Servicio.EstadoCodigo == "CR")
                        && m.Servicio.AseguradoraId == clienteId
                        && m.Concepto.TipoConcepto == TipoConcepto.Cliente
                        && m.Aprobado == aprobado
                        && m.DocumentoId == null)
                    .ToList();
                return Ok(movimientos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/movimientos/proveedor")]
        public IHttpActionResult ObtenerMovimientosProveedores([FromUri]DateTime inicio, [FromUri]DateTime fin, int proveedorId)
        {
            try
            {
                var movimientos = _movimientosService
                    .Obtener(
                        m => DbFunctions.TruncateTime(m.Servicio.Fecha) >= DbFunctions.TruncateTime(inicio)
                        && DbFunctions.TruncateTime(m.Servicio.Fecha) <= DbFunctions.TruncateTime(fin)
                        && (m.Servicio.EstadoCodigo == "CF" || m.Servicio.EstadoCodigo == "CR")
                        && (
                            (m.ConceptoCodigo == "PROVE_COND_ELE" &&  m.Servicio.ConductorId == proveedorId)
                            ||
                            (m.ConceptoCodigo == "PROVE_RUTA_COND_ELE" && m.Servicio.RutaId == proveedorId))
                        && m.DocumentoId == null)
                    .ToList();
                return Ok(movimientos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/movimientos/{id}")]
        public IHttpActionResult ObtenerPorId([FromUri] int id)
        {
            try
            {
                var movimiento = _movimientosService
                    .Obtener(m => m.Id == id)
                    .SingleOrDefault();
                return Ok(movimiento);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        [Route("api/movimientos")]
        public IHttpActionResult Guardar([FromBody]Movimiento movimiento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                if (movimiento.Id == 0)
                {
                    movimiento.UsuarioRegistroId = HttpContext.Current.User.Identity.GetUserId();
                    movimiento.FechaRegistro = DateTime.Now;
                }
                else
                {
                    movimiento.UsuarioModificacionId = HttpContext.Current.User.Identity.GetUserId();
                    movimiento.FechaModificacion = DateTime.Now;
                }
                _movimientosService.Guardar(movimiento);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("api/movimientos")]
        public IHttpActionResult Actualizar([FromBody]Movimiento movimiento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                if (movimiento.Id == 0)
                {
                    movimiento.UsuarioRegistroId = HttpContext.Current.User.Identity.GetUserId();
                    movimiento.FechaRegistro = DateTime.Now;
                }
                else
                {
                    movimiento.UsuarioModificacionId = HttpContext.Current.User.Identity.GetUserId();
                    movimiento.FechaModificacion = DateTime.Now;
                }
                _movimientosService.ActualizarParaCierreFacuracion(movimiento);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}