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

        public async Task<bool> EnviarReciboPedido(Pedidos pedido, Cliente usuario)
        {
            try
            {
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromPassword = _configuration["EmailSettings:Password"];
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
                var enableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"]);

                var cuerpoRecibo = ConstruirCuerpoRecibo(pedido, usuario);

                var mensaje = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = $"Recibo de Pedido #{pedido.Id} - Jarred's Order Hub",
                    Body = cuerpoRecibo,
                    IsBodyHtml = true
                };
                mensaje.To.Add(usuario.Email);

                using (var cliente = new SmtpClient(smtpHost, smtpPort))
                {
                    cliente.EnableSsl = enableSsl;
                    cliente.Credentials = new NetworkCredential(fromEmail, fromPassword);
                    await cliente.SendMailAsync(mensaje);
                }

                _logger.LogInformation($"Recibo enviado para el pedido #{pedido.Id} a {usuario.Email}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al enviar recibo para el pedido #{pedido.Id}");
                return false;
            }
        }

        private string ConstruirCuerpoRecibo(Pedidos pedido, Cliente usuario)
        {
            decimal subtotal = pedido.Detalles?.Sum(d => d.Total) ?? 0;
            decimal descuento = 0;

            if (pedido.Cupon != null)
            {
                descuento = pedido.Cupon.EsPorcentual
                    ? subtotal * (pedido.Cupon.Descuento / 100)
                    : pedido.Cupon.Descuento;
            }

            var detallesProductos = new StringBuilder();
            if (pedido.Detalles != null)
            {
                foreach (var detalle in pedido.Detalles)
                {
                    detallesProductos.AppendLine($@"
         <tr>
             <td>{detalle.Platillo?.Nombre ?? "Producto"}</td>
             <td>{detalle.Cantidad}</td>
             <td>{detalle.PrecioUnitario.ToString("C")}</td>
             <td>{detalle.Total.ToString("C")}</td>
         </tr>");
                }
            }

            return $@"
             <html>
             <head>
                 <style>
                     body {{
                         font-family: 'Arial', sans-serif;
                         margin: 0;
                         padding: 0;
                         background-color: #F2EAD0;
                         color: #262626;
                     }}
                     .email-container {{
                         width: 100%;
                         padding: 20px;
                         background-color: #F2EAD0;
                     }}
                     .email-content {{
                         max-width: 600px;
                         margin: auto;
                         background-color: #ffffff;
                         padding: 30px;
                         border-radius: 10px;
                         box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                     }}
                     .header {{
                         padding-bottom: 20px;
                         border-bottom: 1px solid #eeeeee;
                         text-align: center;
                     }}
                     .header h2 {{
                         color: #2EA629;
                         font-size: 28px;
                         margin: 0;
                     }}
                     .info-cliente {{
                         margin: 20px 0;
                         padding: 15px;
                         background-color: #f9f9f9;
                         border-radius: 5px;
                     }}
                     table {{
                         width: 100%;
                         border-collapse: collapse;
                         margin: 20px 0;
                     }}
                     th, td {{
                         padding: 10px;
                         text-align: left;
                         border-bottom: 1px solid #ddd;
                     }}
                     th {{
                         background-color: #F2A413;
                         color: white;
                     }}
                     .totales {{
                         margin-top: 20px;
                         text-align: right;
                     }}
                     .footer {{
                         margin-top: 30px;
                         font-size: 12px;
                         color: #999999;
                         text-align: center;
                     }}
                 </style>
             </head>
             <body>
                 <div class='email-container'>
                     <div class='email-content'>
                         <div class='header'>
                             <h2>Recibo de Pedido</h2>
                             <p>Número de pedido: #{pedido.Id}</p>
                             <p>Fecha: {pedido.FechaPedido.ToString("dd/MM/yyyy HH:mm")}</p>
                             <p>Estado: {pedido.EstadoPedido}</p>
                         </div>
         
                         <div class='info-cliente'>
                             <h3>Información del Cliente</h3>
                             <p>Nombre: {usuario.Nombre}</p>
                             <p>Email: {usuario.Email}</p>
                         </div>
         
                         <h3>Detalles del Pedido</h3>
                         <table>
                             <thead>
                                 <tr>
                                     <th>Producto</th>
                                     <th>Cantidad</th>
                                     <th>Precio Unitario</th>
                                     <th>Subtotal</th>
                                 </tr>
                             </thead>
                             <tbody>
                                 {detallesProductos}
                             </tbody>
                         </table>
         
                         <div class='totales'>
                             <p><strong>Subtotal:</strong> {subtotal.ToString("C")}</p>
                             {(descuento > 0 ? $"<p><strong>Descuento:</strong> -{descuento.ToString("C")}</p>" : "")}
                             <p><strong>Total:</strong> {pedido.Total.ToString("C")}</p>
                             <p><strong>Método de Pago:</strong> {pedido.MetodoPago}</p>
                         </div>
         
                         <div class='footer'>
                             <p>Gracias por tu compra en Jarred's Order Hub</p>
                             <p>Si tienes alguna pregunta, contáctanos a {_configuration["EmailSettings:FromEmail"]}</p>
                             <p>&copy; {DateTime.Now.Year} Jarred's Order Hub. Todos los derechos reservados.</p>
                         </div>
                     </div>
                 </div>
             </body>
             </html>";
        }
    }
}
