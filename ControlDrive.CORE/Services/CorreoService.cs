using ControlDrive.CORE.Infraestructura;
using ControlDrive.CORE.Modelos;
using ControlDrive.CORE.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlDrive.CORE.Services
{
    public class CorreoService: ICorreoService
    {
        private readonly IEntityBaseRepository<Cuenta> _cuentaRepositorio;

        public CorreoService(IEntityBaseRepository<Cuenta> cuentaRepositorio)
        {
            _cuentaRepositorio = cuentaRepositorio;
        }

        public void Enviar(List<Correo> correos)
        {
            var cuenta =  _cuentaRepositorio.GetAll().FirstOrDefault();
            var SmtpClient = Smtp.IniciarSmtpClient(cuenta);
            Smtp.EnviarMensajesAClienteSMTP(correos, cuenta, SmtpClient);
        }
    }

    public interface ICorreoService {
        void Enviar(List<Correo> correos);
    }
}
