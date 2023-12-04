using System.Diagnostics;

namespace day04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("../input/day04.txt");
            var stopwatch = Stopwatch.StartNew();

            int[] amountOfCards = new int[lines.Length];
            amountOfCards = amountOfCards.Select(n => 1).ToArray();

            int partOne = 0;
            int partTwo = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                int corrects = 0;
                
                lines[i] = lines[i].Substring(8);

                string[] ourNumbers = lines[i].Split('|')[1]
                                              .Split(' ')  
                                              .ToArray();

                string[] winningNumbers = lines[i].Split('|')[0]
                                                  .Split(' ')
                                                  .ToArray();

                foreach (string number in ourNumbers)
                {
                    if (number == "") 
                        continue;

                    if (winningNumbers.Contains(number))
                        corrects++;
                }
        
                for (int j = 0; j < corrects; j++)
                {
                    amountOfCards[i + j + 1] += amountOfCards[i];
                }

                partOne += (int)(Math.Pow(2, corrects - 1));
            }
            partTwo = amountOfCards.Sum();
            stopwatch.Stop();

            Console.WriteLine($"execution time\t: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine("part one\t: " + partOne); // 23235
            Console.WriteLine("part two\t: " + partTwo); // 5920640
        }
    }
}

