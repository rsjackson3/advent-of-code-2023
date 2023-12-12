namespace AdventOfCode
{
    public class Day4
    {
        public Day4(string input)
        {
            Input = input;
            Lines = Input.Split("\r\n").ToList();
        }

        private string Input { get; }

        private List<string> Lines { get; }

        public int Part1()
        {
            int sum = 0;

            foreach (var line in Lines)
            {
                var matches = GetMatches(line);

                if (matches.Any())
                {
                    sum += (int)Math.Pow(2, matches.Count() - 1);
                }
            }

            return sum;
        }

        public int Part2()
        {
            int sum = 0;
            for (int i = 0; i < Lines.Count; i++)
                SumMatches(ref sum, i);

            return sum;
        }

        public int Part2Optimized()
        {
            var map = new Dictionary<int, ScratchcardInfo>();
            for (int i = 0; i < Lines.Count; i++)
            {
                map.Add(i, new ScratchcardInfo(1, GetMatches(Lines[i]).Count()));
            }

            for (int i = 0; i < map.Keys.Count; i++)
            {
                if (map[i].MatchCount > 0)
                {
                    for (int j = 1; j <= map[i].MatchCount; j++)
                    {
                        map[i + j].Count += map[i].Count;
                    }
                }
            }

            return map.Values.Select(s => s.Count).Sum();
        }

        private void SumMatches(ref int sum, int index)
        {
            sum += 1;

            var matchCount = GetMatches(Lines[index]).Count();

            for (int j = 1; j <= matchCount; j++)
            {
                SumMatches(ref sum, index + j);
            }
        }

        private IEnumerable<int> GetMatches(string line)
        {
            var parts = PartitionNumbers(line);
            var matches = parts[1].Intersect(parts[0]);

            return matches;
        }

        private List<List<int>> PartitionNumbers(string line)
        {
            List<int> winningNumbers = line[(line.IndexOf(':') + 1)..line.IndexOf('|')].Trim().Split(' ').Where(n => !string.IsNullOrEmpty(n)).Select(x => int.Parse(x)).ToList();
            List<int> heldNumbers = line[(line.IndexOf('|') + 1)..].Trim().Split(' ').Where(n => !string.IsNullOrEmpty(n)).Select(x => int.Parse(x)).ToList();

            return new List<List<int>>() { winningNumbers, heldNumbers };
        }
    }

    public class ScratchcardInfo
    {
        public ScratchcardInfo(int count, int matchCount)
        {
            Count = count;
            MatchCount = matchCount;
        }

        public int Count { get; set; }

        public int MatchCount { get; }
    }
}