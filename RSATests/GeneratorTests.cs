﻿using System.Numerics;

namespace RSATests
{
    [TestClass]
    public class GeneratorTests
    {
        [TestMethod]
        public void TestPrimeGenerator()
        {
            BigInteger maxValue = 100;
            BigInteger minValue = 40;
            var primes = RSA.Utils.SieveEratosthenes(maxValue);
            BigInteger firstPrime = 0;
            BigInteger secondPrime = 0;

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

        [TestMethod]
        public void TestRelativelyPrimeGenerator()
        {
            ulong maxValue = 1000;
            ulong minValue = 400;

            for (ulong i = 0; i < maxValue; ++i)
            {
                var relativePrime = KeyGenerator.Generator.GenerateRelativelyPrime(maxValue ,minValue);
                Assert.AreEqual(1ul, RSA.Utils.GCD(relativePrime, maxValue));
            }
        }
    }
}
