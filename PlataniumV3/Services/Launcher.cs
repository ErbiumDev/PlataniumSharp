using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;

namespace PlataniumV3.Services
{
    public static class Launcher
    {
        [Flags]
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);


        private static void SuspendProcess(int pid)
        {
            var process = Process.GetProcessById(pid);

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                SuspendThread(pOpenThread);

                CloseHandle(pOpenThread);
            }
        }

        public static void ResumeProcess(int pid)
        {
            var process = Process.GetProcessById(pid);

            if (process.ProcessName == string.Empty)
                return;

            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                var suspendCount = 0;
                do
                {
                    suspendCount = ResumeThread(pOpenThread);
                } while (suspendCount > 0);

                CloseHandle(pOpenThread);
            }
        }

        private static readonly byte[] VerifyPeerPatch = { 0x41, 0x39, 0x28, 0x0F, 0x94, 0xC0, 0x88};
        private static Process? P_Launcher;
        private static Process? P_EAC;
        private static Process? FN;

        public static void StartGame()
        {
            //P_Launcher = Process.Start(Path + "\\FortniteGame\\Binaries\\Win64\\FortniteLauncher.exe", "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -nobe -fromfl=eac -fltoken=none -caldera=eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2NvdW50X2lkIjoiXHUwMDNjbnZpZGlhXHUwMDNlIiwiZ2VuZXJhdGVkIjoxNjY3ODkwMDM1LCJjYWxkZXJhR3VpZCI6IjVlMGEzZmYxLTI4MWEtNDYwNS1iZDhlLWJjMjUxZjg3NzA1MyIsImFjUHJvdmlkZXIiOiJFYXN5QW50aUNoZWF0Iiwibm90ZXMiOiI4MzFhNDkzYy1kYzYxLTQ0NTgtYjI1YS05OGYwZjMxMTUzMTgiLCJmYWxsYmFjayI6ZmFsc2V9.qESXPMaacHpGwK_OPxN2DR-NYel-y1e9mGYT8oJX3bXn099f16cAy4C5l-6q9R7_wlHTVLFypOyIy3_5IM4FHA -skippatchcheck -AUTH_TYPE=epic -AUTH_PASSWORD=Test -httpproxy=127.0.0.1:8888 -AUTH_LOGIN=" + Username + "@.");
            //SuspendProcess(P_Launcher.Id);
            //P_EAC = Process.Start(Path + "\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping_EAC.exe", "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -nobe -fromfl=eac -fltoken=none -caldera=eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2NvdW50X2lkIjoiXHUwMDNjbnZpZGlhXHUwMDNlIiwiZ2VuZXJhdGVkIjoxNjY3ODkwMDM1LCJjYWxkZXJhR3VpZCI6IjVlMGEzZmYxLTI4MWEtNDYwNS1iZDhlLWJjMjUxZjg3NzA1MyIsImFjUHJvdmlkZXIiOiJFYXN5QW50aUNoZWF0Iiwibm90ZXMiOiI4MzFhNDkzYy1kYzYxLTQ0NTgtYjI1YS05OGYwZjMxMTUzMTgiLCJmYWxsYmFjayI6ZmFsc2V9.qESXPMaacHpGwK_OPxN2DR-NYel-y1e9mGYT8oJX3bXn099f16cAy4C5l-6q9R7_wlHTVLFypOyIy3_5IM4FHA -skippatchcheck -AUTH_TYPE=epic -AUTH_PASSWORD=Test -httpproxy=127.0.0.1:8888 -AUTH_LOGIN=" + Username + "@.");
            //SuspendProcess(P_EAC.Id);
            //FN = Process.Start(Path + "\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe", "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -nobe -fromfl=eac -fltoken=none -caldera=eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2NvdW50X2lkIjoiXHUwMDNjbnZpZGlhXHUwMDNlIiwiZ2VuZXJhdGVkIjoxNjY3ODkwMDM1LCJjYWxkZXJhR3VpZCI6IjVlMGEzZmYxLTI4MWEtNDYwNS1iZDhlLWJjMjUxZjg3NzA1MyIsImFjUHJvdmlkZXIiOiJFYXN5QW50aUNoZWF0Iiwibm90ZXMiOiI4MzFhNDkzYy1kYzYxLTQ0NTgtYjI1YS05OGYwZjMxMTUzMTgiLCJmYWxsYmFjayI6ZmFsc2V9.qESXPMaacHpGwK_OPxN2DR-NYel-y1e9mGYT8oJX3bXn099f16cAy4C5l-6q9R7_wlHTVLFypOyIy3_5IM4FHA -skippatchcheck -AUTH_TYPE=epic -AUTH_PASSWORD=Test -httpproxy=127.0.0.1:8888 -AUTH_LOGIN=" + Username + "@.");
            //FN.WaitForInputIdle();
            //SuspendProcess(FN.Id);
            //var SS = new SigScan(SigScan.OpenProcess(SigScan.PROCESS_ALL_ACCESS, false, FN.Id));
            //SS.SelectModule(FN.MainModule);
            //var Addr = SS.FindPattern("41 39 28 0F 95 C0 88 87 ? ? ? ? 48 8B", out long Time);
            //if (Addr == 0)
            //{
            //    //Older Versions
            //    Addr = SS.FindPattern("41 39 28 0F 95 C0 88 83 50 04", out Time);
            //}
            //SigScan.WriteProcessMemory(SigScan.OpenProcess(SigScan.PROCESS_ALL_ACCESS, false, FN.Id), (IntPtr)Addr, VerifyPeerPatch, VerifyPeerPatch.Length, out IntPtr bytesWritten);
            //Log.Information("Addr:" + Addr);
            
            //ResumeProcess(FN.Id);
            Process.Start("cmd.exe", "/C start com.epicgames.launcher://apps/Fortnite?action=launch"); //Start Fortnite
        }

        public static void CloseGame()
        {
            //P_Launcher.Kill();
            //P_EAC.Kill();
            //FN.Kill();
        }
    }
}
