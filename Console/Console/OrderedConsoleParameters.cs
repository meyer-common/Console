using System.Collections;
using System.Collections.Generic;

namespace Meyer.Common.Console;

/// <summary>
/// Represents a collection of OrderedConsoleParameters
/// </summary>
public class OrderedConsoleParameters : IEnumerable<IOrderedConsoleParameter>
{
    private readonly List<IOrderedConsoleParameter> consoleParameters;

    /// <summary>
    /// Instantiates a new instance of OrderedConsoleParameters
    /// </summary>
    public OrderedConsoleParameters()
    {
        consoleParameters = new List<IOrderedConsoleParameter>();
    }

    /// <summary>
    /// Adds an INamedConsoleParameter to the collection
    /// </summary>
    /// <param name="value">The object to add</param>
    public void Add(IOrderedConsoleParameter value)
    {
        consoleParameters.Add(value);
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
    public IEnumerator<IOrderedConsoleParameter> GetEnumerator()
    {
        return ((IEnumerable<IOrderedConsoleParameter>)consoleParameters).GetEnumerator();
    }
}