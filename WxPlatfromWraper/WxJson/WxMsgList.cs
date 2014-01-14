using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WxPlatfromWraper.WxJson
{
    /// <summary>
    /// 多图文信息列表
    /// </summary>
    [Serializable]
    public class WxMsgList
    {
        public List<WxMsg> MsgList = new List<WxMsg>();

        public bool AddMsg(WxMsg msg)
        {
            try
            {
                MsgList.Add(msg);
                return true;
            }
            catch
            {

            }
            return false;
        }

        public bool DelMsg(int i)
        {
            try
            {
                MsgList.RemoveAt(i);
                return true;
            }
            catch
            {

            }

            return false;
        }

        public bool EdtMsg(int i, WxMsg msg)
        {
            try
            {
                MsgList[i] = msg;
                return true;
            }
            catch
            {

            }
            return false;
        }

        public WxMsg GetMsg(int i)
        {
            return MsgList[i];
        }
    }
}
