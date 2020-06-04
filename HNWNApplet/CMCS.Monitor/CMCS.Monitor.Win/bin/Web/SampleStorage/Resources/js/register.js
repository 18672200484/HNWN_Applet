
/*存样柜管理*/

var SampleStorageV8Cef;
if (!SampleStorageV8Cef) SampleStorageV8Cef = {};

(function () {

    //切换存样柜选中
    SampleStorageV8Cef.ChangeSelected = function (paramSampler) {
        native function ChangeSelected(paramSampler);
        return ChangeSelected(paramSampler);
    };

    //显示存样柜详情
    SampleStorageV8Cef.ShowYGDetail = function (paramSampler, ygType) {
        native function ShowYGDetail(paramSampler, ygType);
        return ShowYGDetail(paramSampler, ygType);
    };

})(); 