using Fiddler;
using static Fiddler.FiddlerApplication;
namespace PlataniumV3.Services
{
    public static class Proxy
    {
        public static void BeforeReq(Session Ses)
        {
            if(Ses.hostname.Contains("ol.epicgames.com"))
            {
                //Credits to Lawin Server <3
                Console.WriteLine("[PROXY] Redirected: " + Ses.PathAndQuery);
                if (Ses.HTTPMethodIs("CONNECT"))
                {
                    Ses["x-replywithtunnel"] = "FortniteTunnel";
                    return;
                }
                
                Ses.fullUrl = "https://lawinserver.milxnor.repl.co" + Ses.PathAndQuery;
            }
        }

        public static void Start()
        {
            Console.WriteLine("Starting Proxy...");
            FiddlerCoreStartupSettings Settings = new FiddlerCoreStartupSettingsBuilder().ListenOnPort(8888).RegisterAsSystemProxy().OptimizeThreadPool().DecryptSSL().Build();
            BeforeRequest += BeforeReq;
            Startup(Settings);
            Console.WriteLine("Proxy Started!");
        }

        public static void Stop()
        {
            Console.WriteLine("Stopping Proxy...");
            Shutdown();
            Console.WriteLine("Proxy Stopped!");
        }
    }
}
