<%@ Page Title="Admit Card" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdmitCard.aspx.cs" Inherits="Admission.Admission.Candidate.Prints.AdmitCard" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel-defualt">
                <div class="panel-heading">
                    <h3><strong>Admit card</strong></h3>
                </div>
                <hr />
                <%--<p style="color: crimson; font-weight: bolder">NOTE: Admit Card will be available from 13 October, 2019</p>--%>

               <%-- <h3>
                    <p style="color: crimson; font-weight: bolder">Website is under maintenance. You can download Admit Card on 17 October, 2019 onwards after<br /> 2 PM </p>
                </h3>--%>

                <p style="color: crimson; font-weight: bolder">NOTE: YOU MUST BRING COLOR PRINT OUT OF ADMIT CARD</p>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanel1" runat="server" UpdateMode="Always" Visible="true">
                        <ContentTemplate>

                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                <asp:HiddenField ID="hfMessage" runat="server" />
                            </asp:Panel>

                            <%-- Panel1 might be needed in the future for undergrad admission --%>
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
                                            <%--<asp:Label ID="Label2" runat="server" Text='<%#Eval("") %>'></asp:Label>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnDownloadAdmitCard" runat="server"
                                                OnClick="btnDownloadAdmitCard_Click" ToolTip="Download Admit Card"
                                                CommandArgument='<%#Eval("PaymentId") %>'>
                                                <div align="center">
                                                    View/Download
                                                </div> 
                                            </asp:LinkButton>
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
            <div>
                <p>
                    <%--Note: Click the Disk icon to view your admit card.--%>
                </p>
            </div>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana"
                Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                AsyncRendering="true" Width="100%" SizeToReportContent="true">
                <LocalReport ReportPath="Admission/Candidate/Prints/AdmitCard.rdlc" EnableExternalImages="true">
                </LocalReport>
            </rsweb:ReportViewer>

            <rsweb:ReportViewer ID="ReportViewerMasters" runat="server" Font-Names="Verdana"
                Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt"
                AsyncRendering="true" Width="100%" SizeToReportContent="true">
                <LocalReport ReportPath="Admission/Candidate/Prints/AdmitCardMasters.rdlc" EnableExternalImages="true">
                </LocalReport>
            </rsweb:ReportViewer>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
