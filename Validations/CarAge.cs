using System.ComponentModel.DataAnnotations;

public class CarAge : ValidationAttribute
{
    private readonly int minimumAge;
    private readonly int maximumAge;

    public CarAge(int minimumAge,int maximumAge)
    {
        this.minimumAge = minimumAge;
        this.maximumAge = maximumAge;
    }

    public override bool IsValid(object value)
    {
        if (value is DateTime manufacteredTime)
        {
            var age = (DateTime.Now - manufacteredTime).TotalDays / 365;

            var isAgeValid = age >=minimumAge && age <= maximumAge;
            if(isAgeValid)
            return true;
            else
            {
                ErrorMessage = $"Car age should be between{minimumAge} and {maximumAge}";
            }
            return false;

        }
        else 
        {
            ErrorMessage = "Car manufactered date time should be DateTime type.";
            return false;
        }
    }

}