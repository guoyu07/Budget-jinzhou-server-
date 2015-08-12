<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cashier_keshiyewufei.aspx.cs" Inherits="cashier_keshiyewufei" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <title>财务报销系统</title>


    <link href="../../css/common.css" rel="stylesheet" />
    <link href="../../css/mbcsmbmenu.css" rel="stylesheet" />

    <link href="../../css/sub.css" rel="stylesheet" />

    <script type="text/javascript" src="script/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="script/index.js"></script>
    <script src="layer/layer.js"></script>
    <script src="My97DatePicker/WdatePicker.js"></script>
    <script type="text/javascript">

        function txtBlur(id) {

            var StrNum = parseInt($("#" + id).val());
            if (isNaN(StrNum)) {
                layer.tips('请输入数字', $("#" + id), {
                    guide: 1,
                    time: 2,
                    style: ['background-color:#c00; color:#fff', '#c00'],
                    maxWidth: 100
                });
                $($("#" + id)).val("");
                //   $($("#" + id)).focus();
            }
        }

        function OperatTips(msg) {
            layer.msg(msg, 2, -1);
        }

        $(function () {
            $("#tab input[type='submit']").click(function () {
                var aa = $(this).attr("mark");

                $("#hidKsidAdd").val(aa);

                // var event = event || window.event;
                //var event = event;
                event.preventDefault();
                $(this).blur();
                if (!$('#dialog_wrap').length) {
                    $('<div id="dialog_wrap"><div id="dialog_bg"></div><div id="dialog_body"></div></div>').appendTo($("#form1"));
                }
                $('#dialog_wrap').css('position', 'absolute').css('z-index', '98').css({ 'top': 0, 'left': 0 });
                $('#dialog_bg').css({ 'width': jQuery(window).width(), 'height': jQuery(window).height(), 'background-color': '#333', 'opacity': 0.1 });
                $('#dialog_body').css({ 'width': 0, 'height': 0 }).css('position', 'absolute').css('z-index', '99').css({ 'left': jQuery(window).width() / 2 - 203, 'top': jQuery(window).height() / 2 - 139 });
                $('#dialog_wrap').show();
                $('#add_department_div').prependTo('#dialog_body');
                $('#add_department_div').show();
            });
        })


    </script>
    <style type="text/css">
        .auto-style1 {
            height: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="HidAddFlag" Value="0" runat="server" />
        <asp:HiddenField ID="HidUpdFlag" Value="0" runat="server" />

        <asp:HiddenField ID="hidKsidAdd" Value="0" runat="server" />
        <asp:HiddenField ID="hidpath" runat="server" />
        <div id="wrap">
            <%--   <uc1:verify_header runat="server" ID="verify_header" />--%>
            <div id="content_sub">
                <h2 class="h2_tit w_100"><strong>项目类型列表</strong></h2>
                <div class="select_div" style="width: 820px; height: 40px;">
                    <%--    <p class="fl">
                        <asp:Button ID="btn_add_department" runat="server" Text="增 加" class="btn01" OnClick="btn_add_department_Click" />
                        <asp:Button ID="btn_update_department" runat="server" Text="修 改" class="btn01" />
                        <asp:Button ID="btn_delete" runat="server" OnClientClick="javascript:if(isDel()){return true;}else{return false;};" Text="删 除" class="btn01" />
                    </p>--%>
                    <p class="fl">
                        <asp:FileUpload ID="fileSelect" runat="server" />
                        &nbsp; &nbsp; &nbsp;
                        <asp:Button ID="btnIn" runat="server" Text="导入" class="btn01" OnClick="btnIn_Click" />


                        <asp:Button ID="Button1" runat="server" Text="导出" class="btn01" OnClick="Button1_Click" />
                    </p>
                </div>
                <div class="scroll02">
                    <table width="800" border="0" id="tab" cellspacing="0" cellpadding="0" class="data_list_a ac_td">
                        <colgroup>
                            <col width="10%" />
                            <col width="10%" />
                            <col width="10%" />
                            <col width="10%" />
                            <col width="10%" />
                            <col width="10%" />
                            <col width="10%" />
                            <col width="10%" />
                            <col width="10%" />
                            <col width="10%" />
                            <col width="10%" />
                            <col width="10%" />
                            <col width="10%" />
                        </colgroup>
                        <tr>
                            <%=name %>
                            <%--    <th>部门名</th>
                            <th>上年余额(元)</th>
                            <th>本期金额(元)</th>
                            <th>追加金额(元)</th>
                            <th>合计(元)</th>--%>
                        </tr>
                        <asp:Repeater ID="repBudMonYear" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%# Eval("Depname") %>
                                        <asp:HiddenField ID="HiddenField1" Value='<%# Eval("Depname") %>' runat="server" />
                                    </td>
                                    <td>
                                        <%# Eval("SQJE") %>
                                    </td>
                                    <td>
                                        <%# Eval("BQJE") %>
                                    </td>

                                    <%# Eval("aaaa") %>


                                    <td>
                                        <%# Eval("Total") %>
                                    </td>

                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    <div class="page">
                        <p class="ac">
                            &nbsp;
                         <%--   <webdiyer:AspNetPager ID="pagerBudMon" Width="600" ImagePath="images/PagerGif"
                                NavigationButtonType="Image" PageIndexBoxType="DropDownList" ShowPageIndexBox="Always"
                                ButtonImageNameExtension="n" DisabledButtonImageNameExtension="g" ButtonImageExtension="gif"
                                PageSize="14" NumericButtonCount="10" MoreButtonType="Image" runat="server" OnPageChanged="pagerBudMon_PageChanged">
                            </webdiyer:AspNetPager>--%>
                        </p>
                    </div>
                </div>
            </div>
            <div id="footer"></div>
        </div>



        <div class="filter01" id="update_department_div" style="display: none;">
            <h2>修改项目归类</h2>
            <input type="hidden" name="depa_id" id="update_depa_id" value="" />
            <p>项目归类：<asp:DropDownList ID="ddlUpdClass" Height="24" class="input" runat="server"></asp:DropDownList></p>
            <p>项目类型：<asp:DropDownList ID="ddlUpdDep" Height="24" class="input" runat="server"></asp:DropDownList></p>
            <p>自定义项目类型名称：<asp:TextBox ID="txtUpdPrj" class="input" runat="server"></asp:TextBox></p>
            <p class="ac cl">
                <asp:Button ID="btnUpd" runat="server" Text="保 存" title="保 存" CssClass="btn01" />
                <asp:Button ID="btnUpdClose" class="btn01 btn_close" runat="server" Text="关 闭" />
                <asp:Label ID="lblUpd" runat="server" ForeColor="Red"></asp:Label>
            </p>
        </div>
    </form>
</body>
</html>

