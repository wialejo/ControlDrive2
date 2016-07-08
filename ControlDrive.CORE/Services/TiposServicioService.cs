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
    public class TiposServicioService : ITiposServiciosService
    {

        private readonly IEntityBaseRepository<TipoServicio> _tipoServicioRepositorio;
        private readonly IUnitOfWork _unitOfWork;

        public TiposServicioService(IEntityBaseRepository<TipoServicio> tipoServicioRepositorio, IUnitOfWork unitOfWork)
        {
            _tipoServicioRepositorio = tipoServicioRepositorio;
            _unitOfWork = unitOfWork;
        }

        public List<TipoServicioDto> Obtener()
        {
            var estados = _tipoServicioRepositorio.GetAll()
                .Select(t =>  new TipoServicioDto { Id = t.Id, Descripcion = t.Descripcion, RequiereSeguimiento = t.RequiereSeguimiento,  ConceptosPagos = t.ConceptosPagos  }
                
                ).AsQueryable().ToList();
            return estados;
        }
    }
    public interface ITiposServiciosService {
        List<TipoServicioDto> Obtener();
    }
}
