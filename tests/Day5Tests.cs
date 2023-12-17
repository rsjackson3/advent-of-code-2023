using AdventOfCode;

namespace tests;

[TestClass]
public class Day5Tests
{
    public Day5Tests()
    {
        Input = Utilities.GetResourceData("day5.txt", "tests");
    }

    private string Input { get; }

    [TestMethod]
    public void GetSeedValues()
    {
        var day5 = new Day5(Input);
        var expectedValues = new List<long>() { 79, 14, 55, 13 };
        var seedValues = day5.GetSeedValues();

        Assert.AreEqual(expectedValues.Count, seedValues.Intersect(expectedValues).Count());
    }

    [TestMethod]
    public void GetSeedValuesWithRanges()
    {
        var day5 = new Day5(Input);

        List<long> seedValues = day5.GetSeedValuesWithRanges();

        Assert.AreEqual(27, seedValues.Count);

        var expectedValues = new List<long>() { 79, 92, 55, 67 };

        Assert.AreEqual(expectedValues.Count, expectedValues.Intersect(seedValues).Count());     
    }

    [TestMethod]
    public void GetMapsList()
    {
        var day5 = new Day5(Input);
        List<List<MapInfo>> maps = day5.GetMaps();

        Assert.AreEqual(7, maps.Count);
        
        Assert.AreEqual(50, maps[0].First().DestinationStart);
        Assert.AreEqual(98, maps[0].First().SourceStart);
        Assert.AreEqual(2, maps[0].First().Range);
    }

    [TestMethod]
    public void GetMappedValue()
    {
        var day5 = new Day5(Input);
        List<List<MapInfo>> maps = day5.GetMaps();

        var input0 = new { source = 98, expected = 50 };
        var input1 = new { source = 99, expected = 51 };
        var input2 = new { source = 98, expected = 50 };
        var input3 = new { source = 61, expected = 63 };
        var input4 = new { source = 4, expected = 4 };

        var inputs = new[] { input0, input1, input2, input3, input4 };

        foreach (var input in inputs)
        {
            Assert.AreEqual(input.expected, day5.GetMappedValue(maps[0], input.source));
        }
    }

    [TestMethod]
    public void GetLocation()
    {
        var day5 = new Day5(Input);

        Assert.AreEqual(82, day5.GetLocation(79));
    }

    [TestMethod]
    public void GetLowestLocation()
    {
        var day5 = new Day5(Input);

        Assert.AreEqual(35, day5.Part1());
    }

    [TestMethod]
    public void Part2()
    {
        var day5 = new Day5(Input);
        Assert.AreEqual(46, day5.Part2());
    }

    [TestMethod]
    public void SpliceTest()
    {
        var day5 = new Day5(Input);
        var seeds = new List<RangeInfo>() { new RangeInfo(10, 10) };
        var mapInfo = new MapInfo(5, 14, 3);

        var spliced = day5.Splice(mapInfo, seeds);

        Assert.AreEqual(2, spliced.Unmapped.Count);

        Assert.AreEqual(1, spliced.Mapped.Count);

        var mappedValue = spliced.Mapped.First();

        Assert.AreEqual(5, mappedValue.StartIndex);
        Assert.AreEqual(7, mappedValue.EndIndex);
        Assert.AreEqual(3, mappedValue.Range);
    }
}