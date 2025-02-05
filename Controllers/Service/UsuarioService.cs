using JarredsOrderHub.DbaseContext;
using JarredsOrderHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;



namespace JarredsOrderHub.Controllers.Service
{

    public class UsuarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(ApplicationDbContext context, ILogger<UsuarioService> logger)
        {
            _context = context;
            _logger = logger;
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



    }
}
