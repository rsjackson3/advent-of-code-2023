namespace AdventOfCode
{
    public class Day2
    {
        public Day2(string input, int redCount, int greenCount, int blueCount)
        {
            Input = input;
            CubeCounts = new Dictionary<string, int>()
            {
                {"red", redCount },
                {"green", greenCount },
                {"blue", blueCount }
            };
        }

        private string Input { get; }

        private Dictionary<string, int> CubeCounts { get; set; }

        private bool IsPossible(string line)
        {
            var sets = line[(line.IndexOf(':') + 1)..].Split("; ").Select(s => s.Split(',')).SelectMany(s => s);
            sets = sets.Select(s => s.Trim());

            foreach (var set in sets)
            {
                var colorCounts = set.Split(' ');
                string color = colorCounts[1];
                int count = int.Parse(colorCounts[0]);

                if (count > CubeCounts[color])
                {
                    return false;
                }
            }

            return true;
        }

        public int SumPossibleGames()
        {
            var lines = Input.Split("\r\n");

            return lines.Where(l => IsPossible(l)).Select(l => int.Parse(l[(l.IndexOf(' ') + 1)..l.IndexOf(':')])).Sum();
        }

        private int GetPowerOfSet(string line)
        {
            var sets = line[(line.IndexOf(':') + 1)..].Split("; ").Select(s => s.Split(',')).SelectMany(s => s);
            sets = sets.Select(s => s.Trim());

            var map = new Dictionary<string, int>()
            {
                {"red", 1 },
                {"green", 1 },
                {"blue", 1 }
            };

            foreach (var set in sets)
            {
                var colorCounts = set.Split(' ');
                string color = colorCounts[1];
                int count = int.Parse(colorCounts[0]);

                if (map[color] < count)
                    map[color] = count;
            }

            return map.Values.Aggregate((a, b) => a * b);
        }

        public int FewestNumberOfCubes()
        {
            var lines = Input.Split("\r\n");

            return lines.Select(l => GetPowerOfSet(l)).Sum();
        }
    }
}