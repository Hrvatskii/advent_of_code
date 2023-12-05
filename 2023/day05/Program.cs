using System.Diagnostics;

namespace day05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("../input/day05.txt");
            var stopwatch = Stopwatch.StartNew();

            long partOne;

            Console.WriteLine("hello");

            long[] seedList = lines[0].Split(": ")[1]
                                     .Split(' ')
                                     .Select(n => Convert.ToInt64(n))
                                     .ToArray();

//            long lowestLocationNumber = 0;
//            foreach (long seed in seedList)
//            {
//                long currentCategory = seed;
//                bool isWithinRange = false;
//                long difference = 0;
//                string category = "";
//                for (int i = 1; i < lines.Length; i++)
//                {
//                    if (lines[i] == "")
//                    {
//                        if (isWithinRange)
//                            currentCategory += difference;
//                        if (category == "humidity-to-location map:")
//                        {
//                           if (lowestLocationNumber == 0 || currentCategory < lowestLocationNumber)
//                               lowestLocationNumber = currentCategory;
//                        }
//                        isWithinRange = false;
//                    }
//                    else if (int.TryParse(lines[i][0].ToString(), out _))
//                    {
//                        long[] data = lines[i].Split(' ').Select(n => Convert.ToInt64(n)).ToArray();
//                        long destination = data[0];
//                        long source = data[1];
//                        long range = data[2];
//
//                        if (currentCategory >= source && currentCategory <= source + range && !isWithinRange)
//                        {
//                            isWithinRange = true;
//                            difference = destination - source;
//                        }
//                    }
//                    else 
//                    {
//                        category = lines[i];
//                    }
//
//                }
//            }

//            partOne = lowestLocationNumber;

            long partTwo = 0;
            bool hasFoundPartTwo = false;
            long counter = 0;
            var a = Stopwatch.StartNew();
            while (!hasFoundPartTwo)
            {
                long seed = counter;
                long difference = 0;
                bool isWithinRange = false;
                for (int i = lines.Length - 1; i > 0; i--) 
                {
                    if (lines[i] == "") {
                        if (isWithinRange)
                            seed += difference;
                        isWithinRange = false;
                    }
                    else if ((int)lines[i][0] >= 48 && (int)lines[i][0] <= 57)
                    {

                        long[] data = lines[i].Split(' ').Select(n => long.Parse(n)).ToArray();
                        long destination = data[0];
                        long source = data[1];
                        long range = data[2];

                        if (seed >= destination && seed <= destination + range)
                        {
                            difference = source - destination;
                            isWithinRange = true;
                        }
                    }

                }
                for (int i = 0; i < seedList.Length; i += 2)
                {
                    if (seed >= seedList[i] && seed <= seedList[i] + seedList[i + 1]) {
                        hasFoundPartTwo = true;
                        partTwo = counter;
                    }
                }
                counter++;

                if (counter % 10000 == 0) {
                    a.Stop();
                    Console.WriteLine("passed " + counter + " " + a.ElapsedMilliseconds);
                    a = Stopwatch.StartNew();
                }
            }
            Console.WriteLine("2 " + partTwo);

            stopwatch.Stop();

            Console.WriteLine($"execution time\t: {stopwatch.ElapsedMilliseconds} ms");
 //           Console.WriteLine("part one\t: " + partOne);
            Console.WriteLine("part two\t: ");
        }
    }
}

