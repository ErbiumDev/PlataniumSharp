using System.Diagnostics;

namespace PlataniumV3.Services
{
    public static class Launcher
    {
        public static void StartGame(/*string Path*/)
        {
            Process.Start("cmd.exe", "/C start com.epicgames.launcher://apps/fn%3A4fe75bbc5a674f4f9b356b5c90567da5%3AFortnite?action=launch&silent=true"); //Start Fortnite
        }
    }
}
