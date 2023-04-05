namespace RSA
{
    public static class Utils
    {
        //-------------------------------------------------------------------------------------------------------------
        // public
        //-------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Euclidean algorithm
        /// </summary>
        /// <returns>GCD of lhs and rhs</returns>
        public static ulong GCD(ulong lhs, ulong rhs)
        {
            if (lhs == 0)
                return rhs;
            if (rhs == 0)
                return lhs;
            while (lhs != 0 && rhs != 0)
            {
                if (lhs > rhs)
                    lhs %= rhs;
                else
                    rhs %= lhs;
            }
            return Math.Max(lhs, rhs);
        }

        /// <summary>
        /// Extended euclidean algorithm
        /// </summary>
        /// <param name="lhs">First number</param>
        /// <param name="rhs">Second number</param>
        /// <param name="lhsCoef">First bezout ratio</param>
        /// <param name="rhsCoef">Second bezout ratio</param>
        /// <returns>GCD of lhs and rhs</returns>
        public static ulong ExtendedGCD(ulong lhs, ulong rhs, out long lhsCoef, out long rhsCoef)
        {
            long upL = 0, upR = 1, downL = 1, downR = 0;
            while (lhs != 0)
            {
                ulong quotient = rhs / lhs;
                ulong remainder = rhs % lhs;
                long newDownL = upL - downL * (long)quotient;
                long newDownR = upR - downR * (long)quotient;
                rhs = lhs;
                lhs = remainder;
                upL = downL;
                upR = downR;
                downL = newDownL;
                downR = newDownR;
            }
            lhsCoef = upL;
            rhsCoef = upR;
            return rhs;
        }

        /// <summary>
        /// Get reverse element to number modulo deductions
        /// </summary>
        /// <param name="number">The number to which we are looking for the inverse</param>
        /// <param name="mod">Deduction module</param>
        /// <returns>Reverse to number. If the number and the modulus are not coprime, then returns 0</returns>
        public static ulong GetReverse(ulong number, ulong mod)
        {
            long leftCoef = 0, rightCoef = 0;
            var gcd = ExtendedGCD(number, mod, out leftCoef, out rightCoef);
            if (gcd != 1)
                return 0;
            if (leftCoef < 0)
                return mod - (ulong)Math.Abs(leftCoef);
            return (ulong)leftCoef;
        }

        /// <summary>
        /// Obtaining a reduced system of residues modulo mod
        /// </summary>
        public static IEnumerable<ulong> GetReducedSystem(ulong mod)
        {
            List<ulong> result = new();
            for (ulong i = 0; i < mod; ++i)
            {
                if (GCD(i, mod) == 1)
                    result.Add(i);
            }
            return result;
        }

        /// <summary>
        /// Finding prime numbers using the sieve of eratosthenes
        /// </summary>
        public static IEnumerable<ulong> SieveEratosthenes(ulong length)
        {
            var numbers = new List<ulong>();
            // initial list filling
            for (var i = 2u; i < length; i++)
            {
                numbers.Add(i);
            }

            for (var i = 0; i < numbers.Count; i++)
            {
                for (var j = 2u; j < length; j++)
                {
                    // remove multiples from a list
                    numbers.Remove(numbers[i] * j);
                }
            }

            return numbers;
        }

        public static int Euler(ulong number)
        {
            if (number == 1)
                return 1;
            return GetReducedSystem(number).Count();
        }

        //-------------------------------------------------------------------------------------------------------------
        // private
        //-------------------------------------------------------------------------------------------------------------
    }
}
