namespace RSATests
{
    [TestClass]
    public class UtilsTests
    {
        [TestMethod]
        public void TestGCD()
        {
            // zeros
            Assert.AreEqual(0ul, RSA.Utils.GCD(0, 0));
            Assert.AreEqual(1ul, RSA.Utils.GCD(0, 1));
            Assert.AreEqual(1ul, RSA.Utils.GCD(1, 0));
            Assert.AreEqual(ulong.MaxValue, RSA.Utils.GCD(ulong.MaxValue, 0));
            Assert.AreEqual(ulong.MaxValue / 2, RSA.Utils.GCD(ulong.MaxValue / 2, 0));

            // same
            ulong maxIter = 100;
            for (ulong i = 1; i < maxIter; i += ulong.MaxValue / maxIter)
                Assert.AreEqual(i, RSA.Utils.GCD(i, i));

            // main
            Assert.AreEqual(4ul, RSA.Utils.GCD(4, 60));
            Assert.AreEqual(20ul, RSA.Utils.GCD(40, 60));
            Assert.AreEqual(3ul, RSA.Utils.GCD(468732, 2121));
            Assert.AreEqual(1ul, RSA.Utils.GCD(3641684623648, 1009999));
            Assert.AreEqual(1ul, RSA.Utils.GCD(ulong.MaxValue, 1));
            Assert.AreEqual(9ul, RSA.Utils.GCD(987654321234, 234567));
            Assert.AreEqual(2ul, RSA.Utils.GCD(74936254782, 1526428164820298));
            Assert.AreEqual(99ul, RSA.Utils.GCD(112233445566778899, 998877665544332211));
        }

        [TestMethod]
        public void TestExtendedGCD()
        {
            long leftCoeff = 0, rightCoeff = 0;

            // zeros

            Assert.AreEqual(0ul, RSA.Utils.ExtendedGCD(0, 0, out leftCoeff, out rightCoeff));
            Assert.AreEqual(0L, leftCoeff);
            Assert.AreEqual(1L, rightCoeff);

            Assert.AreEqual(1ul, RSA.Utils.ExtendedGCD(0, 1, out leftCoeff, out rightCoeff));
            Assert.AreEqual(0L, leftCoeff);
            Assert.AreEqual(1L, rightCoeff);

            Assert.AreEqual(1ul, RSA.Utils.ExtendedGCD(1, 0, out leftCoeff, out rightCoeff));
            Assert.AreEqual(1L, leftCoeff);
            Assert.AreEqual(0L, rightCoeff);

            // same
            ulong maxIter = 100;
            for (ulong i = 1; i < maxIter; i += ulong.MaxValue / maxIter)
            {
                Assert.AreEqual(i, RSA.Utils.ExtendedGCD(i, i, out leftCoeff, out rightCoeff));
                Assert.AreEqual(1L, leftCoeff);
                Assert.AreEqual(0L, rightCoeff);
            }

            // main
            Assert.AreEqual(3ul, RSA.Utils.ExtendedGCD(4632, 61551, out leftCoeff, out rightCoeff));
            Assert.AreEqual(-3043L, leftCoeff);
            Assert.AreEqual(229L, rightCoeff);

            Assert.AreEqual(4ul, RSA.Utils.ExtendedGCD(53928464, 364927364, out leftCoeff, out rightCoeff));
            Assert.AreEqual(-7616318L, leftCoeff);
            Assert.AreEqual(1125529L, rightCoeff);

            Assert.AreEqual(1ul, RSA.Utils.ExtendedGCD(45728, 9999, out leftCoeff, out rightCoeff));
            Assert.AreEqual(-703L, leftCoeff);
            Assert.AreEqual(3215L, rightCoeff);

            Assert.AreEqual(4ul, RSA.Utils.ExtendedGCD(53928464, 364927364, out leftCoeff, out rightCoeff));
            Assert.AreEqual(-7616318L, leftCoeff);
            Assert.AreEqual(1125529L, rightCoeff);

            Assert.AreEqual(99ul, RSA.Utils.ExtendedGCD(112233445566778899, 998877665544332211, out leftCoeff, out rightCoeff));
            Assert.AreEqual(-89L, leftCoeff);
            Assert.AreEqual(10L, rightCoeff);

            Assert.AreEqual(1ul, RSA.Utils.ExtendedGCD(2639263538191, 41923400129346160, out leftCoeff, out rightCoeff));
            Assert.AreEqual(-19752544724080689L, leftCoeff);
            Assert.AreEqual(1243510090210L, rightCoeff);

            Assert.AreEqual(1ul, RSA.Utils.ExtendedGCD(12345678910, 10987654321, out leftCoeff, out rightCoeff));
            Assert.AreEqual(-49450475L, leftCoeff);
            Assert.AreEqual(55562331L, rightCoeff);

            Assert.AreEqual(5ul, RSA.Utils.ExtendedGCD(6372839463517235, 215, out leftCoeff, out rightCoeff));
            Assert.AreEqual(11L, leftCoeff);
            Assert.AreEqual(-326052251621812L, rightCoeff);
        }

        [TestMethod]
        public void TestReverse()
        {
            // same
            Assert.AreEqual(0ul, RSA.Utils.GetReverse(0, 0));
            Assert.AreEqual(1ul, RSA.Utils.GetReverse(1, 1));
            for (ulong i = 2; i < 100; ++i)
            {
                Assert.AreEqual(0ul, RSA.Utils.GetReverse(i, i));
            }

            // main tests
            Assert.AreEqual(0ul, RSA.Utils.GetReverse(1287247836921, 0));
            Assert.AreEqual(0ul, RSA.Utils.GetReverse(0, 1287247836921));
            Assert.AreEqual(9ul, RSA.Utils.GetReverse(3, 26));
            Assert.AreEqual(8933ul, RSA.Utils.GetReverse(3, 26798));
            Assert.AreEqual(101748590926ul, RSA.Utils.GetReverse(524723782, 1287247836921));
            
            Assert.AreEqual(21481267ul, RSA.Utils.GetReverse(69182549281122748, 24828289));
            Assert.AreEqual(0ul, RSA.Utils.GetReverse(8357100938261829039, 7357100938261017));
        }
    }
}