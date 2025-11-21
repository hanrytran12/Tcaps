namespace Application.DTOs.Response
{
    public class ComponentDefectsDTO
    {
        public Guid Id { get; set; }
        public string DefectType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Solution { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string NameStaff { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
