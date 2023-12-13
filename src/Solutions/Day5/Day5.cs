using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day5
    {
        public Day5(string input)
        {
            Input = input;
            Lines = Input.Split("\r\n").ToList();            
        }

        private string Input { get; }
        private List<string> Lines { get; }

        public long GetLowestLocation()
        {
            var seeds = GetSeedValues();
            var maps = GetMaps();
            long lowestLocation = long.MaxValue;

            foreach (var seed in seeds)
            {
                lowestLocation = Math.Min(lowestLocation, GetLocation(seed, maps));
            }

            return lowestLocation;
        }

        public long GetLocation(long seedValue, List<List<MapInfo>> maps)
        {
            long current = seedValue;

            foreach (var mapGroup in maps)
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
        }

        public long DestinationStart { get; }

        public long SourceStart { get; }

        public long Range { get; }
    }
}