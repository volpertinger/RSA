using System.Numerics;

namespace RSA
{
    public class RSA
    {
        public Key Key { get; private init; }
        public bool IsCanEncrypt { get; private init; }
        public bool IsCanDecrypt { get; private init; }
        public uint ByteBlockLength { get; private init; }

        // ------------------------------------------------------------------------------------------------------------
        // public
        // ------------------------------------------------------------------------------------------------------------

        public RSA(Key key, uint maxBlockLength = Constants.MaxBlockLength)
        {
            IsCanEncrypt = key.OpenKey.HasValue;
            IsCanDecrypt = key.SecretKey.HasValue;
            if (key.ModNumber <= 0)
                throw new ArgumentException(String.Format("Modulo number {0} is incorrect! " +
                    "Number must greater that zero.", key.ModNumber));
            if (!IsCanEncrypt && !IsCanDecrypt)
                throw new ArgumentException("key must contains at least public key or secret key!");
            Key = key;
            ByteBlockLength = CalculateByteBlockLength(maxBlockLength);
        }

        public BigInteger EncryptBlock(BigInteger block)
        {
            if (!IsCanEncrypt)
                throw new ArgumentException("Current RSA class can`t encrypt because open key is Null");

            return Utils.FastPow(block, Key.OpenKey!.Value, Key.ModNumber);
        }

        public BigInteger DecryptBlock(BigInteger block)
        {
            if (!IsCanDecrypt)
                throw new ArgumentException("Current RSA class can`t decrypt because secret key is Null");

            return Utils.FastPow(block, Key.SecretKey!.Value, Key.ModNumber);
        }

        // ------------------------------------------------------------------------------------------------------------
        // private
        // ------------------------------------------------------------------------------------------------------------

        private uint CalculateByteBlockLength(uint maxBlockLength)
        {
            uint result = 0;
            var counter = Key.ModNumber;
            while (counter > 0)
            {
                counter >>= Constants.ByteLength;
                if (counter > 0)
                    ++result;
            }
            if (result < Constants.MinBlockLength)
                throw new ArgumentException(String.Format("Modulo number {0} too low for " +
                    "minimum block encryption = {1} bytes.", Key.ModNumber, Constants.MinBlockLength));
            if (result > maxBlockLength)
                return maxBlockLength;
            return result;
        }
    }
}
