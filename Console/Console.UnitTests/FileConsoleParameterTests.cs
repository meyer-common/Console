using Meyer.Common.Console.UnitTests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Meyer.Common.Console.UnitTests
{
    [TestClass]
    public class FileConsoleParameterTests
    {
        [TestInitialize]
        public void TestInitialization()
        {
            Program.f = default;
        }

        [TestMethod]
        public void CanParseFilePath()
        {
            var parameter = new FileConsoleParameter(new[] { "f" }, x => Program.f = x, "");

            bool mapped = parameter.PerformMapping(new LinkedList<string>($"-f {Environment.CurrentDirectory}/Meyer.Common.Console.dll".Split(' ')));

            Assert.IsNotNull(Program.f);
            Assert.AreEqual(Program.f.Name, "Meyer.Common.Console.dll");
            Assert.IsTrue(mapped);
        }

        [TestMethod]
        public void CanParseRelativePath()
        {
            var parameter = new FileConsoleParameter(new[] { "f" }, x => Program.f = x, "");

            bool mapped = parameter.PerformMapping(new LinkedList<string>($"-f .{Path.DirectorySeparatorChar}Meyer.Common.Console.dll".Split(' ')));

            Assert.IsNotNull(Program.f);
            Assert.AreEqual(Program.f.FullName, $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}Meyer.Common.Console.dll");
            Assert.IsTrue(mapped);
        }

        [TestMethod]
        public void NotPresentDoesNotPerformAction()
        {
            var parameter = new FileConsoleParameter(new[] { "f" }, x => Program.f = x, "");

            bool mapped = parameter.PerformMapping(new LinkedList<string>("".Split(' ')));

            Assert.AreEqual(Program.f, default);
            Assert.IsFalse(mapped);
        }

        [TestMethod]
        public void CanParseFirstParameter()
        {
            var parameter = new FileConsoleParameter(new[] { "f" }, x => Program.f = x, "");

            bool mapped = parameter.PerformMapping(new LinkedList<string>($"-f {Environment.CurrentDirectory}/Meyer.Common.Console.dll -aaa sadfsdf".Split(' ')));

            Assert.IsNotNull(Program.f);
            Assert.AreEqual(Program.f.Name, "Meyer.Common.Console.dll");
            Assert.IsTrue(mapped);
        }

        [TestMethod]
        public void CanParseMiddleParameter()
        {
            var parameter = new FileConsoleParameter(new[] { "f" }, x => Program.f = x, "");

            bool mapped = parameter.PerformMapping(new LinkedList<string>($"-aaa asdfsad -f {Environment.CurrentDirectory}/Meyer.Common.Console.dll -b -i 5 -bbb sdfasdf".Split(' ')));

            Assert.IsNotNull(Program.f);
            Assert.AreEqual(Program.f.Name, "Meyer.Common.Console.dll");
            Assert.IsTrue(mapped);
        }

        [TestMethod]
        public void CanParseLastParameter()
        {
            var parameter = new FileConsoleParameter(new[] { "f" }, x => Program.f = x, "");

            bool mapped = parameter.PerformMapping(new LinkedList<string>($"-aaa asdfsad -b -f {Environment.CurrentDirectory}/Meyer.Common.Console.dll".Split(' ')));

            Assert.IsNotNull(Program.f);
            Assert.AreEqual(Program.f.Name, "Meyer.Common.Console.dll");
            Assert.IsTrue(mapped);
        }

        [TestMethod]
        public void CanParseMultiple()
        {
            HashSet<FileInfo> fileInfos = new HashSet<FileInfo>();

            var parameter = new FileConsoleParameter(new[] { "f" }, x => fileInfos.Add(x), "");

            bool mapped = parameter.PerformMapping(new LinkedList<string>($"-f {Environment.CurrentDirectory}/Meyer.Common.Console.dll -f {Environment.CurrentDirectory}/Meyer.Common.Console.pdb".Split(' ')));

            Assert.AreEqual(2, fileInfos.Count);
            Assert.AreEqual(fileInfos.First().Name, "Meyer.Common.Console.dll");
            Assert.AreEqual(fileInfos.Last().Name, "Meyer.Common.Console.pdb");
            Assert.IsTrue(mapped);
        }

        [TestMethod]
        public void ArgsRemovedOnSuccess()
        {
            var parameter = new FileConsoleParameter(new[] { "f" }, x => Program.f = x, "");

            var args = new LinkedList<string>($"-q -f {Environment.CurrentDirectory}/Meyer.Common.Console.dll -g".Split(' '));

            bool mapped = parameter.PerformMapping(args);

            Assert.IsTrue(mapped);
            Assert.AreEqual(2, args.Count);
            Assert.IsFalse(args.Contains("-f"));
            Assert.IsFalse(args.Contains("Meyer.Common.Console.dll"));
        }

        [TestMethod]
        public void ToStringText()
        {
            var parameter = new FileConsoleParameter(new[] { "f", "file" }, x => Program.f = x, "aaaa");

            Assert.AreEqual("-f -file : aaaa", parameter.ToString());
        }

        [TestMethod]
        public void ToStringRequiredText()
        {
            var parameter = new FileConsoleParameter(new[] { "f", "file" }, x => Program.f = x, "aaaa", true);

            Assert.AreEqual("-f -file : (Required) aaaa", parameter.ToString());
        }
    }
}