using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ControlDrive.Core.Controllers
{
    //[Authorize]
    public class BaseController : ApiController
    {
        public int IdSucursal
        {
            get
            {
                if (!Request.Headers.Contains("idSucursal"))
                    throw new System.Exception("Es necesario seleccionar una sucursal.");

                IEnumerable<string> headerValues = Request.Headers.GetValues("idSucursal");
                return int.Parse(headerValues.FirstOrDefault());
            }
        }

    }
}