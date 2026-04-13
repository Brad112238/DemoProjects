namespace WebApiDemo.ViewModels.UserCredit
{
    public class CreateTradeTokenRequest
    {
        public int UserId { get; set; }

        public int HostId { get; set; }

        public string? CarruerNum { get; set; }

        public string? PointAmount { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? CompanyTitle { get; set; }

        public string? CompanyTaxId { get; set; }

        public string? CompanyAddr { get; set; }

        public string? InvoiceType { get; set; }
    }
}
