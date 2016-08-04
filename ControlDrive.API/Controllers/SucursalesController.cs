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

namespace ControlDrive.Core.Controllers
{
    [Authorize]
    public class SucursalesController : ApiController
    {
        private readonly ICommonInterface<Sucursal> _sucursalSevice;
        private readonly ISucursalService _sucursalServiceExt;

        public SucursalesController(ICommonInterface<Sucursal> sucursalSevice, ISucursalService sucursalSeviceExt)
        {
            _sucursalSevice = sucursalSevice;
            _sucursalServiceExt = sucursalSeviceExt;
        }

        [HttpGet]
        public IHttpActionResult Obtener()
        {
            var sucursales  = _sucursalSevice.Obtener();
            return Ok(sucursales);
        }

        [HttpGet]
        public IHttpActionResult Obtener(int id)
        {
            var sucursal  = _sucursalSevice.ObtenerPorId(id);
            return Ok(sucursal);
        }

        [HttpGet]
        public IHttpActionResult ObtenerPorUsuario()
        {
            var idUsuario = HttpContext.Current.User.Identity.GetUserId();
            var sucursales = _sucursalServiceExt.ObtenerPorUsuario(idUsuario);
            return Ok(sucursales);
        }

        // POST: api/Sucursales
        [HttpPost]
        public IHttpActionResult Guardar(Sucursal sucursal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var sucursalRepo = _sucursalSevice.Guardar(sucursal);

            return Ok(sucursalRepo);
        }

        // DELETE: api/Sucursales/5
        [HttpDelete]
        public IHttpActionResult DeleteSucursal(int id)
        {
            _sucursalSevice.Eliminar(id);

            return Ok();
        }
    }
}