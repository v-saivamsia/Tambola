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

        public Ticket[] Tickets { get; }
        public TicketManager()
        {
            Tickets = new Ticket[6];
            for (int i = 0; i < 6; i++)
                Tickets[i] = new Ticket();
            bool isValidTickets = false;
            int count = 1;
            while (!isValidTickets)
            {
                try
                {
                    generateTickets();
                    isValidTickets = true;

                    // flip all the ticket rows
                    for (int i = 0; i < 6; i++)
                        for (int j = 0; j < 3; j++)
                            flipTicketRow(i, j);

                    // permute all the ticket rows
                    permuteTickets();

                }
                catch (Exception ex)
                {
                    Tickets = new Ticket[6];
                    for (int i = 0; i < 6; i++)
                        Tickets[i] = new Ticket();

                    Console.WriteLine($"Failed at attempt: {count} and the exception is:\n {ex}");
                    count++;
                }
                finally
                {
                    Console.WriteLine($"Number of attempts: {count}");
                }
            }
        }
        public TicketManager(PlayerTicket playerTicket)
        {
            Tickets = new Ticket[6];
            for (int i = 0; i < 6; i++)
                Tickets[i] = new Ticket(playerTicket.tickets[i]); ;
        }
        public static implicit operator List<string>(TicketManager ticketManager)
        {
            List<string> result = new List<string>();
            for (int i = 0; i < 6; i++) result.Add(ticketManager.Tickets[i]);
            return result;
        }

        private void generateTickets()
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
            // add the 9 numbers to all tickets
            addNineNums(nums, numsSet);

            // Add the remaining 36 numbers to the tickets
            addTwoNums(nums, numsSet);
        }

        // add 9 numbers in each ticket with each row containing 3 numbers
        private void addNineNums(List<List<int>> nums, List<int> numsSet)
        {
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
                    nums[j].RemoveAt(nums[j].Count - 1);
                    count++;
                }
            }

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
                    List<int> availableIndices = getAvailableIndices(availableBits);

                    specialShuffle(nums, availableIndices);
                    int a = availableIndices[0], b = availableIndices[1];

                    Tickets[i].TicketArray[j, a] = nums[a][nums[a].Count - 1];
                    Tickets[i].TicketSet[j] ^= 1 << a;

                    Tickets[i].TicketArray[j, b] = nums[b][nums[b].Count - 1];
                    Tickets[i].TicketSet[j] ^= 1 << b;

                    nums[a].RemoveAt(nums[a].Count - 1);
                    if (nums[a].Count == 0) num ^= 1 << a;
                    nums[b].RemoveAt(nums[b].Count - 1);
                    if (nums[b].Count == 0) num ^= 1 << b;

                }
            }
        }
        private void permuteTickets()
        {
            for (int i = 0; i < 6; i++)
            {
                premuteTicket(i);
            }
        }

        private void premuteTicket(int ticketNumber)
        {
            bool isPermutedWell = false;
            int count = 0;
            while (!isPermutedWell && count < 5)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (checkContinuity(ticketNumber, i % 3))
                    {
                        shuffleTicket(ticketNumber, i % 3);
                    }
                }
                isPermutedWell = !(checkContinuity(ticketNumber, 0) || checkContinuity(ticketNumber, 1) || checkContinuity(ticketNumber, 2));
                count++;
            }

        }
        private void shuffleTicket(int ticketNumber, int row)
        {
            if (!swapWithRow(ticketNumber, row, (row + 1) % 3))
                swapWithRow(ticketNumber, row, (row + 2) % 3);
        }
        private bool swapWithRow(int ticketNumber, int row, int swapRow)
        {
            int possibleSwapsinBoth = Tickets[ticketNumber].TicketSet[row] ^ Tickets[ticketNumber].TicketSet[swapRow];
            int possibleSwapsinOne = possibleSwapsinBoth & Tickets[ticketNumber].TicketSet[row];
            int possibleSwapsinTwo = possibleSwapsinBoth & Tickets[ticketNumber].TicketSet[swapRow];

            List<int> availableSwapsinOne = getAvailableIndices(getOnlyContinuousIndices(possibleSwapsinOne, ticketNumber, row));
            List<int> availableSwapsinTwo = getAvailableIndices(possibleSwapsinTwo);

            shuffleList(availableSwapsinOne);
            shuffleList(availableSwapsinTwo);
            bool isPermuted = false;
            for (int index = 0; availableSwapsinOne.Count > 0 && availableSwapsinTwo.Count > 0 && !isPermuted && index < availableSwapsinTwo.Count; index++)
            {
                swap(ticketNumber, row, swapRow, availableSwapsinOne[0], availableSwapsinTwo[index]);
                if (!checkContinuity(ticketNumber, swapRow))
                {
                    isPermuted = true;
                    break;
                }
                swap(ticketNumber, swapRow, row, availableSwapsinOne[0], availableSwapsinTwo[index]);
            }

            return isPermuted;
        }
        private void swap(int ticketNumber, int a, int b, int cola, int colb)
        {
            int tempa = (int)Tickets[ticketNumber].TicketArray[a, cola];
            int tempb = (int)Tickets[ticketNumber].TicketArray[b, colb];

            Tickets[ticketNumber].TicketArray[a, cola] = null;
            Tickets[ticketNumber].TicketSet[a] ^= 1 << cola;

            Tickets[ticketNumber].TicketArray[b, cola] = tempa;
            Tickets[ticketNumber].TicketSet[b] ^= 1 << cola;


            Tickets[ticketNumber].TicketArray[b, colb] = null;
            Tickets[ticketNumber].TicketSet[b] ^= 1 << colb;


            Tickets[ticketNumber].TicketArray[a, colb] = tempb;
            Tickets[ticketNumber].TicketSet[a] ^= 1 << colb;
        }
        private void flipTicketRow(int ticketNumber, int row)
        {
            int mask = (1 << 9) - 1;
            Tickets[ticketNumber].TicketSet[row] = (~Tickets[ticketNumber].TicketSet[row]) & mask;
        }
        private int getOnlyContinuousIndices(int possibleSwaps, int ticketNumber, int row)
        {
            int temp = possibleSwaps, lsb = 0;
            while (temp > 0)
            {
                lsb = temp & -temp;
                if ((possibleSwaps & (lsb >> 1)) == 0 && (possibleSwaps & (lsb << 1)) == 0)
                    possibleSwaps -= lsb;
                temp -= lsb;
            }
            return possibleSwaps;
        }
        private bool checkContinuity(int ticketNumber, int row)
        {
            int maxCount = 0;
            for (int i = 0, count = 0; i < 9; i++)
            {
                if (!Tickets[ticketNumber].TicketArray[row, i].HasValue)
                {
                    count = 0;
                    continue;
                }
                count++;
                maxCount = count > maxCount ? count : maxCount;
            }
            return maxCount >= 4;
        }

        private List<int> getAvailableIndices(int availableBits)
        {
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
            return availableIndices;
        }
        private void shuffleList(List<int> list)
        {
            shuffleListWithIndex(list, 0, list.Count - 1);
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
            int i = 0, j = 0;
            for (; i < list.Count; i++)
            {
                if (nums[i].Count == nums[j].Count) continue;
                shuffleListWithIndex(list, j, i - 1);
                j = i;
            }
            shuffleListWithIndex(list, j, i - 1);

        }
    }
}
