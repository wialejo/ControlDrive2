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
    public class EstadoService : ICommonInterface<Estado>, IEstadoService
    {

        private readonly IEntityBaseRepository<Estado> _estadoRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public EstadoService(IEntityBaseRepository<Estado> estadoRepositorio, IUnitOfWork unitOfWork)
        {
            _estadoRepositorio = estadoRepositorio;
            _unitOfWork = unitOfWork;
        }

        public List<Estado> Obtener()
        {
            var estados = _estadoRepositorio.GetAll().OrderBy(e=>e.Orden).ToList();
            return estados;
        }

        public List<Estado> ObtenerPorDescripcion(string descripcion)
        {
            throw new NotImplementedException();
        }

        public Estado ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        public void Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public Estado Guardar(Estado entidad)
        {
            throw new NotImplementedException();
        }

        public Estado ObtenerPorCodigo(string codigo)
        {
            var estadoRepo = _estadoRepositorio.FindBy(e => e.Codigo == codigo).FirstOrDefault();
            return estadoRepo;
        }
    }
    public interface IEstadoService
    {
        Estado ObtenerPorCodigo(string codigo);
    }
}
