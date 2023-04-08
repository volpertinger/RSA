using System.Numerics;

namespace RSA
{
    public class RSA
    {
        public Key Key { get; private init; }
        public bool IsCanEncrypt { get; private init; }
        public bool IsCanDecrypt { get; private init; }
        public int ByteBlockLength { get; private init; }

        // ------------------------------------------------------------------------------------------------------------
        // public
        // ------------------------------------------------------------------------------------------------------------

        public RSA(Key key, int maxBlockLength = Constants.MaxBlockLength)
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

        public bool Encrypt(FileStream ifs, FileStream ofs)
        {
            var buffer = new byte[ByteBlockLength];
            int length;
            while ((length = ifs.Read(buffer, 0, ByteBlockLength)) > 0)
            {
                var block = ByteArrayToBlock(buffer);
                var encrypt = EncryptBlock(block);
                var byteArray = BlockToByteArray(encrypt, ByteBlockLength);
                ofs.Write(byteArray, 0, ByteBlockLength);
                buffer = new byte[ByteBlockLength];
            }
            return true;
        }

        public bool Decrypt(FileStream ifs, FileStream ofs)
        {
            var buffer = new byte[ByteBlockLength];
            int length;
            while ((length = ifs.Read(buffer, 0, ByteBlockLength)) > 0)
            {
                var block = ByteArrayToBlock(buffer);
                var decrypt = DecryptBlock(block);
                var byteArray = BlockToByteArray(decrypt, length);
                ofs.Write(byteArray, 0, length);
            }
            return true;
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

        private int CalculateByteBlockLength(int maxBlockLength)
        {
            int result = 0;
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

        private static ulong ByteArrayToBlock(byte[] bytes)
        {
            ulong result = 0;
            for (int i = bytes.Length - 1; i >= 0; --i)
            {
                result <<= Constants.ByteLength;
                result += bytes[i];
            }
            return result;
        }

        private static byte[] BlockToByteArray(BigInteger block, int length)
        {
            var result = new byte[length];
            for (byte i = 0; i < length; ++i)
            {
                result[i] = (byte)(block & Constants.ByteMask);
                block >>= Constants.ByteLength;
            }
            return result;
        }
    }

    public class Key
    {
        public BigInteger ModNumber { get; set; }
        public BigInteger? OpenKey { get; set; }
        public BigInteger? SecretKey { get; set; }

        public Key(BigInteger modNumber, BigInteger? openKey, BigInteger? secretKey)
        {
            ModNumber = modNumber;
            OpenKey = openKey;
            SecretKey = secretKey;
        }
    }
}
