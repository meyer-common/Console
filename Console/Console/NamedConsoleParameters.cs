using Meyer.Common.Console.Exceptions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Meyer.Common.Console;

/// <summary>
/// Represents a collection of INamedConsoleParameter
/// </summary>
public class NamedConsoleParameters : IEnumerable<INamedConsoleParameter>
{
    private readonly List<INamedConsoleParameter> consoleParameters;

    /// <summary>
    /// Instantiates a new instance of NamedConsoleParameters
    /// </summary>
    public NamedConsoleParameters()
    {
        consoleParameters = new List<INamedConsoleParameter>();
    }

    /// <summary>
    /// Adds an INamedConsoleParameter to the collection
    /// </summary>
    /// <param name="value">The object to add</param>
    public void Add(INamedConsoleParameter value)
    {
        consoleParameters.Add(value);

        CheckForDuplicateFlags();
    }

    private void CheckForDuplicateFlags()
    {
        var duplicates = consoleParameters
            .SelectMany(x => x.Ids)
            .GroupBy(x => x)
            .Where(x => x.Count() > 1)
            .ToArray();

        if (duplicates.Any())
        {
            throw new DuplicateConsoleParameterException(duplicates.Select(x => x.Key));
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns> An enumerator that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns> An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<INamedConsoleParameter> GetEnumerator()
    {
        return ((IEnumerable<INamedConsoleParameter>)consoleParameters).GetEnumerator();
    }
}