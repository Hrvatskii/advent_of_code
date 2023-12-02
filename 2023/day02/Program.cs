using System;

namespace day02 // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines(@"../input/day02.txt");
            int totalSum = 0;  // part 1
            int powerSums = 0; // part 2
            for (int i = 0; i < lines.Length; i++)
            {
                int gameNumber = int.Parse(lines[i].Split(':')[0].Split(' ')[1]);
                string information = lines[i].Split(':')[1]; // 3 blue, 4 red; ...
                information = information.Replace(";", ",");
                string[] values = information.Split(','); // { "3 blue", "4 red", ... }
                bool isValidGame = true; // used for part 1
                int minimumRed = 0, minimumGreen = 0, minimumBlue = 0; // used for part 2
                for (int j = 0; j < values.Length; j++)
                {
                    values[j] = values[j].Trim();
                    int amountOfBalls = int.Parse(values[j].Split(' ')[0]);
                    string color = values[j].Split(' ')[1];

                    if (color == "red")
                    {
                        if (amountOfBalls > 12) isValidGame = false;
                        if (amountOfBalls > minimumRed) minimumRed = amountOfBalls;
                    }
                    
                    if (color == "green")
                    {
                        if (amountOfBalls > 13) isValidGame = false;
                        if (amountOfBalls > minimumGreen) minimumGreen = amountOfBalls;
                    }

                    if (color == "blue")
                    {
                        if (amountOfBalls > 14) isValidGame = false;
                        if (amountOfBalls > minimumBlue) minimumBlue = amountOfBalls;
                    }
                }

                int power = minimumRed * minimumGreen * minimumBlue;
                powerSums += power;

                if (isValidGame)
                {
                    totalSum += gameNumber;
                }
            }
        Console.WriteLine("part 1: " + totalSum);  // 2476
        Console.WriteLine("part 2: " + powerSums); // 54911
        }
    }
}
