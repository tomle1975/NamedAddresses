namespace Tql.Agdata.NamedAddress.Domain.SeedWork;

/// <summary>
/// Represents a result of a process, action, or an operation.
/// </summary>
/// <typeparam name="TData"></typeparam>
public class Result<TData>
{
    /// <summary>
    /// Gets the result status as TRUE or FALSE.
    /// </summary>
    public bool IsSuccessful { get; set; }

    /// <summary>
    /// Gets the short message of the result.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets the long description of the result.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets list of exceptions which may occur during the processing.
    /// </summary>
    public List<Exception> Errors { get; } = new();

    /// <summary>
    /// Gets the data resulted from the processing.
    /// </summary>
    public TData? Data { get; set; }
}