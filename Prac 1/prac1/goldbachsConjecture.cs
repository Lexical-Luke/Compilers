//                          Goldbach's conjecture
/*
is that every even number greater than 2 can be expressed as the sum of two prime numbers. 
Write a program that examines every even integer N from 4 to Limit, attempting to find a pair 
of prime numbers (A, B) such that N = A + B. If successful the program should write N, A and B; 
otherwise it should write a message indicating that the conjecture has been disproved. 
This might be done in various ways. Since the hidden agenda is to familiarize you with 
the use of a class for manipulating "sets", you must use the variation on the sieve method 
suggested by the code you have already seen: create a "set" of prime numbers first in an object 
of the IntSet class, and then use this set intelligently to check the conjecture.
*/

using Library;

class goldbachsConjecture
{
    public static void Main(string[] args)
    {
        const int Max = 32000;
        IntSet uncrossed = new IntSet(Max);
        int i, limit, k, it, iterations, primes = 0;   // counters
        IO.Write("How many iterations? ");
        iterations = IO.ReadInt();
        bool display = true;
        IO.Write("Supply largest number to be tested ");
        limit = IO.ReadInt();
        if (limit > Max)
        {
            IO.Write("limit too large, sorry");
            System.Environment.Exit(1);
        }
        for (it = 1; it <= iterations; it++)
        {
            primes = 0;
            for (i = 2; i <= limit; i++)
            {
                // clear sieve
                uncrossed.Incl(i);
            }
            for (i = 2; i <= limit; i++)
            {               // the passes over the sieve
                if (uncrossed.Contains(i) == true)
                {
                    primes++;
                    k = i;                               // now cross out multiples of i
                    do
                    {
                        uncrossed.Excl(k);
                        k += i;
                    } while (k <= limit);
                }
            }
        }
        uncrossed.Incl(1);
        IntSet primeSet = new IntSet(uncrossed);
        IO.WriteLine("here");

        for (i = 2; i <= limit; i++)                 // clear sieve
            uncrossed.Incl(i);
        for (i = 3; i <= limit; i += 2)              // exclude reference to odd integers
            uncrossed.Excl(i);
        for (i = 4; i <= limit; i += 2)
        {               // the passes over the sieve in even numbers greater than 4
            if (uncrossed.Contains(i) == true)
            {
                for (int a = 1; a <= limit; a++)
                {
                    for (int b = 1; b <= limit; b++)
                    {
                        if (primeSet.Contains(a) && primeSet.Contains(b))
                        { 
                            IO.WriteLine(a);
                            IO.WriteLine(b);
                            IO.WriteLine(i);
                            if (a + b == i)
                            {
                                IO.WriteLine("N = A + B: -> " + i.ToString() + " = " + a.ToString() + " + " + b.ToString());                           
                            }                              
                        }     
                    }
                }
            }
        if (display) IO.WriteLine();
        } // main
    }
}