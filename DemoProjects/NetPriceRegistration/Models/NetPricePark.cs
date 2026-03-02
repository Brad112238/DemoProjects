using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetPriceRegistration.Models
{
    [Table("NetPricePark")]
    public class NetPricePark
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int MainId { get; set; }

        public string ParkingType { get; set; } = "";

        public string ParkingFloor { get; set; } = "";

        public double TotalArea { get; set; }

        public int TotalPrice { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }
    }
}
