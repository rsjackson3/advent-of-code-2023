namespace AdventOfCode
{
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
    }
}