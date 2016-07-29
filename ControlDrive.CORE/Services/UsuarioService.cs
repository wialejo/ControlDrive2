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
    public class UsuarioService : IUsuarioService
    {

        private readonly IEntityBaseRepository<ApplicationUser> _usuarioRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityBaseRepository<Sucursal> _sucursalRepositorio;

        public UsuarioService(IEntityBaseRepository<Sucursal> sucursalRepositorio,
            IEntityBaseRepository<ApplicationUser> usuarioRepositorio, IUnitOfWork unitOfWork)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sucursalRepositorio = sucursalRepositorio;
            _unitOfWork = unitOfWork;
        }

        public void AsignarSucursal(string idUsuario, int idSucursal)
        {

            var usuario = _usuarioRepositorio.FindBy(u => u.Id == idUsuario).FirstOrDefault();

            if (usuario.Sucursales.Where(s => s.Id == idSucursal).Count() == 0)
            {
                var sucursal = _sucursalRepositorio.FindBy(e => e.Id == idSucursal).FirstOrDefault();
                usuario.Sucursales.Add(sucursal);
                _usuarioRepositorio.Update(usuario);
                _unitOfWork.Commit();
            }
        }
        public void DesAsignarSucursal(string idUsuario, int idSucursal)
        {
            var usuario = _usuarioRepositorio.FindBy(u => u.Id == idUsuario).FirstOrDefault();
            var sucursal = _sucursalRepositorio.FindBy(e => e.Id == idSucursal).FirstOrDefault();
            usuario.Sucursales.Remove(sucursal);
            _usuarioRepositorio.Update(usuario, u => u.Sucursales);
            _unitOfWork.Commit();
        }

        public List<ApplicationUserDto> Obtener()
        {
            return _usuarioRepositorio.FindByIncluding(a => a.Id != "", u => u.Sucursales)
                .Select(u => new ApplicationUserDto { Id = u.Id, Nombre = u.Nombre, Email = u.Email, UserName = u.UserName, Sucursales = u.Sucursales })
                .ToList();
        }

        public ApplicationUserDto ObtenerPorId(string id)
        {
            return _usuarioRepositorio.FindByIncluding(a => a.Id == id, u => u.Sucursales)
                    .Select(u => new ApplicationUserDto { Id = u.Id, Nombre = u.Nombre, Email = u.Email, UserName = u.UserName, Sucursales = u.Sucursales })
                    .FirstOrDefault();
        }
    }

    public interface IUsuarioService
    {
        void AsignarSucursal(string idUsuario, int idSucursal);
        void DesAsignarSucursal(string idUsuario, int idSucursal);
        List<ApplicationUserDto> Obtener();
        ApplicationUserDto ObtenerPorId(string id);
    }
}

