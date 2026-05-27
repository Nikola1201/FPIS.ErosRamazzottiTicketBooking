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
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length), "Length must be non-negative.");

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var buffer = new char[length];
        for (int i = 0; i < length; i++)
            buffer[i] = chars[Random.Shared.Next(chars.Length)];
        return new string(buffer);
    }
}
