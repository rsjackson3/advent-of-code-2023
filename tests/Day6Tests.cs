using AdventOfCode;

namespace tests;

[TestClass]
public class Day6Tests
{
    public Day6Tests()
    {
        Input = Utilities.GetResourceData("day6.txt", "tests");
    }

    private string Input { get; }

    [TestMethod]
    public void Part1Test()
    {
        var day6 = new Day6(Input);

        Assert.AreEqual(288, day6.Part1());
    }

    [TestMethod]
    public void Part2Test()
    {
        var day6 = new Day6(Input);

        Assert.AreEqual(71503, day6.Part2());
    }

    [TestMethod]
    public void GetRaceInfoTest()
    {
        var day6 = new Day6(Input);
        List<int> expectedTimes = new List<int>() { 7, 15, 30 };
        List<int> expectedDistances = new List<int>() { 9, 40, 200 };
        Assert.AreEqual(3, day6.Races.Count);

        for (int i = 0; i < day6.Races.Count; i++)
        {
            var race = day6.Races[i];
            Assert.AreEqual(expectedTimes[i], race.Time);
            Assert.AreEqual(expectedDistances[i], race.Distance);
        }
    }

    [TestMethod]
    public void WaysToWinTest()
    {
        var day6 = new Day6(Input);

        Assert.AreEqual(4, day6.NumberOfWaysToWin(day6.Races[0]));
    }
}