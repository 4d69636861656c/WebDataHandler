namespace BrowserDataFetcher
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// The Windows application programming interface references.
    /// </summary>
    internal sealed class WinApi
    {
        /// <summary>
        /// Loads the specified module into the address space of the calling process.
        /// </summary>
        /// <remarks>
        /// The specified module may cause other modules to be loaded.
        /// </remarks>
        /// <param name="sFileName">
        /// The name of the module. This can be either a library module (a .dll file) or an executable module (an .exe file).
        /// If the specified module is an executable module, static imports are not loaded; instead, the module is loaded as
        /// if by LoadLibraryEx with the <c>DONT_RESOLVE_DLL_REFERENCES</c> flag.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is a handle to the module.
        /// If the function fails, the return value is <c>null</c>.
        /// </returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr LoadLibrary(string sFileName);

        /// <summary>
        /// Frees the loaded dynamic-link library (DLL) module and, if necessary, decrements its reference count.
        /// </summary>
        /// <remarks>
        /// When the reference count reaches zero, the module is unloaded from the address space of the calling process and the handle is no longer valid.
        /// </remarks>
        /// <param name="hModule">
        /// A handle to the loaded library module.
        /// The <c>LoadLibrary</c>, <c>LoadLibraryEx</c>, <c>GetModuleHandle</c>, or <c>GetModuleHandleEx</c> function returns this handle.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FreeLibrary(IntPtr hModule);

        /// <summary>
        /// Retrieves the address of an exported function (also known as a procedure) or variable from the specified dynamic-link library (DLL).
        /// </summary>
        /// <param name="hModule">
        /// A handle to the DLL module that contains the function or variable.
        /// The <c>LoadLibrary</c>, <c>LoadLibraryEx</c>, <c>LoadPackagedLibrary</c>, or <c>GetModuleHandle</c> function returns this handle.
        /// </param>
        /// <param name="sProcName">
        /// The function or variable name, or the function's ordinal value.
        /// If this parameter is an ordinal value, it must be in the low-order word; the high-order word must be zero.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is the address of the exported function or variable.
        /// If the function fails, the return value is <c>null</c>.
        /// </returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string sProcName);
    }
}