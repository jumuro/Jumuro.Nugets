using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumuro.Security.Cryptography.Extensions
{
    /// <summary>
    /// CryptographyExtensions class with some extension methods.
    /// </summary>
    public static class CryptographyExtensions
    {
        #region Extension Methods

        #region ToHexString

        /// <summary>
        /// Returns an hexadecimal string that represents the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="upperCase"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] bytes, bool upperCase = false)
        {
            StringBuilder result = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
            }

            return result.ToString();
        }

        #endregion

        #region ToBase64String

        /// <summary>
        /// Returns a base 64 string that represents the current object.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        #endregion

        #endregion
    }
}
