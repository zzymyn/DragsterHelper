using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace DragsterHelper
{
    public static class Win32
    {
        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Keys vKey);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapVirtualKeyMapTypes uMapType);

        public enum MapVirtualKeyMapTypes : uint
        {
            MAPVK_VK_TO_VSC = 0x00,
            MAPVK_VSC_TO_VK = 0x01,
            MAPVK_VK_TO_CHAR = 0x02,
        }

        public static ushort MapVirtualKeyToScanCode(Keys key)
        {
            return (ushort)MapVirtualKey((uint)key, MapVirtualKeyMapTypes.MAPVK_VK_TO_VSC);
        }

        public static string GetWindowText(IntPtr hWnd)
        {
            var sb = new StringBuilder(256);
            GetWindowText(hWnd, sb, 256);
            return sb.ToString();
        }

        public static string GetClassName(IntPtr hWnd)
        {
            var sb = new StringBuilder(256);
            GetClassName(hWnd, sb, 256);
            return sb.ToString();
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}
