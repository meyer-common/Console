using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Meyer.Common.Console;

/// <summary>
/// Implements a console parameter that must be one of a set of predefined values
/// </summary>
/// <typeparam name="T">The Type of the Enum field</typeparam>
public class NamedEnumConsoleParameter<T> : INamedConsoleParameter where T : struct
{
    private readonly Expression<Func<T>> func;

    /// <summary>
    /// Gets a list of possible parameter names to uniquely identify the parameter
    /// </summary>
    public string[] Ids { get; }

    /// <summary>
    /// Gets the description of the parameter (ie: help documentation)
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets whether the parameter is required to run the program. Returns true, if the parameter is required.
    /// Returns false, if the parameter is optional
    /// </summary>
    public bool IsRequired { get; }

    /// <summary>
    /// Instantiates a new instance of NamedEnumConsoleParameter
    /// </summary>
    /// <param name="ids">A list of possible parameter names to uniquely identify the parameter</param>
    /// <param name="func">The field to map the parameter</param>
    /// <param name="description">The description of the parameter (ie: help documentation)</param>
    /// <param name="isRequired">Indicates whether the parameter is reqiured. Default is optional (false)</param>
    public NamedEnumConsoleParameter(string[] ids, Expression<Func<T>> func, string description, bool isRequired = false)
    {
        Ids = ids;
        this.func = func;
        Description = description;
        IsRequired = isRequired;
    }

    /// <summary>
    /// Maps the parsed parameter to the specified Enum field
    /// </summary>
    /// <param name="args">The arguments passed to the program from the command line</param>
    /// <returns>Returns true if mapping was done</returns>
    public bool PerformMapping(LinkedList<string> args)
    {
        var node = args.Find(args.SingleOrDefault(x => Ids.Contains($"{x.Replace("-", "")}")));
        if (node == null || node.Next == null)
        {
            return false;
        }

        if (!Enum.GetNames(typeof(T))
            .Select(x => x.ToLower())
            .Contains(node.Next.Value.ToLower()))
        {
            return false;
        }

        switch (func.Body.NodeType)
        {
            case ExpressionType.MemberAccess:
                var field = (FieldInfo)((MemberExpression)func.Body).Member;
                field.SetValue(null, Enum.Parse(typeof(T), node.Next.Value, true));
                break;
            default:
                throw new InvalidOperationException();
        }

        args.Remove(node.Next);
        args.Remove(node);

        return true;
    }

    /// <summary>
    /// Overridden to display help text
    /// </summary>
    /// <returns>Help text as string</returns>
    public override string ToString()
    {
        return $"{string.Join(" ", Ids.Select(x => $"-{x}"))} : {(IsRequired ? "(Required) " : "")}{Description}";
    }
}