using ControlDrive.Core.Infraestructura;
using ControlDrive.Core.Modelos;
using ControlDrive.CORE.Modelos;
using ControlDrive.CORE.Repositorios;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDrive.CORE.Services
{
    public class SeguimientoService : ICommonInterface<Seguimiento>, ISeguimientoService
    {

        private readonly IEntityBaseRepository<Seguimiento> _SeguimientoRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEstadoService _estadoService;
        private readonly IServicioService _servicioServiceExt;

        public SeguimientoService(IEntityBaseRepository<Seguimiento> SeguimientoRepositorio, IUnitOfWork unitOfWork, 
            IEstadoService estadoService, IServicioService servicioServiceExt)
        {
            _SeguimientoRepositorio = SeguimientoRepositorio;
            _estadoService = estadoService;
            _servicioServiceExt = servicioServiceExt;
            _unitOfWork = unitOfWork;
        }

        public List<Seguimiento> Obtener()
        {
            var seguimientos = _SeguimientoRepositorio.GetAll().ToList();
            return seguimientos;
        }
        
        public Seguimiento ObtenerPorId(int id)
        {
            var seguimiento = _SeguimientoRepositorio.FindBy(c => c.Id == id).FirstOrDefault();
            return seguimiento;
        }

        public void Eliminar(int id)
        {
            var seguimientoRepo = _SeguimientoRepositorio.FindBy(c => c.Id == id).FirstOrDefault();
            _SeguimientoRepositorio.Delete(seguimientoRepo);

            _unitOfWork.Commit();
        }

        public Seguimiento Guardar(Seguimiento seguimiento)
        {
            var seguimientoRepo = new Seguimiento();
            if (seguimiento.Id == 0)
            {
                var nuevoEstado = _estadoService.ObtenerPorCodigo(seguimiento.NuevoEstado);
                seguimiento.Observacion  = "Nuevo estado: " + nuevoEstado.Descripcion + ",  " + seguimiento.Observacion;
                seguimiento.Fecha = DateTime.Now;
                _servicioServiceExt.CambioEstado(seguimiento.ServicioId, nuevoEstado);
                seguimientoRepo = _SeguimientoRepositorio.Add(seguimiento);
            }

            _unitOfWork.Commit();
            return seguimientoRepo;
        }

        public List<Seguimiento> ObtenerPorDescripcion(string descripcion)
        {
            throw new NotImplementedException();
        }

        public List<Seguimiento> ObtenerPorServicio(int id)
        {
            var seguimientos = _SeguimientoRepositorio.FindBy(s => s.ServicioId == id).ToList();
            return seguimientos;
        }
    }

    public interface ISeguimientoService
    {
        List<Seguimiento> ObtenerPorServicio(int id);
    }
}
