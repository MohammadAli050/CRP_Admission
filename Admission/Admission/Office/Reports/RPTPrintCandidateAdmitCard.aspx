<%@ Page Title="Candidate Admit Card" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="RPTPrintCandidateAdmitCard.aspx.cs" Inherits="Admission.Admission.Office.Reports.RPTPrintCandidateAdmitCard" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <script type="text/javascript">

        //function InProgress() {
        //    var panelProg = $get('divProgress');
        //    panelProg.style.display = '';
        //}

        //function onComplete() {
        //    var panelProg = $get('divProgress');
        //    panelProg.style.display = 'none';
        //}

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%--<div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="250px" Width="250px" />
    </div>--%>

    <%--<br />--%>

    <asp:UpdatePanel ID="updatePanelAll" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <h3>Candidate Admit Card</h3>
                    <%--<hr style="margin-top: 0px; margin-bottom: 10px;" />--%>
                </div>
            </div>


             <%--type="number"--%>

            <div class="panel panel-default">
                <div class="panel-body">

                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Payment ID/Test Roll/User ID <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:TextBox ID="txtPaymentId"  runat="server"  
                                    placeholder="Entry Payment ID"
                                    Width="100%" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="txtPlaceOfBirth_ReqV" runat="server"
                                    ControlToValidate="txtPaymentId"
                                    ErrorMessage="Required"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    Font-Bold="true"
                                    ValidationGroup="gr1">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <br />
                            <asp:Button ID="btnPaymentIdSearch" runat="server" Text="Load"
                                Style="width: 100%; margin-top: 4px;"
                                ValidationGroup="gr1"
                                CssClass="btn btn-info"
                                OnClick="btnPaymentIdSearch_Click" />

                            <asp:HiddenField ID="hfCandidateID" runat="server" />
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Faculty <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlAdmissionUnit" runat="server"
                                    Style="width: 100%; margin-top: 4px;" CssClass="form-control"
                                    OnSelectedIndexChanged="ddlAdmissionUnit_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <br />
                            <asp:Button ID="btnDownloadAdmitCard" runat="server" Text="Download"
                                Style="width: 100%; margin-top: 4px;"
                                ValidationGroup="gr1"
                                CssClass="btn btn-default"
                                OnClick="btnDownloadAdmitCard_Click" />
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



            <div class="panel panel-default">
                <div class="panel-body">
                    <div>
                        <rsweb:ReportViewer ID="ReportViewerBachelor" runat="server" Font-Names="Verdana"
                            Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                            AsyncRendering="true" Width="100%" SizeToReportContent="true">
                        </rsweb:ReportViewer>

                        <rsweb:ReportViewer ID="ReportViewerMBAProfessional" runat="server" Font-Names="Verdana"
                            Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                            AsyncRendering="true" Width="100%" SizeToReportContent="true">
                        </rsweb:ReportViewer>

                        <rsweb:ReportViewer ID="ReportViewerLLMProfessional" runat="server" Font-Names="Verdana"
                            Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                            AsyncRendering="true" Width="100%" SizeToReportContent="true">
                        </rsweb:ReportViewer>

                        <rsweb:ReportViewer ID="ReportViewerMPCHRS" runat="server" Font-Names="Verdana"
                            Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                            AsyncRendering="true" Width="100%" SizeToReportContent="true">
                        </rsweb:ReportViewer>

                        <rsweb:ReportViewer ID="ReportViewerMICT" runat="server" Font-Names="Verdana"
                            Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                            AsyncRendering="true" Width="100%" SizeToReportContent="true">
                        </rsweb:ReportViewer>

                        <rsweb:ReportViewer ID="ReportViewerMISS" runat="server" Font-Names="Verdana"
                            Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                            AsyncRendering="true" Width="100%" SizeToReportContent="true">
                        </rsweb:ReportViewer>

                        <rsweb:ReportViewer ID="ReportViewerMPH" runat="server" Font-Names="Verdana"
                            Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                            AsyncRendering="true" Width="100%" SizeToReportContent="true">
                        </rsweb:ReportViewer>

                        <rsweb:ReportViewer ID="ReportViewerMCSE" runat="server" Font-Names="Verdana"
                            Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                            AsyncRendering="true" Width="100%" SizeToReportContent="true">
                        </rsweb:ReportViewer>

                        <rsweb:ReportViewer ID="ReportViewerMDSProfessional" runat="server" Font-Names="Verdana"
                            Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                            AsyncRendering="true" Width="100%" SizeToReportContent="true">
                        </rsweb:ReportViewer>
                        <rsweb:ReportViewer ID="ReportViewerMCS" runat="server" Font-Names="Verdana"
                            Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                            AsyncRendering="true" Width="100%" SizeToReportContent="true">
                        </rsweb:ReportViewer>
                        <rsweb:ReportViewer ID="ReportViewerMESM" runat="server" Font-Names="Verdana"
                            Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                            AsyncRendering="true" Width="100%" SizeToReportContent="true">
                        </rsweb:ReportViewer>
                        <rsweb:ReportViewer ID="ReportViewerHospitalManagement" runat="server" Font-Names="Verdana"
                            Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                            AsyncRendering="true" Width="100%" SizeToReportContent="true">
                        </rsweb:ReportViewer>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownloadAdmitCard" />
        </Triggers>
    </asp:UpdatePanel>

    <%--
    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="updatePanelAll" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnPaymentIdSearch" Enabled="false" />
                    <EnableAction AnimationTarget="ddlAdmissionUnit" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnPaymentIdSearch" Enabled="true" />
                    <EnableAction   AnimationTarget="ddlAdmissionUnit" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>--%>


    <%--<div class="row">
        <div class="col-md-12">
            <div class="panel-default">

                <div class="panel-heading">
                    <strong>REPORT - Print Admit Card</strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanelFilter" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                           
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td style="width: 10%">Mobile Number</td>
                                    <td>
                                        <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
                                        <asp:Button ID="btnMobileSearch" runat="server" Text="Load"
                                            OnClick="btnMobileSearch_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Payment ID</td>
                                    <td>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">Faculty</td>
                                    <td>
                                        
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="updatePanelAdmitCard" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <asp:Panel ID="messagePanel1" runat="server">
                        <asp:Label ID="lblMessage1" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
                        Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                        AsyncRendering="true" Width="100%" SizeToReportContent="true">
                        <LocalReport ReportPath="Admission/Office/Reports/AdmitCard.rdlc" EnableExternalImages="true"></LocalReport>
                    </rsweb:ReportViewer>

                    <rsweb:ReportViewer ID="ReportViewerMasters" runat="server" Font-Names="Verdana"
                        Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                        AsyncRendering="true" Width="100%" SizeToReportContent="true">
                        <LocalReport ReportPath="Admission/Office/Reports/AdmitCardMasters.rdlc" EnableExternalImages="true"></LocalReport>
                    </rsweb:ReportViewer>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
