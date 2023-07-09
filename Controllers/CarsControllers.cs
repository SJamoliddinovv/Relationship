using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]

public class CarsControllers : ControllerBase
{
    private readonly AppDbContext dbContext;

    public CarsControllers(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpPost]

    public async Task <IActionResult> CreateCar(CreateCarDto CarDto)
    {
        var owner = await dbContext.Users
        .FirstOrDefaultAsync(x => x.Id == CarDto.OwnerId);

        if (owner is null)
        return BadRequest("Owner does not exists");

        var savedCar = dbContext.Cars.Add( new Car
        {
            Id = Guid.NewGuid(),
            Brand = CarDto.Brand,
            Model = CarDto.Model,
            Color = CarDto.Color,
            ManufacteredAt = CarDto.ManufacteredAt,
            Owner = owner
        });
        await dbContext.SaveChangesAsync();
        return Ok(new GetCarDto
        {
            Id = savedCar.Entity.Id,
            Model = savedCar.Entity.Model,
            Color = savedCar.Entity.Color,
            Brand = savedCar.Entity.Brand,
            ManufacteredAt = savedCar.Entity.ManufacteredAt,
            OwnerId = savedCar.Entity.OwnerId,
            Owner = new GetUserDto
            {
             Id = savedCar.Entity.Owner.Id,
            Name = savedCar.Entity.Owner.Name,
            Username = savedCar.Entity.Owner.Username,
            Birthday = savedCar.Entity.Owner.Birthday,
            Email = savedCar.Entity.Owner.Email,
            DriverLicense = null
            }
        });
    }

}