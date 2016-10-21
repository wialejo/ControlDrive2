using ControlDrive.Core.Infraestructura;
using ControlDrive.Core.Modelos;
using ControlDrive.CORE.Modelos;
using ControlDrive.CORE.Repositorios;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDrive.CORE.Services
{
    public class RolService : IRolService
    {

        private readonly IEntityBaseRepository<ApplicationUser> _usuarioRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityBaseRepository<IdentityRole> _rolRepositorio;
        private IEntityBaseRepository<IdentityUserRole> _rolUserRepositorio;

        public RolService(IEntityBaseRepository<IdentityRole> rolRepositorio, IEntityBaseRepository<IdentityUserRole> rolUserRepositorio,
            IEntityBaseRepository<ApplicationUser> usuarioRepositorio, IUnitOfWork unitOfWork)
        {
            _rolUserRepositorio = rolUserRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _rolRepositorio = rolRepositorio;
            _unitOfWork = unitOfWork;
        }

        public List<IdentityRole> Obtener()
        {
            return _rolRepositorio.GetAll()
                //.Select(u => new IdentityRoleDto { Id = u.Id, Nombre = u.Nombre, Email = u.Email, UserName = u.UserName, Roles = u.Roles })
                .ToList();
        }

        public IdentityRole ObtenerPorId(string id)
        {
            return _rolRepositorio.FindByIncluding(a => a.Id == id)
                    //.Select(u => new ApplicationUserDto { Id = u.Id, Nombre = u.Nombre, Email = u.Email, UserName = u.UserName, Roles = u.Roles, Roles = u.Roles })
                    .FirstOrDefault();
        }

        public IdentityRole Guardar(string nombre)
        {
            var repoRol = _rolRepositorio.FindBy(n => n.Name == nombre).FirstOrDefault();


            if (repoRol == null)
            {
                repoRol = new IdentityRole();
                repoRol.Id = Guid.NewGuid().ToString();
                repoRol.Name = nombre;
                repoRol = _rolRepositorio.Add(repoRol);
            }
            else
            {

                repoRol.Name = nombre;
                _rolRepositorio.Edit(repoRol);
            }
            _unitOfWork.Commit();

            return repoRol;
        }
    }

    public interface IRolService
    {
        IdentityRole Guardar(string nombre);
        List<IdentityRole> Obtener();
        IdentityRole ObtenerPorId(string id);
    }
}

