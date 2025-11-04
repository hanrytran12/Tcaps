namespace Application.DTOs.Response
{
    public class PendingRequestDTO
    {
        public Guid RequestId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
        public decimal QuantityRequest { get; set; }
        public string BatchCode { get; set; } = string.Empty;
    }
}
