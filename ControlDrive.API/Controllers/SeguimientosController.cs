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
using System.Web;
using Microsoft.AspNet.Identity;
using ControlDrive.CORE.Services;
using ControlDrive.CORE.Modelos;

namespace ControlDrive.Core.Controllers
{
    [Authorize]
    public class SeguimientosController : ApiController
    {
        private readonly ICommonInterface<Seguimiento> _seguimientoService;
        private readonly ISeguimientoService _seguimientoServiceExtend;
        private readonly IServicioService _servicioServiceExt;

        public SeguimientosController(ICommonInterface<Seguimiento> seguimientoService, ISeguimientoService seguimientoServiceExtend, IServicioService servicioServiceExt)
        {
            _seguimientoService = seguimientoService;
            _seguimientoServiceExtend = seguimientoServiceExtend;
            _servicioServiceExt = servicioServiceExt;
        }


        [HttpGet]
        public IHttpActionResult ObtenerPorServicio(int id)
        {
            var seguimientos = _seguimientoServiceExtend.ObtenerPorServicio(id);

            return Ok(seguimientos);
        }

        [HttpPost]
        public IHttpActionResult Guardar(Seguimiento seguimiento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            seguimiento.UsuarioRegistroId = HttpContext.Current.User.Identity.GetUserId();
            var seguimientoRepo = _seguimientoService.Guardar(seguimiento);

            return Ok(seguimientoRepo);
        }

        [HttpGet]
        [Route("api/seguimientos/servicios/rango")]
        public IHttpActionResult ObtenerServicios([FromUri]DateTime startDate)
        {
            var servicios = new List<ServicioDto>();
            var periodo = new PeriodoService().Obtener(startDate);
            servicios = _servicioServiceExt.Obtener(s => s.Fecha > periodo.Inicio && s.Fecha < periodo.Fin & s.EstadoCodigo != "AN").ToList();
            return Ok(servicios);
        }
    }
}