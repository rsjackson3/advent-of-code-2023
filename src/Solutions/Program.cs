// See https://aka.ms/new-console-template for more information

using AdventOfCode;

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
Console.WriteLine($"Day 3 Pt. 1: {day3Part1}");

var day3Part2 = day3.Part2();
Console.WriteLine($"Day 3 Pt. 2: {day3Part2}");

Console.WriteLine("\r\nPress any button to exit...");

Console.ReadKey();
