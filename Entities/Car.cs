public class Car
{
    public Guid Id { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }

    public string Brand { get; set; }

    public DateTime  ManufacteredAt { get; set; }
    public Guid OwnerId { get; set; }

    public virtual User Owner { get; set; }

    
}