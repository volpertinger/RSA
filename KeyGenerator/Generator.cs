namespace KeyGenerator
{
    public static class Generator
    {
        public static void GeneratePrimesTuple(
            out ulong firstPrime,
            out ulong secondPrime,
            ulong minBorder = 0,
            ulong maxBorder = ulong.MaxValue,
            int seed = 0
            )
        {

            if (minBorder >= maxBorder)
                throw new ArgumentException(String.Format("Invalid borders value." +
                    "minBorder = {0} must be lower than maxBorder = {1}",
                    minBorder, maxBorder));
            var primes = RSA.Utils.SieveEratosthenes(maxBorder);
            primes = primes.Where(item => (item >= minBorder) && (item <= maxBorder));

            Random random = new Random(seed);
            firstPrime = primes.ElementAt(random.Next(primes.Count()));
            while ((secondPrime = primes.ElementAt(random.Next(primes.Count()))) == firstPrime) ;
        }
    }
}
