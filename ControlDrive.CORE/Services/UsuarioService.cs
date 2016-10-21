using ControlDrive.Core.Infraestructura;
using ControlDrive.Core.Modelos;
using ControlDrive.CORE.Modelos;
using ControlDrive.CORE.Repositorios;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ControlDrive.CORE.Services
{
    public class UsuarioService : IUsuarioService
    {

        private readonly IEntityBaseRepository<ApplicationUser> _usuarioRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityBaseRepository<Sucursal> _sucursalRepositorio;
        private IEntityBaseRepository<IdentityUserRole> _rolUserRepositorio;
        private IEntityBaseRepository<IdentityRole> _rolRepositorio;

        public UsuarioService(IEntityBaseRepository<Sucursal> sucursalRepositorio,
            IEntityBaseRepository<ApplicationUser> usuarioRepositorio, IEntityBaseRepository<IdentityRole> rolRepositorio, IEntityBaseRepository<IdentityUserRole> rolUserRepositorio, IUnitOfWork unitOfWork)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sucursalRepositorio = sucursalRepositorio;
            _unitOfWork = unitOfWork;
            _rolUserRepositorio = rolUserRepositorio;
            _rolRepositorio = rolRepositorio;
        }



        public void AsignarSucursal(string idUsuario, int idSucursal)
        {

            var usuario = _usuarioRepositorio.FindBy(u => u.Id == idUsuario).FirstOrDefault();

            if (usuario.Sucursales.Where(s => s.Id == idSucursal).Count() == 0)
            {
                var sucursal = _sucursalRepositorio.FindBy(e => e.Id == idSucursal).FirstOrDefault();
                usuario.Sucursales.Add(sucursal);
                _usuarioRepositorio.Edit(usuario);
                _unitOfWork.Commit();
            }
        }
        public void DesAsignarSucursal(string idUsuario, int idSucursal)
        {
            var usuario = _usuarioRepositorio.FindByIncluding(u => u.Id == idUsuario, s => s.Sucursales).FirstOrDefault();
            var sucursalAEliminar = usuario.Sucursales.Where(s => s.Id.Equals(idSucursal)).FirstOrDefault();
            usuario.Sucursales.Remove(sucursalAEliminar);

            _usuarioRepositorio.Edit(usuario);
            _unitOfWork.Commit();
        }

        public void AsignarRol(string idUsuario, string idRol)
        {
            var userRol = _rolUserRepositorio.FindBy(r => r.RoleId == idRol && r.UserId == idUsuario).FirstOrDefault();
            if (userRol == null)
            {
                _rolUserRepositorio.Add(new IdentityUserRole { RoleId = idRol, UserId = idUsuario });
                _unitOfWork.Commit();
            }
        }

        public void DesAsignarRol(string idUsuario, string idRol)
        {
            var userRol = _rolUserRepositorio.FindBy(r => r.RoleId == idRol && r.UserId == idUsuario).FirstOrDefault();
            if (userRol != null)
            {
                _rolUserRepositorio.Delete(userRol);
                _unitOfWork.Commit();
            }
        }


        public List<ApplicationUserDto> Obtener()
        {
            return ObtenerPor(a => a.Id != "");
        }
        public ApplicationUserDto ObtenerPorId(string id)
        {
            return ObtenerPor(a => a.Id == id).FirstOrDefault();
        }
        public List<ApplicationUserDto> ObtenerPor(Expression<Func<ApplicationUser, bool>> predicado)
        {
            var usuarios = _usuarioRepositorio.FindByIncluding(predicado, u => u.Sucursales, r => r.Roles)
                    .Select(u => new ApplicationUserDto
                    {
                        Id = u.Id,
                        Nombre = u.Nombre,
                        Email = u.Email,
                        UserName = u.UserName,
                        Sucursales = u.Sucursales,
                        Rol = u.Rol
                    }).ToList();
            return usuarios;
        }

        public void Guardar(ApplicationUser usuario)
        {
            var usuarioRepo = new ApplicationUser();
            var nuevoUsuario = string.IsNullOrEmpty(usuario.Id);
            if (!nuevoUsuario)
            {
                usuarioRepo = _usuarioRepositorio.FindBy(u => u.Id == usuario.Id).FirstOrDefault();
            }
            usuarioRepo.Nombre = usuario.Nombre;
            usuarioRepo.Email = usuario.Email;
            usuarioRepo.Rol = usuario.Rol;


            if (nuevoUsuario)
            {
                usuario.Sucursales.ToList().ForEach(s =>
               {
                   _sucursalRepositorio.Unchanged(s);
                   usuarioRepo.Sucursales.Add(s);
               });
            }
            _usuarioRepositorio.Edit(usuarioRepo);
            _unitOfWork.Commit();
        }
    }

    public interface IUsuarioService
    {
        void AsignarSucursal(string idUsuario, int idSucursal);
        void DesAsignarSucursal(string idUsuario, int idSucursal);
        void AsignarRol(string idUsuario, string idRol);
        void DesAsignarRol(string idUsuario, string idRol);
        void Guardar(ApplicationUser usuario);
        List<ApplicationUserDto> Obtener();
        ApplicationUserDto ObtenerPorId(string id);
    }
}

