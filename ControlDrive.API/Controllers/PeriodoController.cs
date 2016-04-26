using ControlDrive.CORE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ControlDrive.API.Controllers
{
    public class PeriodoController : ApiController
    {

        [HttpGet]
        [Route("api/periodo/FechaEnPeriodoActual")]
        public IHttpActionResult FechaEnPeriodoActual([FromUri]DateTime fecha)
        {
            var fechaEnPeriodoActual = new PeriodoService().FechaEnPeriodoActual(fecha);
            return Ok(fechaEnPeriodoActual);
        }
    }
}
