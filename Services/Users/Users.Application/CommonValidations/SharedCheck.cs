namespace Users.Application.CommonValidations
{
    public class SharedCheck
    {
        public static bool BeAValidPastDate(string birthDateString)
        {
            if (DateOnly.TryParseExact(birthDateString, "yyyy-MM-dd", out var birthDate))
            {
                return birthDate < DateOnly.FromDateTime(DateTime.Now);
            }

            return false;
        }

        public static bool BeValidYear(string birthDateString)
        {
            if (DateOnly.TryParseExact(birthDateString, "yyyy-MM-dd", out var birthDate))
            {
                int currentYear = DateTime.Now.Year;
                return birthDate.Year >= 1900 && birthDate.Year <= currentYear;
            }

            return false;
        }
    }
}
