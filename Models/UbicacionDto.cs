using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JarredsOrderHub.Models;

namespace JarredsOrderHub.Models
{
    public class UbicacionDto
    {
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
    }
}
