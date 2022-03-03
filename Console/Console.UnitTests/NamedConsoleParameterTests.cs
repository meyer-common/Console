using Meyer.Common.Console.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Meyer.Common.Console.UnitTests;

[TestClass]
public class NamedConsoleParameterTests
{
    [TestInitialize]
    public void TestInitialization()
    {
        Program.i = default;
    }

    [TestMethod]
    public void CanParseInteger()
    {
        var parameter = new NamedConsoleParameter<int>(new[] { "i" }, () => Program.i, null);

        var mapped = parameter.PerformMapping(new LinkedList<string>("-i 5".Split(' ')));

        Assert.AreEqual(5, Program.i);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void NotPresentDoesNotPerformMap()
    {
        var parameter = new NamedConsoleParameter<int>(new[] { "i" }, () => Program.i, null);

        var mapped = parameter.PerformMapping(new LinkedList<string>("".Split(' ')));

        Assert.AreEqual(Program.i, default);
        Assert.IsFalse(mapped);
    }

    [TestMethod]
    public void CanParseFirstParameter()
    {
        var parameter = new NamedConsoleParameter<int>(new[] { "i" }, () => Program.i, null);

        var mapped = parameter.PerformMapping(new LinkedList<string>("-i 5 -aaa sadfsdf".Split(' ')));

        Assert.AreEqual(5, Program.i);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void CanParseMiddleParameter()
    {
        var parameter = new NamedConsoleParameter<int>(new[] { "i" }, () => Program.i, null);

        var mapped = parameter.PerformMapping(new LinkedList<string>("-aaa asdfsad -b -i 5 -bbb sdfasdf".Split(' ')));

        Assert.AreEqual(5, Program.i);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void CanParseLastParameter()
    {
        var parameter = new NamedConsoleParameter<int>(new[] { "i" }, () => Program.i, null);

        var mapped = parameter.PerformMapping(new LinkedList<string>("-aaa asdfsad -b -i 5".Split(' ')));

        Assert.AreEqual(5, Program.i);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void CanAssignMultipleFlags()
    {
        var parameter = new NamedConsoleParameter<int>(new[] { "i", "int" }, () => Program.i, null);

        var mapped1 = parameter.PerformMapping(new LinkedList<string>("-i 5".Split(' ')));

        Assert.AreEqual(5, Program.i);
        Assert.IsTrue(mapped1);

        var mapped2 = parameter.PerformMapping(new LinkedList<string>("-int 5".Split(' ')));

        Assert.AreEqual(5, Program.i);
        Assert.IsTrue(mapped2);
    }

    [TestMethod]
    public void ArgsRemovedOnSuccess()
    {
        var parameter = new NamedConsoleParameter<int>(new[] { "i" }, () => Program.i, null);

        var args = new LinkedList<string>("-q -i 5 -g".Split(' '));

        var mapped = parameter.PerformMapping(args); ;

        Assert.AreEqual(5, Program.i);
        Assert.IsTrue(mapped);
        Assert.AreEqual(2, args.Count);
        Assert.IsFalse(args.Contains("-i"));
        Assert.IsFalse(args.Contains("5"));
    }

    [TestMethod]
    public void ToStringText()
    {
        var parameter = new NamedConsoleParameter<int>(new[] { "i", "int" }, () => Program.i, "aaaa");

        Assert.AreEqual("-i -int : aaaa", parameter.ToString());
    }

    [TestMethod]
    public void ToStringRequiredText()
    {
        var parameter = new NamedConsoleParameter<int>(new[] { "i", "int" }, () => Program.i, "aaaa", true);

        Assert.AreEqual("-i -int : (Required) aaaa", parameter.ToString());
    }
}