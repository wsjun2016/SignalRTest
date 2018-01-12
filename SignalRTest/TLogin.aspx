<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TLogin.aspx.cs" Inherits="SignalRTest.TLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        * {
        /*border:1px solid red;*/
        }
        .loginBox{
            position:relative;
            top:0;
            left:0;
            right:0;
            bottom:0;
            margin:50px auto;
            border:1px solid darkgrey;
            width:400px;
            height:300px;           
        }
        p,div.errormsg {
            width:70%;
            height:30px;       
            margin-left:15%;   
            line-height:30px;  
            text-align:center;

        }
        p>span {
            display:inline-block;
            width:70px;
            float:left;
            text-align:right;
        }
      
        .btnSubmit,.btnReset {
            height:25px;
            width:70px;
            margin-left:10px;
        }
       
        .tbx {
            width:190px;
            height:20px;            
        }
        .lblMsg {
        color:red;
        }
    </style>
    <script src="scripts/jquery-1.10.2.min.js"></script>
    <script>
        $(function () {
            $(".btnReset").click(function () {
                $("#tbxUserName").val('');
                $("#txbPassword").val('');
                $("#tbxUserName").focus();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="loginBox">
        <p style="margin-top:80px;">
            <span>用户名</span>            
                <asp:TextBox ID="tbxUserName" runat="server" CssClass="tbx"></asp:TextBox>
        </p>
        <p>
            <span>密&emsp;码</span>            
                <asp:TextBox ID="tbxPassword" TextMode="Password" runat="server" CssClass="tbx"></asp:TextBox>            
        </p>
         <p style="text-align:left;">
            <span>&nbsp;</span>            
               <asp:Button ID="btnSubmit" runat="server" CssClass="btnSubmit" Text="登&emsp;陆" OnClick="btnSubmit_Click" />       
              <input type="button" class="btnReset"  value="重&emsp;置"/>     
        </p> 
        <div class="errormsg"><asp:Label ID="lblMsg" runat="server" Text="" CssClass="lblMsg" Visible="false"></asp:Label></div>    
    </div>
    </form>
</body>
</html>
