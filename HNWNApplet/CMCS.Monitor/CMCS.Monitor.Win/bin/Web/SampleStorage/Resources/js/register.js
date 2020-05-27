 
/*存样柜管理*/

 var SampleStorageV8Cef;
    if (!SampleStorageV8Cef)SampleStorageV8Cef = {};
    
    (function() {

       //切换存样柜选中
      SampleStorageV8Cef.ChangeSelected=function(paramSampler){
      native function ChangeSelected(paramSampler);
      return ChangeSelected(paramSampler);
      };

    })(); 