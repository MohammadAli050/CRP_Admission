<%@ Page Title="Maintenance Setup" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="MaintenanceSetup.aspx.cs" Inherits="Admission.Admission.Admin.MaintenanceSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Setup Maintenance Notice</strong>
                </div>
                <div class="panel-body">
                    <table class="table_fullwidth table_form">
                        <tr>
                            <td class="style_td" style="width: 20%">Show Maintenance Notice</td>
                            <td style="width: 80%">
                                <asp:CheckBox ID="chbxShowMaintenanceNotice" runat="server" />
                                &nbsp;
                        <asp:Label ID="lblIsActive" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Message</td>
                            <td>
                                <asp:TextBox ID="txtMaintenanceNotice" runat="server" TextMode="MultiLine" Width="50%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnSave" Text="Update" runat="server" OnClick="btnSave_Click" />
                </div>
                <%-- END PANEL-BODY --%>
            </div>
            <%-- END PANEL-DEFAULT --%>
        </div>
        <%-- END COL-MD_12 --%>
    </div>
    <%-- END ROW --%>
</asp:Content>
