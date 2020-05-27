 
/*全自动制样机监控界面*/
 
 var AutoMakerV8Cef;
    if (!AutoMakerV8Cef)AutoMakerV8Cef = {};

    (function() {
      
      //查看故障信息
      AutoMakerV8Cef.GetHitchs=function(paramSampler){
      native function GetHitchs(paramSampler);
      return GetHitchs(paramSampler);
      };

      // 打开全自动制样机监控界面
      AutoMakerV8Cef.OpenAutoMaker = function(paramSampler) {
        native function OpenAutoMaker(paramSampler);
        OpenAutoMaker(paramSampler);
      };  

    })(); 