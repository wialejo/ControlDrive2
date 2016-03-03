using ControlDrive.Core.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDrive.Core.Infraestructura
{
    public interface IDbFactory : IDisposable
    {
        ApplicationDbContext Init();
    }
}
