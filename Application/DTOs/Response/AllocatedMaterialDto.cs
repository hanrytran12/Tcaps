namespace Application.DTOs.Response
{
    public class AllocatedMaterialDto
    {
        public Guid MaterialId { get; set; }
        public string MaterialName { get; set; } = string.Empty;
    }
}
