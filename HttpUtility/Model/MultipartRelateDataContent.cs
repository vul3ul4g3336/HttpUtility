using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HttpUtility.Model
{
    public class MultipartRelateDataContent
    {
        public static MultipartContent Content(object data, StreamContent contents = null)
        {
            // 建立 JSON 內容物件
            //var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");


            //// 讀取檔案流
            //var videoFileStream = new FileStream(videoFilePath, FileMode.Open, FileAccess.Read);

            //// 建立影片內容物件
            //var videoContent = new StreamContent(videoFileStream);
            //// 設定影片內容的 Content-Type
            //videoContent.Headers.ContentType = new MediaTypeHeaderValue("video/mp4"); // 根據您的影片類型調整
            // 建立一個 MultipartContent 物件


            var json = JsonConvert.SerializeObject(data);
            var multipartContent = new MultipartContent("related", "YOUR_BOUNDARY_STRING");
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
            // 將 JSON 內容加入到 multipart 請求中
            multipartContent.Add(jsonContent);
            if (contents != null)
            {
                multipartContent.Add(contents);

                // 將影片檔案內容加入到 multipart 請求中

            }
            return multipartContent;
        }
    }
}
