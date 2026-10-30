using System.Runtime.CompilerServices;
using Toarnbeike.AppLibrary.Entities;
using Toarnbeike.AppLibrary.Failures;
using Toarnbeike.Results;
using Toarnbeike.Results.Extensions;

namespace Toarnbeike.AppLibrary.Results;

/// <summary>
/// Extensions for ensuring that a property value has been modified before proceeding with an operation.
/// </summary>
public static class EnsureModifiedExtensions
{
    private const string MissingPropertyName = "Property";

    /// <summary>
    /// Validates that the entity value in the result differs from the current entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to compare, which must implement IEntity.</typeparam>
    /// <param name="modified">The result that contains the proposed entity value.</param>
    /// <param name="current">The current entity value used as the comparison baseline.</param>
    /// <returns>A result containing the proposed entity when a modification is detected; otherwise a failure result. Existing failure state is preserved.</returns>
    public static Result<TEntity> EnsureModified<TEntity>(this Result<TEntity> modified, TEntity current)
        where TEntity : IEntity =>
        modified.Bind(newValue => Evaluate(current, newValue, typeof(TEntity).Name));

    /// <summary>
    /// Validates that the entity value in the result differs from the current entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to compare, which must implement IEntity.</typeparam>
    /// <param name="modified">The async result that contains the proposed entity value.</param>
    /// <param name="current">The current entity value used as the comparison baseline.</param>
    /// <returns>A result containing the proposed entity when a modification is detected; otherwise a failure result. Existing failure state is preserved.</returns>
    public static async Task<Result<TEntity>> EnsureModified<TEntity>(this Task<Result<TEntity>> modified, TEntity current)
        where TEntity : IEntity =>
        await modified.Bind(newValue => Evaluate(current, newValue, typeof(TEntity).Name));

    /// <summary>
    /// Propagates the current result and verifies that a property value changed from the current value to the new value.
    /// </summary>
    /// <remarks>Uses caller argument expression metadata to infer the property name when no explicit name is provided.</remarks>
    /// <typeparam name="TOther">The type of the value carried by the input result.</typeparam>
    /// <typeparam name="TProp">The type of the property value being compared.</typeparam>
    /// <param name="result">The result that must be successful before the modification check runs.</param>
    /// <param name="currentValue">The current property value.</param>
    /// <param name="newValue">The proposed property value.</param>
    /// <param name="propertyName">The property name used in validation messages. If not provided, the argument expression for currentValue is used.</param>
    /// <returns>A successful result containing the new property value when the input result is successful and the value is modified; otherwise, a failed result.</returns>
    public static Result<TProp> EnsureModified<TOther, TProp>(this Result<TOther> result, TProp currentValue, TProp newValue, 
        [CallerArgumentExpression(nameof(currentValue))] string? propertyName = null) => 
        result.Bind(_ => Evaluate(currentValue, newValue, propertyName ?? MissingPropertyName));

    /// <summary>
    /// Propagates the current result and verifies that a property value changed from the current value to the new value.
    /// </summary>
    /// <remarks>Uses caller argument expression metadata to infer the property name when no explicit name is provided.</remarks>
    /// <typeparam name="TProp">The type of the property value being compared.</typeparam>
    /// <param name="result">The result that must be successful before the modification check runs.</param>
    /// <param name="currentValue">The current property value.</param>
    /// <param name="newValue">The proposed property value.</param>
    /// <param name="propertyName">The property name used in validation messages. If not provided, the argument expression for currentValue is used.</param>
    /// <returns>A successful result containing the new property value when the input result is successful and the value is modified; otherwise, a failed result.</returns>
    public static Result<TProp> EnsureModified<TProp>(this Result result, TProp currentValue, TProp newValue,
        [CallerArgumentExpression(nameof(currentValue))] string? propertyName = null) =>
        result.Bind(() => Evaluate(currentValue, newValue, propertyName ?? MissingPropertyName));

    /// <summary>
    /// Propagates the current result and verifies that a property value changed from the current value to the new value.
    /// </summary>
    /// <remarks>Uses caller argument expression metadata to infer the property name when no explicit name is provided.</remarks>
    /// <typeparam name="TOther">The type of the value carried by the input result.</typeparam>
    /// <typeparam name="TProp">The type of the property value being compared.</typeparam>
    /// <param name="result">The async result that must be successful before the modification check runs.</param>
    /// <param name="currentValue">The current property value.</param>
    /// <param name="newValue">The proposed property value.</param>
    /// <param name="propertyName">The property name used in validation messages. If not provided, the argument expression for currentValue is used.</param>
    /// <returns>A successful result containing the new property value when the input result is successful and the value is modified; otherwise, a failed result.</returns>
    public static async Task<Result<TProp>> EnsureModified<TOther, TProp>(this Task<Result<TOther>> result, TProp currentValue, TProp newValue,
        [CallerArgumentExpression(nameof(currentValue))] string? propertyName = null) =>
        await result.Bind(_ => Evaluate(currentValue, newValue, propertyName ?? MissingPropertyName));

    /// <summary>
    /// Propagates the current result and verifies that a property value changed from the current value to the new value.
    /// </summary>
    /// <remarks>Uses caller argument expression metadata to infer the property name when no explicit name is provided.</remarks>
    /// <typeparam name="TProp">The type of the property value being compared.</typeparam>
    /// <param name="result">The async result that must be successful before the modification check runs.</param>
    /// <param name="currentValue">The current property value.</param>
    /// <param name="newValue">The proposed property value.</param>
    /// <param name="propertyName">The property name used in validation messages. If not provided, the argument expression for currentValue is used.</param>
    /// <returns>A successful result containing the new property value when the input result is successful and the value is modified; otherwise, a failed result.</returns>
    public static async Task<Result<TProp>> EnsureModified<TProp>(this Task<Result> result, TProp currentValue, TProp newValue,
        [CallerArgumentExpression(nameof(currentValue))] string? propertyName = null) =>
        await result.Bind(() => Evaluate(currentValue, newValue, propertyName ?? MissingPropertyName));

    /// <summary>
    /// Compares the current and proposed property values and produces a result indicating whether a change occurred.
    /// </summary>
    /// <typeparam name="TProp">The type of the property value being compared.</typeparam>
    /// <param name="currentValue">The existing property value.</param>
    /// <param name="newValue">The proposed property value.</param>
    /// <param name="propertyName">The name of the property associated with the comparison.</param>
    /// <returns>A successful result containing <paramref name="newValue"/> when the values differ; otherwise an <see cref="UnmodifiedFailure"/> for the specified property.</returns>
    private static Result<TProp> Evaluate<TProp>(TProp currentValue, TProp newValue, string propertyName)
    {
        return !EqualityComparer<TProp>.Default.Equals(currentValue, newValue)
            ? Result.Success(newValue)
            : new UnmodifiedFailure(propertyName);
    }
}
