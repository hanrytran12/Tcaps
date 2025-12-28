namespace Application.DTOs.Response
{
    public class EvaluateDTO
    {
        public Guid Id { get; set; }
        public Guid ProductionId { get; set; }
        public int QuantityError { get; set; }
        public int QuantitySuccess { get; set; }
        public string Note { get; set; } = string.Empty;
        public string Status { get; set; }
        public DateTime Created_At { get; set; }

        public List<string> Images { get; set; } = new();
        public List<ComponentDefectsDTO> Defects { get; set; } = new();
    }
}
