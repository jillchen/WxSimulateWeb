using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WxSimulateWeb.App_Code;
using System.Web.Profile;
using WxPlatfromWraper;
using Simple.Data;
using WxSimulateWebHelper;
using System.Web.Security;
using WxPlatfromWraper.WxJson;

public partial class Business_MultiSendMsg : System.Web.UI.Page
{
    List<WxMsg> profMsg = null;

    protected void Page_Load(object sender, EventArgs e)
    {


        profMsg = Profile.ProfileMsg.MsgList;
        warnMsg.Text = string.Empty;
        

        if (!IsPostBack)
        {
            //检查是否需要继续上次未完成的任务
            SendMsgHelper helper = new SendMsgHelper
            {
                uidUser = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey,
            };
            if (helper.IsHasWaitingTasks())
            {
                Response.Redirect("./Submitting.aspx");
                return;
            }

            if (profMsg.Count == 0)
                profMsg.Add(new WxMsg());

            LoadProfMsg(0);


        }
    }

    private void BindProfMsg()
    {
        lvMsgList.DataSource = profMsg;
        lvMsgList.DataBind();
    }

    private void SaveProfMsg(int i)
    {
        profMsg[i] = new WxMsg
        {
            Title = Base64.base64Decode(hfTitle.Value),
            FileId = hfCoverImgUrl.Value,
            Author = Base64.base64Decode(hfAuthor.Value),
            SourceUrl = Base64.base64Decode(hfSourceUrl.Value),
            Digest = Base64.base64Decode(hfDigest.Value),
            Content = Base64.base64Decode(hfContent.Value),
            ShowCoverPic = true
        };
    }

    private void LoadProfMsg(int i)
    {
        WxMsg msg = profMsg[i];
        hfTitle.Value = msg.Title != null ? Base64.base64Encode(msg.Title) : string.Empty;
        hfAuthor.Value = msg.Author != null ? Base64.base64Encode(msg.Author) : string.Empty;
        hfContent.Value = msg.Content != null ? Base64.base64Encode(msg.Content) : string.Empty;
        hfDigest.Value = msg.Digest != null ? Base64.base64Encode(msg.Digest) : string.Empty;
        hfSourceUrl.Value = msg.SourceUrl != null ? Base64.base64Encode(msg.SourceUrl) : string.Empty;

        hfCoverImgUrl.Value = msg.FileId != null ? msg.FileId : string.Empty;
        CoverImage.ImageUrl = hfCoverImgUrl.Value;

        hfCurMsg.Value = i.ToString();
        BindProfMsg();
    }

    private void DelProfMsg(int i)
    {
        profMsg.RemoveAt(i);
        if (profMsg.Count == 0)
        {
            profMsg.Add(new WxMsg());
        }
    }

    private void SendProfMsg()
    {
        string errMsg = null;
        if (!WxSimulater.CheckMsgList(profMsg, out errMsg))
        {
            warnMsg.Text = errMsg;
            return;
        }
        Response.Redirect("Submitting.aspx");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveProfMsg(int.Parse(hfCurMsg.Value));
        LoadProfMsg(int.Parse(hfCurMsg.Value));
        BindProfMsg();
    }

    protected void lvMsgList_OnItemCommand(object sender, ListViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "EditMsg":
                int nextMsg = e.Item.DataItemIndex;
                SaveProfMsg(int.Parse(hfCurMsg.Value));
                LoadProfMsg(nextMsg);
                break;
            case "DelMsg":
                int curMsgIndex = int.Parse(hfCurMsg.Value);
                SaveProfMsg(curMsgIndex);
                DelProfMsg(e.Item.DataItemIndex);
                LoadProfMsg(0);
                break;
            case "AddMsg":
                if (profMsg.Count < 8)
                {
                    SaveProfMsg(int.Parse(hfCurMsg.Value));
                    profMsg.Add(new WxMsg());
                    LoadProfMsg(profMsg.Count - 1);
                }
                else
                {
                    warnMsg.Text = "最多只能同时发8条图文消息！";
                }
                break;
            case "SendMsg":
                SaveProfMsg(int.Parse(hfCurMsg.Value));
                BindProfMsg();
                SendProfMsg();
                break;
        }
    }
}