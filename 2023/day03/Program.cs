using System;

namespace day03 // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("../input/day03.txt");
            int partOne = 0;
            int partTwo = 0;
            
            // structure:
            // "yPosition, xPosition": [count, gearRatio]
            Dictionary<string, int[]> gearMap = new Dictionary<string, int[]>();

            Dictionary<int, int[]> gearPositions = new Dictionary<int, int[]>();
            
            // corresponds to the index of neighbors
            gearPositions.Add(0, new int[] {-1, -1});
            gearPositions.Add(1, new int[] {-1, 0});
            gearPositions.Add(2, new int[] {-1, 1});
            gearPositions.Add(3, new int[] {0, -1});
            gearPositions.Add(4, new int[] {0, 1});
            gearPositions.Add(5, new int[] {1, -1});
            gearPositions.Add(6, new int[] {1, 0});
            gearPositions.Add(7, new int[] {1, 1});


            for (int i = 0; i < lines.Length; i++)
            {
                // dont reset for each digit. we need to save these for the entire number
                int gearOffsetX = 0, gearOffsetY = 0, gearPositionX = 0, gearPositionY = 0;

                // stores the number we're looking at and not just each digit
                int number = 0;

                bool isPartNumber = false;
                bool hasFoundGear = false;

                for (int j = 0; j < lines[i].Length; j++)
                {

                    // we don't really care if it's not a digit
                    if (!Char.IsDigit(lines[i][j])) continue;

                    // save the value of each neighbor
                    string[] neighbors = new string[8];

                    if (i != 0 && j != 0)                                  neighbors[0] = lines[i - 1][j - 1].ToString();
                    if (i != 0)                                            neighbors[1] = lines[i - 1][j].ToString();
                    if (i != 0 && j != lines[i].Length - 1)                neighbors[2] = lines[i - 1][j + 1].ToString();

                    if (j != 0)                                            neighbors[3] = lines[i][j - 1].ToString();
                    if (j != lines[i].Length - 1)                          neighbors[4] = lines[i][j + 1].ToString();

                    if (i != lines.Length - 1 && j != 0)                   neighbors[5] = lines[i + 1][j - 1].ToString();
                    if (i != lines.Length - 1)                             neighbors[6] = lines[i + 1][j].ToString();
                    if (i != lines.Length - 1 && j != lines[i].Length - 1) neighbors[7] = lines[i + 1][j + 1].ToString();

                    for (int neighborIndex = 0; neighborIndex < neighbors.Length; neighborIndex++)
                    {
                        string neighbor = neighbors[neighborIndex];

                        // check if there is a symbol for part 1
                        if (!int.TryParse(neighbor, out _) && neighbor != "." && neighbor != null)
                        {
                            isPartNumber = true;
                        }
                        
                        // check if we're neighboring a "*" and we haven't found a gear yet for part 2
                        if (!hasFoundGear && neighbor == "*")
                        {
                            gearOffsetX = gearPositions[neighborIndex][1];
                            gearOffsetY = gearPositions[neighborIndex][0];

                            gearPositionX = j + gearOffsetX;
                            gearPositionY = i + gearOffsetY;

                            if (!gearMap.ContainsKey($"{gearPositionY} {gearPositionX}"))
                            {
                                gearMap.Add($"{gearPositionY} {gearPositionX}", new int[2]);
                                gearMap[$"{gearPositionY} {gearPositionX}"][0] = 0;
                                gearMap[$"{gearPositionY} {gearPositionX}"][1] = 1;
                            }
                            hasFoundGear = true;
                        }
                    }
                    
                    // add to the number we're looking at
                    number = number * 10 + (lines[i][j] - '0');

                    // check if the character to the right is not a digit or the edge of the map. if true, we're at the end of the number
                    if (!int.TryParse(neighbors[4], out _) || j == lines[i].Length)
                    {
                        if (isPartNumber)
                        {
                            partOne += number;
                        }

                        if (hasFoundGear)
                        {
                            gearMap[$"{gearPositionY} {gearPositionX}"][0]++;
                            gearMap[$"{gearPositionY} {gearPositionX}"][1] *= number;

                        }

                        // reset values for number-specific variables
                        number = 0;
                        isPartNumber = false;
                        hasFoundGear = false;
                    }

                }
            }
            
            // part 2 things
            foreach (int[] data in gearMap.Values)
            {
                if (data[0] == 2)
                {
                    partTwo += data[1];
                }
            }

            Console.WriteLine(partOne); // 529618
            Console.WriteLine(partTwo); // 77509019
        }
    }
}
