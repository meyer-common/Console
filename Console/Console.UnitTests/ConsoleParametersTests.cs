using Meyer.Common.Console.Exceptions;
using Meyer.Common.Console.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Meyer.Common.Console.UnitTests;

[TestClass]
public class ConsoleParametersTests
{
    [TestMethod]
    public void CanDefineConsoleParameters()
    {
        var options = new ConsoleParameters
        {
            OrderedConsoleParameters =
            {
                new OrderedEnumConsoleParameter<Verb>(null, null),
                new OrderedEnumConsoleParameter<Verb>(null, null)
            },
            NamedConsoleParameters =
            {
                new NamedConsoleParameter<object>(new[] { "w" }, null, ""),
                new BooleanConsoleParameter(new[] { "b" }, null, ""),
                new ActionConsoleParameter(new[] { "c" }, null, "")
            }
        };

        Assert.AreEqual(2, options.OrderedConsoleParameters.Count());
        Assert.AreEqual(3, options.NamedConsoleParameters.Count());
    }

    [TestMethod]
    public void AllParametersReturnsAllParameters()
    {
        var options = new ConsoleParameters
        {
            OrderedConsoleParameters =
            {
                new OrderedEnumConsoleParameter<Verb>(null, null),
                new OrderedEnumConsoleParameter<Verb>(null, null)
            },
            NamedConsoleParameters =
            {
                new NamedConsoleParameter<object>(new[] { "w" }, null, ""),
                new BooleanConsoleParameter(new[] { "b" }, null, ""),
                new ActionConsoleParameter(new[] { "c" }, null, "")
            }
        };

        Assert.AreEqual(5, options.AllParameters.Count());
    }

    [TestMethod, ExpectedException(typeof(DuplicateConsoleParameterException))]
    public void DuplicateParameterDefinitionThrowsException()
    {
        var options = new ConsoleParameters
        {
            NamedConsoleParameters =
            {
                new BooleanConsoleParameter(new[] { "b", "oobl" }, () => Program.b, ""),
                new NamedConsoleParameter<int>(new[] { "i", "b" }, () => Program.i, "")
            }
        };
    }

    [TestMethod, ExpectedException(typeof(UndefinedConsoleParameterException))]
    public void UndefinedParamtersThrowsException()
    {
        var options = new ConsoleParameters
        {
            OrderedConsoleParameters =
            {
                new OrderedEnumConsoleParameter<Verb>(() => Program.v, "")
            },
            NamedConsoleParameters =
            {
                new BooleanConsoleParameter(new[] { "b" }, () => Program.b, "")
            }
        };

        options.Map("get -ASD 4".Split(' '), false);
    }

    [TestMethod]
    public void IgnoreUndefinedParamtersDoesNotThrowException()
    {
        var options = new ConsoleParameters
        {
            OrderedConsoleParameters =
            {
                new OrderedEnumConsoleParameter<Verb>(() => Program.v, "")
            },
            NamedConsoleParameters =
            {
                new BooleanConsoleParameter(new[] { "b" }, () => Program.b, "")
            }
        };

        options.Map("get -ASD 4".Split(' '), true);
    }

    [TestMethod, ExpectedException(typeof(MissingRequiredConsoleParameterException))]
    public void MissingOrderedConsoleParameterThrowsException()
    {
        var options = new ConsoleParameters
        {
            OrderedConsoleParameters =
            {
                new OrderedEnumConsoleParameter<Verb>(() => Program.v, "")
            },
            NamedConsoleParameters =
            {
                new BooleanConsoleParameter(new[] { "b" }, () => Program.b, "")
            }
        };

        options.Map("-b -ASD 4".Split(' '), false);
    }

    [TestMethod, ExpectedException(typeof(MissingRequiredConsoleParameterException))]
    public void MissingNamedConsoleParameterThrowsException()
    {
        var options = new ConsoleParameters
        {
            OrderedConsoleParameters =
            {
                new OrderedEnumConsoleParameter<Verb>(() => Program.v, "")
            },
            NamedConsoleParameters =
            {
                new NamedConsoleParameter<int>(new[] { "i", "aaa" }, () => Program.i, "", true)
            }
        };

        options.Map("get".Split(' '), false);
    }

    [TestMethod]
    public void ToStringOutputsHelp()
    {
        var options = new ConsoleParameters
        {
            OrderedConsoleParameters =
            {
                new OrderedEnumConsoleParameter<Verb>(() => Program.v, "AAAAAAA"),
                new OrderedEnumConsoleParameter<Noun>(() => Program.n, "ZZZZZ")
            },
            NamedConsoleParameters =
            {
                new NamedConsoleParameter<int>(new[] { "i", "ASD" }, () => Program.i, "AAAAAAA", true),
                new BooleanConsoleParameter(new[] { "b", "ADA" }, () => Program.b, "BBBBBBBBB"),
                new ActionConsoleParameter(new[] { "a", "ASDASD" }, x => Program.w = x += "w", "BDSDFSD")
            }
        };

        var expected = $"get, push : (Required) AAAAAAA{Environment.NewLine}{Environment.NewLine}stuff, things : (Required) ZZZZZ{Environment.NewLine}{Environment.NewLine}-i -ASD : (Required) AAAAAAA{Environment.NewLine}{Environment.NewLine}-b -ADA : BBBBBBBBB{Environment.NewLine}{Environment.NewLine}-a -ASDASD : BDSDFSD";

        Assert.AreEqual(expected, options.ToString());
    }
}