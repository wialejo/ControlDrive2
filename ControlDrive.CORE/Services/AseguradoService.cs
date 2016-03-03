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
    public class AseguradoService : ICommonInterface<Asegurado>
    {

        private readonly IEntityBaseRepository<Asegurado> _aseguradoRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public AseguradoService(IEntityBaseRepository<Asegurado> aseguradoRepositorio, IUnitOfWork unitOfWork)
        {
            _aseguradoRepositorio = aseguradoRepositorio;
            _unitOfWork = unitOfWork;
        }

        public List<Asegurado> Obtener()
        {
            var aseguradoes = _aseguradoRepositorio.GetAll().ToList();
            return aseguradoes;
        }

        public List<Asegurado> ObtenerPorDescripcion(string descripcion)
        {
            var aseguradoes = _aseguradoRepositorio.FindBy(c => c.Nombre.Contains(descripcion)).ToList();
            return aseguradoes;
        }

        public Asegurado ObtenerPorId(int id)
        {
            var asegurado = _aseguradoRepositorio.FindBy(c => c.Id == id).FirstOrDefault();
            return asegurado;
        }

        public void Eliminar(int id)
        {
            var aseguradoRepo = _aseguradoRepositorio.FindBy(c => c.Id == id).FirstOrDefault();
            _aseguradoRepositorio.Delete(aseguradoRepo);

            _unitOfWork.Commit();
        }

        public Asegurado Guardar(Asegurado asegurado)
        {
            var aseguradoRepo = new Asegurado();
            if (asegurado.Id == 0)
            {
                aseguradoRepo = _aseguradoRepositorio.Add(asegurado);
            }
            else
            {
                aseguradoRepo = _aseguradoRepositorio.FindBy(c => c.Id == asegurado.Id).FirstOrDefault();
                aseguradoRepo.Nombre = asegurado.Nombre;
                aseguradoRepo.Telefono1 = asegurado.Telefono1;
                aseguradoRepo.Telefono2 = asegurado.Telefono2;
                _aseguradoRepositorio.Edit(aseguradoRepo);
            }
            _unitOfWork.Commit();
            return aseguradoRepo;
        }
    }
}
