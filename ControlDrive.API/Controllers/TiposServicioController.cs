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
using ControlDrive.CORE.Services;
using ControlDrive.CORE.Modelos;

namespace ControlDrive.Core.Controllers
{
    //[Authorize]
    public class TiposServicioController : ApiController
    {
        private readonly ITiposServiciosService _tipoServicioService;

        public TiposServicioController(ITiposServiciosService tipoServicioService) {
            _tipoServicioService = tipoServicioService;

        }

        [HttpGet]
        public IHttpActionResult Obtener()
        {
            var tipoServicios = _tipoServicioService.Obtener();
            return Ok(tipoServicios);
        }
    }
}