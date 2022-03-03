using System;
using System.Collections.Generic;

namespace Meyer.Common.Console.Exceptions;

/// <summary>
/// Thrown when an INamedConsoleParameter contains parameters with duplicate ID's
/// </summary>
public class DuplicateConsoleParameterException : Exception
{
    /// <summary>
    /// Instantiates a new instance of DuplicateConsoleParameterException
    /// </summary>
    /// <param name="ids">The duplicate ID's</param>
    public DuplicateConsoleParameterException(IEnumerable<string> ids) : base($"Ambiguous mapping found for option(s) {string.Join(",", ids)}. Flag(s) defined more than once") { }
}