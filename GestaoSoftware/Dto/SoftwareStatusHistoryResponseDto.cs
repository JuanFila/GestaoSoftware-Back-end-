public class SoftwareStatusHistoryResponseDto
{
    public string SoftwareName { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public string OldStatus { get; set; }
    public string NewStatus { get; set; }
    public DateTime ChangedAt { get; set; }
}
