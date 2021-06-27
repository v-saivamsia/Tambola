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
                    Tickets[i].TicketSet[rowPointer] ^= 1 << j;
                    Tickets[i].AvailableIndices[rowPointer].Remove(j);
                    nums[j].RemoveAt(nums[j].Count - 1);
                    count++;
                }
            }

            // Add the remaining 36 numbers to the tickets
            addTwoNums(nums, numsSet);


            // Since there are many possible combinations backtrack for the solution
            // not implemented yet
            printArray();

        }

        // add the remaining two numbers in each row
        private void addTwoNums(List<List<int>> nums, List<int> numsSet)
        {
            int num = (1 << 9) - 1;
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int availableBits = num & Tickets[i].TicketSet[j];
                    List<int> availableIndices = new List<int>();
                    while (availableBits > 0)
                    {
                        int lsb = availableBits & -availableBits;
                        int count = 0;
                        while (lsb != 1)
                        {
                            count++;
                            lsb = lsb >> 1;
                        }
                        availableIndices.Add(count);
                        availableBits -= availableBits & -availableBits;
                    }
                    specialShuffle(nums,availableIndices);
                    int a = availableIndices[0], b = availableIndices[1];
                    Tickets[i].TicketArray[j, a] = nums[a][nums[a].Count - 1];
                    Tickets[i].TicketArray[j, b] = nums[b][nums[b].Count - 1];
                    nums[a].RemoveAt(nums[a].Count - 1);
                    if (nums[a].Count == 0) num ^= 1 << a;
                    nums[b].RemoveAt(nums[b].Count - 1);
                    if (nums[b].Count == 0) num ^= 1 << b;

                }
            }
        }
        private void shuffleList(List<int> list)
        {
            shuffleListWithIndex(list,0, list.Count-1); 
        }
        private void shuffleListWithIndex(List<int> list, int a, int b)
        {
            Random random = new Random();
            for (int i = b; i > a; i--)
            {
                int temp = random.Next(0, i);
                int swap = list[i];
                list[i] = list[temp];
                list[temp] = swap;
            }

            for (int i = b; i > a; i--)
            {
                int temp = random.Next(0, i);
                int swap = list[i];
                list[i] = list[temp];
                list[temp] = swap;
            }
        }
        private void specialShuffle(List<List<int>> nums, List<int> list)
        {
            list.Sort((a, b) => nums[b].Count - nums[a].Count);
            int i=0,j=0;
            for(; i< list.Count; i++)
            {
                if (nums[i].Count == nums[j].Count) continue;
                shuffleListWithIndex(list, j, i - 1);
                j = i;
            }
            shuffleListWithIndex(list, j, i - 1);

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
