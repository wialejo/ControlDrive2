﻿using ControlDrive.Core.Infraestructura;
using ControlDrive.Core.Modelos;
using ControlDrive.CORE.Infraestructura;
using ControlDrive.CORE.Modelos;
using ControlDrive.CORE.Repositorios;
using FileHelpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ControlDrive.CORE.Extensions;
using System.Linq.Expressions;
using ControlDrive.CORE.Enums;

namespace ControlDrive.CORE.Services
{
    public class ServicioService : ICommonInterface<Servicio>, IServicioService
    {

        private readonly IEntityBaseRepository<Servicio> _servicioRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommonInterface<Direccion> _direccionService;
        private readonly ICommonInterface<Asegurado> _aseguradoService;
        private readonly ICommonInterface<Vehiculo> _vehiculoService;
        private readonly ICorreoService _correoService;
        private readonly IEntityBaseRepository<Sucursal> _sucursalRepositorio;
        private readonly IEntityBaseRepository<TipoServicio> _tipoServicioRepositorio;
        private readonly IEntityBaseRepository<Conductor> _conductorRepositorio;
        private readonly IEntityBaseRepository<Seguimiento> _seguimientoRepositorio;
        private readonly IEntityBaseRepository<Movimiento> _movimientoRepositorio;

        public IEntityBaseRepository<Direccion> _direccionRepositorio { get; }

        public ServicioService(IEntityBaseRepository<Movimiento> movimientoRepositorio, IEntityBaseRepository<Servicio> servicioRepositorio, ICommonInterface<Direccion> direccionService,
            ICommonInterface<Asegurado> aseguradoService, ICommonInterface<Vehiculo> vehiculoService,
            ICorreoService correoService,
            IEntityBaseRepository<Direccion> direccionRepositorio,
            IEntityBaseRepository<Sucursal> sucursalRepositorio,
            IEntityBaseRepository<TipoServicio> tipoServicioRepositorio,
            IEntityBaseRepository<Conductor> conductorRepositorio,
            IEntityBaseRepository<Seguimiento> seguimientoRepositorio,
            IUnitOfWork unitOfWork)
        {
            _direccionRepositorio = direccionRepositorio;
            _sucursalRepositorio = sucursalRepositorio;
            _tipoServicioRepositorio = tipoServicioRepositorio;
            _conductorRepositorio = conductorRepositorio;
            this._seguimientoRepositorio = seguimientoRepositorio;
            _servicioRepositorio = servicioRepositorio;
            _movimientoRepositorio = movimientoRepositorio;
            _direccionService = direccionService;
            _aseguradoService = aseguradoService;
            _vehiculoService = vehiculoService;
            _correoService = correoService;
            _unitOfWork = unitOfWork;
        }

