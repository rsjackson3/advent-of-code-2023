using AdventOfCode;

namespace tests;

[TestClass]
public class Day3Tests
{
    public Day3Tests()
    {
        Input = Utilities.GetResourceData("day3.txt", "tests");
    }

    private string Input { get; }

    [TestMethod]
    public void Part1Test()
    {
        var day3 = new Day3(Input);

        var sum = day3.SumPartNumbers();

        Assert.AreEqual(4361, sum);
    }

    [TestMethod]
    public void Part2Test()
    {
        var day3 = new Day3(Input);

        var sum = day3.Part2();

        Assert.AreEqual(467835, sum);
    }
}