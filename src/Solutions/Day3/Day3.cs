using System.Text.RegularExpressions;

namespace AdventOfCode
{
    // implemented two solutions, one with Regex and one without
    public class Day3
    {
        public Day3(string input)
        {
            Input = input;

            var lines = Input.Split("\r\n");
            int rowCount = lines.Length;
            int colCount = lines[0].Length;

            RowCount = rowCount;
            ColCount = colCount;

            char[,] schematic = new char[rowCount, colCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    schematic[i, j] = lines[i][j];
                }
            }

            Schematic = schematic;
        }

        private string Input { get; }

        private char[,] Schematic { get; }

        private int RowCount { get; }

        private int ColCount { get; }

        private List<(int, int)> GetAdjacentIndexes(int i, int j)
        {
            return new List<(int, int)>()
            {
                (i + 1, j),
                (i - 1, j),
                (i, j + 1),
                (i, j - 1),
                (i + 1, j + 1),
                (i + 1, j - 1),
                (i - 1, j + 1),
                (i - 1, j - 1)
            };
        }

        private int[,] GetAdjacentSquares()
        {
            int[,] adjacentSquares = new int[RowCount, ColCount];

            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColCount; j++)
                {
                    char c = Schematic[i, j];
                    if (!char.IsDigit(c) && c != '.')
                        PopulateAdjacentSquares(adjacentSquares, i, j);
                }
            }

            return adjacentSquares;
        }

        private void PopulateAdjacentSquares(int[,] arr, int i, int j)
        {
            int rowCount = arr.GetLength(0);
            int colCount = arr.GetLength(1);
            var indexes = GetAdjacentIndexes(i, j);

            foreach (var indexPair in indexes)
            {
                if (IsInRange(indexPair.Item1, indexPair.Item2, rowCount, colCount))
                    arr[indexPair.Item1, indexPair.Item2] = 1;
            }
        }

        private bool IsInRange(int i, int j, int rowCount, int colCount)
        {
            return i < rowCount && i >= 0 && j < colCount && j >= 0;
        }

        public int SumPartNumbers()
        {
            int[,] adjacentSquares = GetAdjacentSquares();

            var partNumbers = new List<int>();
            string partNumber = "";
            bool isAdjacent = false;

            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColCount; j++)
                {
                    if (char.IsDigit(Schematic[i, j]))
                    {
                        partNumber += Schematic[i, j];
                        if (adjacentSquares[i, j] == 1)
                            isAdjacent = true;
                    }
                    else
                    {
                        if (isAdjacent)
                        {
                            partNumbers.Add(int.Parse(partNumber));
                            isAdjacent = false;
                        }

                        partNumber = "";
                    }
                }

                // don't continue appending to number if going to next row...
                if (partNumber.Length > 0)
                {
                    if (isAdjacent)
                    {
                        partNumbers.Add(int.Parse(partNumber));
                        isAdjacent = false;
                    }

                    partNumber = "";
                }            
            }

            return partNumbers.Sum();
        }

        private List<int> GetGearNumbers()
        {
            var gearNumbers = new List<int>();

            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColCount; j++)
                {
                    if (Schematic[i, j] == '*')
                    {
                        List<int> adjacentPartNumbers = GetAdjacentPartNumbers(i, j);
                        if (adjacentPartNumbers.Count == 2)
                            gearNumbers.Add(adjacentPartNumbers.Aggregate((a, b) => a * b));
                    }
                }
            }

            return gearNumbers;
        }

        private List<int> GetAdjacentPartNumbers(int i, int j)
        {
            int[,] visitedNumbers = new int[RowCount, ColCount];
            var indexes = GetAdjacentIndexes(i, j);

            var adjacentPartNumbers = new List<int>();

            foreach (var indexPair in indexes)
            {
                var row = indexPair.Item1;
                var col = indexPair.Item2;

                if (!IsInRange(row, col, RowCount, ColCount))
                    continue;

                if (visitedNumbers[row, col] != 1 && char.IsDigit(Schematic[row, col]))
                {
                    adjacentPartNumbers.Add(GetAdjacentPartNumber(row, col, visitedNumbers));
                }
            }

            return adjacentPartNumbers;
        }

        private int GetAdjacentPartNumber(int row, int col, int[,] visitedNumbers)
        {
            string left = "";
            string right = "";

            for (int j = col; j < ColCount; j++)
            {
                if (!char.IsDigit(Schematic[row, j]))
                    break;

                right += Schematic[row, j];
                visitedNumbers[row, j] = 1;
            }

            if (col - 1 < 0)
                return int.Parse(right);

            for (int j = col - 1; j >= 0; j--)
            {
                if (!char.IsDigit(Schematic[row, j]))
                    break;

                left = Schematic[row, j] + left;
                visitedNumbers[row, j] = 1;
            }

            return int.Parse($"{left}{right}");
        }

        public int Part2()
        {
            return GetGearNumbers().Sum();
        }

        public int Part1Regex()
        {
            string numPattern = @"\d+";
            var numRegex = new Regex(numPattern);

            string symbolPattern = @"[^0-9.]";
            var symbolRegex = new Regex(symbolPattern);

            var lines = Input.Split("\r\n");

            var symbolIndexes = new List<(int, int)>();
            var numInfo = new List<NumberInfo>();

            for (int i = 0; i < lines.Length; i++)
            {
                symbolIndexes.AddRange(symbolRegex.Matches(lines[i]).Select(m => (i, m.Index)));
                numInfo.AddRange(numRegex.Matches(lines[i]).Select(m => new NumberInfo(i, m.Index, m.Length, int.Parse(m.Value))));
            }

            return numInfo.Where(n => symbolIndexes.Any(s => IsAdjacent(n, s))).Sum(n => n.Value);
        }

        public int Part2Regex()
        {
            string numPattern = @"\d+";
            var numRegex = new Regex(numPattern);

            string starPattern = @"\*";
            var starRegex = new Regex(starPattern);

            var lines = Input.Split("\r\n");

            var starIndexes = new List<(int, int)>();
            var numInfo = new List<NumberInfo>();

            for (int i = 0; i < lines.Length; i++)
            {
                starIndexes.AddRange(starRegex.Matches(lines[i]).Select(m => (i, m.Index)));
                numInfo.AddRange(numRegex.Matches(lines[i]).Select(m => new NumberInfo(i, m.Index, m.Length, int.Parse(m.Value))));
            }

            var grouping = starIndexes.GroupBy(s => numInfo.Where(n => IsAdjacent(n, s)).Count());

            var matches = starIndexes.Select(s => GetAdjacentMatches(numInfo, s)).Where(n => n.Count == 2);

            return matches.Sum(inner => inner.Aggregate(1, (acc, numInfo) => acc * numInfo.Value));
        }

        private bool IsAdjacent(NumberInfo numInfo, (int, int) symbolInfo)
        {
            int startIndex = numInfo.Col - 1;
            int endIndex = numInfo.Col + numInfo.Length;

            return Math.Abs(numInfo.Row - symbolInfo.Item1) <= 1 && symbolInfo.Item2 >= startIndex && symbolInfo.Item2 <= endIndex;
        }

        private List<NumberInfo> GetAdjacentMatches(List<NumberInfo> numInfo, (int, int) symbolInfo)
        {
            return numInfo.Where(n => IsAdjacent(n, symbolInfo)).ToList();
        }
    }

    public class NumberInfo
    {
        public NumberInfo(int row, int col, int length, int value)
        {
            Row = row;
            Col = col;
            Length = length;
            Value = value;
        }

        public int Row { get; }

        public int Col { get; }

        public int Length { get; }

        public int Value { get; }
    }
}