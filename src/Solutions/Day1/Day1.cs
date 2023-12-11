using System.Reflection;

namespace AdventOfCode
{
    public class Day1
    {
        public Day1(string input)
        {
            Input = input;
            // when replacing, the first or last letter can chain together with other valid digits. e.g. twone or threeight
            DigitsMap = new Dictionary<string, string>()
            {
                {"one", "o1e" },
                {"two", "t2o" },
                {"three", "t3e" },
                {"four", "f4r" },
                {"five", "f5e" },
                {"six", "s6x" },
                {"seven", "s7n" },
                {"eight", "e8t" },
                {"nine", "n9e" }
            };
        }

        private string Input { get; set; }

        private Dictionary<string, string> DigitsMap { get; }

        public int SumCalibrationValues()
        {
            var lines = Input.Split("\r\n");

            int sum = 0;

            foreach (var line in lines)
            {
                var nums = line.Where(c => char.IsDigit(c)).Select(c => (int)Char.GetNumericValue(c)).ToArray();
                var calibrationVal = int.Parse($"{nums[0]}{nums[nums.Length - 1]}");

                sum += calibrationVal;
            }

            return sum;
        }

        public int SumCalibrationValues2()
        {
            var lines = Input.Split("\r\n");

            int sum = 0;

            foreach (var line in lines)
            {
                var nums = line.Where(c => char.IsDigit(c)).Select(c => (int)Char.GetNumericValue(c)).ToArray();
                var targetVals = new int[2] { nums[nums.Length - 1], nums[0] };

                var calibrationVal = targetVals.Select((num, i) => num * (int)Math.Pow(10, i)).Sum();

                sum += calibrationVal;
            }

            return sum;
        }

        private string ReplaceDigits(string input)
        {
            foreach (var digit in DigitsMap.Keys)
            {
                input = input.Replace(digit, DigitsMap[digit]);
            }

            return input;
        }

        public int SumPart2()
        {
            var sanitizedInput = ReplaceDigits(Input);
            var lines = sanitizedInput.Split("\r\n");

            int sum = 0;

            foreach (var line in lines)
            {
                var nums = line.Where(c => char.IsDigit(c)).Select(c => (int)Char.GetNumericValue(c)).ToArray();
                var calibrationVal = int.Parse($"{nums[0]}{nums[nums.Length - 1]}");

                sum += calibrationVal;
            }

            return sum;
        }      
    }
}