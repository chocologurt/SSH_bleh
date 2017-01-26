<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="View_Report.aspx.cs" Inherits="ASPJ.Admin.View_Report" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/admin_general.css" rel="stylesheet" type="text/css" />
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
        <h2>View Report</h2>
         <ajaxToolkit:LineChart ID="lineChart" runat="server" ChartHeight="300" ChartWidth="450"
    ChartType="Basic" ChartTitleColor="#0E426C" Visible="false" CategoryAxisLineColor="#D08AD9"
    ValueAxisLineColor="#D08AD9" BaseLineColor="#A156AB">
</ajaxToolkit:LineChart>
    </div>
    </div>
</asp:Content>
