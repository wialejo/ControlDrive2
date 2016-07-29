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
    public class SucursalService : ICommonInterface<Sucursal>, ISucursalService
    {

        private readonly IEntityBaseRepository<Sucursal> _sucursalRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public SucursalService(IEntityBaseRepository<Sucursal> sucursalRepositorio, IUnitOfWork unitOfWork)
        {
            _sucursalRepositorio = sucursalRepositorio;
            _unitOfWork = unitOfWork;
        }

        public List<Sucursal> Obtener()
        {
            var sucursales = _sucursalRepositorio.GetAll().ToList();
            return sucursales;
        }

        public List<Sucursal> ObtenerPorDescripcion(string descripcion)
        {
            var sucursales = _sucursalRepositorio.FindBy(c => c.Descripcion.Contains(descripcion)).ToList();
            return sucursales;
        }

        public Sucursal ObtenerPorId(int id)
        {
            var sucursal = _sucursalRepositorio.FindBy(c => c.Id == id).FirstOrDefault();
            return sucursal;
        }

        public void Eliminar(int id)
        {
            var sucursalRepo = _sucursalRepositorio.FindBy(c => c.Id == id).FirstOrDefault();
            _sucursalRepositorio.Delete(sucursalRepo);

            _unitOfWork.Commit();
        }

        public Sucursal Guardar(Sucursal sucursal)
        {
            var sucursalRepo = new Sucursal();
            if (sucursal.Id == 0)
            {
                sucursalRepo = _sucursalRepositorio.Add(sucursal);
            }
            else
            {
                sucursalRepo = _sucursalRepositorio.FindBy(c => c.Id == sucursal.Id).FirstOrDefault();
                sucursalRepo.Descripcion = sucursal.Descripcion;
                _sucursalRepositorio.Edit(sucursalRepo);
            }
            _unitOfWork.Commit();
            return sucursalRepo;
        }

        public List<Sucursal> ObtenerPorUsuario(string idUsuario)
        {
            var sucursales = _sucursalRepositorio.FindBy(s => s.Usuarios.Any(u => u.Id == idUsuario)).ToList();
            return sucursales;
        }
    }

    public interface ISucursalService
    {
        List<Sucursal> ObtenerPorUsuario(string idUsuario);
    }
}

