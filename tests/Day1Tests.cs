using AdventOfCode;
using static System.Net.Mime.MediaTypeNames;

namespace tests;

[TestClass]
public class Day1Tests
{
    [TestMethod]
    public void Part2Test()
    {
        var input = Utilities.GetResourceData("day1.txt", "tests");
        var day1 = new Day1(input);

        var sum = day1.SumPart2();

       Assert.AreEqual(281, sum);
    }
}