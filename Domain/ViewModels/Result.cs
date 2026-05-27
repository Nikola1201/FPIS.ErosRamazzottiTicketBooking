namespace FPIS.Domain.ViewModels;

/// <summary>
/// Rezultat operacije koji enkapsulira uspeh ili neuspeh, vrednost, poruku greške i opcioni kod greške.
/// </summary>
/// <typeparam name="T">Tip uspešne vrednosti.</typeparam>
public class Result<T>
{
    /// <summary>Da li je operacija uspela.</summary>
    public bool IsSuccess { get; }
    /// <summary>Vrednost vraćena pri uspehu; null u slučaju greške.</summary>
    public T? Value { get; }
    /// <summary>Poruka greške u slučaju neuspeha; null pri uspehu.</summary>
    public string? Error { get; }
    /// <summary>Opcioni numerički kod greške (npr. HTTP status kod).</summary>
    public int? ErrorCode { get; }

    private Result(bool isSuccess, T? value, string? error, int? errorCode)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        ErrorCode = errorCode;
    }

    /// <summary>Kreira uspešan rezultat sa datom vrednošću.</summary>
    /// <param name="value">Vrednost koja se vraća pri uspehu.</param>
    /// <returns>Uspešan <see cref="Result{T}"/>.</returns>
    public static Result<T> Success(T value) => new(true, value, null, null);
    /// <summary>Kreira neuspešan rezultat sa porukom greške i opcionim kodom.</summary>
    /// <param name="error">Poruka greške.</param>
    /// <param name="errorCode">Opcioni numerički kod greške (npr. HTTP status).</param>
    /// <returns>Neuspešan <see cref="Result{T}"/>.</returns>
    public static Result<T> Failure(string error, int? errorCode = null) => new(false, default, error, errorCode);
}
