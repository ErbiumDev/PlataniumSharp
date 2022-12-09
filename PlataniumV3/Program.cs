global using Serilog;

﻿//Called on Startup
Console.Title = "PlataniumV3 | Made by GD";
Console.WriteLine("Welcome to PlataniumV3!");
Console.WriteLine("Setting Up...");

// logger 
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console()
    .CreateLogger();

static void ExceptionHandlerVOID(object sender, object e)
{
    PlataniumV3.Services.Proxy.Stop();
    PlataniumV3.Services.Launcher.CloseGame();
}

AppDomain.CurrentDomain.ProcessExit += new EventHandler(ExceptionHandlerVOID);
AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandlerVOID);
PlataniumV3.Services.Proxy.Start(); //Start Proxy
Console.WriteLine("Enter Game Path: ");
string Path = Console.ReadLine();
PlataniumV3.Services.Launcher.StartGame(Path);
Console.WriteLine("Press Enter to Close.");
Console.ReadLine();
PlataniumV3.Services.Proxy.Stop();
PlataniumV3.Services.Launcher.CloseGame();
