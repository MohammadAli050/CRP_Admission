<%@ Page Title="Status Report (Final Submit)" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="RPTStatusReportFinalSubmit.aspx.cs" Inherits="Admission.Admission.Office.Reports.RPTStatusReportFinalSubmit" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>



<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="updatePanelAll" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <h3>Status Report (Final Submit)</h3>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-body">

                    <div class="row">
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <div class="form-group">
                                <label><strong>Faculty </strong></label>
                                <asp:DropDownList ID="ddlAdmUnit" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <div class="form-group">
                                <label><strong>Session <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlSession" runat="server" Width="100%" CssClass="form-control">
                                </asp:DropDownList>
                                <asp:CompareValidator ID="ddlNationality_ComV" runat="server" 
                                    ControlToValidate="ddlSession" 
                                    ErrorMessage="Required"
                                    Font-Bold="true" 
                                    Font-Size="9pt" 
                                    ForeColor="Crimson"
                                    Display="Dynamic" 
                                    ValueToCompare="-1" 
                                    Operator="NotEqual" 
                                    ValidationGroup="gr1"></asp:CompareValidator>
                            </div>
                        </div>
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <br />
                                <asp:Button ID="btnLoadData" runat="server" Text="Load" Width="100%" CssClass="btn btn-info" 
                                    OnClick="btnLoadData_Click" ValidationGroup="gr1"/>
                        </div>
                    </div>

                    

                </div>
            </div>


            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btnLoadData" />
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>
