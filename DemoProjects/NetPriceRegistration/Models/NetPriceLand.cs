using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetPriceRegistration.Models
{
    [Table("NetPriceLand")]
    public class NetPriceLand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int MainId { get; set; }

        public string Section { get; set; } = "";

        public string LandNo { get; set; } = "";

        public double TotalArea { get; set; }

        public string LandUseType { get; set; } = "";

        public string TransferType { get; set; } = "";

        public int ShareDenominator { get; set; }

        public int ShareNumerator { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }
    }
}
