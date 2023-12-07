using System.Diagnostics;

namespace day07
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("../input/day07.txt");
            var stopwatch = Stopwatch.StartNew();

            string[] cards = lines.Select(n => n.Split(' ')[0]).ToArray();
            int [] bids = lines.Select(n => int.Parse(n.Split(' ')[1])).ToArray();

            List<int> five = new List<int>();               
            List<int> four = new List<int>();               
            List<int> full = new List<int>();               
            List<int> three = new List<int>();               
            List<int> two = new List<int>();               
            List<int> one = new List<int>();               
            List<int> high = new List<int>();               
            
            List<int> five2 = new List<int>();               
            List<int> four2 = new List<int>();               
            List<int> full2 = new List<int>();               
            List<int> three2 = new List<int>();               
            List<int> two2 = new List<int>();               
            List<int> one2 = new List<int>();               
            List<int> high2 = new List<int>();               


            Dictionary<int, int> bidMap = new Dictionary<int, int>();
            Dictionary<int, int> bidMap2 = new Dictionary<int, int>();

            for (int i = 0; i < lines.Length; i++)
            {
                string card = cards[i];
                string card2 = cards[i];
                string cardCopy = card;

                List<int> counts = new List<int>();               

                int jokerCount = cardCopy.Count(n => n == 'J');
                while (cardCopy.Length > 0)
                {
                    int count = cardCopy.Count(n => n == cardCopy[0]);
                    counts.Add(count);
                    cardCopy = cardCopy.Replace(cardCopy[0].ToString(), "");
                }
                counts.Sort();

                card2 = card2.Replace("J", "1");
                card2 = card2.Replace("T", "B");
                card2 = card2.Replace("Q", "C");
                card2 = card2.Replace("K", "D");
                card2 = card2.Replace("A", "E");

                card = card.Replace("2", "1");
                card = card.Replace("3", "2");
                card = card.Replace("4", "3");
                card = card.Replace("5", "4");
                card = card.Replace("6", "5");
                card = card.Replace("7", "6");
                card = card.Replace("8", "7");
                card = card.Replace("9", "8");
                card = card.Replace("T", "9");
                card = card.Replace("J", "B");
                card = card.Replace("Q", "C");
                card = card.Replace("K", "D");
                card = card.Replace("A", "E");

                int cardValue = Convert.ToInt32(card, 16);
                int cardValue2 = Convert.ToInt32(card2, 16);

                bidMap.Add(cardValue, bids[i]);
                bidMap2.Add(cardValue2, bids[i]);

                if (counts[counts.Count - 1] == 5)
                {
                    five.Add(cardValue);
                    five2.Add(cardValue2);
                }
                else if (counts[counts.Count - 1] == 4) 
                {
                    four.Add(cardValue);
                    if (jokerCount == 1)
                        five2.Add(cardValue2);
                    else if (jokerCount == 4)
                        five2.Add(cardValue2);
                    else
                        four2.Add(cardValue2);
                }
                else if (counts[counts.Count - 1] == 3 && counts[counts.Count - 2] == 2) 
                {
                    full.Add(cardValue);
                    if (jokerCount == 2)
                        five2.Add(cardValue2);
                    else if (jokerCount == 3)
                        five2.Add(cardValue2);
                    else
                        full2.Add(cardValue2);
                }
                else if (counts[counts.Count - 1] == 3 && counts[counts.Count - 2] == 1) 
                {
                    three.Add(cardValue);
                    if (jokerCount == 1)
                        four2.Add(cardValue2);
                    else if (jokerCount == 3)
                        four2.Add(cardValue2);
                    else
                        three2.Add(cardValue2);
                }
                else if (counts[counts.Count - 1] == 2 && counts[counts.Count - 2] == 2) 
                {
                    two.Add(cardValue);
                    if (jokerCount == 2)
                        four2.Add(cardValue2);
                    else if (jokerCount == 1)
                        full2.Add(cardValue2);
                    else
                        two2.Add(cardValue2);
                }
                else if (counts[counts.Count - 1] == 2 && counts[counts.Count - 2] == 1) 
                {
                    one.Add(cardValue);
                    if (jokerCount == 1)
                        three2.Add(cardValue2);
                    else if (jokerCount == 2)
                        three2.Add(cardValue2);
                    else
                        one2.Add(cardValue2);
                }
                else
                {
                    high.Add(cardValue);
                    if (jokerCount == 1)
                        one2.Add(cardValue2);
                    else
                        high2.Add(cardValue2);
                }
            }

            high.Sort();
            one.Sort();
            two.Sort();
            three.Sort();
            full.Sort();
            four.Sort();
            five.Sort();

            high2.Sort();
            one2.Sort();
            two2.Sort();
            three2.Sort();
            full2.Sort();
            four2.Sort();
            five2.Sort();

            int offset = 0;
            int offset2 = 0;

            for (int i = 0; i < high.Count; i++)
                bidMap[high[i]] *= (i + 1);
            offset += high.Count;
            for (int i = 0; i < one.Count; i++)
                bidMap[one[i]] *= (i + 1 + offset);
            offset += one.Count;
            for (int i = 0; i < two.Count; i++)
                bidMap[two[i]] *= (i + 1 + offset);
            offset += two.Count;
            for (int i = 0; i < three.Count; i++)
                bidMap[three[i]] *= (i + 1 + offset);
            offset += three.Count;
            for (int i = 0; i < full.Count; i++)
                bidMap[full[i]] *= (i + 1 + offset);
            offset += full.Count;
            for (int i = 0; i < four.Count; i++)
                bidMap[four[i]] *= (i + 1 + offset);
            offset += four.Count;
            for (int i = 0; i < five.Count; i++)
                bidMap[five[i]] *= (i + 1 + offset);

            for (int i = 0; i < high2.Count; i++)
                bidMap2[high2[i]] *= (i + 1);
            offset2 += high2.Count;
            for (int i = 0; i < one2.Count; i++)
                bidMap2[one2[i]] *= (i + 1 + offset2);
            offset2 += one2.Count;
            for (int i = 0; i < two2.Count; i++)
                bidMap2[two2[i]] *= (i + 1 + offset2);
            offset2 += two2.Count;
            for (int i = 0; i < three2.Count; i++)
                bidMap2[three2[i]] *= (i + 1 + offset2);
            offset2 += three2.Count;
            for (int i = 0; i < full2.Count; i++)
                bidMap2[full2[i]] *= (i + 1 + offset2);
            offset2 += full2.Count;
            for (int i = 0; i < four2.Count; i++)
                bidMap2[four2[i]] *= (i + 1 + offset2);
            offset2 += four2.Count;
            for (int i = 0; i < five2.Count; i++)
                bidMap2[five2[i]] *= (i + 1 + offset2);

            int partOne = bidMap.Values.Sum();
            int partTwo = bidMap2.Values.Sum();



            stopwatch.Stop();

            Console.WriteLine($"execution time\t: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine("part one\t: " + partOne); // 246409899
            Console.WriteLine("part two\t: " + partTwo); // 244848487
        }
    }
}