        public Servicio Guardar(Servicio servicio)
        {
            var servicioRepo = new Servicio();

            bool EsEdicion = servicio.Id != 0;

            if (EsEdicion)
            {
                if (!string.IsNullOrEmpty(servicio.Radicado)
                    && _servicioRepositorio.FindBy(s => s.Radicado == servicio.Radicado && s.Id != servicio.Id).Count() > 0)
                {
                    if (servicio.TipoServicio.Id != 6 && servicio.TipoServicio.Id != 3)
                    {
                        throw new Exception("El consecutivo ingresado ya se encuentra registrado.");
                    }
                }
                servicioRepo = _servicioRepositorio.FindByIncluding(s => s.Id == servicio.Id, s => s.Movimientos).FirstOrDefault();

                servicioRepo.FechaModificacion = DateTime.Now;
                servicioRepo.UsuarioModificacionId = servicio.UsuarioModificacionId;
            }
            else
            {
                if (_servicioRepositorio.FindBy(s => s.Radicado == servicio.Radicado).Count() > 0
                    && !string.IsNullOrEmpty(servicio.Radicado))
                {
                    if (servicio.TipoServicio.Id != 6)
                    {
                        throw new Exception("El consecutivo ingresado ya se encuentra registrado.");
                    }
                }
                servicioRepo.FechaModificacion = DateTime.Now;
                servicioRepo.UsuarioModificacionId = servicio.UsuarioModificacionId;
                servicioRepo.FechaRegistro = DateTime.Now;
                servicioRepo.UsuarioRegistroId = servicio.UsuarioRegistroId;
                servicioRepo.SucursalId = servicio.SucursalId;

                if (!servicio.TipoServicio.RequiereSeguimiento)
                {
                    servicio.EstadoCodigo = "TE";
                    servicio.Fecha = servicio.Fecha.AddHours(18);
                    servicio.Hora = new TimeSpan(18, 0, 0);
                }
                else
                {
                    servicio.EstadoCodigo = "RG";
                }
            }


            servicioRepo.EstadoCodigo = servicio.EstadoCodigo;
            servicioRepo.TipoServicioId = servicio.TipoServicio.Id;
            servicioRepo.Hora = servicio.Hora;
            servicioRepo.Fecha = servicio.Fecha;
            servicioRepo.Radicado = servicio.Radicado;
            servicioRepo.AsignadoPor = servicio.AsignadoPor;
            servicioRepo.Observacion = servicio.Observacion;
            servicioRepo.Notificado = false;

            if (servicio.Aseguradora != null)
            {
                servicioRepo.AseguradoraId = servicio.Aseguradora.Id;
            }

            if (servicio.Asegurado != null)
            {
                _aseguradoService.Guardar(servicio.Asegurado);
                servicioRepo.AseguradoId = servicio.Asegurado.Id;
            }

            if (servicio.Vehiculo != null)
            {
                _vehiculoService.Guardar(servicio.Vehiculo);
                servicioRepo.VehiculoId = servicio.Vehiculo.Id;
            }

            if (servicio.DireccionInicio != null)
            {
                _direccionService.Guardar(servicio.DireccionInicio);
                servicioRepo.DireccionInicioId = servicio.DireccionInicio.Id;
            }

            if (servicio.DireccionDestino != null)
            {
                _direccionService.Guardar(servicio.DireccionDestino);
                servicioRepo.DireccionDestinoId = servicio.DireccionDestino.Id;
            }

            if (servicio.Conductor == null)
                servicioRepo.ConductorId = null;
            else
                servicioRepo.ConductorId = servicio.Conductor.Id;

            if (servicio.Ruta == null)
                servicioRepo.RutaId = null;
            else
                servicioRepo.RutaId = servicio.Ruta.Id;


            if (servicioRepo.Movimientos != null)
            {
                servicioRepo.Movimientos.ToList().ForEach(movimiento =>
                {
                    if (movimiento.Concepto.TipoConcepto == TipoConcepto.Proveedor && movimiento.Concepto.TipoProveedor == TipoProveedor.Conductor)
                    {
                        movimiento.ProveedorId = servicio.ConductorId;
                    }

                    if (movimiento.Concepto.TipoConcepto == TipoConcepto.Proveedor && movimiento.Concepto.TipoProveedor == TipoProveedor.Ruta)
                    {
                        movimiento.ProveedorId = servicio.RutaId;
                    }

                    if (movimiento.Concepto.TipoConcepto == TipoConcepto.Cliente)
                    {
                        movimiento.ClienteId = servicio.AseguradoraId;
                    }
                });
            }

            if (EsEdicion)
                _servicioRepositorio.Edit(servicioRepo);
            else
                servicioRepo = _servicioRepositorio.Add(servicioRepo);

            _unitOfWork.Commit();

            return servicioRepo;


            //if (servicio.Id == 0)
            //{
            //    if (_servicioRepositorio.FindBy(s => s.Radicado == servicio.Radicado).Count() > 0 && !string.IsNullOrEmpty(servicio.Radicado))
            //    {
            //        throw new Exception("El consecutivo ingresado ya se encuentra registrado.");
            //    }

            //    servicioRepo.TipoServicio = servicio.TipoServicio;
            //    servicioRepo.UsuarioRegistroId = servicio.UsuarioRegistroId;
            //    servicioRepo.EstadoCodigo = "RG";

            //    servicioRepo.Fecha = servicio.Fecha;
            //    servicioRepo.Hora = servicio.Hora;
            //    servicioRepo.Radicado = servicio.Radicado;
            //    servicioRepo.AsignadoPor = servicio.AsignadoPor;

            //    if (servicio.Aseguradora != null)
            //    {
            //        servicioRepo.AseguradoraId = servicio.Aseguradora.Id;
            //    }

            //    if (servicio.Asegurado != null)
            //    {
            //        _aseguradoService.Guardar(servicio.Asegurado);
            //        servicioRepo.AseguradoId = servicio.Asegurado.Id;
            //    }

            //    if (servicio.Vehiculo != null)
            //    {
            //        _vehiculoService.Guardar(servicio.Vehiculo);
            //        servicioRepo.VehiculoId = servicio.Vehiculo.Id;
            //    }

            //    if (servicio.DireccionInicio != null) {

            //        _direccionService.Guardar(servicio.DireccionInicio);
            //        servicioRepo.DireccionInicioId = servicio.DireccionInicio.Id;
            //    }

            //    if (servicio.DireccionDestino != null)
            //    {
            //        _direccionService.Guardar(servicio.DireccionDestino);
            //        servicioRepo.DireccionDestinoId = servicio.DireccionDestino.Id;
            //    }

            //    servicioRepo.FechaModificacion = DateTime.Now;
            //    servicioRepo.FechaRegistro = DateTime.Now;
            //    servicioRepo.Observacion = servicio.Observacion;

            //    if (servicio.Conductor == null)
            //        servicioRepo.ConductorId = null;
            //    else
            //        servicioRepo.ConductorId = servicio.Conductor.Id;

            //    if (servicio.Ruta == null)
            //        servicioRepo.RutaId = null;
            //    else
            //        servicioRepo.RutaId = servicio.Ruta.Id;

            //    servicioRepo = _servicioRepositorio.Add(servicioRepo);
            //}
            //else
            //{

            //    if (!string.IsNullOrEmpty(servicio.Radicado) && _servicioRepositorio.FindBy(s => s.Radicado == servicio.Radicado && s.Id != servicio.Id).Count() > 0)
            //    {
            //        throw new Exception("El consecutivo ingresado ya se encuentra registrado.");
            //    }

            //    servicioRepo = _servicioRepositorio.FindBy(s => s.Id == servicio.Id).FirstOrDefault();
            //    servicioRepo.TipoServicio = servicio.TipoServicio;
            //    servicioRepo.UsuarioModificacionId = servicio.UsuarioModificacionId;
            //    servicioRepo.Fecha = servicio.Fecha;
            //    servicioRepo.Hora = servicio.Hora;
            //    servicioRepo.Radicado = servicio.Radicado;
            //    servicioRepo.EstadoCodigo = servicio.EstadoCodigo;
            //    servicioRepo.AsignadoPor = servicio.AsignadoPor;
            //    servicioRepo.AseguradoraId = servicio.Aseguradora.Id;

            //    _aseguradoService.Guardar(servicio.Asegurado);
            //    servicioRepo.AseguradoId = servicio.Asegurado.Id;

            //    _vehiculoService.Guardar(servicio.Vehiculo);
            //    servicioRepo.VehiculoId = servicio.Vehiculo.Id;

            //    _direccionService.Guardar(servicio.DireccionInicio);
            //    servicioRepo.DireccionInicioId = servicio.DireccionInicio.Id;

            //    _direccionService.Guardar(servicio.DireccionDestino);
            //    servicioRepo.DireccionDestinoId = servicio.DireccionDestino.Id;

            //    servicioRepo.FechaModificacion = DateTime.Now;
            //    servicioRepo.Observacion = servicio.Observacion;

            //    if (servicio.Conductor == null)
            //        servicioRepo.ConductorId = null;
            //    else
            //        servicioRepo.ConductorId = servicio.Conductor.Id;

            //    if (servicio.Ruta == null)
            //        servicioRepo.RutaId = null;
            //    else
            //        servicioRepo.RutaId = servicio.Ruta.Id;

            //    servicioRepo.Notificado = false;

            //    _servicioRepositorio.Edit(servicioRepo);
            //}

            //_unitOfWork.Commit();

            //return servicioRepo;
        }

