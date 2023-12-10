using System.Diagnostics;

namespace day10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("../input/day10.txt");
            var stopwatch = Stopwatch.StartNew();

            int height = lines.Length;
            string[] map = new string[height + 2];
            string padding = "".PadLeft(lines[0].Length + 2, '.');

            for (int i = 1; i < height + 1; i++)
                map[i] = "." + lines[i - 1] + ".";

            map[0] = padding;
            map[map.Length - 1] = padding;
            Console.WriteLine(map.Length);

            Console.WriteLine(String.Join("\n", map));

            int[] position = new int[2];


            var enterances = new Dictionary<string, char[]>()
            {
                { "1 0",  new char[] { '-', 'J', '7', 'S' } }, // east
                { "-1 0", new char[] { '-', 'L', 'F', 'S' } }, // west
                { "0 -1", new char[] { '|', '7', 'F', 'S' } }, // north
                { "0 1",  new char[] { '|', 'J', 'L', 'S' } }, // south
            };

            var validExits = new Dictionary<string, string[]>()
            {
                { "1 0",  new string[] { "-", "L", "F", "S" } }, // east
                { "-1 0", new string[] { "-", "J", "7", "S" } }, // west
                { "0 -1", new string[] { "|", "J", "L", "S" } }, // north
                { "0 1",  new string[] { "|", "7", "F", "S" } }, // south
            };

            int startingY = 0;

            while (!map[startingY].Contains("S"))
                startingY++;

            int startingX = map[startingY].IndexOf("S");
            position[0] = startingY;
            position[1] = startingX;

            int distance = 0;

            char currentSymbol = 'S';

            string previous = "0 0";

            List<string> wallCoordinates = new List<string>();

            while (true)
            {
                int y = position[0];
                int x = position[1];
                // go east
                if (x != map[y].Length - 1 && previous != "-1 0" && enterances["1 0"].Contains(map[y][x + 1]) && enterances["-1 0"].Contains(map[y][x]))
                    position[1]++;
                // go west
                else if (x != 0 && previous != "1 0" && enterances["-1 0"].Contains(map[y][x - 1]) && enterances["1 0"].Contains(map[y][x]))
                    position[1]--;
                // go north
                else if (y != 0 && previous != "0 1" && enterances["0 -1"].Contains(map[y - 1][x]) && enterances["0 1"].Contains(map[y][x]))
                    position[0]--;
                // go south
                else if (y != map.Length - 1 && previous != "0 -1" && enterances["0 1"].Contains(map[y + 1][x]) && enterances["0 -1"].Contains(map[y][x]))
                    position[0]++;

                distance++;
                currentSymbol = map[position[0]][position[1]];

                previous = $"{position[1] - x} {position[0] - y}";

                wallCoordinates.Add($"{position[1]} {position[0]}");

                if (currentSymbol == 'S')
                    break;
            }

            string sReplace = "";

            int deltaX = int.Parse(wallCoordinates[wallCoordinates.Count - 2].Split(' ')[0]) - int.Parse(wallCoordinates[0].Split(' ')[0]);
            int deltaY = int.Parse(wallCoordinates[wallCoordinates.Count - 2].Split(' ')[1]) - int.Parse(wallCoordinates[0].Split(' ')[1]);

            if (deltaX == 1 && deltaY == 1)
                sReplace = "F";
            else if (deltaX == 1 && deltaY == -1)
                sReplace = "L";
            else if (deltaX == -1 && deltaY == -1)
                sReplace = "J";
            else if (deltaX == -1 && deltaY == 1)
                sReplace = "7";
            else if (deltaX == 0 && deltaY == 2)
                sReplace = "|";
            else if (deltaX == 2 && deltaY == 0)
                sReplace = "-";

            map[startingY] = map[startingY].Replace("S", sReplace);
            
            int partOne = distance / 2;

            var queue = new Queue<string>();
            var visited = new Queue<string>();

            queue.Enqueue("0 0");

            int outside = 0;

            string[] vertical   = { "JL", "JF", "J|", "7L", "7F", "7|", "|L", "|F", "||" };
            string[] horizontal = { "JF", "J7", "J-", "LF", "L7", "L-", "-F", "-7", "--" };



            while (queue.Count != 0)
            {
                string current = queue.Dequeue();
                visited.Enqueue(current);

                int x = int.Parse(current.Split(' ')[0]);
                int y = int.Parse(current.Split(' ')[1]);

                if (Requirements(x, y, 1, 0, map, queue, visited, wallCoordinates))
                    queue.Enqueue($"{x+1} {y}");
                if (Requirements(x, y, 0, 1, map, queue, visited, wallCoordinates))
                    queue.Enqueue($"{x} {y+1}");
                if (Requirements(x, y, -1, 0, map, queue, visited, wallCoordinates))
                    queue.Enqueue($"{x-1} {y}");
                if (Requirements(x, y, 0, -1, map, queue, visited, wallCoordinates))
                    queue.Enqueue($"{x} {y-1}");

                outside++;
                Console.WriteLine(String.Join(" | ", queue));

            }

            Console.WriteLine(map.Length * map[0].Length - outside - partOne * 2 + " " + visited.Count);

            static bool Requirements(int x, int y, int offsetX, int offsetY, string[] map, Queue<string> queue, Queue<string> visited, List<string> wallCoordinates)
            {

                int limitX = 0, limitY = 0;

                if (offsetX == -1)
                    limitX = 0;
                else if (offsetX == 1)
                    limitX = map[0].Length - 1;
                else
                    limitX = -1;

                if (offsetY == -1)
                    limitY = 0;
                else if (offsetY == 1)
                    limitY = map.Length - 1;
                else
                    limitY = -1;

                return (x != limitX && y != limitY && !queue.Contains($"{x+offsetX} {y+offsetY}") && !visited.Contains($"{x+offsetX} {y+offsetY}") && !wallCoordinates.Contains($"{x+offsetX} {y+offsetY}"));

                
            }












 //               string type = current.Split(' ')[2];
 //
 //               if (type == "0")
 //               {
 //                   Console.WriteLine(current);
 //                   // "normal" no squeeze
 //                   if (x != map[y].Length - 1 && !queue.Contains($"{x+1} {y} 0") && !visited.Contains($"{x+1} {y} 0") && !wallCoordinates.Contains($"{x+1} {y}"))
 //                   {
 //                       queue.Enqueue($"{x+1} {y} 0");
 //                   }
 //                   if (x != 0 && !queue.Contains($"{x-1} {y} 0") && !visited.Contains($"{x-1} {y} 0") && !wallCoordinates.Contains($"{x-1} {y}"))
 //                   {
 //                       queue.Enqueue($"{x-1} {y} 0");
 //                   }
 //                   if (y != 0 && !queue.Contains($"{x} {y-1} 0") && !visited.Contains($"{x} {y-1} 0") && !wallCoordinates.Contains($"{x} {y-1}"))
 //                   {
 //                       queue.Enqueue($"{x} {y-1} 0");
 //                   }
 //                   if (y != map.Length - 1 && !queue.Contains($"{x} {y+1} 0") && !visited.Contains($"{x} {y+1} 0") && !wallCoordinates.Contains($"{x} {y+1}"))
 //                   {
 //                       queue.Enqueue($"{x} {y+1} 0");
 //                   }
 //
 //                   // squeeze horizontal and vertical
 //
 //                   // veritcal top left and top middle. save top left
 //                   if (y != 0 && x != 0 && !queue.Contains($"{x-1} {y-1} v") && !visited.Contains($"{x-1} {y-1} v") && wallCoordinates.Contains($"{x-1} {y-1}") 
 //                       && vertical.Contains($"{map[y-1][x-1]}{map[y-1][x]}"))
 //                   {
 //                       queue.Enqueue($"{x-1} {y-1} v");
 //                   }
 //                   // vertical top middle and top right. save top middle
 //                   if (y != 0 && x != map[y].Length - 1 && !queue.Contains($"{x} {y-1} v") && !visited.Contains($"{x} {y-1} v") && wallCoordinates.Contains($"{x} {y-1}")
 //                       && vertical.Contains($"{map[y-1][x]}{map[y-1][x+1]}"))
 //                   {
 //                       queue.Enqueue($"{x} {y-1} v");
 //                   }
 //                   // horizontal top right and middle right. save top right
 //                   if (y != 0 && x != map[y].Length - 1 && !queue.Contains($"{x+1} {y-1} h") && !visited.Contains($"{x+1} {y-1} h") && wallCoordinates.Contains($"{x+1} {y-1}")
 //                       && horizontal.Contains($"{map[y-1][x+1]}{map[y][x+1]}"))
 //                   {
 //                       queue.Enqueue($"{x+1} {y-1} h");
 //                   }
 //                   // horizontal middle right and bottom right. save middle right
 //                   if (y != map.Length - 1 && x != map[y].Length - 1 && !queue.Contains($"{x+1} {y} h") && !visited.Contains($"{x+1} {y} h") && wallCoordinates.Contains($"{x+1} {y}")
 //                       && horizontal.Contains($"{map[y][x+1]}{map[y+1][x+1]}"))
 //                   {
 //                       queue.Enqueue($"{x+1} {y} h");
 //                   }
 //                   // vertical bottom middle and bottom right. save bottom middle
 //                   if (y != map.Length && x != map[y].Length - 1 && !queue.Contains($"{x} {y+1} v") && !visited.Contains($"{x} {y+1} v") && wallCoordinates.Contains($"{x} {y+1}")
 //                       && vertical.Contains($"{map[y+1][x]}{map[y+1][x+1]}"))
 //                   {
 //                       queue.Enqueue($"{x} {y+1} v");
 //                   }
 //                   // vertical bottom left and bottom middle. save bottom left
 //                   if (y != map.Length && x != 0 && !queue.Contains($"{x-1} {y+1} v") && !visited.Contains($"{x-1} {y+1} v") && wallCoordinates.Contains($"{x-1} {y+1}")
 //                       && vertical.Contains($"{map[y+1][x-1]}{map[y+1][x]}"))
 //                   {
 //                       queue.Enqueue($"{x-1} {y+1} v");
 //                   }
 //                   // horizontal middle left and bottom left. save middle left
 //                   if (y != map.Length - 1 && x != 0 && !queue.Contains($"{x-1} {y} h") && !visited.Contains($"{x-1} {y} h") && wallCoordinates.Contains($"{x-1} {y}")
 //                       && horizontal.Contains($"{map[y][x-1]}{map[y+1][x-1]}"))
 //                   {
 //                       queue.Enqueue($"{x-1} {y} h");
 //                   }
 //                   // horizontal top left and middle left. save top left
 //                   if (y != 0 && x != 0 && !queue.Contains($"{x-1} {y-1} h") && !visited.Contains($"{x-1} {y-1} h") && wallCoordinates.Contains($"{x-1} {y-1}")
 //                       && horizontal.Contains($"{map[y-1][x-1]}{map[y][x-1]}"))
 //                   {
 //                       queue.Enqueue($"{x-1} {y-1} h");
 //                   }
 //
 //
 //
 //
 //
 //                   outside++;
 //               }
 //               else if (type == "v")
 //               {
 //                   // vertical top middle and top right. save top middle
 //                   if (y != 0 && x != map[y].Length - 1 && !queue.Contains($"{x} {y-1} v") && !visited.Contains($"{x} {y-1} v") && wallCoordinates.Contains($"{x} {y-1}") && vertical.Contains($"{map[y-1][x]}{map[y-1][x+1]}"))
 //                       queue.Enqueue($"{x} {y-1} v");
 //                   // normal up
 //                   else if (y != 0 && !queue.Contains($"{x} {y-1} 0") && !visited.Contains($"{x} {y-1} 0") && !wallCoordinates.Contains($"{x} {y-1}"))
 //                       queue.Enqueue($"{x} {y-1} 0");
 //                   else if (y != 0 && x != map[y].Length - 1 && !queue.Contains($"{x+1} {y-1} 0") && !visited.Contains($"{x+1} {y-1} 0") && !wallCoordinates.Contains($"{x+1} {y-1}"))
 //                       queue.Enqueue($"{x+1} {y-1} 0");
 //
 //                   // vertical bottom middle and bottom right. save bottom middle
 //                   if (y != map.Length && x != map[y].Length - 1 && !queue.Contains($"{x} {y+1} v") && !visited.Contains($"{x} {y+1} v") && wallCoordinates.Contains($"{x} {y+1}") && vertical.Contains($"{map[y+1][x]}{map[y+1][x+1]}"))
 //                       queue.Enqueue($"{x} {y+1} v");
 //                   else if (y != map.Length - 1 && !queue.Contains($"{x} {y+1} 0") && !visited.Contains($"{x} {y+1} 0") && !wallCoordinates.Contains($"{x} {y+1}"))
 //                       queue.Enqueue($"{x} {y+1} 0");
 //                   else if (y != map.Length - 1 && x != map[y].Length - 1 && !queue.Contains($"{x+1} {y+1} 0") && !visited.Contains($"{x+1} {y+1} 0") && !wallCoordinates.Contains($"{x+1} {y+1}"))
 //                       queue.Enqueue($"{x+1} {y+1} 0");
 //
 //                   if (y != 0 && !queue.Contains($"{x} {y-1} h") && !visited.Contains($"{x} {y-1} h") && wallCoordinates.Contains($"{x} {y-1}") && horizontal.Contains($"{map[y-1][x]}{map[y][x]}"))
 //                       queue.Enqueue($"{x} {y-1} h");
 //                   if (y != 0 && x != map[y].Length - 1 && !queue.Contains($"{x+1} {y-1} h") && !visited.Contains($"{x+1} {y-1} h") && wallCoordinates.Contains($"{x+1} {y-1}") && horizontal.Contains($"{map[y-1][x+1]}{map[y][x+1]}"))
 //                       queue.Enqueue($"{x+1} {y-1} h");
 //                   if (y != map.Length - 1 && !queue.Contains($"{x} {y} h") && !visited.Contains($"{x} {y} h") && wallCoordinates.Contains($"{x} {y}") && horizontal.Contains($"{map[y][x]}{map[y+1][x]}"))
 //                       queue.Enqueue($"{x} {y} h");
 //                   if (y != map.Length - 1 && x != map[y].Length - 1 && !queue.Contains($"{x+1} {y} h") && !visited.Contains($"{x+1} {y} h") && wallCoordinates.Contains($"{x+1} {y}") && horizontal.Contains($"{map[y][x+1]}{map[y+1][x+1]}"))
 //                       queue.Enqueue($"{x+1} {y} h");
 //
 //               }
 //               else if (type == "h")
 //               {
 //                   // horizontal middle left and bottom left. save middle left
 //                   if (y != map.Length - 1 && x != 0 && !queue.Contains($"{x-1} {y} h") && !visited.Contains($"{x-1} {y} h") && wallCoordinates.Contains($"{x-1} {y}") && horizontal.Contains($"{map[y][x-1]}{map[y+1][x-1]}"))
 //                       queue.Enqueue($"{x-1} {y} h");
 //                   else if (x != 0 && !queue.Contains($"{x-1} {y} 0") && !visited.Contains($"{x-1} {y} 0") && !wallCoordinates.Contains($"{x-1} {y}"))
 //                       queue.Enqueue($"{x-1} {y} 0");
 //                   else if (x != 0 && y != map.Length - 1 && !queue.Contains($"{x-1} {y+1} 0") && !visited.Contains($"{x-1} {y+1} 0") && !wallCoordinates.Contains($"{x-1} {y+1}"))
 //                       queue.Enqueue($"{x-1} {y+1} 0");
 //
 //                   // horizontal middle right and bottom right. save middle right
 //                   if (y != map.Length - 1 && x != map[y].Length - 1 && !queue.Contains($"{x+1} {y} h") && !visited.Contains($"{x+1} {y} h") && wallCoordinates.Contains($"{x+1} {y}") && horizontal.Contains($"{map[y][x+1]}{map[y+1][x+1]}"))
 //                       queue.Enqueue($"{x+1} {y} h");
 //                   else if (x != map[y].Length - 1 && !queue.Contains($"{x+1} {y} 0") && !visited.Contains($"{x+1} {y} 0") && !wallCoordinates.Contains($"{x+1} {y}"))
 //                       queue.Enqueue($"{x+1} {y} 0");
 //                   else if (x != map[y].Length - 1 && y != map.Length - 1 && !queue.Contains($"{x+1} {y+1} 0") && !visited.Contains($"{x+1} {y+1} 0") && !wallCoordinates.Contains($"{x+1} {y+1}"))
 //                       queue.Enqueue($"{x+1} {y+1} 0");
 //
 //                   
 //                   if (x != 0 && !queue.Contains($"{x-1} {y} v") && !visited.Contains($"{x-1} {y} v") && wallCoordinates.Contains($"{x-1} {y}") && vertical.Contains($"{map[y][x-1]}{map[y][x]}"))
 //                       queue.Enqueue($"{x-1} {y} v");
 //                   if (x != 0 && y != map.Length - 1 && !queue.Contains($"{x-1} {y+1} v") && !visited.Contains($"{x-1} {y+1} v") && wallCoordinates.Contains($"{x-1} {y+1}") && vertical.Contains($"{map[y+1][x-1]}{map[y+1][x]}"))
 //                       queue.Enqueue($"{x-1} {y+1} v");
 //                   if (x != map[y].Length - 1 && !queue.Contains($"{x+1} {y} v") && !visited.Contains($"{x+1} {y} v") && wallCoordinates.Contains($"{x+1} {y}") && vertical.Contains($"{map[y][x+1]}{map[y][x]}"))
 //                       queue.Enqueue($"{x+1} {y} v");
 //                   if (x != map[y].Length - 1 && y != map.Length - 1 && !queue.Contains($"{x+1} {y+1} v") && !visited.Contains($"{x+1} {y+1} v") && wallCoordinates.Contains($"{x+1} {y+1}") && vertical.Contains($"{map[y+1][x+1]}{map[y+1][x]}"))
 //                       queue.Enqueue($"{x+1} {y+1} v");
 //               }
 //               else 
 //               {
 //                   Console.WriteLine("oh no");
 //               }
 //               if (y != 0 && x != map[y].Length - 1 && !queue.Contains($"{x+1} {y-1} 0") && !visited.Contains($"{x+1} {y-1} 0") && !wallCoordinates.Contains($"{x+1} {y-1}") && $"{map[y-1][x]}{map[y][x+1]}" == "JF")
 //                   queue.Enqueue($"{x+1} {y-1} 0");
 //
 //               if (y != map.Length - 1 && x != map[y].Length - 1 && !queue.Contains($"{x+1} {y+1} 0") && !visited.Contains($"{x+1} {y+1} 0") && !wallCoordinates.Contains($"{x+1} {y+1}") && $"{map[y+1][x]}{map[y][x+1]}" == "7L")
 //                   queue.Enqueue($"{x+1} {y+1} 0");
 //
 //               if (y != map.Length - 1 && x != 0 && !queue.Contains($"{x-1} {y+1} 0") && !visited.Contains($"{x-1} {y+1} 0") && !wallCoordinates.Contains($"{x-1} {y+1}") && $"{map[y+1][x]}{map[y][x-1]}" == "FJ")
 //                   queue.Enqueue($"{x-1} {y+1} 0");
 //
 //               if (y != 0 && x != 0 && !queue.Contains($"{x-1} {y-1} 0") && !visited.Contains($"{x-1} {y-1} 0") && !wallCoordinates.Contains($"{x-1} {y-1}") && $"{map[y-1][x]}{map[y][x-1]}" == "L7")
 //                   queue.Enqueue($"{x-1} {y-1} 0");
 //
 //           }
 //           int a = 0;
 //           for (int i = 0; i < map.Length; i++)
 //               for (int j = 0; j < map[i].Length; j++)
 //                   if (!visited.Contains($"{j} {i} 0") && !wallCoordinates.Contains($"{j} {i}"))
 //                       a++;
 //           int inside = (map.Length * map[0].Length) - outside - partOne * 2;
 //
 //           Console.WriteLine(inside + " " + a + " " + outside);
            stopwatch.Stop();


            Console.WriteLine($"execution time\t: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine("part one\t: " + partOne);
            Console.WriteLine("part two\t: ");
        }
    }
}

