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
    public class MovimientosService : IMovimientosService
    {

        private readonly IEntityBaseRepository<Movimiento> _movimientoRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityBaseRepository<Servicio> _servicioRepositorio;

        public MovimientosService(IEntityBaseRepository<Movimiento> movimientoRepositorio, IEntityBaseRepository<Servicio> servicioRepositorio, IUnitOfWork unitOfWork)
        {
            _movimientoRepositorio = movimientoRepositorio;
            _servicioRepositorio = servicioRepositorio;
            _unitOfWork = unitOfWork;
        }

        public List<Movimiento> Obtener(Expression<Func<Movimiento, bool>> predicate)
        {
            var movimientos = _movimientoRepositorio.FindByIncluding(predicate, s => s.Servicio).OrderBy(m => m.Servicio.Fecha).ToList();

            movimientos.ForEach(movimiento =>
            {
                movimiento.Documento = null;
                movimiento.Servicio.Movimientos = null;
                movimiento.Cliente = null;
                movimiento.Proveedor = null;
                movimiento.UsuarioModificacion = null;
                movimiento.UsuarioRegistro = null;
                movimiento.Servicio.Conductor = null;
                movimiento.Servicio.Ruta = null;
                movimiento.Servicio.UsuarioModificacion = null;
                movimiento.Servicio.UsuarioRegistro = null;
            });

            return movimientos;
        }

        public Movimiento ObtenerPorId(int id)
        {
            var movimiento = _movimientoRepositorio.FindBy(m => m.Id == id).FirstOrDefault();
            return movimiento;
        }
        public void Guardar(Movimiento movimiento)
        {
            //movimientos.ForEach(movimiento =>
            //{
            if (movimiento.Id != 0)
            {
                _movimientoRepositorio.Update(movimiento, m => m.Valor);
            }
            else
            {
                _movimientoRepositorio.Add(movimiento);
            }
            // });
            _unitOfWork.Commit();
        }

        public void ActualizarParaCierreFacuracion(Movimiento movimiento)
        {
            _servicioRepositorio.Update(new Servicio
            {
                Id = movimiento.Servicio.Id,
                Radicado = movimiento.Servicio.Radicado,
                AseguradoraId = movimiento.Servicio.Aseguradora.Id
            }, s => s.Radicado, s => s.AseguradoraId);

            _movimientoRepositorio.Update(new Movimiento
            {
                Id = movimiento.Id,
                Valor = movimiento.Valor,
                ClienteId = movimiento.Servicio.Aseguradora.Id,
                Aprobado = true
            }, m => m.Valor, m => m.ClienteId, m => m.Aprobado);

            _unitOfWork.Commit();
        }
    }

    public interface IMovimientosService
    {
        void Guardar(Movimiento movimiento);
        List<Movimiento> Obtener(Expression<Func<Movimiento, bool>> predicate);
        void ActualizarParaCierreFacuracion(Movimiento movimiento);
        Movimiento ObtenerPorId(int id);
    }
}
