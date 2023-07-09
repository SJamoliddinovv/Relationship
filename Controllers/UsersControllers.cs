using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("/api/[controller]")]

public class UsersControllers : ControllerBase
{
    private readonly AppDbContext dbContext;

    public UsersControllers(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
  [HttpPost]
public async Task<IActionResult> CreateUser(CreateUserDto user)
{
    if(await dbContext.Users.AnyAsync(u => u.Username.ToLower() == user.Username.ToLower()))
    return Conflict("User with this name already exists");
    if(await dbContext.Users.AnyAsync(u => u.Email.ToLower() == user.Email.ToLower()))
    return Conflict("User with this name already exists");
     var created = dbContext.Users.Add(new User
     {
        Id = Guid.NewGuid(),
        Name = user.Name,
        Birthday = user.Birthday,
        Email = user.Email,
        Username = user.Username
    });

    await dbContext.SaveChangesAsync();
    return Ok(created.Entity);

}

[HttpGet("{id}")]
public async Task<IActionResult> GetUser(
    [FromRoute] Guid id)
    {
       var user = await dbContext.Users
        .Where(u => u.Id == id)
        .Include(u => u.DriverLicense)
        .FirstOrDefaultAsync(); 

        if (user is null )
        return NotFound();

        return Ok(new GetUserDto
        {
            Id = user.Id,
            Name = user.Name,
            Username = user.Username,
            Birthday = user.Birthday,
            Email = user.Email,
            DriverLicense = user.DriverLicense is null
            ? null : new GetDriverLicenseDto
            {
            Id = user.DriverLicense.Id,
            Serial = user.DriverLicense.Serial,
            IssiuedDate = user.DriverLicense.IssuedDate,
            ExperiationDate = user.DriverLicense.ExpirationDate,
            }

        });
    }
    [HttpGet]
    public async Task<IActionResult> GetUsers( [FromRoute] Guid id, [FromQuery] string search)
    {
        var usersQuery = dbContext.Users.AsQueryable();
        
        if (false == string.IsNullOrWhiteSpace(search))
        usersQuery = usersQuery.Where(u => 
        u.Name.ToLower().Contains(search.ToLower()) || 
        u.Username.ToLower().Contains(search.ToLower()));

        var users = await usersQuery
        .Select(u => new GetUserDto 
        {
            Id = u.Id,
            Name = u.Name,
            Username = u.Username,
            Birthday = u.Birthday,
            Email = u.Email,
        })
        .ToListAsync();
        return Ok(users);
}
[HttpPut("{id}")]
public async Task<IActionResult> UpdateUser([FromRoute] Guid id,UpdateUserDto updateUser)
    {
        var user = await dbContext.Users
        .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
        return NotFound();


    if(await dbContext.Users.AnyAsync(u => u.Username.ToLower() == updateUser.Username.ToLower()))
    return Conflict("User with this name already exists");
    if(await dbContext.Users.AnyAsync(u => u.Email.ToLower() == updateUser.Email.ToLower()))
    return Conflict("User with this name already exists");

        user.Name = updateUser.Name;
        user.Birthday = updateUser.Birthday;
        user.Username = updateUser.Username;
        user.Email = updateUser.Email;

    await dbContext.SaveChangesAsync();
    return Ok(user.Id);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute]Guid id)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

        if ( user is null)
        NotFound();

         dbContext.Users.Remove(user);
         await dbContext.SaveChangesAsync();
         return Ok();
    }

    [HttpPost("{userId}/driver-license")]

    public async Task<IActionResult> CreateDriverLicense(Guid userId,CreateDriverLicenseDto driverLicense)
    {
        var user = await  dbContext.Users
        .Where(u => u.Id == userId)
        .Include(u => u.DriverLicense)
        .FirstOrDefaultAsync();

        if (user is null)
        return BadRequest($"User with {userId} id doesnt exists ");
        
        if(user.DriverLicense is not null )
        return BadRequest("User has already license");

        user.DriverLicense = new DriverLicense
        {
            Serial = driverLicense.Serial,
            IssuedDate = driverLicense.IssiuedDate,
            ExpirationDate = driverLicense.ExperiationDate
        };
        await dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("{id}/driver-license")]
public async Task<IActionResult> GetUsersDriverLicense(
    [FromRoute] Guid id)
    {
        var user = await dbContext.Users
        .Where(u => u.Id == id)
        .Include(u => u.DriverLicense)
        .FirstOrDefaultAsync(); 

        if (user is null || user.DriverLicense is null)
        return NotFound();

        return Ok(new GetDriverLicenseDto
        {
            Id = user.DriverLicense.Id,
            Serial = user.DriverLicense.Serial,
            IssiuedDate = user.DriverLicense.IssuedDate,
            ExperiationDate = user.DriverLicense.ExpirationDate,
            

        });
    }

    [HttpPut("{id}/driver-license")]
public async Task<IActionResult> UpdateDriverLicence([FromRoute] Guid id,UpdateDriverLicenceDto driverLicenseUpdate)
    {
        var user = await dbContext.Users
        .Where(u => u.Id == id)
        .FirstOrDefaultAsync();

        if (user is null || user.DriverLicense is null)
        return NotFound();
        
        user.DriverLicense.Id = driverLicenseUpdate.Id;
        user.DriverLicense.Serial = driverLicenseUpdate.Serial;
        user.DriverLicense.IssuedDate = driverLicenseUpdate.IssiuedDate;
        user.DriverLicense.ExpirationDate = driverLicenseUpdate.ExperiationDate;
        
    await dbContext.SaveChangesAsync();
    return Ok(user.DriverLicense.Id);

    }
    [HttpDelete("{id}/driver-license")]
    public async Task<IActionResult> DeleteDriverLicense([FromRoute]Guid id)
    {
        var license = await dbContext.Users
        .Where(u => u.Id == id)
        .FirstOrDefaultAsync();

        if ( license is null)
        NotFound();

         dbContext.Users.Remove(license);
         await dbContext.SaveChangesAsync();
         return Ok();
    }
}