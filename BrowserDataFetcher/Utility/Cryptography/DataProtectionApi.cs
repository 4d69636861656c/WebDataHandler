namespace BrowserDataFetcher
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Data Protection API.
    /// </summary>
    internal static class DataProtectionApi
    {
        /// <summary>
        /// The <see cref="CryptprotectPromptstruct"/> structure provides the text of a prompt and information about when and where that
        /// prompt is to be displayed when using the CryptProtectData and CryptUnprotectData functions.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CryptprotectPromptstruct
        {
            public int cbSize;
            public int dwPromptFlags;
            public IntPtr hwndApp;
            public string szPrompt;
        }

        /// <summary>
        /// The <see cref="DataBlob"/>.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct DataBlob
        {
            public int cbData;
            public IntPtr pbData;
        }

        /// <summary>
        /// Decrypts and does an integrity check of the data in a <see cref="DataBlob"/> structure.
        /// Usually, the only user who can decrypt the data is a user with the same logon credentials as the user who encrypted the data.
        /// In addition, the encryption and decryption must be done on the same computer.
        /// </summary>
        /// <param name="pCipherText">
        /// A pointer to a <see cref="DataBlob"/> structure that holds the encrypted data.
        /// The <see cref="DataBlob"/> structure's cbData member holds the length of the pbData member's byte string that contains the text to be encrypted.
        /// </param>
        /// <param name="pszDescription">
        /// A pointer to a string-readable description of the encrypted data included with the encrypted data.
        /// This parameter can be set to <c>null</c>. When you have finished using ppszDataDescr, free it by calling the LocalFree function.
        /// </param>
        /// <param name="pEntropy">
        /// A pointer to a <see cref="DataBlob"/> structure that contains a password or other additional entropy used when the data was encrypted.
        /// This parameter can be set to <c>null</c>; however, if an optional entropy <see cref="DataBlob"/> structure was used in the encryption phase,
        /// that same <see cref="DataBlob"/> structure must be used for the decryption phase.
        /// </param>
        /// <param name="pReserved">
        /// This parameter is reserved for future use and must be set to <c>null</c>.
        /// </param>
        /// <param name="pPrompt">
        /// A pointer to a <see cref="CryptprotectPromptstruct"/> structure that provides information about where and when prompts are to be displayed and
        /// what the content of those prompts should be. This parameter can be set to <c>null</c>.
        /// </param>
        /// <param name="dwFlags">
        /// An <see cref="Int32"/> value that specifies options for this function. This parameter can be <c>zero</c>, in which case no option is set.
        /// </param>
        /// <param name="pPlainText">
        /// A pointer to a <see cref="DataBlob"/> structure where the function stores the decrypted data.
        /// When you have finished using the <see cref="DataBlob"/> structure, free its pbData member by calling the LocalFree function.
        /// </param>
        /// <returns>
        /// If the function succeeds, the function returns <c>true</c>. If the function fails, it returns <c>false</c>.
        /// </returns>
        [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CryptUnprotectData(ref DataBlob pCipherText, ref string pszDescription, ref DataBlob pEntropy, IntPtr pReserved, ref CryptprotectPromptstruct pPrompt, int dwFlags, ref DataBlob pPlainText);

        /// <summary>
        /// Decrypts data.
        /// </summary>
        /// <param name="bCipher">
        /// The encrypted data.
        /// </param>
        /// <param name="bEntropy">
        /// Password or other additional entropy used when the data was encrypted.
        /// </param>
        /// <returns>
        /// Returns the decrypted data.
        /// </returns>
        public static byte[] Decrypt(byte[] bCipher, byte[] bEntropy = null)
        {
            DataBlob pPlainText = new DataBlob();
            DataBlob pCipherText = new DataBlob();
            DataBlob pEntropy = new DataBlob();

            CryptprotectPromptstruct pPrompt = new CryptprotectPromptstruct()
            {
                cbSize = Marshal.SizeOf(typeof(CryptprotectPromptstruct)),
                dwPromptFlags = 0,
                hwndApp = IntPtr.Zero,
                szPrompt = (string)null
            };

            string sEmpty = string.Empty;

            try
            {
                try
                {
                    if (bCipher == null)
                    {
                        bCipher = new byte[0];
                    }

                    pCipherText.pbData = Marshal.AllocHGlobal(bCipher.Length);
                    pCipherText.cbData = bCipher.Length;
                    Marshal.Copy(bCipher, 0, pCipherText.pbData, bCipher.Length);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.StackTrace);
                }

                try
                {
                    if (bEntropy == null)
                    {
                        bEntropy = new byte[0];
                    }

                    pEntropy.pbData = Marshal.AllocHGlobal(bEntropy.Length);
                    pEntropy.cbData = bEntropy.Length;

                    Marshal.Copy(bEntropy, 0, pEntropy.pbData, bEntropy.Length);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.StackTrace);
                }

                CryptUnprotectData(ref pCipherText, ref sEmpty, ref pEntropy, IntPtr.Zero, ref pPrompt, 1, ref pPlainText);

                byte[] bDestination = new byte[pPlainText.cbData];
                Marshal.Copy(pPlainText.pbData, bDestination, 0, pPlainText.cbData);
                return bDestination;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.StackTrace);
            }
            finally
            {
                if (pPlainText.pbData != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pPlainText.pbData);
                }

                if (pCipherText.pbData != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pCipherText.pbData);
                }

                if (pEntropy.pbData != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pEntropy.pbData);
                }
            }

            return new byte[0];
        }
    }
}