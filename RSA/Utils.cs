using System.Collections;
using System.Numerics;

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
        public static BigInteger GCD(BigInteger lhs, BigInteger rhs)
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
            if (lhs > rhs)
                return lhs;
            return rhs;
        }

        /// <summary>
        /// Extended euclidean algorithm
        /// </summary>
        /// <param name="lhs">First number</param>
        /// <param name="rhs">Second number</param>
        /// <param name="lhsCoef">First bezout ratio</param>
        /// <param name="rhsCoef">Second bezout ratio</param>
        /// <returns>GCD of lhs and rhs</returns>
        public static BigInteger ExtendedGCD(BigInteger lhs, BigInteger rhs, out BigInteger lhsCoef, out BigInteger rhsCoef)
        {
            BigInteger upL = 0, upR = 1, downL = 1, downR = 0;
            while (lhs != 0)
            {
                BigInteger quotient = rhs / lhs;
                BigInteger remainder = rhs % lhs;
                BigInteger newDownL = upL - downL * quotient;
                BigInteger newDownR = upR - downR * quotient;
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
        public static BigInteger GetReverse(BigInteger number, BigInteger mod)
        {
            BigInteger leftCoef = 0, rightCoef = 0;
            var gcd = ExtendedGCD(number, mod, out leftCoef, out rightCoef);
            if (gcd != 1)
                return 0;
            if (leftCoef < 0)
                return mod + leftCoef;
            return leftCoef;
        }

        /// <summary>
        /// Obtaining a reduced system of residues modulo mod
        /// </summary>
        public static IEnumerable<BigInteger> GetReducedSystem(BigInteger mod)
        {
            List<BigInteger> result = new();
            for (BigInteger i = 0; i < mod; ++i)
            {
                if (GCD(i, mod) == 1)
                    result.Add(i);
            }
            return result;
        }

        /// <summary>
        /// Finding prime numbers (that less than input number) using the sieve of eratosthenes
        /// </summary>
        public static IEnumerable<BigInteger> SieveEratosthenes(BigInteger upperLimit)
        {
            if (upperLimit > int.MaxValue)
                throw new ArgumentException(String.Format("upper limit is BigInteger only for " +
                    "interface interaction. Max vaule is {0}, but given {1}",
                    int.MaxValue, upperLimit));

            BitArray composite = new BitArray((int)upperLimit);

            int sqrt = (int)Math.Sqrt((int)upperLimit);
            for (int p = 2; p <= sqrt; ++p)
            {
                if (composite[p]) continue;

                yield return p;

                for (int i = p * p; i < upperLimit; i += p)
                    composite[i] = true;
            }
            for (int p = sqrt + 1; p < upperLimit; ++p)
            {
                if (!composite[p]) yield return p;
            }
        }

        /// <summary>
        /// Number factorization
        /// </summary>
        public static IEnumerable<NumberFactor> Factorization(BigInteger number)
        {
            var primes = SieveEratosthenes(number + 1);
            var result = new List<NumberFactor>();
            foreach (var prime in primes)
            {
                if (number == 1)
                    break;
                var currentFactor = new NumberFactor(prime);
                while (number % prime == 0)
                {
                    number /= prime;
                    ++currentFactor.Degree;
                }
                if (currentFactor.Degree > 0)
                    result.Add(currentFactor);
                if (primes.Contains(number))
                {
                    result.Add(new NumberFactor(number, 1));
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Calculates the Euler`s function from number
        /// </summary>
        public static BigInteger Euler(BigInteger number)
        {
            if (number == 1)
                return 1;
            return GetReducedSystem(number).Count();
        }

        /// <summary>
        /// Calculates the Euler`s function from number factorization
        /// </summary>
        public static BigInteger EulerByFactoriation(IEnumerable<NumberFactor> factorization)
        {
            BigInteger result = 1;
            foreach (var factor in factorization)
            {
                result *= FastPow(factor.Prime, factor.Degree) - FastPow(factor.Prime, factor.Degree - 1);
            }
            return result;
        }

        /// <summary>
        /// Binary exponentiation modulo mod
        /// </summary>
        public static BigInteger FastPow(BigInteger number, BigInteger degree, BigInteger mod)
        {
            BigInteger result = 1;
            while (degree > 0)
            {
                if ((degree & 1) != 0)
                {
                    result = (result * number) % mod;
                }
                number = (number * number) % mod;
                degree >>= 1;
            }
            return result;
        }

        /// <summary>
        /// Binary exponentiation
        /// </summary>
        public static BigInteger FastPow(BigInteger number, BigInteger degree)
        {
            BigInteger result = 1;
            while (degree > 0)
            {
                if ((degree & 1) != 0)
                {
                    result *= number;
                }
                number *= number;
                degree >>= 1;
            }
            return result;
        }

        //-------------------------------------------------------------------------------------------------------------
        // private
        //-------------------------------------------------------------------------------------------------------------
    }

    public class NumberFactor
    {
        public BigInteger Prime { get; set; }
        public uint Degree { get; set; }

        public NumberFactor(BigInteger prime)
        {
            Prime = prime;
            Degree = 0;
        }

        public NumberFactor(BigInteger prime, uint degree)
        {
            Prime = prime;
            Degree = degree;
        }

        public static bool operator ==(NumberFactor lhs, NumberFactor rhs)
        {
            return (lhs.Prime == rhs.Prime) && (lhs.Degree == rhs.Degree);
        }
        public static bool operator !=(NumberFactor lhs, NumberFactor rhs)
        {
            return !(lhs == rhs);
        }
    }
}
