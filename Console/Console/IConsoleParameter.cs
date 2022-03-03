using System.Collections.Generic;

namespace Meyer.Common.Console;

/// <summary>
/// Interface outlines methods for parsing a parameter passed to a program from the command line
/// </summary>
public interface IConsoleParameter
{
    /// <summary>
    /// Gets whether the parameter is required to run the program
    /// </summary>
    bool IsRequired { get; }

    /// <summary>
    /// Gets the description of the parameter (ie: help documentation)
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Parses the list of arguments and performs some action based on the input
    /// </summary>
    /// <param name="args">The arguments passed to the program from the command line</param>
    /// <returns>Returns true if mapping was done</returns>
    bool PerformMapping(LinkedList<string> args);
}