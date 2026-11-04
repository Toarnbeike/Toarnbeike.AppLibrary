using Toarnbeike.AppLibrary.Repositories;
using Toarnbeike.Results;

namespace Toarnbeike.AppLibrary.State;

/// <summary>
/// The snapshot store is the manager that handles loading and saving snapshots with the chosen persistence method.
/// This is the last boundary between generic appliction libarry code and the persistence specific implementations.
/// Depending on the implementation, multiple states (backups) can be stored and retrieved by identifier, or only the latest state can be stored and retrieved.
/// </summary>
public interface ISnapshotStore
{
    /// <summary>
    /// Get the state name of the latest stored state.
    /// </summary>
    /// <returns>The identifier of the last saved state. This identifier can be used in the <see cref="Read"/> method.</returns>
    string GetLatestState();

    /// <summary>
    /// Get the state name for a custom identifier.
    /// This identifier can be obtained using the method <see cref="GetAvailableIdentifiers"/>
    /// </summary>
    /// <param name="identifier">The name of the identifier to get the state name for.</param>
    /// <returns>The state name corresponding to the given identifier.</returns>
    string GetState(string identifier);

    /// <summary>
    /// Gets the identifiers that are currently available.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> of available identifier strings.</returns>
    IEnumerable<string> GetAvailableIdentifiers();

    /// <summary>
    /// Read the current state 
    /// </summary>
    /// <param name="state">The name of the state to read.</param>
    /// <param name="aggregateStore">The aggregate store to read the state into.</param>
    /// <returns>The result of the read operation.</returns>
    Result Read(string state, IAggregateStore aggregateStore);

    /// <summary>
    /// Write the current state to the data store.
    /// </summary>
    /// <param name="aggregateStore">The aggregate store containing the state to write.</param>
    /// <returns>The result of the write operation.</returns>
    Result WriteAll(IAggregateStore aggregateStore);
}