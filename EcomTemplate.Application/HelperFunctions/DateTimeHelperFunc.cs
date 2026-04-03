namespace EcomTemplate.API.HelperFunctions;

public static class DateTimeHelperFunc
{
    public static DateTime ToUtc(DateTime date)
    {
        return DateTime.SpecifyKind(date, DateTimeKind.Utc);
    }

    public static (DateTime start, DateTime end) GetMonthRange(int offset = 0)
    {
        var now = DateTime.UtcNow.AddMonths(offset);

        var start = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var end = start.AddMonths(1);

        return (start, end);
    }
}