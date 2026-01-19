namespace WebApiDemo.ViewModels.Ecpay
{
    public class EcpayInvoiceRequest
    {
        public string MerchantTradeNo { get; set; } = null!;

        public int TotalAmount { get; set; }

        public string CustomerEmail { get; set; } = null!;

        public string? CompanyTaxId { get; set; }

        public string? CompanyTitle { get; set; }

        public string? CompanyAddr { get; set; }

        public string? InvoiceCode { get; set; }
    }
}
