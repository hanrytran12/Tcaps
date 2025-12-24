namespace Application.DTOs.Response
{
    public class StaffSummaryDashboardDTO
    {
        public BatchDTO Batches { get; set; } = new BatchDTO();
        public List<StaffAssignmentDTO> Assignments { get; set; } = new List<StaffAssignmentDTO>();
    }
}
