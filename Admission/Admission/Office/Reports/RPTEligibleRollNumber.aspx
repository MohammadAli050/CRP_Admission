<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="RPTEligibleRollNumber.aspx.cs" Inherits="Admission.Admission.Office.Reports.RPTEligibleRollNumber" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../../Content/formStyle.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            width: 15%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Eligible Roll Number's (Bachelors)</strong>
                </div>
                <div class="panel-body">

                    <asp:UpdatePanel ID="UpdatePanelMassage" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td class="style_td" style="width: 10%">Upload File (Excel)</td>
                                    <td class="auto-style1">
                                        <asp:FileUpload runat="server" ID="fuExcel" />
                                    </td>
                                    <td class="style_td style_td_secondCol" style="width: 10%">
                                        <asp:Button runat="server" ID="btnUpload" Text="Upload" ValidationGroup="gr1"
                                            OnClick="btnUpload_Click" />
                                    </td>
                                    <td style="width: 40%"></td>
                                </tr>

                                <tr>
                                    <td class="style_td" style="width: 10%">
                                        <asp:Button runat="server" ID="btnDownload" Text="View" OnClick="btnDownload_Click" />
                                    </td>
                                    <td class="auto-style1"></td>
                                    <td class="style_td style_td_secondCol" style="width: 10%"></td>
                                    <td style="width: 40%"></td>
                                </tr>

                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnUpload" />
                        </Triggers>
                    </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>



    <asp:UpdatePanel ID="UpdatePanel_Lv" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="row" style="margin-bottom: 1%; padding-left: 1%">
                            Records: &nbsp;
                                <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                        </div>
                        <div class="panel-body">

                            <asp:ListView ID="lvEligibleRollNumber" runat="server"
                                OnItemDataBound="lvEligibleRollNumber_ItemDataBound">
                                <LayoutTemplate>
                                    <table id="tbl"
                                        class="table table-hover table-condensed table-striped"
                                        style="width: 100%; text-align: left">
                                        <tr runat="server" style="background-color: #1387de; color: white;">
                                            <th runat="server">SL#</th>
                                            <th runat="server">Eligible Roll Number</th>
                                            <th runat="server">IsEligible</th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder" />
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr runat="server" style="">
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblSerial" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblEligibleRollNumber" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblIsEligible" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <div class="alert alert-warning" role="alert" style="text-align: center">No item to display.</div>
                                </EmptyDataTemplate>
                            </asp:ListView>
                        </div>
                        <%-- END PANEL-BODY --%>
                    </div>
                </div>
                <%-- END COL-MD-12 --%>
            </div>
            <%-- END ROW 2 --%>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
