using System.IO;

namespace Meyer.Common.Console.UnitTests.Mocks;

internal static class Program
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

internal enum Verb
{
    Null,
    Get,
    Push
}

internal enum Noun
{
    Null,
    Stuff,
    Things
}