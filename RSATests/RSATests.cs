using System.Numerics;

namespace RSATests
{
    [TestClass]
    public class RSATests
    {
        private RSA.RSA rsa = new(new RSA.Key(8612189837, 6968567183, 2644925087), 4);

        [TestMethod]
        public void TestBlocks()
        {
            List<BigInteger> plain = new() { 3, 26, 481, 1820, 15095, 172891, 2810294, 66753582, 987654321 };
            List<BigInteger> encrypted = plain.Select(x => rsa.EncryptBlock(x)).ToList();
            List<BigInteger> decrypted = encrypted.Select(x => rsa.DecryptBlock(x)).ToList();

            for (int i = 0; i < plain.Count && i < encrypted.Count; ++i)
            {
                Assert.AreEqual(plain[i], decrypted[i]);
                Assert.AreNotEqual(plain[i], encrypted[i]);
            }
        }
    }
}
