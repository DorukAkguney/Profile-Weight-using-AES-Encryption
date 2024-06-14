using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Profile_Weight_Calculations
{
    internal static class Program
    {
        public static class GlobalData
        {
            public static Dictionary<string, double> Dataset { get; set; } = new Dictionary<string, double>();
        }

        // AES Anahtarı ve IV (Initialization Vector) tanımlamaları
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("A3CDE9ABFC4E0B6CDBE6F1DAE9F29C16"); // 32 bytes for AES-256
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("A1B2C3D4E5F6G7H8"); // 16 bytes for AES

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LoadData();
            Application.Run(new Form1());
            SaveData();
        }

        static void LoadData()
        {
            string txtFilePath = "veriler.txt";

            if (File.Exists(txtFilePath))
            {
                GlobalData.Dataset.Clear();

                try
                {
                    string[] lines = File.ReadAllLines(txtFilePath);
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(';');
                        if (parts.Length == 2)
                        {
                            string encryptedKey = parts[0];
                            string encryptedValue = parts[1];
                            string key = DecryptValue(encryptedKey);
                            double value = double.Parse(DecryptValue(encryptedValue));
                            GlobalData.Dataset[key] = value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Veri okuma hatası: {ex.Message}");
                }
            }
            else
            {
                GlobalData.Dataset = new Dictionary<string, double>();
                SaveData();
            }
        }

        static void SaveData()
        {
            string txtFilePath = "veriler.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(txtFilePath, false))
                {
                    foreach (var entry in GlobalData.Dataset)
                    {
                        string encryptedKey = EncryptValue(entry.Key);
                        string encryptedValue = EncryptValue(entry.Value.ToString());
                        writer.WriteLine($"{encryptedKey};{encryptedValue}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Veri kaydetme hatası: {ex.Message}");
            }
        }

        static string EncryptValue(string value)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(value);
                        }
                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }

        static string DecryptValue(string encryptedValue)
        {
            byte[] cipherText = Convert.FromBase64String(encryptedValue);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
