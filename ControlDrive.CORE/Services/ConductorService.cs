using ControlDrive.Core.Infraestructura;
using ControlDrive.Core.Modelos;
using ControlDrive.CORE.Modelos;
using ControlDrive.CORE.Repositorios;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ControlDrive.CORE.Services
{
    public class ConductorService : IConductorService
    {

        private readonly IEntityBaseRepository<Conductor> _conductorRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public ConductorService(IEntityBaseRepository<Conductor> conductorRepositorio, IUnitOfWork unitOfWork)
        {
            _conductorRepositorio = conductorRepositorio;
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Conductor> Obtener()
        {
            var conductores = _conductorRepositorio.GetAll();
            return conductores;
        }

        public void Eliminar(int id)
        {
            var conductorRepo = _conductorRepositorio.FindBy(c => c.Id == id).FirstOrDefault();
            _conductorRepositorio.Delete(conductorRepo);

            _unitOfWork.Commit();
        }

        public Conductor Guardar(Conductor conductor)
        {
            var conductorRepo = new Conductor();
            if (conductor.Id == 0)
            {
                conductorRepo = _conductorRepositorio.Add(conductor);
            }
            else
            {
                conductorRepo = _conductorRepositorio.FindBy(c => c.Id == conductor.Id).FirstOrDefault();
                conductorRepo.Nombre = conductor.Nombre;
                conductorRepo.Email = conductor.Email;
                conductorRepo.TipoIdentificacion = conductor.TipoIdentificacion;
                conductorRepo.Identificacion = conductor.Identificacion;
                conductorRepo.Telefono1 = conductor.Telefono1;
                conductorRepo.Telefono2 = conductor.Telefono2;
                conductorRepo.Direccion = conductor.Direccion;
                conductorRepo.Activo = conductor.Activo;
                conductorRepo.SucursalId = conductor.SucursalId;
                _conductorRepositorio.Edit(conductorRepo);
            }
            _unitOfWork.Commit();
            return conductorRepo;
        }
    }

    public interface IConductorService {
        Conductor Guardar(Conductor conductor);
        IQueryable<Conductor> Obtener();
        void Eliminar(int id);
    }
}
