namespace RSA
{
    public static class Utils
    {
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
        /// <param name="lhs">first number</param>
        /// <param name="rhs">second number</param>
        /// <param name="lhsCoef">first bezout ratio</param>
        /// <param name="rhsCoef">second bezout ratio</param>
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
    }
}
