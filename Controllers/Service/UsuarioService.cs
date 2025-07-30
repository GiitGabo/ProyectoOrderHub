using JarredsOrderHub.DbaseContext;
using JarredsOrderHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;



namespace JarredsOrderHub.Controllers.Service
{

    public class UsuarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(ApplicationDbContext context, ILogger<UsuarioService> logger, EmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        //RegistroCliente
        public async Task<(bool success, string message)> RegistrarCliente(Cliente cliente)
        {
            try
            {
                // Verificar email único
                var existeCliente = await _context.Clientes
                    .AnyAsync(c => c.Email == cliente.Email);

                if (existeCliente)
                {
                    _logger.LogWarning($"Intento de registro con email duplicado: {cliente.Email}");
                    return (false, "El correo electrónico ya está registrado");
                }

                // Hash de contraseña
                cliente.Contrasenia = HashPassword(cliente.Contrasenia);

                // Agregar cliente
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Cliente registrado exitosamente: {cliente.Email}");
                return (true, "Registro exitoso");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error al guardar cliente en la base de datos");
                return (false, "Error al guardar los datos. Intente nuevamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en registro de cliente");
                return (false, "Ocurrió un error inesperado");
            }
        }


        // Verificar Login Cliente
        public async Task<Cliente> VerificarLoginCliente(string email, string contrasenia)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Email == email);

            if (cliente != null && cliente.Contrasenia == HashPassword(contrasenia))
            {
                return cliente;  //Login
            }

