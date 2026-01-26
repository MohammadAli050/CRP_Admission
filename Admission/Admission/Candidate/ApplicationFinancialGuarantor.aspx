<%@ Page Title="Application Form - Financial Guarantor" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApplicationFinancialGuarantor.aspx.cs" Inherits="Admission.Admission.Candidate.ApplicationFinancialGuarantor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/ApplicationForm.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading panelHeaderHeight">
                    <ol class="breadcrumb">
                        <li><a href="ApplicationBasic.aspx">Basic</a></li>
                        <li><a href="ApplicationPriority.aspx">Program Priority</a></li>
                        <li><a href="ApplicationEducation.aspx">Education</a></li>
                        <li><a href="ApplicationRelation.aspx">Parent/Guardian</a></li>
                        <li><a href="ApplicationAddress.aspx">Address</a></li>
                        <li><a href="ApplicationAdditional.aspx">Additional/Work Experience</a></li>
                        <li class="active">Financial Guarantor</li>
                        <li><a href="ApplicationAttachment.aspx">Upload Photo</a></li>
                        <li><a href="ApplicationDeclaration.aspx">Declaration</a></li>
                    </ol>
                </div>
                <%-- end panel heading --%>
                <div class="panel-body">


                    <asp:UpdatePanel ID="updatePanelFinGuarantor" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%; margin-bottom: 1%">
                                <tr>
                                    <td style="width: 45%" class="style_td">Source of fund to meet total expenses for study with income certificate of guardian:
                                    </td>
                                    <td style="width: 55%">
                                        <asp:TextBox ID="txtSrcFundIncomeCertGuardian" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                        <span style="color: cornflowerblue">Candidate must provide the documents during Viva/Interview.</span>
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%" class="table table-condensed table-striped">
                                <tr>
                                    <td colspan="2"><strong>Name and Business Address of the Financial Guarantor (All correspondence including academic records will be released to this person unless admised otherwise in writing)</strong></td>
                                </tr>
                                <tr>
                                    <td style="width: 15%" class="style_td">Name (Full Name)</td>
                                    <td style="width: 85%">
                                        <asp:TextBox ID="txtFinGuarantorName" runat="server" Width="50%" CssClass="form-control"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td">Relationship with the applicant</td>
                                    <td>
                                        <asp:DropDownList ID="ddlRelationWithGuarantor" runat="server" Width="35%" CssClass="form-control"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlRelationWithGuarantor_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td">Others</td>
                                    <td>
                                        <asp:TextBox ID="txtRelationWithGuarantorOthers" runat="server" Width="50%" CssClass="form-control"
                                            Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td">Occupation</td>
                                    <td>
                                        <asp:TextBox ID="txtFinGuarantorOccupation" runat="server" Width="50%" CssClass="form-control"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td">Organization</td>
                                    <td>
                                        <asp:TextBox ID="txtFinGuarantorOrganization" runat="server" Width="50%" CssClass="form-control"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td">Position</td>
                                    <td>
                                        <asp:TextBox ID="txtFinGuarantorPosition" runat="server" Width="50%" CssClass="form-control"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td">Mailing Address
                                                                        <br />
                                        <strong>(Postal)</strong></td>
                                    <td>
                                        <asp:TextBox ID="txtFinGuarantorAddress" runat="server" Width="50%" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td">Email (if any)</td>
                                    <td>
                                        <asp:TextBox ID="txtFinGuarantorEmail" runat="server" Width="50%" CssClass="form-control" TextMode="Email"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td">Mobile</td>
                                    <td>
                                        <asp:TextBox ID="txtFinGuarantorMobile" runat="server" Width="50%" CssClass="form-control"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td">Source of Fund</td>
                                    <td>
                                        <asp:TextBox ID="txtFinGuarantorSourceFund" runat="server" Width="50%" CssClass="form-control"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"><strong>I agree to bear the financial responsibility of this student to meet total expenses for study and I also declare that I will abide by the rules and regulations of the university.</strong></td>
                                </tr>
                            </table>

                            <asp:Panel ID="messagePanel_FinGuar" runat="server">
                                <asp:Label ID="lblMessageFinGuar" runat="server" Text=""></asp:Label>
                            </asp:Panel>

                            <asp:Button ID="btnSave_Guarantor" runat="server" Text="Save Financial Guarantor Info"
                                CssClass="btn btn-primary" OnClick="btnSave_Guarantor_Click"
                                ValidationGroup="finguar1" />


                            <asp:Button ID="btnNext" runat="server" Text="Next"
                                CssClass="btn btn-primary" OnClick="btnNext_Click" />

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
