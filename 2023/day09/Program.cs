using System.Diagnostics;

namespace day09
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("../input/day09.txt");
            var stopwatch = Stopwatch.StartNew();

            long partOne = 0;
            long partTwo = 0;

            foreach (string history in lines)
            {
                int[] line = history.Split(' ').Select(n => int.Parse(n)).ToArray();
                List<int[]> sequences = new List<int[]>();
                sequences.Add(line);

                int[] currentPattern = line;
                bool isCompleted = false;

                while (!isCompleted)
                {
                    isCompleted = true;
                    int[] newPattern = new int[currentPattern.Length - 1];
                    for (int i = 1; i < currentPattern.Length; i++)
                    {
                        newPattern[i - 1] = currentPattern[i] - currentPattern[i - 1];
                        if (newPattern[i - 1] != 0)
                            isCompleted = false;
                    }
                    sequences.Add(newPattern);
                    currentPattern = newPattern;
                }

                long placeholderOne = 0;
                long placeholderTwo = 0;
                for (int i = sequences.Count - 1; i > 0; i--)
                {
                    placeholderOne += sequences[i - 1][sequences[i - 1].Length - 1];
                    placeholderTwo = sequences[i - 1][0] - placeholderTwo;
                }
                partOne += placeholderOne;
                partTwo += placeholderTwo;

            }

            stopwatch.Stop();

            Console.WriteLine($"execution time\t: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine("part one\t: " + partOne); // 2174807968
            Console.WriteLine("part two\t: " + partTwo); // 1208
        }
    }
}

