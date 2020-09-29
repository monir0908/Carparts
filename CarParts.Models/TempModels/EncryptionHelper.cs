using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CarParts.Models.TempModels
{
    public class EncryptionHelper
    {
        public static string GetPrivateKey()
        {
            return "<RSAKeyValue><Modulus>xo8s7Hm6CjgRk0+lfBY7LOsErmL/IZMzHy23Mc/3uHW4EqHvPMVzysgzRR8AqZuEkR/Kh8E9ozVn7qj7q06HvT6l7O982aMYlk5/Bmq+2qL+T9SS0+w3bZ+SazhSmjjiAqlSBQriM5sYvkM+f6dN4vJsADGBweUA+k7OXXcsBHM=</Modulus><Exponent>AQAB</Exponent><P>4gjTQt0GMVYWVDm3saHFcwSbfWFXJAkM8+h0k9ZU40C9kr/PSBvHbnqSAm3LfW/M8PeRPDJ6EVenBZP5OQew2w==</P><Q>4OHlIm0xiFTR9kQvaiifwa8sJ8usrPQ+hmyotZmfnLTS5OgyxcZtXrf7dLjnuk/6seLyQR0wwE4APsdJlzCiSQ==</Q><DP>BkRKXyszlcCWQ+WJw0IB8GtrSDGfsd8SXdzPBp5FojtURjJNM+mZQOXjEHAs2SB1ZSToAOxBWCO+/LeajEs7Sw==</DP><DQ>HkWKSKYWQtnYIaRwFYQ+bR4dfYXnSqjeOc4qr7dsSvX0Zaf0HbdmRZfSm5XAa84UWxnHrog1Zc2aLtk4yMddMQ==</DQ><InverseQ>e+Gj1IcystcNVk3Yt9WeJO86wl2Zsnq5mQLYKtYETrvpvYdh09BKFkaYPAPaOLUCf05HDugXlCipjlChv/tzvw==</InverseQ><D>UcZ/uvi7HSBQZLolroA9aNd1+xg8eSh1on6idzpujKK2572XmRC9CqP/MZV+IVwycc6FC/oTF5eUMV+ZHsld72IbDJ1qEvLfQA7YI3vSBFcjDQlwRhAglhV+NQ2QfK43eyRnH4nxKJDGIAy+NG0xOqA/i/tuscBAGWgxVaWysbE=</D></RSAKeyValue>";
        }

        // define the triple des provider
        private TripleDESCryptoServiceProvider m_des = new TripleDESCryptoServiceProvider();

        // define the string handler
        private UTF8Encoding m_utf8 = new UTF8Encoding();

        // define the local property arrays
        private byte[] m_key;
        private byte[] m_iv;

        // define the local key and vector byte arrays
        private readonly byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        private readonly byte[] iv = { 8, 7, 6, 5, 4, 3, 2, 1 };

        /// <summary>
        /// Set value for m_key and m_iv
        /// </summary>
        public EncryptionHelper()
        {
            this.m_key = key;
            this.m_iv = iv;
        }

        /// <summary>
        /// Transform into Byte.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] input)
        {
            return Transform(input, m_des.CreateEncryptor(m_key, m_iv));
        }

        /// <summary>
        /// Transform into byte
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] input)
        {
            return Transform(input, m_des.CreateDecryptor(m_key, m_iv));
        }

        /// <summary>
        /// convert string to base64string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Encrypt(string text)
        {
            byte[] input = m_utf8.GetBytes(text);
            byte[] output = Transform(input, m_des.CreateEncryptor(m_key, m_iv));

            return Convert.ToBase64String(output);
        }

        /// <summary>
        /// convert base64string to string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Decrypt(string text)
        {
            byte[] input = Convert.FromBase64String(text);
            byte[] output = Transform(input, m_des.CreateDecryptor(m_key, m_iv));

            return m_utf8.GetString(output);
        }

        /// <summary>
        /// Transform to byte.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="CryptoTransform"></param>
        /// <returns></returns>
        private byte[] Transform(byte[] input, ICryptoTransform CryptoTransform)
        {
            // create the necessary streams
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptStream = new CryptoStream(memStream, CryptoTransform, CryptoStreamMode.Write);


            // transform the bytes as requested
            cryptStream.Write(input, 0, input.Length);
            cryptStream.FlushFinalBlock();

            // Read the memory stream and convert it back into byte array
            memStream.Position = 0;
            byte[] result = new byte[Convert.ToInt32(memStream.Length - 1) + 1];
            memStream.Read(result, 0, Convert.ToInt32(result.Length));

            // close and release the streams
            memStream.Close();
            cryptStream.Close();

            // hand back the encrypted buffer
            return result;
        }
    }
}
