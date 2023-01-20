using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using System.IO;

public class Encryption
{

    public byte[] CreateSalt()
    {
        // Create a new salt
        var salt = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }
    public (byte[], byte[]) CreateKey()
    {
        byte[] key = new byte[32];
        byte[] iv = new byte[16];
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(key);
            rng.GetBytes(iv);
        }
        return (key, iv);
    }

    public bool ValidatePassword(string providedPassword, string storedHash, byte[] storedSalt)
    {
        // Hash the provided password using the stored salt
        string providedHash = HashPassword(providedPassword, storedSalt);

        // Compare the provided hash to the stored hash
        return providedHash.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
    }

    public string HashPassword(string password, byte[] salt)
    {
        // Create a new SHA256 object
        using (SHA256 sha256 = SHA256.Create())
        {
            // Convert the password and salt to a byte array
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] saltedPasswordBytes = new byte[passwordBytes.Length + salt.Length];
            Buffer.BlockCopy(passwordBytes, 0, saltedPasswordBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, saltedPasswordBytes, passwordBytes.Length, salt.Length);

            // Compute the hash of the salted password
            byte[] hashBytes = sha256.ComputeHash(saltedPasswordBytes);

            // Convert the hash bytes to a string
            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

            return hash;
        }
    }

    public string EncryptEmail(string email, byte[] key, byte[] iv)
    {
        
        byte[] emailBytes = System.Text.Encoding.UTF8.GetBytes(email);

       
        Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        
        MemoryStream memoryStream = new MemoryStream();

        // Create a new crypto stream using the memory stream and the AES object
        CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

       
        cryptoStream.Write(emailBytes, 0, emailBytes.Length);

        
        cryptoStream.FlushFinalBlock();
        cryptoStream.Close();

      
        byte[] encryptedEmailBytes = memoryStream.ToArray();

        
        return Convert.ToBase64String(encryptedEmailBytes);
    }

    public  string DecryptEmail(string encryptedEmail, byte[] key, byte[] iv)
    {
        // Convert the encrypted email from a string to a byte array
        byte[] encryptedEmailBytes = Convert.FromBase64String(encryptedEmail);

        // Create a new AES object with the key and iv
        Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        // Create a new memory stream
        using (MemoryStream memoryStream = new MemoryStream(encryptedEmailBytes))
        {
           
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
            {
               
                byte[] emailBytes = new byte[encryptedEmailBytes.Length];
                int bytesRead = cryptoStream.Read(emailBytes, 0, emailBytes.Length);

                
                return System.Text.Encoding.UTF8.GetString(emailBytes, 0, bytesRead);
            }
        }
    }
}
