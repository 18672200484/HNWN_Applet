 
/*汽车衡监控界面*/
 
 var CarSamplerV8Cef;
    if (!CarSamplerV8Cef)CarSamplerV8Cef = {};

    (function() {
        // 急停
      CarSamplerV8Cef.Stop = function(paramSampler) {
        native function Stop(paramSampler);
        return Stop(paramSampler);
      }; 
      
        // 车辆信息
      CarSamplerV8Cef.CarInfo = function(paramSampler) {
        native function CarInfo(paramSampler);
        CarInfo(paramSampler);
      };   
      
        // 故障复位
      CarSamplerV8Cef.ErrorReset = function(paramSampler) {
        native function ErrorReset(paramSampler);
        ErrorReset(paramSampler);
      };   
      
        // 采样历史记录
      CarSamplerV8Cef.SampleHistory = function(paramSampler) {
        native function SampleHistory(paramSampler);
        SampleHistory(paramSampler);
      };   

      //查看故障信息
      CarSamplerV8Cef.GetHitchs=function(paramSampler){
      native function GetHitchs(paramSampler);
      return GetHitchs(paramSampler);
      };

      //切换采样机选中
      CarSamplerV8Cef.ChangeSelected=function(paramSampler){
      native function ChangeSelected(paramSampler);
      return ChangeSelected(paramSampler);
      };

    })(); 