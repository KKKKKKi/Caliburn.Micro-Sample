namespace CaliburnMicroSample.ViewModels
{
    using Caliburn.Micro;
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Helpers;
    using Services;
    using Views;
    using System.Windows.Input;

    public class WinMsgViewModel : Screen, ICleanup
    {
        private WinApiHelper.HookProc _keyBoardHookDelegate;
        private int hHook;

        protected override void OnInitialize()
        {
            (GetView() as WinMsgView).Owner = System.Windows.Application.Current.MainWindow;
            _keyBoardHookDelegate = new WinApiHelper.HookProc(KeyBoardHookProc);
            ProcessModule cModule = Process.GetCurrentProcess().MainModule;
            IntPtr hm = WinApiHelper.GetModuleHandle(cModule.ModuleName);
            hHook = WinApiHelper.SetWindowsHookEx(MSG_DEF.WH_KEYBOARD_LL, _keyBoardHookDelegate, hm, 0);
        }

        private int KeyBoardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
                return -1;

            KeyBoardHookStruct keyDataFromHook = Marshal.PtrToStructure<KeyBoardHookStruct>(lParam);
            int keyData = keyDataFromHook.vkCode;

            if (wParam.ToInt32() == MSG_DEF.WM_KETDOWN || wParam.ToInt32() == MSG_DEF.WN_SYSKEYDOWN)
            {
                Key key = KeyInterop.KeyFromVirtualKey(keyData);
            }

            if (wParam.ToInt32() == MSG_DEF.WM_KEYUP || wParam.ToInt32() == MSG_DEF.WN_SYSKEYDOWN)
            {
                //
            }

            return WinApiHelper.CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Handle messages...
            switch (msg)
            {
                case MSG_DEF.WM_COPYDATA:
                    {
                        COPYDATASTRUCT param = Marshal.PtrToStructure<COPYDATASTRUCT>(lParam);
                        // 不使用自定义结构体
                        // byte[] buffer = new byte[param.cbData];
                        // Marshal.Copy(param.lpData, buffer, 0, param.cbData);
                        // string content = Encoding.Default.GetString(buffer);

                        // 自定义结构体
                        LONGDATASTRUCT data = Marshal.PtrToStructure<LONGDATASTRUCT>(param.lpData);
                        // TODO: 数据处理
#if DEBUG
                        Debug.WriteLine($"Received: \n\tsender: {data.sender}\n\tcontent: {data.content}\n\ttag: {data.tag}");
#endif
                        handled = true;
                    break;
                    }
                default:
                    break;
            }

            return IntPtr.Zero;
        }

        /* C++ 中相关代码
         * 向本窗口发送消息
         ...
         typedef struct tagLONGDATASTRUCT
         {
         char sender[256];
		 char content[1024];
		 char tag[256];
         } LONGDATASTRUCT, * PLONGDATASTRUCT;
         ...
         LONGDATASTRUCT data;
         LPCSTR sender = "nobu";
         memset(data.sender, 0, 256);
         memcpy(data.sender, sender, strlen(sender));
         LPCSTR content = "你好啊";
         memset(data.content, 0, 1024);
         memcpy(data.content, content, strlen(content));
         LPCSTR tag = "来自 C++ MFC";
         memset(data.tag, 0, 256);
         memcpy(data.tag, tag, strlen(tag));

         COPYDATASTRUCT copyData;
         copyData.dwData = 0;
         copyData.cbData = sizeof(data);
         copyData.lpData = &data;

         HWND hWnd = ::FindWindow(NULL, L"Message Api 窗口");

         ::SendMessage(hWnd, WM_COPYDATA, NULL, (LPARAM)&copyData);
         ...
         */

        public bool CanSendMsg(string windowName) => !string.IsNullOrEmpty(windowName);

        public void SendMsg(string windowName)
        {
            IntPtr hWnd = WinApiHelper.FindWindow(null, windowName);
            if (hWnd == IntPtr.Zero)
            {
                return;
            }
            // WinApiHelper.SetWindowText(hWnd, "Hello");
            // WinApiHelper.ShowWindow(hWnd, 5);
            // WinApiHelper.SetForegroundWindow(hWnd);
            // WinApiHelper.SendMessage(hWnd, 0x1081, 0, "msg");

            LONGDATASTRUCT longData = new LONGDATASTRUCT()
            {
                sender = System.Windows.Application.Current.MainWindow.Title,
                content = "你好。",
                tag = "来自 C# WPF"
            };
            // 将数据拷贝至非托管内存
            IntPtr longDataPtr = Marshal.AllocHGlobal(Marshal.SizeOf(longData));
            Marshal.StructureToPtr<LONGDATASTRUCT>(longData, longDataPtr, true);

            COPYDATASTRUCT copyData = new COPYDATASTRUCT()
            {
                dwData = IntPtr.Zero,
                cbData = Marshal.SizeOf(longData),
                lpData = longDataPtr,
            };

            IntPtr cpDataPtr = Marshal.AllocHGlobal(Marshal.SizeOf(copyData));
            Marshal.StructureToPtr(copyData, cpDataPtr, true);

            WinApiHelper.SendMessage(hWnd, MSG_DEF.WM_COPYDATA, IntPtr.Zero, cpDataPtr);

            // 释放掉相应的非托管内存数据
            Marshal.FreeHGlobal(longDataPtr);
            Marshal.FreeHGlobal(cpDataPtr);
        }

        /* C++ 中相关代码
         * 处理 WM_COPYDATA 消息
         * Header File(.h)
         ---------------------------------------------------------------------
         ...
         afx_msg BOOL OnCopyData(CWnd *pWnd, COPYDATASTRUCT *pCopyDataStruct);
         ...
         DECLARE_MESSAGE_MAP()
         ---------------------------------------------------------------------

         * Source File(.cpp)
         BEGIN_MESSAGE_MAP(CxxxDlg, CDialogEx)
            ...
            ON_WM_COPYDATA()
            ...
         END_MESSAGE_MAP()
         ...
         ...
         ...
         BOOL CxxxDlg::OnCopyData(CWnd *pWnd, COPYDATASTRUCT *pCopyDataStruct)
         {
             if (pCopyDataStruct != NULL)
             {
                 PMYDATASTRUCT data = (PMYDATASTRUCT)(pCopyDataStruct->lpData);
                 DWORD dwLen = pCopyDataStruct->cbData;
                 char buffer[512] = { 0 };
                 memcpy(buffer, data->content, 512);
                 // TODO: 数据处理
                 TRACE(buffer);
             }

             return CDialogEx::OnCopyData(pWnd, pCopyDataStruct);
         }
         */

        public void Cleanup()
        {
            // TODO: Cleanup
            WinApiHelper.UnhookWindowsHookEx(hHook);
        }
    }
}
