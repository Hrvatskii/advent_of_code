using System.Diagnostics;

namespace day11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("../input/day11.txt");
            var stopwatch = Stopwatch.StartNew();

            List<int> emptyRows = new List<int>();
            List<int> emptyCols = new List<int>();
            List<int[]> galaxyPositions = new List<int[]>();

            for (int i = 0; i < lines.Length; i++)
                if (!lines[i].Contains('#'))
                    emptyRows.Add(i);

            for (int i = 0; i < lines[0].Length; i++)
            {
                bool empty = true;
                for (int j = 0; j < lines.Length; j++)
                {
                    if (lines[j][i] == '#')
                    {
                        galaxyPositions.Add(new int[] { i, j });
                        empty = false;
                    }
                }
                if (empty)
                    emptyCols.Add(i);
            }

            int partOne = 0;
            long partTwo = 0;

            for (int i = 0; i < galaxyPositions.Count; i++)
                for (int j = i + 1; j < galaxyPositions.Count; j++)
                {
                    int x1 = galaxyPositions[i][0];
                    int y1 = galaxyPositions[i][1];
                    int x2 = galaxyPositions[j][0];
                    int y2 = galaxyPositions[j][1];
                    int deltaX = Math.Abs(x1 - x2);
                    int deltaY = Math.Abs(y1 - y2); 

                    partOne += deltaX + deltaY;
                    partTwo += deltaX + deltaY;

                    foreach (int e in emptyRows)
                        if ((y1 < e && e < y2) || (y2 < e && e < y1))
                        {
                            partOne++;
                            partTwo += 999999;
                        }

                    foreach (int e in emptyCols)
                        if ((x1 < e && e < x2) || (x2 < e && e < x1))
                        {
                            partOne++;
                            partTwo += 999999;
                        }
                }

            stopwatch.Stop();

            Console.WriteLine($"execution time\t: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine("part one\t: " + partOne); // 9522407
            Console.WriteLine("part two\t: " + partTwo); // 544723432977
        }
    }
}

