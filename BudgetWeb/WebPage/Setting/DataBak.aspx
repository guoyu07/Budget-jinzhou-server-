<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataBak.aspx.cs" Inherits="WebPage_Setting_DataBak" %>

<%@ Register Assembly="Ext.Net" Namespace="Ext.Net" TagPrefix="ext" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../js/download.js"></script>
    <script src="../../js/jquery-1.7.2.min.js"></script>
    <script>
        var Doit = function (sender, command, record) {
            if (command == "restore") {
                Ext.Msg.confirm("提示", "是否还原？", function (btn) {
                    if (btn == "yes") {
                        App.direct.btnrestore(record.data.DbID);
                    }
                });
            }
            //if (command == "download") {
            //     App.direct.btndownload(record.data.Name);  
            //    //downloadFile(record.data.DirectoryName + "\\" + record.data.Name);
               
            //    //var dPath = record.data.DirectoryName + "\\" + record.data.Name;
            //    //alert(dPath);
            //    //$("#downloadLabel").prop("href", dPath);
            //    //alert($("#downloadLabel"));
            //    //$("#downloadLabel").onclick();
            //}
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <a id="downloadLabel" href="http://www.whcode.com/UploadFiles/AdsImg/201518091104558341.jpg" target="_blank"></a>
        <ext:ResourceManager ID="ResourceManager1" runat="server" />
        <ext:Viewport runat="server" ID="vwpLayout" Layout="fit">
            <Items>
                <ext:GridPanel runat="server" Title="数据库备份与还原" ID="gridpl" ColumnLines="True" RowLines="True">
                    <Store>
                        <ext:Store runat="server" ID="gridplstore">
                            <Model>
                                <ext:Model runat="server">
                                    <Fields>
                                        <ext:ModelField Name="DbID" runat="server"></ext:ModelField>
                                        <ext:ModelField Name="DbName" runat="server"></ext:ModelField>
                                        <ext:ModelField Name="DbCreationTime" runat="server"></ext:ModelField>
                                    </Fields>
                                </ext:Model>
                            </Model>
                        </ext:Store>
                    </Store>
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:Column ID="Column2"
                                runat="server"
                                Text="备份数据"
                                DataIndex="DbName"
                                Flex="1" />
                            <ext:DateColumn runat="server" Text="创建日期" DataIndex="DbCreationTime" Format="yyyy-MM-dd HH:mm:ss"  Flex="1" />
                            <ext:CommandColumn runat="server" Width="100" Text="还原">
                                <Commands>
                                    <ext:GridCommand Icon="DatabaseRefresh" CommandName="restore">
                                        <ToolTip Text="还原" />
                                    </ext:GridCommand>
                                   <%-- <ext:CommandSeparator />
                                    <ext:GridCommand Icon="ArrowDown" CommandName="download">
                                        <ToolTip Text="下载" />
                                    </ext:GridCommand>--%>
                                </Commands>
                                <Listeners>
                                    <Command Fn="Doit" />
                                </Listeners>
                            </ext:CommandColumn>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel runat="server" Mode="Single" />
                    </SelectionModel>
                    <TopBar>
                        <ext:Toolbar runat="server" ID="toolb">
                            <Items>
                                <ext:Button runat="server" Text="备份" ID="btnbak" OnDirectClick="btnbak_DirectClick"></ext:Button>
                                <%--  <ext:Button runat="server" Text="还原" ID="btngetbak" >
                                    <DirectEvents>
                                        <Click OnEvent="btngetbak_DirectClick">
                                            <ExtraParams>
                                                <ext:Parameter Name="Values" Value="Ext.encode(#{gridpl}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                 <ext:Button runat="server" Text="下载" ID="btndl" >
                                       <DirectEvents>
                                        <Click OnEvent="btndl_DirectClick">
                                            <ExtraParams>
                                                <ext:Parameter Name="Values" Value="Ext.encode(#{gridpl}.getRowsValues({selectedOnly:true}))" Mode="Raw" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                 </ext:Button>--%>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                </ext:GridPanel>



            </Items>
        </ext:Viewport>
        <ext:Window ID="Winadd" runat="server"
            Title="备份数据"
            Width="400"
            Height="130"
            Icon="ApplicationForm"
            AnimCollapse="false"
            Border="false"
            HideMode="Offsets"
            Resizable="false"
            Layout="FitLayout"
            CloseAction="Hide"
            Hidden="true">
            <Items>
                <ext:FormPanel runat="server" ID="resetform">
                    <Items>
                        <ext:TextField ID="AdName" runat="server" Name="AdName" FieldLabel="备份名称" AllowBlank="false" LabelWidth="80" Width="300" MarginSpec="10 0 0 30">
                        </ext:TextField>
                    </Items>

                    <DockedItems>
                        <ext:Toolbar ID="Toolbar2" runat="server" Dock="Bottom">
                            <Items>
                                <ext:Button ID="btnWinAdd" MarginSpec="0 0 0 0" runat="server" Text="确认" Icon="Add" OnDirectClick="btnWinAdd_DirectClick">
                                    <Listeners>
                                        <Click Handler="
                            if (!#{AdName}.validate() ) {
                                Ext.Msg.alert('提示','备份名称不能为空!'); 
                                            return false; 
                            }" />
                                    </Listeners>
                                </ext:Button>
                                <ext:Button ID="btnWinCancel" runat="server" Text="取消" Icon="ApplicationEdit" OnDirectClick="btnWinCancel_DirectClick"></ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </DockedItems>
                </ext:FormPanel>
            </Items>
        </ext:Window>
    </form>
</body>
</html>
