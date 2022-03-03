namespace Meyer.Common.Console;

/// <summary>
/// Interface outlines methods for parsing a parameter passed to a program based on key
/// </summary>
public interface INamedConsoleParameter : IConsoleParameter
{
    /// <summary>
    /// Gets a list of possible parameter names to uniquely identify a parameter
    /// </summary>
    string[] Ids { get; }
}