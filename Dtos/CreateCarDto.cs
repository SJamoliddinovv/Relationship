
using System.ComponentModel.DataAnnotations;

public class CreateCarDto
{
    [Required,MinLength(2),MaxLength(20)]
    public string Model { get; set; }

    [Required,MinLength(2),MaxLength(20)]
    public string Color { get; set; }

    public string Brand { get; set; }


    [Required,CarAge(minimumAge : 0,  maximumAge : 20)]
    public DateTime  ManufacteredAt { get; set; }
    public Guid OwnerId { get; set; }
}
