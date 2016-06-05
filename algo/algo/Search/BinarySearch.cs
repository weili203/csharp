using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace algo
{
    class BinarySearch
    {
        // a is an sorted array
        public static int BinSearch(int[] a, int x)
        {
            int low = 0, high = a.Length - 1 , mid;

            while (low <= high)
            {
                mid = (low + high) / 2;

                if (a[mid] == x)
                    return mid;
                else if (a[mid] < x)
                    low = mid + 1;
                else
                    high = mid - 1;
            }

            return -1; // x is not in the sorted array a
        }

        public static void Try_BinSearch()
        {
            int[] a = { 1, 2, 3 };
            int x = 3;
            int ret = BinSearch(a, x);

            if (ret < 0)
                Console.WriteLine("{0} not found", x);
            else
                Console.WriteLine("{0} is at index {1}", x, ret);
        }
    }
}
