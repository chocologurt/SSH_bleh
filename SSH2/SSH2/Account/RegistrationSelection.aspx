<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistrationSelection.aspx.cs" Inherits="SSH_ASPJ.Account.RegistrationSelection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="form-horizontal">
        <h4>Please choose one of the following by clicking on the image</h4>
        <hr />
        <asp:ValidationSummary runat="server" CssClass="text-danger" />


            <div class="Mentee">
                <asp:ImageButton runat="server"
                    ImageUrl="~/Imagesss/user_mentee_med.jpg" ID="menteeButton" OnClick="menteeButton_Click"/>
                <asp:Label runat="server" CssClass="col-md-2 control-label" AssociatedControlID="menteeButton" Font-Size="Large">Mentee</asp:Label>
            </div>

            <div class="Mentor">
                <asp:ImageButton runat="server"
            ImageUrl="~/Imagesss/introductions_800_clr_10909-1.png" ID="mentorButton" Height="100px" Width="100px" OnClick="mentorButton_Click"/>
                <asp:Label runat="server" CssClass="col-md-2 control-label" AssociatedControlID="mentorButton" Font-Size="Large">Mentor</asp:Label>
            </div>
        </div>
   
</asp:Content>