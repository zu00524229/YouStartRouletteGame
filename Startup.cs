using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;

//告訴應用程式: 這個類別 Startup 是 OWIN 啟動類別
[assembly: OwinStartup(typeof(YSPFrom.Startup))]

namespace YSPFrom
{
    public class Startup    // 負責設定 OWIN 應用程式啟動流程
    {
        // Configuration 方法會在應用程式被呼叫
        public void Configuration(IAppBuilder app)
        {
            //  啟用 CORS（允許跨來源）
            app.UseCors(CorsOptions.AllowAll);

            // 將 "/signalr" 路徑對應到 SignalR Hub 的處理邏輯
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);  // 再次允許跨來源請求
                map.RunSignalR(new HubConfiguration // 啟動 SignalR 並設定詳細錯誤資訊開啟 (方便debug )
                {
                    EnableDetailedErrors = true // 若發生錯誤, 會回傳詳細堆疊
                });
            });
        }
    }
}
