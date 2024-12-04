using System.Diagnostics;

namespace day17
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("../input/day17.txt");
            var stopwatch = Stopwatch.StartNew();

            List<string> visited = new List<string>();
            visited.Add("0 0");
            List<char> directions = new List<char>();
            int partOne = 10000000;
            DFS(0, 0, visited, lines[0].Length - 1, lines.Length - 1, directions, 0, lines, ref partOne);


            static void DFS (int x, int y, List<string> visited, int endX, int endY, List<char> directions, int sum, string[] lines, ref int partOne)
            {
                if (x == endX && y == endY && sum < partOne)
                {
                    partOne = sum;
                }
                else if (sum < partOne && (4 * (endY - y + endX - x)) < ((partOne - sum)))
                {
                    List<string> neighbors = FindNeighbors(x, y, visited, directions, endX, endY);

                    foreach (string neighbor in neighbors)
                    {
                        int nx = int.Parse(neighbor.Split(' ')[0]);
                        int ny = int.Parse(neighbor.Split(' ')[1]);
                        char direction = char.Parse(neighbor.Split(' ')[2]);
                        int heatLoss = lines[ny][nx] - '0';
                        directions.Add(direction);
                        visited.Add($"{nx} {ny}");
                        sum += heatLoss;
                        
                        DFS(nx, ny, visited, endX, endY, directions, sum, lines, ref partOne);

                        directions.RemoveAt(directions.Count - 1);
                        visited.RemoveAt(visited.Count - 1);
                        sum -= heatLoss;
                    }
                }
            }

            static List<string> FindNeighbors (int x, int y, List<string> visited, List<char> directions, int edgeX, int edgeY)
            {
                List<string> neighbors = new List<string>();
                char disallowed = 'x';
                if (directions.Count > 2 && directions[directions.Count - 1] == directions[directions.Count - 2] && directions[directions.Count - 2] == directions[directions.Count - 3])
                    disallowed = directions[directions.Count - 1];

                // north
                if (y != 0 && disallowed != 'n' && IsValid(x, y, visited, 'n'))
                    neighbors.Add($"{x} {y - 1} n");
                // east
                if (x != edgeX && disallowed != 'e' && IsValid(x, y, visited, 'e'))
                    neighbors.Add($"{x + 1} {y} e");
                // south
                if (y != edgeY && disallowed != 's' && IsValid(x, y, visited, 's'))
                    neighbors.Add($"{x} {y + 1} s");
                // west
                if (x != 0 && disallowed != 'w' && IsValid(x, y, visited, 'w'))
                    neighbors.Add($"{x - 1} {y} w");

                return neighbors;
            }

            static bool IsValid (int x, int y, List<string> visited, char direction)
            {
                if (direction == 'n' && (visited.Contains($"{x + 1} {y - 1}") || visited.Contains($"{x - 1} {y - 1}") || visited.Contains($"{x} {y - 2}") || visited.Contains($"{x} {y - 1}")))
                    return false;
                if (direction == 'e' && (visited.Contains($"{x + 1} {y - 1}") || visited.Contains($"{x + 1} {y + 1}") || visited.Contains($"{x + 2} {y}") || visited.Contains($"{x + 1} {y}")))
                    return false;
                if (direction == 's' && (visited.Contains($"{x + 1} {y + 1}") || visited.Contains($"{x - 1} {y + 1}") || visited.Contains($"{x} {y + 2}") || visited.Contains($"{x} {y + 1}")))
                    return false;
                if (direction == 'w' && (visited.Contains($"{x - 1} {y - 1}") || visited.Contains($"{x - 1} {y + 1}") || visited.Contains($"{x - 2} {y}") || visited.Contains($"{x - 1} {y}")))
                    return false;

                return true;
            }

            Console.WriteLine(partOne);

            stopwatch.Stop();

            Console.WriteLine($"execution time\t: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine("part one\t: ");
            Console.WriteLine("part two\t: ");
        }
    }
}

