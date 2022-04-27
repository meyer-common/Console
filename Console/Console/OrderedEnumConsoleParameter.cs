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
public class OrderedEnumConsoleParameter<T> : IOrderedConsoleParameter where T : struct
{
    private readonly Expression<Func<T>> func;

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
    /// Instantiates a new instance of a EnumConsoleParameter
    /// </summary>
    /// <param name="func">The field to map the paramete</param>
    /// <param name="description">The description of the parameter (ie: help documentation)</param>
    public OrderedEnumConsoleParameter(Expression<Func<T>> func, string description)
    {
        this.func = func;
        Description = description;
        IsRequired = true;
    }

    /// <summary>
    /// Maps the parsed parameter to the specified enum field
    /// </summary>
    /// <param name="args">The arguments passed to the program from the command line</param>
    /// <returns>Returns true if mapping was done</returns>
    public bool PerformMapping(LinkedList<string> args)
    {
        if(!args.Any())
            return false;

        var node = args.First;

        if (!Enum.GetNames(typeof(T))
            .Select(x => x.ToLower())
            .Contains(node.Value.ToLower()))
        {
            return false;
        }

        switch (func.Body.NodeType)
        {
            case ExpressionType.MemberAccess:
                var field = (FieldInfo)((MemberExpression)func.Body).Member;
                field.SetValue(null, Enum.Parse(typeof(T), node.Value, true));
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
        return $"{string.Join(", ", Enum.GetNames(typeof(T)).Skip(1).Select(x => x.ToLower()))} : {(IsRequired ? "(Required) " : "")}{Description}";
    }
}