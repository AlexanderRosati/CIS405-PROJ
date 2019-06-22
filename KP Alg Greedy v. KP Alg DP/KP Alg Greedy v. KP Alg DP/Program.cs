using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Threading;

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
        public static Tuple<int[], int> Solve(int[] V, int[] W, long Cap, int N)
        {
            long remainingCapacity = Cap; //How much space is left in knapsack
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
                if (W[items[i].itemIndex] <= remainingCapacity)
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

    //Description: Randomly selectes the given number of items from the given file.
    public static class ItemPicker
    {
        //Description: Randomly selectes the given number of items from the given file.
        //Input: File with items and number of items to select.
        //Output: Subset of items in file.
        //Precondition: File must follow the proper format.
        public static Tuple<int[], int[]> PickItems(List<string> linesOfFile, int numItems)
        {
            string line = null;
            string[] weightAndValue = null;
            int[] weights = new int[numItems];
            int[] values = new int[numItems];

            Random RNG = new Random();
            
            //Randomly select the given number of lines from the file.
            for (int i = 0; i < numItems; ++i)
            {
                line = linesOfFile[RNG.Next(linesOfFile.Count)];
                weightAndValue = line.Split(",");
                weights[i] = Convert.ToInt32(weightAndValue[0]);
                values[i] = Convert.ToInt32(weightAndValue[1]);
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
            //Vars
            string fileToPullItemsFrom = null;
            string csv1Name = null;
            string csv2Name = null;
            string csv1 = null;
            string csv2 = "";
            string values = "";
            string weights = "";
            string itemsThatGreedyIncluded = "";
            string itemsThatDPIncluded = "";
            string record = null;

            int errorOfGreedy = 0;
            int numberOfProblems = 0;
            int numberOfItems = 0;
            int capacity = 0;
            int sleepCounter = 0;

            long timeForGreedySolvingProblems = 0;
            long timeForDPSolvingProblems = 0;

            Tuple<int[], int[]>[] Problems = null;
            Tuple<int[], int>[] GreedySolutions = null;
            Tuple<int[], int>[] DynamicProgSolutions = null;

            Stopwatch sw = new Stopwatch();

            //Prompt user for various things.
            Console.Write("File to pull items from: ");
            fileToPullItemsFrom = Console.ReadLine();

            Console.Write("Name of first output file: ");
            csv1Name = Console.ReadLine();

            Console.Write("Name of second output file: ");
            csv2Name = Console.ReadLine();

            Console.Write("Number of problems to solve: ");
            numberOfProblems = Convert.ToInt32(Console.ReadLine());
            GreedySolutions = new Tuple<int[], int>[numberOfProblems];
            DynamicProgSolutions = new Tuple<int[], int>[numberOfProblems];

            Console.Write("Number of items: ");
            numberOfItems = Convert.ToInt32(Console.ReadLine());

            Console.Write("Capacity: ");
            capacity = Convert.ToInt32(Console.ReadLine());

            //Create array for problems
            Problems = new Tuple<int[], int[]>[numberOfProblems];

            //Turn file into list
            List<string> linesOfFile = new List<string>(File.ReadAllLines(fileToPullItemsFrom));

            Console.WriteLine("Generating problems.");

            //Generate Problems
            for(int i = 0; i < numberOfProblems; ++i)
            {
                if (sleepCounter == 1000)
                {
                    sleepCounter = 0;
                    --i;
                    Console.WriteLine($"{i + 1} problems generated.");
                    Thread.Sleep(1000); //Im pauing here to try and make the item selection more random.
                }
                
                else
                {
                    Problems[i] = ItemPicker.PickItems(linesOfFile, numberOfItems);
                    ++sleepCounter;
                }
            }

            Console.WriteLine("Done generating problems.");
            Console.WriteLine("Greedy is solving.");

            //Timing the greedy algorithm solving all the problems.
            sw.Start();
            for (int i = 0; i < numberOfProblems; ++i)
            {              
                GreedySolutions[i] = KPGreedyAlg.Solve(Problems[i].Item2, Problems[i].Item1, capacity, numberOfItems);
            }
            sw.Stop();

            timeForGreedySolvingProblems = sw.ElapsedMilliseconds;

            Console.WriteLine($"Greedy is done solving. Took {timeForGreedySolvingProblems}ms.");
            Console.WriteLine("Dynamic Programming is solving.");

            sw.Reset();

            //Timing dynamic programming solving all the problems.
            sw.Start();
            for (int i = 0; i < numberOfProblems; ++i)
            {
                DynamicProgSolutions[i] = KPDynamicProgAlg.Solve(Problems[i].Item2, Problems[i].Item1, capacity, numberOfItems);
            }
            sw.Stop();

            timeForDPSolvingProblems = sw.ElapsedMilliseconds;
            Console.WriteLine($"Dynamic Programming is done solving. Took {timeForDPSolvingProblems}ms.");
            Console.WriteLine("Creating first CSV.");

            //Generating first CSV
            csv1 = "Statistic,Value\n";
            csv1 += $"\"Average Runtime of the Greedy Algorithm\",{timeForGreedySolvingProblems / Convert.ToDouble(numberOfProblems)}\n";
            csv1 += $"\"Average Runtime of the DP Algorithm\",{timeForDPSolvingProblems / Convert.ToDouble(numberOfProblems)}\n";
            csv1 += $"\"Total time for Greedy\",{timeForGreedySolvingProblems}\n";
            csv1 += $"\"Total time for DP\",{timeForDPSolvingProblems}\n";
            csv1 += $"\"Capacity\",{capacity}\n";
            csv1 += $"\"Number of Problems\",{numberOfProblems}\n";
            csv1 += $"\"Number of Items\",{numberOfItems}\n";

            File.WriteAllText(csv1Name, csv1);
            Console.WriteLine("Done creating first CSV.");
            Console.WriteLine("Creating second CSV.");

            //Generating second CSV
            for (int i = 0; i < numberOfProblems; ++i)
            {
                //Set various things to empty string
                record = "";
                values = "";
                weights = "";
                itemsThatDPIncluded = "";
                itemsThatGreedyIncluded = "";

                //Turn value array into string
                for (int j = 0; j < numberOfItems - 1; ++j) values += $"{Problems[i].Item2[j]},";
                values += $"{Problems[i].Item2[numberOfItems - 1]}";

                //Turn weight array into string
                for (int j = 0; j < numberOfItems - 1; ++j) weights += $"{Problems[i].Item1[j]},";
                weights += $"{Problems[i].Item1[numberOfItems - 1]}";

                //Turning greedy solution into a string
                for (int j = 0; j < numberOfItems; ++j)
                {
                    itemsThatGreedyIncluded += $"{GreedySolutions[i].Item1[j]}";
                }

                //Turning DP solution into a string
                for (int j = 0; j < numberOfItems; ++j)
                {
                    itemsThatDPIncluded += $"{DynamicProgSolutions[i].Item1[j]}";
                }

                //Calculate error of Greedy
                errorOfGreedy = DynamicProgSolutions[i].Item2 - GreedySolutions[i].Item2;

                //Create record
                record += $"\"{values}\",";
                record += $"\"{weights}\",";
                record += $"{numberOfItems},";
                record += $"{capacity},";
                record += $"{GreedySolutions[i].Item2},";
                record += $"{DynamicProgSolutions[i].Item2},";
                record += $"\"{itemsThatGreedyIncluded}\",";
                record += $"\"{itemsThatDPIncluded}\",";
                record += $"{errorOfGreedy}\n";

                //Add record to beggining of file
                csv2 = csv2.Insert(0, record);

                //So the user knows how much of the file has been generated
                if ((i + 1) % 1000 == 0)
                {
                    Console.WriteLine($"{i + 1} records have been generated.");
                }
            }

            //Input header at begining of file
            csv2 = csv2.Insert(0, "Values,Weights,Number of Items, Capacity,Value that Greedy Put in Bag," +
                   "Value that DP Put in Bag,Greedy Solution,DP Solution,Error of Greedy\n");

            Console.WriteLine("Writing file.");
            File.WriteAllText(csv2Name, csv2);

            Console.WriteLine("Done generating second CSV.");
            Console.Write("Press enter to coninue . . .");
            Console.ReadLine();
        }
    }
}