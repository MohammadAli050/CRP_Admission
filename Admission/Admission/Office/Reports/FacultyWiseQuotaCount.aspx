<%@ Page Title="Faculty Wise Quota Count" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="FacultyWiseQuotaCount.aspx.cs" Inherits="Admission.Admission.Office.Reports.FacultyWiseQuotaCount" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">





    <script type="text/javascript">

        function InProgress() {
            var panelProg = $get('divProgress');
            panelProg.style.display = '';
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
        }

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="250px" Width="250px" />
    </div>

    <br />



    <asp:UpdatePanel ID="UpdatePanelAll" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">

                    <asp:Panel ID="messagePanel" runat="server" Visible="false">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </asp:Panel>

                    <div class="panel panel-default">
                        <div class="panel-heading"><b>Faculty Wise Quota Count</b></div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <asp:DropDownList ID="ddlSession" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                    <asp:CompareValidator ID="ddlSessionCompare" runat="server"
                                        ControlToValidate="ddlSession" Display="Dynamic"
                                        ErrorMessage="Session is required" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual" ValidationGroup="gr1"></asp:CompareValidator>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <asp:DropDownList ID="ddlEducationCategory" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server"
                                        ControlToValidate="ddlEducationCategory" Display="Dynamic"
                                        ErrorMessage="Education category is required" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual" ValidationGroup="gr1"></asp:CompareValidator>
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    <asp:Button ID="btnLoad" runat="server" ValidationGroup="gr1"
                                        CssClass="btn btn-info"
                                        Text="Load" OnClick="btnLoad_Click" />
                                </div>
                                <div class="col-sm-3 col-md-3 col-lg-3">
                                    
                                </div>
                            </div>
                            <br />

                            <asp:Panel ID="panelForFilter" runat="server" Visible="false">
                                <div class="row">
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <asp:DropDownList ID="ddlAdmissionUnit" runat="server" Width="100%" CssClass="form-control"
                                            AutoPostBack="true" OnSelectedIndexChanged="filterQuotaInfo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <asp:DropDownList ID="ddlQuota" runat="server" Width="100%" CssClass="form-control"
                                            AutoPostBack="true" OnSelectedIndexChanged="filterQuotaInfo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <asp:DropDownList ID="ddlQuotaType" runat="server" Width="100%" CssClass="form-control"
                                            AutoPostBack="true" OnSelectedIndexChanged="filterQuotaInfo_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3">
                                        <asp:Button ID="btnClear" runat="server" 
                                        CssClass="btn btn-default"
                                        Text="Clear" OnClick="btnClear_Click" />
                                    </div>
                                </div>
                            </asp:Panel>

                        </div>
                    </div>


                </div>
            </div>

            <br />

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
                        Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                        AsyncRendering="true" Width="100%" SizeToReportContent="true" Visible="false">
                        <LocalReport ReportPath="Admission/Office/Reports/FacultyWiseQuotaCount.rdlc" EnableExternalImages="true"></LocalReport>
                    </rsweb:ReportViewer>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnLoad" />
        </Triggers>
    </asp:UpdatePanel>



      <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="UpdatePanelAll" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnLoad" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnLoad" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>
</asp:Content>
