using System.Diagnostics;

namespace day14
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("../input/day14.txt");
            var stopwatch = Stopwatch.StartNew();

            int partOne = tilt(lines, false).Item1;

            List<string[]> already = new List<string[]>();
            string[] newMap = lines;

            int index = 0;
            while (!already.Any(arr => arr.SequenceEqual(newMap)))
            {
                already.Add(DeepCopy(newMap));
                for (int i = 0; i < 4; i++)
                    newMap = Rotate(tilt(newMap, false).Item2);
                index++;
            }

            int start = 0;
            while (!already[start].SequenceEqual(newMap))
                start++;

            int partTwo = tilt(already[(((int) Math.Pow(10, 9) - start) % (index - start)) + start], true).Item1;

            static Tuple<int, string[]> tilt(string[] map, bool partTwo)
            {
                int totalLoad = 0;

                for (int i = 0; i < map[0].Length; i++)
                {
                    int squareSubtract = 0;
                    int amountOfSpheres = 0;

                    for (int j = 0; j < map.Length; j++)
                    {
                        char square = map[j][i];

                        if (square == 'O')
                        {
                            amountOfSpheres++;
                            char[] row = map[j].ToCharArray();
                            row[i] = '.';
                            map[j] = new string(row);
                            if (partTwo)
                                totalLoad += map.Length - j;
                        }
                        else if (square == '#' && !partTwo)
                        {
                            for (int k = 0; k < amountOfSpheres; k++)
                            {
                                map = UpdateMap(map, squareSubtract, k, i);
                                totalLoad += map.Length - k - squareSubtract;
                            }
                            amountOfSpheres = 0;
                            squareSubtract = j + 1;
                        }
                    }
                    if (partTwo)
                        continue;

                    for (int k = 0; k < amountOfSpheres; k++)
                    {
                        map = UpdateMap(map, squareSubtract, k, i);
                        totalLoad += map.Length - k - squareSubtract;
                    }
                }
                return Tuple.Create(totalLoad, map);
            }

            static string[] UpdateMap (string[] newMap, int squarePosition, int offset, int row)
            {
                char[] a = newMap[squarePosition + offset].ToCharArray();
                a[row] = 'O';
                newMap[squarePosition + offset] = new string(a);

                return newMap;
            }

            static string[] Rotate (string[] matrix)
            {
                string[] transposed = matrix[0]
                    .Select((val, index) => new string(matrix.Select(row => row[index]).Reverse().ToArray()))
                    .ToArray();
                return transposed;
            }

            string[] DeepCopy(string[] original)
            {
                string[] copy = new string[original.Length];
                Array.Copy(original, copy, original.Length);
                return copy;
            }

            stopwatch.Stop();

            Console.WriteLine($"execution time\t: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine("part one\t: " + partOne); // 105461
            Console.WriteLine("part two\t: " + partTwo); // 102829
        }
    }
}
