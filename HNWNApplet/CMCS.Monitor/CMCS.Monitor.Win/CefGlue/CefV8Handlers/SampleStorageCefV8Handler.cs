using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xilium.CefGlue;

namespace CMCS.Monitor.Win.CefGlue.CefV8Handlers
{
    /// <summary>
    /// 存样柜管理 CefV8Handler
    /// </summary>
    public class SampleStorageCefV8Handler : CefV8Handler
    {
        protected override bool Execute(string name, CefV8Value obj, CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
        {
            exception = null;
            returnValue = null;
            string paramSampler = arguments[0].GetStringValue();

            switch (name)
            {
                case "ChangeSelected":
                    CefProcessMessage cefMsg = CefProcessMessage.Create("ChangeSelected");
                    cefMsg.Arguments.SetSize(0);
                    cefMsg.Arguments.SetString(0, paramSampler);
                    CefV8Context.GetCurrentContext().GetBrowser().SendProcessMessage(CefProcessId.Browser, cefMsg);
                    break;
                default:
                    returnValue = null;
                    break;
            }

            return true;
        }
    }
}
