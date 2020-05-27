 
/*汽车监控界面*/
 
 var CarMonitorV8Cef;
    if (!CarMonitorV8Cef)CarMonitorV8Cef = {};

    (function() {
        // 发送控制指令
      CarMonitorV8Cef.SendCmd = function(paramSampler) {
        native function SendCmd(paramSampler);
        SendCmd(paramSampler);
      };   
      
    })(); 