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

namespace ControlDrive.Core.Controllers
{
    [Authorize]
    public class DocumentosController : ApiController
    {
        private readonly IDocumentosService _documentosService;

        public DocumentosController(IDocumentosService documentosService)
        {
            _documentosService = documentosService;
        }

        [HttpGet]
        [Route("api/documentos/cliente")]
        public IHttpActionResult ObtenerDocumentosCliente([FromUri]DateTime inicio, [FromUri]DateTime fin, [FromUri] int clienteId)
        {
            try
            {
                var documentos = _documentosService.Obtener(d => d.ClienteId == clienteId).ToList();
                return Ok(documentos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("api/documentos/relacionServicios")]
        public IHttpActionResult ObtenerDocumentosServicios([FromBody]List<Documento> documentos)
        {
            try
            {
                var documentosRelacionServicios = _documentosService.ObtenerRelacionServicios(documentos).ToList();
                return Ok(documentosRelacionServicios);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Route("api/documentos")]
        public IHttpActionResult Guardar([FromBody]Documento documento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                documento.UsuarioRegistroId = HttpContext.Current.User.Identity.GetUserId();
                _documentosService.Guardar(documento);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}