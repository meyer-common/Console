using System;
using System.Collections.Generic;

namespace Meyer.Common.Console.Exceptions;

/// <summary>
/// Thrown when a mapping cannot be determined for input args
/// </summary>
public class UndefinedConsoleParameterException : Exception
{
    /// <summary>
    /// Instantiates a new instance of UndefinedConsoleParameterException
    /// </summary>
    /// <param name="argsList">A list of args that could not be mapped</param>
    public UndefinedConsoleParameterException(IEnumerable<string> argsList) : base($"Error performing mapping for \"{string.Join(" ", argsList)}\". Undefined") { }
}