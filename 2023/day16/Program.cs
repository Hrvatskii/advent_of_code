using System.Diagnostics;

namespace day16
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // warning: unoptimized code up ahead

            string[] lines = File.ReadAllLines("../input/day16.txt");
            var stopwatch = Stopwatch.StartNew();

            int partOne = FindAmount(1, -1, 0, lines);
            int partTwo = 0;

            for (int i = 0; i < lines[0].Length; i++)
            {
                int amount = FindAmount(2, i, -1, lines);
                if (amount > partTwo)
                    partTwo = amount;

                amount = FindAmount(4, i, lines.Length, lines);
                if (amount > partTwo)
                    partTwo = amount;

            }
            for (int i = 0; i < lines.Length; i++)
            {
                int amount = FindAmount(1, -1, i, lines);
                if (amount > partTwo)
                    partTwo = amount;

                amount = FindAmount(3, lines[0].Length, i, lines);
                if (amount > partTwo)
                    partTwo = amount;
            }

            static int FindAmount (int startDirection, int startX, int startY, string[] lines)
            {

                List<string> energized = new List<string>();
                Queue<string> visited = new Queue<string>();
                Queue<string> current = new Queue<string>();

                int amountEnergized = -1;

                current.Enqueue($"{startDirection} {startX} {startY}");

                // directions: 
                // 1: right 
                // 2: down 
                // 3: left 
                // 4: up 

                while (current.Count != 0)
                {
                    var beam = current.Dequeue().Split(' ').Select(n => int.Parse(n)).ToArray();
                    int direction = beam[0];
                    int x = beam[1];
                    int y = beam[2];

                    if (!energized.Contains($"{x} {y}"))
                    {
                        energized.Add($"{x} {y}");
                        amountEnergized++;
                    }
                    visited.Enqueue(String.Join(" ", beam));

                    if (direction == 1)
                        x++;
                    else if (direction == 2)
                        y++;
                    else if (direction == 3)
                        x--;
                    else
                        y--;

                    if (y < 0 || y == lines.Length || x < 0 || x == lines[0].Length)
                        continue;

                    char tile = lines[y][x];

                    if (tile == '.' || (tile == '|' && direction % 2 == 0) || (tile == '-' && direction % 2 != 0))
                    {
                        string newPos = Positionator(direction, x, y);

                        if (!visited.Contains(newPos) && !current.Contains(newPos))
                            current.Enqueue(newPos);
                    }
                    else if (tile == '\\')
                    {
                        if (direction == 1)
                            direction = 2;
                        else if (direction == 2)
                            direction = 1;
                        else if (direction == 3)
                            direction = 4;
                        else
                            direction = 3;

                        string newPos = Positionator(direction, x, y);

                        if (!visited.Contains(newPos) && !current.Contains(newPos))
                            current.Enqueue(newPos);
                    }
                    else if (tile == '/')
                    {
                        if (direction == 1)
                            direction = 4;
                        else if (direction == 2)
                            direction = 3;
                        else if (direction == 3)
                            direction = 2;
                        else
                            direction = 1;

                        string newPos = Positionator(direction, x, y);

                        if (!visited.Contains(newPos) && !current.Contains(newPos))
                            current.Enqueue(newPos);
                    }
                    else if (tile == '|' && direction % 2 != 0)
                    {
                        string new1 = Positionator(2, x, y);
                        string new2 = Positionator(4, x, y);

                        if (!visited.Contains(new1) && !current.Contains(new1))
                            current.Enqueue(new1);
                        if (!current.Contains(new2) && !current.Contains(new2))
                            current.Enqueue(new2);
                    }
                    else if (tile == '-' && direction % 2 == 0)
                    {
                        string new1 = Positionator(1, x, y);
                        string new2 = Positionator(3, x, y);

                        if (!visited.Contains(new1) && !current.Contains(new1))
                            current.Enqueue(new1);
                        if (!current.Contains(new2) && !current.Contains(new2))
                            current.Enqueue(new2);
                    }

                }


                static string Positionator (int direction, int x, int y) 
                {
                    return $"{direction} {x} {y}";
                }

                return amountEnergized;
            }


            stopwatch.Stop();

            Console.WriteLine($"execution time\t: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine("part one\t: " + partOne); // 6622
            Console.WriteLine("part two\t: " + partTwo); // 7130
        }
    }
}

