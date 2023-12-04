namespace day04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("../input/day04.txt");

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
        
                for (int j = 0; j < amountOfCards[i]; j++)
                {
                    for (int k = 0; k < corrects; k++)
                    {
                        amountOfCards[i + k + 1]++;
                    }
                }

                partOne += (int)(Math.Pow(2, corrects - 1));
            }
            partTwo = amountOfCards.Sum();

            Console.WriteLine(partOne); // 23235
            Console.WriteLine(partTwo); // 5920640
        }
    }
}

