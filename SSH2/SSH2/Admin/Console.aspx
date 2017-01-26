<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Console.aspx.cs" Inherits="ASPJ.Admin.Console" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <link href="Content/admin_general.css" rel="stylesheet" type="text/css" />
     <!-- <style>
        body{
            background: #50a3a2;	
            background: linear-gradient(to bottom right, #50a3a2 0%, #53e3a6 100%);
            background: -webkit-linear-gradient(top left, #50a3a2 0%, #53e3a6 100%);
            background: -moz-linear-gradient(top left, #50a3a2 0%, #53e3a6 100%);
            background: -o-linear-gradient(top left, #50a3a2 0%, #53e3a6 100%);
            
        }
    </style>-->
    <div class="container">
 <ul class="nav_admin">
     <li onclick="location.href = 'Console.aspx';">Home<span class="arrow">»</span></li>
  <li>Admin Settings<span class="arrow">»</span>
<ul>
      <h4>Settings</h4>
      <hr />
        <li>
          <ul>
            <li><a href="Change_Key.aspx" >Change Key Pair</a></li>
          </ul>
      </li>
    </ul>
  </li>
  <li>Logging<span class="arrow">»</span>
      <ul>
      <h4>Logging</h4>
      <hr />
        <li>
          <ul>
            <li><a href="View_Log.aspx" >View/Analyse Log Files</a></li>
              <li><a href="View_Log.aspx?view=2">View Saved Search</a></li>
          </ul>
      </li>
    </ul>
  </li>
  <li>Alerts and Notifications <span class="arrow">»</span>
      <ul>
      <h4>Alerts and Notifications</h4>
      <hr />
        <li>
          <ul>
            <li>Alerts</li>
          </ul>
      </li>
    </ul>
  </li>
  <li>Monitor Users <span class="arrow">»</span></li>
</ul>
    <div class="admin_content" >
        <h1>Welcome</h1>
        <p><asp:Label ID="lastLogin" runat="server" Text="Label"></asp:Label>
            </p>
    </div>
    </div>
</asp:Content>
