﻿using ControlDrive.Core.Infraestructura;
using ControlDrive.Core.Modelos;
using ControlDrive.CORE.Extensions;
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
    public class DocumentosService : IDocumentosService
    {
        private readonly IEntityBaseRepository<Documento> _documentoRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityBaseRepository<Servicio> _servicioRepositorio;
        private IEntityBaseRepository<Empresa> _empresaRepositorio;
        private IMovimientosService _movimientoService;

        public DocumentosService(IEntityBaseRepository<Documento> documentoRepositorio, IMovimientosService movimientoService, IEntityBaseRepository<Empresa> empresaRepositorio, IEntityBaseRepository<Servicio> servicioRepositorio, IUnitOfWork unitOfWork)
        {
            _documentoRepositorio = documentoRepositorio;
            _empresaRepositorio = empresaRepositorio;
            _servicioRepositorio = servicioRepositorio;
            _movimientoService = movimientoService;
            _unitOfWork = unitOfWork;
        }

        public List<Documento> Obtener(Expression<Func<Documento, bool>> predicate)
        {
            var documentos = _documentoRepositorio.FindByIncluding(predicate, d => d.Cliente, d => d.Proveedor).ToList();
            return documentos;
        }

        public List<Documento> Obtener()
        {
            var documentos = _documentoRepositorio.AllIncluding(d => d.Cliente).ToList();
            return documentos;
        }

        public void Guardar(Documento documento)
        {
            var documentoRepo = new Documento();
            if (documento.Id != 0)
            {
                documentoRepo = _documentoRepositorio.FindBy(d => d.Id == documento.Id).FirstOrDefault();
                documentoRepo.Numero = documento.Numero;
                _documentoRepositorio.Edit(documentoRepo);
            }
            else
            {
                documentoRepo.Tipo = documento.Tipo;
                documentoRepo.Numero = documento.Numero;
                documentoRepo.Fecha = DateTime.Now;

                if (documento.Tipo == "FA")
                {
                    var empresa = _empresaRepositorio.GetAll().FirstOrDefault();
                    if (empresa == null)
                    {
                        throw new Exception("No se ha registrado una empresa.");
                    }

                    documentoRepo.Concepto = "Servicios de conductor elegido de la ciudad de " + empresa.Ciudad;
                    documentoRepo.ClienteId = documento.ClienteId;
                }
                else
                {
                    var numero = _documentoRepositorio.FindBy(d => d.Tipo == "CP").Select(d => d.Numero).FirstOrDefault();
                    int nuevoNumero = 0;
                    int.TryParse(numero, out nuevoNumero);
                    nuevoNumero = nuevoNumero + 1;

                    documentoRepo.Numero = nuevoNumero.ToString();
                    documentoRepo.Concepto = "Servicios de conductor elegido y/o ruta";
                    documentoRepo.ProveedorId = documento.ProveedorId;
                }

                var nuevosMovimientos = new List<Movimiento>();
                decimal valorDocumento = 0;
                documento.Movimientos.ForEach(movimiento =>
                {
                    var movimientoGuardado = _movimientoService.ObtenerPorId(movimiento.Id);
                    valorDocumento = valorDocumento + movimientoGuardado.Valor;
                    nuevosMovimientos.Add(movimientoGuardado);
                });

                documentoRepo.Valor = valorDocumento;
                documentoRepo.Movimientos = nuevosMovimientos;
                documentoRepo.FechaRegistro = DateTime.Now;
                documentoRepo.UsuarioRegistroId = documento.UsuarioRegistroId;

                _documentoRepositorio.Add(documentoRepo);
            }
            _unitOfWork.Commit();
        }

        public List<DocumentoRelacionServicio> ObtenerRelacionServicios(List<Documento> documentos)
        {
            var documentosRelacionServicios = new List<DocumentoRelacionServicio>();
            documentos.ForEach(documento =>
            {
                var docRepo = _documentoRepositorio.FindByIncluding(d => d.Id == documento.Id, d => d.Movimientos)
                                .Select(s => new DocumentoDto
                                {
                                    Numero = s.Numero,
                                    Fecha = s.Fecha,
                                    Valor = s.Valor,
                                    Movimientos = s.Movimientos.Select(m => new MovimientoDto
                                    {
                                        Valor = m.Valor,
                                        Servicio = new ServicioDto
                                        {
                                            Radicado = m.Servicio.Radicado,
                                            Fecha = m.Servicio.Fecha,
                                            Vehiculo = m.Servicio.Vehiculo,
                                            DireccionInicio = m.Servicio.DireccionInicio,
                                            DireccionDestino = m.Servicio.DireccionDestino
                                        },
                                        Concepto = new ServicioConceptoDto { Codigo = m.Concepto.Codigo , Descripcion = m.Concepto.Descripcion }
                                    }).ToList()
                                })
                                .AsQueryable()
                                .FirstOrDefault();

                docRepo.Movimientos.ForEach(mov =>
                {
                    documentosRelacionServicios.Add(new DocumentoRelacionServicio
                    {
                        Numero = docRepo.Numero,
                        FechaDoc = docRepo.Fecha,
                        ValorDoc = docRepo.Valor,
                        ValorMov = mov.Valor,
                        Concepto = mov.Concepto.Descripcion,
                        ConsecutivoServicio = mov.Servicio.Radicado,
                        FechaServicio = mov.Servicio.Fecha,
                        VehiculoServicio = mov.Servicio.Vehiculo.ToResumen(),
                        OrigenServicio = mov.Servicio.DireccionInicio.ToResumen(),
                        DestinoServicio = mov.Servicio.DireccionDestino.ToResumen(),
                    });
                });
            });

            return documentosRelacionServicios;
        }
    }

    public interface IDocumentosService
    {
        void Guardar(Documento documento);
        List<Documento> Obtener(Expression<Func<Documento, bool>> predicate);
        List<Documento> Obtener();
        List<DocumentoRelacionServicio> ObtenerRelacionServicios(List<Documento> documentos);
    }

    public class DocumentoRelacionServicio
    {
        public string Numero { get; set; }
        public DateTime FechaDoc { get; set; }
        public decimal ValorDoc { get; set; }

        public decimal ValorMov { get; set; }

        public string ConsecutivoServicio { get; set; }
        public DateTime FechaServicio { get; set; }
        public string VehiculoServicio { get; set; }
        public string OrigenServicio { get; set; }
        public string DestinoServicio { get; set; }
        public string Concepto { get; internal set; }
    }
}
