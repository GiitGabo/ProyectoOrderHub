using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JarredsOrderHub.Models
{
    public class CreateOrderRequestModel
    {
        public decimal Value { get; set; }
    }
}
