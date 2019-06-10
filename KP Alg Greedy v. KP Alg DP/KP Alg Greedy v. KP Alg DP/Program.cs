using System;
using System.Collections.Generic;
using System.IO;

namespace KP_Alg_Greedy_v._KP_Alg_DP
{
    //Description: Generates a file of random numbers for use in testing.
    public static class FileGenerator
    {
        //Description: Generates a file with the given number of items. The file is named fileName
        //and is created in the solution folder. The format of the file is weight,value. Each
        //line is an item. The value of an item is at most 1.5 * its weight and at least
        //0.5 times its weight.
        //Input: Maximum capacity, number of items to generate, file name
        //Output: File of items.
        public static void Generate(int maxCap, int numItems, string fileName)
        {
            string newFileText = String.Empty;
            Random RNG = new Random();
            string dir = "C:\\Users\\Fuck You Microsoft\\Documents\\GitHub\\CIS405-PROJ\\" +
                         "KP Alg Greedy v. KP Alg DP\\KP Alg Greedy v. KP Alg DP\\" + fileName;

            for (int i = 0; i < numItems; ++i)
            {
                int w = RNG.Next(1, maxCap + 1);
                double multiplier = (RNG.Next(1, 101) / 100.0) + 0.5;
                int v = Convert.ToInt16(Math.Ceiling(multiplier * w));
                newFileText += $"{w},{v}\n";
            }

            File.WriteAllText(dir, newFileText);
        }
    }

    public static class KPGreedyAlg
    {
        //Description: Represents an item. Only useful in this class. itemIndex points to V and W.
        private class Item
        {
            public double ratio; //Item Value / Item Weight
            public int itemIndex; //Points to object in original arrays
            public Item(double r, int i) { ratio = r; itemIndex = i; }
        }

        //Description: Merges two sorted item arrays into one sorted item array.
        //Helper method for SortItems().
        private static void Merge(Item[] items, Item[] sub1, Item[] sub2)
        {
            int i = 0;
            int j = 0;
            int k = 0;

            while (i < sub1.Length && j < sub2.Length)
            {
                if (sub1[i].ratio > sub2[j].ratio)
                {
                    items[k] = sub1[i];
                    ++i;
                }

                else
                {
                    items[k] = sub2[j];
                    ++j;
                }

                ++k;
            }

            while (i < sub1.Length)
            {
                items[k] = sub1[i];
                ++i;
                ++k;
            }

            while(j < sub2.Length)
            {
                items[k] = sub2[j];
                ++j;
                ++k;
            }
        }

        //Description: Sorts array of items. Variation of merge sort.
        private static void SortItems(Item[] items)
        {
            if (items.Length > 1)
            {
                //Split array into two arrays
                int m = items.Length / 2;
                Item[] sub1 = new Item[items.Length - m];
                Item[] sub2 = new Item[m];

                //Copy over values
                for(int i = 0; i < m; ++i)
                {
                    sub2[i] = items[i];
                }

                for(int i = m; i < items.Length; ++i)
                {
                    sub1[i - m] = items[i];
                }

                SortItems(sub1);
                SortItems(sub2);

                Merge(items, sub1, sub2);
            }
        }

        //Description: Approximates solution to KP using a greedy approach.
        //Input: V, values of items, W, weights of items, Cap, capacity of knapsack, N, # of items
        //Output: An array that tells you which items where put into the knapsack and the value of
        //the items in the knapsack.
        //Pre-condition: V and W must have the same number of elements. N must equal the number of
        //elements in V and W. Capacity must be nonnegative. V and W must contain positive integers.
        public static Tuple<int[], int> Solve(int[] V, int[] W, int Cap, int N)
        {
            int remainingCapacity = Cap; //How much space is left in knapsack
            int valueInKnapsack = 0; //Current value in knapsack

            Item[] items = new Item[N];
            int[] itemsInKnapsack = new int[N]; //Tells you which items the alg put in knapsack

            //Create N Item Objects
            for(int i = 0; i < N; ++i)
            {
                items[i] = new Item(Convert.ToDouble(V[i]) / W[i], i);
            }

            //Sort items by ratio = value of item / weight of item
            SortItems(items);

            //Iterate through all items
            for (int i = 0; i < N; ++i)
            {
                //If item fits in knapsack
                if (W[items[i].itemIndex] < remainingCapacity)
                {
                    remainingCapacity -= W[items[i].itemIndex];
                    valueInKnapsack += V[items[i].itemIndex];
                    itemsInKnapsack[items[i].itemIndex] = 1;
                }

                //If items doesn't fit in knapsack
                else
                {
                    itemsInKnapsack[items[i].itemIndex] = 0;
                }
            }

            return new Tuple<int[], int>(itemsInKnapsack, valueInKnapsack);
        }
    }

    //Description: For testing purposes only. Serves no other purpose.
    public static class TestingClass
    {
        static string dir = "C:\\Users\\Fuck You Microsoft\\Documents\\GitHub\\CIS405-PROJ\\" +
                         "KP Alg Greedy v. KP Alg DP\\KP Alg Greedy v. KP Alg DP\\PROBLEM_SIZE_5.txt";

