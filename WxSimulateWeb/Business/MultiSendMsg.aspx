<%@ Page Title="群发图文信息" Language="C#" MasterPageFile="./MultiSendSite.master" AutoEventWireup="true"
    CodeFile="MultiSendMsg.aspx.cs" Inherits="Business_MultiSendMsg" ValidateRequest="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../ueditor/themes/default/css/umeditor.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/jquery-v1.9.1.js"></script>
    <script type="text/javascript" charset="utf-8" src="../ueditor/umeditor.js"></script>
    <script type="text/javascript" charset="utf-8" src="../ueditor/umeditor.config.js"></script>
    <script type="text/javascript" charset="utf-8" src="../Scripts/Base64.js"></script>
    `
    <link href="../Scripts/plupload/jquery.plupload.queue/css/jquery.plupload.queue.css"
        rel="stylesheet" type="text/css" />
    <%--<link href="../style/Reset.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript" src="../scripts/plupload/plupload.full.js"></script>
    <script type="text/javascript" src="../scripts/plupload/jquery.plupload.queue/jquery.plupload.queue.js"></script>
    <script type="text/javascript" src="../scripts/ww.jquery.js"></script>
    <%--<script type="text/javascript" src="../scripts/UploadImages.js"></script>--%>
    <style type="text/css">
        ol
        {
            list-style-type: decimal;
            padding: 0px;
            margin: 20px 0 0 10px;
        }
        ol li
        {
            background-repeat: no-repeat;
            background-position: 0px 5px;
            padding-left: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hfCurMsg" runat="server" />
    <asp:HiddenField ID="hfCoverImgUrl" runat="server" />
    <asp:Label ID="warnMsg" runat="server" Text="" ForeColor="Red" Style="margin-left: 19px"></asp:Label>
    <asp:Panel ID="plMsgList" runat="server" DefaultButton="">
        <asp:ListView ID="lvMsgList" runat="server" OnItemCommand="lvMsgList_OnItemCommand">
            <ItemSeparatorTemplate>
                <br />
            </ItemSeparatorTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="delete" runat="server" OnClientClick="SavePage()" CommandName="DelMsg"
                    Text="删除"></asp:LinkButton>
                《<asp:LinkButton ID="title" runat="server" OnClientClick="SavePage()" CommandName="EditMsg"
                    Text='<%# Eval("Title") %>'>
                </asp:LinkButton>》
                <br />
                <asp:ImageButton ID="cover" runat="server" OnClientClick="SavePage()" CommandName="EditMsg"
                    ImageUrl='<%# Eval("FileId") %>' AlternateText="尚无封面" Height="60px" Width="100%" />
            </ItemTemplate>
            <LayoutTemplate>
                <div style="float: right; outline: grey dashed 1px; margin-right: 0; width: 30%;">
                    <ol id="itemPlaceholderContainer" runat="server" style="margin-right: 10px">
                        <li runat="server" id="itemPlaceholder" style="" />
                    </ol>
                    <asp:Button ID="btnAddMsg" runat="server" CommandName="AddMsg" Text="增加" OnClientClick="SavePage()"
                        Style="margin-left: 80px" />
                    <asp:Button ID="btnSendMsg" runat="server" CommandName="SendMsg" Text="一键发送" OnClientClick="SavePage()"
                        Style="margin-left: 30px" />
                </div>
            </LayoutTemplate>
        </asp:ListView>
    </asp:Panel>
    <%-- <asp:Label ID="warnTitle" runat="server" Text="" ForeColor="Red" Style="margin-left: 19px"></asp:Label>
    <asp:Label ID="warnCoverImg" runat="server" Text="" ForeColor="Red" Style="margin-left: 19px"></asp:Label>
    <asp:Label ID="warnAuthor" runat="server" Text="" ForeColor="Red" Style="margin-left: 19px"></asp:Label>
    <asp:Label ID="warnSourceUrl" runat="server" Text="" ForeColor="Red" Style="margin-left: 19px"></asp:Label>
    <asp:Label ID="warnDigest" runat="server" Text="" ForeColor="Red" Style="margin-left: 19px"></asp:Label>--%>
    <div id="singleMsg" runat="server">
        <asp:Panel ID="plEditMsg" runat="server" DefaultButton="btnSave">
            <div class="divSendMsg">
                <asp:Label ID="lblTitle" runat="server" Text="标题"></asp:Label>
                <asp:TextBox ID="tbTitle" runat="server" MaxLength="64" Width="386px" Style="margin-left: 19px"></asp:TextBox>
                <asp:HiddenField ID="hfTitle" runat='server' />
            </div>
            <div class="divSendMsg" style="width: 50%; margin-left: 0;">
                <div class="containercontent">
                    <div id="ImageContainer">
                        <asp:Image ID="CoverImage" runat="server" AlternateText="暂无封面" Style="width: 50%" />
                    </div>
                    <div id="Uploader" style="width: 450px; margin-left: 0;">
                    </div>
                </div>
            </div>
            <div class="divSendMsg">
                <asp:Label ID="lblAuthor" runat="server" Text="作者（可不填）"></asp:Label>
                <asp:TextBox ID="tbAuthor" runat="server" MaxLength="8" Width="322px" Style="margin-left: 19px"></asp:TextBox>
                <asp:HiddenField ID="hfAuthor" runat='server' />
            </div>
            <div class="divSendMsg">
                <asp:Label ID="lblSourceUrl" runat="server" Text="原文链接（可不填）"></asp:Label>
                <asp:TextBox ID="tbSourceUrl" runat="server" Width="296px" Style="margin-left: 19px"></asp:TextBox>
                <asp:HiddenField ID="hfSourceUrl" runat='server' />
            </div>
            <div class="divSendMsg">
                <asp:Label ID="lblDigest" runat="server" Text="摘要（可不填）"></asp:Label>
                <br />
                <asp:TextBox ID="tbDigest" runat="server" MaxLength="120" TextMode="MultiLine" Width="437px"
                    Height="92px"></asp:TextBox>
                <asp:HiddenField ID="hfDigest" runat='server' />
            </div>
        </asp:Panel>
        <div class="divSendMsg">
            <textarea id="taContent" cols="53" rows="2" runat="server"></textarea><br />
            <asp:Button ID="btnSave" runat="server" Text="保存" OnClientClick="SavePage()" OnClick="btnSave_Click"
                Style="margin: 20px 0 0 150px" />
            <asp:HiddenField ID="hfContent" runat="server" />
        </div>
        <script type="text/javascript">
            window.UMEDITOR_CONFIG.imageUrl += "?SESSION=<%= Membership.GetUser(User.Identity.Name).ProviderUserKey %>&USER=<%= User.Identity.Name %>";
            var tbTitle = document.getElementById('<%= tbTitle.ClientID %>');
            var tbAuthor = document.getElementById('<%= tbAuthor.ClientID %>');
            var tbSourceUrl = document.getElementById('<%= tbSourceUrl.ClientID %>');
            var tbDigest = document.getElementById('<%= tbDigest.ClientID %>');
            var ue = UM.getEditor('<%= taContent.ClientID %>', {
                //这里可以选择自己需要的工具按钮名称,此处仅选择如下七个
                //toolbar: ['fullscreen source undo redo bold italic underline'],
                //focus时自动清空初始化时的内容
                autoClearinitialContent: true,
                //关闭字数统计
                wordCount: true,
                //关闭elementPath
                elementPathEnabled: true,
                //默认的编辑区域高度
                initialFrameHeight: 300
                //更多其他参数，请参考umeditor.config.js中的配置项
            });
            var hfTitle = document.getElementById('<%= hfTitle.ClientID %>');
            var hfAuthor = document.getElementById('<%=hfAuthor.ClientID %>');
            var hfSourceUrl = document.getElementById('<%=hfSourceUrl.ClientID %>');
            var hfDigest = document.getElementById('<%=hfDigest.ClientID %>');
            var hfContent = document.getElementById('<%= hfContent.ClientID %>');

            ue.ready(function () {
                LoadPage();
            })

            function SavePage() {
                hfTitle.value = base64encode(utf16to8(tbTitle.value));
                hfAuthor.value = base64encode(utf16to8(tbAuthor.value));
                hfSourceUrl.value = base64encode(utf16to8(tbSourceUrl.value));
                hfDigest.value = base64encode(utf16to8(tbDigest.value));
                hfContent.value = base64encode(utf16to8(ue.getContent()));

                tbTitle.value = "";
                tbAuthor.value = "";
                tbSourceUrl.value = "";
                tbDigest.value = "";
                ue.setContent("");
            }

            function LoadPage() {
                tbTitle.value = utf8to16(base64decode(hfTitle.value));
                tbAuthor.value = utf8to16(base64decode(hfAuthor.value));
                tbSourceUrl.value = utf8to16(base64decode(hfSourceUrl.value));
                tbDigest.value = utf8to16(base64decode(hfDigest.value));
                ue.setContent(utf8to16(base64decode(hfContent.value)));
            }
        </script>
        <script type="text/javascript">
            $(document).ready(function () {
                // initialize status bar
                showStatus({ autoClose: true });

                $("#Uploader").pluploadQueue({
                    runtimes: 'html5,silverlight,flash,html4',
                    url: '../Business/ImageUploadHandler.ashx?SESSION=<%= Membership.GetUser(User.Identity.Name).ProviderUserKey %>&USER=<%= User.Identity.Name %>',
                    max_file_size: '2mb',
                    chunk_size: '64kb',
                    unique_names: false,
                    // Resize images on clientside if we can
                    resize: { width: 800, height: 600, quality: 90 },
                    // Specify what files to browse for
                    filters: [{ title: "Image files", extensions: "bmp,jpg,jpeg,gif,png"}],
                    flash_swf_url: './plupload/plupload.flash.swf',
                    silverlight_xap_url: './plupload/plupload.silverlight.xap',
                    multiple_queues: true
                });

                // get uploader instance
                var uploader = $("#Uploader").pluploadQueue();


                $("#btnStopUpload").click(function () {
                    uploader.stop();
                });
                $("#btnStartUpload").click(function () {
                    uploader.start();
                });

                // bind uploaded event and display the image
                // response.response returns the last response from server
                // which is the URL to the image that was sent by OnUploadCompleted
                uploader.bind("FileUploaded", function (upload, file, response) {
                    // remove the file from the list
                    upload.removeFile(file);

                    // Response.response returns server output from onUploadCompleted
                    // our code returns the url to the image so we can display it
                    var imageUrl = response.response;

                    //$("#ImageContainer").html("");
                    $("#<%= CoverImage.ClientID %>").attr({ src: imageUrl });
                    //.appendTo($("#ImageContainer"));
                    //alert(imageUrl);
                    $("#<%= hfCoverImgUrl.ClientID %>").val(response.response);
                    //alert($("#<%= hfCoverImgUrl.ClientID %>").val());
                });

                // Error handler displays client side errors and transfer errors
                // when you click on the error icons
                uploader.bind("Error", function (upload, error) {
                    showStatus(error.message, 3000, true);
                });

                // only allow 5 files to be uploaded at once
                uploader.bind("FilesAdded", function (up, filesToBeAdded) {
                    if (up.files.length > 1) {
                        up.files.splice(0, up.files.length - 1);
                        showStatus("只能上传一个封面图片！", 3000, true);
                        return false;
                    }
                    return true;
                });

                //The plUpload labels are not customizable explicitly
                //so if you want to do this you have to directly manipulate the DOM
                setTimeout(function () {
                    $(".plupload_header_title").text("上传封面图片")
                    $(".plupload_header_text").html("支持.bmp .png .jpg .jpeg .gif,文件大小不能超过2MB")
                }, 200);
            });
        </script>
    </div>
</asp:Content>