        public List<Servicio> Obtener()
        {
            var servicios = _servicioRepositorio.GetAll().ToList();
            return servicios;
        }

        public List<ResumenServiciosEstados> ObtenerResumenEstados(Expression<Func<Servicio, bool>> predicate)
        {
            var servicios = _servicioRepositorio.FindBy(predicate)
                 .GroupBy(s => s.Estado)
                .Select(s => new ResumenServiciosEstados
                {
                    Estado = s.FirstOrDefault().Estado,
                    Total = s.Count()
                })
                .ToList();
            return servicios;
        }

        public List<Servicio> ObtenerPorDescripcion(string descripcion)
        {
            throw new NotImplementedException();
        }

        public ServicioDto ObtenerPorId(int id)
        {
            var servicio = Obtener(s => s.Id == id).FirstOrDefault();
            return servicio;
        }

        public List<ServicioDto> Obtener(Expression<Func<Servicio, bool>> predicate)
        {
            var servicios = _servicioRepositorio
                .FindBy(predicate)
                .Select(s => new ServicioDto
                {
                    Id = s.Id,
                    EstadoCodigo = s.EstadoCodigo,
                    Estado = s.Estado,
                    SucursalId = s.SucursalId,
                    TipoServicioId = s.TipoServicioId,
                    Fecha = s.Fecha,
                    Hora = s.Hora,
                    Radicado = s.Radicado,
                    AsignadoPor = s.AsignadoPor,
                    VehiculoId = s.VehiculoId,
                    Vehiculo = s.Vehiculo,
                    AseguradoraId = s.AseguradoraId,
                    Aseguradora = s.Aseguradora,
                    AseguradoId = s.AseguradoId,
                    Observacion = s.Observacion,
                    Asegurado = s.Asegurado,
                    DireccionInicioId = s.DireccionInicioId,
                    DireccionDestinoId = s.DireccionDestinoId,
                    ConductorId = s.ConductorId,
                    RutaId = s.RutaId,
                    UsuarioRegistro = new ApplicationUserDto { Nombre = s.UsuarioRegistro.Nombre },
                    FechaRegistro = s.FechaRegistro,
                    UsuarioModificacion = new ApplicationUserDto { Nombre = s.UsuarioModificacion.Nombre },
                    FechaModificacion = s.FechaModificacion,
                    Notificado = s.Notificado,

                })
                .ToList()
                .OrderBy(s => s.Fecha).ToList();


            List<int> idsDirecciones = servicios.Where(s => s.DireccionInicioId != null)
                .Select(s => (int)s.DireccionInicioId).ToList();
            idsDirecciones.AddRange(servicios.Where(s => s.DireccionDestinoId != null)
                .Select(s => (int)s.DireccionDestinoId).ToList());

            List<Direccion> direcciones = _direccionRepositorio.FindBy(d => idsDirecciones.Contains(d.Id)).ToList();

            List<SucursalDto> sucursales = _sucursalRepositorio.All
                .Select(s => new SucursalDto { Id = s.Id, Descripcion = s.Descripcion }).ToList();

            List<TipoServicioDto> tiposServicio = _tipoServicioRepositorio.All.Select(t => new TipoServicioDto
            {
                Id = t.Id,
                Descripcion = t.Descripcion,
                ConceptosPagos = t.ConceptosPagos,
                RequiereSeguimiento = t.RequiereSeguimiento
            }).ToList();

            List<int> idsConductores = servicios.Where(s => s.ConductorId != null).Select(s => (int)s.ConductorId).ToList();
            idsConductores.AddRange(servicios.Where(s => s.RutaId != null).Select(s => (int)s.RutaId).ToList());

            List<Conductor> conductores = _conductorRepositorio.FindBy(c => idsConductores.Contains(c.Id)).ToList();

            List<int> idsServicios = servicios.Select(s => s.Id).ToList();

            List<MovimientoDto> movimientos = _movimientoRepositorio
                .FindBy(m => idsServicios.Contains(m.ServicioId))
                .Select(m => new MovimientoDto
                {
                    Id = m.Id,
                    ServicioId = m.ServicioId,
                    Valor = m.Valor,
                    ConceptoCodigo = m.ConceptoCodigo,
                    Documento = new DocumentoDto { Numero = m.Documento.Numero, Tipo = m.Documento.Tipo },
                    Concepto = new ServicioConceptoDto
                    { Codigo = m.Concepto.Codigo, Descripcion = m.Concepto.Descripcion },
                    //ProveedorId = m.ProveedorId,
                    //ClienteId = m.ClienteId,
                    DocumentoId = m.DocumentoId,
                    FechaRegistro = m.FechaRegistro,
                    UsuarioRegistroId = m.UsuarioRegistroId,
                    FechaModificacion = m.FechaModificacion,
                    UsuarioModificacionId = m.UsuarioModificacionId,
                    Aprobado = m.Aprobado,
                }).ToList();


            List<SeguimientoDto> seguimientos = _seguimientoRepositorio
                .FindBy(s => idsServicios.Contains(s.ServicioId))
                .Select(sg => new SeguimientoDto
                {
                    Id = sg.Id,
                    Fecha = sg.Fecha,
                    NuevoEstado = sg.Estado.Descripcion,
                    Observacion = sg.Observacion,
                    UsuarioRegistro = new ApplicationUserDto { Nombre = sg.UsuarioRegistro.Nombre },
                    ServicioId = sg.ServicioId
                })
                .OrderBy(sg => sg.Fecha)
                .ToList();

            servicios.ForEach(servicio =>
            {
                if (servicio.DireccionInicioId != null)
                    servicio.DireccionInicio = direcciones.FirstOrDefault(d => d.Id == servicio.DireccionInicioId);

                if (servicio.DireccionDestinoId != null)
                    servicio.DireccionDestino = direcciones.FirstOrDefault(d => d.Id == servicio.DireccionDestinoId);

                if (servicio.SucursalId != null)
                    servicio.Sucursal = sucursales.FirstOrDefault(s => s.Id == servicio.SucursalId);

                if (servicio.TipoServicioId != null)
                    servicio.TipoServicio = tiposServicio.FirstOrDefault(s => s.Id == servicio.TipoServicioId);

                servicio.Conductor = conductores.FirstOrDefault(c => c.Id == servicio.ConductorId);
                servicio.ConductorResumen = servicio.Conductor != null ? servicio.Conductor.ToResumen() : string.Empty;

                servicio.Ruta = conductores.FirstOrDefault(c => c.Id == servicio.RutaId);
                servicio.RutaResumen = servicio.Ruta != null ? servicio.Ruta.ToResumen() : string.Empty;

                servicio.Movimientos = movimientos.Where(m => m.ServicioId == servicio.Id).ToList();
                servicio.Seguimientos = seguimientos.Where(s => s.ServicioId == servicio.Id).ToList();
            });

            return servicios;
        }

