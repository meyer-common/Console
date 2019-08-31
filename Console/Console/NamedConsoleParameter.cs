﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Meyer.Common.Console
{
    /// <summary>
    /// Implements a basic console parameter that automatically maps to the specified field
    /// </summary>
    /// <typeparam name="T">The Type of the field</typeparam>
    public class NamedConsoleParameter<T> : INamedConsoleParameter
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
        /// Instantiates a new instance of a ConsoleParameter
        /// </summary>
        /// <param name="ids">A list of possible parameter names to uniquely identify the parameter</param>
        /// <param name="func">The field to map the parameter</param>
        /// <param name="description">The description of the parameter (ie: help documentation)</param>
        /// <param name="isRequired">Indicates whether the parameter is reqiured. Default is optional (false)</param>
        public NamedConsoleParameter(string[] ids, Expression<Func<T>> func, string description, bool isRequired = false)
        {
            this.Ids = ids;
            this.func = func;
            this.Description = description;
            this.IsRequired = isRequired;
        }

        /// <summary>
        /// Maps the parsed parameter to the specified T field
        /// </summary>
        /// <param name="args">The arguments passed to the program from the command line</param>
        /// <returns>Returns true if mapping was done</returns>
        public bool PerformMapping(LinkedList<string> args)
        {
            var node = args.Find(args.SingleOrDefault(x => this.Ids.Contains($"{x.Replace("-", "")}")));
            if (node == null || node.Next == null)
                return false;

            switch (this.func.Body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    FieldInfo field = (FieldInfo)((MemberExpression)this.func.Body).Member;
                    if (typeof(T) != typeof(string))
                        field.SetValue(null, Convert.ChangeType(node.Next.Value, typeof(T)));
                    else
                        field.SetValue(null, node.Next.Value);
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
            return $"{string.Join(" ", this.Ids.Select(x => $"-{x}"))} : {(this.IsRequired ? "(Required) " : "")}{this.Description}";
        }
    }
}