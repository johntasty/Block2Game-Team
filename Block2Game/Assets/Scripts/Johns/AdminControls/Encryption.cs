using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
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
}
