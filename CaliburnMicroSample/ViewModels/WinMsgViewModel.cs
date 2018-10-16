namespace CaliburnMicroSample.ViewModels
{
    using Caliburn.Micro;
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Text;
    using Helpers;
    using Services;
    using Views;

    public class WinMsgViewModel : Screen, ICleanup
    {
        protected override void OnInitialize()
        {
            (GetView() as WinMsgView).Owner = System.Windows.Application.Current.MainWindow;
        }

        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Handle messages...
            switch (msg)
            {
                case MSG_DEF.WM_COPYDATA:
                    COPYDATASTRUCT param = Marshal.PtrToStructure<COPYDATASTRUCT>(lParam);
                    // 不使用自定义结构体
                    // byte[] buffer = new byte[param.cbData];
                    // Marshal.Copy(param.lpData, buffer, 0, param.cbData);
                    // string content = Encoding.Default.GetString(buffer);

                    // 自定义结构体
                    MYDATASTRUCT data = Marshal.PtrToStructure<MYDATASTRUCT>(param.lpData);
                    string content = data.content;
                    // TODO: 数据处理
#if DEBUG
                    Debug.WriteLine(content);
#endif
                    break;
                default:
                    break;
            }

            return IntPtr.Zero;
        }

        /* C++ 中相关代码
         * 向本窗口发送消息
         ...
         typedef struct tagMYDATASTRUCT
         {
             char content[512];
         } MYDATASTRUCT, * PMYDATASTRUCT;
         ...
         MYDATASTRUCT data;
         memset(data.content, 0, 512);
         memcpy(data.content, "来自 C++ MFC", 512);

         COPYDATASTRUCT cds;
         cds.dwData = 0;
         cds.cbData = sizeof(MYDATASTRUCT);
         cds.lpData = &data;

         HWND hWnd = ::FindWindow(NULL, L"Message Api 窗口");
         ::SendMessage(hWnd, WM_COPYDATA, 0, (LPARAM)&cds);
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
            MYDATASTRUCT myDATASTRUCT = new MYDATASTRUCT() { content = "来自 C# WPF" };
            // 将数据拷贝至非托管内存
            IntPtr myDataPtr = Marshal.AllocHGlobal(Marshal.SizeOf(myDATASTRUCT));
            Marshal.StructureToPtr(myDATASTRUCT, myDataPtr, true);

            COPYDATASTRUCT copyData = new COPYDATASTRUCT()
            {
                dwData = IntPtr.Zero,
                cbData = Marshal.SizeOf(myDATASTRUCT),
                lpData = myDataPtr
            };

            IntPtr cpDataPtr = Marshal.AllocHGlobal(Marshal.SizeOf(copyData));
            Marshal.StructureToPtr(copyData, cpDataPtr, true);

            WinApiHelper.SendMessage(hWnd, MSG_DEF.WM_COPYDATA, IntPtr.Zero, cpDataPtr);

            // 释放掉相应的非托管内存数据
            Marshal.FreeHGlobal(myDataPtr);
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
        }
    }
}
