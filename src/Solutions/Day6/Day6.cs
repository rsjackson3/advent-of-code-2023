using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day6
    {
        public Day6(string input)
        {
            Input = input;
            Races = new List<RaceInfo>();

            var lines = Input.Split("\r\n").ToList();

            var regex = new Regex(@"(\d+)");
            var times = regex.Matches(lines[0]);
            var distances = regex.Matches(lines[1]);

            for (int i = 0; i < times.Count; i++)
            {
                Races.Add(new RaceInfo(int.Parse(times[i].Value), int.Parse(distances[i].Value)));
            }
        }

        private string Input { get; }

        public List<RaceInfo> Races { get; }

        public long Part1()
        {
            return Races.Select(r => NumberOfWaysToWin(r)).Aggregate((long)1, (acc, val) => acc * val);
        }

        public long Part2()
        {
            var time = long.Parse(string.Join("", Races.Select(r => r.Time.ToString())));
            var distance = long.Parse(string.Join("", Races.Select(r => r.Distance.ToString())));

            var raceInfo = new RaceInfo(time, distance);

            return NumberOfWaysToWin(raceInfo);
        }

        public long NumberOfWaysToWin(RaceInfo race)
        {
            long distanceToBeat = race.Distance;
            long winCount = 0;

            for (long i = 0; i < race.Time; i++)
            {
                if (i * (race.Time - i) > distanceToBeat)
                    winCount++;
            }

            return winCount;
        }
    }

    public class RaceInfo
    {
        public RaceInfo(long time, long distance)
        {
            Time = time;
            Distance = distance;
        }
        public long Time { get; }

        public long Distance { get; }
    }
}