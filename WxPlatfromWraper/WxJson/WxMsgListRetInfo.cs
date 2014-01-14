using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WxPlatfromWraper.WxJson
{
    public class BaseResp
    {
        public int ret { get; set; }
        public string err_msg { get; set; }
    }

    public class MultiItem
    {
        public int seq { get; set; }
        public string cover { get; set; }
        public string title { get; set; }
        public string digest { get; set; }
        public string content_url { get; set; }
        public int file_id { get; set; }
        public string source_url { get; set; }
        public string author { get; set; }
        public int show_cover_pic { get; set; }
    }

    public class Item
    {
        public int seq { get; set; }
        public int app_id { get; set; }
        public int file_id { get; set; }
        public string title { get; set; }
        public string digest { get; set; }
        public string create_time { get; set; }
        public List<MultiItem> multi_item { get; set; }
        public string content_url { get; set; }
        public string img_url { get; set; }
        public string author { get; set; }
        public int show_cover_pic { get; set; }
    }

    public class FileCnt
    {
        public int total { get; set; }
        public int img_cnt { get; set; }
        public int voice_cnt { get; set; }
        public int video_cnt { get; set; }
        public int app_msg_cnt { get; set; }
        public int commondity_msg_cnt { get; set; }
        public int video_msg_cnt { get; set; }
    }

    public class AppMsgInfo
    {
        public List<Item> item { get; set; }
        public FileCnt file_cnt { get; set; }
        public int is_upload_cdn_ok { get; set; }
    }

    /// <summary>
    /// 模拟请求返回得到的图文信息列表，这是用http://json2csharp.com/生成的
    /// </summary>
    public class WxMsgListRetInfo
    {
        public BaseResp base_resp { get; set; }
        public AppMsgInfo app_msg_info { get; set; }
    }

}
