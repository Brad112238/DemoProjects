using System.ComponentModel.DataAnnotations;

namespace WebApiDemo.Models.TestDb;

public partial class UserTopUp
{
    [Key]
    public int Id { get; set; }

    public int HostId { get; set; }

    public int UserId { get; set; }

    [MaxLength(100)]
    public string? Product { get; set; }

    public int Type { get; set; }

    public double Amount { get; set; }

    public int Quantity { get; set; }

    [MaxLength(500)]
    public string? Note { get; set; }

    [MaxLength(50)]
    public string? PayType { get; set; }

    [MaxLength(20)]
    public string? TradeNo { get; set; }

    public int Status { get; set; }

    [MaxLength(1000)]
    public string? Message { get; set; }

    [MaxLength(50)]
    public string? TPSTradeNo { get; set; }

    [MaxLength(20)]
    public string? InvoiceNumber { get; set; }

    [MaxLength(200)]
    public string? InvoiceNote { get; set; }

    [MaxLength(20)]
    public string? CarruerNum { get; set; }

    [MaxLength(10)]
    public string? CarruerType { get; set; }

    [MaxLength(200)]
    public string? CompAddr { get; set; }

    [MaxLength(20)]
    public string? CompIdNumber { get; set; }

    [MaxLength(100)]
    public string? CompName { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? InvoiceType { get; set; }

    [MaxLength(30)]
    public string? LoveCode { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(20)]
    public string? UserIdNumber { get; set; }

    public string? SendParams { get; set; }

    public string? ResponseParams { get; set; }

    public bool Deleted { get; set; }

    public DateTime CreateTime { get; set; }

    public int CreateUser { get; set; }

    public DateTime? ModifyTime { get; set; }

    public int? ModifyUser { get; set; }
}
