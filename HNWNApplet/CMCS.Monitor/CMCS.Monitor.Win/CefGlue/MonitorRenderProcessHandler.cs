using System;
using System.Collections.Generic;
using System.Text;
//
using Xilium.CefGlue;
using Xilium.CefGlue.Wrapper;
using CMCS.Monitor.Win.CefGlue;
using CMCS.Monitor.Win.CefGlue.CefV8Handlers;

namespace Xilium.CefGlue.Demo
{
    public class MonitorRenderProcessHandler : CefRenderProcessHandler
    {
        protected override void OnWebKitInitialized()
        {
            // 注册CefTester脚本
            CefRuntime.RegisterExtension("CefTester.Register", System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Web\CefTester\Resources\js", "register.js")), new CefTesterCefV8Handler());

            // 注册集中管控脚本
            CefRuntime.RegisterExtension("HomePage.Register", System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Web\HomePage\Resources\js", "register.js")), new HomePageCefV8Handler());
            // 注册汽车衡脚本
            CefRuntime.RegisterExtension("TruckWeighter.Register", System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Web\TruckWeighter\Resources\js", "register.js")), new TruckWeighterCefV8Handler());
            // 注册汽车采样机脚本
            CefRuntime.RegisterExtension("CarSampler.Register", System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Web\CarSampler\Resources\js", "register.js")), new CarSamplerCefV8Handler());
            // 注册汽车监控脚本
            CefRuntime.RegisterExtension("CarMonitor.Register", System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Web\CarMonitor\Resources\js", "register.js")), new CarMonitorCefV8Handler());

            // 注册存样柜管理脚本
            CefRuntime.RegisterExtension("SampleStorage.Register", System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Web\SampleStorage\Resources\js", "register.js")), new SampleStorageCefV8Handler());

            base.OnWebKitInitialized();
        }

        protected override void OnContextCreated(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            CefV8Value cefV8Value = context.GetGlobal();
            cefV8Value.SetValue("appName", CefV8Value.CreateString("CMCS.Monitor.Win"), CefV8PropertyAttribute.ReadOnly);

            base.OnContextCreated(browser, frame, context);
        }

        protected override bool OnProcessMessageReceived(CefBrowser browser, CefProcessId sourceProcess, CefProcessMessage message)
        {
            return base.OnProcessMessageReceived(browser, sourceProcess, message);
        }
    }
}
