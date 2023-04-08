#pragma warning disable CS8618
using System.Numerics;

namespace RSA
{
    public class Settings
    {
        public SKey SKey { get; set; }
        public AtomicOperation[] Operations { get; set; }
    }
    public class AtomicOperation
    {
        public string PathInput { get; set; }
        public string PathOutput { get; set; }
        public string Operation { get; set; }
    }

    public static class Operations
    {
        public const string Encrypt = "Encrypt";
        public const string Decrypt = "Decrypt";
    }

    /// <summary>
    /// Key with string representation for BigInteger deserialization
    /// </summary>
    public class SKey
    {
        public string ModNumber { get; set; }
        public string? OpenKey { get; set; }
        public string? SecretKey { get; set; }
    }
}
#pragma warning restore CS8618