        public void ActualizarConsecutivo(int idServicio, string nuevoConsecutivo)
        {
            _servicioRepositorio.Update(new Servicio { Id = idServicio, Radicado = nuevoConsecutivo }, s => s.Radicado);
            _unitOfWork.Commit();
        }

        public void CambioEstado(int idServicio, Estado nuevoEstado)
        {
            var servicioRepo = _servicioRepositorio.FindBy(s => s.Id == idServicio).FirstOrDefault();
            servicioRepo.EstadoCodigo = nuevoEstado.Codigo;
            _servicioRepositorio.Edit(servicioRepo);
            _unitOfWork.Commit();
        }

        public void Cerrar(List<Servicio> servicios)
        {
            servicios.ForEach(servicio =>
            {
                var nuevoEstado = string.Empty;
                switch (servicio.EstadoCodigo)
                {
                    case "TE":
                        nuevoEstado = "CR";
                        break;
                    case "CN":
                        nuevoEstado = "CF";
                        break;
                    case "FL":
                        nuevoEstado = "CF";
                        break;
                }
                servicio.EstadoCodigo = nuevoEstado;
                _servicioRepositorio.Update(servicio, s => s.EstadoCodigo);
            });
            _unitOfWork.Commit();
        }

        public void Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public MemoryStream GenerarExcelServiciosResumen(Periodo periodo)
        {
            var servicios = _servicioRepositorio.FindBy(s => s.Fecha > periodo.Inicio && s.Fecha < periodo.Fin && s.Estado.Codigo != "AN").OrderBy(s => s.Fecha).ToList();
            var serviciosResumen = GenerarServiciosResumen(servicios);
            var memoryExcel = ExcelUtilities<ServicioResumen>.Export(serviciosResumen);
            return memoryExcel;
        }

