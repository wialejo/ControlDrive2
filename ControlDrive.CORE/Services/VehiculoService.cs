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
    public class VehiculoService : ICommonInterface<Vehiculo>
    {

        private readonly IEntityBaseRepository<Vehiculo> _vehiculoRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public VehiculoService(IEntityBaseRepository<Vehiculo> vehiculoRepositorio, IUnitOfWork unitOfWork)
        {
            _vehiculoRepositorio = vehiculoRepositorio;
            _unitOfWork = unitOfWork;
        }

        public List<Vehiculo> Obtener()
        {
            var vehiculoes = _vehiculoRepositorio.GetAll().ToList();
            return vehiculoes;
        }

        public List<Vehiculo> ObtenerPorDescripcion(string descripcion)
        {
            throw new NotImplementedException();
        }

        public Vehiculo ObtenerPorId(int id)
        {
            var vehiculo = _vehiculoRepositorio.FindBy(c => c.Id == id).FirstOrDefault();
            return vehiculo;
        }

        public void Eliminar(int id)
        {
            var vehiculoRepo = _vehiculoRepositorio.FindBy(c => c.Id == id).FirstOrDefault();
            _vehiculoRepositorio.Delete(vehiculoRepo);

            _unitOfWork.Commit();
        }

        public Vehiculo Guardar(Vehiculo vehiculo)
        {
            var vehiculoRepo = new Vehiculo();
            if (vehiculo.Id == 0)
            {
                vehiculoRepo = _vehiculoRepositorio.Add(vehiculo);
            }
            else
            {
                vehiculoRepo = _vehiculoRepositorio.FindBy(c => c.Id == vehiculo.Id).FirstOrDefault();
                vehiculoRepo.Placa = vehiculo.Placa;
                vehiculoRepo.Marca = vehiculo.Marca;
                vehiculoRepo.Observaciones = vehiculo.Observaciones;
                vehiculoRepo.Referencia = vehiculo.Referencia;
                _vehiculoRepositorio.Edit(vehiculoRepo);
            }
            _unitOfWork.Commit();
            return vehiculoRepo;
        }
    }
}
