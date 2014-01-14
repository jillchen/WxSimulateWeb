using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using WxSimulateWeb.App_Code;
using Simple.Data;
using WxPlatfromWraper;
using WxPlatfromWraper.WxJson;

public partial class Business_AddPubUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            hfGuidUser.Value = Membership.GetUser(User.Identity.Name).ProviderUserKey.ToString();
        }

        tbImgCode.Text = string.Empty;
        tbInputAccount.Text = Base64.base64Decode(hfInputAccount.Value);
        warnAdd.Text = "";
        //gvFindPubUsr.Visible = false;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Guid uidUser = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
        //首先判断是否已存在该公众号
        var db = Database.Open();
        string addUid = Base64.base64Decode(hfUid.Value);
        string addPw = Base64.base64Decode(hfPw.Value);
        string addImgCode = Base64.base64Decode(hfImgCode.Value);
        bool isExist = db.WxPubUsersDB.Exists(db.WxPubUsersDB.pubAccount == addUid &&
                       db.WxPubUsersDB.uidUser == uidUser);
        if (isExist)
        {
            warnAdd.Text = "已存在该公众号！若要修改请先删除！";
            return;
        }

        //检查账号是否能正常登陆
        WxPublicUser usr = WxSimulater.ExecLogin(addUid, addPw, addImgCode);
        string retMsg =  WxSimulater.ParseLoginRetInfo(usr.LoginRetInfo);
        switch (usr.LoginRetInfo.ErrCode)
        {
            case 65201:
            case 65202:
            case 0:
                //登陆成功后，可以添加到数据库
                try
                {
                    db.WxPubUsersDB.Insert(uidUser: uidUser, pubAccount: addUid, pubPw: addPw, pubName: usr.User_name);
                    warnAdd.Text = "添加成功";
                    hfUid.Value = "";
                    hfPw.Value = "";
                    hfImgCode.Value = "";
                    gvUserList.DataBind();
                }
                catch (Exception ex)
                {
                    warnAdd.Text = "添加失败，无法保存到数据库中<br />" + ex.Message;
                }
                break;
            case -6:
            case -32:
                warnAdd.Text = "请输入图中的验证码";
                imImgCode.ImageUrl = "https://mp.weixin.qq.com/cgi-bin/verifycode?username=jane_199039@163.com&r=" + DateTime.Now.Ticks;
                divImgCode.Visible = true;
                break;
            default:
                warnAdd.Text = retMsg;
                break;
        }
    }

    protected void lbtnImgCode_Click(object sender, EventArgs e)
    {
        imImgCode.ImageUrl = "https://mp.weixin.qq.com/cgi-bin/verifycode?username=jane_199039@163.com&r=" + DateTime.Now.Ticks;
    }

    protected void btnFind_Click(object sender, EventArgs e)
    {
        gvFindPubUsr.Visible = true;
        gvFindPubUsr.DataBind();
    }

    protected void gvDeleteUser(object sender, EventArgs e)
    {
        gvUserList.DataBind();
    }
}