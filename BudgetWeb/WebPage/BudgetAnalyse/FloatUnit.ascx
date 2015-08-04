<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FloatUnit.ascx.cs" Inherits="WebPage_BudgetAnalyse_FloatUnit" %>
<link href="../../浮动窗/css/style.css" rel="stylesheet" />
<script src="../../浮动窗/js/popup_layer.js"></script>
<script src="../../js/jquery-1.7.2.min.js"></script>
<script src="../../js/kkpager.min.js"></script>
<link href="../../css/kkpager_blue.css" rel="stylesheet" />
<script>
    function getParameter(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }
     


    $(function () {
        //var curr = Number($("#hidpageindex").val());
        //var totalCount = Number($("hidRecordCount").val());

        $("#FloatUnit_filterUnit").keyup(function () {
            var postUrl = getRootPath() + "/BudgetAnalyse/QueryHandler.ashx?filter=" + this.value;
            $.post(postUrl, function (comment) {
                if (comment == '0') { 
                    DoPagerbind();
                }
                else {
                    $("#kkpager").hide();
                    $("#dataPanl").empty();                    var htmlContent = "";
                    for (var i = 0; i < comment.length; i++) {
                         var temp = comment[i]["UnitName"].length > 7 ? comment[i]["UnitName"].substr(0, 7) : comment[i]["UnitName"];
                       // var temp = comment[i]["UnitName"];
                        htmlContent += "<li><a  id='a" + i + "' style='visibility:collapse;height:0px'  >" + comment[i]["UnitCode"] + "</a><a id='b" + i + "' title='" + comment[i]["UnitName"] + "'   onclick='btnOnclick(this)'>" + temp + "</a></li>"
                    }
                    $("#dataPanl").append($(htmlContent));
                }
            })
        });

        var totalRecords = Number($("#FloatUnit_hidRecordCount").val());
        var totalPage = totalRecords / 14;
        function DoPagerbind() {
            var pgIndex = parseInt($("#FloatUnit_hidpageindex").val());
            if (isNaN(pgIndex)) {
                pgIndex = 0;
            }
            var postUrl = getRootPath() + "/BudgetAnalyse/PageChangeDadaBind.ashx?pageindex=" + pgIndex + "&RecordCount=" + totalRecords;
            $.post(postUrl, function (comment) {
                //var comment = $.parseJSON(pzh);
                $("#kkpager").show();
                $("#dataPanl").empty();                var htmlContent = "";
                for (var i = 0; i < comment.length; i++) {
                    var temp = comment[i]["UnitName"].length > 7 ? comment[i]["UnitName"].substr(0, 7) : comment[i]["UnitName"];
                    //var temp = comment[i]["UnitName"];
                    htmlContent += "<li><a  id='a" + i + "' style='visibility:collapse;height:0px'>" + comment[i]["UnitCode"] + "</a><a id='b" + i + "' title='" + comment[i]["UnitName"] + "'   onclick='btnOnclick(this)'>" + temp + "</a></li>"
                }
                $("#dataPanl").append($(htmlContent));
            });
        };


        $("#FloatUnit_ele9").click(function () {
            DoPagerbind();
        });

        var pageNo = getParameter('pno');
        if (!pageNo) {
            pageNo = 1;
        }
        kkpager.generPageHtml({
            pno: pageNo,
            //总页码
            total: totalPage,
            //总数据条数
            totalRecords: totalRecords,
            mode: 'click',//默认值是link，可选link或者click
            click: function (n) {
                // do something
                //手动选中按钮
                this.selectPage(n);
                $("#FloatUnit_hidpageindex").val(n);
                DoPagerbind();
                return false;
            }
            /*
            ,lang				: {
                firstPageText			: '首页',
                firstPageTipText		: '首页',
                lastPageText			: '尾页',
                lastPageTipText			: '尾页',
                prePageText				: '上一页',
                prePageTipText			: '上一页',
                nextPageText			: '下一页',
                nextPageTipText			: '下一页',
                totalPageBeforeText		: '共',
                totalPageAfterText		: '页',
                currPageBeforeText		: '当前第',
                currPageAfterText		: '页',
                totalInfoSplitStr		: '/',
                totalRecordsBeforeText	: '共',
                totalRecordsAfterText	: '条数据',
                gopageBeforeText		: ' 转到',
                gopageButtonOkText		: '确定',
                gopageAfterText			: '页',
                buttonTipBeforeText		: '第',
                buttonTipAfterText		: '页'
            }*/
        });
        //示例9，综合效果
        var t9 = new PopupLayer({
            trigger: "#FloatUnit_ele9",
            popupBlk: "#blk9",
            closeBtn: "#close9",
            useOverlay: true,
            useFx: true,
            offsets: {
                x: 0,
                y: -41
            }
        });
        t9.doEffects = function (way) {
            if (way == "open") {
                this.popupLayer.css({ opacity: 0.3 }).show(300, function () {
                    this.popupLayer.animate({
                        left: ($(document).width() - this.popupLayer.width()) / 2,
                        top: (document.documentElement.clientHeight - this.popupLayer.height()) / 2 + $(document).scrollTop(),
                        opacity: 0.8
                    }, 300, function () { this.popupLayer.css("opacity", 1) }.binding(this));
                }.binding(this));
            }
            else {
                this.popupLayer.animate({
                    left: this.trigger.offset().left,
                    top: this.trigger.offset().top,
                    opacity: 0.1
                }, { duration: 200, complete: function () { this.popupLayer.css("opacity", 1); this.popupLayer.hide() }.binding(this) });
            }
        }




    });
    function getRootPath() {
        var strFullPath = window.document.location.href;
        var strPath = window.document.location.pathname;
        var pos = strFullPath.indexOf(strPath);
        var prePath = strFullPath.substring(0, pos);
        var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
        return (prePath + postPath);
    }    function btnOnclick(e) {
        //var curr = Number($("#hidpageindex").val());
        //var totalCount = Number($("hidRecordCount").val());
      
        var unitcode = $("#" + e.id).parents("li").find("a:eq(0)").text();
        var unitname = e.title;
        var postUrl = getRootPath() + "/BudgetAnalyse/UnitHandler.ashx?UnitCode=" + unitcode;
        $.post(postUrl, function (data) {
            if (data == "0") {
                //$(".popupLayer").hide();
                $("#FloatUnit_ele9").html(unitname)
                $("#close9").click();
                $("#FloatUnit_btnmoni").click();
            } else {
                alert("服务器连接失败");
            }
        });
    }
