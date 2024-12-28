using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;

namespace PlataniumV3.Services
{
    public static class Launcher
    {
        public static void StartGame()
        {
            Process.Start("cmd.exe", "/C start com.epicgames.launcher://apps/Fortnite?action=launch"); //Start Fortnite
        }
    }
}
