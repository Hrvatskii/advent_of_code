using System.Diagnostics;

namespace day12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("../input/day12.txt");
            var stopwatch = Stopwatch.StartNew();
            
            foreach (string line in lines)
            {
                Console.WriteLine(line);
                string map = line.Split(' ')[0];
                string mapCopy = map;
                int[] sequence = line.Split(' ')[1].Split(',').Select(n => int.Parse(n)).ToArray();

                List<int[]> clusters = new List<int[]>();
                while (mapCopy.Contains('?') || mapCopy.Contains('#'))
                {
                    int start = 0;
                    if (mapCopy.IndexOf('?') == -1)
                        start = mapCopy.IndexOf('#');
                    else if (mapCopy.IndexOf('#') == -1)
                        start = mapCopy.IndexOf('?');
                    else
                        start = Math.Min(mapCopy.IndexOf('?'), mapCopy.IndexOf('#'));
                    int length = 0;
                    int index = start;

                    while (mapCopy[index] != '.')
                    {
                        mapCopy = mapCopy.Remove(index, 1);
                        length++;
                        if (index >= mapCopy.Length)
                            break;
                    }

                    clusters.Add(new int[] { start, length });
                }


                for (int i = 0; i < sequence.Length; i++)
                {
                    int minDistance = 0;

                    for (int j = 0; j < i; j++)
                        minDistance += sequence[j] + 1;
                }
            }

            stopwatch.Stop();

            Console.WriteLine($"execution time\t: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine("part one\t: ");
            Console.WriteLine("part two\t: ");
        }
    }
}
