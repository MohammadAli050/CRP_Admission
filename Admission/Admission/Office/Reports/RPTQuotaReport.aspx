<%@ Page Title="Quota Wise Summary and Verification Report" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true"
    CodeBehind="RPTQuotaReport.aspx.cs" Inherits="Admission.Admission.Office.Reports.RPTQuotaReport" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <asp:UpdatePanel ID="updatePanelAll" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <h3>Quota Wise Summary and Verification Report</h3>
                </div>
            </div>


            <div class="panel panel-default">
                <div class="panel-body">

                    <div class="row">
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Category</strong></label>
                                <asp:DropDownList ID="ddlEducationCategory" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Faculty</strong></label>
                                <asp:DropDownList ID="ddlAdmUnit" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Quota</strong></label>
                                <asp:DropDownList ID="ddlQuota" runat="server" Width="100%" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Session <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlSession" runat="server" Width="100%" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <br />
                            <asp:Button ID="btnLoad" runat="server" Text="Load" Width="100%" CssClass="btn btn-info"
                                OnClick="btnLoad_Click" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </asp:Panel>
                        </div>
                    </div>

                </div>
            </div>

            <hr />

            <div class="row">
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                    AsyncRendering="true" Width="100%" SizeToReportContent="true" Visible="true">
                    <%--<LocalReport ReportPath="Admission/Office/Reports/RPTAttendanceReport.rdlc" EnableExternalImages="true">
                    </LocalReport>--%>
                </rsweb:ReportViewer>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
