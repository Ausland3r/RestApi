using System;
using System.Text;
using RestApiClient.Models;

namespace RestApiClient.Utils
{
    public static class DataUtils
    {
        private static readonly Random Random = new Random();
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(chars[Random.Next(chars.Length)]);
            }

            return stringBuilder.ToString();
        }
    }
}
