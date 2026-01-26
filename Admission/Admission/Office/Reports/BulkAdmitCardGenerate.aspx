<%@ Page Title="Generate Bulk Admit Card" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="BulkAdmitCardGenerate.aspx.cs" Inherits="Admission.Admission.Office.Reports.BulkAdmitCardGenerate" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <%--<link href="../../../Content/formStyle.css" rel="stylesheet" />--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">

        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Generate Bulk Admit Card</h4>
                </div>
                <div class="panel-body">

                    <div class="row">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <asp:UpdatePanel ID="UpdatePanelMassage" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="messagePanel" runat="server" Visible="false">
                                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>


                    <%--<asp:Label ID="lblMessage" runat="server" Text="Message"></asp:Label>--%>

                    <%--<asp:UpdatePanel ID="updatePanel_filter" runat="server">
                        <ContentTemplate>--%>
                    <asp:Panel ID="panel_Filter" runat="server">

                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-ms-4">
                                <b>Faculty/Program</b>
                                <asp:DropDownList ID="ddlAdmUnit" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlAdmissionUnitCompare" runat="server"
                                    ControlToValidate="ddlAdmUnit" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                    ForeColor="Crimson" Display="Dynamic" ValueToCompare="-1" Operator="NotEqual"
                                    ValidationGroup="gr1"></asp:CompareValidator>
                            </div>
                            <div class="col-lg-2 col-md-2 col-ms-2">
                                <b>Session</b>
                                <asp:DropDownList ID="ddlSession" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlSessionComV" runat="server"
                                    ControlToValidate="ddlSession" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                    ForeColor="Crimson" Display="Dynamic" ValueToCompare="-1" Operator="NotEqual"
                                    ValidationGroup="gr1"></asp:CompareValidator>
                            </div>
                            <div class="col-lg-2 col-md-2 col-ms-2">
                                <b>Min. SL</b>
                                <asp:TextBox ID="txtMin" runat="server" TextMode="Number" Text="0" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-lg-2 col-md-2 col-ms-2">
                                <b>Max. SL</b>
                                <asp:TextBox ID="txtMax" runat="server" TextMode="Number" Text="0" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>





                        <div class="row" style="margin-top:10px">
                             <div class="col-lg-2 col-md-2 col-ms-2">
                                <asp:Button ID="btnAdmitCardGenerate" runat="server" CssClass="btn-info form-control" Text="Generate (Previous)" ValidationGroup="gr1"
                                    OnClick="btnAdmitCardGenerate_Click" />
                            </div>
                             <div class="col-lg-2 col-md-2 col-ms-2">
                                <asp:Button ID="btnAdmitCardGenerateNew" runat="server" CssClass="btn-default form-control" Text="Generate (New)" ValidationGroup="gr1"
                                    OnClick="btnAdmitCardGenerate_ClickNew" />
                            </div>
                        </div>

                    </asp:Panel>
                    <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
            </div>
        </div>
        <%-- end col-md-12 --%>
    </div>
    <%-- end row 1 --%>


    <asp:UpdatePanel ID="upProgress" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Timer ID="ProgressTimer" runat="server" Interval="1000" Enabled="false" OnTick="ProgressTimer_Tick"></asp:Timer>

            <asp:Panel ID="pnlStatus" runat="server" Visible="false" Style="margin: 20px; padding: 15px; border: 1px solid #ccc; background: #f9f9f9;">
                <p>
                    Generating Admit Cards: 
                <strong>
                    <asp:Label ID="lblCurrent" runat="server" Text="0"></asp:Label></strong> of 
                <strong>
                    <asp:Label ID="lblTotal" runat="server" Text="0"></asp:Label></strong>
                </p>
                <div style="width: 100%; background: #ddd; height: 20px; border-radius: 5px;">
                    <div id="progressBar" runat="server" style="width: 0%; height: 100%; background: #28a745; border-radius: 5px; transition: width 0.5s;"></div>
                </div>
                <asp:Label ID="Label1" runat="server" ForeColor="Blue" Text="Processing..." />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%--    <div id="divProgress" style="display: none; z-index: 1000000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/AppImg/oscar_data_loop__1_.gif" Height="150px" Width="150px" />
    </div>--%>

    <%--<ajaxToolkit:UpdatePanelAnimationExtender
        ID="UpdatePanelAnimationExtender2"
        TargetControlID="UpdatePanel2"
        runat="server">
        <Animations>
            <OnUpdating>
               <Parallel duration="0">
                    <ScriptAction Script = "InProgress();" />
                    <EnableAction AnimationTarget = "btnAdmitCardGenerate" 
                                  Enabled = "false"/>
               </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnAdmitCardGenerate" 
                                    Enabled="true"/>
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>--%>

    <%--<div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
                        Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                        asynrendering="true" Width="100%" SizeToReportContent="True">
                    </rsweb:ReportViewer>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>--%>


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

</asp:Content>
