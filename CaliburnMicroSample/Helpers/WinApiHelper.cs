namespace CaliburnMicroSample.Helpers
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// COPYDATASTRUCT
    /// 对应 C++ 里的 COPYDATASTRUCT
    /// 不能更改
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData; //可以是任意值
        public int cbData;    //指定lpData内存区域的字节数
        public IntPtr lpData; //发送给目录窗口所在进程的数据
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KeyBoardHookStruct
    {
        public int vkCode;
        public int scanCode;
        public int flags;
        public int time;
        public int dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SHORTDATASTRUCT
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string sender;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string content;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string tag;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DATASTRUCT
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string sender;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string content;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string tag;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LONGDATASTRUCT
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string sender;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string content;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string tag;
    }

    /// <summary>
    /// 消息定义
    /// </summary>
    public static class MSG_DEF
    {
        /// <summary>
        /// Windows 消息
        /// 不能更改值
        /// </summary>
        public const int WH_KEYBOARD_LL = 0xD;
        public const int WM_COPYDATA = 0x004A;
        public const int WM_KETDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WN_SYSKEYDOWN = 0x104;
        public const int WM_SYSKEYUP = 0x105;
        public const int WN_USER = 0x400;
    }

    public static class WinApiHelper
    {
        [DllImport("User32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("User32.dll", EntryPoint = "IsWindow")]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("User32.dll", EntryPoint = "GetWindowText")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("User32.dll", EntryPoint = "SetWindowText")]
        public static extern bool SetWindowText(IntPtr hWnd, string lpString);

        [DllImport("User32.dll", EntryPoint = "ShowWindow")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("User32.dll", EntryPoint = "UpdateWindow")]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll", EntryPoint = "GetParent")]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("User32.dll", EntryPoint = "SetParent")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("User32.dll", EntryPoint = "SetWindowPos")]
        static public extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("User32.dll", EntryPoint = "MoveWindow")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        // SendMessage method, using Windows API
        [DllImport("User32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        public static extern int SendMessage(
            IntPtr hWnd,                // handle to destination window
            int Msg,                    // message
            int wParam,                 // first message parameter
            int lParam                  // second message parameter
        );

        // SendMessage method, using Windows API
        [DllImport("User32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        public static extern int SendMessage(
            IntPtr hWnd,                // handle to destination window
            int Msg,                    // message
            IntPtr wParam,              // first message parameter
            IntPtr lParam               // second message parameter
        );

        //消息发送API
        [DllImport("User32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto)]
        public static extern int PostMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
            int wParam,         // 参数1
            int lParam          // 参数2
        );

        //消息发送API
        [DllImport("User32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto)]
        public static extern int PostMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
            IntPtr wParam,      // 参数1
            IntPtr lParam       // 参数2
        );

        [DllImport("User32.dll", EntryPoint = "PostThreadMessage", CharSet = CharSet.Auto)]
        public static extern bool PostThreadMessage(uint idThread, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll", EntryPoint = "PostQuitMessage", CharSet = CharSet.Auto)]
        public static extern void PostQuitMessage(int nExitCode);

        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        //移除钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //把信息传递到下一个监听
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", EntryPoint = "GetModuleHandle")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("User32.dll", EntryPoint = "ToAscii")]
        public static extern int ToAscii(int uVirtKey, //[in] 指定虚拟关键代码进行翻译。
                                         int uScanCode, // [in] 指定的硬件扫描码的关键须翻译成英文。高阶位的这个值设定的关键，如果是（不压）
                                         byte[] lpbKeyState, // [in] 指针，以256字节数组，包含当前键盘的状态。每个元素（字节）的数组包含状态的一个关键。如果高阶位的字节是一套，关键是下跌（按下）。在低比特，如果设置表明，关键是对切换。在此功能，只有肘位的CAPS LOCK键是相关的。在切换状态的NUM个锁和滚动锁定键被忽略。
                                         byte[] lpwTransKey, // [out] 指针的缓冲区收到翻译字符或字符。
                                         int fuState); // [in] Specifies whether a menu is active. This parameter must be 1 if a menu is active, or 0 otherwise.

        [DllImport("User32.dll", EntryPoint = "GetDlgCtrlID")]
        public static extern int GetDlgCtrlID(IntPtr hWnd);

        [DllImport("User32.dll", EntryPoint = "GetDlgItem")]
        public static extern IntPtr GetDlgItem(IntPtr hWnd, int nIDDlgItem);

        [DllImport("User32.dll", EntryPoint = "SetCursorPos")]
        public static extern bool SetCursorPos(int x, int y);
    }
}
