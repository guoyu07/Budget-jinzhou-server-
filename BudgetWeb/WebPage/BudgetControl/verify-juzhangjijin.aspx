<%@ Page Language="C#" AutoEventWireup="true" CodeFile="verify-juzhangjijin.aspx.cs" Inherits="verify_juzhangjijin" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
 



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
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 20%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="HidAddFlag" Value="0" runat="server" />
        <asp:HiddenField ID="HidUpdFlag" Value="0" runat="server" />
        <asp:HiddenField ID="hidpath" runat="server" />
        <div id="wrap">
       
            <div id="content_sub">
                <h2 class="h2_tit w_100"><strong>项目类型列表</strong></h2>
                <div class="select_div" style="width: 820px;height:40px;">
                    <%--    <p class="fl">
                        <asp:Button ID="btn_add_department" runat="server" Text="增 加" class="btn01" OnClick="btn_add_department_Click" />
                        <asp:Button ID="btn_update_department" runat="server" Text="修 改" class="btn01" />
                        <asp:Button ID="btn_delete" runat="server" OnClientClick="javascript:if(isDel()){return true;}else{return false;};" Text="删 除" class="btn01" />
                    </p>--%>
                    <p class="fl">
                        <asp:FileUpload ID="fileSelect" runat="server" />
                        &nbsp; &nbsp; &nbsp;
                        <asp:Button ID="btnIn" runat="server" Text="导入" class="btn01" OnClick="btnIn_Click" />

                        <asp:Button ID="Button1" runat="server" Text="导出" class="btn01" OnClick="Button1_Click"  />


                         
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

        <div class="filter01" id="add_department_div" style="display: none;">
            <h2>增加项目归类</h2>
            <p>项目归类：<asp:DropDownList ID="ddlAddClass" class="input" Height="24" runat="server"></asp:DropDownList></p>
            <p>部门名称：<asp:DropDownList ID="ddlAddDep" class="input" Height="24" runat="server"></asp:DropDownList></p>
            <p>自定义项目类型名称：<asp:TextBox ID="txtAddPrj" class="input" runat="server"></asp:TextBox></p>
            <p class="ac cl">
                <asp:Button ID="btnAdd" runat="server" Text="保 存" title="保 存" CssClass="btn01" />
                <asp:Button ID="btnAddClose" class="btn01 btn_close" runat="server" Text="关 闭" />
                <asp:Label ID="lblAdd" runat="server" ForeColor="Red"></asp:Label>
            </p>
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

