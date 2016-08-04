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
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace ControlDrive.Core.Controllers
{
    //[Authorize]
    public class MovimientosController : ApiController
    {
        private readonly IMovimientosService _movimientosService;
        private readonly ISucursalService _sucursalServiceExt;

        public MovimientosController(IMovimientosService movimientosService, ISucursalService sucursalServiceExt)
        {
            _movimientosService = movimientosService;
            _sucursalServiceExt = sucursalServiceExt;
        }

        [HttpGet]
        [Route("api/movimientos/cliente")]
        public IHttpActionResult ObtenerMovimientosCliente([FromUri]DateTime inicio, [FromUri]DateTime fin, int clienteId)
        {
            try
            {
                var idUsuario = HttpContext.Current.User.Identity.GetUserId();
                var sucursales = _sucursalServiceExt.ObtenerPorUsuario(idUsuario);

                var movimientos = from s in sucursales.AsQueryable()
                                  join m in _movimientosService
                                          .Obtener(
                                                  m => DbFunctions.TruncateTime(m.Servicio.Fecha) >= DbFunctions.TruncateTime(inicio)
                                                  && DbFunctions.TruncateTime(m.Servicio.Fecha) <= DbFunctions.TruncateTime(fin)
                                                  && (m.Servicio.EstadoCodigo == "CF" || m.Servicio.EstadoCodigo == "CR")
                                                  && m.Servicio.AseguradoraId == clienteId
                                                  && m.Concepto.TipoConcepto == TipoConcepto.Cliente
                                                  && m.DocumentoId == null).AsQueryable() on s.Id equals m.Servicio.SucursalId
                                  select m;


                return Ok(movimientos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<HttpResponseMessage> ObtenerPorPeriodoCSV(Periodo periodo)

        [HttpPost]
        [AllowAnonymous]
        [Route("api/movimientos/clienteCsv")]
        public async Task<HttpResponseMessage> ObtenerMovimientosClienteCSV(paraCsv data)
        {
            var idUsuario = HttpContext.Current.User.Identity.GetUserId();
            var sucursales = _sucursalServiceExt.ObtenerPorUsuario(idUsuario);

            var movimientos = from s in sucursales.AsQueryable()
                              join m in _movimientosService
                            .Obtener(
                                m => DbFunctions.TruncateTime(m.Servicio.Fecha) >= DbFunctions.TruncateTime(data.inicio)
                                && DbFunctions.TruncateTime(m.Servicio.Fecha) <= DbFunctions.TruncateTime(data.fin)
                                && (m.Servicio.EstadoCodigo == "CF" || m.Servicio.EstadoCodigo == "CR")
                                && m.Servicio.AseguradoraId == data.clienteId
                                && m.Concepto.TipoConcepto == TipoConcepto.Cliente
                                && m.DocumentoId == null).AsQueryable() on s.Id equals m.Servicio.SucursalId
                              select m;

            byte[] output = null;
            await Task.Run(() =>
            {
                using (var stream = _movimientosService.ObtenerResumenMovimientosEnCSV(movimientos.ToList()))
                {
                    stream.Flush();
                    output = stream.ToArray();
                }
            });

            if (output != null)
            {
                var result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(output) };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "prefacturacion.xls"
                };
                result.Content.Headers.Add("x-filename", "prefacturacion.xls");
                return result;
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NoContent, "No hay datos.");
        }

        [HttpGet]
        [Route("api/movimientos/cliente")]
        public IHttpActionResult ObtenerMovimientosCliente([FromUri]DateTime inicio, [FromUri]DateTime fin, int clienteId, bool aprobado)
        {
            try
            {
                var idUsuario = HttpContext.Current.User.Identity.GetUserId();
                var sucursales = _sucursalServiceExt.ObtenerPorUsuario(idUsuario);

                var movimientos = from s in sucursales.AsQueryable()
                                  join m in _movimientosService
                    .Obtener(
                        m => DbFunctions.TruncateTime(m.Servicio.Fecha) >= DbFunctions.TruncateTime(inicio)
                        && DbFunctions.TruncateTime(m.Servicio.Fecha) <= DbFunctions.TruncateTime(fin)
                        && (m.Servicio.EstadoCodigo == "CF" || m.Servicio.EstadoCodigo == "CR")
                        && m.Servicio.AseguradoraId == clienteId
                        && m.Concepto.TipoConcepto == TipoConcepto.Cliente
                        && m.Aprobado == aprobado
                        && m.DocumentoId == null).AsQueryable() on s.Id equals m.Servicio.SucursalId
                                  select m;
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

                var idUsuario = HttpContext.Current.User.Identity.GetUserId();
                var sucursales = _sucursalServiceExt.ObtenerPorUsuario(idUsuario);

                var movimientos = from s in sucursales.AsQueryable()
                                  join m in _movimientosService.Obtener(
                                                m => DbFunctions.TruncateTime(m.Servicio.Fecha) >= DbFunctions.TruncateTime(inicio)
                                                && DbFunctions.TruncateTime(m.Servicio.Fecha) <= DbFunctions.TruncateTime(fin)
                                                && (m.Servicio.EstadoCodigo == "CF" || m.Servicio.EstadoCodigo == "CR")
                                                && (m.ProveedorId == proveedorId)
                                                && m.Concepto.TipoConcepto == TipoConcepto.Proveedor
                                                && m.DocumentoId == null).AsQueryable() on s.Id equals m.Servicio.SucursalId
                                  select m;

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
    public class paraCsv
    {
        public DateTime inicio { get; set; }
        public DateTime fin { get; set; }
        public int clienteId { get; set; }
    }
}