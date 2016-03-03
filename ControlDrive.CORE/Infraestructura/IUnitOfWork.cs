using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDrive.Core.Infraestructura
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
