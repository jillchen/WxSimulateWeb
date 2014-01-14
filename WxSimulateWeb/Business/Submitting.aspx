<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Submitting.aspx.cs" Inherits="Business_Submitting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Timer ID="UpdateTimer" runat="server" Interval="5000" OnTick="UpdateTimer_Tick"
        Enabled="true">
    </asp:Timer>
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="UpdateTimer" EventName="Tick" />
        </Triggers>
        <ContentTemplate>
            <asp:Label ID="Label1" runat="server" Text="正在发送请稍候……"></asp:Label>
            <asp:HiddenField ID="logFileSize" runat="server" Value="0" />
            <div id="divProcMsg" runat="server">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
