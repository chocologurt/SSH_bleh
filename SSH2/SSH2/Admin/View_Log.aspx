<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="View_Log.aspx.cs" Inherits="ASPJ.Admin.View_Log" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Content/admin_general.css" rel="stylesheet" type="text/css" />
    <style>
        .queryBox{
            width:500px;
            max-width:500px;
        }
    </style>
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
    <script language="javascript" type="text/javascript">
    function pageLoad() {
        ShowPopup();
        setTimeout(HidePopup, 2000);
    }

    function ShowPopup() {
        $find('modalpopup').show();
        //$get('Button1').click();
    }

    function HidePopup() {
        $find('modalpopup').hide();
        //$get('btnCancel').click();
    }
</script>
    <style>
.ModalPopupBG{
    background-color: #a0abbc;
    filter: alpha(opacity=50);
    opacity: 0.7;
}

.postDiv
{
    min-width:200px;
    background:white;
    padding:20px;
}
.replyBox{
    width:200px;
}
.searchTable th{
    color:white;
    background-color:#ea6153;
    padding:10px;
    
}
.searchTable td{
    padding:10px;
}
.searchTable{
    margin-bottom:10px;
}
tr:nth-child(odd){background: #e9e9e9;}
.searchTable {
    
}
.logTable td{
    padding:5px;
}
.logTable th{
    color:white;
    background-color:#ea6153;
    padding:10px;
}
    </style>
    <div class="admin_content" >
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
             <asp:View ID="View1" runat="server">
        <h1>Select Log file to view</h1>
        <p><asp:DropDownList ID="logList" runat="server" AutoPostBack="True"
            onselectedindexchanged="getColumns">
            <asp:ListItem Text="access.log" Value="access.log"/>
            <asp:ListItem Text="auth.log" Value="auth.log" />

           </asp:DropDownList></p>
        <asp:BulletedList ID="columns" runat="server" >
        </asp:BulletedList>
                 <asp:HiddenField ID="title" runat="server" Value="Custom Search"/>
        <asp:TextBox ID="query" runat="server" CssClass="queryBox" ></asp:TextBox>
        <asp:Button ID="view" runat="server" Text="search" OnClick="search_clicked" />
        <asp:Button ID="saveSearch" runat="server" Text="Save Search" />
                 <asp:DropDownList ID="TimeList" runat="server" AutoPostBack="True"
            onselectedindexchanged="getColumns">
                    <asp:ListItem Text="All Time" Value="0"/>
                    <asp:ListItem Text="Last Hour" Value="1" />
                     <asp:ListItem Text="Last 4 Hour" Value="4" />
                     <asp:ListItem Text="Last 24 Hour" Value="24" />
                     <asp:ListItem Text="Last Week" Value="168" />
           </asp:DropDownList>
                 <br />
           <asp:Button ID="rawButton" runat="server" Text="Raw Data" OnCommand="switchDataView" CommandArgument="0"/>  
           <asp:Button ID="pieButton" runat="server" Text="Pie Chart" OnCommand="switchDataView" CommandArgument="1" />
           <asp:Button ID="barButton" runat="server" Text="Bar Chart" OnCommand="switchDataView" CommandArgument="2"/>
                 <br />
                <asp:MultiView ID="DataView" runat="server" ActiveViewIndex="0">
                    <asp:View ID="tableView" runat="server">
                        <asp:Table ID="LogTable" runat="server" CssClass="logTable">
                        </asp:Table>
                        </asp:View>
                    <asp:View ID="pieView" runat="server">
                        <asp:Image ID="pieChart" runat="server" />
                    </asp:View>
                    <asp:View ID="barView" runat="server">
                        <asp:Image ID="barChart" runat="server" />
                    </asp:View>
                </asp:MultiView>
               </asp:View>
            <asp:View ID="View2" runat="server">
                <h2>View Saved Search</h2>
                <p></p>
                <asp:Table ID="searchTable" runat="server" CssClass="searchTable"></asp:Table>
                <asp:Table ID="reportTable" runat="server" CssClass="searchTable"></asp:Table>
                </asp:View>
        </asp:MultiView>
    </div>
    <ajaxToolKit:modalpopupextender id="ModalPopupExtender1" runat="server" 
	cancelcontrolid="btnCancel" 
	targetcontrolid="saveSearch" popupcontrolid="postPanel" 
	popupdraghandlecontrolid="PopupHeader" drag="true" 
	backgroundcssclass="ModalPopupBG">
</ajaxToolKit:modalpopupextender>
    <asp:panel id="postPanel" style="display: none" runat="server">
	<div class="postDiv">
                <div class="PopupHeader" id="PopupHeader">Enter search name</div>
                <div class="PopupBody">
                    <asp:TextBox CssClass="replyBox" ID="searchName" runat="server"></asp:TextBox>
                </div>
                <div class="Controls">
                    <asp:Button ID="btnPost" runat="server" Text="Post" onClick="saveSearches"/>
                    <input id="btnCancel" type="button" value="Cancel" />
		</div>
        </div>
</asp:panel>
</asp:Content>
