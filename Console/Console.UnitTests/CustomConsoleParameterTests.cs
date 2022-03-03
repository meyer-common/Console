using Meyer.Common.Console.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Meyer.Common.Console.UnitTests;

[TestClass]
public class CustomConsoleParameterTests
{
    [TestInitialize]
    public void TestInitialization()
    {
        Program.i = default;
    }

    [TestMethod]
    public void CanParseAction()
    {
        var parameter = new ActionConsoleParameter(new[] { "i" }, x => Program.i = int.Parse(x), "");

        var mapped = parameter.PerformMapping(new LinkedList<string>("-i 5".Split(' ')));

        Assert.AreEqual(5, Program.i);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void NotPresentDoesNotPerformAction()
    {
        var parameter = new ActionConsoleParameter(new[] { "i" }, x => Program.i = int.Parse(x), "");

        var mapped = parameter.PerformMapping(new LinkedList<string>("".Split(' ')));

        Assert.AreEqual(Program.i, default);
        Assert.IsFalse(mapped);
    }

    [TestMethod]
    public void CanParseFirstParameter()
    {
        var parameter = new ActionConsoleParameter(new[] { "i" }, x => Program.i = int.Parse(x), "");

        var mapped = parameter.PerformMapping(new LinkedList<string>("-i 5 -aaa sadfsdf".Split(' ')));

        Assert.AreEqual(5, Program.i);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void CanParseMiddleParameter()
    {
        var parameter = new ActionConsoleParameter(new[] { "i" }, x => Program.i = int.Parse(x), "");

        var mapped = parameter.PerformMapping(new LinkedList<string>("-aaa asdfsad -b -i 5 -bbb sdfasdf".Split(' ')));

        Assert.AreEqual(5, Program.i);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void CanParseLastParameter()
    {
        var parameter = new ActionConsoleParameter(new[] { "i" }, x => Program.i = int.Parse(x), "");

        var mapped = parameter.PerformMapping(new LinkedList<string>("-aaa asdfsad -b -i 5".Split(' ')));

        Assert.AreEqual(5, Program.i);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void CanParseMultiple()
    {
        var parameter = new ActionConsoleParameter(new[] { "i" }, x => Program.i += int.Parse(x), "");

        var mapped = parameter.PerformMapping(new LinkedList<string>("-i 5 -i 10".Split(' ')));

        Assert.AreEqual(15, Program.i);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void CanAssignMultipleFlags()
    {
        var parameter = new ActionConsoleParameter(new[] { "i", "int" }, x => Program.i = int.Parse(x), "");

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
        var parameter = new ActionConsoleParameter(new[] { "i" }, x => Program.i = int.Parse(x), "");

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
        var parameter = new ActionConsoleParameter(new[] { "i", "int" }, x => Program.i = int.Parse(x), "aaaa");

        Assert.AreEqual("-i -int : aaaa", parameter.ToString());
    }

    [TestMethod]
    public void ToStringRequiredText()
    {
        var parameter = new ActionConsoleParameter(new[] { "i", "int" }, x => Program.i = int.Parse(x), "aaaa", true);

        Assert.AreEqual("-i -int : (Required) aaaa", parameter.ToString());
    }
}