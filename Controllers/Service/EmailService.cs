using JarredsOrderHub.DbaseContext;
using JarredsOrderHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace JarredsOrderHub.Controllers.Service
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> EnviarEmailRecuperacionContrasenia(string emailDestinatario, string token, string callbackUrl)
        {
            try
            {
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromPassword = _configuration["EmailSettings:Password"];
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"]);

                var mensaje = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = "Recuperación de contraseña - Jarred's Order Hub",
                    Body = $@"
                        <html>
                        <head>
                            <style>
                                body {{ font-family: Arial, sans-serif; }}
                                .container {{ padding: 20px; }}
                                .btn {{ padding: 10px 15px; background-color: #4CAF50; color: white; text-decoration: none; border-radius: 4px; }}
                            </style>
                        </head>
                        <body>
                            <div class='container'>
                                <h2>Recuperación de contraseña</h2>
                                <p>Has solicitado recuperar tu contraseña. Haz clic en el siguiente enlace para establecer una nueva contraseña:</p>
                                <p><a class='btn' href='{callbackUrl}'>Restablecer contraseña</a></p>
                                <p>Si no solicitaste recuperar tu contraseña, ignora este correo.</p>
                                <p>Este enlace expirará en 24 horas.</p>
                            </div>
                        </body>
                        </html>
                    ",
                    IsBodyHtml = true
                };
                mensaje.To.Add(emailDestinatario);

                using (var cliente = new SmtpClient(smtpHost, smtpPort))
                {
                    cliente.EnableSsl = enableSsl;
                    cliente.Credentials = new NetworkCredential(fromEmail, fromPassword);
                    await cliente.SendMailAsync(mensaje);
                }

                _logger.LogInformation($"Correo de recuperación enviado a {emailDestinatario}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al enviar correo de recuperación a {emailDestinatario}");
                return false;
            }
        }
    }
}
