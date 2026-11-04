namespace Toarnbeike.AppLibrary.Snapshots.Implementation.Validations;

/// <summary>
/// The snapshot registry consists of definitions and migrators.
/// When building the registry, we need to validate that the definitions and migrators are consistent and complete.
/// Different validation steps can each implement this interface and be run in sequence to validate the registry before it is built.
/// </summary>
public interface ISnapshotRegistryValidator
{
    /// <summary>
    /// Validate one aspect of the snapshot registry.
    /// Should throw an <see cref="InvalidOperationException"/> when the validation fails, with a message describing the problem with the registered snapshots.
    /// </summary>
    void Validate();
}