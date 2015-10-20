using ControlDrive.CORE.Enums;
using ControlDrive.CORE.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ControlDrive.CORE.Infraestructura
{
    public class Smtp
    {
        public static void EnviarMensajesAClienteSMTP(List<Correo> correosAenviar, Cuenta cuenta, SmtpClient ClienteSMTP)
        {
            foreach (var correo in correosAenviar)
            {
                var mailMensaje = new MailMessage();
                try
                {
                    mailMensaje = ConstruirMailMensaje(correo, cuenta);
                    ClienteSMTP.Send(mailMensaje);
                    //DatosCorreos.RegistrarCambioEstado(correo, "EN", nombreServidor, null);
                }
                catch (SmtpFailedRecipientsException exc)
                {
                    throw exc;
                   //DatosCorreos.RegistrarCambioEstado(correo, "ER", nombreServidor, exc);
                }
                catch (SmtpFailedRecipientException exc)
                {
                    throw exc;
                    //DatosCorreos.RegistrarCambioEstado(correo, "ER", nombreServidor, exc);
                }
                catch (SmtpException exc)
                {
                    throw exc;
                    //DatosCorreos.RegistrarCambioEstado(correo, "ER", nombreServidor, exc);
                }
                catch (Exception exc)
                {
                    throw exc;
                    //DatosCorreos.RegistrarCambioEstado(correo, "ER", nombreServidor, exc);
                }
                finally
                {
                    mailMensaje.Dispose();
                }
            }
        }

        private static MailMessage ConstruirMailMensaje(Correo correoOrigen, Cuenta cuenta)
        {
            var Mensaje = new MailMessage();

            var from = new MailAddress(cuenta.CorreoSalida, cuenta.NombreMostrar, Encoding.UTF8);
            Mensaje.From = from;


            var MAC_PRIN = (MailAddressCollection)ObtenerDestinatarios(TipoDestinatario.Principal, correoOrigen);
            foreach (MailAddress MA in MAC_PRIN)
            {
                Mensaje.To.Add(MA);
            }

            var MAC_CC = (MailAddressCollection)ObtenerDestinatarios(TipoDestinatario.Copia, correoOrigen);
            foreach (MailAddress MA in MAC_CC)
            {
                Mensaje.CC.Add(MA);
            }

            var MAC_CCO = (MailAddressCollection)ObtenerDestinatarios(TipoDestinatario.CopiaOculta, correoOrigen);
            foreach (MailAddress MA in MAC_CCO)
            {
                Mensaje.Bcc.Add(MA);
            }

            #region Creación de adjuntos

            //if (correoOrigen.correosAdjuntos.Count > 0)
            //{
            //    foreach (var adjunto in correoOrigen.correosAdjuntos)
            //    {
            //        if (!string.IsNullOrEmpty(adjunto.CADrutaArchivo))
            //        {
            //            var attachment = new Attachment(adjunto.CADrutaArchivo.Replace("<RutaArchivo>", "").Replace("</RutaArchivo>", ""), MediaTypeNames.Application.Octet);
            //            Mensaje.Attachments.Add(attachment);
            //        }
            //    }
            //}

            #endregion

            #region Creación del cuerpo del mensaje

            //Mensaje.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(correoOrigen.CORmensajeTexto, Encoding.UTF8, "text/plain"));
            Mensaje.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(correoOrigen.CORmensajeHTML, Encoding.UTF8, "text/html"));
            Mensaje.Subject = correoOrigen.CORasunto;
            Mensaje.SubjectEncoding = Encoding.UTF8;

            #endregion

            #region Prioridad mensaje

            //switch (correoOrigen.CORprioridad)
            //{
            //    case "AL":
            //        Mensaje.Priority = MailPriority.High;
            //        break;
            //    case "NO":
            //        Mensaje.Priority = MailPriority.Normal;
            //        break;
            //    case "BA":
            //        Mensaje.Priority = MailPriority.Low;
            //        break;
            //    default:
            //        Mensaje.Priority = MailPriority.Normal;
            //        break;
            //}
            Mensaje.Priority = MailPriority.Normal;
            #endregion


            if (!string.IsNullOrEmpty(cuenta.CorreoRespuesta))
                Mensaje.ReplyToList.Add(new MailAddress(cuenta.CorreoRespuesta, cuenta.NombreMostrar, Encoding.UTF8));

            /* Depuracion: Enviar un mensaje si el mensaje se envio correctamente */
            Mensaje.DeliveryNotificationOptions = DeliveryNotificationOptions.Never;

            /* Confirmación de lectura */
            if (!string.IsNullOrEmpty(cuenta.CorreoConfirmacionLectura))
                Mensaje.Headers.Add("Disposition-Notification-To", cuenta.CorreoConfirmacionLectura);

            //Encabezado de mensaje. evitar filtros spam
            //  string messageId = Guid.NewGuid().ToString().Replace("-", "").ToLower() + this.CORcuenta.CCOusuario;
            Mensaje.Headers.Add("Message-ID", correoOrigen.Id.ToString());

            Mensaje.BodyEncoding = Encoding.UTF8;

            // Se agregó para omitir las validaciones de servidor que ejecuta google a servidores corporativos
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s
                    , X509Certificate certificate
                    , X509Chain chain
                    , SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
            return Mensaje;
        }

        private static MailAddressCollection ObtenerDestinatarios(TipoDestinatario Tipo, Correo correo)
        {
            MailAddressCollection MAC = new MailAddressCollection();

            try
            {
                string Lista = string.Empty;

                switch (Tipo)
                {
                    case TipoDestinatario.Principal:
                        Lista = correo.CORdestinatarios;
                        break;
                    //case TipoDestinatario.Copia:
                    //    Lista = correo.CORdestinatariosCopia;
                    //    break;
                    //case TipoDestinatario.CopiaOculta:
                    //    Lista = correo.CORdestinatariosCopiaOculta;
                    //    break;
                    default:
                        Lista = correo.CORdestinatarios;
                        break;
                }

                if (!string.IsNullOrEmpty(Lista))
                {
                    string[] Dest = Lista.Split(';');

                    foreach (string S in Dest)
                    {
                        if (!string.IsNullOrEmpty(S) && S.Split('<').Length > 1 && S.Split('"').Length > 1)
                        {
                            string Direccion = S.Split('<')[1].Replace('>', ' ').Trim();
                            string NombreMostrar = S.Split('"')[1].Trim();
                            MailAddress MA = new MailAddress(Direccion, NombreMostrar,
                                Encoding.UTF8);
                            MAC.Add(MA);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                new Exception("Error al obtener destinatario", exc);
            }

            return MAC;
        }

        public static SmtpClient IniciarSmtpClient(Cuenta cuenta)
        {
            SmtpClient ClienteSMTP = new SmtpClient();
            ClienteSMTP.Host = cuenta.NombreServidorSMPT;
            ClienteSMTP.Port = cuenta.Puerto;
            ClienteSMTP.EnableSsl = cuenta.Ssl;
            ClienteSMTP.DeliveryMethod = SmtpDeliveryMethod.Network;
            ClienteSMTP.Credentials = new NetworkCredential(cuenta.Usuario, cuenta.Contrasena);

            return ClienteSMTP;
        }
    }
}
