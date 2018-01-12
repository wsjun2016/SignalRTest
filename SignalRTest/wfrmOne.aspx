<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfrmOne.aspx.cs" Inherits="SignalRTest.wfrmOne" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style>
        p {
            /*border:1px solid red;*/
        }

        .lv-container {
            width:1000px;
            min-height:480px;
            position:relative;
            top:0;
            left:0;
            right:0;
            bottom:0;
            margin:10px auto;      
            background-color:#f0f0f0;  
        }
        .lv-left {
            width:580px;
            height:480px;
            float:left;
        }
         .lv-right {
            width:400px;
            height:480px;
            float:right;
        }
        .clear {
            display:none;
            clear:both;
        }
        .lv-left p {
            margin-top:0px;
            margin-bottom:0px;
            width:580px;
            height:30px;            
            line-height:30px;    
            background-color:darkgray;
            color:white;    
        }

        .lv-right> p {
            margin-top:0px;
            margin-bottom:0px;
            width:99%;
            height:30px;            
            line-height:30px;   
            text-align:center;
            background-color:white;
            border-bottom:1px solid #f0f0f0;
            border-top:1px solid #f0f0f0;
        }

         .lv-left p>span {            
            float:right;
            margin-right:10px;      
        }
        .lv-left img {
            width:100%;
            height:450px;
            margin-top:0px;
        }
        .im-list {
            width:99%;
            height:400px;
            background-color:#fff;
            margin-top:0px;
            overflow:auto;
        }
        .bar-send {
            width:99%;
            height:50px;
            background-color:#f0f0f0;
        }
            .bar-send  .msgs {
                margin:10px 15px 0px 0px;
                width:70%;
                height:26px;
            }
        #btnSend {
            width:80px;
            height:32px;
            font-size:16px;
            border:none;
            background-color:#bdbdbd;
            color:white;
        }
        .im-contents {
            width:100%;
            padding-top:10px;
            min-height:50px;
        }

        .im-systeminfo {
            width:100%;
            padding-top:10px;
            min-height:30px;
        }
        .im-systeminfo .im-sititle {
            text-align:center;
            font-size:12px;  
        }

         .im-systeminfo .im-sicontent {
            text-align:justify;
            font-size:12px;  
            background-color:#f5f5f5;
            width:80%;
            margin-left:10%;
        }

        .im-headpic {
            max-width:50px;
            max-height:50px;
            float:left;
            margin-left:2px;
        }
       .im-nickname {
           margin:0px 0px 5px 55px;
            /*margin-left:55px;*/
            text-align:left;          
            font-size:12px;  
        }

        .im-msgs {
            margin:0px;
            margin-left:55px;
            text-align:left;
            font-size:12px;  
            word-break:break-all;
            min-height:30px;
            background-color:#f5f5f5;
            line-height:150%;
        }
    </style>

    <script src="scripts/jquery-1.10.2.min.js"></script>
    <script src="scripts/jquery.signalR-2.2.2.min.js"></script>
    <script src="signalr/hubs"></script>

    <script>
        $(function () {
            // 链接hub            
            var ticker = $.connection.groupChatHub;
          
            $.connection.hub.start().done(function () {
                //创建聊天房间
                ticker.server.joinRoom(QueryString("sid")).done(function () {
                    
                });
            });

            // 接收服务端发送的消息
            $.extend(ticker.client, {
                // 接收聊天消息
                publishMsg: function (data) {     
                    if (data) {
                        var html = '';
                        //系统消息
                        if (data.IType == 0) {
                            html = '<div class="im-systeminfo">'+
                                       '<p class="im-sititle">' + data.Name + '&emsp;' + data.Time + '</p>' +
                                       '<p class="im-sicontent">' + data.Msg + '</p>' +
                                    '</div>';
                        }
                        //群聊消息
                        else if (data.IType == 1) {
                            html = '<div class="im-contents">' +
                                      '<img class="im-headpic" src="' + data.Pic + '"/>' +
                                      '<div>' +
                                          '<p class="im-nickname">' + data.Name + '&emsp;' + data.Time + '</p>' +
                                          '<p class="im-msgs">' + data.Msg + '</p>' +
                                      '</div>' +
                                   '</div>';
                        }
                    }

                    
                    $(".im-list").append(html);                   
                    $(".im-list").scrollTop($(".im-list")[0].scrollHeight);
                    
                },                               
                intoRoom: function (data) {
                    //打印出当前连接的ConnectionID
                    //alert(data);
                    
                    //调用ajax接口，将当前用户的ID(Session中)与ConnectionID关联起来
                    var param = { action: 'joinroom', liveId: QueryString("sid"), connectionId: data };
                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        url: 'Index.ashx',
                        data: JSON.stringify(param),
                        success: function (data) {
                            if (data && data.returnValue == 0) {
                                console.log(data.returnMsg);
                            }
                            else alert(data.returnMsg);
                        }
                    });
                }
            });
            

            $("#btnSend").click(function () {
                //获取文本框内容
                var tbxInput = $(this).parent().children(".msgs");
                if (tbxInput) {
                    var msg = tbxInput.val() || '';
                    if (msg.length > 0) {         
                        // 主动发送消息，传入直播ID，和发送的内容。
                        ticker.server.sendMsg(QueryString("sid"), msg);
                        tbxInput.val('');
                    }
                    else tbxInput.focus();
                }                
            });

            $(".msgs").bind("keydown", event, function () {
                if (event.keyCode == 13)
                    $("#btnSend").click();
            });
        });

        //根据key 获取查询字符串中的值
        if (!window.QueryString) {
            window.QueryString = function QueryString(key) {
                var value = '';
                if (typeof key === 'string') {
                    key = key.trim();
                    if (key.length > 0) {
                        var gp = window.location.search.match(new RegExp('[\?\&]' + key + '=([^\&]*)')) || [];
                        if (gp.length > 1)
                            value = gp[1];
                    }
                }
                return value;
            }
        }
        
    </script>

