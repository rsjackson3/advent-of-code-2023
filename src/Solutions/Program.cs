// See https://aka.ms/new-console-template for more information

using AdventOfCode;
using System.Diagnostics;

var stopwatch = new Stopwatch();

var input = Utilities.GetResourceData("day1.txt");
var day1 = new Day1(input);
var sum1 = day1.SumCalibrationValues();
Console.WriteLine($"Day 1 Pt. 1: {sum1}");

//var sum2 = day1.SumCalibrationValues2();

var part2Sum = day1.SumPart2();
Console.WriteLine($"Day 1 Pt. 2: {part2Sum}");

var day2Input = Utilities.GetResourceData("day2.txt");
var day2 = new Day2(day2Input, 12, 13, 14);

var day2Sum = day2.SumPossibleGames();
Console.WriteLine($"Day 2 Pt. 1: {day2Sum}");

var day2Part2 = day2.FewestNumberOfCubes();
Console.WriteLine($"Day 2 Pt. 2: {day2Part2}");

var day3Input = Utilities.GetResourceData("day3.txt");
var day3 = new Day3(day3Input);
var day3Part1 = day3.SumPartNumbers();
var day3Part1Regex = day3.Part1Regex();

Console.WriteLine($"Day 3 Pt. 1: {day3Part1}");
Console.WriteLine($"Day 3 Pt. 1 Regex: {day3Part1Regex}");

var day3Part2 = day3.Part2();
var day3Part2Regex = day3.Part2Regex();

Console.WriteLine($"Day 3 Pt. 2: {day3Part2}");
Console.WriteLine($"Day 3 Pt. 2 Regex: {day3Part2Regex}");

var day4Input = Utilities.GetResourceData("day4.txt");
var day4 = new Day4(day4Input);


var day4Part1 = day4.Part1();
Console.WriteLine($"Day 4 Pt. 1: {day4Part1}");

stopwatch.Restart();
var day4Part2Optimized = day4.Part2Optimized();
stopwatch.Stop();
Console.WriteLine($"Day 4 Pt. 2 Optimized: {day4Part2Optimized}. Time: {stopwatch.Elapsed.TotalMilliseconds}");

var day5Input = Utilities.GetResourceData("day5.txt");
var day5 = new Day5(day5Input);
stopwatch.Restart();
var day5Part1 = day5.Part1();
stopwatch.Stop();

Console.WriteLine($"Day 5 Pt. 1: {day5Part1}. Time: {stopwatch.Elapsed.TotalMilliseconds}");

stopwatch.Restart();
var day5Part2 = day5.Part2();
stopwatch.Stop();
Console.WriteLine($"Day 5 Pt. 2: {day5Part2}. Time: {stopwatch.Elapsed.TotalMilliseconds}");

Console.WriteLine("\r\nPress any button to exit...");

Console.ReadKey();
