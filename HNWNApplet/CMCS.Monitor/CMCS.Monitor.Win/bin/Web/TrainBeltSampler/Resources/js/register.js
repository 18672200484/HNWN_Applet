 
/*皮带采样监控界面*/
 
 var TrainBeltSamplerV8Cef;
    if (!TrainBeltSamplerV8Cef)TrainBeltSamplerV8Cef = {};

    (function() {
        // 设置
      TrainBeltSamplerV8Cef.SubmitSet = function(paramSampler1,paramSampler2) {
        native function SubmitSet(paramSampler1,paramSampler2);
        SubmitSet(paramSampler1,paramSampler2);
      };   
      
      //查看故障信息
      TrainBeltSamplerV8Cef.GetHitchs=function(paramSampler){
      native function GetHitchs(paramSampler);
      return GetHitchs(paramSampler);
      };

      //查看采样记录
      TrainBeltSamplerV8Cef.ShowSamplePlan=function(){
      native function ShowSamplePlan();
      ShowSamplePlan();
      };
      
      //查看火车门式采样记录
      TrainBeltSamplerV8Cef.ShowHCSamplePlan=function(){
      native function ShowHCSamplePlan();
      ShowHCSamplePlan();
      };

    })(); 