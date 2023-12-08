using System.Diagnostics;

namespace day08
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("../input/day08.txt");
            var stopwatch = Stopwatch.StartNew();

            string instructions = lines[0];

            string[] network = lines.Skip(2).ToArray();

            Dictionary<string, string[]> map = new Dictionary<string, string[]>();
            List<string> positions = new List<string>();

            foreach (string nodePointer in network)
            {
                string node = nodePointer.Substring(0, 3);

                string[] directions = new string[2];
                directions[0] = nodePointer.Substring(7, 3);
                directions[1] = nodePointer.Substring(12, 3);

                if (node.EndsWith("A"))
                    positions.Add(node);

                map.Add(node, directions);
            }


            string currentNode = "AAA";
            int counter = 0, partOne = 0;
            // part 1
            while (currentNode != "ZZZ")
            {

                int currentDirection = instructions[counter] == 'L' ? 0 : 1;

                currentNode = map[currentNode][currentDirection];
                partOne++;
                counter = partOne % instructions.Length;
            }

            // part 2
            // i dont like this because i copied it nearly 1:1 from someone else
            // i would have maybe come up with the solution myself but im tired okay
            // and i dont want to fall behind too far
            List<int> factors = new List<int>();
            int factor = 0;
            foreach (string position in positions)
            {
                factor = 0;
                currentNode = position;
                while (!currentNode.EndsWith("Z"))
                {
                    int currentDirection = instructions[counter] == 'L' ? 0 : 1;

                    currentNode = map[currentNode][currentDirection];
                    factor++;
                    counter = factor % instructions.Length;
                }
                factors.Add(factor);
            }

            long greatestFactor = factors[0];

            for (int i = 1; i < factors.Count; i++)
                greatestFactor = GreatestFactor(greatestFactor, factors[i]);                

            long partTwo = 1;

            for (int i = 0; i < factors.Count; i++)
                partTwo *= factors[i] / greatestFactor;

            partTwo *= greatestFactor;

            static long GreatestFactor(long numberOne, long numberTwo)
            {
                while (numberOne != 0 && numberTwo != 0)
                {
                    if (numberOne > numberTwo)
                        numberOne %= numberTwo;
                    else
                        numberTwo %= numberOne;
                }

                return numberOne | numberTwo;
            }

            stopwatch.Stop();

            Console.WriteLine($"execution time\t: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine("part one\t: " + partOne); // 19667
            Console.WriteLine("part two\t: " + partTwo); // 19185263738117
        }
    }
}

