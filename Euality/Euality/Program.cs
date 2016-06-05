using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euality
{
    class Program
    {
        static void Main(string[] args)
        {
            ValueType();

            ReferenceType();
        }

        struct ValueTpe
        {
            public int a;
            public int b;

            // Value type must overload == to use it
            public static bool operator == (ValueTpe lhs, ValueTpe rhs)
            {
                return lhs.a == rhs.a && lhs.b == rhs.b;
            }

            public static bool operator !=(ValueTpe lhs, ValueTpe rhs)
            {
                return !(lhs == rhs);
            }
        }

        static void ValueType()
        {
            int a = 10;
            int b = 10;

            Console.WriteLine(a == b);
            Console.WriteLine(a.Equals(b));

            var aV = new ValueTpe { a = 1, b = 2 };
            var bV = new ValueTpe { a = 1, b = 2 };
            Console.WriteLine(aV == bV);
            Console.WriteLine(aV.Equals(bV));   // if Value type doesnt override Equals then it uses reflection and compare memory byte by byte

        }

        static void ReferenceType()
        {
            // Create two equal but distinct strings
            string a = new string(new char[] { 'h', 'e', 'l', 'l', 'o' });
            string b = new string(new char[] { 'h', 'e', 'l', 'l', 'o' });

            Console.WriteLine(a == b);   // string class overload ==
            Console.WriteLine(a.Equals(b));    // string class override Equals virtual method

            // Now let's see what happens with the same tests but
            // with variables of type object
            object c = a;
            object d = b;

            Console.WriteLine(c == d);
            Console.WriteLine(c.Equals(d));   // Equal is virtual method
        }



    }
}
