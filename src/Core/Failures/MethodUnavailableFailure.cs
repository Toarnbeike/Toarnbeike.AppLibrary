using Toarnbeike.Results.Failures;

namespace Toarnbeike.AppLibrary.Failures;

/// <summary>
/// Represents a domain failure indicating that a requested method is unavailable.
/// </summary>
/// <param name="MethodName">Name of the unavailable method.</param>
/// <param name="Reason">Optional explanation of why the method is unavailable.</param>
public sealed record MethodUnavailableFailure(string MethodName, string? Reason = null)
    : DomainFailure($"Method '{MethodName}' is unavailable{(Reason is null ? string.Empty : $": {Reason}")}");