<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="HD_ApplicationAdditional.aspx.cs" Inherits="Admission.Admission.HelpDesk.HD_ApplicationAdditional" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../../Content/ApplicationForm.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading panelHeaderHeight">
                    <ol class="breadcrumb">
                        <%--<li class="active">Basic</li>--%>
                        <li><asp:HyperLink ID="hrefAppBasic" runat="server">Basic Info</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppPriority" runat="server">Program Priority</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppEducation" runat="server">Education</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppRelation" runat="server">Parent/Guardian</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppAddress" runat="server">Address</asp:HyperLink></li>
                        <li class="active"><asp:HyperLink ID="hrefAppAdditional" runat="server">Additional/Work Experience</asp:HyperLink></li>
                        <%--<li><asp:HyperLink ID="hrefAppFinGuar" runat="server">Financial Guarantor</asp:HyperLink></li>--%>
                        <li><asp:HyperLink ID="hrefAppAttachment" runat="server">Upload Photo</asp:HyperLink></li>
                    </ol>
                </div>
                <%-- end panel heading --%>
                <div class="panel-body">


                    <asp:UpdatePanel ID="updatePanelAdditional" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%" class="table table-condensed table-striped">
                                <tr>
                                    <td style="width: 30%" class="style_td verAlignMid">Have you ever been admitted to 
                                        <asp:Label ID="lblUniShortName" runat="server"></asp:Label>? 
                                        <span class="spanAsterisk">*</span>
                                    </td>
                                    <td style="width: 70%">
                                        <asp:DropDownList ID="ddlAdmittedBefore" runat="server" Width="45%" CssClass="form-control"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlAdmittedBefore_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td verAlignMid">If yes, state Student ID No. <span class="spanAsterisk">*</span></td>
                                    <td>
                                        <asp:TextBox ID="txtCurrentStudentId" runat="server" Width="45%" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td verAlignMid">Candidate Annual Income</td>
                                    <td>
                                        <asp:TextBox ID="txtCandidateAnnualIncome" runat="server" Width="45%" CssClass="form-control"
                                            TextMode="Number"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td verAlignMid">Father Annual Income <span class="spanAsterisk">*</span></td>
                                    <td>
                                        <asp:TextBox ID="txtFatherAnnualIncome" runat="server" Width="45%" CssClass="form-control"
                                            TextMode="Number"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td verAlignMid">Mother Annual Income </td>
                                    <td>
                                        <asp:TextBox ID="txtMotherAnnualIncome" runat="server" Width="45%" CssClass="form-control"
                                            TextMode="Number"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="panel_Occupation" runat="server">
                                <table style="width: 100%" class="table table-condensed table-striped">
                                    <tr>
                                        <th class="info" colspan="4"><%--<td colspan="2">Occupation details (if any)</td>--%>Occupation details (if any) </th>
                                        <tr>
                                            <td class="style_td" style="width: 10%">Designation</td>
                                            <td style="width: 40%">
                                                <asp:TextBox ID="txtWorkDesignation" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                            </td>
                                            <td class="style_td style_td_secondCol" style="width: 10%">Organization</td>
                                            <td style="width: 40%">
                                                <asp:TextBox ID="txtWorkOrganization" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                                <span style="color: cornflowerblue">Please provide the exact name of the organization.</span> </td>
                                        </tr>
                                        <tr>
                                            <td class="style_td">Office Address</td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtWorkAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style_td">Start Date</td>
                                            <td>
                                                <asp:TextBox ID="txtStartDateWE" runat="server" CssClass="form-control" placeholder="dd/MM/yyyy" Width="50%"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="txtStartDateWECE" runat="server" Format="dd/MM/yyyy" TargetControlID="txtStartDateWE" />
                                            </td>
                                            <td class="style_td style_td_secondCol">End Date</td>
                                            <td>
                                                <asp:TextBox ID="txtEndDateWE" runat="server" CssClass="form-control" placeholder="dd/MM/yyyy" Width="50%"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="txtEndDateWECE" runat="server" Format="dd/MM/yyyy" TargetControlID="txtEndDateWE" />
                                            </td>
                                        </tr>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table style="width: 100%" class="table table-condensed table-striped">
                                <tr>
                                    <th class="info" colspan="6"><%--<td colspan="2">Occupation details (if any)</td>--%>Extracurricular Activity (if any) </th>
                                    <tr>
                                        <td class="style_td verAlignMid" style="width: 6%">Activity</td>
                                        <td style="width: 27%">
                                            <asp:TextBox ID="txtActivity1" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                        </td>
                                        <td class="style_td style_td_secondCol" style="width: 6%">Award</td>
                                        <td style="width: 27%">
                                            <asp:TextBox ID="txtAward1" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                        </td>
                                        <td class="style_td style_td_secondCol" style="width: 6%">Date</td>
                                        <td style="width: 27%">
                                            <asp:TextBox ID="txtEcaDate1" runat="server" CssClass="form-control" placeholder="dd/MM/yyyy" Width="100%"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="txtEcaDate1_CE" runat="server" Format="dd/MM/yyyy" TargetControlID="txtEcaDate1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style_td">Activity</td>
                                        <td>
                                            <asp:TextBox ID="txtActivity2" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                        </td>
                                        <td class="style_td style_td_secondCol">Award</td>
                                        <td>
                                            <asp:TextBox ID="txtAward2" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                        </td>
                                        <td class="style_td style_td_secondCol">Date</td>
                                        <td>
                                            <asp:TextBox ID="txtEcaDate2" runat="server" CssClass="form-control" placeholder="dd/MM/yyyy" Width="100%"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="txtEcaDate2_CE" runat="server" Format="dd/MM/yyyy" TargetControlID="txtEcaDate2" />
                                        </td>
                                    </tr>
                                </tr>
                            </table>

                            <asp:Panel ID="messagePanel_Additional" runat="server">
                                <asp:Label ID="lblMessageAdditional" runat="server" Text=""></asp:Label>
                            </asp:Panel>

                            <%--<asp:Button ID="btnSave_Additional" runat="server" Text="Save"
                                CssClass="btn btn-primary" OnClick="btnSave_Additional_Click"/>

                            <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                                CssClass="btn btn-primary" />--%>

                            <span id="validationMsg" class="validationErrorMsg"></span>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <%-- end panel body --%>
            </div>
            <%-- end panel default --%>
        </div>
        <%-- end col-md-12 --%>
    </div>
    <%-- end row --%>
</asp:Content>
