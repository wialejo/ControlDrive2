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
    public class CiudadService : ICommonInterface<Ciudad>, ICiudadService
    {

        private readonly IEntityBaseRepository<Ciudad> _ciudadRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public CiudadService(IEntityBaseRepository<Ciudad> ciudadRepositorio, IUnitOfWork unitOfWork)
        {
            _ciudadRepositorio = ciudadRepositorio;
            _unitOfWork = unitOfWork;
        }

        public List<Ciudad> Obtener()
        {
            var ciudades = _ciudadRepositorio.GetAll().ToList();
            return ciudades;
        }

        public List<Ciudad> ObtenerPorDescripcion(string descripcion)
        {
            var ciudades = _ciudadRepositorio.FindBy(c => c.Nombre.Contains(descripcion)).ToList();
            return ciudades;
        }

        public Ciudad ObtenerPorId(int id)
        {
            var ciudad = _ciudadRepositorio.FindBy(c => c.Id == id).FirstOrDefault();
            return ciudad;
        }

        public void Eliminar(int id)
        {
            var ciudadRepo = _ciudadRepositorio.FindBy(c => c.Id == id).FirstOrDefault();
            _ciudadRepositorio.Delete(ciudadRepo);

            _unitOfWork.Commit();
        }

        public Ciudad Guardar(Ciudad ciudad)
        {
            var ciudadRepo = new Ciudad();
            if (ciudad.Id == 0)
            {
                ciudadRepo = _ciudadRepositorio.Add(ciudad);
            }
            else
            {
                ciudadRepo = _ciudadRepositorio.FindBy(c => c.Id == ciudad.Id).FirstOrDefault();
                ciudadRepo.Principal = ciudad.Principal;
                ciudadRepo.Nombre = ciudad.Nombre;
                ciudadRepo.Tarifa = ciudad.Tarifa;
                ciudadRepo.SucursalId = ciudad.SucursalId;
                _ciudadRepositorio.Edit(ciudadRepo);
            }
            _unitOfWork.Commit();
            return ciudadRepo;
        }

        public IQueryable<Ciudad> ObtenerQ()
        {
            var ciudades = _ciudadRepositorio.GetAll();
            return ciudades;
        }
    }

    public interface ICiudadService {
        IQueryable<Ciudad> ObtenerQ();
    }
}
