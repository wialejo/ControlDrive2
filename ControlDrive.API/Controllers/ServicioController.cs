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
using System.Diagnostics;
using ControlDrive.CORE.Modelos;
using ControlDrive.CORE.Infraestructura;
using ControlDrive.CORE.Services;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using System.Web;

namespace ControlDrive.Core.Controllers
{
    public class ServicioController : ApiController
    {
        private readonly ICommonInterface<Servicio> _servicioService;
        private IServicioService _servicioServiceExt;

        public ServicioController(ICommonInterface<Servicio> servicioService, IServicioService servicioServiceExt)
        {
            _servicioService = servicioService;
            _servicioServiceExt = servicioServiceExt;
        }

        [HttpPost]
        public IHttpActionResult Obtener(Periodo periodo)
        {   
            var servicios =  _servicioServiceExt.Obtener(periodo).Where(s => s.Estado.Codigo != "AN").OrderBy(s => s.Fecha).ToList();
            return Ok(servicios);
        }
        
        [HttpPost]
        public IHttpActionResult ObtenerParaSeguimiento(Periodo periodo)
        {
            var servicios = _servicioServiceExt.ObtenerParaSeguimiento(periodo);

            return Ok(servicios);
        }

        #region exportar a SCV
        [HttpPost]
        public async Task<HttpResponseMessage> ObtenerPorPeriodoCSV(Periodo periodo)
        {
            var periodoCompleto = new PeriodoService().Obtener(periodo.Inicio);
            var serviciosSeparadoPorComas = _servicioServiceExt.ObtenerPorPeriodoCSV(periodoCompleto);

            byte[] output = null;
            await Task.Run(() =>
            {
                using (var stream = GenerateStreamFromString(serviciosSeparadoPorComas))
                {
                    //this.CreateFile(FileContents[guid], stream);
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
                    FileName =  "servicios.csv"
                };
                result.Content.Headers.Add("x-filename", "servicios.csv");
                return result;
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NoContent, "No hay datos.");
        }


        public MemoryStream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        #endregion
        
        [HttpGet]
        public IHttpActionResult ObtenerPorId(int id)
        {
            var servicio = _servicioService.ObtenerPorId(id);
            return Ok(servicio);
        }

        [HttpPost]
        public IHttpActionResult Guardar(Servicio servicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var servicioModeloVista = _servicioService.Guardar(servicio);

            return Ok(servicioModeloVista);
        }
        
        [HttpPost]
        public IHttpActionResult NotificarServiciosAConductor([FromBody]ICollection<Servicio> servicios)
        {
            var respuesta = _servicioServiceExt.NotificarServiciosAConductor(servicios);
            return Ok(respuesta);
        }

        [HttpPost]
        public IHttpActionResult NotificarServiciosARuta([FromBody]ICollection<Servicio> servicios)
        {
            var respuesta = _servicioServiceExt.NotificarServiciosARuta(servicios);
            return Ok(respuesta);
        }
    }
}