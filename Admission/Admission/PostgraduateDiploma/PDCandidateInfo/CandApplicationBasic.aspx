<%@ Page Title="Office Edit - Application Basic" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CandApplicationBasic.aspx.cs" Inherits="Admission.Admission.PostgraduateDiploma.PDCandidateInfo.CandApplicationBasic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../../Content/ApplicationForm.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading panelHeaderHeight">
                    <ol class="breadcrumb">
                        <li class="active"><asp:HyperLink ID="hrefAppBasic" runat="server" Enabled="false">Basic Info</asp:HyperLink></li>
                        <%--<li><asp:HyperLink ID="hrefAppPriority" runat="server">Program Priority</asp:HyperLink></li>--%>
                        <li><asp:HyperLink ID="hrefAppEducation" runat="server">Education</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppRelation" runat="server">Parent/Guardian</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppAddress" runat="server">Address</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppAdditional" runat="server">Additional/Work Experience</asp:HyperLink></li>
                        <%--<li><asp:HyperLink ID="hrefAppFinGuar" runat="server">Financial Guarantor</asp:HyperLink></li>--%>
                        <li><asp:HyperLink ID="hrefAppAttachment" runat="server">Upload Photo</asp:HyperLink></li>
                    </ol>
                </div>
                <%-- end panel heading --%>
                <div class="panel-body">
                    
                    <table style="width: 100%" class="table table-condensed table-striped">
                        <tr>
                            <td colspan="4" class="style_td">
                                <span class="spanAsterisk">*</span> indicate required fields.<br />
                                <%--<span class="spanAsterisk">Please note that there is no validation for Admin in this form.</span>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" class="style_td">Name in FULL <span class="spanAsterisk">*</span></td>
                            <td style="width: 35%" colspan="3">
                                <asp:TextBox ID="txtFirstName" runat="server" Width="50%" CssClass="form-control"></asp:TextBox>
                            </td>
                            <%--<td style="width: 15%" class="style_td style_td_secondCol">Middle Name</td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtMiddleName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>--%>
                        </tr>
                        <%--<tr>
                            <td class="style_td">Last Name</td>
                            <td>
                                <asp:TextBox ID="txtLastName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td class="style_td style_td_secondCol">Nick Name</td>
                            <td>
                                <asp:TextBox ID="txtNickName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="style_td" style="width: 15%">Date Of Birth <span class="spanAsterisk">*</span></td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtDateOfBirth" runat="server" Width="50%" CssClass="form-control" placeholder="dd/MM/yyyy"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalenderExtender_DOB" runat="server"
                                    TargetControlID="txtDateOfBirth" Format="dd/MM/yyyy" />
                            </td>
                            <%--<td class="style_td style_td_secondCol" style="width: 15%">Place of Birth<span class="spanAsterisk">*</span></td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtPlaceOfBirth" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>--%>
                        </tr>
                        <tr>
                            <td class="style_td">Nationality<span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlNationality" runat="server" Width="50%" CssClass="form-control"></asp:DropDownList>
                            </td>
                           <%-- <td class="style_td style_td_secondCol">Mother Tongue<span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlLanguage" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                            </td>--%>
                        </tr>
                        <tr>
                            <td class="style_td">Gender <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlGender" runat="server" Width="50%" CssClass="form-control"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td style_td_secondCol">Marital Status<span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlMaritalStatus" runat="server" Width="50%" CssClass="form-control"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Blood Group <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlBloodGroup" runat="server" Width="50%" CssClass="form-control"></asp:DropDownList>
                            </td>
                            
                        </tr>
                        <tr>
                            <td class="style_td style_td_secondCol">Email <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" Width="50%" TextMode="Email" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Mobile <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:TextBox ID="txtMobile" runat="server" Width="50%" CssClass="form-control" 
                                    placeholer="Format: +8801XXXXXXXXX"></asp:TextBox>
                                <asp:RegularExpressionValidator runat="server" ID="mobileReg"
                                    ValidationGroup="basic1"
                                    ForeColor="Crimson"
                                    ErrorMessage="Invalid format."
                                    ControlToValidate="txtMobile"
                                    ValidationExpression="^(\+88)\d{11}$"></asp:RegularExpressionValidator>
                            </td>
                            
                            <%--<td></td>
                            <td></td>--%>
                        </tr>
                        <tr>
                            <td class="style_td style_td_secondCol">Religion<span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlReligion" runat="server" Width="50%" CssClass="form-control"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>

                    <asp:Panel ID="messagePanel_Basic" runat="server">
                        <asp:Label ID="lblMessageBasic" runat="server" Text=""></asp:Label>
                    </asp:Panel>

                    <asp:Button ID="btnSave_Basic" runat="server" Text="Save"
                        CssClass="btn btn-primary" ValidationGroup="basic1"
                        OnClick="btnSave_Basic_Click" />

                    <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                        CssClass="btn btn-primary" />

                    <span id="validationMsg" class="validationErrorMsg"></span>

                </div>
                <%-- end panel body --%>
            </div>
            <%-- end panel default --%>
        </div>
        <%-- end col-md-12 --%>
    </div>
    <%-- end row --%>

</asp:Content>
