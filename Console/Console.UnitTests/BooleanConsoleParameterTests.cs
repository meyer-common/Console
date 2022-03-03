using Meyer.Common.Console.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Meyer.Common.Console.UnitTests;

[TestClass]
public class BooleanConsoleParameterTests
{
    [TestInitialize]
    public void TestInitialization()
    {
        Program.b = default;
    }

    [TestMethod]
    public void CanParseBoolTrue()
    {
        var parameter = new BooleanConsoleParameter(new[] { "b" }, () => Program.b, "");

        var mapped = parameter.PerformMapping(new LinkedList<string>("-b".Split(' ')));

        Assert.IsTrue(Program.b);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void BoolNotPresentMapsTrue()
    {
        var parameter = new BooleanConsoleParameter(new[] { "b" }, () => Program.b, "");

        var mapped = parameter.PerformMapping(new LinkedList<string>("".Split(' ')));

        Assert.IsFalse(Program.b);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void CanParseBoolFirstParameter()
    {
        var parameter = new BooleanConsoleParameter(new[] { "b" }, () => Program.b, "");

        var mapped = parameter.PerformMapping(new LinkedList<string>("-b -aaa sadfsdf".Split(' ')));

        Assert.IsTrue(Program.b);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void CanParseBoolMiddleParameter()
    {
        var parameter = new BooleanConsoleParameter(new[] { "b" }, () => Program.b, "");

        var mapped = parameter.PerformMapping(new LinkedList<string>("-aaa asdfsad -b -bbb sdfasdf".Split(' ')));

        Assert.IsTrue(Program.b);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void CanParseBoolMultipleBoolParameter()
    {
        var parameter = new BooleanConsoleParameter(new[] { "b" }, () => Program.b, "");

        var mapped = parameter.PerformMapping(new LinkedList<string>("-aaa asdfsad -b -v -bbb sdfasdf".Split(' ')));

        Assert.IsTrue(Program.b);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void CanParseBoolLastParameter()
    {
        var parameter = new BooleanConsoleParameter(new[] { "b" }, () => Program.b, "");

        var mapped = parameter.PerformMapping(new LinkedList<string>("-aaa asdfsad -b".Split(' ')));

        Assert.IsTrue(Program.b);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void CanAssignMultipleFlags()
    {
        var parameter = new BooleanConsoleParameter(new[] { "b", "c" }, () => Program.b, "");

        var mapped1 = parameter.PerformMapping(new LinkedList<string>("-b".Split(' ')));

        Assert.IsTrue(Program.b);
        Assert.IsTrue(mapped1);

        var mapped2 = parameter.PerformMapping(new LinkedList<string>("-c".Split(' ')));

        Assert.IsTrue(Program.b);
        Assert.IsTrue(mapped2);
    }

    [TestMethod]
    public void ArgsRemovedOnSuccess()
    {
        var parameter = new BooleanConsoleParameter(new[] { "b" }, () => Program.b, "");

        var args = new LinkedList<string>("-q -b -g".Split(' '));

        var mapped = parameter.PerformMapping(args); ;

        Assert.IsTrue(Program.b);
        Assert.IsTrue(mapped);
        Assert.AreEqual(2, args.Count);
        Assert.IsFalse(args.Contains("-b"));
    }

    [TestMethod]
    public void ToStringText()
    {
        var parameter = new BooleanConsoleParameter(new[] { "b", "c" }, () => Program.b, "aaaa");

        Assert.AreEqual("-b -c : aaaa", parameter.ToString());
    }
}