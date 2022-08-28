namespace BrowserDataFetcher
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// The <see cref="GeckoDecryptor"/> helper class.
    /// </summary>
    internal static class GeckoDecryptor
    {
        private static IntPtr hNss3;
        private static IntPtr hMozGlue;

        private static Nss3.NssInit fpNssInit;
        private static Nss3.Pk11SdrDecrypt fpPk11SdrDecrypt;
        private static Nss3.NssShutdown fpNssShutdown;

        private const string MozGlueDll = "\\mozglue.dll";
        private const string NssDll = "\\nss3.dll";

        /// <summary>
        /// Load libraries and functions for Mozilla Firefox value decryption.
        /// </summary>
        /// <param name="mozillaPath">
        /// Mozilla Firefox folder path in ProgramFiles.
        /// </param>
        /// <returns>
        /// <c>True</c> if everything was successful, <c>false</c> otherwise.
        /// </returns>
        public static bool LoadNSS(string mozillaPath)
        {
            if (!Environment.Is64BitProcess)
            {
                throw new BrowserEngineException(BrowserEngineError.ProcessIsNot64Bit, "The current process is 32-bit! To decrypt firefox values it needs to be 64-bit");
            }

            // Check if DLL exists.
            if (!File.Exists(mozillaPath + MozGlueDll))
            {
                throw new BrowserEngineException(BrowserEngineError.Nss3NotFound, $"MozGlue was not found: {mozillaPath + MozGlueDll}");
            }

            // Check if DLL exists.
            if (!File.Exists(mozillaPath + NssDll))
            {
                throw new BrowserEngineException(BrowserEngineError.Nss3NotFound, $"NSS3 was not found: {mozillaPath + NssDll}");
            }

            // Load libraries with the WinApi static class.
            hMozGlue = WinApi.LoadLibrary(mozillaPath + MozGlueDll); // This is necessary to make NSS3 work.
            hNss3 = WinApi.LoadLibrary(mozillaPath + NssDll);

            // Check if both libraries were loaded successfully.
            if (hMozGlue == IntPtr.Zero)
            {
                throw new BrowserEngineException(BrowserEngineError.MozGlueNotFound, $"{MozGlueDll} could not be found: {mozillaPath + MozGlueDll}");
            }

            if (hNss3 == IntPtr.Zero)
            {
                throw new BrowserEngineException(BrowserEngineError.Nss3NotFound, $"{NssDll} could not be found: {mozillaPath + NssDll}");
            }

            // Get addresses of functions.
            IntPtr ipNssInitAddr = WinApi.GetProcAddress(hNss3, "NSS_Init"); // NSS_Init()
            IntPtr ipNssPk11SdrDecrypt = WinApi.GetProcAddress(hNss3, "PK11SDR_Decrypt"); // PK11SDR_Decrypt()
            IntPtr ipNssShutdown = WinApi.GetProcAddress(hNss3, "NSS_Shutdown"); // NSS_Shutdown()

            // Check if all addresses were found.
            if (ipNssInitAddr == IntPtr.Zero)
            {
                throw new BrowserEngineException(BrowserEngineError.AddressNotFound, $"Process Address of NSS_Init was not found!");
            }

            if (ipNssPk11SdrDecrypt == IntPtr.Zero)
            {
                throw new BrowserEngineException(BrowserEngineError.AddressNotFound, $"Process Address of PK11SDR_Decrypt was not found!");
            }

            if (ipNssShutdown == IntPtr.Zero)
            {
                throw new BrowserEngineException(BrowserEngineError.AddressNotFound, $"Process Address of NSS_Shutdown was not found!");
            }

            // Get delegates from function pointers.
            fpNssInit = (Nss3.NssInit)Marshal.GetDelegateForFunctionPointer(ipNssInitAddr, typeof(Nss3.NssInit)); // NSS_Init()
            fpPk11SdrDecrypt = (Nss3.Pk11SdrDecrypt)Marshal.GetDelegateForFunctionPointer(ipNssPk11SdrDecrypt, typeof(Nss3.Pk11SdrDecrypt)); // PK11SDR_Decrypt()
            fpNssShutdown = (Nss3.NssShutdown)Marshal.GetDelegateForFunctionPointer(ipNssShutdown, typeof(Nss3.NssShutdown)); // NSS_Shutdown()

            // Check if all functions were found.
            if (fpNssInit == null)
            {
                throw new BrowserEngineException(BrowserEngineError.FunctionNotFound, $"Function 'NSS_Init()' was not found!");
            }
            if (fpPk11SdrDecrypt == null)
            {
                throw new BrowserEngineException(BrowserEngineError.FunctionNotFound, $"Function 'PK11SDR_Decrypt()' was not found!");
            }
            if (fpNssShutdown == null)
            {
                throw new BrowserEngineException(BrowserEngineError.FunctionNotFound, $"Function 'NSS_Shutdown()' was not found!");
            }

            // All functions were found.
            if (fpNssInit != null && fpPk11SdrDecrypt != null && fpNssShutdown != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Free Libraries and close NSS3.
        /// </summary>
        public static void UnLoadNSS()
        {
            fpNssShutdown();
            WinApi.FreeLibrary(hNss3);
            WinApi.FreeLibrary(hMozGlue);
        }

        /// <summary>
        /// Sets the Mozilla Firefox profile.
        /// </summary>
        /// <param name="path">
        /// Path to the Mozilla Firefox profile.
        /// </param>
        /// <returns>
        /// <c>True</c> if set successfully, <c>false</c> otherwise.
        /// </returns>
        public static bool SetProfile(string path)
        {
            return fpNssInit(path) == 0;
        }

        /// <summary>
        /// Decrypt an encrypted value with NSS3.
        /// </summary>
        /// <param name="value">
        /// The encrypted value.
        /// </param>
        /// <returns>
        /// The decrypted value, or <c>null</c> if decryption was unsuccessful.
        /// </returns>
        public static string DecryptValue(string value)
        {
            IntPtr lpMemory = IntPtr.Zero;

            try
            {
                // String from base 64.
                byte[] bPassDecoded = Convert.FromBase64String(value);

                // Allocate some memory.
                lpMemory = Marshal.AllocHGlobal(bPassDecoded.Length);
                Marshal.Copy(bPassDecoded, 0, lpMemory, bPassDecoded.Length);

                Nss3.TSECItem tsiOut = new Nss3.TSECItem();
                Nss3.TSECItem tsiItem = new Nss3.TSECItem
                {
                    SECItemType = 0,
                    SECItemData = lpMemory,
                    SECItemLen = bPassDecoded.Length
                };

                // Check if decrypted successfully.
                if (fpPk11SdrDecrypt(ref tsiItem, ref tsiOut, 0) == 0)
                {
                    if (tsiOut.SECItemLen != 0)
                    {
                        // Create a byte array and make space for the data.
                        byte[] bDecrypted = new byte[tsiOut.SECItemLen];
                        // copy tsiOut.SECItemData to bDecrypted.
                        Marshal.Copy(tsiOut.SECItemData, bDecrypted, 0, tsiOut.SECItemLen);

                        return Encoding.UTF8.GetString(bDecrypted);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BrowserEngineException(BrowserEngineError.UnknownError, ex.ToString());
            }
            finally
            {
                if (lpMemory != IntPtr.Zero)
                {
                    // Free the allocated memory.
                    Marshal.FreeHGlobal(lpMemory);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the Unicode Transformation Format 8 string.
        /// </summary>
        /// <remarks>
        /// This is useless. :)
        /// </remarks>
        /// <param name="sNonUtf8">
        /// The non-UTF-8 string.
        /// </param>
        /// <returns>
        /// UTF-8 string.
        /// </returns>
        public static string GetUTF8(string sNonUtf8)
        {
            try
            {
                byte[] bData = Encoding.Default.GetBytes(sNonUtf8);
                return Encoding.UTF8.GetString(bData);
            }
            catch { return sNonUtf8; }
        }
    }
}