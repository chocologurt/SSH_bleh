<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProfilePage.aspx.cs" Inherits="SSH2.Account.ProfilePage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="profilemain" runat="server">
        <div id="profilepageHeader">
            <asp:Image runat="server" ID="userPicture" ImageUrl="~/Imagesss/introductions_800_clr_10909-1.png" Height="100px" Width="100px" />
            <asp:Label runat="server" ID="userFullNameText" Text="Full Name: James Seah" Font-Size="Large"></asp:Label>
        </div>
        <div id="row">
            <asp:Button runat="server" ID="personalInfoButton" CssClass="btn btn-primary" Text="Personal Information" OnClick="personalInfoButton_Click"/>
            <asp:Button runat="server" ID="SkillsOwnedButton" CssClass="btn btn-primary" Text="Skilled Owned" OnClick="SkillsOwnedButton_Click" />
            <asp:Button runat="server" ID="userPostsButton" CssClass="btn btn-primary" Text="My Posts" OnClick="userPostsButton_Click" />
        </div>

        <div id="content" runat="server">
            <asp:MultiView runat="server" ID="MultiView1">
                <asp:View ID="personalInfoView" runat="server">
                    <div id="personalInfoContent">
                        <asp:Label runat="server" ID="userEmailLabel" Text="Email:" Font-Size="XX-Large"></asp:Label>
                        <asp:Label runat="server" ID="userEmailText" Text="-insert something here-" Font-Size="XX-Large"></asp:Label>
                        <hr />
                        <asp:Label runat="server" ID="userUsernameLabel" Text="Username:" Font-Size="XX-Large"></asp:Label>
                        <asp:Label runat="server" ID="userUsernameText" Text="-insert something here-" Font-Size="XX-Large"></asp:Label>
                    </div>
                </asp:View>
                <asp:View ID="skillsOwnedView" runat="server">
                    <div id="skillsOwnedContent">
                        <asp:Label runat="server" Text="Testing for Skills Owned" Font-Size="XX-Large"></asp:Label>
                    </div>
                </asp:View>
                <asp:View ID="userPostsView" runat="server">
                    <asp:Label runat="server" Text="Testing for User Posts" Font-Size="XX-Large"></asp:Label>
                </asp:View>
            </asp:MultiView>
        </div>
    </div>
</asp:Content>
