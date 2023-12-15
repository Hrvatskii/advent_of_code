using System.Diagnostics;

namespace day13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("../input/day13.txt");
            var stopwatch = Stopwatch.StartNew();

            string[] boards = String.Join("\n", lines).Split("\n\n");


            int partOne = 0, partTwo = 0;

            foreach (string board in boards)
            {
                string[] rows = board.Split("\n");

                int boardValue = 0, boardValueTwo = 0;

                int[] boardValues = FindMiddle(rows);

                boardValue += boardValues[0] * 100;
                boardValueTwo += boardValues[1] * 100;

                Console.WriteLine("");

                int[] boardValuesVertical = FindMiddle(Rotate(rows));

                if (boardValue == 0)
                    boardValue += boardValuesVertical[0];

                if (boardValueTwo == 0)
                    boardValueTwo += boardValuesVertical[1];

                if (boardValueTwo == 0)
                    Console.WriteLine("something is wrong");
                
                Console.WriteLine("hello" + boardValueTwo);
                partOne += boardValue;
                partTwo += boardValueTwo;
            }


            static string[] Rotate (string[] board)
            {
                string[,] temp = new string[board[0].Length, board.Length]; 
                string[] output = new string[board[0].Length];

                for (int i = 0; i < board.Length; i++)
                    for (int j = 0; j < board[0].Length; j++)
                        temp[j, board.Length - 1 - i] = board[i][j].ToString();

                for (int i = 0; i < temp.GetLength(0); i++)
                {
                    string a = "";
                    for (int j = 0; j < temp.GetLength(1); j++)
                        a += temp[i, j];
                    output[i] = a;
                }

                return output;
            }

            static int[] FindMiddle (string[] board)
            {
                int middle = 0, middleTwo = 0;
                bool potential = false, potentialTwo = false, partTwoDone = false;
                string[] copy = board;
                for (int i = 0; i < board.Length; i++)
                {
                    for (int j = board.Length - 1; j > i; j--)
                    {
                        if (board[i] == board[j])
                        {
                            if (!potential)
                            {
 //                               Console.WriteLine("here" + i + " " + j);
                                potential = true;
                                middle = (int) (((i + j ) / 2) + 1); 
                                int offset = Math.Min((board.Length - 1 - j), i);
                                for (int k = i - offset; k < middle; k++)
                                {
                                    if (board[k] != board[j + i - k])
                                    {
                                        potential = false;
                                        break;
                                    }
                                }

                            }
                                    

                            
                        }
                        int[] values = FindError(board[i], board[j]);
                        bool hasOneError = values[0] == 1 ? true : false;
                        int errorIndex = values[1];
                        if (hasOneError && !partTwoDone)
                        {

                            if (!potentialTwo)
                            {
 //                               Console.WriteLine("here" + i + " " + j);
                                potentialTwo = true;
                                middleTwo = (int) (((i + j ) / 2) + 1); 
                                int offset = Math.Min((board.Length - 1 - j), i);
                                for (int k = i - offset; k < middleTwo; k++)
                                {
                                    if (j + i - k - k == 1)
                                    {
                                        if (board[k] == board[j + i - k])
                                        {
                                            partTwoDone = true;
                                            break;
                                        }
                                    }
                                    if (k == i)
                                        continue;
                                    if (board[k] != board[j + i - k])
                                    {
                                        potentialTwo = false;
                                        break;
                                    }
                                }

                            }
                        }
                    }
                }
                if (potential && potentialTwo)
                    return new int[] { middle, middleTwo };
                else if (potential && !potentialTwo)
                    return new int[] { middle, 0 };
                else if (!potential && potentialTwo)
                    return new int[] { 0, middleTwo };
                else
                    return new int[] { 0, 0 };
            }

            static int[] FindError(string one, string two)
            {
                int error = 0;
                int errorIndex = 0;

                for (int i = 0; i < one.Length; i++)
                {
                    if (one[i] != two[i])
                    {
                        error++;
                        errorIndex = i;
                    }
                    if (error > 1)
                        return new int[] { 0, 0 };
                }

                return error == 1 ? new int[] { 1, errorIndex } : new int[] { 0, 0 };
            }

            Console.WriteLine("\n");

            stopwatch.Stop();

            Console.WriteLine($"execution time\t: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine("part one\t: " + partOne);
            Console.WriteLine("part two\t: " + partTwo);
        }
    }
}

