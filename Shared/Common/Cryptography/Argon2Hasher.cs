using Isopoh.Cryptography.Argon2;
using Isopoh.Cryptography.SecureArray;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Cryptography
{
    public static class Argon2Hasher
    {
        public static byte[] GenerateSecret()
        {
            var buffer = new byte[16];
#pragma warning disable SYSLIB0023 // Type or member is obsolete
            var rng = new RNGCryptoServiceProvider();
#pragma warning restore SYSLIB0023 // Type or member is obsolete
            rng.GetBytes(buffer);

            return buffer;
        }

        public static string HashString(string input, byte[] secret)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);

            var config = new Argon2Config
            {
                Secret = secret,
                Password = inputBytes
            };

            var argon2 = new Argon2(config);

            string hash;
            using (SecureArray<byte> sa = argon2.Hash())
            {
                hash = Convert.ToBase64String(sa.Buffer);
            }

            return hash;
        }
    }
}
