using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Meyer.Common.Console
{
    /// <summary>
    /// Implements a type of console parameter that parses a DirectoryInfo
    /// </summary>
    public class DirectoryConsoleParameter : INamedConsoleParameter
    {
        private readonly Action<DirectoryInfo> action;

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
        /// Instantiates a new instance of a DirectoryConsoleParameter
        /// </summary>
        /// <param name="ids">A list of possible parameter names to uniquely identify the parameter</param>
        /// <param name="action">The action to execute when the parameter is parsing</param>
        /// <param name="description">The description of the parameter (ie: help documentation)</param>
        /// <param name="isRequired">Indicates whether the parameter is reqiured. Default is optional (false)</param>
        public DirectoryConsoleParameter(string[] ids, Action<DirectoryInfo> action, string description, bool isRequired = false)
        {
            this.Ids = ids;
            this.action = action;
            this.Description = description;
            this.IsRequired = isRequired;
        }

        /// <summary>
        /// Executes the custom action
        /// </summary>
        /// <param name="args">The arguments passed to the program from the command line</param>
        /// <returns>Returns true if mapping was done</returns>
        public bool PerformMapping(LinkedList<string> args)
        {
            var keys = args.Where(x => this.Ids.Contains($"{x.Replace("-", "")}")).ToArray();
            if (!keys.Any())
                return false;

            foreach (var key in keys)
            {
                var node = args.Find(key);

                if (node.Next == null)
                    return false;

                if (Path.IsPathRooted(node.Next.Value))
                    this.action(new DirectoryInfo(node.Next.Value));
                else
                    this.action(new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, node.Next.Value)));

                args.Remove(node.Next);
                args.Remove(node);
            }

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