namespace FPIS.ErosRamazzottiTicketBooking.Api.Utility;

public static class TokenGenerator
{
    public static string GenerateTokenValue(int length = 10)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
