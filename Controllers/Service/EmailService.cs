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
                    Subject = "Restablecer contraseña - Jarred's Order Hub",
                    Body = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: 'Arial', sans-serif;
                    margin: 0;
                    padding: 0;
                    background-color: #F2EAD0;
                    color: #262626;
                    text-align: center;
                }}
                .email-container {{
                    width: 100%;
                    padding: 20px;
                    background-color: #F2EAD0;
                    text-align: center;
                }}
                .email-content {{
                    max-width: 600px;
                    margin: auto;
                    background-color: #ffffff;
                    padding: 30px;
                    border-radius: 10px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                    text-align: center;
                }}
                .header {{
                    padding-bottom: 20px;
                    border-bottom: 1px solid #eeeeee;
                }}
                .header h2 {{
                    color: #2EA629;
                    font-size: 28px;
                    margin: 0;
                }}
                .content {{
                    margin: 20px 0;
                    line-height: 1.6;
                }}
                .btn {{
                    display: inline-block;
                    padding: 12px 20px;
                    background-color: #F2A413;
                    color: #ffffff !important;
                    text-decoration: none;
                    border-radius: 5px;
                    font-weight: bold;
                    margin: 20px 0;
                }}
                .footer {{
                    margin-top: 30px;
                    font-size: 12px;
                    color: #999999;
                    text-align: center;
                }}
            </style>
            </head> 
        </html>
        <html>
        <body>
            <div class='email-container'>
                <div class='email-content'>
                    <div class='header'>
                        <h2>Recuperación de Contraseña</h2>
                    </div>
                    <div class='content'>
                        <p>Hola,</p>
                        <p>Has solicitado recuperar tu contraseña para Jarred's Order Hub.</p>
                        <p>Para restablecerla, haz clic en el siguiente botón:</p>
                        <p>
                            <a class='btn' style='color: #ffffff !important; background-color: #F2A413 !important;' href='{callbackUrl}'>
                                Restablecer Contraseña
                            </a>
                        </p>
                        <p>Si no solicitaste este cambio, por favor ignora este correo.</p>
                        <p><em>Nota: Este enlace expirará en 1 hora.</em></p>
                    </div>
                    <div class='footer'>
                        <p>&copy; {DateTime.Now.Year} Jarred's Order Hub. Todos los derechos reservados.</p>
                    </div>
                </div>
            </div>
        </body>
        </html>",
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
