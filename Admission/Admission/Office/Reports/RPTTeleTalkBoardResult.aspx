<%@ Page Title="TeleTalk Board Result Verification" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="RPTTeleTalkBoardResult.aspx.cs" Inherits="Admission.Admission.Office.Reports.RPTTeleTalkBoardResult" %>

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


    <%--   <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="250px" Width="250px" />
    </div>

    <br />--%>

    <asp:UpdatePanel ID="updatePanelAll" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <h3>TeleTalk Board Result Verification</h3>
                    <hr style="margin-top: 0px; margin-bottom: 10px;" />
                </div>
            </div>




            <div class="panel panel-default">
                <div class="panel-body">

                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Faculty <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlAdmUnit" runat="server" Width="100%" CssClass="form-control">
                                </asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator2" runat="server"
                                    ControlToValidate="ddlAdmUnit" ErrorMessage="*" Font-Size="14pt" Font-Bold="true"
                                    ForeColor="Crimson" Display="Dynamic" ValueToCompare="-1" Operator="NotEqual"
                                    ValidationGroup="gr1"></asp:CompareValidator>
                            </div>
                        </div>

                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Category <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlEducationCategory" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlEducationCategoryComV" runat="server"
                                    ControlToValidate="ddlEducationCategory" ErrorMessage="*" Font-Size="14pt" Font-Bold="true"
                                    ForeColor="Crimson" Display="Dynamic" ValueToCompare="-1" Operator="NotEqual"
                                    ValidationGroup="gr1"></asp:CompareValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Session <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlSession" runat="server" Width="100%" CssClass="form-control">
                                </asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator1" runat="server"
                                    ControlToValidate="ddlSession" ErrorMessage="*" Font-Size="14pt" Font-Bold="true"
                                    ForeColor="Crimson" Display="Dynamic" ValueToCompare="-1" Operator="NotEqual"
                                    ValidationGroup="gr1"></asp:CompareValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <br />
                            <asp:Button ID="btnViewProperty" runat="server" Text="View Property" Width="100%" CssClass="btn btn-default"
                                OnClick="btnViewProperty_Click" />
                        </div>
                    </div>

                </div>
            </div>



            <asp:Panel ID="panelViewResult" runat="server" Visible="false">



                <div class="row">
                    <div class="col-sm-6 col-md-6 col-lg-6">
                        <div class="panel panel-default">
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-sm-6 col-md-6 col-lg-6">
                                        <asp:Button ID="btnGetDataFromTeleTalk" runat="server" Text="Get Data From TeleTalk" Width="100%" CssClass="btn btn-danger"
                                            ValidationGroup="gr1"
                                            OnClick="btnGetDataFromTeleTalk_Click" />
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-6">
                                        <asp:Button ID="btnLoad" runat="server" Text="View" Width="100%" CssClass="btn btn-default"
                                            OnClick="btnLoad_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="col-sm-6 col-md-6 col-lg-6">


                        <div class="panel panel-default">
                            <div class="panel-body">

                                <span>Upload File (Excel)</span><br />
                                <asp:FileUpload runat="server" ID="fuExcel" />
                                <asp:Button runat="server" ID="btnUploadEligibleRoll" Text="Upload" OnClick="btnUploadEligibleRoll_Click" />


                            </div>
                        </div>

                    </div>
                </div>





            </asp:Panel>


            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                </div>
            </div>

            <hr />


            <div>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
                    Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                    AsyncRendering="true" Width="100%" SizeToReportContent="true" Visible="true">
                    <LocalReport ReportPath="Admission/Office/Reports/RPTTeleTalkBoardResult.rdlc" EnableExternalImages="true">
                    </LocalReport>
                </rsweb:ReportViewer>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUploadEligibleRoll" />
        </Triggers>
    </asp:UpdatePanel>


    <%--   <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="updatePanelAll" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnSubmit" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnSubmit" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>--%>
</asp:Content>
