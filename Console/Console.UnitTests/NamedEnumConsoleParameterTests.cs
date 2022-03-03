using Meyer.Common.Console.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Meyer.Common.Console.UnitTests;

[TestClass]
public class NamedEnumConsoleParameterTests
{
    [TestInitialize]
    public void TestInitialization()
    {
        Program.v = default;
    }

    [TestMethod]
    public void CanParseEnum()
    {
        var parameter = new NamedEnumConsoleParameter<Verb>(new[] { "v" }, () => Program.v, null);

        var mapped = parameter.PerformMapping(new LinkedList<string>("-v get".Split(' ')));

        Assert.AreEqual(Verb.Get, Program.v);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void NotPresentDoesNotPerformMap()
    {
        var parameter = new NamedEnumConsoleParameter<Verb>(new[] { "v" }, () => Program.v, null);

        var mapped = parameter.PerformMapping(new LinkedList<string>("".Split(' ')));

        Assert.AreEqual(Program.v, default);
        Assert.IsFalse(mapped);
    }

    [TestMethod]
    public void CanParseFirstParameter()
    {
        var parameter = new NamedEnumConsoleParameter<Verb>(new[] { "v" }, () => Program.v, null);

        var mapped = parameter.PerformMapping(new LinkedList<string>("-v get -aaa sadfsdf".Split(' ')));

        Assert.AreEqual(Verb.Get, Program.v);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void CanParseMiddleParameter()
    {
        var parameter = new NamedEnumConsoleParameter<Verb>(new[] { "v" }, () => Program.v, null);

        var mapped = parameter.PerformMapping(new LinkedList<string>("-aaa asdfsad -b -v get -bbb sdfasdf".Split(' ')));

        Assert.AreEqual(Verb.Get, Program.v);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void CanParseLastParameter()
    {
        var parameter = new NamedEnumConsoleParameter<Verb>(new[] { "v" }, () => Program.v, null);

        var mapped = parameter.PerformMapping(new LinkedList<string>("-aaa asdfsad -b -v get".Split(' ')));

        Assert.AreEqual(Verb.Get, Program.v);
        Assert.IsTrue(mapped);
    }

    [TestMethod]
    public void CanAssignMultipleFlags()
    {
        var parameter = new NamedEnumConsoleParameter<Verb>(new[] { "v", "verb" }, () => Program.v, null);

        var mapped1 = parameter.PerformMapping(new LinkedList<string>("-v get".Split(' ')));

        Assert.AreEqual(Verb.Get, Program.v);
        Assert.IsTrue(mapped1);

        var mapped2 = parameter.PerformMapping(new LinkedList<string>("-verb push".Split(' ')));

        Assert.AreEqual(Verb.Push, Program.v);
        Assert.IsTrue(mapped2);
    }

    [TestMethod]
    public void ArgsRemovedOnSuccess()
    {
        var parameter = new NamedEnumConsoleParameter<Verb>(new[] { "v" }, () => Program.v, null);

        var args = new LinkedList<string>("-q -v get -g".Split(' '));

        var mapped = parameter.PerformMapping(args); ;

        Assert.AreEqual(Verb.Get, Program.v);
        Assert.IsTrue(mapped);
        Assert.AreEqual(2, args.Count);
        Assert.IsFalse(args.Contains("-v"));
        Assert.IsFalse(args.Contains("get"));
    }

    [TestMethod]
    public void ToStringText()
    {
        var parameter = new NamedEnumConsoleParameter<Verb>(new[] { "v", "verb" }, () => Program.v, "aaaa");

        Assert.AreEqual("-v -verb : aaaa", parameter.ToString());
    }

    [TestMethod]
    public void ToStringRequiredText()
    {
        var parameter = new NamedEnumConsoleParameter<Verb>(new[] { "v", "verb" }, () => Program.v, "aaaa", true);

        Assert.AreEqual("-v -verb : (Required) aaaa", parameter.ToString());
    }
}