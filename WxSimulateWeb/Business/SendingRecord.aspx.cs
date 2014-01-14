using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Business_SendingRecord : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            uidUser.Value = Membership.GetUser(User.Identity.Name).ProviderUserKey.ToString();
            //GridView1.DataBind();
        }
    }
    protected string GiveMeActive(object oItem)
    {
        int taskStatus = (int)DataBinder.Eval(oItem, "taskStatus");

        switch (taskStatus)
        {
            case 100:
                return "成功";
            case 0:
                return "等待执行";
            case 1:
                return "登陆需要输入验证码";
            case 2:
                return "密码或账号错误";
            case 3:
                return "其他登陆错误";
            case 4:
                return "上传图片错误";
            case 99:
                return "今天该公众号的群发数量已超过微信限制";
            case 98:
                return "消息中可能含有具备安全风险的链接，请检查";
            case 97:
                return "发送信息时需要人工输入验证码，无法发送！";
            case 96:
                return "发送失败，未知代码";
            default:
                return "";
        }
        
    }
}