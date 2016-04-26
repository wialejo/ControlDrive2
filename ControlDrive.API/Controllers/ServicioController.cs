﻿using System;
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
using System.Diagnostics;
using ControlDrive.CORE.Modelos;
using ControlDrive.CORE.Infraestructura;
using ControlDrive.CORE.Services;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.AspNet.Identity;

namespace ControlDrive.Core.Controllers
{
    [Authorize]
    public class ServicioController : ApiController
    {
        private readonly ICommonInterface<Servicio> _servicioService;
        private IServicioService _servicioServiceExt;
        private ICommonInterface<Seguimiento> _seguimientoService;

        public ServicioController(ICommonInterface<Servicio> servicioService, ICommonInterface<Seguimiento> seguimientoService, IServicioService servicioServiceExt)
        {
            _servicioService = servicioService;
            _servicioServiceExt = servicioServiceExt;
            _seguimientoService = seguimientoService;
        }

        [HttpGet]
        [Route("api/servicios/rango")]
        public IHttpActionResult Obtener([FromUri]DateTime inicio, [FromUri]DateTime fin)
        {
            var servicios = _servicioServiceExt.Obtener(s => DbFunctions.TruncateTime(s.Fecha) >= DbFunctions.TruncateTime(inicio) && DbFunctions.TruncateTime(s.Fecha) <= DbFunctions.TruncateTime(fin)).ToList();
            return Ok(servicios);
        }

        [HttpGet]
        [Route("api/servicios/seguimiento")]
        public IHttpActionResult ObtenerServicios([FromUri]DateTime inicioPeriodo)
        {
            var servicios = new List<ServicioDto>();
            var periodo = new PeriodoService().Obtener(inicioPeriodo);
            servicios = _servicioServiceExt.Obtener(s => s.Fecha > periodo.Inicio && s.Fecha < periodo.Fin & s.EstadoCodigo != "AN").ToList();
            return Ok(servicios);
        }

        [HttpGet]
        [Route("api/servicios/cierre")]
        public IHttpActionResult ObtenerCierre([FromUri]DateTime inicioPeriodo)
        {
            var servicios = new List<ServicioDto>();
            var periodo = new PeriodoService().Obtener(inicioPeriodo);
            servicios = _servicioServiceExt
                .Obtener(s => s.Fecha > periodo.Inicio && s.Fecha < periodo.Fin & (s.EstadoCodigo == "FL" || s.EstadoCodigo == "CN" || s.EstadoCodigo == "TE"))
                .ToList();
            return Ok(servicios);
        }

        [HttpGet]
        [Route("api/servicios/facturacion/rango")]
        public IHttpActionResult ObtenerFacturacion([FromUri]DateTime inicio, [FromUri]DateTime fin)
        {
            var servicios = new List<ServicioDto>();
            servicios = _servicioServiceExt
                            .Obtener(s => DbFunctions.TruncateTime(s.Fecha) >= DbFunctions.TruncateTime(inicio)
                                    && DbFunctions.TruncateTime(s.Fecha) <= DbFunctions.TruncateTime(fin)
                                    && (s.EstadoCodigo == "CR" || s.EstadoCodigo == "CF"))
                             .ToList();

            return Ok(servicios);
        }

        [HttpGet]
        public IHttpActionResult ObtenerPorId(int id)
        {
            var servicio = _servicioServiceExt.ObtenerPorId(id);
            return Ok(servicio);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> ObtenerPorPeriodoCSV(Periodo periodo)
        {
            var periodoCompleto = new PeriodoService().Obtener(periodo.Inicio);

            byte[] output = null;
            await Task.Run(() =>
            {
                using (var stream = _servicioServiceExt.GenerarExcelServiciosResumen(periodoCompleto))
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
                    FileName = "servicios.xls"
                };
                result.Content.Headers.Add("x-filename", "servicios.xls");
                return result;
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NoContent, "No hay datos.");
        }

        [HttpPost]
        public IHttpActionResult Guardar(Servicio servicio)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (servicio.Id == 0)
            {
                servicio.UsuarioRegistroId = HttpContext.Current.User.Identity.GetUserId();
            }
            else
            {
                servicio.UsuarioModificacionId = HttpContext.Current.User.Identity.GetUserId();
            }

            var servicioId = _servicioService.Guardar(servicio).Id;
            var servicioRepo = _servicioServiceExt.ObtenerPorId(servicioId);

            return Ok(servicioRepo);
        }

        [HttpPost]
        public IHttpActionResult NotificarServiciosAConductor([FromBody]ICollection<Servicio> servicios)
        {
            var respuesta = _servicioServiceExt.NotificarServiciosAConductor(servicios);
            return Ok(respuesta);
        }

        [HttpPost]
        public IHttpActionResult ObtenerHtmlServiciosAConductor([FromBody]ICollection<Servicio> servicios)
        {
            var respuesta = _servicioServiceExt.ObtenerHtmlServiciosAConductor(servicios);
            return Ok(respuesta);
        }

        [HttpPost]
        public IHttpActionResult NotificarServiciosARuta([FromBody]ICollection<Servicio> servicios)
        {
            var respuesta = _servicioServiceExt.NotificarServiciosARuta(servicios);
            return Ok(respuesta);
        }

        [HttpPost]
        public IHttpActionResult ObtenerHtmlServiciosARuta([FromBody]ICollection<Servicio> servicios)
        {
            var respuesta = _servicioServiceExt.ObtenerHtmlServiciosARuta(servicios);
            return Ok(respuesta);
        }


        [HttpPut]
        [Route("api/servicio/{servicioId}/cerrar")]
        public IHttpActionResult Cerrar(int servicioId, Valor valores)
        {
            var usuarioId = HttpContext.Current.User.Identity.GetUserId();
            _servicioServiceExt.Cerrar(servicioId, valores);

            return Ok();
        }

        [HttpPut]
        [Route("api/servicio/{servicioId}/valores")]
        public IHttpActionResult Valores(int servicioId, Valor valores)
        {
            _servicioServiceExt.GuardarValores(servicioId, valores);
            return Ok();
        }

        [HttpPut]
        [Route("api/servicio/{servicioId:int}/facturar/{NoFactura}")]
        public IHttpActionResult Facturar(int servicioId, string NoFactura)
        {

            var usuarioId = HttpContext.Current.User.Identity.GetUserId();
            _servicioServiceExt.Facturar(servicioId, NoFactura);

            var seguimiento = new Seguimiento
            {
                NuevoEstado = "FA",
                Observacion = "Factura asignada: " + NoFactura,
                Fecha = DateTime.Now,
                ServicioId = servicioId,
                UsuarioRegistroId = usuarioId
            };
            _seguimientoService.Guardar(seguimiento);

            return Ok();
        }
    }
}