</script>
<%--<asp:SqlDataSource runat="server" ID="UnitDataSource" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select *  from UnitTable" FilterExpression="UnitName like '%{0}%' ">
    <FilterParameters>
        <asp:ControlParameter Name="UnitName"   ControlID="filterUnit" />
    </FilterParameters>
</asp:SqlDataSource>--%>
<input runat="server" type="hidden" id="hidpageindex" />
<input runat="server" type="hidden" id="hidRecordCount" />
<div id="emample9" class="example">
    <asp:Button runat="server"  id="btnmoni"   OnClick="Button_Click" style="display:none"   />
   
    <div id="ele9" runat="server" class="tigger" style="width: 120px">选择单位</div>
    &nbsp;<div id="blk9" class="blk" style="display: none; width: 500px; height: 600px;">
        <div class="head">
            <div class="head-right"></div>
        </div>
        <div class="main">
            <h2>选择单位 
                <asp:TextBox runat="server" ID="filterUnit"  ></asp:TextBox></h2>
            <a href="javascript:void(0)" id="close9" class="closeBtn">关闭</a>
            
            <ul id="dataPanl">
            </ul>
           <%-- <asp:ListView runat="server"
                ID="UnitListView"
                DataSourceID="UnitDataSource" Visible="false">
                <LayoutTemplate>
                    <ul id="dataPan">
                        <asp:PlaceHolder runat="server"
                            ID="itemPlaceholder" />
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li><a id="Unitco" runat="server" style="visibility: collapse"><%# Eval("UnitCode") %></a><a title="<%# Eval("UnitName") %>" id="UnitA" onclick="btnOnclick(this)"><%# Eval("UnitName").ToString().Substring(0,Eval("UnitName").ToString().Length>7?7:Eval("UnitName").ToString().Length) %></a></li>
                </ItemTemplate>
            </asp:ListView>--%>
            <div id="kkpager" ></div>
        </div>
    </div>

</div>
<!--emample9 end-->
