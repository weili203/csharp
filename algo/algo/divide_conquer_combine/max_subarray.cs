/*
 * Find the subarray which give the maximum sum
 * For example: 
 * array 
 *  index  0   1   2    3   4   5    6    7   8    9   10   11   12   13   14   15 
 *  value 13  -3  -25  20  -3  -16  -23  18   20  -7   12   -5  -22   15   -4   7
 * 
 * the max subarray is [7, 10] with sum 43
 * 
 * steps:
 * 1. divide the array into two subarray, left and right
 * 2. process the left subarray 
 * 3. process the right array
 * 4. process the subarray which across the mid point
 * 5. recursively repeat from step1 ro step 4 for each subarray
 * 
 * 
 * Important:
 * For divide-conquer-combine method, the subproblem is just recursive function calls without any real work
 * The real work is done by the combine logic
*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace algo.divide_conquer_combine
{
    class Max_Subarray
    {
        private int[] _array = new int[] {13, -3, -25, 20, -3, -16, -23, 18, 20, -7, 12, -5, -22, 15, -4, 7};
        private int[] _array2 = new int[] { -1, 2, 5, -1, 3, -2, 1 };
        private int[] _array3 = new int[] { -2, 1, -3, 4, -1, 2, 1, -5, 4 };
        private int[] _array4 = new int[] { -3, 2, 1, -4, 5, 2, -1, 3, -1 };
        private int[] _array5 = new int[] { -1, 3, -5, 4, 6, -1, 2, -7, 13, -3 };

        public void TryFindMaxCrossingSubArray()
        {
            int left, right, maxValue;
            Find_Max_Crossing_Subarray(_array, 0, _array.Length / 2, _array.Length - 1, out left, out right, out maxValue);

            Console.WriteLine("Max sub array is from index range [{0},... {1}], with max sum of {2}", left, right, maxValue);
            Console.Write("the subarray is: [");
            for (int i = left; i <= right; ++i)
                Console.Write(_array[i].ToString() + " ");
            Console.Write("]\n");

        }

        public void TryFind_Max_Subarray()
        {
            int[] array = _array5;
            int left, right, maxValue;
            recursive_Find_Max_Subarray(array, 0, array.Length - 1, out left, out right, out maxValue);

            Console.WriteLine("Max sub array is from index range [{0},... {1}], with max sum of {2}", left, right, maxValue);
            Console.Write("the subarray is: [");
            for (int i = left; i <= right; ++i)
                Console.Write(array[i].ToString() + " ");
            Console.Write("]\n");
 
        }

        // O(nlgn)
        public void recursive_Find_Max_Subarray(int[] array, int low_bound, int high_bound,
                                                out int left, out int right, out int maxValue)
        {
            // check for one element array
            if (low_bound == high_bound)
            {
                left = low_bound;
                right = high_bound;
                maxValue = array[low_bound];
                return;
            }
            
            int mid = (low_bound + high_bound) / 2;
            int leftLow, leftHigh, leftMaxSum;
            recursive_Find_Max_Subarray(array, low_bound, mid, out leftLow, out leftHigh, out leftMaxSum);

            int rightLow, rightHigh, rightMaxSum;
            recursive_Find_Max_Subarray(array, mid + 1, high_bound, out rightLow, out rightHigh, out rightMaxSum);

            int crossLow = 0, crossHigh = 0, crossMaxSum = 0;
            Find_Max_Crossing_Subarray(array, low_bound, mid, high_bound, out crossLow, out crossHigh, out crossMaxSum);

            if ((leftMaxSum >= rightMaxSum) && (leftMaxSum >= crossMaxSum))
            {
                left = leftLow;
                right = leftHigh;
                maxValue = leftMaxSum;
            }
            else if ((rightMaxSum >= leftMaxSum) && (rightMaxSum >= crossMaxSum))
            {
                left = rightLow;
                right = rightHigh;
                maxValue = rightMaxSum;
            }
            else
            {
                left = crossLow;
                right = crossHigh;
                maxValue = crossMaxSum;
            }
        }

        // get the max subarray which cross the mid point
        // O(n)
        private void Find_Max_Crossing_Subarray(int[] array, int low_bound, int mid, int high_bound,
                                                           out int left, out int right, out int maxValue)
        {
            int sum=0, leftMaxSum = 0, rightMaxSum = 0;
            int leftIndex = 0, rightIndex = 0;
            // work on the left half [low, mid]
            for (int i = mid; i >= low_bound; --i)
            {
                sum += array[i];
                if (sum > leftMaxSum)
                {
                    leftMaxSum = sum;
                    leftIndex = i;
                }
 
            }

            // work on the left half, 
            // START FROM mid + 1,   [mid + 1, high]
            sum = 0;
            for (int i = mid + 1; i <= high_bound; ++i)
            {
                sum += array[i];
                if (sum > rightMaxSum)
                {
                    rightMaxSum = sum;
                    rightIndex = i;
                }

            }

            left = leftIndex;
            right = rightIndex;
            maxValue = leftMaxSum + rightMaxSum;

        }


    }
}