        private List<ServicioResumen> GenerarServiciosResumen(List<Servicio> servicios)
        {
            var serviciosResumen = new List<ServicioResumen>();
            servicios.ForEach(f =>
            {
                var servicio = new ServicioResumen
                {
                    hora = f.Fecha.ToString("HH:mm"),
                    fecha = f.Fecha.ToString("dd/MM/yyyy"),
                    codigo = f.Radicado,
                    aseguradora = (f.Aseguradora != null) ?
                            f.Aseguradora.Nombre : "",
                    asegurado = (f.Asegurado != null) ? (
                            f.Asegurado.Nombre +
                            (!string.IsNullOrEmpty(f.Asegurado.Telefono1) ? " " + f.Asegurado.Telefono1 : "") +
                            (!string.IsNullOrEmpty(f.Asegurado.Telefono2) ? " " + f.Asegurado.Telefono2 : "")) : "",

                    vehiculo = (f.Vehiculo != null) ? (
                            f.Vehiculo.Placa +
                            (!string.IsNullOrEmpty(f.Vehiculo.Descripcion) ? " " + f.Vehiculo.Descripcion : "")) : "",

                    origen = (f.DireccionInicio != null) ? (
                            f.DireccionInicio.Descripcion +
                            (!string.IsNullOrEmpty(f.DireccionInicio.Barrio) ? " " + f.DireccionInicio.Barrio : "") +
                            (!string.IsNullOrEmpty(f.DireccionInicio.Ciudad.Nombre) ? " " + f.DireccionInicio.Ciudad.Nombre : "")) : "",

                    destino = (f.DireccionDestino != null) ? (
                            f.DireccionDestino.Descripcion +
                            (!string.IsNullOrEmpty(f.DireccionDestino.Barrio) ? " " + f.DireccionDestino.Barrio : "") +
                            (!string.IsNullOrEmpty(f.DireccionDestino.Ciudad.Nombre) ? " " + f.DireccionDestino.Ciudad.Nombre : "")) : "",

                    conductor = (f.Conductor != null) ?
                            (!string.IsNullOrEmpty(f.Conductor.Nombre) ? f.Conductor.Nombre : "") +
                            (!string.IsNullOrEmpty(f.Conductor.Telefono1) ? " " + f.Conductor.Telefono1 : "") : "",

                    ruta = (f.Ruta != null) ?
                            (!string.IsNullOrEmpty(f.Ruta.Nombre) ? f.Ruta.Nombre : "") +
                            (!string.IsNullOrEmpty(f.Ruta.Telefono1) ? " " + f.Ruta.Telefono1 : "") : "",

                    asignadoPor = f.AsignadoPor,
                    estado = f.Estado.Descripcion,
                    tipoServicio = f.TipoServicio.Descripcion,
                    observacion = f.Observacion
                };
                serviciosResumen.Add(servicio);
            });
            return serviciosResumen;
        }

