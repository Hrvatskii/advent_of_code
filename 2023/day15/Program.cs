using System.Diagnostics;

namespace day15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("../input/day15.txt");
            var stopwatch = Stopwatch.StartNew();

            string[] sequence = lines[0].Split(',');

            List<List<string>> boxesLabels = new List<List<string>>();
            List<List<int>> boxesFocalLengths = new List<List<int>>();
            for (int i = 0; i < 256; i++)
            {
                boxesLabels.Add(new List<string>());
                boxesFocalLengths.Add(new List<int>());
            }

            int partOne = 0;
            int partTwo = 0;

            foreach (string step in sequence)
            {
                partOne += GetValue(step);
                
                string label = step.EndsWith('-') ? step.Substring(0, step.IndexOf('-')) : step.Substring(0, step.IndexOf('='));
                int box = GetValue(label);

                if (step.Contains('-'))
                {
                    if (boxesLabels[box].Contains(label))
                    {
                        int index = boxesLabels[box].IndexOf(label);
                        boxesLabels[box].RemoveAt(index);
                        boxesFocalLengths[box].RemoveAt(index);
                    }
                }
                else
                {
                    if (boxesLabels[box].Contains(label))
                    {
                        int index = boxesLabels[box].IndexOf(label);
                        boxesFocalLengths[box][index] = int.Parse(step[step.Length - 1].ToString());
                    }
                    else
                    {
                        boxesLabels[box].Add(label);
                        boxesFocalLengths[box].Add(int.Parse(step[step.Length - 1].ToString()));
                    }
                }
            }

            for (int i = 0; i < 256; i++)
            {
                List<int> box = boxesFocalLengths[i];

                for (int j = 0; j < box.Count; j++)
                    partTwo += (i + 1) * (j + 1) * box[j];
            }

            static int GetValue (string characters)
            {
                int value = 0;

                foreach (int letter in characters)
                {
                    value += letter;
                    value *= 17;
                    value %= 256;
                }

                return value;
            }

            stopwatch.Stop();

            Console.WriteLine($"execution time\t: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine("part one\t: " + partOne); // 513643
            Console.WriteLine("part two\t: " + partTwo); // 265345
        }
    }
}

