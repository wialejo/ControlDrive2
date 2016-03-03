using ControlDrive.Core.Infraestructura;
using ControlDrive.CORE.Modelos;
using ControlDrive.CORE.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDrive.CORE.Services
{
    public class DireccionService : ICommonInterface<Direccion>
    {

        private readonly IEntityBaseRepository<Direccion> _direccionRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public DireccionService(IEntityBaseRepository<Direccion> direccionRepositorio, IUnitOfWork unitOfWork)
        {
            _direccionRepositorio = direccionRepositorio;
            _unitOfWork = unitOfWork;
        }

        public void Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public Direccion Guardar(Direccion direccion)
        {
            var direccionRepo = new Direccion();
            if (direccion.Id == 0)
            {
                direccion.CiudadId = direccion.Ciudad.Id;
                direccion.Ciudad = null;
                
                direccionRepo = _direccionRepositorio.Add(direccion);
            }
            else {
                direccionRepo = _direccionRepositorio.FindBy(d => d.Id == direccion.Id).FirstOrDefault();
                direccionRepo.Descripcion = direccion.Descripcion;
                direccionRepo.CiudadId = direccion.Ciudad.Id;
                direccionRepo.Barrio = direccion.Barrio;

                _direccionRepositorio.Edit(direccionRepo);
            }
            _unitOfWork.Commit();
            
            return direccionRepo;
        }

       
        public List<Direccion> Obtener()
        {
            throw new NotImplementedException();
        }

        public List<Direccion> ObtenerPorDescripcion(string descripcion)
        {
            throw new NotImplementedException();
        }

        public Direccion ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
