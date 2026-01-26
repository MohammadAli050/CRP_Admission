<%@ Page Title="Candidate List of final Submission" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="RPTPaidandFinalSumbitReport.aspx.cs" Inherits="Admission.Admission.Office.Reports.RPTPaidandFinalSumbitReport" %>


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

    <asp:UpdatePanel ID="updatePanelAll" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <h3 style="font-size: 18px"><b>Candidate List of final Submission</b></h3>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-body">

                    <div class="row">
                        <div class="col-sm-4 col-md-4 col-lg-4">
                            <div class="form-group">
                                <label><strong>Faculty </strong></label>
                                <asp:DropDownList ID="ddlAdmUnit" runat="server" Width="100%" CssClass="form-control"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlAdmUnit_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Session <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlSession" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlSession_SelectedIndexChanged">
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
                        
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Final Submitted Status </strong></label>
                                <asp:DropDownList ID="ddlSubmitStatus" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlSubmitStatus_SelectedIndexChanged">
                                    <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Submitted" Selected="True" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Not Submitted" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2" >
                            <div class="form-group">
                                <label><strong>Payment Status</strong></label>
                                <asp:DropDownList ID="ddlPaymentStatus" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentStatus_SelectedIndexChanged">
                                    <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Paid" Selected="True" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="UnPaid" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <br />
                            <asp:Button ID="btnLoadData" runat="server" Text="Load" Width="100%" CssClass="btn btn-info form-control"
                                OnClick="btnLoadData_Click" ValidationGroup="gr1" />
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

            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
                Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                AsyncRendering="true" Width="100%" SizeToReportContent="true" Visible="true">
            </rsweb:ReportViewer>


        </ContentTemplate>
        <%--<Triggers>
            <asp:PostBackTrigger ControlID="btnLoadData" />
        </Triggers>--%>
    </asp:UpdatePanel>


    
        <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="updatePanelAll" runat="server">
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
