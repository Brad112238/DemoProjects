using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetPriceRegistration.Models
{
    [Table("NetPriceBuild")]
    public class NetPriceBuild
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int MainId { get; set; }

        public string? MainPurpose { get; set; }

        public string? Material { get; set; }

        public string? BuildCompleteDate { get; set; }

        public string? FloorCount { get; set; }

        public string? FloorInfo { get; set; }

        public string? TransferType { get; set; }

        public int? HouseYear { get; set; }

        public double TotalArea { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }
    }
}
