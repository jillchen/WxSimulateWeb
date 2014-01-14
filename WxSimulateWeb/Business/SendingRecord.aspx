<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="SendingRecord.aspx.cs" Inherits="Business_SendingRecord" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="taskId"
        DataSourceID="SqlDataSource1" AllowPaging="True" AllowSorting="True">
        <Columns>
            <asp:BoundField DataField="taskId" HeaderText="taskId" ReadOnly="True" Visible="false"
                SortExpression="taskId" />
            <asp:BoundField DataField="uidUser" HeaderText="uidUser" Visible="false" SortExpression="uidUser" />
            <asp:BoundField DataField="uidWxPubUsr" HeaderText="uidWxPubUsr" Visible="false"
                SortExpression="uidWxPubUsr" />
            <asp:BoundField DataField="pubAccount" HeaderText="账号" SortExpression="pubAccount" />
            <asp:BoundField DataField="pubName" HeaderText="名称" SortExpression="pubName" />
            <asp:BoundField DataField="taskEndTime" HeaderText="任务结束时间" SortExpression="taskEndTime" />
            <asp:TemplateField HeaderText="状态码" SortExpression="taskStatus">
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("taskStatus") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="任务状态">
                <ItemTemplate>
                    <asp:Label ID="label2" runat="server"><%#GiveMeActive(Container.DataItem)%></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="taskRetCode" HeaderText="微信返回代码" SortExpression="taskRetCode" />
            <asp:BoundField DataField="taskRetMsg" HeaderText="微信返回信息" SortExpression="taskRetMsg" />
        </Columns>
        <EmptyDataTemplate>
            暂无任何发送记录
        </EmptyDataTemplate>
    </asp:GridView>
    <asp:HiddenField ID="uidUser" runat="server" />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:WxSimulateDB %>"
        SelectCommand="SELECT dbo.SendTask.taskId, dbo.SendTask.uidWxPubUsr, dbo.SendTask.MsgId, dbo.SendTask.taskEndTime, dbo.SendTask.taskStatus, dbo.SendTask.taskRetCode, dbo.SendTask.taskRetMsg, dbo.WxPubUsersDB.uidWxPubUsr, dbo.WxPubUsersDB.pubName, dbo.SendTask.uidUser, dbo.WxPubUsersDB.pubAccount FROM dbo.SendTask INNER JOIN dbo.WxPubUsersDB ON dbo.SendTask.uidWxPubUsr = dbo.WxPubUsersDB.uidWxPubUsr WHERE (dbo.SendTask.uidUser = @uidUsr) ORDER BY dbo.SendTask.taskEndTime DESC ">
        <SelectParameters>
            <asp:ControlParameter ControlID="uidUser" Name="uidUsr" PropertyName="Value" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
