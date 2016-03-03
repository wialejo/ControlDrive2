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
    public class EstadosController : ApiController
    {
        private readonly ICommonInterface<Estado> _estadoService;

        public EstadosController(ICommonInterface<Estado> estadoService) {
            _estadoService = estadoService;

        }

        [HttpGet]
        public IHttpActionResult Obtener()
        {
            var estados = _estadoService.Obtener();
            return Ok(estados);
        }
    }
}