            return null;  //No Logeo

        }

        // Verificar Login Empleado
        public async Task<Empleado> VerificarLoginEmpleado(string email, string contrasena)
        {
            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(e => e.Email == email);

            if (empleado != null && empleado.Contrasenia == HashPassword(contrasena))
            {
                return empleado;  // Login exitoso
            }

            return null;  //No Logeo
        }

        // Función de Encriptacion de Pass
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }


        //---------RECUPERACION DE CONTRASEÑA----------------//
        // Método para iniciar recuperación de contraseña
        public async Task<(bool success, string message)> IniciarRecuperacionContrasenia(string email)
        {
            try
            {
                // Verificar si el email existe (cliente o empleado)
                var clienteExists = await _context.Clientes.AnyAsync(c => c.Email == email);
                var empleadoExists = await _context.Empleados.AnyAsync(e => e.Email == email);

                if (!clienteExists && !empleadoExists)
                {
                    _logger.LogWarning($"Intento de recuperación para email no registrado: {email}");
                    return (false, "El correo electrónico no está registrado en el sistema");
                }

                // Generar token único
                string token = GenerarTokenUnico();

                // Crear registro de recuperación
                var recuperacion = new RecuperacionContrasenia
                {
                    Email = email,
                    Token = token,
                    FechaExpiracion = DateTime.UtcNow.AddHours(1), // Token válido por 1 hora
                    Utilizado = false
                };

                // Guardar en base de datos
                _context.RecuperacionesContrasenias.Add(recuperacion);
                await _context.SaveChangesAsync();

                // Construir URL de recuperación
                var request = _httpContextAccessor.HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}";
                var callbackUrl = $"{baseUrl}/Usuario/RestablecerContrasenia?token={token}&email={Uri.EscapeDataString(email)}";

                // Enviar email
                var emailSent = await _emailService.EnviarEmailRecuperacionContrasenia(email, token, callbackUrl);
                if (!emailSent)
                {
                    return (false, "Error al enviar el correo electrónico. Intente nuevamente más tarde.");
                }

                _logger.LogInformation($"Proceso de recuperación iniciado para: {email}");
                return (true, "Se ha enviado un correo con instrucciones para recuperar tu contraseña");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en proceso de recuperación para {email}");
                return (false, "Ocurrió un error inesperado. Intente nuevamente más tarde.");
            }
        }

        // Método para validar token de recuperación
        public async Task<bool> ValidarTokenRecuperacion(string token, string email)
        {
            try
            {
                var recuperacion = await _context.RecuperacionesContrasenias
                    .FirstOrDefaultAsync(r => r.Token == token && r.Email == email && !r.Utilizado);

                if (recuperacion == null)
                {
                    _logger.LogWarning($"Intento de validación con token inválido para {email}");
                    return false;
                }

                if (recuperacion.FechaExpiracion < DateTime.UtcNow)
                {
                    _logger.LogWarning($"Intento de validación con token expirado para {email}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al validar token para {email}");
                return false;
            }
        }

        // Método para resetear contraseña
        public async Task<(bool success, string message)> ResetearContrasenia(string token, string email, string nuevaContrasenia)
        {
            try
            {
                // Validar token
                var recuperacion = await _context.RecuperacionesContrasenias
                    .FirstOrDefaultAsync(r => r.Token == token && r.Email == email && !r.Utilizado);

                if (recuperacion == null)
                {
                    return (false, "Token inválido");
                }

                if (recuperacion.FechaExpiracion < DateTime.UtcNow)
                {
                    return (false, "El token ha expirado. Solicite un nuevo enlace de recuperación.");
                }

                // Hash de nueva contraseña
                string hashedPassword = HashPassword(nuevaContrasenia);

                // Actualizar contraseña en cliente o empleado
                var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);
                if (cliente != null)
                {
                    cliente.Contrasenia = hashedPassword;
                }
                else
                {
                    var empleado = await _context.Empleados.FirstOrDefaultAsync(e => e.Email == email);
                    if (empleado != null)
                    {
                        empleado.Contrasenia = hashedPassword;
                    }
                    else
                    {
                        return (false, "Usuario no encontrado");
                    }
                }

                // Marcar token como utilizado
                recuperacion.Utilizado = true;

                // Guardar cambios
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Contraseña restablecida para {email}");

                return (true, "Contraseña restablecida exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al resetear contraseña para {email}");
                return (false, "Ocurrió un error inesperado. Intente nuevamente.");
            }
        }

        // Genera un token único aleatorio
        private string GenerarTokenUnico()
        {
            byte[] tokenBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }
            return Convert.ToBase64String(tokenBytes)
                .Replace("/", "_")
                .Replace("+", "-")
                .Replace("=", "");
        }

        public async Task<bool> ActualizarUbicacionAsync(int IdEmpleado, double? latitud, double? longitud)
        {
            if (!latitud.HasValue || !longitud.HasValue)
            {
                _logger.LogWarning("No se proporcionaron coordenadas válidas para el empleado {IdEmpleado}", IdEmpleado);
                return false;
            }

            var empleado = await _context.Empleados.FindAsync(IdEmpleado);
            if (empleado == null)
            {
                _logger.LogWarning("Empleado no encontrado: {IdEmpleado}", IdEmpleado);
                return false;
            }

            // Usa latitud.Value y longitud.Value para obtener el double
            empleado.LatitudActual = latitud.Value;
            empleado.LongitudActual = longitud.Value;

            _context.Empleados.Update(empleado);
            await _context.SaveChangesAsync();
            return true;
        }

        // Add these methods to your UsuarioService class

        // Obtener datos del perfil de cliente
        public async Task<Cliente> ObtenerPerfilCliente(int idCliente)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.IdCliente == idCliente);
        }


        // Obtener datos del perfil de empleado
        public async Task<Empleado> ObtenerPerfilEmpleado(int idEmpleado)
        {
            return await _context.Empleados
                .Include(e => e.Rol)
                .Include(e => e.Horario)
                .FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);
        }

        // Actualizar perfil de cliente
        public async Task<(bool success, string message)> ActualizarPerfilCliente(Cliente cliente)
        {
            try
            {
                var existente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.IdCliente == cliente.IdCliente);

                if (existente == null)
                    return (false, "Cliente no encontrado");

                // Validar email único
                if (existente.Email != cliente.Email)
                {
                    var emailExiste = await _context.Clientes
                        .AnyAsync(c => c.Email == cliente.Email);
                    if (emailExiste)
                        return (false, "El correo electrónico ya está registrado");
                }

                // Actualizar solo los campos permitidos
                existente.Nombre = cliente.Nombre;
                existente.Apellido = cliente.Apellido;
                existente.Email = cliente.Email;
                existente.NumeroContacto = cliente.NumeroContacto;


                await _context.SaveChangesAsync();
                return (true, "Perfil de cliente actualizado exitosamente");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error de base de datos al actualizar cliente");
                return (false, "Error al guardar en la base de datos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado actualizando cliente");
                return (false, "Error interno del servidor");
            }
        }

        // Actualizar perfil de empleado
        public async Task<(bool success, string message)> ActualizarPerfilEmpleado(Empleado empleado)
        {
            try
            {
                var empleadoExistente = await _context.Empleados
                    .FirstOrDefaultAsync(e => e.IdEmpleado == empleado.IdEmpleado);

                if (empleadoExistente == null)
                {
                    return (false, "Empleado no encontrado");
                }

                // Validar email único
                if (empleadoExistente.Email != empleado.Email)
                {
                    var emailExiste = await _context.Empleados
                        .AnyAsync(e => e.Email == empleado.Email);

                    if (emailExiste)
                    {
                        return (false, "El correo electrónico ya está registrado");
                    }
                }

                // Actualizar campos permitidos
                empleadoExistente.Nombre = empleado.Nombre;
                empleadoExistente.Apellido = empleado.Apellido;
                empleadoExistente.Email = empleado.Email;

                // Actualizar contraseña solo si se proporciona
                if (!string.IsNullOrWhiteSpace(empleado.Contrasenia))
                {
                    empleadoExistente.Contrasenia = HashPassword(empleado.Contrasenia);
                }

                await _context.SaveChangesAsync();
                return (true, "Perfil actualizado exitosamente");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error de base de datos al actualizar empleado");
                return (false, "Error al guardar en la base de datos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado actualizando empleado");
                return (false, "Error interno del servidor");
            }
        }

    }
}
