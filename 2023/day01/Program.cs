using System;

namespace day01 // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines(@"../input/day01.txt");
            int totalSum = 0;
            // part 1 *********************************************************
//            for (int i = 0; i < lines.Length; i++)
//            {
//                string currentLine = lines[i];
//                int partialSum = 0;
//                int j = 0;
//                while ((int)currentLine[j] > 58 || (int)currentLine[j] < 48)
//                {
//                    j++;
//                }
//                partialSum = (currentLine[j] - '0') * 10;
//                j = currentLine.Length - 1;
//
//                while ((int)currentLine[j] > 58 || (int)currentLine[j] < 48)
//                {
//                    j--;
//                }
//                partialSum += (currentLine[j] - '0');
//
//                totalSum += partialSum;
//            }
//            Console.WriteLine(totalSum); // 54697
//
            // part 2 *********************************************************
            string[] digits = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            for (int i = 0; i < lines.Length; i++)
            {
                string currentLine = lines[i];
                int partialSum = 0;
                string digitInLetters = "";

                for (int j = 0; j < currentLine.Length; j++)
                {
                    if ((int)currentLine[j] < 58 && (int)currentLine[j] > 48)
                    {
                        partialSum = (currentLine[j] - '0') * 10;
                        j = currentLine.Length;
                    }
                    else
                    {
                        digitInLetters += currentLine[j];

                        for (int k = 0; k < digits.Length; k++)
                        {
                            if (digitInLetters.Contains(digits[k]))
                            {
                                partialSum = (k + 1) * 10;
                                j = currentLine.Length;
                            }
                        }
                    }
                }
                digitInLetters = "";
                for (int j = currentLine.Length - 1; j >= 0; j--)
                {
                    if ((int)currentLine[j] < 58 && (int)currentLine[j] > 48)
                    {
                        partialSum += (currentLine[j] - '0');
                        j = 0;
                    }
                    else
                    {
                        digitInLetters = currentLine[j] + digitInLetters;

                        for (int k = 0; k < digits.Length; k++)
                        {
                            if (digitInLetters.Contains(digits[k]))
                            {
                                partialSum += k + 1;
                                j = 0;
                            }
                        }
                    }
                }
                totalSum += partialSum;
            }
            Console.WriteLine(totalSum); // 54885
        }
    }
}
