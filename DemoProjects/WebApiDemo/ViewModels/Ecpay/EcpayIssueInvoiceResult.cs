namespace WebApiDemo.ViewModels.Ecpay
{
    public class EcpayIssueInvoiceResult
    {
        public bool Success { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? Message { get; set; }
    }
}
