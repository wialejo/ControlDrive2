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
    public class AseguradoraService : ICommonInterface<Aseguradora>
    {

        private readonly IEntityBaseRepository<Aseguradora> _aseguradoraRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public AseguradoraService(IEntityBaseRepository<Aseguradora> aseguradoraRepositorio, IUnitOfWork unitOfWork)
        {
            _aseguradoraRepositorio = aseguradoraRepositorio;
            _unitOfWork = unitOfWork;
        }

        public List<Aseguradora> Obtener()
        {
            var aseguradoraes = _aseguradoraRepositorio.GetAll().ToList();
            return aseguradoraes;
        }

        public List<Aseguradora> ObtenerPorDescripcion(string descripcion)
        {
            var aseguradoraes = _aseguradoraRepositorio.FindBy(c => c.Nombre.Contains(descripcion)).ToList();
            return aseguradoraes;
        }

        public Aseguradora ObtenerPorId(int id)
        {
            var aseguradora = _aseguradoraRepositorio.FindBy(c => c.Id == id).FirstOrDefault();
            return aseguradora;
        }

        public void Eliminar(int id)
        {
            var aseguradoraRepo = _aseguradoraRepositorio.FindBy(c => c.Id == id).FirstOrDefault();
            _aseguradoraRepositorio.Delete(aseguradoraRepo);

            _unitOfWork.Commit();
        }

        public Aseguradora Guardar(Aseguradora aseguradora)
        {
            var aseguradoraRepo = new Aseguradora();
            if (aseguradora.Id == 0)
            {
                aseguradoraRepo = _aseguradoraRepositorio.Add(aseguradora);
            }
            else
            {
                aseguradoraRepo = _aseguradoraRepositorio.FindBy(c => c.Id == aseguradora.Id).FirstOrDefault();
                aseguradoraRepo.Nombre = aseguradora.Nombre;
                _aseguradoraRepositorio.Edit(aseguradoraRepo);
            }
            _unitOfWork.Commit();
            return aseguradoraRepo;
        }
    }
}
