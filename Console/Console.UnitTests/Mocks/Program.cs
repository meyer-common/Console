using System.IO;

namespace Meyer.Common.Console.UnitTests.Mocks
{
    static class Program
    {
        public static int i;
        public static int ii;
        public static bool b;
        public static string w;
        public static Verb v;
        public static Noun n;
        public static FileInfo f;
        public static DirectoryInfo d;
    }

    enum Verb
    {
        Null,
        Get,
        Push
    }

    enum Noun
    {
        Null,
        Stuff,
        Things
    }
}