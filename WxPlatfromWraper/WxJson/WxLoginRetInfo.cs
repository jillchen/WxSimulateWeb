using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WxPlatfromWraper.WxJson
{
    /// <summary>
    /// 登录时微信公众平台网页返回的信息
    /// </summary>
    public class WxLoginRetInfo
    {
        public int Ret { get; set; }
        public string ErrMsg { get; set; }
        public int ShowVerifyCode { get; set; }
        public int ErrCode { get; set; }
    }
}
