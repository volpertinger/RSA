namespace RSATests
{
    [TestClass]
    public class GeneratorTests
    {
        [TestMethod]
        public void TestGenerator()
        {
            ulong maxValue = 1000;
            ulong minValue = 400;
            var primes = RSA.Utils.SieveEratosthenes(maxValue);
            var firstPrime = 0ul;
            var secondPrime = 0ul;

            for (ulong i = 0; i < maxValue; ++i)
            {
                KeyGenerator.Generator.GeneratePrimesTuple(out firstPrime, out secondPrime, minValue, maxValue);
                Assert.IsTrue(primes.Contains(firstPrime));
                Assert.IsTrue(primes.Contains(secondPrime));
                Assert.AreNotEqual(firstPrime, secondPrime);
                Assert.IsTrue((firstPrime >= minValue) && (firstPrime <= maxValue));
                Assert.IsTrue((secondPrime >= minValue) && (secondPrime <= maxValue));
            }
        }
    }
}
