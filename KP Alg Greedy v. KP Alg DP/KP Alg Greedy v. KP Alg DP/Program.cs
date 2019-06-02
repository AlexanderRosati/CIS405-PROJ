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
                if (Table[currW, j - 1] > Table[currW - W[j - 1], j - 1] + V[j - 1])
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
            ItemPicker.PickItems(dir, 10);
        }
    }
}