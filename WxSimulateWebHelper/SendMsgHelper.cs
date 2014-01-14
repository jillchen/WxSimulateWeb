using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Data;
using WxPlatfromWraper;
using WxPlatfromWraper.WxJson;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace WxSimulateWebHelper
{
    public class SendMsgHelper
    {
        public Guid uidUser { get; set; }
        public Logger logger { get; set; }
        public string ImgsPath { get; set; }

        public bool IsHasWaitingTasks()
        {
            var db = Database.Open();
            return db.SendTask.Exists(db.SendTask.uidUser == uidUser && db.SendTask.taskStatus == 0);
        }

        public int InitTask()
        {
            var db = Database.Open();
            List<DbUserPubUser> pubUserList = db.WxPubUsersDB.FindAllByuidUser(uidUser);
            if (pubUserList.Count == 0)
            {
                logger.Log("还没有添加任何公众号！取消发送！");
                return 0;
            }
            using (var tran = db.BeginTransaction())
            {
                db.SendTask.Insert(pubUserList);
            }

            logger.Log(string.Format("总共要发送{0}条图文信息", pubUserList.Count));
            return pubUserList.Count;
        }

        public void ExecTask(List<WxMsg> msgs)
        {
            //创建公众号账号和密码词典 PubUserDict
            var db = Database.Open();
            List<DbPubUser> pubUsers = db.WxPubUsersDB.FindAllByuidUser(uidUser);
            Dictionary<Guid, DictPubUser> dictPubUsers = new Dictionary<Guid, DictPubUser>(pubUsers.Count);
            foreach (var pubUsr in pubUsers)
            {
                dictPubUsers.Add(pubUsr.uidWxPubUsr, new DictPubUser { pubAccount = pubUsr.pubAccount, pubPw = pubUsr.pubPw });
            }

            //获取待执行任务
            List<DbWaitingTask> waitingTasks = db.SendTask.FindAllByuidUserAndtaskStatus(uidUser, 0);
            WxPublicUser curPubUsr = null;
            string errMsg = null;
            foreach (var task in waitingTasks)
            {
                // 登陆，得到 WxPublicUser curPubUsr
                if (!TryLogin(task, dictPubUsers, out curPubUsr, out errMsg))
                    continue;

                #region 上传MsgList
                int idx = 1;
                List<WxMsg> newMsgs = new List<WxMsg>();
                WxMsg newMsg = null;//为了不修改Prof的内容
                bool failMsg = false;//标记是否有失败操作
                //string CntImgUrl = null;
                //string newMsgCnt = null;//暂时保存新的正文内容
                string outFileID = null;
                string ImgPath = null;
                string ImgUrl = null;//暂时保存上传后的Url
                foreach (var msg in msgs)
                {
                    logger.Log(string.Format("开始上传第{0}条信息的相关图片", idx));
                    newMsg = new WxMsg(msg);//为了不修改Prof的内容
                    #region 上传封面图片，得到FileId
                    logger.Log("正在上传封面图片");
                    string coverFileName = msg.FileId.Substring(msg.FileId.LastIndexOf('/') + 1);
                    int retCode = WxSimulater.UpCoverImg(curPubUsr, ImgsPath + '\\' + coverFileName, out outFileID, out errMsg);
                    if (retCode != 0 || outFileID == null)
                    {
                        logger.Log(string.Format("上传第条{0}信息的封面图片失败！请选择上传更小的图片！错误码：{1}{2}", idx, retCode, errMsg));
                        failMsg = true;
                        break;
                    }
                    newMsg.FileId = outFileID;
                    logger.Log("上传成功！");
                    #endregion

                    #region 上传正文图片
                    logger.Log("正在上传正文图片");
                    string strTargetString = newMsg.Content;

                    string strRegex = string.Format(@"<img([^<]+?)src=[\""']([^<]+?)/{0}/(?<fileName>([^<]+?))[\""']([^<]*?)>", uidUser.ToString());
                    Dictionary<string, string> dictCntImgs = new Dictionary<string, string>();//fileName as key, and Url as value

                    Regex matchImgs = new Regex(strRegex, RegexOptions.IgnoreCase);
                    foreach (Match myMatch in matchImgs.Matches(strTargetString))
                    {
                        if (myMatch.Success)
                        {
                            ImgPath = myMatch.Groups["fileName"].Value;
                            if (WxSimulater.UpCntImg(curPubUsr, Path.Combine(this.ImgsPath, ImgPath), out ImgUrl, out errMsg) && ImgUrl != null)
                            {
                                dictCntImgs.Add(ImgPath, ImgUrl);
                            }
                            else
                            {
                                logger.Log("上传正文图片出错：" + errMsg);
                                failMsg = true;
                                break;
                            }
                        }
                    }

                    if (failMsg)
                    {
                        break;
                    }

                    Regex replaceImgs = null;
                    string strMatchRegexTemplate = @"<img([^<]+?)src=[\""']([^<]+?)/{0}/{1}[\""']([^<]*?)>";
                    string strReplaceTemplate = @"<img src=""{0}"" style=""float:none;"" />";
                    foreach (var img in dictCntImgs)
                    {
                        replaceImgs = new Regex(string.Format(strMatchRegexTemplate, uidUser.ToString(), img.Key), RegexOptions.IgnoreCase);
                        strTargetString = replaceImgs.Replace(strTargetString, string.Format(strReplaceTemplate, img.Value));
                    }

                    newMsg.Content = strTargetString;//保存替换后的正文
                    logger.Log("上传成功");
                    #endregion

                    newMsgs.Add(newMsg);
                    idx++;//上传下一条
                }
                if (failMsg)
                {
                    logger.Log("该用户上传图片失败！跳过该用户！");
                    db.SendTask.UpdateBytaskId(taskId: task.taskId,
                                                                 taskStatus: 4,//上传图片错误
                                                                 taskEndTime: DateTime.Now,
                        //taskRetCode: ,
                                                                 taskRetMsg: errMsg);

                    continue;
                }

                logger.Log("开始上传图文信息的正文内容");
                int tmpCode = WxSimulater.UpMultiMsg(curPubUsr, newMsgs, out errMsg);
                if (tmpCode != 0)
                {
                    logger.Log("上传正文失败！跳过该用户！");
                    db.SendTask.UpdateBytaskId(taskId: task.taskId,
                                                                 taskStatus: 5,//上传正文内容失败
                                                                 taskEndTime: DateTime.Now,
                                                                 taskRetCode: tmpCode,
                                                                 taskRetMsg: errMsg);
                    continue;
                }
                logger.Log("上传成功！");
                #endregion

                #region 开始群发
                logger.Log("获取公众平台的APP_ID");
                WxMsgListRetInfo newUploadMsg = WxSimulater.GetMsgList(curPubUsr);
                logger.Log("开始群发");
                tmpCode = -1;
                tmpCode = WxSimulater.SendMsg(curPubUsr, newUploadMsg.app_msg_info.item[0].app_id.ToString(), out errMsg);
                int taskStat = 0;
                switch (tmpCode)
                {
                    case 0:
                        logger.Log("群发发送成功！");
                        taskStat = 100;
                        break;
                    case 64004://今天的群发数量已到，无法群发
                        logger.Log("今天该公众号的群发数量已超过微信限制");
                        taskStat = 99;
                        break;
                    case 67008://消息中可能含有具备安全风险的链接，请检查
                        logger.Log("消息中可能含有具备安全风险的链接，请检查");
                        taskStat = 98;
                        break;
                    case -6://请输入验证码
                        logger.Log("发送信息时需要人工输入验证码，无法发送！");
                        taskStat = 97;
                        break;
                    default:
                        logger.Log("发送失败，未知代码");
                        taskStat = 96;
                        break;
                }
                db.SendTask.UpdateBytaskId(taskId: task.taskId,
                             taskStatus: taskStat,
                             taskEndTime: DateTime.Now,
                             taskRetCode: tmpCode,
                             taskRetMsg: errMsg);
                //"{\"ret\":\"64004\", \"msg\":\"not have masssend quota today!\"}"
                //if (r.ret && r.ret == "0") {
                //  i.suc("发送成功"), t && t(r);
                //  return;
                //}
                //r.ret && r.ret == "64004" ? i.err("今天的群发数量已到，无法群发") : 
                //r.ret && r.ret == "67008" ? i.err("消息中可能含有具备安全风险的链接，请检查") : 
                //r.ret == "-6" ? i.err("请输入验证码") : i.err("发送失败"), n && n(r);
                #endregion
            }
        }

        //登陆，得到 WxPublicUser curPubUsr
        private bool TryLogin(DbWaitingTask task, Dictionary<Guid, DictPubUser> dictPubUsers,
                                out WxPublicUser curPubUsr, out string errMsg)
        {
            var db = Database.Open();
            string pubAccount = dictPubUsers[task.uidWxPubUsr].pubAccount;
            string pubPw = dictPubUsers[task.uidWxPubUsr].pubPw;
            logger.Log(string.Format("尝试登陆公众号：{0}", pubAccount));
            curPubUsr = WxSimulater.ExecLogin(pubAccount, pubPw, string.Empty);
            if (curPubUsr.IsLogin())
            {
                logger.Log("登陆成功");
                errMsg = null;
                return true;
            }
            else
            {
                errMsg = WxSimulater.ParseLoginRetInfo(curPubUsr.LoginRetInfo);
                logger.Log(string.Format("登陆失败，跳过该用户，错误：errCode:{0} {1}", curPubUsr.LoginRetInfo.ErrCode, errMsg));

                int tmpStatCode = 0;
                switch (curPubUsr.LoginRetInfo.ErrCode)
                {
                    case -6:
                    case -32:
                        tmpStatCode = 1;//验证码错误
                        break;
                    case -2:
                    case -3:
                    case -4:
                        tmpStatCode = 2;//密码或账号错误
                        break;
                    default:
                        tmpStatCode = 3;//其他登陆错误
                        break;
                }
                db.SendTask.UpdateBytaskId(taskId: task.taskId,
                                                      taskStatus: tmpStatCode,
                                                      taskEndTime: DateTime.Now,
                                                      taskRetCode: curPubUsr.LoginRetInfo.ErrCode,
                                                      taskRetMsg: errMsg);


                return false;
            }
        }
    }
}
