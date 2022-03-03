using Meyer.Common.Console.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Meyer.Common.Console;

/// <summary>
/// Represents a collection of IConsoleParameter
/// </summary>
public class ConsoleParameters
{
    /// <summary>
    /// Gets a collection of IOrderedConsoleParameter
    /// </summary>
    public OrderedConsoleParameters OrderedConsoleParameters { get; }

    /// <summary>
    /// Gets a collection of NamedConsoleParameters
    /// </summary>
    public NamedConsoleParameters NamedConsoleParameters { get; }

    /// <summary>
    /// Instantiates a new instance of ConsoleParameters
    /// </summary>
    public ConsoleParameters()
    {
        OrderedConsoleParameters = new OrderedConsoleParameters();
        NamedConsoleParameters = new NamedConsoleParameters();
    }

    /// <summary>
    /// Executes the action for each argument flag
    /// </summary>
    /// <param name="args">The arguments passed to the program from the command line</param>
    /// <param name="ignoreExtraParameters"></param>
    public void Map(string[] args, bool ignoreExtraParameters)
    {
        var argsList = new LinkedList<string>(args);

        foreach (var parameter in AllParameters)
        {
            var performed = parameter.PerformMapping(argsList);

            if (!performed && parameter.IsRequired)
            {
                throw new MissingRequiredConsoleParameterException();
            }
        }

        if (argsList.Any() && !ignoreExtraParameters)
        {
            throw new UndefinedConsoleParameterException(argsList);
        }
    }

    /// <summary>
    /// Gets a collection of all the OrderedConsoleParameters and NamedConsoleParameters
    /// </summary>
    public IEnumerable<IConsoleParameter> AllParameters => Array.Empty<IConsoleParameter>()
            .Union(OrderedConsoleParameters)
            .Union(NamedConsoleParameters)
            .ToArray();

    /// <summary>
    /// Overridden to output each parameter object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Join(Environment.NewLine + Environment.NewLine, AllParameters.Select(x => x.ToString()));
    }
}