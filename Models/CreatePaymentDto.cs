namespace JarredsOrderHub.Models
{
    // Models/Dto/CreatePaymentDto.cs
    public class CreatePaymentDto
    {
        public List<DetallePedidoDto> Detalles { get; set; }
        public int UsuarioId { get; set; }
        public string Comentarios { get; set; }
        public decimal CuponDescuentoPorcentaje { get; set; }  // 0 si no hay
        public int? CuponId { get; set; }
        public double LatitudEntrega { get; set; }
        public double LongitudEntrega { get; set; }
    }

    public class DetallePedidoDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

}
