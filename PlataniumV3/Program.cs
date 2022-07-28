﻿//Called on Startup

Console.Title = "PlataniumV3 | Made by GD";
Console.WriteLine("Welcome to PlataniumV3!");
Console.WriteLine("Setting Up...");
static void ExceptionHandlerVOID(object sender, object e)
{
    PlataniumV3.Services.Proxy.Stop();
}
AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandlerVOID);
PlataniumV3.Services.Launcher.StartGame();
PlataniumV3.Services.Proxy.Start(); //Start Proxy
Console.WriteLine("Press Enter to Close.");
Console.ReadLine();
PlataniumV3.Services.Proxy.Stop();