namespace Application.DTOs.Response
{
    public class WorkshopsDTO
    {
        public Guid WorkshopId { get; set; }
        public string WorkshopName { get; set; } = string.Empty;
        public int StepOrder { get; set; }
    }
}