        public static void TestDPAlg()
        {
            int[] V = null;
            int[] W = null;
            Tuple<int[], int[]> T1 = null;
            Tuple<int[], int> T2 = null;
            

            for (int i = 0; i < 1000; ++i)
            {
                T1 = ItemPicker.PickItems(dir, 500);
                W = T1.Item1;
                V = T1.Item2;
                T2 = KPDynamicProgAlg.Solve(V, W, 5000, 500);
                Console.Write($"Maximum Value: {T2.Item2}");
                Console.WriteLine();
            }

            Console.ReadLine();
        }

        public static void TestItemPicker()
        {
            while(true)
            {
                var tuple = ItemPicker.PickItems(dir, 10);
                Console.Write("W: ");
                for (int i = 0; i < 10; i++) Console.Write($" {tuple.Item1[i]}");
                Console.WriteLine();
                Console.Write("V: ");
                for (int i = 0; i < 10; i++) Console.Write($" {tuple.Item2[i]}");
                Console.WriteLine();
                Console.WriteLine();
                Console.ReadLine();
            }
        }
    }

    //Description: Randomly selected the given number of items from the given file.
    public static class ItemPicker
    {
        //Description: Randomly selected the given number of items from the given file.
        //Input: File with items and number of items to select.
        //Output: Subset of items in file.
        //Precondition: File must follow the proper format.
        public static Tuple<int[], int[]> PickItems(string dirOfItems, int numItems)
        {
            string tmpString = String.Empty;
            string[] tmpStringArr = null;
            int[] weights = new int[numItems];
            int[] values = new int[numItems];

            Random RNG = new Random();

            //Turn input file into a list of strings
            List<string> linesOfFile = new List<string>(File.ReadAllLines(dirOfItems));
            
            //Shuffle list of strings
            for (int i = 0; i < linesOfFile.Count; ++i)
            {
                int randomIndx = RNG.Next(0, linesOfFile.Count);
                tmpString = linesOfFile[i];
                linesOfFile[i] = linesOfFile[randomIndx];
                linesOfFile[randomIndx] = tmpString;
            }

            //Use first ten elements in string list as items.
            for (int i = 0; i < numItems; ++i)
            {
                tmpStringArr = linesOfFile[i].Split(",");
                weights[i] = Convert.ToInt16(tmpStringArr[0]);
                values[i] = Convert.ToInt16(tmpStringArr[1]);
            }

            return new Tuple<int[], int[]>(weights, values);
        }
    }

    //Description: Solves 0/1 Knapsack Problem using Dynamic Programming
    public static class KPDynamicProgAlg
    {
        //To store solutions to overlapping sub-problems
        static int[,] Table;
        static int[] ItemsIncluded;

        //Description: Solves 0/1 Knapsack Problem
        //Input: Weights, W[], Values, V[], Capacity, Cap, Number of Items, N.
        //Output: Items included in Knapsack and Maximum Value. A value of 1 means the item
        //was included and a value of 0 means it was not included.
        //Post-condition: The optimal value will be returned.
        //Pre-condition: V and W must have the same number of elements. N must equal the number of
        //elements in V and W. Capacity must be nonnegative. V and W must contain positive integers.
        public static Tuple<int[], int> Solve(int[] V, int[] W, int Cap, int N)
        {
            //Initialize arrays
            Table = new int[Cap + 1, N + 1];
            ItemsIncluded = new int[N];

            //Initialize table
            for (int i = 0; i < N + 1; ++i)
            {
                Table[0, i] = 0;
            }

            for (int i = 1; i < Cap + 1; ++i)
            {
                Table[i, 0] = 0;
            }

            //Iterate through items and fill out table
            for (int j = 1; j < N + 1; j++)
            {
                //Iterate through capacities
                for ( int w = 1; w < Cap + 1; w++)
                {
                    if (W[j - 1] > w)
                    {
                        Table[w, j] = Table[w, j - 1];
                    }

                    else
                    {
                        Table[w, j] = Math.Max(Table[w, j - 1], Table[w - W[j - 1], j - 1] + V[j - 1]);
                    }
                }
            }

            //Initialize variable to keep track of capicity
            int currW = Cap;

            //Look at table to determine which items where included
            for (int j = N; j > 0; j--)
            {
                if ((currW - W[j - 1] < 0) || (Table[currW, j - 1] > Table[currW - W[j - 1], j - 1] + V[j - 1]))
                {
                    ItemsIncluded[j - 1] = 0;
                }

                else
                {
                    ItemsIncluded[j - 1] = 1;
                    currW -= W[j - 1];
                }
            }

            return new Tuple<int[], int>(ItemsIncluded, Table[Cap, N]);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string dir = "C:\\Users\\Fuck You Microsoft\\Documents\\GitHub\\CIS405-PROJ\\" +
                         "KP Alg Greedy v. KP Alg DP\\KP Alg Greedy v. KP Alg DP\\PROBLEM_SIZE_1.txt";

            Tuple<int[], int[]> t1;
            Tuple<int[], int> t2;
            Tuple<int[], int> t3;
            int runningTotal = 0;

            for (int i = 0; i < 100000; ++i)
            {
                t1 = ItemPicker.PickItems(dir, 10);
                t2 = KPGreedyAlg.Solve(t1.Item1, t1.Item2, 100, 10);
                t3 = KPDynamicProgAlg.Solve(t1.Item1, t1.Item2, 100, 10);
                runningTotal += (t3.Item2 - t2.Item2);
            }

            Console.WriteLine(runningTotal / 100_000.0);
            Console.ReadLine();
        }
    }
}