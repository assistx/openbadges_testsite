<%@ Page Title="MS OpenBadges Test Site" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Debug="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FeaturedContent" runat="server">
    <script type="text/javascript" src="https://msbackpack.azurewebsites.net/issuer.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Thank you for visiting the Microsoft Research OpenBadges Test Site</h2>
    
    <span class="failureNotification">
        <asp:Literal ID="FailureText" runat="server"></asp:Literal>
    </span>

    <asp:Panel id="newPanel" runat="server">
        <fieldset class="register">
            <p>
                <asp:Label ID="contactLbl" runat="server" AssociatedControlID="contact">Enter Your Email To Receive Your Badge</asp:Label>
                <asp:Textbox name="contact" type="text" value="" id="contact" style="width: 100%" runat="server" />
            </p>
        </fieldset>
        <p class="submitButton">
           <asp:Button ID="apiBtn" runat="server" Text="Get Your Open Badge" OnClick="apiBtn_OnClick" />
        </p>
    </asp:Panel>
</asp:Content>
