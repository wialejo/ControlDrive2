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
    public class DocumentosService : IDocumentosService
    {
        private readonly IEntityBaseRepository<Documento> _documentoRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityBaseRepository<Servicio> _servicioRepositorio;
        private IEntityBaseRepository<Empresa> _empresaRepositorio;
        private IMovimientosService _movimientoService;

        public DocumentosService(IEntityBaseRepository<Documento> documentoRepositorio, IMovimientosService movimientoService, IEntityBaseRepository<Empresa> empresaRepositorio,  IEntityBaseRepository<Servicio> servicioRepositorio, IUnitOfWork unitOfWork)
        {
            _documentoRepositorio = documentoRepositorio;
            _empresaRepositorio = empresaRepositorio;
            _servicioRepositorio = servicioRepositorio;
            _movimientoService = movimientoService;
            _unitOfWork = unitOfWork;
        }

        public List<Documento> Obtener(Expression<Func<Documento, bool>> predicate)
        {
            var documentos = _documentoRepositorio.FindByIncluding(predicate, d => d.Cliente).ToList();
            return documentos;
        }

        public List<Documento> Obtener()
        {
            var documentos = _documentoRepositorio.AllIncluding(d => d.Cliente).ToList();
            return documentos;
        }

        public void Guardar(Documento documento)
        {
            if (documento.Id != 0)
            {
                throw new Exception("No se puede editar");
            }
            else
            {
                var documentoRepo = new Documento();
                documentoRepo.Tipo = "FA";
                documentoRepo.Numero = documento.Numero;
                documentoRepo.Fecha = DateTime.Now;

                var empresa = _empresaRepositorio.GetAll().FirstOrDefault();
                if (empresa == null)
                {
                    throw new Exception("No se ha registrado una empresa.");
                }

                documentoRepo.Concepto = "Servicios de conductor elegido de la ciudad de " + empresa.Ciudad;
                documentoRepo.ClienteId = documento.ClienteId;

                var nuevosMovimientos = new List<Movimiento>();
                decimal valorFactura = 0;
                documento.Movimientos.ForEach(movimiento =>
                {
                    var movimientoGuardado = _movimientoService.ObtenerPorId(movimiento.Id);
                    valorFactura = valorFactura + movimientoGuardado.Valor;
                    nuevosMovimientos.Add(movimientoGuardado);
                });

                documentoRepo.Valor = valorFactura;
                documentoRepo.Movimientos = nuevosMovimientos;
                documentoRepo.FechaRegistro = DateTime.Now;
                documentoRepo.UsuarioRegistroId = documento.UsuarioRegistroId;

                _documentoRepositorio.Add(documentoRepo);
            }
            _unitOfWork.Commit();
        }
    }

    public interface IDocumentosService
    {
        void Guardar(Documento documento);
        List<Documento> Obtener(Expression<Func<Documento, bool>> predicate);
        List<Documento> Obtener();
    }
}
