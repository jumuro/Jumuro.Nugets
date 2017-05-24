using System.Security.Cryptography;
using System.Text;

namespace Jumuro.Security.Cryptography
{
    /// <summary>
    /// Represents a Hash provider.
    /// </summary>
    public interface IHashProvider
    {
        /// <summary>
        /// Computes the Hash for data using the received hash algorithm and <see cref="F:System.Text.Encoding.Default"/>.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="algo"></param>
        /// <returns></returns>
        byte[] GetHash(string data, HashAlgorithm algo);

        /// <summary>
        /// Computes the Hash for data using the received hash algorithm and encoding.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="algo"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        byte[] GetHash(string data, HashAlgorithm algo, Encoding encoding);
        
        /// <summary>
        /// Computes the Hash for received data using SHA1 algorithm and the default Encoding.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] GetSHA1Hash(string data);

        /// <summary>
        /// Computes the Hash for received data using SHA1 algorithm and the received Encoding.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        byte[] GetSHA1Hash(string data, Encoding encoding);

        /// <summary>
        /// Computes the Hash for received data using SHA256 algorithm and the default Encoding.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] GetSHA256Hash(string data);

        /// <summary>
        /// Computes the Hash for received data using SHA256 algorithm and the received Encoding.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        byte[] GetSHA256Hash(string data, Encoding encoding);

        /// <summary>
        /// Computes the Hash for received data using MD5 algorithm and the default Encoding.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] GetMD5Hash(string data);

        /// <summary>
        /// Computes the Hash for received data using MD5 algorithm and the received Encoding.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        byte[] GetMD5Hash(string data, Encoding encoding);
    }
}
