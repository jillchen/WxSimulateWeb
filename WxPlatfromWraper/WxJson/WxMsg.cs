using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WxPlatfromWraper.WxJson
{
    /// <summary>
    /// 单个多图文信息的内容
    /// </summary>
    [Serializable]
    public class WxMsg
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Digest { get; set; }
        public string Author { get; set; }
        public string FileId { get; set; }
        public bool ShowCoverPic { get; set; }
        public string SourceUrl { get; set; }

        public WxMsg(WxMsg msg)
        {
            Title = msg.Title;
            Content = msg.Content;
            Digest = msg.Digest;
            Author = msg.Author;
            FileId = msg.FileId;
            ShowCoverPic = msg.ShowCoverPic;
            SourceUrl = msg.SourceUrl;
        }

        public WxMsg()
        {
            // TODO: Complete member initialization
        }
    }
}
