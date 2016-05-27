using ControlDrive.Core.Infraestructura;
using ControlDrive.Core.Modelos;
using ControlDrive.CORE.Enums;
using ControlDrive.CORE.Infraestructura;
using ControlDrive.CORE.Modelos;
using ControlDrive.CORE.Repositorios;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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

        public List<MovimientoDto> Obtener(Expression<Func<Movimiento, bool>> predicate)
        {
            var movimientos = _movimientoRepositorio.FindByIncluding(predicate, s => s.Servicio)
                .OrderBy(m => m.Servicio.Fecha)
                .Select(m => new MovimientoDto
                {
                    Id = m.Id,
                    ServicioId = m.ServicioId,
                    Valor = m.Valor,
                    Concepto = new ServicioConceptoDto { Codigo = m.Concepto.Codigo, Descripcion = m.Concepto.Descripcion },
                    ConceptoCodigo = m.ConceptoCodigo,
                    DocumentoId = m.DocumentoId,
                    FechaRegistro = m.FechaRegistro,
                    UsuarioRegistroId = m.UsuarioRegistroId,
                    FechaModificacion = m.FechaModificacion,
                    UsuarioModificacionId = m.UsuarioModificacionId,
                    Proveedor = m.ConceptoCodigo == "PROVE_COND_ELE" ? m.Servicio.Conductor : m.Servicio.Ruta,
                    Aprobado = m.Aprobado,
                    Servicio = new ServicioDto
                    {
                        Id = m.Servicio.Id,
                        EstadoCodigo = m.Servicio.EstadoCodigo,
                        Estado = m.Servicio.Estado,
                        Fecha = m.Servicio.Fecha,
                        Hora = m.Servicio.Hora,
                        Radicado = m.Servicio.Radicado,
                        AsignadoPor = m.Servicio.AsignadoPor,
                        VehiculoId = m.Servicio.VehiculoId,
                        Vehiculo = m.Servicio.Vehiculo,
                        AseguradoraId = m.Servicio.AseguradoraId,
                        Aseguradora = m.Servicio.Aseguradora,
                        AseguradoId = m.Servicio.AseguradoId,
                        Asegurado = m.Servicio.Asegurado,
                        DireccionInicioId = m.Servicio.DireccionInicioId,
                        DireccionInicio = m.Servicio.DireccionInicio,
                        DireccionDestinoId = m.Servicio.DireccionDestinoId,
                        DireccionDestino = m.Servicio.DireccionDestino,
                        UsuarioRegistro = new ApplicationUserDto { Nombre = m.Servicio.UsuarioRegistro.Nombre },
                        FechaRegistro = m.Servicio.FechaRegistro
                    }
                })
                .ToList();

            return movimientos;
        }

        public Movimiento ObtenerPorId(int id)
        {
            var movimiento = _movimientoRepositorio.FindBy(m => m.Id == id).FirstOrDefault();
            return movimiento;
        }

        public void Guardar(Movimiento movimiento)
        {
            var movimientoRepo = _movimientoRepositorio.FindBy(m => m.ConceptoCodigo == movimiento.ConceptoCodigo && m.ServicioId == movimiento.ServicioId).FirstOrDefault();
            if (movimientoRepo != null)
            {
                movimientoRepo.Valor = movimiento.Valor;
                _movimientoRepositorio.Edit(movimientoRepo);
            }
            else
            {
                _movimientoRepositorio.Add(movimiento);
            }
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
                Aprobado = true
            }, m => m.Valor, m => m.Aprobado);

            _unitOfWork.Commit();
        }

        public MemoryStream ObtenerResumenMovimientosEnCSV(List<MovimientoDto> movimientos)
        {
            var resumenMovimientos = GenerarResumenMovimientos(movimientos);
            var memoryExcel = ExcelUtilities<MovimientoResumen>.Export(resumenMovimientos);
            return memoryExcel;
        }

        private List<MovimientoResumen> GenerarResumenMovimientos(List<MovimientoDto> movimientos)
        {
            var movimientosResumen = new List<MovimientoResumen>();
            movimientos.ForEach(f =>
            {
                var movimientoResumen = new MovimientoResumen
                {
                    hora = f.Servicio.Fecha.ToString("HH:mm"),
                    fecha = f.Servicio.Fecha.ToString("dd/MM/yyyy"),
                    codigo = f.Servicio.Radicado,
                    aseguradora = f.Servicio.Aseguradora.Nombre,
                    asignadoPor = f.Servicio.AsignadoPor,
                    asegurado =
                            f.Servicio.Asegurado.Nombre +
                            (!string.IsNullOrEmpty(f.Servicio.Asegurado.Telefono1) ? " " + f.Servicio.Asegurado.Telefono1 : "") +
                            (!string.IsNullOrEmpty(f.Servicio.Asegurado.Telefono2) ? " " + f.Servicio.Asegurado.Telefono2 : ""),
                    vehiculo =
                            f.Servicio.Vehiculo.Placa +
                            (!string.IsNullOrEmpty(f.Servicio.Vehiculo.Descripcion) ? " " + f.Servicio.Vehiculo.Descripcion : ""),

                    origen =
                            f.Servicio.DireccionInicio.Descripcion +
                            (!string.IsNullOrEmpty(f.Servicio.DireccionInicio.Barrio) ? " " + f.Servicio.DireccionInicio.Barrio : "") +
                            (!string.IsNullOrEmpty(f.Servicio.DireccionInicio.Ciudad.Nombre) ? " " + f.Servicio.DireccionInicio.Ciudad.Nombre : ""),

                    destino =
                            f.Servicio.DireccionDestino.Descripcion +
                            (!string.IsNullOrEmpty(f.Servicio.DireccionDestino.Barrio) ? " " + f.Servicio.DireccionDestino.Barrio : "") +
                            (!string.IsNullOrEmpty(f.Servicio.DireccionDestino.Ciudad.Nombre) ? " " + f.Servicio.DireccionDestino.Ciudad.Nombre : ""),
                    valor = f.Valor
                };

                movimientosResumen.Add(movimientoResumen);
            });

            return movimientosResumen;
        }
    }

    public interface IMovimientosService
    {
        void Guardar(Movimiento movimiento);
        List<MovimientoDto> Obtener(Expression<Func<Movimiento, bool>> predicate);
        void ActualizarParaCierreFacuracion(Movimiento movimiento);
        MemoryStream ObtenerResumenMovimientosEnCSV(List<MovimientoDto> movimientos);

        Movimiento ObtenerPorId(int id);
    }

    public class MovimientoResumen
    {
        public string asegurado { get; internal set; }
        public string aseguradora { get; internal set; }
        public string asignadoPor { get; internal set; }
        public string codigo { get; internal set; }
        public string destino { get; internal set; }
        public string fecha { get; internal set; }
        public string hora { get; internal set; }
        public string origen { get; internal set; }
        public decimal valor { get; internal set; }
        public string vehiculo { get; internal set; }
    }
}
