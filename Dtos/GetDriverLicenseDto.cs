public class  GetDriverLicenseDto
{
    public Guid Id { get; set; }
    public string  Serial { get; set; }
    public DateTime IssiuedDate { get; set; }
    public DateTime ExperiationDate { get; set; }
    
    public bool IsExpired => ExperiationDate < DateTime.UtcNow;
}