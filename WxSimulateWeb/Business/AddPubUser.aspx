<%@ Page Title="公众号管理" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="AddPubUser.aspx.cs" Inherits="Business_AddPubUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript" charset="utf-8" src="../Scripts/Base64.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-v1.9.1.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:GridView ID="gvUserList" runat="server" AllowSorting="True" DataSourceID="dbUserList"
        AutoGenerateColumns="False" AllowPaging="True" DataKeyNames="uidWxPubUsr">
        <Columns>
            <asp:BoundField DataField="uidWxPubUsr" HeaderText="uidWxPubUsr" SortExpression="uidWxPubUsr"
                ReadOnly="True" Visible="False" />
            <asp:BoundField DataField="pubAccount" HeaderText="公众号账号" SortExpression="pubAccount" />
            <asp:BoundField DataField="pubName" HeaderText="公众号名字" SortExpression="pubName" />
            <%--<asp:CommandField ShowDeleteButton="True" DeleteText="删除" />--%>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" OnClientClick="return confirm('确定删除？')">删除</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <div style="width: 200px;">
                尚未保存任何公众号
            </div>
        </EmptyDataTemplate>
    </asp:GridView>
    <asp:SqlDataSource ID="dbUserList" runat="server" ConnectionString="<%$ ConnectionStrings:WxSimulateDB %>"
        OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [uidWxPubUsr],[pubAccount],[pubName] FROM [WxPubUsersDB] WHERE ([uidUser] = @uidUser)"
        ConflictDetection="CompareAllValues" DeleteCommand="DELETE FROM [WxPubUsersDB] WHERE [uidWxPubUsr] = @original_uidWxPubUsr">
        <DeleteParameters>
            <asp:Parameter Name="original_uidWxPubUsr" Type="Object" />
        </DeleteParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="hfGuidUser" Name="uidUser" PropertyName="Value"
                Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    <br />
    <asp:HiddenField ID="hfGuidUser" runat="server" />
    <div style="float: left; outline-width: thin; outline-color: Gray; outline-style: double;">
        <asp:Label ID="lblDivTitle" runat="server" Text="添加新的公共号" Style="margin-left: 18px"></asp:Label><br />
        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnAdd">
            <span class="divSendMsg">
                <asp:Label ID="lblUid" runat="server" Text="账号" Style="margin-left: 18px"></asp:Label>
                <asp:TextBox ID="tbUid" runat="server" Style="margin-left: 18px"></asp:TextBox>
                <asp:HiddenField ID="hfUid" runat="server" />
                <asp:Label ID="warnUid" runat="server" Text="" Style="margin-left: 18px;" ForeColor="Red"></asp:Label>
            </span><span class="divSendMsg">
                <asp:Label ID="lblPw" runat="server" Text="密码" Style="margin-left: 18px"></asp:Label>
                <asp:TextBox ID="tbPw" runat="server" TextMode="Password" Style="margin-left: 18px"></asp:TextBox>
                <asp:HiddenField ID="hfPw" runat="server" />
                <asp:Label ID="warnPw" runat="server" Text="" Style="margin-left: 18px" ForeColor="Red"></asp:Label>
            </span><span class="divSendMsg" id="divImgCode" runat="server" visible="false">
                <asp:TextBox ID="tbImgCode" runat="server"></asp:TextBox>
                <asp:HiddenField ID="hfImgCode" runat="server" />
                <asp:Image ID="imImgCode" ImageUrl="" runat="server" />
                <asp:LinkButton ID="lbtnImgCode" runat="server" OnClick="lbtnImgCode_Click">换一张</asp:LinkButton>
            </span><span class="divSendMsg" style="width: 100%">
                <asp:Button ID="btnAdd" runat="server" Text="添加" Style="margin-left: 100px; float: left;"
                    OnClick="btnAdd_Click" OnClientClick="return AddPost()" />
                <asp:Label ID="warnAdd" runat="server" Text="" Style="margin-left: 18px" ForeColor="Red"></asp:Label>
            </span>
        </asp:Panel>
    </div>
    <div style="float: left; clear: left; margin-top: 10px; outline-width: thin; outline-color: Gray;
        outline-style: double;">
        <asp:Label ID="lblFind" runat="server" Text="删除已有的公共号" Style="margin-left: 18px"></asp:Label><br />
        <asp:Panel ID="Panel2" runat="server" DefaultButton="btnFind">
            <span class="divSendMsg">
                <asp:Label ID="lblInputAccount" runat="server" Text="请输入公众号的账号" Style="margin-left: 18px"></asp:Label>
                <asp:TextBox ID="tbInputAccount" runat="server" Style="margin-left: 18px"></asp:TextBox>
                <asp:HiddenField ID="hfInputAccount" runat="server" />
                <asp:Button ID="btnFind" runat="server" Text="查找" OnClick="btnFind_Click" OnClientClick="return FindPost()" />
                <asp:Label ID="warnInputAccount" runat="server" Text="" Style="margin-left: 18px;"
                    ForeColor="Red"></asp:Label>
            </span>
        </asp:Panel>
        <div style="float: left; margin-left: 100px; clear: left">
            <asp:GridView ID="gvFindPubUsr" runat="server" AutoGenerateColumns="False" DataKeyNames="uidWxPubUsr"
                Visible="false" OnRowDeleted="gvDeleteUser" DataSourceID="dbFindPubUsr">
                <Columns>
                    <asp:BoundField DataField="uidWxPubUsr" HeaderText="uidWxPubUsr" SortExpression="uidWxPubUsr"
                        ReadOnly="True" Visible="False" />
                    <asp:BoundField DataField="pubAccount" HeaderText="公众号账号" SortExpression="pubAccount" />
                    <asp:BoundField DataField="pubName" HeaderText="公众号名字" SortExpression="pubName" />
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="DeleteButton" runat="server" CommandName="Delete" OnClientClick="return confirm('确定删除？')">删除</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:CommandField ShowDeleteButton="True" DeleteText="删除" />--%>
                </Columns>
                <EmptyDataTemplate>
                    <div style="width: 200px; text-align: center; color: red">
                        没有找到输入的账号！
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
            <asp:SqlDataSource ID="dbFindPubUsr" runat="server" ConnectionString="<%$ ConnectionStrings:WxSimulateDB %>"
                OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [uidWxPubUsr],[pubAccount],[pubName] FROM [WxPubUsersDB] WHERE ([uidUser] = @uidUser AND [pubAccount]= @inAccount) "
                ConflictDetection="CompareAllValues" DeleteCommand="DELETE FROM [WxPubUsersDB] WHERE [uidWxPubUsr] = @original_uidWxPubUsr">
                <DeleteParameters>
                    <asp:Parameter Name="original_uidWxPubUsr" Type="Object" />
                </DeleteParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="hfGuidUser" Name="uidUser" PropertyName="Value"
                        Type="String" />
                    <asp:ControlParameter ControlID="tbInputAccount" Name="inAccount" PropertyName="Text"
                        Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </div>
    <script type="text/javascript">
        $('form').submit(function () {
            SavePage();
        });

        var hfInputAccount = document.getElementById('<%= hfInputAccount.ClientID %>');
        var tbInputAccount = document.getElementById('<%= tbInputAccount.ClientID %>');

        var hfUid = document.getElementById('<%= hfUid.ClientID %>');
        var tbUid = document.getElementById('<%= tbUid.ClientID %>');
        var hfPw = document.getElementById('<%= hfPw.ClientID %>');
        var tbPw = document.getElementById('<%= tbPw.ClientID %>');
        var hfImgCode = document.getElementById('<%= hfImgCode.ClientID %>');
        var tbImgCode = document.getElementById('<%= tbImgCode.ClientID %>');

        LoadPage();

        function FindPost() {
            if (tbInputAccount.value == "") {
                alert("内容不能为空");
                return false;
            }
            return true;
        }

        function AddPost() {
            if (tbUid.value == "" || tbPw.value == "" || (tbImgCode != null && tbImgCode.value != null)) {
                alert("内容不能为空");
                return false;
            }
            return true;
        }

        function SavePage() {
            hfUid.value = base64encode(utf16to8(tbUid.value));
            hfPw.value = base64encode(utf16to8(tbPw.value));
            hfInputAccount.value = base64encode(utf16to8(tbInputAccount.value));

            tbUid.value = "";
            tbPw.value = "";
            tbInputAccount.value = "";

            if (tbImgCode != null) {
                hfImgCode.value = base64encode(utf16to8(tbImgCode.value));
                tbImgCode.value = "";
            }
        }

        function LoadPage() {
            tbUid.value = utf8to16(base64decode(hfUid.value));
            tbPw.value = utf8to16(base64decode(hfPw.value));
            tbInputAccount.value = utf8to16(base64decode(hfInputAccount.value));
            if (hfImgCode != null) {
                tbImgCode.value = utf8to16(base64decode(hfImgCode.value));
            }
        }
    </script>
</asp:Content>
