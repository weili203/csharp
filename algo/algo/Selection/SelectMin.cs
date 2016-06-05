/*
 * ----currently broken----
 * 
 * The algo select the i-th smallest number in A[]
 * 
 * ASSUMPTIOIN:
 * the numbers in the set are distinct
 * 
 * As in quicksort, we partition the input array
   recursively. But unlike quicksort, which recursively processes both sides of the
   partition, RANDOMIZED-SELECT works on only one side of the partition. This
   difference shows up in the analysis: whereas quicksort has an expected running
   time of O(nlgn), the expected running time of RANDOMIZED-SELECT is O(n), assuming that the elements are distinct.
 * 
 * worst case O(nlgn), expected running time O(n)
 * 
 * RANDOMIZED-SELECT (A, p, r, i)
     if p == r
        return A[p]
     q = RANDOMIZED-PARTITION(A, p, r)
     k = q - p + 1
     if i == k // the pivot value is the answer
        return A[q]
     elseif i < k
        return RANDOMIZED-SELECT(A, p, q - 1, i )
     else 
        return RANDOMIZED-SELECT(A, q + 1, r, i - k)
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace algo.Selection
{
    class SelectNthMin
    {
        public static void Try_SelectNSmallest()
        {
            SelectNthMin sm = new SelectNthMin();
            List<int> A = new List<int>() { 1, 2, 3, 4, 5, 6, 7};
            Console.WriteLine("The array is:");
            foreach (int i in A)
                Console.Write(i.ToString() + " ");
            
            Console.WriteLine("");

            int order = 3;
            Console.WriteLine("The {0}th smallest number is {1}", order.ToString(), sm.SelectNSmallest(A, order).ToString());
            
 
        }

        // select the ith order, which is the ith smallest value
        public int SelectNSmallest(List<int> A, int i)
        {

            return Recursive_RandomnizedSelect(A, 0, A.Count - 1, i);
        }

        private int Recursive_RandomnizedSelect(List<int> A, int low, int high, int i)
        {
            if (low == high)
                return A[low];

            int q = RandomnizedPartition(A, low, high);   // q is index
            int k = q - low + 1;    // k is the k-th number in A[]

            if (i == k)
                return A[q];
            else if (i < k)
                return Recursive_RandomnizedSelect(A, low, q -1, i);
            else
                return Recursive_RandomnizedSelect(A, q + 1, high, i - k);

        }

        private int RandomnizedPartition(List<int> A, int low, int high)
        {
            // pick a random pivot to achieve average-case
            Random rand = new Random();
            int n = rand.Next(high);
            // let the random position be the last one, which will be used as pivot in Partition
            Swap(A, high, n);

            return Partition(A, low, high);
        }


        private int Partition(List<int> A, int low, int high)
        {
            int x = A[high];
            int i = low - 1;

            for (int j = low; j <= high - 1; ++j)
            {
                if (A[j] <= x)
                {
                    i += 1;
                    Swap(A, i, j);
                }
            }

            Swap(A, i + 1, high);
            return i + 1;
        }

        protected void Swap(List<int> A, int a, int b)
        {
            int temp = A[a];
            A[a] = A[b];
            A[b] = temp;
        }
    }
}
