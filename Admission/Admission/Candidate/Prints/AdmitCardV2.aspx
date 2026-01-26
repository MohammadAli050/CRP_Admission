<%@ Page Title="Admit Card" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdmitCardV2.aspx.cs" Inherits="Admission.Admission.Candidate.Prints.AdmitCardV2" %>

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

    <div class="row">
        <div class="col-md-12">
            <div class="panel-defualt">
                <div class="panel-heading">
                    <h3><strong>Admit card</strong></h3>
                </div>
                <hr />
                <%--<p style="color: crimson; font-weight: bolder">NOTE: YOU MUST BRING COLOR PRINT OUT OF ADMIT CARD</p>--%>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Always" Visible="true">
                        <ContentTemplate>

                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                <asp:HiddenField ID="hfMessage" runat="server" />
                            </asp:Panel>

                            <asp:Panel ID="Panel1" runat="server" Visible="false">
                                <asp:Label ID="lblTemp" runat="server" Text="" ForeColor="Crimson" Font-Bold="true"></asp:Label><br />
                                <p>
                                    <asp:HyperLink ID="navUrl" NavigateUrl=""
                                        Visible="false" runat="server" Target="_blank"
                                        Text="Please refer to this link for more information regarding eligibility."></asp:HyperLink>
                                </p>
                            </asp:Panel>
                            <br />
                            <asp:GridView ID="gvAppliedProgs" runat="server"
                                CssClass="table table-responsive table-hover table-bordered table-condensed"
                                AutoGenerateColumns="false" Width="100%"
                                OnRowCreated="gvAppliedProgs_RowCreated">
                                <HeaderStyle BackColor="#1387de" ForeColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SL" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate><%# Container.DataItemIndex + 1 %>.</ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnitName" runat="server" Text='<%#Eval("UnitName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAcaCalId" runat="server" Text='<%#Eval("AcaCalID") %>'></asp:Label>
                                            <asp:Label ID="lblAdmUnitId" runat="server" Text='<%#Eval("admUnitID") %>'></asp:Label>
                                            <asp:Label ID="lblTestRoll" runat="server" Text='<%#Eval("TestRoll") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div style="text-align: center">
                                                <asp:LinkButton ID="btnDownloadAdmitCard" runat="server" Text="View/Download" OnClientClick="this.disabled = true"
                                                    OnClick="btnDownloadAdmitCard_Click" ToolTip="Download Admit Card"
                                                    CommandArgument='<%#Eval("PaymentId") %>'>
                                                </asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    Sorry, admit card not available.
                                </EmptyDataTemplate>
                            </asp:GridView>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <%-- END PANEL-DEFUALT --%>
        </div>
    </div>

    <asp:UpdatePanel ID="updatePaneAdmitCard" runat="server" Visible="false">
        <ContentTemplate>

            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
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

            <rsweb:ReportViewer ID="ReportViewerHospitalManagement" runat="server" Font-Names="Verdana"
                Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                AsyncRendering="true" Width="100%" SizeToReportContent="true">
            </rsweb:ReportViewer>

            <rsweb:ReportViewer ID="ReportViewerMESM" runat="server" Font-Names="Verdana"
                Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                AsyncRendering="true" Width="100%" SizeToReportContent="true">
            </rsweb:ReportViewer>

            <%--<LocalReport ReportPath="Admission/Candidate/Prints/AdmitCard.rdlc" EnableExternalImages="true">
                </LocalReport>

            <LocalReport ReportPath="Admission/Candidate/Prints/AdmitCardMasters.rdlc" EnableExternalImages="true">
                </LocalReport>--%>
        </ContentTemplate>
    </asp:UpdatePanel>



    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="updatePanel1" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnDownloadAdmitCard" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnDownloadAdmitCard" Enabled="true" />

                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
