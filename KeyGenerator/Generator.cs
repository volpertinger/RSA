namespace KeyGenerator
{
    public static class Generator
    {
        //-------------------------------------------------------------------------------------------------------------
        // public
        //-------------------------------------------------------------------------------------------------------------
        public static void GeneratePrimesTuple(
            out ulong firstPrime,
            out ulong secondPrime,
            ulong minBorder = 0,
            ulong maxBorder = ulong.MaxValue
            )
        {
            ValidateBorders(minBorder, maxBorder);
            var primes = RSA.Utils.SieveEratosthenes(maxBorder);
            primes = primes.Where(item => (item >= minBorder) && (item <= maxBorder));

            Random random = new Random((int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            firstPrime = primes.ElementAt(random.Next(primes.Count()));
            while ((secondPrime = primes.ElementAt(random.Next(primes.Count()))) == firstPrime) ;
        }

        public static ulong GenerateRelativelyPrime(ulong number,
            ulong minBorder = 0)
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

        private static void ValidateBorders(ulong minBorder, ulong maxBorder)
        {
            if (minBorder >= maxBorder)
                throw new ArgumentException(String.Format("Invalid borders value." +
                    "minBorder = {0} must be lower than maxBorder = {1}",
                    minBorder, maxBorder));
        }
    }
}
