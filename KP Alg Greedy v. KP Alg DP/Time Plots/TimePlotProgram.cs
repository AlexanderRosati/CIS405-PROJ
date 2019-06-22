using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using KP_Alg_Greedy_v._KP_Alg_DP;

namespace Time_Plots
{
    class TimePlotProgram
    {
        static void Main(string[] args)
        {
            //Vars
            long worstCaseTime = 0;
            long avgCaseTime = 0;
            long bestCaseTime = 0;
            long capAvg = 0;
            int numItems;
            int numberOfItemsIncluded = 0;

            double percentOfItemsIncluded = 0.0;
            Stopwatch sw = new Stopwatch();
            Tuple<int[], int> solution;

            //Prompt user for number of problems
            Console.Write("How many problems? ");
            numItems = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Generating Items.");

            //Generate items
            List<string> linesOfFile = new List<string>(File.ReadAllLines("..//Problems//PROBLEM_SIZE_5.txt"));
            Tuple<int[], int[]> problem = ItemPicker.PickItems(linesOfFile, numItems);

            Console.WriteLine("Done Generating Items. Doing WC Time.");

            //Get Worst Case Time
            //Note: Cap = 8_000_000_000 so everything will fit in knapsack
            sw.Start();
            KPGreedyAlg.Solve(problem.Item2, problem.Item1, 8_000_000_000, numItems);
            sw.Stop();

            worstCaseTime = sw.ElapsedMilliseconds;
            sw.Reset();

            Console.WriteLine("Done. Doing BC Time.");

            //Get Best Case Time
            //Note: Cap = 0 so nothing will fit in knapsack
            sw.Start();
            KPGreedyAlg.Solve(problem.Item2, problem.Item1, 0, numItems);
            sw.Stop();

            bestCaseTime = sw.ElapsedMilliseconds;
            sw.Reset();

            Console.WriteLine("Done. Doing AVG Case time.");

            //Get Average Case Time
            capAvg = numItems * 25;

            sw.Start();
            KPGreedyAlg.Solve(problem.Item2, problem.Item1, capAvg, numItems);
            sw.Stop();

            avgCaseTime = sw.ElapsedMilliseconds;

            Console.WriteLine("Done. Wrapping up.");

            //Calculate percentage of items included for average case
            solution = KPGreedyAlg.Solve(problem.Item2, problem.Item1, capAvg, numItems);

            for (int i = 0; i < solution.Item1.Length; i++)
            {
                if (solution.Item1[i] == 1)
                {
                    ++numberOfItemsIncluded;
                }
            }

            percentOfItemsIncluded = (Convert.ToDouble(numberOfItemsIncluded)) / numItems;

            //Display results on screen
            Console.WriteLine($"Number of Items: {numItems}");
            Console.WriteLine($"Worst Case Time: {worstCaseTime}");
            Console.WriteLine($"Average Case Time: {avgCaseTime}");
            Console.WriteLine($"Best Case Time: {bestCaseTime}");
            Console.WriteLine($"Average Capacity: {capAvg}");
            Console.WriteLine($"Percent Included: {percentOfItemsIncluded}");
            Console.WriteLine($"Number of Items Included: {numberOfItemsIncluded}");
            Console.ReadLine();
        }
    }
}