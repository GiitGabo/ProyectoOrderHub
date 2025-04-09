using JarredsOrderHub.DbaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JarredsOrderHub.Controllers
{
    public class EnvioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnvioController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult CalificarPedido()
        {
            return View();
        }

        public ActionResult EstadoPedido()
        {

            return View();
        }

        public ActionResult GestionProblemas()
        {

            return View();
        }

        public ActionResult RetrasoEnvio()
        {

            return View();
        }


        public async Task<ActionResult> ListadoPedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Platillo)
                .Where(p => p.EstadoPedido.ToLower() == "pendiente" ||
                            p.EstadoPedido.ToLower() == "preparando" ||
                            p.EstadoPedido.ToLower() == "en_camino")
                .ToListAsync();

            return View(pedidos);
        }


        public ActionResult VisualizarRetroalimentaciones()
        {

            return View();
        }
    }
}



namespace JarredsOrderHub.Models
{
    public class DetallePedido
    {
        [Key]
        public int Id { get; set; }
        public int PedidoId { get; set; }
        [ForeignKey("Platillo")]
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }

        public virtual Pedidos? Pedido { get; set; }
        public virtual Platillo? Platillo { get; set; }
    }

    public class Pedidos
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaPedido { get; set; } = DateTime.Now;
        public string EstadoPedido { get; set; } = "Pendiente"; // Estados: Pendiente, En preparación, En camino, Entregado, Cancelado
        public string MetodoPago { get; set; } // Efectivo, Tarjeta, PayPal
        public decimal Total { get; set; }
        public string Comentarios { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Cliente? Cliente { get; set; }
        public virtual List<DetallePedido>? Detalles { get; set; }
    }
}