using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetPriceRegistration.Models
{
    [Table("NetPriceCoordinate")]
    public class NetPriceCoordinate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? City { get; set; }

        public string? Area { get; set; }

        public string? Address { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }
    }
}
