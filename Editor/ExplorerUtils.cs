using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace HexTecGames.Basics.Editor
{
    public static class ExplorerUtils
    {
        private const int MAX_PATH = 260;

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        public static bool IsFolderOrParentOpen(string folderPath)
        {
            folderPath = Path.GetFullPath(folderPath)
                             .TrimEnd('\\')
                             .ToLowerInvariant();

            bool found = false;

            EnumWindows((hWnd, lParam) =>
            {
                uint pid;
                GetWindowThreadProcessId(hWnd, out pid);

                Process proc;
                try
                {
                    proc = Process.GetProcessById((int)pid);
                }
                catch
                {
                    return true;
                }

                if (!proc.ProcessName.Equals("explorer", StringComparison.OrdinalIgnoreCase))
                    return true;

                var sb = new StringBuilder(MAX_PATH);
                GetWindowText(hWnd, sb, MAX_PATH);
                string title = sb.ToString().Trim().ToLowerInvariant();

                if (string.IsNullOrEmpty(title))
                    return true;

                // Normalize Explorer window title to a path-like form
                string windowPath = title.Replace(" - file explorer", "")
                                         .TrimEnd('\\')
                                         .ToLowerInvariant();

                // Check exact match
                if (windowPath == folderPath)
                {
                    found = true;
                    return false;
                }

                // Check parent match
                if (folderPath.StartsWith(windowPath + "\\"))
                {
                    found = true;
                    return false;
                }

                return true;
            }, IntPtr.Zero);

            return found;
        }
    }
}