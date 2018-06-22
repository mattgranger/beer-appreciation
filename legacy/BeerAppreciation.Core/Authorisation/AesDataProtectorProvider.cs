using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Owin.Security.DataProtection;

namespace BeerAppreciation.Core.Authorisation
{
    /// <summary>
    /// Provides the ability for symmetric AES encryption and decription. Symmetric means that it uses the same key for both encryption and decryption.
    /// This provider does not use the machineKey as the encryption key, so can therefore encrypt/decrypt on different machines
    /// </summary>
    /// <seealso cref="Microsoft.Owin.Security.DataProtection.IDataProtector" />
    public class AesDataProtectorProvider : IDataProtector
    {
        /// <summary>
        /// The encryption key
        /// </summary>
        private readonly byte[] key;

        /// <summary>
        /// Initializes a new instance of the <see cref="AesDataProtectorProvider"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public AesDataProtectorProvider(string key)
        {
            using (SHA256Managed sha1 = new SHA256Managed())
            {
                this.key = sha1.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
        }

        /// <summary>
        /// Protects the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>A byte array of the protected data</returns>
        public byte[] Protect(byte[] data)
        {
            byte[] dataHash;

            using (SHA256Managed sha = new SHA256Managed())
            {
                dataHash = sha.ComputeHash(data);
            }

            using (AesManaged aesManaged = new AesManaged())
            {
                aesManaged.Key = this.key;
                aesManaged.GenerateIV();

                using (ICryptoTransform encryptor = aesManaged.CreateEncryptor(aesManaged.Key, aesManaged.IV))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Write(aesManaged.IV, 0, 16);

                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    using (BinaryWriter binaryWriter = new BinaryWriter(cryptoStream))
                    {
                        binaryWriter.Write(dataHash);
                        binaryWriter.Write(data.Length);
                        binaryWriter.Write(data);
                    }

                    return memoryStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Unprotects the specified protected data.
        /// </summary>
        /// <param name="protectedData">The protected data.</param>
        /// <returns>The unprotected data</returns>
        /// <exception cref="SecurityException">Signature does not match the computed hash</exception>
        public byte[] Unprotect(byte[] protectedData)
        {
            using (AesManaged aesManaged = new AesManaged())
            {
                aesManaged.Key = this.key;

                using (MemoryStream memoryStream = new MemoryStream(protectedData))
                {
                    byte[] iv = new byte[16];
                    memoryStream.Read(iv, 0, 16);

                    aesManaged.IV = iv;

                    using (ICryptoTransform decryptor = aesManaged.CreateDecryptor(aesManaged.Key, aesManaged.IV))
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    using (BinaryReader binaryReader = new BinaryReader(cryptoStream))
                    {
                        byte[] signature = binaryReader.ReadBytes(32);
                        int len = binaryReader.ReadInt32();
                        byte[] data = binaryReader.ReadBytes(len);

                        byte[] dataHash;
                        using (SHA256Managed sha = new SHA256Managed())
                        {
                            dataHash = sha.ComputeHash(data);
                        }

                        if (!dataHash.SequenceEqual(signature))
                        {
                            throw new SecurityException("Signature does not match the computed hash");
                        }

                        return data;
                    }
                }
            }
        }
    }

}