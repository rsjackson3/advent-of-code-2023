using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day5
    {
        public Day5(string input)
        {
            Input = input;
            Lines = Input.Split("\r\n").ToList();
            Maps = GetMaps();
            Cache = new Dictionary<long, long>();
            Ranges = new List<RangeInfo>();
        }

        private string Input { get; }
        private List<string> Lines { get; }

        private Dictionary<long, long> Cache { get; set; }

        private List<List<MapInfo>> Maps { get; }

        private List<RangeInfo> Ranges { get; }

        public long Part1()
        {
            var seeds = GetSeedValues();
            long lowestLocation = long.MaxValue;

            foreach (var seed in seeds)
            {
                lowestLocation = Math.Min(lowestLocation, GetLocation(seed));
            }

            return lowestLocation;
        }

        // takes too long because it has to loop through each seed value
        public long Part2SlowVersion()
        {
            List<long> values = GetSeedValues();
            long lowestLocation = long.MaxValue;

            for (int i = 0; i < values.Count; i += 2)
            {
                lowestLocation = Math.Min(lowestLocation, GetMin(values[i], values[i + 1]));
            }

            return lowestLocation;
        }

        public long Part2()
        {
            var seedValues = GetSeedValues();
            var seedRanges = new List<RangeInfo>();

            for (int i = 0; i < seedValues.Count; i += 2)
            {
                Ranges.Add(new RangeInfo(seedValues[i], seedValues[i + 1]));
            }

            return FindRangesWithMaps(Maps, Ranges).Select(r => r.StartIndex).Min();
        }

        public List<RangeInfo> FindRangesWithMaps(List<List<MapInfo>> maps, List<RangeInfo> seeds)
        {
            var outputRanges = new List<RangeInfo>(seeds);

            foreach (List<MapInfo> map in maps)
            {
                outputRanges = UpdateSeedRanges(map, outputRanges);
            }

            return outputRanges;
        }

        public List<RangeInfo> UpdateSeedRanges(List<MapInfo> map, List<RangeInfo> seed)
        {
            List<RangeInfo> allMapped = new List<RangeInfo>();

            foreach (var inputSeed in seed)
            {
                var ordered = map.OrderBy(m => m.SourceStart).ToList();

                var mapped = new List<RangeInfo>();
                var unmpapped = new List<RangeInfo>() { inputSeed };

                for (int i = 0; i < ordered.Count(); i++)
                {
                    var spliced = Splice(ordered[i], unmpapped);
                    mapped.AddRange(spliced.Mapped);
                    unmpapped = spliced.Unmapped;
                }

                if (unmpapped.Any())
                    mapped.AddRange(unmpapped);

                allMapped.AddRange(mapped);
            }

            return allMapped;
        }

        public SpliceInfo Splice(MapInfo mapInfo, List<RangeInfo> unmappedSeeds)
        {
            var notMapped = new List<RangeInfo>();
            var spliceInfo = new SpliceInfo();
            for (int i = 0; i < unmappedSeeds.Count; i++)
            {
                var seed = unmappedSeeds[i];
                if (seed.EndIndex < mapInfo.SourceStart)
                    notMapped.Add(seed);
                else if (seed.StartIndex > mapInfo.SourceEnd)
                    notMapped.Add(seed);
                else
                {
                    (long, long) intersection = GetIntersection(mapInfo, seed);
                    var mappedSeed = new RangeInfo(mapInfo.DestinationStart + (intersection.Item1 - mapInfo.SourceStart), (intersection.Item2 - intersection.Item1) + 1);
                    spliceInfo.Mapped.Add(mappedSeed);

                    if (seed.StartIndex < mapInfo.SourceStart)
                    {
                        (long, long) beforeStart = BeforeStart(mapInfo, seed);
                        notMapped.Add(new RangeInfo(beforeStart.Item1, (beforeStart.Item2 - beforeStart.Item1) + 1));
                    }

                    if (seed.EndIndex > mapInfo.SourceEnd)
                    {
                        (long, long) afterEnd = AfterEnd(mapInfo, seed);
                        notMapped.Add(new RangeInfo(afterEnd.Item1, (afterEnd.Item2 - afterEnd.Item1) + 1));
                    }                      
                }
            }

            spliceInfo.Unmapped = notMapped;

            return spliceInfo;
        }

        private (long, long) GetIntersection(MapInfo map, RangeInfo seed)
        {
            long mapStart = map.SourceStart;
            long mapEnd = map.SourceEnd;
            long seedStart = seed.StartIndex;
            long seedEnd = seed.EndIndex;

            if (seedStart >= mapStart)
                return (seedStart, Math.Min(seedEnd, mapEnd));
            else
                return (mapStart, Math.Min(seedEnd, mapEnd));
        }

        private (long, long) BeforeStart(MapInfo map, RangeInfo seed)
        {
            return (seed.StartIndex, map.SourceStart - 1);
        }

        private (long, long) AfterEnd(MapInfo map, RangeInfo seed)
        {
            return (map.SourceEnd + 1, seed.EndIndex);
        }

        public long GetMin(long start, long range)
        {
            var seedValues = GetRange(start, range);
            var locations = new List<long>();

            int counter = 0;
            Parallel.ForEach(seedValues, seedValue =>
            {
                if (Cache.ContainsKey(seedValue))
                {
                    lock (locations)
                    {
                        locations.Add(Cache[seedValue]);
                    }
                }
                else
                {
                    var location = GetLocation(seedValue);

                    lock (locations)
                    {
                        locations.Add(location);
                    }

                    lock (Cache)
                    {
                        Cache.Add(seedValue, location);
                    }
                }

                counter++;
            });

            return locations.Min();
        }

        public long GetLocation(long seedValue)
        {
            long current = seedValue;

            foreach (var mapGroup in Maps)
            {
                current = GetMappedValue(mapGroup, current);
            }

            return current;
        }

        public long GetMappedValue(List<MapInfo> mapInfos, long source)
        {
            var match = mapInfos.Where(m => source >= m.SourceStart && source <= (m.SourceStart + m.Range) - 1).FirstOrDefault();

            if (match == null)
                return source;

            return match.DestinationStart + (source - match.SourceStart);
        }

        public List<List<MapInfo>> GetMaps()
        {
            var maps = new List<List<MapInfo>>();
            var group = new List<MapInfo>();

            foreach (var line in Lines.Skip(1))
            {
                if (Regex.IsMatch(line, @"^\d[^\n]*"))
                {
                    var vals = GetValues(line);
                    group.Add(new MapInfo(vals[0], vals[1], vals[2]));
                }
                else if (string.IsNullOrEmpty(line) && group.Any())
                {
                    maps.Add(new List<MapInfo>(group));
                    group.Clear();
                }
            }

            if (group.Any())
                maps.Add(group);

            return maps;
        }

        public List<long> GetSeedValues()
        {
            var line = Regex.Replace(Lines[0], "[^0-9 ]", "");

            return GetValues(line);
        }

        public List<long> GetSeedValuesWithRanges()
        {
            List<long> values = GetSeedValues();
            var valuesWithRanges = new List<long>();

            for (int i = 0; i < values.Count; i += 2)
            {
                valuesWithRanges.AddRange(GetRange(values[i], values[i + 1]));
            }

            return valuesWithRanges;
        }

        private IEnumerable<long> GetRange(long start, long range)
        {
            var vals = new List<long>();
            for (long i = 0; i < range; i++)
            {
                yield return start + i;
            }
        }

        private List<long> GetValues(string line)
        {
            return line.Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(s => long.Parse(s)).ToList();
        }
    }

    public class MapInfo
    {
        public MapInfo(long destinationStart, long sourceStart, long range)
        {
            DestinationStart = destinationStart;
            SourceStart = sourceStart;
            Range = range;
            SourceEnd = sourceStart + Range - 1;
        }

        public long DestinationStart { get; }

        public long SourceStart { get; }

        public long Range { get; }

        public long SourceEnd { get; }
    }

    public class RangeInfo
    {
        public RangeInfo(long startIndex, long range)
        {
            StartIndex = startIndex;
            EndIndex = startIndex + range - 1;
            Range = range;
        }

        public long StartIndex { get; }

        public long EndIndex { get; }

        public long Range { get; }
    }

    public class SpliceInfo
    {
        public SpliceInfo()
        {
            Mapped = new List<RangeInfo>();
            Unmapped = new List<RangeInfo>();
        }

        public SpliceInfo(List<RangeInfo> unmapped)
        {
            Mapped = new List<RangeInfo>();
            Unmapped = unmapped;
        }
        public SpliceInfo(List<RangeInfo> mapped, List<RangeInfo> unmapped)
        {
            Mapped = mapped;
            Unmapped = unmapped;
        }



        public List<RangeInfo> Mapped { get; set; }
        public List<RangeInfo> Unmapped { get; set; }
    }
}