        #region Notificaciones
        public string NotificarServiciosARuta(ICollection<Servicio> servicios)
        {
            IEnumerable<Conductor> Rutas = servicios.DistinctBy(s => s.RutaId).Select(c => c.Ruta);


            string respuesta = string.Empty;
            var rutasValidos = Rutas.Where(c => !string.IsNullOrEmpty(c.Email));

            if (Rutas.Where(c => string.IsNullOrEmpty(c.Email)).Count() > 0)
            {
                respuesta = "Existen conductores de ruta sin un email asignado";
            }

            if (rutasValidos.Count() > 0)
            {
                var correosDeServiciosPorRuta = CrearCorreosDeServiciosPorRuta(rutasValidos, servicios);
                _correoService.Enviar(correosDeServiciosPorRuta);
                respuesta = string.IsNullOrEmpty(respuesta) ? "Ruta notificada correctamente." : ", Ruta notificada correctamente.";
            }

            return respuesta;
        }

        public string NotificarServiciosAConductor(ICollection<Servicio> servicios)
        {
            IEnumerable<Conductor> Conductores = servicios.DistinctBy(s => s.ConductorId).Select(c => c.Conductor);

            string respuesta = string.Empty;
            if (Conductores.Where(c => string.IsNullOrEmpty(c.Email)).Count() > 0)
            {
                respuesta = "Existen conductores sin un email asignado";
            }

            var conductoresValidos = Conductores.Where(c => !string.IsNullOrEmpty(c.Email));
            if (conductoresValidos.Count() > 0)
            {
                var correosDeServiciosPorConductor = CrearCorreosDeServiciosPorConductor(conductoresValidos, servicios);
                _correoService.Enviar(correosDeServiciosPorConductor);
                respuesta = string.IsNullOrEmpty(respuesta) ? "Conductores notificados correctamente." : ", Conductores notificados correctamente.";
                servicios.ToList().ForEach(servicio =>
                {
                    _servicioRepositorio.Update(new Servicio { Id = servicio.Id, Notificado = true }, s => s.Notificado);
                });
                _unitOfWork.Commit();
            }
            return respuesta;
        }

