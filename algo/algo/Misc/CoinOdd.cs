using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace algo
{
    /*
     * The coin game: 5 people in the room. At each round, a person is selected at random. 
     * A coin is then tossed: if head, he wins, if tail, he loses and is removed from the group. 
     * Continue until someone wins or nobody left. 
     * What is the probability of winning of a person (in the original 5)?
     */
    class CoinOdd
    {
        public static double Calculate()
        {
            double ret = 0;

            // probability to win - toss head
            double p = 1.0 / 2.0;
            // probability to lose - toss tail
            double q = 1.0 - p;

            // the odds for the 1st person to win
            double d1 = (1.0 / 5.0) * p;

            // the odds for the 2nd person to win
            double d2 = ((1.0 / 5.0) * q) * ((1.0 / 4.0) * p);

            // the odds for the 3rd person to win
            double d3 = ((1.0 / 5.0) * q) * ((1.0 / 4.0) * q) * ((1.0 / 3.0) * p);

            // the odds for the 4th person to win
            double d4 = ((1.0 / 5.0) * q) * ((1.0 / 4.0) * q) * ((1.0 / 3.0) * q) * ((1.0 / 2.0) * p);

            // the odds for the 4th person to win
            double d5 = ((1.0 / 5.0) * q) * ((1.0 / 4.0) * q) * ((1.0 / 3.0) * q) * ((1.0 / 2.0) * q) * ((1.0 / 1.0) * p);

            ret = d1 + d2 + d3 + d4 + d5;

            return ret;
        }
    }
}
