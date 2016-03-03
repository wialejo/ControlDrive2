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
    public class ConductorService : ICommonInterface<Conductor>
    {

        private readonly IEntityBaseRepository<Conductor> _conductorRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public ConductorService(IEntityBaseRepository<Conductor> conductorRepositorio, IUnitOfWork unitOfWork)
        {
            _conductorRepositorio = conductorRepositorio;
            _unitOfWork = unitOfWork;
        }

        public List<Conductor> Obtener()
        {
            var conductores = _conductorRepositorio.GetAll().ToList();
            return conductores;
        }

        public List<Conductor> ObtenerPorDescripcion(string descripcion)
        {
            var conductores = _conductorRepositorio.FindBy(c => c.Nombre.Contains(descripcion)).ToList();
            return conductores;
        }

        public Conductor ObtenerPorId(int id)
        {
            var conductor = _conductorRepositorio.FindBy(c => c.Id == id).FirstOrDefault();
            return conductor;
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
                _conductorRepositorio.Edit(conductorRepo);
            }
            _unitOfWork.Commit();
            return conductorRepo;
        }
    }
}