        private List<Correo> CrearCorreosDeServiciosPorConductor(IEnumerable<Conductor> conductores, ICollection<Servicio> servicios)
        {
            var correosDeServiciosPorConductor = new List<Correo>();
            foreach (var conductor in conductores)
            {
                string mensaje = "<p>Servicios asignados a: " + conductor.Nombre + "</p>";
                string destinatarios = string.Empty;
                var Servicios = servicios.Where(s => s.ConductorId == conductor.Id);
                destinatarios += string.Format("\"{0}\"<{1}>;", conductor.Nombre, conductor.Email);

                mensaje += ConstruirHtmlServicios(Servicios);

                var correo = new Correo()
                {
                    CORdestinatarios = destinatarios,
                    CORasunto = "Servicios de conductor elegido asignados",
                    CORmensajeHTML = mensaje
                };
                correosDeServiciosPorConductor.Add(correo);
            }
            return correosDeServiciosPorConductor;
        }

        private List<Correo> CrearCorreosDeServiciosPorRuta(IEnumerable<Conductor> rutas, ICollection<Servicio> servicios)
        {
            var correosDeServiciosPorConductor = new List<Correo>();
            foreach (var ruta in rutas)
            {
                string mensaje = "<p>Servicios de la ruta: " + ruta.Nombre + "</p>";
                string destinatarios = string.Empty;
                var Servicios = servicios.Where(s => s.RutaId == ruta.Id);
                destinatarios += string.Format("\"{0}\"<{1}>;", ruta.Nombre, ruta.Email);

                mensaje += ConstruirHtmlServicios(Servicios);

                var correo = new Correo()
                {
                    CORdestinatarios = destinatarios,
                    CORasunto = "Servicios asignados a su ruta",
                    CORmensajeHTML = mensaje
                };
                correosDeServiciosPorConductor.Add(correo);
            }
            return correosDeServiciosPorConductor;
        }

        private string ConstruirHtmlServicios(IEnumerable<Servicio> Servicios)
        {
            string mensaje = @"
                                <style>
                                    table {
                                        border-collapse: collapse;
                                        color:darkgray;
                                        font-size:12px;
                                        width:100%;
                                    }

                                    table, th, td {
                                        border: 1px solid lightgray;
                                        padding: 2px;
                                    }
                                </style>

                                <table>
                                    <thead>
                                        <tr>
                                            <th>Id</th>
                                            <th>Feha y hora</th>
                                            <th>Consecutivo</th>
                                            <th>Tipo</th>
                                            <th>Red</th>

                                            <th>Asegurado</th>
                                            <th>Vehiculo</th>

                                            <th>Origen</th>
                                            <th>Destino</th>
                                            <th>Conductor</th>
                                            <th>Ruta</th>
                                            <th>Observación</th>
                                        </tr>
                                    </thead>
                                    <tbody>";

            foreach (var servicio in Servicios)
            {
                if (servicio.Ruta == null)
                    servicio.Ruta = new Conductor();
                if (servicio.Conductor == null)
                    servicio.Conductor = new Conductor();

                mensaje += string.Format(@"
                                        <tr>
                                            <td>{0}</td>
                                            <td>{1}</td>
                                            <td>{2}</td>
                                            <td>{11}</td>
                                            <td>{3}</td>

                                            <td>{4}</td>
                                            <td>{5}</td>

                                            <td>{6}</td>
                                            <td>{7}</td>
                                            <td>{8}</td>
                                            <td>{9}</td>
                                            <td>{10}</td>
                                        </tr>
                    ",
                    servicio.Id.ToString(),
                    servicio.Fecha.ToString("HH:mm dd/MM/yyyy"),
                    servicio.Radicado,
                    servicio.Aseguradora.Nombre,

                    servicio.Asegurado.Nombre +
                    (!string.IsNullOrEmpty(servicio.Asegurado.Telefono1) ? ", " + servicio.Asegurado.Telefono1 : "") +
                    (!string.IsNullOrEmpty(servicio.Asegurado.Telefono2) ? ", " + servicio.Asegurado.Telefono2 : ""),


                    servicio.Vehiculo.Placa +
                    (!string.IsNullOrEmpty(servicio.Vehiculo.Descripcion) ? ", " + servicio.Vehiculo.Descripcion : ""),


                    servicio.DireccionInicio.Descripcion +
                    (!string.IsNullOrEmpty(servicio.DireccionInicio.Barrio) ? ", " + servicio.DireccionInicio.Barrio : "") +
                    (!string.IsNullOrEmpty(servicio.DireccionInicio.Ciudad.Nombre) ? ", " + servicio.DireccionInicio.Ciudad.Nombre : ""),

                    servicio.DireccionDestino.Descripcion +
                    (!string.IsNullOrEmpty(servicio.DireccionDestino.Barrio) ? ", " + servicio.DireccionDestino.Barrio : "") +
                    (!string.IsNullOrEmpty(servicio.DireccionDestino.Ciudad.Nombre) ? ", " + servicio.DireccionDestino.Ciudad.Nombre : ""),

                    (servicio.Conductor != null) ?
                    (!string.IsNullOrEmpty(servicio.Conductor.Nombre) ? servicio.Conductor.Nombre : "") +
                    (!string.IsNullOrEmpty(servicio.Conductor.Telefono1) ? ", " + servicio.Conductor.Telefono1 : "") : "",

                    (servicio.Ruta != null) ?
                    (!string.IsNullOrEmpty(servicio.Ruta.Nombre) ? servicio.Ruta.Nombre : "") +
                    (!string.IsNullOrEmpty(servicio.Ruta.Telefono1) ? ", " + servicio.Ruta.Telefono1 : "") : "",

                    servicio.Observacion,
                    servicio.TipoServicio.Descripcion

                    );
            }
            return mensaje + "</tbody></table>";
        }

