using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PatientProject.Domain.Aggregates.PatientAggregate
{
    public class EncryptedSocialInsuranceNumber
    {
        private readonly string _encryptionKey;

        public EncryptedSocialInsuranceNumber(IConfiguration configuration)
        {   
            _encryptionKey = configuration["EncryptionSettings:EncryptionKey"] ?? "gcp4YEaaGjtJ2sOExfc8NeFi5HuYJ16JoqrIbxFdpIU=";
        }
        public (byte[] encryptedData, byte[] iv) Encrypt(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(_encryptionKey);
                aes.GenerateIV(); 
                byte[] iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
                using (var ms = new MemoryStream())
                {
                 
                    ms.Write(iv, 0, iv.Length);

                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cs))
                    {
                        writer.Write(plainText);
                    }

                    return (ms.ToArray(), iv);
                }
            }
        }

        public string Decrypt(byte[] encryptedData, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(_encryptionKey);
                aes.IV = iv; 

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(encryptedData, iv.Length, encryptedData.Length - iv.Length))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs))
                {
                    return reader.ReadToEnd();
                }
            }
        }

    }
}
