using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace HikISCApi.Core
{
    #region [结构体]


    [StructLayout(LayoutKind.Sequential)]
    public struct VIDEO_PLAY_REQ
    {
        public int iHardWareDecode;  // 是否开启GPU硬解 0-不开启 1-开启（如果开启硬解，但如显卡不支持等导致错误，SDK内部自动切换成软解；开启硬解只是增加显示性能，并不能减少内存占用率，如果不是特别需求，建议不开启硬解）

        public pfnStreamCallback fnStream;              // 码流数据回调

        public pfnMsgCallback fnMsg;                    // 取流、播放消息回调

        public pfnDecodedDataCallback fnDecodedStream;  // 解码后的YUV数据回调

        public IntPtr pUserData;                         // 用户数据

        //保留参数
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] szResevre;
    }

    // 录像下载请求结构体
    [StructLayout(LayoutKind.Sequential)]
    public struct VIDEO_DOWNLOAD_REQ
    {
        public pfnDownloadCallback fnDownload;                // 下载进度回调 

        public IntPtr pUserData;                       // 用户数据

        public long i64FileMaxSize;                    // 录像分包大小

        public long i64RecordSize;                     // 录像总大小，请填充查询回放URL时查询到的各录像片段大小之和

        public long i64StartTimeStamp;                 // 录像下载开始时间，请请填充查询回放URL时的查询开始时间戳，单位：秒

        public long i64EndTimeStamp;                   // 录像下载结束时间，请请填充查询回放URL时的查询结束时间戳，单位：秒

        //char szReserve[64];                            // 保留参数
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] szResevre;
    }


    // 画面字符叠加结构体
    [StructLayout(LayoutKind.Sequential)]
    public struct VIDEO_OSD_INFO
    {
        public int iBold;              // 是否粗体，0-一般 1-粗体，暂不支持，请填充0

        public int ix;                 // 叠加字符相对与播放窗口左上角起点的横坐标

        public int iy;                 // 叠加字符相对与播放窗口左上角起点的纵坐标

        public int iFontSize;          // 字体大小，暂不支持，请填充0

        public int iAlignType;         // 多行情况下字符字符对齐方式，0-左对齐 1-居中 2-右对齐，暂不支持，请填充0

        public long i64Color;   // 字符颜色，Windows中可使用RGB宏获得颜色值

        //char szReserve[64];     // 保留参数 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] szResevre;
    }


    // 视频详情结构体
    public struct VIDEO_DETAIL_INFO
    {
        public int iWidth;                 // 视频图像宽度

        public int iHeight;                // 视频图像高度

        public int iEncodeType;            // 编码类型  1-H264 2-MPEG2_PS 3-MPEG4 4-H265 5-GB28181 6-raw

        public int iEncapsulationType;     // 封装类型  1-ps 2-ts 3-rtp 4-raw 5-rtp + ps

        public long i64FrameRate;   // 帧率

        //char szReserve[64];         // 保留参数
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] szResevre;
    }

    #endregion

    #region [回调函数]

    // 码流回调
    // i64PlayHandle：预览或回放接口返回的句柄
    // iStreamDataType：码流数据类型，0-码流头 1-码流数据 2-结束标记
    // pDataArray：流数据数组（需当成二进制数据来解析）
    // iDataLen：流数据长度
    // pUserData：用户数据
    //typedef void(CALLBACK * pfnStreamCallback)(VIDEO_INT64 i64PlayHandle, int iStreamDataType, const char* pDataArray, int iDataLen, void* pUserData);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void pfnStreamCallback(long i64PlayHandle, int iStreamDataType, string pDataArray, int iDataLen, IntPtr pUserData);

    // 取流、播放消息回调
    // i64PlayHandle：预览或回放接口返回的句柄，当回调的是语音对讲取流消息时，该值无效值（-1）
    // iMsg：消息类型，1-播放开始 2-播放结束（视频预览无此消息） 3-播放异常 10-取流开始 11-取流结束 12-取流异常
    //typedef void(CALLBACK * pfnMsgCallback)(VIDEO_INT64 i64PlayHandle, int iMsg, void* pUserData);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void pfnMsgCallback(long i64PlayHandle, int iMsg, IntPtr pUserData);


    // YUV数据回调
    // i64PlayHandle：预览或回放接口返回的句柄
    // pDataArray：YUV数据数组（需当成二进制数据来解析）
    // iDataLen：流数据长度
    // iWidth：图像宽度
    // iHeight：图像高度
    // iFrameType：图像YUV类型（目前为YV12，值为3）
    // iTimeStamp：时间戳
    // pUserData：用户数据
    //typedef void(CALLBACK* pfnDecodedDataCallback)(VIDEO_INT64 i64PlayHandle, const char* pDataArray, int iDataLen, int iWidth, int iHeight, int iFrameType, int iTimeStamp, void* pUserData);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void pfnDecodedDataCallback(long i64PlayHandle, string pDataArray, int iDataLen, int iWidth, int iHeight, int iFrameType, int iTimeStamp, IntPtr pUserData);

    // 录像下载回调（进度和消息）
    // i64DownloadHandle：网络录像下载句柄
    // fPercent：已下载录像进度，当iMsg为0时进度有效
    // iMsg：录像下载消息，0-开始录像下载 1-录像下载中 2-录像下载完成 3-录像下载即将分包 4-录像下载分包失败 5-录像下载分包完成 6-录像下载时断流
    // pUserData：用户自定义数据
    //typedef void (__stdcall* pfnDownloadCallback)(VIDEO_INT64 i64DownloadHandle, float fPercent, int iMsg, void* pUserData);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void pfnDownloadCallback(long i64DownloadHandle, float fPercent, int iMsg, IntPtr pUserData);


    // 取流、播放消息回调
    // i64PlayHandle：预览或回放接口返回的句柄，当回调的是语音对讲取流消息时，该值无效值（-1）
    // iMsg：消息类型，1-播放开始 2-播放结束（视频预览无此消息） 3-播放异常 10-取流开始 11-取流结束 12-取流异常
    //typedef void(CALLBACK * pfnMsgCallback)(VIDEO_INT64 i64PlayHandle, int iMsg, void* pUserData);
    //[UnmanagedFunctionPointer(CallingConvention.StdCall)]
    //public delegate void pfnMsgCallback(long i64PlayHandle, int iMsg, IntPtr pUserData);


    // 事件回调
    // pszEvent：事件信息， 格式为“事件类型 ,事件源编号 ;…;事件类型 ,事件源编号”， 如“131331,3ce931f9f3be4159a6f14e75c5a01c36;131330，ce931f9f3be4159a6f14e75c5a01c36”
    // pUserData：用户自定义数据
    //typedef void (__stdcall* pfnEventCallback)(const char* pszEvent, void* pUserData);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void pfnEventCallback(string pszEvent, IntPtr pUserData);



    #endregion

    public static class ISCSDK
    {
        private const string ISC_SDK = ".\\SDK\\VideoSDK.dll";

        // SDK初始化
        // pszEvn：保留参数，传NULL即可
        // 备注：调一次即可，初始化成功情况下重复调直接返回成功
        // 成功返回VIDEO_ERR_SUCCESS，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE int Video_Init(_IN_ const char* pszEvn);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, EntryPoint = "Video_Init", CallingConvention = CallingConvention.StdCall)]
        public static extern int Video_Init(string pszEvn);


        // SDK反初始化
        // 备注：调一次即可，反初始化成功情况下重复调直接返回成功
        // 成功返回VIDEO_ERR_SUCCESS，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE int Video_Fini();
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Video_Fini();


        // 视频预览（支持取实时流、回调取流与播放的消息，以及回调YUV数据，需要播放请求参数中指定）
        // pszUrl：预览URL，每次预览需要重新查询URL
        // hWnd：视频预览的Windows窗口句柄
        // pstPlayReq：播放请求参数，具体参数详见结构体定义
        // 备注：预览无播放结束消息
        // 成功返回大于0的预览句柄，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE VIDEO_INT64 Video_StartPreview(_IN_ const char* pszUrl, _IN_ void* hWnd, _IN_ PVIDEO_PLAY_REQ pstPlayReq);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern long Video_StartPreview(string pszUrl, IntPtr hWnd, ref VIDEO_PLAY_REQ pstPlayReq);


        // 停止预览（停止取流，停止回调消息，停止回调YUV数据）
        // i64PlayHandle：预览句柄，来源于Video_StartPreview返回值
        // 备注：预览无播放结束消息，当调停止预览接口成功就可以认为是播放结束了
        // 成功返回VIDEO_ERR_SUCCESS，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE int Video_StopPreview(_IN_ VIDEO_INT64 i64PlayHandle);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Video_StopPreview(long i64PlayHandle);

        // 网络录像回放、倒放（支持取网络录像流、回调取流与播放的消息，以及回调YUV数据，需要播放请求参数中指定）
        // pszUrl：回放URL
        // hWnd：网络录像正放或倒放的Window窗口句柄
        // i64StartTimeStamp：正放时为查询回放URL时的开始时间戳，倒放时为查询回放URL时的结束时间戳，单位：秒
        // i64EndTimeStamp：正放时为查询回放URL时的结束时间戳，倒放时为查询回放URL时的开始时间戳，单位：秒
        // pstPlayReq：播放请求参数，详细参数详见结构体定义
        // 成功返回大于0的播放句柄，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE VIDEO_INT64 Video_StartPlayback(_IN_ const char* pszUrl, _IN_ void* hWnd, _IN_ VIDEO_INT64 i64StartTimeStamp, _IN_ VIDEO_INT64 i64EndTimeStamp, _IN_ PVIDEO_PLAY_REQ pstPlayReq);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern long Video_StartPlayback(string pszUrl, IntPtr hWnd, long i64StartTimeStamp, long i64EndTimeStamp, ref VIDEO_PLAY_REQ pstPlayReq);


        // 停止网络录像正放、倒放（停止取流，停止回调消息，停止回调YUV数据）
        // i64PlayHandle：网络录像播放句柄，来源于Video_StartPlayback接口
        // 成功返回VIDEO_ERR_SUCCESS，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE int Video_StopPlayback(_IN_ VIDEO_INT64 i64PlayHandle);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Video_StopPlayback(long i64PlayHandle);


        // 网络录像播放、下载控制
        // i64PlayHandle：网络录像播放或录像下载句柄，来源于Video_StartPlayback接口
        // iCtrlType：控制类型，0-暂停正放或倒放 1-恢复正放或倒放 2-正放或倒放定位播放 3-正放或倒放倍速播放 4-暂停下载 5-恢复下载 其它值-不支持
        // i64Param：播放控制参数。当iCtrlType为0和1时无效；当iCtrlType为2时，该参数为定位播放时间戳；当iCtrlType为3时，该参数为倍速（播放速度从慢到快分别为：-16、-8、-4、-2、1、2、4、8、16）
        // 成功返回VIDEO_ERR_SUCCESS，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE int Video_PlayCtrl(_IN_ VIDEO_INT64 i64PlayHandle, _IN_ int iCtrlType, _IN_ VIDEO_INT64 i64Param);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Video_PlayCtrl(long i64PlayHandle, int iCtrlType, long i64Param);


        // 查询网络录像正放或倒放当前播放时间戳（指打入流中的时间戳）（单位：秒）
        // i64PlayHandle：网络录像正放或倒放句柄，来源于Video_StartPlayback接口
        // 成功返回时间戳，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE VIDEO_INT64 Video_GetCurrentPlayTime(_IN_ VIDEO_INT64 i64PlayHandle);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern long Video_GetCurrentPlayTime(long i64PlayHandle);


        // 播放抓图（支持视频预览和网络录像正放与倒放播放抓图）
        // i64PlayHandle：视频预览或网络录像正放、倒放句柄，来源于Video_StartPreview或Video_StartPlayback接口
        // pszAbsoluteFile：抓图图片文件绝对路径名称，只支持“bmp”和“jpg”后缀，如“D:\\SnapShot\test.bmp”，“D:\\SnapShot\test.jpg”，路径和文件名称中不能含有特殊字符（路径中目录名称全是空格字符串，中英文下的“*”、“|”以及英文下的“?”等特殊字符串）
        // 成功返回VIDEO_ERR_SUCCESS，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE int Video_PlaySnap(_IN_ VIDEO_INT64 i64PlayHandle, _IN_ const char* pszAbsoluteFile);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Video_PlaySnap(long i64PlayHandle, string pszAbsoluteFile);


        // 共享式声音控制（即支持打开多路的声音，如要独占式声音控制，可自行互斥的调该接口进行声音控制）
        // i64PlayHandle：视频预览或网络录像正放、倒放句柄，来源于Video_StartPreview或Video_StartPlayback接口
        // iCtrlType：控制类型，0-打开声音 1-关闭声音 其它值-参数错误
        // 成功返回VIDEO_ERR_SUCCESS，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE int Video_SoundCtrl(_IN_ VIDEO_INT64 i64PlayHandle, _IN_ int iCtrlType);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Video_SoundCtrl(long i64PlayHandle, int iCtrlType);


        // 获取音量
        // i64PlayHandle：视频预览或网络录像正放、倒放句柄，来源于Video_StartPreview或Video_StartPlayback接口
        // 成功返回音量值（取值范围为[0 100]），失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE int Video_GetVolume(_IN_ VIDEO_INT64 i64PlayHandle);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Video_GetVolume(long i64PlayHandle);


        // 设置音量
        // i64PlayHandle：视频预览或网络录像正放、倒放句柄，来源于Video_StartPreview或Video_StartPlayback接口
        // iVolume：音量值，范围应在[0 100]，如果超出范围，SDK内部使用音量最大或最小值来替代
        // 成功返回VIDEO_ERR_SUCCESS，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE int Video_SetVolume(_IN_ VIDEO_INT64 i64PlayHandle, _IN_ int iVolume);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Video_SetVolume(long i64PlayHandle, int iVolume);


        // 网络录像下载（下载到本地）
        // pszUrl：录像下载URL
        // i64SpszFileName：录像文件绝对路径文件名称，后缀必须是“mp4”，如“E:\\test.mp4”，路径和文件名称中不能含有特殊字符（路径中目录名称全是空格字符串，中英文下的“*”、“|”以及英文下的“?”等特殊字符串）
        // pstDownloadReq：录像下载请求参数
        // 成功返回大于0的录像下载句柄，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE VIDEO_INT64 Video_StartDownload(_IN_ const char* pszUrl, _IN_ const char* pszFileName, _IN_ PVIDEO_DOWNLOAD_REQ pstDownloadReq);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern long Video_StartDownload(string pszUrl, string pszFileName, ref VIDEO_DOWNLOAD_REQ pstDownloadReq);


        // 停止网络录像下载
        // i64DownloadHandle：网络录像下载句柄，来源于Video_StartDownload接口
        // 成功返回VIDEO_ERR_SUCCESS，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE int Video_StopDownload(_IN_ VIDEO_INT64 i64DownloadHandle);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Video_StopDownload(long i64DownloadHandle);


        // 画面字符叠加（对同一个画面支持多次叠加，每次叠加以返回的ID标识。如果叠加的字符串在位置上有重合，则字符串将被覆盖叠加。出于播放性能的考虑，不建议在同一个画面上多次叠加，请谨慎使用。）
        // i64PlayHandle：视频预览或网络录像正放、倒放句柄，来源于Video_StartPreview或Video_StartPlayback接口
        // iOSDId：字符叠加id，首次叠加请填充0，否则刷新画面上字符叠加id指定的字符
        // pszText：待叠加字符串，支持“\n”换行，字符串不超过2048个字节，如超过截断显示。
        // pstOSDInfo：画面字符叠加参数
        // 成功返回大于0的字符叠加id（当更新画面上字符时，返回值和iOSDId相同，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE int Video_SetOSDText(_IN_ VIDEO_INT64 i64PlayHandle, _IN_ int iOSDId, _IN_ const char* pszText, _IN_ PVIDEO_OSD_INFO pstOSDInfo);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Video_SetOSDText(long i64PlayHandle, int iOSDId, string pszText, ref VIDEO_OSD_INFO pstOSDInfo);


        // 查询视频详情（分辨率、帧率、编码格式、封装格式，码率需自行计算，计算方法：单位时间内接收到的流数据字节）
        // i64PlayHandle：视频预览或网络录像正放、倒放句柄，来源于Video_StartPreview或Video_StartPlayback接口
        // pstVideoDetailInfo：视频详情
        // 成功返回VIDEO_ERR_SUCCESS，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE int Video_GetVideoInfo(_IN_ VIDEO_INT64 i64PlayHandle, _OUT_ PVIDEO_DETAIL_INFO pstVideoDetailInfo);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Video_GetVideoInfo(long i64PlayHandle, IntPtr pstVideoDetailInfo);


        // 语音对讲（同一时刻只支持一路对讲，支持海康SDK、ehome协议接入前端设备）
        // pszUrl：对讲URL，每次使用需要重新查询URL
        // pszFileName：录音文件绝对路径文件名称，后缀必须是wav，如"D:\test.wav"。如不需要录音，直接传NULL即可。路径和文件名称中不能含有特殊字符（路径中目录名称全是空格字符串，中英文下的“*”、“|”以及英文下的“?”等特殊字符串），暂不支持语音录音，请传NULL。
        // fnMsg：语音对讲消息，用于回调语音对讲取流消息，如不需要直接传NULL即可。
        // pUserData：用户数据，用于fnMsg回调中透传出去，如不需要直接传NULL即可。
        // 成功返回VIDEO_ERR_SUCCESS，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE int Video_StartTalk(_IN_ const char* pszUrl, _IN_ const char* pszFileName, _IN_ pfnMsgCallback fnMsg, void* _IN_ pUserData);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Video_StartTalk(string pszUrl, string pszFileName, pfnMsgCallback fnMsg, IntPtr pUserData);


        // 停止语音对讲
        // 成功返回VIDEO_ERR_SUCCESS，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE int Video_StopTalk();
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Video_StopTalk();

        // 获取错误码
        // 返回错误码值
        //VIDEOSDK_DECLARE int Video_GetLastError();
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Video_GetLastError();


        /************************************************************************/
        /*                          以下为定制接口                              */
        /************************************************************************/
        // SDK初始化
        // pszAppkey：appkey
        // pszSecret：secret
        // pszIp：平台ip
        // iPort：https协议端口
        // 备注：调一次即可，初始化成功情况下重复调直接返回成功
        // 成功返回VIDEO_ERR_SUCCESS，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE int Video_InitEx(_IN_ const char* pszAppkey, _IN_ const char* pszSecret, _IN_ const char* pszIp, _IN_ int iPort);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Video_InitEx(string pszEnv, string pszSecret, string pszIp, int iPort);

        // 查询监控点预览取流url
        // pszCameraIndexCode：监控点编号
        // iStreamType：主子码流类型，0-主码流 1-子码流 其它值-未定义
        // pszUrlBuffer：接收URL的字符数组，至少64字节，由外部申请内存
        // iBufferSize：pszUrlBuffer数组长度，请保证pszUrlBuffer数组长度与iBufferSize指定的长度一致
        // 返回值：成功返回VIDEO_ERR_SUCCESS；失败返回VIDEO_ERR_FAIL，通过Video_GetLastError获取错误码，iBufferSize长度不足时失败了错误码为VIDEO_ERR_BUFFER_SMALL
        //VIDEOSDK_DECLARE int Video_GetPreviewUrl(_IN_ const char* pszCameraIndexCode, _IN_ int iStreamType, _OUT_ char* pszBuffer, _IN_ int pBufferSize);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Video_GetPreviewUrl(string pszUrl, int iStreamType, StringBuilder pszBuffer, int pBufferSize);


        // 查询监控点回放取流url
        // pszCameraIndexCode：监控点编号
        // iRecLocation：存储位置，0-中心存储 1-设备存储 其它值-未定义
        // i64StartTimeStamp：查录像开始时间戳，单位：秒
        // i64EndTimeStamp：查录像结束时间戳，单位：秒
        // pszUrlBuffer：接收URL的字符数组，由外部申请内存，建议不少于256字节；url;begintime,endtime,size（回放URL，查询录像时间段存在录像的开始时间，查询录像时间段存在录像的结束时间，查询录像时间段内录像总大小)按此格式存储录像信息，录像总大小单位为字节,建议size使用long long数据类型来解析，否则存在数据溢出风险
        // iBufferSize：pszUrlBuffer数组长度，请保证pszUrlBuffer数组长度与iBufferSize指定的长度一致
        // 返回值：成功返回VIDEO_ERR_SUCCESS；失败返回VIDEO_ERR_FAIL，通过Video_GetLastError获取错误码，iBufferSize长度不足时失败了错误码为VIDEO_ERR_BUFFER_SMALL
        //VIDEOSDK_DECLARE int Video_GetPlaybackUrl(_IN_ const char* pszCameraIndexCode, _IN_ int iRecLocation, _IN_ VIDEO_INT64 i64StartTimeStamp, _IN_ VIDEO_INT64 i64EndTimeStamp, _IN_ _OUT_ char* pszUrlBuffer, _IN_ int iBufferSize);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Video_GetPlaybackUrl(string pszCameraIndexCode, int iRecLocation, long i64StartTimeStamp, long i64EndTimeStamp, StringBuilder pszUrlBuffer, int iBufferSize);


        // 订阅事件
        // pszEventTypeList：订阅事件的类型列表，格式“类型1,类型2...,类型n”，如订阅移动侦测报警和视频遮挡报警事件，传入"131331,131330"
        // pszRecvIp：事件接收地址，请填充本机IP，针对双网卡情况，请自行选择某网络的IP
        // iRecvPort：事件接收端口，一般可以使用http协议端口，如80端口
        // fnEventCB：事件回调
        // pUserData
        // 成功返回VIDEO_ERR_SUCCESS，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE int Video_SubscribeEvent(_IN_ const char* pszEventTypeList, _IN_ const char* pszRecvIp, _IN_ int iRecvPort, _IN_ pfnEventCallback fnEventCB, _IN_ void* pUserData);
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Video_SubscribeEvent(string pszEventTypeList, string pszRecvIp, int iRecvPort, pfnEventCallback fnEventCB, IntPtr pUserData);



        // 取消订阅事件
        // 成功返回VIDEO_ERR_SUCCESS，失败返回VIDEO_ERR_FAIL，失败了通过Video_GetLastError查询错误码
        //VIDEOSDK_DECLARE int Video_UnSubscribeEvent();
        [DllImport(ISC_SDK, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Video_UnSubscribeEvent();

    }
}
