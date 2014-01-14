using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace WxPlatfromWraper.WxJson
{
    /// <summary>
    /// 保存登陆后的用户信息
    /// </summary>
    public class WxPublicUser
    {
        /**下面的信息是从登录成功后的页面那里得到的信息        
         * 其中：
         * t是token        
         * ticket上传素材时要用到
         * uin貌似是公众号的id
         * user_name是公众号的名称
         * */
        //data:{
        //     t:"1835487172",
        //     ticket:"d97d13c9abb09aca1c4761aac5f9d0e4dd5145c3",
        //     lang:'zh_CN',
        //     param:["&token=1835487172",'&lang=zh_CN'].join(""),
        //     uin:"1234",
        //     user_name:"xxxx",
        //     time:"1387886501"||new Date().getTime()/1000
        // }
        #region 对应上面信息，为了方便全部设string
        public string Token { get; set; }
        public string Ticket { get; set; }
        public string Lang { get; set; }
        public string Uin { get; set; }
        public string User_name { get; set; }
        public string Time { get; set; }
        #endregion



        /// <summary>
        /// 保存登录后得到的cookie
        /// </summary>
        public CookieContainer LoginCookie { get; set; }

        /// <summary>
        /// 保存登录后得到的返回信息
        /// </summary>
        public WxLoginRetInfo LoginRetInfo { get; set; }

        /// <summary>
        /// 判断是否登录成功
        /// </summary>
        /// <returns>登录成功返回true，否则false</returns>
        public bool IsLogin()
        {
            if (Token == null || Token == string.Empty)
                return false;
            else
                return true;
        }
    }
}
