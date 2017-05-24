using System.Security.Cryptography;
using System.Text;

namespace Jumuro.Security.Cryptography
{
    /// <summary>
    /// Represents a hash provider that can compute the hash using several algorithms.
    /// </summary>
    public class HashProvider : IHashProvider
    {
        #region GetHash

        /// <summary>
        /// Computes the Hash for data using the received hash algorithm and <see cref="F:System.Text.Encoding.Default"/>.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="algo"></param>
        /// <returns></returns>
        public byte[] GetHash(string data, HashAlgorithm algo)
        {
            return GetHash(data, algo, Encoding.Default);
        }

        /// <summary>
        /// Computes the Hash for data using the received hash algorithm and encoding.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="algo"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public byte[] GetHash(string data, HashAlgorithm algo, Encoding encoding)
        {
            return algo.ComputeHash(encoding.GetBytes(data));
        }

        #endregion

        #region GetSHA1Hash

        /// <summary>
        /// Computes the Hash for received data using SHA1 algorithm and the default Encoding.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] GetSHA1Hash(string data)
        {
            return GetSHA1Hash(data, Encoding.Default);
        }

        /// <summary>
        /// Computes the Hash for received data using SHA1 algorithm and the received Encoding.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public byte[] GetSHA1Hash(string data, Encoding encoding)
        {
            return GetHash(data, new SHA1Cng(), encoding);
        }

        #endregion

        #region GetSHA256Hash

        /// <summary>
        /// Computes the Hash for received data using SHA256 algorithm and the default Encoding.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] GetSHA256Hash(string data)
        {
            return GetSHA256Hash(data, Encoding.Default);
        }

        /// <summary>
        /// Computes the Hash for received data using SHA256 algorithm and the received Encoding.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public byte[] GetSHA256Hash(string data, Encoding encoding)
        {
            return GetHash(data, new SHA256Cng(), encoding);
        }

        #endregion

        #region GetMD5Hash

        /// <summary>
        /// Computes the Hash for received data using MD5 algorithm and the default Encoding.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] GetMD5Hash(string data)
        {
            return GetMD5Hash(data, Encoding.Default);
        }

        /// <summary>
        /// Computes the Hash for received data using MD5 algorithm and the received Encoding.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public byte[] GetMD5Hash(string data, Encoding encoding)
        {
            return GetHash(data, new MD5Cng(), encoding);
        }

        #endregion
    }
}