        public string ObtenerHtmlServiciosAConductor(ICollection<Servicio> servicios)
        {
            IEnumerable<Conductor> conductores = servicios.DistinctBy(s => s.ConductorId).Select(c => c.Conductor);
            string html = string.Empty;
            foreach (var conductor in conductores)
            {
                string mensaje = "<p>Servicios asignados a: " + conductor.Nombre + "</p>";
                var Servicios = servicios.Where(s => s.ConductorId == conductor.Id);
                mensaje += ConstruirHtmlServicios(Servicios);
                html = html + mensaje;
            }
            return html;
        }

        public string ObtenerHtmlServiciosARuta(ICollection<Servicio> servicios)
        {
            IEnumerable<Conductor> Rutas = servicios.DistinctBy(s => s.RutaId).Select(c => c.Ruta);
            string html = string.Empty;
            foreach (var ruta in Rutas)
            {
                string mensaje = "<p>Servicios de la ruta: " + ruta.Nombre + "</p>";
                var Servicios = servicios.Where(s => s.RutaId == ruta.Id);
                mensaje += ConstruirHtmlServicios(Servicios);
                html = html + mensaje;
            }
            return html;
        }

        Servicio ICommonInterface<Servicio>.ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public partial interface IServicioService
    {
        ServicioDto ObtenerPorId(int id);
        List<ServicioDto> Obtener(Expression<Func<Servicio, bool>> predicate);
        List<ResumenServiciosEstados> ObtenerResumenEstados(Expression<Func<Servicio, bool>> predicate);
        void CambioEstado(int idServicio, Estado nuevoEstado);
        void ActualizarConsecutivo(int idServicio, string nuevoConsecutivo);
        string NotificarServiciosAConductor(ICollection<Servicio> servicios);
        string ObtenerHtmlServiciosAConductor(ICollection<Servicio> servicios);

        string NotificarServiciosARuta(ICollection<Servicio> servicios);
        string ObtenerHtmlServiciosARuta(ICollection<Servicio> servicios);
        MemoryStream GenerarExcelServiciosResumen(Periodo periodo);

        void Cerrar(List<Servicio> servicios);
    }

    public class ResumenServiciosEstados
    {
        public Estado Estado { get; internal set; }
        public int Total { get; internal set; }
    }
    public class ServicioResumen
    {
        public string fecha { get; set; }
        public string hora { get; set; }
        public string aseguradora { get; set; }
        public string codigo { get; set; }
        public string asignadoPor { get; set; }
        public string asegurado { get; set; }
        public string vehiculo { get; set; }
        public string origen { get; set; }
        public string destino { get; set; }
        public string conductor { get; set; }
        public string ruta { get; set; }
        public string estado { get; set; }
        public string tipoServicio { get; internal set; }
        public string observacion { get; internal set; }
    }

    //public class ServicioConcepto
    //{
    //    public int Id { get; set; }
    //    public Conductor Proveedor { get; set; }
    //    public string Valor { get; set; }
    //    public string Concepto { get; set; }
    //    public Aseguradora Aseguradora { get; set; }
    //    public Asegurado Cliente { get; set; }
    //    public Direccion DireccionInicio { get; set; }
    //    public Direccion DireccionDestino { get; set; }
    //    public string Radicado { get; set; }
    //    public DateTime Fecha { get; set; }
    //}
}
