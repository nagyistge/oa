﻿<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>二等奖补抽</title>
    <script type="text/javascript" src="Scripts/jquery-1.7.min.js"></script>
    <script type="text/javascript" src="Scripts/json2.js"></script>
    <script type="text/javascript" src="Scripts/Lucky.js"></script>
    <script type="text/javascript" src="Scripts/DBProxy.js"></script>
    <script type="text/javascript" src="default.js"></script>
   <style>
        body, html
        {
            padding: 0;
            margin: 0;
            height: 100%;
            overflow: hidden;
        }
        body
        {            
            font-size: 1em;
            font-family: Tahoma, Geneva, sans-serif;
            background-color: #000;
            color: #fff;
            font-weight: bold;
           
        }
        ul,li{ margin:0; padding:0;}
       
        .list_num
        { 
            width: 520px;
            margin: 0;
            padding: 0;
            text-align: center;
            overflow: hidden;
        }
        .list_num li
        {
            list-style: none;
            padding: 0 0 5px 0;
            margin: 0;
            font-weight: bold;            
            white-space: nowrap;
            word-break: keep-all;            
            color: #a20000;
        }
        #num_award
        {
            display: none;
            text-shadow: 0px 2px 1px rgba(255,255,255,0.5);
        }
#num_award li{ cursor:pointer;}
        #num_award li:hover{  text-decoration:line-through; color:red!important; cursor:url(images/del_cur.cur),auto;}
        .sz_award,.bj_award{ display:none;  position:absolute;   list-style-type:none; padding:10px 0;  /*background-image: -webkit-linear-gradient(top, rgba(0,0,0,0.5), rgba(0,0,0,0.1), rgba(0,0,0,0.5))*/;}
       .sz_award li,.bj_award li{  float:left;color:#FFF1B4; text-align:center; width:49%;border-radius: 14px; margin:0 0 4px 2px; padding:2px 0;
                                    text-shadow: 0px 1px 2px #000; font-weight:bold; box-shadow: 0px 2px 4px #000; border-top:1px solid rgba(252,252,162,0.4);
                                  background:#a60000; background: -webkit-gradient(linear, left top, left bottom, from(#c40e00), to(#a60000));}
        #roll_num,#num_award{  margin: auto; position: absolute;z-index: 9999; left:50%;}
        .num_mask_top{ background:url(images/num_mask_top.png) no-repeat; width:571px; height:20px; position:absolute; top:203px; left:50%; margin-left:-285px; z-index:9999;}
        .num_mask_bottom{ background:url(images/num_mask_bottom.png) no-repeat; width:571px; height:20px; position:absolute; top:618px; left:50%; margin-left:-285px; z-index:9999;}
         .stage_left {
-webkit-animation: sl 2s 1 linear;
}
@-webkit-keyframes sl {
from { -webkit-transform:  skew(0deg) ;  }
to { -webkit-transform:  skew(-50deg) ; }
}

.stage_right {
-webkit-animation: sr 2s 1 linear;
}
@-webkit-keyframes sr {
from { -webkit-transform:  skew(0deg) ;  }
to { -webkit-transform: skew(40deg) ; }
}
    </style>
    <script type="text/javascript">
        var playing = true;
        var cur_lev = 2, cur_num = 1, open = false, batch = "2";  //当前奖项和奖票数目
        var isSupply = true;//补抽模式
        $(function () {
            //点击备选号
            $("#num_award").on("click", "li", function () {
                playing_element = $(this);
                del_award = playing_element.text();
                lucky.DiscardAward(del_award); //获取当前选号，选票删除
                dbProxy.DiscardAward(del_award); //获取当前选号，并传服务器
                start(); //开始
                playing = true;
            });
        });
        document.onkeyup = function (e) {
            var theEvent = window.event || e;
            var code = theEvent.keyCode || theEvent.which;
            //alert(event.keyCode);

            if (code == 32) {//按空格键
                if (!open) {
                    open = true;
                    stage(); //Default.js
                } else {
//                    if (!playing) {
//                        start(); //Default.js
//                        playing = true;
                    //                    }
                    if (playing) {
                        stop(cur_lev, cur_num); //停止动画，显示获奖号
                        playing = false;
                    } else {
                        start(); //开始
                        playing = true;
                    }
                }
                return false
            }

            if (code == 83 && open){//按s键
                if (playing) {
                    stop_loc('SZ'); //停止动画，显示获奖号 Default.js
                    playing = false;
                } 
                return false
            }
            if (code == 66 && open) {//按b键

                if (playing) {
                    stop_loc('BJ'); //停止动画，显示获奖号                    
                    playing = false;
                } 
                return false
            }
            if (code == 46 && !playing) {//按删除键
                var $children_li = $("#num_award").children();
                $children_li.each(function () {
                    var del_award = $(this).text();
                    lucky.DiscardAward(del_award); //获取当前选号，选票删除
                    dbProxy.DiscardAward(del_award); //获取当前选号，并传服务器
                })
                start(); //开始
                playing = true;
                return false
            }
        }
        window.onresize = function () { resize(cur_lev) };
        window.onload = function () { resize(cur_lev) };
    </script>
</head>
<body>
    <div id="musicdiv">    
<%--必须使用支持html5的网页才能播放背景声音，以下html标签只能在html5的浏览器中受支持，如google--%>  
        <%--<embed id='myaudio' src='mp3/kamen.mp3' volume="0" loop=true autostart=true name=bgss width=0 height=0 />--%>
    </div>
<img src="images/2.jpg" style="width:100%; position:absolute; z-index:-1;" id="back_img" />
    <ul id="roll_num" class="list_num"></ul>
    <ul id="num_award" class="list_num"></ul>
    <ul class="sz_award">
    </ul>
    <ul class="bj_award">
    </ul>
    <div id="stage" style="width: 100%; height:100%; position: absolute; z-index: 9999;">  
      <%--  <div style="position:absolute;left:0;top:0;width:100%;height:100%;background-color:#000" id="black_mask"></div>      --%>
        <%--<img src="images/2015/2_stage_left.jpg" width="50%" id="stage_left02" style=" position:absolute; left:0;top:0;" /> 
        <img src="images/2015/2_stage_right.jpg" width="50%" id="stage_right02" style=" position:absolute; right:0;top:0;"/>--%>
       <%-- <img src="images/stage_left01.png" width="16.85%" id="stage_left01" style=" position:absolute; left:0; top:0;" /> 
        <img src="images/stage_right01.png" width="16.85%" id="stage_right01" style=" position:absolute; right:0;top:0;"/>
        <img src="images/stage_top.png" width="100%" id="stage_top" style=" position:absolute; top:0;"/>--%>
    </div>

    
</body>
</html>
