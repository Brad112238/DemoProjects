using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetPriceRegistration.Models
{
    [Table("NetPriceMain")]
    public class NetPriceMain
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CoordinateId { get; set; }

        public string Oid { get; set; } = "";

        public string City { get; set; } = "";

        public string Area { get; set; } = "";

        public string FullAddress { get; set; } = "";

        public string Address { get; set; } = "";

        public string TradeType { get; set; } = "";

        public string TradeDate { get; set; } = "";

        public string TradeCount { get; set; } = "";

        public string? UrbanZone { get; set; }

        public string? NonUrbanZone { get; set; }

        public string? NonUrbanCategory { get; set; }

        public double LandTransferArea { get; set; }

        public string BuildTypeOrigin { get; set; } = "";

        public int BuildType { get; set; }

        public int? FloorS { get; set; }

        public int? FloorE { get; set; }

        public int? FloorCount { get; set; }

        public string? FloorOrigin { get; set; }

        public string? FloorCountOrigin { get; set; }

        public string? Purpose { get; set; }

        public string? Material { get; set; }

        public string? BuildCompleteDate { get; set; }

        public double TotalArea { get; set; }

        public double? MainArea { get; set; }

        public double? AccessoryArea { get; set; }

        public double? BalconyArea { get; set; }

        public int? Room { get; set; }

        public int? Hall { get; set; }

        public int? Bath { get; set; }

        public bool HouseCompartment { get; set; }

        public bool Elevator { get; set; }

        public bool HasManagemnet { get; set; }

        public int TotalPrice { get; set; }

        public int? UnitPrice { get; set; }

        public string? ParkingType { get; set; }

        public double? ParkingArea { get; set; }

        public int? ParkingPrice { get; set; }

        public int? HouseAge { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public bool SpecialTrans { get; set; }

        public string? Remark { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }
    }
}
