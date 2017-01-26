<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Console_Login.aspx.cs" Inherits="ASPJ.Admin.Console_Login" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
           <script language="JavaScript" type="text/javascript" src="/Scripts/jsencrypt.js"></script>
    <script language="JavaScript" type="text/javascript" src="/Scripts/jquery.jcryption.3.1.0.js"></script>
    <script language="JavaScript" type="text/javascript" src="/Scripts/jquery-1.10.2.js"></script>
    <style> 
        body{
            background: #50a3a2;	
            background: linear-gradient(to bottom right, #50a3a2 0%, #53e3a6 100%);
            background: -webkit-linear-gradient(top left, #50a3a2 0%, #53e3a6 100%);
            background: -moz-linear-gradient(top left, #50a3a2 0%, #53e3a6 100%);
            background: -o-linear-gradient(top left, #50a3a2 0%, #53e3a6 100%);
            
        }
    </style>
    
    <div class="container">
		<h1>Welcome</h1>
        <p>Please login with your key</p>
        <input type="file" id="fileinput"  />
        <asp:Button ID="btnValidate" runat="server" Text="Authenticate" OnClientClick="" OnClick="validate" />
        <asp:Label ID="error" runat="server" Text=""></asp:Label>
        <asp:HiddenField ID="encryptedChallenge" runat="server" />
        <asp:HiddenField ID="privKey" runat="server" />
	</div>
    <script type="text/javascript">
        var privKey;
  function readSingleFile(evt) {
    //Retrieve the first (and only!) File from the FileList object
    var f = evt.target.files[0]; 

    if (f) {
      var r = new FileReader();
      r.onload = function(e) { 
	      var contents = e.target.result;
	      privKey = contents;
	  //    alert(privKey);
	      encrypt();
      }
      r.readAsText(f);
    } else { 
      alert("Failed to load file");
    }
  }

  document.getElementById('fileinput').addEventListener('change', readSingleFile, false);

            
            
                
  function encrypt() {

                
          //      alert(privKey);
      //          var rsa = new JSEncrypt();
      //          rsa.setPublicKey(privKey);
      //          var challenge = "<%=challenge%>";
      //          alert(challenge);
      //          var encrypted = rsa.encrypt(challenge);
      //          alert(encrypted);
                
                var privateKey = document.getElementById("<%=privKey.ClientID%>");
            //    alert(encryptedChallenge);
                privateKey.value = privKey;
            }
        </script>
</asp:Content>
