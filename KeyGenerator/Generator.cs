using System.Numerics;

namespace KeyGenerator
{
    public static class Generator
    {
        //-------------------------------------------------------------------------------------------------------------
        // public
        //-------------------------------------------------------------------------------------------------------------
        public static void GeneratePrimesTuple(
            out BigInteger firstPrime,
            out BigInteger secondPrime,
            BigInteger minBorder,
            BigInteger maxBorder
            )
        {
            ValidateBorders(minBorder, maxBorder);
            var primes = RSA.Utils.SieveEratosthenes(maxBorder);
            primes = primes.Where(item => (item >= minBorder) && (item <= maxBorder));

            Random random = new Random((int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            var index = random.Next(primes.Count());
            firstPrime = primes.ElementAt(index);

            var ignoringElement = primes.ElementAt(index);
            primes = primes.Where(item => item != ignoringElement);
            secondPrime = primes.ElementAt(random.Next(primes.Count()));
        }

        public static ulong GenerateRelativelyPrime(BigInteger number,
            BigInteger minBorder)
        {
            var maxBorder = number;
            ValidateBorders(minBorder, maxBorder);
            Random random = new Random((int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            ulong result = (ulong)random.NextInt64((long)minBorder, (long)maxBorder);
            while (RSA.Utils.GCD(result, number) != 1 || result < minBorder)
                result = (ulong)random.NextInt64((long)minBorder, (long)maxBorder);
            return result;
        }

        //-------------------------------------------------------------------------------------------------------------
        // private
        //-------------------------------------------------------------------------------------------------------------

        private static void ValidateBorders(BigInteger minBorder, BigInteger maxBorder)
        {
            if (minBorder >= maxBorder)
                throw new ArgumentException(String.Format("Invalid borders value." +
                    "minBorder = {0} must be lower than maxBorder = {1}",
                    minBorder, maxBorder));
        }
    }
}
