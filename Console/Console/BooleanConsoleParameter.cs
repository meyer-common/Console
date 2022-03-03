using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Meyer.Common.Console;

/// <summary>
/// Implements a basic console parameter that indicates the presence of a parameter
/// </summary>
public class BooleanConsoleParameter : INamedConsoleParameter
{
    private readonly Expression<Func<bool>> func;

    /// <summary>
    /// Gets a list of possible parameter names to uniquely identify the parameter
    /// </summary>
    public string[] Ids { get; }

    /// <summary>
    /// Gets the description of the parameter (ie: help documentation)
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets whether the parameter is required to run the program. Always returns false
    /// </summary>
    public bool IsRequired { get; }

    /// <summary>
    /// Instantiates a new instance of a BooleanConsoleParameter
    /// </summary>
    /// <param name="ids">A list of possible parameter names to uniquely identify the parameter</param>
    /// <param name="func">The field to map the parameter</param>
    /// <param name="description">The description of the parameter (ie: help documentation)</param>
    public BooleanConsoleParameter(string[] ids, Expression<Func<bool>> func, string description)
    {
        Ids = ids;
        this.func = func;
        Description = description;
        IsRequired = false;
    }

    /// <summary>
    /// Maps the parsed parameter to the specified boolean field
    /// </summary>
    /// <param name="args">The arguments passed to the program from the command line</param>
    /// <returns>Returns true if mapping was done</returns>
    public bool PerformMapping(LinkedList<string> args)
    {
        var node = args.Find(args.SingleOrDefault(x => Ids.Contains($"{x.Replace("-", "")}")));
        if (node == null)
        {
            return true;
        }

        switch (func.Body.NodeType)
        {
            case ExpressionType.MemberAccess:
                var field = (FieldInfo)((MemberExpression)func.Body).Member;
                field.SetValue(null, true);
                break;
            default:
                throw new InvalidOperationException();
        }

        args.Remove(node);

        return true;
    }

    /// <summary>
    /// Overridden to display help text
    /// </summary>
    /// <returns>Help text as string</returns>
    public override string ToString()
    {
        return $"{string.Join(" ", Ids.Select(x => $"-{x}"))} : {Description}";
    }
}