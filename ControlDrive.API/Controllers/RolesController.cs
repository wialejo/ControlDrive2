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
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ControlDrive.Core.Controllers
{
    //[Authorize]
    public class RolesController : ApiController
    {
        private IRolService _rolService;

        public RolesController(ICommonInterface<Sucursal> sucursalSevice, IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        public IHttpActionResult Obtener()
        {
            var roles = _rolService.Obtener();
            return Ok(roles);
        }


        [HttpGet]
        public IHttpActionResult Guardar([FromUri]string nombre)
        {
            var rolRepo = _rolService.Guardar(nombre);
            return Ok(rolRepo);
        }
    }
}