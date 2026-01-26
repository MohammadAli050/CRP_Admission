<%@ Page Title="Back Up DB" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="BackupDb.aspx.cs" Inherits="Admission.Admission.Admin.BackupDb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../Content/formStyle.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Back Up</strong>
                </div>
                <div class="panel-body">
                    <asp:Panel ID="panelFilter" runat="server">
                        <asp:Button ID="btnCreateBack" runat="server" Text="Create BackUp Media Now" OnClick="btnCreateBack_Click" />
                        &nbsp;&nbsp;
                        <asp:Label ID="lblMessage" runat="server" Text="No Message"></asp:Label>
                    </asp:Panel>
                    <asp:Panel ID="panelFilterLog" runat="server">
                        <asp:Button ID="btnCreateBackLog" runat="server" Text="Create BackUp Media Now (Log)" OnClick="btnCreateBackLog_Click" />
                        &nbsp;&nbsp;
                        <asp:Label ID="lblMessageLog" runat="server" Text="No Message"></asp:Label>
                    </asp:Panel>
                </div>
                <%-- END PANEL-BODY --%>
            </div>
            <%-- END PANEL-DEFAULT --%>
        </div>
        <%-- END COL-MD_12 --%>
    </div>
    <%-- END ROW --%>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-body">
                    <asp:Panel ID="panelGridViewMessage" runat="server">
                        <asp:Label ID="lblMessageGv" runat="server" Text="No Message"></asp:Label>
                    </asp:Panel>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" EmptyDataText="No files found"
                        CssClass="table table-bordered table-condensed table-hover table-responsive table-striped">
                        <Columns>
                            <asp:TemplateField HeaderText="SL#" HeaderStyle-Width="3%">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FileName" HeaderText="File Name" />
                            <asp:BoundField DataField="DateModified" HeaderText="Date Modified" />
                            <asp:BoundField DataField="FileSize" HeaderText="File Size" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDownload" Text="Download" CommandArgument='<%# Eval("FilePath") %>' runat="server" OnClick="lnkDownload_Click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDelete" Text="Delete" CommandArgument='<%# Eval("FilePath") %>' runat="server" OnClick="lnkDelete_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <%-- END PANEL-BODY --%>
            </div>
            <%-- END PANEL-DEFAULT --%>
        </div>
        <%-- END COL-MD_12 --%>
    </div>

</asp:Content>
