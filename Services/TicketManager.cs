using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tambola.Models;

namespace Tambola.Services
{
    public class TicketManager
    {
        private readonly ILogger<TicketManager> logger;

        public Ticket[] Tickets { get; }
        public TicketManager(ILogger<TicketManager> logger)
        {
            Tickets = new Ticket[6];
            for (int i = 0; i < 6; i++)
                Tickets[i] = new Ticket();
            this.logger = logger;
            generateTickets();
        }
        public void generateTickets()
        {
            // List of all 90 numbers
            List<List<int>> nums = new List<List<int>>();

            // List of index containing non empty integers in nums
            List<int> numsSet = new List<int>();

            // Initializing nums and numsSet and shuffle the nums
            for (int i = 0; i < 9; i++)
            {
                numsSet.Add(i);
                nums.Add(new List<int>());

                // initialize with values
                for (int j = 0; j < 10; j++) nums[i].Add(10 * i + j);

                // shuffle the list
                shuffleList(nums[i]);
            }

            // Fill all tickets with atleast one number in each column
            // i.e fill each row with 3 numbers and all rows are disjoint
            List<int> rowIndices = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            for (int i = 0; i < 6; i++)
            {
                shuffleList(rowIndices);
                int rowPointer = -1, count = 0;
                foreach (int j in rowIndices)
                {
                    if (count % 3 == 0) rowPointer++;
                    int fill = nums[j][nums[j].Count - 1];
                    Tickets[i].TicketArray[rowPointer, j] = fill;
                    Tickets[i].TicketSet[rowPointer] |= 1 << j;
                    Tickets[i].AvailableIndices[rowPointer].Remove(j);
                    nums[j].RemoveAt(nums[j].Count - 1);
                    count++;
                }
            }
            bool isPossible = false;

            // Add the remaining 36 numbers to the tickets
            // Since there are many possible combinations backtrack for the solution


        }

        // backtrack for the solution
        private bool backTrack(int ticketNumber, List<List<int>> nums, List<int> numsSet)
        {
            Random random = new Random();
            bool isValidNumbers = false;
            while (!isValidNumbers)
            {
                for (int i = 0; i < 3; i++)
                {

                }
            }

            return false;
        }
        // shuffle array in O(n) time
        private void shuffleList(List<int> list)
        {
            Random random = new Random();

            for (int i = list.Count - 1; i > 0; i--)
            {
                int temp = random.Next(0, i);
                int swap = list[i];
                list[i] = list[temp];
                list[temp] = swap;
            }
        }
        private void printArray()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        Console.Write($"{Tickets[i].TicketArray[j, k]} ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine();
            }

        }
    }
}
