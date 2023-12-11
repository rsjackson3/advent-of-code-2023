using AdventOfCode;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace tests;

[TestClass]
public class Day2Tests
{
    [TestMethod]
    public void Part1Test()
    {
        var input = Utilities.GetResourceData("day2.txt", "tests");
        var day2 = new Day2(input, 12, 13, 14);

        var sum = day2.SumPossibleGames();

        Assert.AreEqual(8, sum);
    }

    [TestMethod]
    public void Part2Test()
    {
        var input = Utilities.GetResourceData("day2.txt", "tests");
        var day2 = new Day2(input, 12, 13, 14);

        var sum = day2.FewestNumberOfCubes();

        Assert.AreEqual(2286, sum);
    }
}