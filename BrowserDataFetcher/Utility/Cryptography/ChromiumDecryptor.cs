namespace BrowserDataFetcher
{
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The <see cref="ChromiumDecryptor"/> static class.
    /// </summary>
    internal static class ChromiumDecryptor
    {
        /// <summary>
        /// Decrypt a blink AES256GCM encrypted value.
        /// </summary>
        /// <param name="byteValue">
        /// Value to decrypt.
        /// </param>
        /// <param name="key">
        /// The master key.
        /// </param>
        /// <returns>
        /// Decrypted value as a plain string.
        /// </returns>
        public static string DecryptValue(byte[] byteValue, KeyParameter key)
        {
            if (byteValue[0] == 'v' && byteValue[1] == '1' && (byteValue[2] == '0' || byteValue[2] == '1'))
            {
                byte[] initializationVector = byteValue.Skip(3).Take(12).ToArray();
                byte[] payload = byteValue.Skip(15).ToArray();

                return Decrypt(payload, key, initializationVector);
            }
            else
            {
                return Encoding.Default.GetString(DataProtectionApi.Decrypt(byteValue));
            }
        }

        /// <summary>
        /// Decrypt a blink AES256GCM encrypted value.
        /// </summary>
        /// <param name="encryptedBytes">
        /// Value to decrypt.
        /// </param>
        /// <param name="key">
        /// The master key.
        /// </param>
        /// <param name="initializationVector">
        /// The initialization vector.
        /// </param>
        /// <returns>
        /// Decrypted value as a plain string.
        /// </returns>
        private static string Decrypt(byte[] encryptedBytes, KeyParameter key, byte[] initializationVector)
        {
            GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
            AeadParameters parameters = new AeadParameters(key, 128, initializationVector, null);

            cipher.Init(false, parameters);
            byte[] plainBytes = new byte[cipher.GetOutputSize(encryptedBytes.Length)];
            int retLen = cipher.ProcessBytes(encryptedBytes, 0, encryptedBytes.Length, plainBytes, 0);
            cipher.DoFinal(plainBytes, retLen);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}