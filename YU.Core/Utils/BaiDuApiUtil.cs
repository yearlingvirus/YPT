using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace YU.Core.Utils
{
    public static class BaiDuApiUtil
    {

        /// <summary>
        /// 通用文字识别
        /// </summary>
        /// <param name="bmp"></param>
        public static List<string> GeneralBasic(Image bmp)
        {

            string apiOrcKey = ConfigUtil.GetConfigValue(YUConst.SETTING_BAIDUAPIORCKEY);
            string apiSecretKey = ConfigUtil.GetConfigValue(YUConst.SETTING_BAIDUORCSECRETKEY);
            if (apiOrcKey.IsNullOrEmptyOrWhiteSpace() || apiSecretKey.IsNullOrEmptyOrWhiteSpace())
                throw new Exception("无法获取到Api Key和Secret Key，调用百度ORC识别失败。");

            var client = new Baidu.Aip.Ocr.Ocr(apiOrcKey, apiOrcKey);
            var bytes = ImageUtils.ImageToBytes(bmp);
            var result = client.GeneralBasic(bytes);
            if (result.ContainsKey("error_code") && result.ContainsKey("error_msg"))
                throw new Exception(string.Format("调用百度ORC识别失败，错误码：{0}，错误信息：{1}", result["error_code"], result["error_msg"]));

            List<string> orcResults = new List<string>();
            if (result.ContainsKey("words_result_num") && result.ContainsKey("words_result"))
            {
                var wordResults = result["words_result"];
                if (wordResults != null && wordResults.Any())
                {
                    foreach (var word in wordResults)
                    {
                        orcResults.Add(word.Value<string>("words"));
                    }
                }
            }

            return orcResults;
        }

        /// <summary>
        /// 网络图片识别
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static List<string> WebImage(Image bmp)
        {
            string apiOrcKey = ConfigUtil.GetConfigValue(YUConst.SETTING_BAIDUAPIORCKEY);
            string apiSecretKey = ConfigUtil.GetConfigValue(YUConst.SETTING_BAIDUORCSECRETKEY);
            if (apiOrcKey.IsNullOrEmptyOrWhiteSpace() || apiSecretKey.IsNullOrEmptyOrWhiteSpace())
                throw new Exception("无法获取到Api Key和Secret Key，调用百度ORC识别失败。");

            var client = new Baidu.Aip.Ocr.Ocr(apiOrcKey, apiSecretKey);
            var bytes = ImageUtils.ImageToBytes(bmp);
            var result = client.WebImage(bytes);
            if (result.ContainsKey("error_code") && result.ContainsKey("error_msg"))
                throw new Exception(string.Format("调用百度ORC识别失败，错误码：{0}，错误信息：{1}", result["error_code"], result["error_msg"]));

            List<string> orcResults = new List<string>();
            if (result.ContainsKey("words_result_num") && result.ContainsKey("words_result"))
            {
                var wordResults = result["words_result"];
                if (wordResults != null && wordResults.Any())
                {
                    foreach (var word in wordResults)
                    {
                        orcResults.Add(word.Value<string>("words"));
                    }
                }
            }
            return orcResults;
        }


    }
}
