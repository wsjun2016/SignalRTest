<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LiveDataList.aspx.cs" Inherits="SignalRTest.LiveDataList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
       div,span, ul, ul >li  {
            /*border:1px solid red;*/
        }
        .lv-container {
            width:800px;
            min-height:50px;
            position:relative;
            top:0;
            left:0;
            right:0;
            bottom:0;
            margin:10px auto;
        }
        
        ul.lv-list {
            list-style:none;
            max-width:100%;
            min-height:50px;
            /*position:relative;
            top:0;
            left:0;
            right:0;
            bottom:0;
            margin:10px auto;*/           
        }

            ul.lv-list li {
                list-style-type:none;
                display:inline-block;
                float:left;
                margin:0px 10px 20px 10px;
                /*border:1px solid black;*/
                max-width:200px;
                max-height:130px;
                text-align:center;
                cursor:pointer;
            }

        .lv-frontcover {
            width:200px;
            height:100px;
        }
        .title {
            display:block;
            width:100%;
            height:30px;
           margin:10px 0px;
            text-align:center;
            font-weight:bold;
        }

    </style>
    <script src="scripts/jquery-1.6.4.min.js"></script>
    <script>
        $(function () {
            $(".lv-list li").click(function () {
                var id =parseInt($(this).attr("data-id") || "0");
                if (id > 0) {
                    window.open("wfrmOne.aspx?sid=" + id, "_blank", "", true);
                }
            });

        });

    </script>
</head>
<body>
    <div class="lv-container">
    <span class="title">直播列表</span>
   <ul class="lv-list">
       <%foreach (var item in _list) { %>
       <li data-id="<%=item.ID %>">
           <img class="lv-frontcover"  src="<%=item.FrontCover %>"/>
           <span class="lv-title"><%=item.Title %></span>
       </li>
      <%} %>
   </ul>
        </div>
</body>
</html>
