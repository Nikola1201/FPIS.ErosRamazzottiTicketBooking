namespace FPIS.ErosRamazzottiTicketBooking.Api.Utility;

/// <summary>
/// Helper za generisanje slučajnih alfanumeričkih tokena (npr. access token, promo kod).
/// </summary>
public static class TokenGenerator
{
    /// <summary>Generiše slučajan string sastavljen od velikih slova i cifara.</summary>
    /// <param name="length">Dužina generisanog stringa (podrazumevano 10).</param>
    /// <returns>Slučajan alfanumerički string date dužine.</returns>
    public static string GenerateTokenValue(int length = 10)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
