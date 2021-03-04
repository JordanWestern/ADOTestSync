namespace TestSyncConsole.Extensions
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class StringExensions
    {
        private static readonly SHA1CryptoServiceProvider SHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();

        public static Guid GenerateGuid(this string str)
        {
            var bytes = new byte[16];
            Array.Copy(SHA1CryptoServiceProvider.ComputeHash(Encoding.Unicode.GetBytes(str)), bytes, bytes.Length);
            return new Guid(bytes);
        }
    }
}
