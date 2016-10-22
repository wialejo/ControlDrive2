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
    [Authorize]
    public class UsuariosController : ApiController
    {
        private IUsuarioService _usuarioService;

        public UsuariosController(ICommonInterface<Sucursal> sucursalSevice, IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IHttpActionResult Obtener()
        {
            var usuarios = _usuarioService.Obtener();
            return Ok(usuarios);
        }

        [HttpGet]
        [Route("api/usuarios/{idUsuario}/AsignarSucursal/{idSucursal}")]
        public IHttpActionResult AsignarSucursal([FromUri]string idUsuario, [FromUri] int idSucursal)
        {
            try
            {
                _usuarioService.AsignarSucursal(idUsuario, idSucursal);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("api/usuarios/{idUsuario}/DesAsignarSucursal/{idSucursal}")]
        public IHttpActionResult DesAsignarSucursal([FromUri]string idUsuario, [FromUri] int idSucursal)
        {
            _usuarioService.DesAsignarSucursal(idUsuario, idSucursal);
            return Ok();
        }


        [HttpGet]
        [Route("api/usuarios/{idUsuario}/AsignarRol/{idRol}")]
        public IHttpActionResult AsignarRol([FromUri]string idUsuario, [FromUri] string idRol)
        {
            _usuarioService.AsignarRol(idUsuario, idRol);
            return Ok();
        }
        [HttpGet]
        [Route("api/usuarios/{idUsuario}/DesAsignarRol/{idRol}")]
        public IHttpActionResult DesAsignarRol([FromUri]string idUsuario, [FromUri] int idRol)
        {
            _usuarioService.DesAsignarSucursal(idUsuario, idRol);
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult ObtenerPorId(string id)
        {
            var usuarios = _usuarioService.ObtenerPorId(id);
            return Ok(usuarios);
        }

        [HttpPost]
        public IHttpActionResult Guardar(ApplicationUser usuario)
        {
            try
            {
                _usuarioService.Guardar(usuario);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/usuarios/ObtenerUsuarioActual")]
        public IHttpActionResult ObtenerUsuarioActual()
        {
            var idUsuario = HttpContext.Current.User.Identity.GetUserId();
            var usuarios = _usuarioService.ObtenerPorId(idUsuario);
            return Ok(usuarios);
        }
    }
}