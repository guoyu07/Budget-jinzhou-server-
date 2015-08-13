<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BudgetexecutionrateNewyear.aspx.cs" Inherits="WebPage_BudgetAnalyse_BudgetexecutionrateNewyear" ValidateRequest="false" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<%@ Register Src="~/WebPage/BudgetAnalyse/FloatUnit.ascx" TagPrefix="uc1" TagName="FloatUnit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        .x-grid-row td {
            border-bottom: 1px solid #EDEDED !important;
            border-right: 1px solid #EDEDED !important;
        }
    </style>
    <script>
        var saveData = function () {
            //App.GridData.setValue(Ext.encode(App.gridpl.getRowsValues({ selectedOnly: false })));  
            var tab = App.tabcontrol.activeTab.id;
            App.GridData.setValue($("#" + tab + "-body div").html());
        };

        var refreshTree = function (tree) {
            App.direct.RefreshMenu({
                success: function (result) {
                    var nodes = eval(result);
                    if (nodes.length > 0) {
                        tree.setRootNode(nodes[0]);
                    }
                    else {
                        tree.getRootNode().removeAll();
                    }
                }
            });
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:Hidden ID="GridHead" runat="server" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:ResourceManager runat="server"></ext:ResourceManager>
        <ext:Viewport runat="server" Layout="fit">
            <Items>
                <ext:Panel runat="server" Layout="BorderLayout">
                    <Items>
                        <ext:Container runat="server" Region="North" Layout="AnchorLayout" BaseCls="background:tranceparent">
                            <Items>
                                <ext:Panel ID="Container1" Border="false" runat="server" Layout="ColumnLayout" MarginSpec="5 0 5 0" Width="400" BaseCls="background:transeparent">
                                    <Items>
                                        <ext:Label ID="Label3" runat="server" MarginSpec="5 5 0 5" Text="部门名称：" Width="60"></ext:Label>
                                        <ext:ComboBox ID="cmbdept" EmptyText="当期单位没有部门" runat="server" ColumnWidth="0.6" DisplayField="DepName" MinWidth="60">
                                            <SelectedItems>
                                                <ext:ListItem Index="0">
                                                </ext:ListItem>
                                            </SelectedItems>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel ID="Panel1" Border="false" runat="server" Layout="ColumnLayout" MarginSpec="0 0 5 0" Width="400" BaseCls="background:transeparent">
                                    <Items>
                                        <ext:Label ID="Label1" runat="server" MarginSpec="5 5 0 5" Text="年　　份：" Width="60"></ext:Label>
                                        <ext:ComboBox ID="cmbyear" runat="server" ColumnWidth="0.6" DisplayField="Year" MinWidth="60" Editable="false">
                                            <Store>
                                                <ext:Store ID="cmbyearstore" runat="server">
                                                    <Model>
                                                        <ext:Model ID="Model2" runat="server">
                                                            <Fields>
                                                                <ext:ModelField Name="Year"></ext:ModelField>
                                                            </Fields>
                                                        </ext:Model>
                                                    </Model>
                                                </ext:Store>
                                            </Store>
                                            <SelectedItems>
                                                <ext:ListItem Index="0">
                                                </ext:ListItem>
                                            </SelectedItems>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel ID="Panel2" Border="false" Visible="false" runat="server" Layout="ColumnLayout" Width="400" MarginSpec="0 0 10 0" BaseCls="background:transeparent">
                                    <Items>
                                        <ext:Label ID="Label2" runat="server" MarginSpec="5 5 0 5" Text="月　　份：" Width="60"></ext:Label>
                                        <ext:ComboBox ID="cmbmonth" runat="server" ColumnWidth="0.6" MinWidth="60" Editable="false">
                                            <Items>
                                                <ext:ListItem Text="01" Value="01" />
                                                <ext:ListItem Text="02" Value="02" />
                                                <ext:ListItem Text="03" Value="03" />
                                                <ext:ListItem Text="04" Value="04" />
                                                <ext:ListItem Text="05" Value="05" />
                                                <ext:ListItem Text="06" Value="06" />
                                                <ext:ListItem Text="07" Value="07" />
                                                <ext:ListItem Text="08" Value="08" />
                                                <ext:ListItem Text="09" Value="09" />
                                                <ext:ListItem Text="10" Value="10" />
                                                <ext:ListItem Text="11" Value="11" />
                                                <ext:ListItem Text="12" Value="12" />
                                            </Items>
                                            <SelectedItems>
                                                <ext:ListItem Index="0">
                                                </ext:ListItem>
                                            </SelectedItems>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:Panel>
                                <ext:Panel ID="Panel3" Border="false" runat="server" Layout="ColumnLayout" Width="400" MarginSpec="0 0 10 0" BaseCls="background:transeparent">
                                    <Items>
                                        <ext:Button runat="server" ID="btnsend" Text="确定" MarginSpec="0 0 0 70">
                                            <Listeners>
                                                <Click Handler="refreshTree(#{treepl})" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Button runat="server" ID="exbtn" Text="导出表格" MarginSpec="0 0 0 70" AutoPostBack="true" OnClick="exbtn_Click">
                                            <Listeners>
                                                <Click Fn="saveData" />
                                            </Listeners>
                                        </ext:Button>
                                        <ext:Container runat="server" MarginSpec="0 0 0 20">
                                            <Content>
                                                <uc1:FloatUnit runat="server" ID="FloatUnit" />
                                            </Content>
                                        </ext:Container>
                                    </Items>

                                </ext:Panel>

                            </Items>
                        </ext:Container>


                        <ext:TreePanel
                            Region="Center"
                            ID="treepl"
                            Title=""
                            Lines="false"
                            UseArrows="true"
                            runat="server"
                            AutoScroll="true"
                            Animate="true"
                            SingleExpand="False"
                            Mode="Remote"
                            RootVisible="false"
                            ContainerScroll="true">
                            <Fields>
                                <ext:ModelField Name="PIID"></ext:ModelField>
                                <ext:ModelField Name="PIEcoSubName"></ext:ModelField>
                                <ext:ModelField Name="totalMon"></ext:ModelField>
                                <ext:ModelField Name="BQMon"></ext:ModelField>
                                <ext:ModelField Name="RpMoney"></ext:ModelField>
                            </Fields>
                            <ColumnModel>
                                <Columns>
                                    <ext:TreeColumn ID="TreeColumn2" runat="server" DataIndex="text" Text="经济科目" Flex="4">
                                    </ext:TreeColumn>
                                    <ext:Column runat="server" Text="年初预算金额（元）" Width="120" DataIndex="totalMon">
                                        <Renderer Handler="return (value*1.00).toFixed(2)" />
                                    </ext:Column>
                                    <ext:Column runat="server" Text="申请计划数（元）" Width="120" DataIndex="BQMon">
                                        <Renderer Handler="return (value*1.00).toFixed(2)" />
                                    </ext:Column>
                                    <ext:Column runat="server" Text="报销执行金额（元）" Width="120" DataIndex="RpMoney">
                                        <Renderer Handler="return (value*1.00).toFixed(2)" />
                                    </ext:Column>
                                    <ext:Column runat="server" Text="余额">
                                        <Columns>
                                            <ext:Column runat="server" Text="计划（元）" Width="100">
                                                <Renderer Handler="return ((record.data.totalMon-record.data.BQMon)*1.00).toFixed(2)"></Renderer>
                                            </ext:Column>
                                            <ext:Column runat="server" Text="执行（元）" Width="100">
                                                <Renderer Handler="return ((record.data.totalMon-record.data.RpMoney)*1.00).toFixed(2)"></Renderer>
                                            </ext:Column>
                                        </Columns>
                                    </ext:Column>
                                    <ext:Column runat="server" Text="执行率">
                                        <Columns>
                                            <ext:Column runat="server" Text="计划" Width="100">
                                                <Renderer Handler="return ((record.data.BQMon / (record.data.totalMon==0?1:record.data.totalMon))*100).toFixed(2)+'%';" />
                                            </ext:Column>
                                            <ext:Column runat="server" Text="执行" Width="100">
                                                <Renderer Handler="return ((record.data.RpMoney / (record.data.totalMon==0?1:record.data.totalMon))*100).toFixed(2)+'%';" />
                                            </ext:Column>
                                        </Columns>
                                    </ext:Column>

                                </Columns>
                            </ColumnModel>
                            <Listeners>
                                <Activate Handler="#{Label2}.hide();#{cmbmonth}.hide();"></Activate>
                            </Listeners>
                        </ext:TreePanel>
                    </Items>
                </ext:Panel>

            </Items>

        </ext:Viewport>
    </form>
</body>
</html>
