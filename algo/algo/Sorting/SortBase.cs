/*
 * comparison based sorting algo - has lower bound of nlgn for comparison
 * 
 * linear time sorting algo, non-comparison used, no nlgn lwerbound
 * 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace algo
{
    abstract class SortBase
    {
        protected List<int> _array = new List<int>() {5, 2, 4, 6, 1, 5, 3 } ;
        //protected List<int> _array = new List<int>() { 5, 2, 4, 6, 1, 5, 3, 6, 1, 2, 5, 8, 90, 34, 45, 67, 32, 1, 45, 67, 65, 32, 86, 87, 53, 67, 76, 52, 78, 63, 48, 37 };

        public void DoSort()
        {
            // Create new stopwatch
            Stopwatch stopwatch = new Stopwatch();

            // Begin timing
            stopwatch.Start();

            Sort();

            // Stop timing
            stopwatch.Stop();

            // Write result
            Console.WriteLine("{0}: {1}", this.GetType().ToString(),stopwatch.Elapsed);
        }

        public void Print(string comments)
        {
            if (String.IsNullOrEmpty(comments) == false)
                Console.WriteLine(comments);

            if (_array.Count > 0)
            {
                foreach (int i in _array)
                    Console.WriteLine(i.ToString());
            }
        }

        protected abstract void Sort();

        protected void Print(string comments, List<int> array)
        {
            if (String.IsNullOrEmpty(comments) == false)
                Console.WriteLine(comments);

            foreach (int i in array)
                Console.WriteLine(i.ToString());
        }

        protected void Swap(int a, int b)
        {
            int temp = _array[a];
            _array[a] = _array[b];
            _array[b] = temp;
        }

        protected void Swap(List<int> A, int a, int b)
        {
            int temp = A[a];
            A[a] = A[b];
            A[b] = temp;
        }
    }
}
