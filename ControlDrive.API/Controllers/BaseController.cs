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
                IEnumerable<string> headerValues = Request.Headers.GetValues("idSucursal");
                return int.Parse(headerValues.FirstOrDefault());
            }
        }

    }
}