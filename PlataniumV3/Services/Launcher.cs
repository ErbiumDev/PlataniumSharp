using System.Diagnostics;

namespace PlataniumV3.Services
{
    public static class Launcher
    {
        public static void StartGame(/*string Path*/)
        {
            Process.Start("cmd.exe", "/C start com.epicgames.launcher://apps/Fortnite?action=launch&silent=true"); //Start Fortnite
        }
    }
}
