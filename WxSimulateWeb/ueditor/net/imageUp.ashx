<%@ WebHandler Language="C#" Class="imageUp" %>


using System;
using System.Web;
using System.IO;
using System.Collections;
using System.Web.Security;
using WxSimulateWeb.ueditor.net;

public class imageUp : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";

        //上传配置
        string pathbase = "UploadedImages/" + context.Request["SESSION"] + "/";    //保存路径
        int size = 300;                     //文件大小限制,单位kb 
        string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };                    //文件允许格式

        string type = context.Request["type"];
        string editorId = context.Request["editorid"];


        //上传图片
        Hashtable info = new Hashtable();
        UmUploader up = new UmUploader();
        info = up.upFile(context, "~/" + pathbase, filetype, size); //获取上传状态

        if (type == "ajax")
        {
            HttpContext.Current.Response.Write(info["url"]);
        }
        else
        {
            HttpContext.Current.Response.Write("<script>parent.UM.getEditor('" + editorId + "').getWidgetCallback('image')('" + info["url"] + "','" + info["state"] + "')</script>");//回调函数
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
