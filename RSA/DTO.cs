#pragma warning disable CS8618
namespace RSA
{
    public class Settings
    {
        public Key Key { get; set; }
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

    public class Key
    {
        public ulong? OpenNumber { get; set; }
        public ulong? OpenRelativePrime { get; set; }
        public ulong? SecretKey { get; set; }
    }
}
#pragma warning restore CS8618
