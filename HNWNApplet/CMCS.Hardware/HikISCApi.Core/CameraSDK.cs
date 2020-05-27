using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace HikISCApi.Core
{
    public class CameraSDK
    {
        #region [私有变量]

        //监控点取流URL
        string m_strURL;

        //抓拍图片Url
        string m_picURL;

        //播放句柄
        long m_lPlayHandle = -1;

        private pfnStreamCallback _pfnStreamCallback = null;
        private pfnMsgCallback _pfnMsgCallback = null;
        private pfnDecodedDataCallback _pfnDecodeDataCallback = null;

        static HttpUtillib httpUtillib = null;

        string CameraCode;

        /// <summary>
        /// 连接状态
        /// </summary>
        public static bool Status = false;

        #endregion

        /// <summary>
        /// 初始化SDK
        /// </summary>
        /// <param name="ip">ISC平台地址</param>
        /// <param name="port">端口号 默认80</param>
        /// <param name="appkey">appkey</param>
        /// <param name="secret">secret</param>
        /// <returns></returns>
        public static bool InitSDK(string ip, int port, string appkey, string secret)
        {
            try
            {
                HttpUtillib.SetPlatformInfo(appkey, secret, ip, port, false);

                Status = ISCSDK.Video_Init(string.Empty) == 0;

                return Status;
            }
            catch { }

            Status = false;

            return Status;
        }

        /// <summary>
        /// 卸载 SDK
        /// </summary>
        /// <returns></returns>
        public static bool CleanupSDK()
        {
            Status = false;

            try
            {
                return ISCSDK.Video_Fini() == 0;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 开始预览
        /// </summary>
        /// <returns></returns>
        public bool StartPreview(string cameraCode, IntPtr intptr)
        {
            this.CameraCode = cameraCode;

            try
            {
                int iStreamType = 0; //码流模式，0：主码流  1：子码流
                Dictionary<string, object> paramMaps = new Dictionary<string, object>();
                paramMaps.Add("cameraIndexCode", this.CameraCode);
                paramMaps.Add("streamType", iStreamType);
                paramMaps.Add("protocol", "rtsp");
                paramMaps.Add("transmode", 1);
                paramMaps.Add("expand", "streamform=ps");

                string uri = "/artemis/api/video/v1/cameras/previewURLs";
                string body = JsonHelper.SerializeObject(paramMaps);

                byte[] resultByte = HttpUtillib.HttpPost(uri, body, 20);
                string resultStr = Encoding.UTF8.GetString(resultByte);

                Result result = JsonHelper.DeserializeJsonToObject<Result>(resultStr);
                if (result.Code.Equals("0"))
                {
                    Dictionary<string, object> cameraPreviewResult = JsonHelper.DeserializeJsonToObject<Dictionary<string, object>>(result.Data.ToString());
                    m_strURL = cameraPreviewResult["url"].ToString();

                    VIDEO_PLAY_REQ vpr = new VIDEO_PLAY_REQ();
                    vpr.iHardWareDecode = 0;
                    vpr.fnStream = _pfnStreamCallback;
                    vpr.fnMsg = _pfnMsgCallback;
                    vpr.fnDecodedStream = _pfnDecodeDataCallback;
                    m_lPlayHandle = ISCSDK.Video_StartPreview(m_strURL, intptr, ref vpr);

                    return m_lPlayHandle != -1;
                }
            }
            catch { }

            return false;
        }

        /// <summary>
        /// 停止预览
        /// </summary>
        /// <returns></returns>
        public bool StopPreview()
        {
            int iRet = ISCSDK.Video_StopPreview(m_lPlayHandle);

            return iRet == 0;
        }

        /// <summary>
        /// 分页获取监控点资源
        /// </summary>
        /// <returns></returns>
        public static List<Dictionary<string, object>> GetCameraList()
        {
            try
            {
                List<Dictionary<string, object>> cameraList = new List<Dictionary<string, object>>();
                List<string> cameraNameList = new List<string>();

                Dictionary<string, object> paramMaps = new Dictionary<string, object>();
                paramMaps.Add("pageNo", 1);
                paramMaps.Add("pageSize", 100);

                string uri = "/artemis/api/resource/v1/camera/advance/cameraList";
                string body = JsonHelper.SerializeObject(paramMaps);

                byte[] resultByte = HttpUtillib.HttpPost(uri, body, 20);
                string resultStr = Encoding.UTF8.GetString(resultByte);

                Result result = JsonHelper.DeserializeJsonToObject<Result>(resultStr);
                if (result.Code.Equals("0"))
                {
                    ResultData resultData = JsonHelper.DeserializeJsonToObject<ResultData>(result.Data.ToString());
                    return JsonHelper.DeserializeJsonToObject<List<Dictionary<string, object>>>(resultData.List.ToString());
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// 获取抓拍图片Url
        /// </summary>
        /// <returns></returns>
        private bool GetCapturePictureUrl()
        {
            Dictionary<string, object> paramMaps = new Dictionary<string, object>();
            paramMaps.Add("cameraIndexCode", this.CameraCode);

            string uri = "/artemis/api/video/v1/manualCapture";
            string body = JsonHelper.SerializeObject(paramMaps);

            byte[] resultByte = HttpUtillib.HttpPost(uri, body, 20);
            string resultStr = Encoding.UTF8.GetString(resultByte);

            Result result = JsonHelper.DeserializeJsonToObject<Result>(resultStr);

            if (result.Code.Equals("0"))
            {
                Dictionary<string, object> cameraPreviewResult = JsonHelper.DeserializeJsonToObject<Dictionary<string, object>>(result.Data.ToString());

                m_picURL = cameraPreviewResult["picUrl"].ToString();

                return true;
            }
            return false;
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="url">ApiUrl</param>
        /// <param name="localPath">本地物理路径</param>
        public bool CapturePictureJPEG(string localPath)
        {
            try
            {
                if (GetCapturePictureUrl())
                {
                    m_picURL = m_picURL.Replace("https", "http").Replace("443", "80");

                    HttpWebRequest imgRequest = (HttpWebRequest)HttpWebRequest.Create(m_picURL);

                    HttpWebResponse res = (HttpWebResponse)imgRequest.GetResponse();

                    if (res != null && res.StatusCode.ToString() == "OK")
                    {
                        System.Drawing.Image downImage = System.Drawing.Image.FromStream(imgRequest.GetResponse().GetResponseStream());

                        downImage.Save(localPath);

                        downImage.Dispose();

                        return true;
                    }
                }
            }
            catch { }

            return false;
        }
    }
}
