using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDrive.CORE.Services
{ 
    public interface ICommonInterface<T> where T : class
    {
        T Guardar(T entidad);
        List<T> Obtener();
        List<T> ObtenerPorDescripcion(string descripcion);
        T ObtenerPorId(int id);
        void Eliminar(int id);
    }
}