</head>
<body>
   <div class="lv-container">
       <div class="lv-left">
           <p>&emsp;<%=_model.Title %><span>观看人数：<%=_model.ViewCount %></span></p>
           <img src="<%=_model.FrontCover %>"/>
       </div>
       <div class="lv-right">
           <p>聊天区</p>
           <div class="im-list">
              <%-- <div class="im-systeminfo">
                   <p class="im-sititle">系统消息&emsp;2018-01-21 20:31</p>
                   <p class="im-sicontent">直播马上开始啦！直播马上开始啦！直播马上开始啦！直播马上开始啦！直播马上开始啦！直播马上开始啦！</p>
               </div>--%>
              <%-- <div class="im-contents">
                   <img class="im-headpic" src="https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1515647390623&di=1e15100d2044e87f5de5d30113fc1e80&imgtype=jpg&src=http%3A%2F%2Fimg1.imgtn.bdimg.com%2Fit%2Fu%3D3108820096%2C3322755446%26fm%3D214%26gp%3D0.jpg"/>
                   <div>
                       <p class="im-nickname">青云飘零&emsp;2018-01-11 10:27</p>
                       <p class="im-msgs">好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看</p>                       
                   </div>
               </div>
                <div class="im-contents">
                   <img class="im-headpic" src="https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1515647390623&di=1e15100d2044e87f5de5d30113fc1e80&imgtype=jpg&src=http%3A%2F%2Fimg1.imgtn.bdimg.com%2Fit%2Fu%3D3108820096%2C3322755446%26fm%3D214%26gp%3D0.jpg"/>
                   <div>
                       <p class="im-nickname">青云飘零&emsp;2018-01-11 10:27</p>
                       <p class="im-msgs">好看好看好看好看看好看好看看好看好看好看好看</p>                       
                   </div>
               </div>
                <div class="im-contents">
                   <img class="im-headpic" src="https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1515647390623&di=1e15100d2044e87f5de5d30113fc1e80&imgtype=jpg&src=http%3A%2F%2Fimg1.imgtn.bdimg.com%2Fit%2Fu%3D3108820096%2C3322755446%26fm%3D214%26gp%3D0.jpg"/>
                   <div>
                       <p class="im-nickname">青云飘零&emsp;2018-01-11 10:27</p>
                       <p class="im-msgs">好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看好看</p>                       
                   </div>
               </div>
                <div class="im-contents">
                   <img class="im-headpic" src="https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1515647390623&di=1e15100d2044e87f5de5d30113fc1e80&imgtype=jpg&src=http%3A%2F%2Fimg1.imgtn.bdimg.com%2Fit%2Fu%3D3108820096%2C3322755446%26fm%3D214%26gp%3D0.jpg"/>
                   <div>
                       <p class="im-nickname">青云飘零&emsp;2018-01-11 10:27</p>
                       <p class="im-msgs">好看好看好看好看好看好看好看好看好看好看好看</p>                       
                   </div>
               </div>--%>
           </div>
           <div class="bar-send">
               <input type="text" class="msgs" />
               <button id="btnSend">发&nbsp;&nbsp;送</button>
           </div>
       </div>
       <div class="clear"></div>
   </div>
</body>
</html>
