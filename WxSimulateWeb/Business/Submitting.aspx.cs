using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Simple.Data;
using System.Threading;
using System.IO;
using WxSimulateWeb.App_Code;
using WxSimulateWebHelper;
using WxPlatfromWraper;
using WxPlatfromWraper.WxJson;

public partial class Business_Submitting : System.Web.UI.Page
{
    List<WxMsg> profMsg = null;
    string logFile = null;
    string uidUser = null;
    //string sucessToken = "<<<发送成功！>>>";
    string completeToken = "<<<结束>>>";
    string startToken = "<<<开始发送>>>";

    protected void Page_Load(object sender, EventArgs e)
    {
        string ErrMsg = null;
        profMsg = Profile.ProfileMsg.MsgList;
        if (!WxSimulater.CheckMsgList(profMsg, out ErrMsg))
        {
            //maybe a attack!
            FormsAuthentication.SignOut();
            Response.Redirect("../Account/Login.aspx");
        }

        uidUser = Membership.GetUser(User.Identity.Name).ProviderUserKey.ToString();
        logFile = Server.MapPath(string.Format("~/UploadedImages/{0}/log.txt", uidUser));
        if (!IsPostBack)
        {
            WxSimulateWebHelper.UserDirectory.Creat(Server.MapPath(string.Format("~/UploadedImages/{0}", uidUser)),User.Identity.Name);

            divProcMsg.InnerHtml = string.Empty;

            Thread th = new Thread(SubmitMsg);
            th.IsBackground = true;
            th.Start();
        }
    }

    private void SubmitMsg()
    {
        Logger logger = new Logger(logFile);

        logger.Log(startToken);
        logger.Log("初始化SendMsgHelper");
        SendMsgHelper helper = new SendMsgHelper
        {
            uidUser = Guid.Parse(uidUser),
            logger = logger,
            ImgsPath = Server.MapPath("~/UploadedImages/" + uidUser)
        };

        try
        {
            logger.Log("检查遗留任务");
            if (helper.IsHasWaitingTasks())
            {
                logger.Log("发现之前还有未发送的信息！");
                helper.ExecTask(profMsg);
                logger.Log(completeToken);
                return;
            }

            logger.Log("初始化任务数据库");
            if (0 == helper.InitTask())
            {
                logger.Log(completeToken);
                return;
            }
            logger.Log("初始化成功");

            logger.Log("开始发送信息");
            helper.ExecTask(profMsg);
            logger.Log(completeToken);
        }
        catch (Exception ex)
        {
            //sw = new StreamWriter(logFile, true);
            logger.Log(string.Format("系统异常：{0}", ex.ToString()));
            logger.Log(completeToken);
            //sw.Close();
            return;
        }

    }

    protected void UpdateTimer_Tick(object sender, EventArgs e)
    {
        try
        {
            using (StreamReader reader = new StreamReader(new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
            {
                //start at the end of the file
                long lastMaxOffset = long.Parse(logFileSize.Value);

                //if the file size has not changed, return
                if (reader.BaseStream.Length == lastMaxOffset)
                    return;

                //seek to the last max offset
                reader.BaseStream.Seek(lastMaxOffset, SeekOrigin.Begin);

                //read out of the file until the EOF
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    divProcMsg.InnerHtml += string.Format("{0}<br />", line);
                    if (line.Contains(completeToken))
                    {
                        UpdateTimer.Enabled = false;
                        Label1.Text = "任务结束！";
                    }
                }

                //update the last max offset
                logFileSize.Value = reader.BaseStream.Position.ToString();
            }
        }
        catch (Exception ex)
        {
            divProcMsg.InnerHtml += string.Format("{0}<br />", ex.ToString());
        }
    }
}