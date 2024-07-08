using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace HRMS.Functions
{
    public class HashPass
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string representation
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2")); // "x2" formats as hexadecimal
                }
                return builder.ToString();
            }
        }

        public static bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            using (var sha256 = SHA256.Create())
            {
                // Compute hash of entered password
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));

                // Convert byte array to a string representation
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2")); // "x2" formats as hexadecimal
                }
                string hashedEnteredPassword = builder.ToString();

                // Compare hashed entered password with stored hashed password
                return string.Equals(hashedEnteredPassword, storedHashedPassword, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}