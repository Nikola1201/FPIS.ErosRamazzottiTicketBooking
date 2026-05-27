namespace FPIS.Domain.Models;

/// <summary>Tip popusta koji se može primeniti na rezervaciju.</summary>
public enum DiscountType
{
    /// <summary>Popust za rane rezervacije.</summary>
    EarlyBird,
    /// <summary>Popust kada se kupuje peta karta na jednoj rezervaciji.</summary>
    FifthTicket,
    /// <summary>Popust dodeljen preko promo koda od prijatelja.</summary>
    FriendPromo
}
