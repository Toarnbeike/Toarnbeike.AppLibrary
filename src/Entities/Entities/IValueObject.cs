namespace Toarnbeike.AppLibrary.Entities;

/// <summary>
/// Defines a contract for value objects that are compared based on their values rather than their identities.
/// </summary>
/// <remarks>
/// Implement this interface to indicate that an object represents a value in the domain and should be
/// considered equal to other instances with the same value. Value objects should be implemented immutable 
/// and do not have a distinct identity.
/// </remarks>
public interface IValueObject;
