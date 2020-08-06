using Meyer.Common.Console.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Meyer.Common.Console.UnitTests
{
    [TestClass]
    public class DirectoryConsoleParameterTests
    {
        [TestInitialize]
        public void TestInitialization()
        {
            Program.d = default;
        }

        [TestMethod]
        public void CanParseDirectoryPath()
        {
            var parameter = new DirectoryConsoleParameter(new[] { "d" }, x => Program.d = x, "");

            bool mapped = parameter.PerformMapping(new LinkedList<string>($"-d {Environment.CurrentDirectory}".Split(' ')));

            Assert.IsNotNull(Program.d);
            Assert.AreEqual(Program.d.FullName, Environment.CurrentDirectory);
            Assert.IsTrue(mapped);
        }

        [TestMethod]
        public void CanParseRelativePath()
        {
            var parameter = new DirectoryConsoleParameter(new[] { "d" }, x => Program.d = x, "");

            bool mapped = parameter.PerformMapping(new LinkedList<string>($"-d .\\".Split(' ')));

            Assert.IsNotNull(Program.d);
            Assert.AreEqual(Program.d.FullName.Trim('\\'), $"{Environment.CurrentDirectory}");
            Assert.IsTrue(mapped);
        }

        [TestMethod]
        public void NotPresentDoesNotPerformAction()
        {
            var parameter = new DirectoryConsoleParameter(new[] { "d" }, x => Program.d = x, "");

            bool mapped = parameter.PerformMapping(new LinkedList<string>("".Split(' ')));

            Assert.AreEqual(Program.d, default);
            Assert.IsFalse(mapped);
        }

        [TestMethod]
        public void CanParseFirstParameter()
        {
            var parameter = new DirectoryConsoleParameter(new[] { "d" }, x => Program.d = x, "");

            bool mapped = parameter.PerformMapping(new LinkedList<string>($"-d {Environment.CurrentDirectory} -aaa sadfsdf".Split(' ')));

            Assert.IsNotNull(Program.d);
            Assert.AreEqual(Program.d.FullName, Environment.CurrentDirectory);
            Assert.IsTrue(mapped);
        }

        [TestMethod]
        public void CanParseMiddleParameter()
        {
            var parameter = new DirectoryConsoleParameter(new[] { "d" }, x => Program.d = x, "");

            bool mapped = parameter.PerformMapping(new LinkedList<string>($"-aaa asdfsad -d {Environment.CurrentDirectory} -b -i 5 -bbb sdfasdf".Split(' ')));

            Assert.IsNotNull(Program.d);
            Assert.AreEqual(Program.d.FullName, Environment.CurrentDirectory);
            Assert.IsTrue(mapped);
        }

        [TestMethod]
        public void CanParseLastParameter()
        {
            var parameter = new DirectoryConsoleParameter(new[] { "d" }, x => Program.d = x, "");

            bool mapped = parameter.PerformMapping(new LinkedList<string>($"-aaa asdfsad -b -d {Environment.CurrentDirectory}".Split(' ')));

            Assert.IsNotNull(Program.d);
            Assert.AreEqual(Program.d.FullName, Environment.CurrentDirectory);
            Assert.IsTrue(mapped);
        }

        [TestMethod]
        public void CanParseMultiple()
        {
            HashSet<DirectoryInfo> dirInfos = new HashSet<DirectoryInfo>();

            var parameter = new DirectoryConsoleParameter(new[] { "d" }, x => dirInfos.Add(x), "");

            bool mapped = parameter.PerformMapping(new LinkedList<string>($"-d {Environment.CurrentDirectory} -d {Environment.CurrentDirectory}".Split(' ')));

            Assert.AreEqual(2, dirInfos.Count);
            Assert.AreEqual(dirInfos.First().FullName, Environment.CurrentDirectory);
            Assert.AreEqual(dirInfos.Last().FullName, Environment.CurrentDirectory);
            Assert.IsTrue(mapped);
        }

        [TestMethod]
        public void ArgsRemovedOnSuccess()
        {
            var parameter = new DirectoryConsoleParameter(new[] { "d" }, x => Program.d = x, "");

            var args = new LinkedList<string>($"-q -d {Environment.CurrentDirectory} -g".Split(' '));

            bool mapped = parameter.PerformMapping(args);

            Assert.IsTrue(mapped);
            Assert.AreEqual(2, args.Count);
            Assert.IsFalse(args.Contains("-d"));
            Assert.IsFalse(args.Contains(Environment.CurrentDirectory));
        }

        [TestMethod]
        public void ToStringText()
        {
            var parameter = new DirectoryConsoleParameter(new[] { "d", "dir" }, x => Program.d = x, "aaaa");

            Assert.AreEqual("-d -dir : aaaa", parameter.ToString());
        }

        [TestMethod]
        public void ToStringRequiredText()
        {
            var parameter = new DirectoryConsoleParameter(new[] { "d", "dir" }, x => Program.d = x, "aaaa", true);

            Assert.AreEqual("-d -dir : (Required) aaaa", parameter.ToString());
        }
    }
}