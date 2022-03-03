using System;

namespace Meyer.Common.Console.Exceptions;

/// <summary>
/// Thrown when a required parameter was not passed in input args
/// </summary>
public class MissingRequiredConsoleParameterException : Exception
{
    /// <summary>
    /// Instantiates a new instance of MissingRequiredConsoleParameterException
    /// </summary>
    public MissingRequiredConsoleParameterException() : base("Required command line argument(s) were not provided.") { }
}