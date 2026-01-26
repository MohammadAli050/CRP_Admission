<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PostgraduateDiplomaApplicationRelation.aspx.cs" Inherits="Admission.Admission.PostgraduateDiploma.PostgraduateDiplomaApplicationRelation" %>









<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/ApplicationForm.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading panelHeaderHeight">
                    <ol class="breadcrumb">
                        <li>
                            <asp:HyperLink ID="hrefAppBasic" runat="server">Basic Info</asp:HyperLink></li>
                        <%--<li>
                            <asp:HyperLink ID="hrefAppPriority" runat="server">Program Priority</asp:HyperLink></li>--%>
                        <li>
                            <asp:HyperLink ID="hrefAppEducation" runat="server">Education</asp:HyperLink></li>
                        <li class="active">
                            <asp:HyperLink ID="hrefAppRelation" runat="server">Parent/Guardian</asp:HyperLink></li>
                        <li>
                            <asp:HyperLink ID="hrefAppAddress" runat="server">Address</asp:HyperLink></li>
                        <li>
                            <asp:HyperLink ID="hrefAppAdditional" runat="server">Additional/Work Experience</asp:HyperLink></li>
                        <%--<li><asp:HyperLink ID="hrefAppFinGuar" runat="server">Financial Guarantor</asp:HyperLink></li>--%>
                        <li>
                            <asp:HyperLink ID="hrefAppAttachment" runat="server">Upload Photo</asp:HyperLink></li>
                    </ol>
                </div>
                <%-- end panel heading --%>
                <div class="panel-body">

                    <asp:UpdatePanel ID="updatePanelParent" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>

                            <div class="row">
                                <div class="col-md-12">

                                    <div class="col-md-6">
                                        <div class="panel panel-default">
                                            <div class="panel-heading style_thead">
                                                Father
                                            </div>
                                            <div class="panel-body panelBody_edu_marginBottom">
                                                <table style="width: 100%" class="table table-condensed">
                                                    <tr>
                                                        <td style="width: 30%" class="style_td">Father's Name <span class="spanAsterisk">*</span></td>
                                                        <td style="width: 70%">
                                                            <asp:TextBox ID="txtFatherName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Late?</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlIsLateFather" runat="server" Width="85%" CssClass="form-control" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddlIsLateFather_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none">
                                                        <td class="style_td">Occupation <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtFatherOccupation" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Occupation Type<span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlFatherOccupationType" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Organization</td>
                                                        <td>
                                                            <asp:TextBox ID="txtFatherOrganization" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Designation</td>
                                                        <td>
                                                            <asp:TextBox ID="txtFatherDesignation" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td class="style_td">Business Address</td>
                                                        <td>
                                                            <asp:TextBox ID="txtFatherOrgAddress" runat="server" Width="100%" CssClass="form-control"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td class="style_td">Mobile <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtFatherMobile" runat="server" Width="100%" CssClass="form-control"
                                                                placeholder="Format: +8801XXXXXXXXX"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Email</td>
                                                        <td>
                                                            <asp:TextBox ID="txtFatherEmail" runat="server" Width="100%" CssClass="form-control"
                                                                TextMode="Email"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">National ID <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtFatherNationalId" runat="server" Width="100%" TextMode="Number" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Nationality <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlFatherNationality" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="panel panel-default">
                                            <div class="panel-heading style_thead">
                                                Mother
                                            </div>
                                            <div class="panel-body panelBody_edu_marginBottom">
                                                <table style="width: 100%" class="table table-condensed">
                                                    <tr>
                                                        <td style="width: 30%" class="style_td">Mother's Name <span class="spanAsterisk">*</span></td>
                                                        <td style="width: 70%">
                                                            <asp:TextBox ID="txtMotherName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Late?</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlIsLateMother" runat="server" Width="85%" CssClass="form-control" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ddlIsLateMother_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none">
                                                        <td class="style_td">Occupation <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtMotherOccupation" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Occupation Type<span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlMotherOccupationType" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Organization</td>
                                                        <td>
                                                            <asp:TextBox ID="txtMotherOrganization" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Designation</td>
                                                        <td>
                                                            <asp:TextBox ID="txtMotherDesignation" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td class="style_td">Mailing Address<br />
                                                            (Postal)</td>
                                                        <td>
                                                            <asp:TextBox ID="txtMotherMailingAddress" runat="server" Width="100%" CssClass="form-control"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td class="style_td">Mobile <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtMotherMobile" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">National ID <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtMotherNationalId" runat="server" CssClass="form-control" TextMode="Number" Width="100%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Nationality <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlMotherNationality" runat="server" CssClass="form-control" Width="85%">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%-- END COL-MD-12 --%>
                            </div>
                            <%-- END ROW   FATHER & MOTHER --%>
                            <%--<div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-6">
                                        <div class="panel panel-default">
                                            <div class="panel-heading style_thead">
                                                Guardian
                                            </div>
                                            <div class="panel-body panelBody_edu_marginBottom">
                                                <asp:UpdatePanel ID="updatePanel_Guardian" runat="server">
                                                    <ContentTemplate>
                                                        <table style="width: 100%" class="table table-condensed">
                                                            
                                                            <tr>
                                                                <td class="style_td">Relationship with the applicant <span class="spanAsterisk">*</span></td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlGuardianRelation" runat="server" Width="85%" CssClass="form-control" AutoPostBack="true"
                                                                        OnSelectedIndexChanged="ddlGuardianRelation_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 30%" class="style_td">Guardian's Name <span class="spanAsterisk">*</span></td>
                                                                <td style="width: 70%">
                                                                    <asp:TextBox ID="txtGuardian_Name" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            
                                                            <tr>
                                                                <td class="style_td">Other</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtGuardianOtherRelation" runat="server" Width="100%" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr style="display: none">
                                                                <td class="style_td">Occupation</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtGuardianOccupation" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style_td">Occupation Type<span class="spanAsterisk">*</span></td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlGuardianOccupationType" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style_td">Organization</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtGuardianOrganization" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style_td">Mailing Address<br />
                                                                    (Postal)</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtGuardianMailingAddress" runat="server" Width="100%" CssClass="form-control"
                                                                        TextMode="MultiLine"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style_td">Email</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtGuardianEmail" runat="server" Width="100%" CssClass="form-control"
                                                                        TextMode="Email"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style_td">Mobile <span class="spanAsterisk">*</span></td>
                                                                <td>
                                                                    <asp:TextBox ID="txtGuardianMobile" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style_td">National ID</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtGuardianNationalId" runat="server" Width="100%" TextMode="Number" CssClass="form-control"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="style_td">Nationality</td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlGuardianNationality" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                    
                                </div>
                            </div>--%>
                            <%-- END ROW  Guardian --%>

                            <asp:Panel ID="messagePanel_Parent" runat="server">
                                <asp:Label ID="lblMessageParent" runat="server" Text=""></asp:Label>
                            </asp:Panel>

                            <asp:Button ID="btnSave_Parent" runat="server" Text="Save"
                                CssClass="btn btn-primary" OnClick="btnSave_Parent_Click"
                                ValidationGroup="parent1" />

                            <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                                CssClass="btn btn-primary" />

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

