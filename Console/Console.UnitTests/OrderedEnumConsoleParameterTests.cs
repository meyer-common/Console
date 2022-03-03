using Meyer.Common.Console.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Meyer.Common.Console.UnitTests;

[TestClass]
public class EnumConsoleParameterTests
{
    [TestInitialize]
    public void TestInitialization()
    {
        Program.v = default;
    }

    [TestMethod]
    public void CanParseValidValue()
    {
        var parameter = new OrderedEnumConsoleParameter<Verb>(() => Program.v, "");

        var mapped = parameter.PerformMapping(new LinkedList<string>("get".Split(' ')));

        Assert.AreEqual(Verb.Get, Program.v);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void NotPresentDoesNotMap()
    {
        var parameter = new OrderedEnumConsoleParameter<Verb>(() => Program.v, "");

        var mapped = parameter.PerformMapping(new LinkedList<string>("".Split(' ')));

        Assert.AreEqual(Verb.Null, Program.v);
        Assert.IsFalse(mapped);
    }

    [TestMethod]
    public void CanParseWithOtherArgs()
    {
        var parameter = new OrderedEnumConsoleParameter<Verb>(() => Program.v, "");

        var mapped = parameter.PerformMapping(new LinkedList<string>("get -aaa asdfsad -b -v -bbb sdfasdf".Split(' ')));

        Assert.AreEqual(Verb.Get, Program.v);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void CannotParseUndefinedParameter()
    {
        var parameter = new OrderedEnumConsoleParameter<Verb>(() => Program.v, "");

        var mapped = parameter.PerformMapping(new LinkedList<string>("dfdsfdsf -aaa asdfsad -b -v -bbb sdfasdf".Split(' ')));

        Assert.AreEqual(Verb.Null, Program.v);
        Assert.IsFalse(mapped);
    }

    [TestMethod]
    public void ArgsRemovedOnSuccess()
    {
        var parameter = new OrderedEnumConsoleParameter<Verb>(() => Program.v, "");

        var args = new LinkedList<string>("get -b -g".Split(' '));

        var mapped = parameter.PerformMapping(args); ;

        Assert.AreEqual(Verb.Get, Program.v);
        Assert.IsTrue(mapped);
        Assert.AreEqual(2, args.Count);
        Assert.IsFalse(args.Contains("get"));
    }

    [TestMethod]
    public void ToStringText()
    {
        var parameter = new OrderedEnumConsoleParameter<Verb>(() => Program.v, "aaaa");

        Assert.AreEqual("get, push : (Required) aaaa", parameter.ToString());
    }
}