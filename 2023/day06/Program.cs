using System.Diagnostics;

namespace day06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("../input/day06.txt");
            var stopwatch = Stopwatch.StartNew();

            int partOne = 1;

            int[] times = lines[0].Remove(0, 10).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
            int[] lengths = lines[1].Remove(0, 10).Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();

            long partTwoTime = long.Parse(String.Join("", times));
            long partTwoLength = long.Parse(String.Join("", lengths));

            long partTwo = 0;

            for (int i = 0; i < times.Length; i++)
            {
                int combinations = 0;
                for (int j = 0; j < times[i]; j++)
                    if (j * times[i] - j * j > lengths[i])
                        combinations++;
                partOne *= combinations;
            }

            for (long i = 0; i < partTwoTime; i++)
                if ((i * partTwoTime - i * i) > partTwoLength)
                    partTwo++;

            stopwatch.Stop();

            Console.WriteLine($"execution time\t: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine("part one\t: " + partOne); // 303600
            Console.WriteLine("part two\t: " + partTwo); // 23654842
        }
    }
}

