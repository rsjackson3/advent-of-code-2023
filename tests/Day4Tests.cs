using AdventOfCode;

namespace tests;

[TestClass]
public class Day4Tests
{
    public Day4Tests()
    {
        Input = Utilities.GetResourceData("day4.txt", "tests");

    }

    private string Input { get; }

    [TestMethod]
    public void Part1()
    {
        var day4 = new Day4(Input);

        var sum = day4.Part1();

        Assert.AreEqual(13, sum);
    }

    [TestMethod]
    public void Part2()
    {
        var day4 = new Day4(Input);

        var sum = day4.Part2();
        var sumOptimized = day4.Part2Optimized();

        Assert.AreEqual(30, sum);
        Assert.AreEqual(30, sumOptimized);
    }
}