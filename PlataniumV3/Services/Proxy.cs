using Fiddler;
using static Fiddler.FiddlerApplication;
namespace PlataniumV3.Services
{
    public static class Proxy
    {
        public static void BeforeReq(Session Ses)
        {
            if (Ses.hostname.Contains("epicgames.") || Ses.PathAndQuery.Contains("epic-settings"))
            {
                //Credits to LawinServer
                if (Ses.HTTPMethodIs("CONNECT"))
                {
                    Ses["x-replywithtunnel"] = "FortniteTunnel";
                    return;
                }
                else
                {
                    Serilog.Log.Information("[PROXY] Redirected: " + Ses.PathAndQuery);
                }

                Ses.fullUrl = "http://127.0.0.1:5595" + Ses.PathAndQuery;
            }
        }

        //Fixes a weird WebSocket crash
        public static new void WSReq(object sender, WebSocketMessageEventArgs ARGS)
        {
            ARGS.oWSM.Abort();
            return;
        }

        public static void Start()
        {
            Serilog.Log.Information("Setting up Cert...");
            if (!CertMaker.rootCertIsTrusted())
            {
                if (!CertMaker.createRootCert())
                {
                    Serilog.Log.Error("Failed to Create Cert!");
                    return;
                }
                if (!CertMaker.rootCertIsTrusted())
                {
                    CertMaker.trustRootCert();
                }
            }
            Serilog.Log.Information("Starting Proxy...");
            FiddlerCoreStartupSettings Settings = new FiddlerCoreStartupSettingsBuilder().ListenOnPort(8888).OptimizeThreadPool().DecryptSSL().RegisterAsSystemProxy().Build();
            BeforeRequest += BeforeReq;
            OnWebSocketMessage += WSReq;
            Startup(Settings);
            Serilog.Log.Information("Proxy Started!");
        }

        public static void Stop()
        {
            Serilog.Log.Information("Stopping Proxy...");
            Shutdown();
            Serilog.Log.Information("Proxy Stopped!");
        }
    }
}
