<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BudgetMonEdit.aspx.cs" Inherits="WebPage_BudgetEdit_BudgetMonEdit" ValidateRequest="false" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        .cmbdept-list {
            font: 11px tahoma,arial,helvetica,sans-serif;
        }

            .cmbdept-list th {
                font-weight: bold;
            }

            .cmbdept-list td, .cmbdept-list th {
                /*// padding: 3px;*/
                padding: 5px 5px 5px 18px;
            }

        .list-item {
            cursor: pointer;
        }

        .icon-headbg {
            height: 25px;
            width: 25px;
            line-height: 25px;
            background: url(../../img/icon-head.png) no-repeat center center;
            text-align: center;
            color: #fff;
        }
    </style>
    <script type="text/javascript" src="../../js/jquery-1.7.2.min.js"></script>
    <script>
        var SetDisable = function (a, b) {
            App.direct.Dodisable(b.data.CaliberID);
        }
        var saveData = function () {
            //App.GridData.setValue(Ext.encode(App.gridpl.getRowsValues({ selectedOnly: false }))); 
            App.GridData.setValue($("#gridpl-body div").html());
        };
        var edit = function (editor, e) {
            /*
                "e" is an edit event with the following properties:

                    grid - The grid
                    record - The record that was edited
                    field - The field name that was edited
                    value - The value being set
                    originalValue - The original value for the field, before the edit.
                    row - The grid table row
                    column - The grid Column defining the column that was edited.
                    rowIdx - The row index that was edited
                    colIdx - The column index that was edited
            */

            // Call DirectMethod  
            if (!(e.value === e.originalValue || (Ext.isDate(e.value) && Ext.Date.isEqual(e.value, e.originalValue)))) {
                if (e.field == "BGAMMon") {
                    CompanyX.Edit(e.record.data.CaliberID, e.record.data.CaliberName, e.record.data.BGAMID, e.originalValue, e.value, e.record.data);
                }
                else if (e.field == "BGAMIncome") {
                    CompanyX.Edit2(e.record.data.CaliberID, e.record.data.CaliberName, e.record.data.BGAMID, e.originalValue, e.value, e.record.data);
                }
                else { CompanyX.Edit1(e.record.data.CaliberID, e.record.data.CaliberName, e.record.data.BGAMID, e.originalValue, e.value, e.record.data); }

            }
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:Hidden ID="GridHead" runat="server" />
        <ext:Hidden ID="GridData" runat="server" />
        <ext:ResourceManager runat="server"></ext:ResourceManager>
        <ext:Viewport ID="viewport1" runat="server" Layout="BorderLayout">
            <Items>
                <ext:Container runat="server" Region="North" Layout="AnchorLayout" BaseCls="background:tranceparent">
                    <Items>
                        <ext:Panel ID="Panel1" Border="false" runat="server" Layout="ColumnLayout" MarginSpec="5 0 5 0" Width="400" BaseCls="background:transeparent">
                            <Items>
                                <ext:Label ID="Label1" runat="server" MarginSpec="5 5 0 5" Text="年　　份：" Width="60"></ext:Label>
                                <ext:ComboBox AllowBlank="false" ID="cmbyear" runat="server" ColumnWidth="0.6" DisplayField="Year" MinWidth="60" Editable="false">
                                    <Store>
                                        <ext:Store ID="cmbyearstore" runat="server">
                                            <Model>
                                                <ext:Model ID="Model2" runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="Year" Type="int" Mapping="Year"></ext:ModelField>
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>
                                    <SelectedItems>
                                        <ext:ListItem Index="0">
                                        </ext:ListItem>
                                    </SelectedItems>
                                    <Listeners>
                                        <Select Handler="App.direct.DoBind();"></Select>
                                    </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:Panel>

                        <ext:Panel ID="Container1" Border="false" runat="server" Layout="ColumnLayout" MarginSpec="0 0 5 0" Width="400" BaseCls="background:transeparent">
                            <Items>
                                <ext:Label ID="Label3" runat="server" MarginSpec="5 5 0 5" Text="部门名称：" Width="60"></ext:Label>
                                <ext:MultiCombo AllowBlank="false" ValueNotFoundText="Loading..." Editable="false" ID="cmbdept" SelectionMode="Selection" runat="server" ColumnWidth="0.6" DisplayField="DepName" ValueField="DepID" MinWidth="60">
                                    <Store>
                                        <ext:Store ID="cmbdeptStore" runat="server" IsPagingStore="true" >
                                            <Model>
                                                <ext:Model runat="server">
                                                    <Fields>
                                                        <ext:ModelField Name="DepID" />
                                                        <ext:ModelField Name="DepName" />
                                                        <ext:ModelField Name="EditCount" />
                                                    </Fields>
                                                </ext:Model>
                                            </Model>
                                        </ext:Store>
                                    </Store>

                                    <ListConfig Height="300" ItemSelector=".x-boundlist-item">
                                        <Tpl runat="server">
                                            <Html>
                                                <tpl for=".">
						    <tpl if="[xindex] == 1">
							    <table class="cmbdept-list">
								    <tr> 
									    <th>部门名称</th>
                                        <th>编写记录</th>
								    </tr>
						    </tpl>
						    <tr class="x-boundlist-item">
							    <td>{DepName}</td>
							    <td><div class="icon-headbg">{EditCount} </div></td>
						    </tr>
						    <tpl if="[xcount-xindex]==0">
							    </table>
						    </tpl>
					    </tpl>
                                            </Html>
                                        </Tpl>
                                    </ListConfig>
                                    <%-- <SelectedItems>
                                        <ext:ListItem Index="0">
                                        </ext:ListItem>
                                    </SelectedItems>--%>
                                    <SelectedItems>
                                        <ext:ListItem Index="0">
                                        </ext:ListItem>
                                    </SelectedItems>
                                </ext:MultiCombo>
                            </Items>
                        </ext:Panel>

                        <ext:Button runat="server" ID="btnsend" Text="确定" MarginSpec="0 0 0 70" OnDirectClick="btnsend_DirectClick">
                            <Listeners>
                                <Click Handler="if (!#{cmbyear}.isValid() || !#{cmbdept}.isValid()  ) {Ext.Msg.alert('Error','验证有误，请选择!');  return false; }" />
                            </Listeners>
                        </ext:Button>
                        <ext:Button runat="server" ID="exbtn" Text="导出表格" MarginSpec="0 0 0 70" AutoPostBack="true" OnClick="exbtn_Click">
                            <Listeners>
                                <Click Fn="saveData" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </ext:Container>

                <ext:GridPanel
                    Region="Center"
                    ID="GridPanel1"
                    runat="server"
                    Title="预算金额编辑"
                    Icon="ApplicationViewColumns" ColumnLines="true">
                    <Store>
                        <ext:Store runat="server" ID="Store1">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="CaliberID"></ext:ModelField>
                                        <ext:ModelField Name="CBID"></ext:ModelField>
                                        <ext:ModelField Name="DepID"></ext:ModelField>
                                        <ext:ModelField Name="BGAMID"></ext:ModelField>
                                        <ext:ModelField Name="CbLever"></ext:ModelField>
                                        <ext:ModelField Name="CaliberName"></ext:ModelField>
                                        <ext:ModelField Name="ParentID"></ext:ModelField>
                                        <ext:ModelField Name="BGAMMon"></ext:ModelField>
                                        <ext:ModelField Name="BGAMIncome"></ext:ModelField>
                                        <ext:ModelField Name="BGAMLastMon"></ext:ModelField>
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>

                    </Store>
                    <Plugins>
                        <ext:CellEditing runat="server">
                            <Listeners>
                                <Edit Fn="edit" />
                            </Listeners>
                        </ext:CellEditing>
                    </Plugins>
                    <ColumnModel>
                        <Columns>
                            <ext:Column runat="server" ID="cjname" DataIndex="CaliberName"     Text="口径名称"  Sortable="false" ></ext:Column>
                            <ext:Column runat="server" ID="columndepname" Text="">
                                <Columns>
                                    <ext:Column runat="server" DataIndex="BGAMIncome" EmptyCellText="0" Text="预算支出（元）" Sortable="false" Locked="true" MinWidth="280" MaxWidth="280">
                                        <Editor>
                                            <ext:NumberField
                                                runat="server"
                                                ID="NumberField2"
                                                AllowBlank="false"
                                                MinValue="0"
                                                StyleSpec="text-align:left">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column runat="server" DataIndex="BGAMMon" EmptyCellText="0" Text="本期（元）">
                                        <Editor>
                                            <ext:NumberField
                                                runat="server"
                                                ID="editnum"
                                                AllowBlank="false"
                                                MinValue="0"
                                                StyleSpec="text-align:left">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column runat="server" DataIndex="BGAMLastMon" EmptyCellText="0" Text="同期（元）">
                                        <Editor>
                                            <ext:NumberField
                                                runat="server"
                                                ID="NumberField1"
                                                AllowBlank="false"
                                                MinValue="0"
                                                StyleSpec="text-align:left">
                                            </ext:NumberField>
                                        </Editor>
                                    </ext:Column>
                                    <ext:Column runat="server" Text="同比+">
                                        <Renderer Handler="return (record.data.BGAMMon-record.data.BGAMLastMon)"></Renderer>
                                    </ext:Column>
                                    <ext:Column runat="server" Text="同比+%">
                                        <Renderer Handler="if((record.data.BGAMMon-record.data.BGAMLastMon)===0||(record.data.BGAMMon-record.data.BGAMLastMon)===null){return '0%'} else if((record.data.BGAMLastMon)===0||(record.data.BGAMLastMon)===null){return '0%'} else {return  ((record.data.BGAMMon-record.data.BGAMLastMon)*100/record.data.BGAMLastMon).toFixed(2)+'%'}"></Renderer>
                                    </ext:Column>
                                </Columns>
                            </ext:Column>

                        </Columns>
                    </ColumnModel>
                    <Listeners>
                        <ItemClick Fn="SetDisable"></ItemClick>
                    </Listeners>
                </ext:GridPanel>
            </Items>

        </ext:Viewport>


    </form>
</body>
</html>
