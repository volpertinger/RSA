namespace RSA
{
    public static class Constants
    {
        public const byte ByteLength = 8;

        public const byte ByteMask = 0b11111111;

        /// <summary>
        ///  Minimum block length for encryption and decryption in bytes
        /// </summary>
        public const uint MinBlockLength = 1;
    }
}
