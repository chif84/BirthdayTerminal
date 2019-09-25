using ELMA.BirthdayTerminal.Droid.Services;
using ELMA.BirthdayTerminal.Services;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(LocalStorageService))]

namespace ELMA.BirthdayTerminal.Droid.Services
{

    /// <summary>
    /// Сервис локального хранения данных
    /// </summary>
    public class LocalStorageService : ILocalStorageService
    {
        private ISerializationService serializationService;

        private byte[] key = Encoding.UTF8.GetBytes("DBEE63FFFADF4110BD2E2920DB179819");
        private byte[] iv = Encoding.UTF8.GetBytes("4A615FB3BE73465C");

        /// <summary>
        /// Ctor
        /// </summary>
        public LocalStorageService()
        {
            serializationService = DependencyService.Get<ISerializationService>();
        }

        /// <summary>
        /// Поместить сериализованный объект
        /// </summary>
        public async void Set(string key, byte[] obj) //только для теста
        {
            var filePath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, string.Format("{0}", key));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (Stream file = File.OpenWrite(filePath))
            {
                file.Write(obj, 0, obj.Length);
            }
        }

        /// <summary>
        /// Поместить объект
        /// </summary>
        public async void Set<T>(string key, T obj) where T : class
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), string.Format("{0}.dat", key));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            try
            {
                var text = serializationService.Serialize(obj);
                File.WriteAllBytes(filePath, Encrypt(text));
            }
            catch
            {
            }
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        public T Get<T>(string key) where T : class
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), string.Format("{0}.dat", key));
            if (!File.Exists(filePath))
            {
                return null;
            }

            try
            {
                var text = Decrypt(File.ReadAllBytes(filePath));

                var obj = serializationService.Deserialize<T>(text);

                return obj;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Шифрование данных
        /// </summary>
        private byte[] Encrypt(string plainText)
        {
            byte[] encrypted;
            using (var myRijndael = new RijndaelManaged())
            {
                myRijndael.Key = key;
                myRijndael.IV = iv;

                var encryptor = myRijndael.CreateEncryptor(myRijndael.Key, myRijndael.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;

        }

        /// <summary>
        /// Дешифрование данных
        /// </summary>
        private string Decrypt(byte[] cipherText)
        {
            string plaintext = null;

            using (var myRijndael = new RijndaelManaged())
            {
                myRijndael.Key = key;
                myRijndael.IV = iv;

                var decryptor = myRijndael.CreateDecryptor(myRijndael.Key, myRijndael.IV);

                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }
            return plaintext;
        }
    }
}