<%@ Page Title="Review Application" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReviewApplication.aspx.cs" Inherits="Admission.Admission.Candidate.ReviewApplication" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../Content/formStyle.css" rel="stylesheet" />
    <style>
        :root {
            --primary: #091B3F;
            --secondary: #1E3A8A;
            --accent: #3B82F6;
            --accent-light: #93C5FD;
            --success: #059669;
            --warning: #D97706;
            --danger: #DC2626;
            --light: #F9FAFB;
            --dark: #111827;
            --gray: #6B7280;
            --bg-gradient: linear-gradient(145deg, var(--primary), var(--secondary));
            --transition: all 0.3s cubic-bezier(0.25, 0.8, 0.25, 1);
            --shadow-sm: 0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24);
            --shadow-md: 0 4px 6px rgba(0,0,0,0.1);
            --shadow-lg: 0 10px 25px rgba(0,0,0,0.1);
            --radius-sm: 4px;
            --radius-md: 8px;
            --radius-lg: 16px;
        }

        .main-container {
            flex: 1;
            padding: 2rem 0;
            position: relative;
        }

        .content-card {
            background-color: white;
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-md);
            padding: 2rem;
            transition: var(--transition);
        }

            .content-card:hover {
                box-shadow: var(--shadow-lg);
                transform: translateY(-5px);
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-4">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Candidate Photo</strong>
                </div>
                <div class="panel-body">
                    <div style="text-align: center">
                        <asp:Image ID="ImagePhoto" runat="server"
                            ImageUrl="~/Images/AppImg/user7.jpg"
                            Width="154" Height="154" />
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Candidate Signature</strong>
                </div>
                <div class="panel-body">
                    <div style="text-align: center">
                        <div style="text-align: center">
                            <asp:Image ID="ImageSignature" runat="server"
                                ImageUrl="~/Images/AppImg/sign2.png"
                                Width="256" Height="154" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-4" style="display: none">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Financial Guarantor's Signature</strong>
                </div>
                <div class="panel-body">
                    <div style="text-align: center">
                        <div style="text-align: center">
                            <asp:Image ID="FGSignature" runat="server"
                                ImageUrl="~/Images/AppImg/sign2.png"
                                Width="256" Height="154" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- END PHOTO SIGNATURE ROW --%>
    <%-- -------------------------------------------------------------------------------------------------------------- --%>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Basic Information</strong>
                </div>
                <div class="panel-body">
                    <table class="table table_fullwidth table-bordered table-condensed">
                        <%--<tr>
                            <td colspan="4" class="style_td">
                                <span class="spanAsterisk">*</span> indicate required fields.
                            </td>
                        </tr>--%>
                        <tr>
                            <td style="width: 15%" class="style_td">Name in FULL <span class="spanAsterisk">*</span></td>
                            <td style="width: 35%" colspan="3">
                                <asp:Label ID="lblFirstName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <%--<tr>
                            <td class="style_td">Last Name</td>
                            <td>
                                <asp:TextBox ID="lblLastName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td class="style_td style_td_secondCol">Nick Name</td>
                            <td>
                                <asp:TextBox ID="lblNickName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="style_td" style="width: 15%">Date Of Birth <span class="spanAsterisk">*</span></td>
                            <td style="width: 35%">
                                <asp:Label ID="lblDateOfBirth" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <%--                        <tr>
                            <td class="style_td">Nationality<span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblNationality" runat="server"></asp:Label>
                            </td>
                            <td class="style_td style_td_secondCol">Mother Tongue<span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblLanguage" runat="server"></asp:Label>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="style_td">Gender <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblGender" runat="server"></asp:Label>
                            </td>

                            <td class="style_td style_td_secondCol" id="tdMarital" runat="server" visible="false">Marital Status<span class="spanAsterisk">*</span></td>
                            <td id="tdMaritallbl" runat="server" visible="false">
                                <asp:Label ID="lblMaritalStatus" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">National ID No.</td>
                            <td>
                                <asp:Label ID="lblNationalId" runat="server"></asp:Label>
                            </td>
                            <td class="style_td style_td_secondCol">Birth Registration No.</td>
                            <td>
                                <asp:Label ID="lblBirthRegNo" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Blood Group <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblBloodGroup" runat="server"></asp:Label>
                            </td>
                            <td class="style_td style_td_secondCol">Email <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblEmail" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <%--<td class="style_td style_td_secondCol">Phone<br />
                                (Res)</td>
                            <td>
                                <asp:TextBox ID="lblPhoneRes" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Emergency Phone</td>
                            <td>
                                <asp:TextBox ID="lblPhoneEmergency" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>--%>
                        <tr>
                            <td class="style_td">Mobile <span class="spanAsterisk">*</span><br />
                                <span style="color: cornflowerblue; font-weight: normal; font-size: smaller">Format: +8801XXXXXXXXX</span>
                            </td>
                            <td>
                                <asp:Label ID="lblMobile" runat="server"></asp:Label>
                            </td>
                            <td class="style_td style_td_secondCol">Religion<span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblReligion" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <%--<td class="style_td">Quota</td>
                            <td>
                                <asp:Label ID="lblQuota" runat="server"></asp:Label>
                            </td>--%>
                            <asp:PlaceHolder ID="phHall" runat="server">
                                <td class="style_td">Hall accomodation</td>
                                <td>
                                    <asp:Label ID="lblHall" runat="server"></asp:Label>
                                </td>
                            </asp:PlaceHolder>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <%-- END BASIC INFO ROW --%>
    <%-- -------------------------------------------------------------------------------------------------------------- --%>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>SSC/O-Level/Dakhil Information</strong>
                </div>
                <div class="panel-body">
                    <table style="width: 100%" class="table table-condensed table-bordered">
                        <tr>
                            <td style="width: 15%" class="style_td">Exam Type <span class="spanAsterisk">*</span></td>
                            <td style="width: 35%">
                                <asp:Label ID="lblSec_ExamType" runat="server"></asp:Label>
                            </td>
                            <td class="style_td">Education Board <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblSec_EducationBrd" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" class="style_td">Institute <span class="spanAsterisk">*</span></td>
                            <td colspan="3">
                                <asp:Label ID="lblSec_Institute" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" class="style_td">Roll Number <span class="spanAsterisk">*</span></td>
                            <td style="width: 35%">
                                <asp:Label ID="lblSec_RollNo" runat="server"></asp:Label>
                            </td>
                            <td style="width: 15%" class="style_td">Registration Number</td>
                            <td style="width: 35%">
                                <asp:Label ID="lblSec_RegNo" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Group Or Subject <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblSec_GrpOrSub" runat="server"></asp:Label>
                            </td>
                            <td class="style_td">Exam Year <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblSec_PassingYear" runat="server"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td class="style_td">Division/Class <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblSec_DivClass" runat="server"></asp:Label>
                            </td>
                            <td class="style_td">GPA
                            </td>
                            <td>
                                <asp:Label ID="lblSec_GPA" runat="server"></asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td class="style_td">Biology GPA <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblSSCBiologyGPA" runat="server"></asp:Label>
                            </td>
                            <td class="style_td">Student Category
                            </td>
                            <td>
                                <asp:Label ID="lblsscStudentCategory" runat="server"></asp:Label>
                            </td>
                        </tr>

                    </table>
                </div>
            </div>
            <%-- ---------------------------------------------------------------------------------- --%>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>HSC/A-Level/Alim/Diploma Information</strong>
                </div>
                <div class="panel-body">
                    <table style="width: 100%" class="table table-condensed table-bordered">
                        <tr>
                            <td style="width: 15%" class="style_td">Exam Type <span class="spanAsterisk">*</span></td>
                            <td style="width: 35%">
                                <asp:Label ID="lblHighSec_ExamType" runat="server"></asp:Label>
                            </td>
                            <td class="style_td">Education Board <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblHighSec_EducationBrd" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" class="style_td">Institute <span class="spanAsterisk">*</span></td>
                            <td colspan="3">
                                <asp:Label ID="lblHighSec_Institute" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" class="style_td">Roll Number <span class="spanAsterisk">*</span></td>
                            <td style="width: 35%">
                                <asp:Label ID="lblHighSec_RollNo" runat="server"></asp:Label>
                            </td>
                            <td style="width: 15%" class="style_td">Registration Number</td>
                            <td style="width: 35%">
                                <asp:Label ID="lblHighSec_RegNo" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Group Or Subject <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblHighSec_GrpOrSub" runat="server"></asp:Label>
                            </td>
                            <td class="style_td">Exam Year <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblHighSec_PassingYear" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Division/Class <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblHighSec_DivClass" runat="server"></asp:Label>
                            </td>
                            <td class="style_td">GPA
                            </td>
                            <td>
                                <asp:Label ID="lblHighSec_GPA" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Biology GPA <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblHSCBiologyGPA" runat="server"></asp:Label>
                            </td>
                            <td class="style_td">Student Category
                            </td>
                            <td>
                                <asp:Label ID="lblHSCStudentCategory" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <%-- ---------------------------------------------------------------------------------- --%>
            <asp:Panel ID="panel_ForMasters" runat="server">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <strong>Undergraduate/Bachelors Information</strong>
                    </div>
                    <div class="panel-body">
                        <table style="width: 100%" class="table table-condensed table-bordered">
                            <tr>
                                <td style="width: 15%" class="style_td">Institute <span class="spanAsterisk">*</span></td>
                                <td colspan="3">
                                    <asp:Label ID="lblUndergrad_Institute" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="style_td">Program <span class="spanAsterisk">*</span></td>
                                <td style="width: 35%">
                                    <asp:Label ID="lblUndergrad_Program" runat="server"></asp:Label>
                                </td>
                                <td style="width: 15%" class="style_td">Other Program</td>
                                <td style="width: 35%">
                                    <asp:Label ID="lblUndergrad_ProgramOther" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <%--<td class="style_td">Group Or Subject <span class="spanAsterisk">*</span></td>
                                <td>
                                    <asp:Label ID="lblUndergrad_GrpOrSub" runat="server"></asp:Label>
                                </td>--%>
                                <td class="style_td">Division/Class <span class="spanAsterisk">*</span></td>
                                <td>
                                    <asp:Label ID="lblUndergrad_DivClass" runat="server"></asp:Label>
                                </td>

                                <td class="style_td">CGPA/Score <span class="spanAsterisk">*</span></td>
                                <td>
                                    <asp:Label ID="lblUndergrad_CgpaScore" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="style_td">Grade <span class="spanAsterisk">*</span></td>
                                <td>
                                    <asp:Label ID="lblUndergrad_Grade" runat="server"></asp:Label>
                                </td>
                                <td class="style_td">Exam Year <span class="spanAsterisk">*</span></td>
                                <td>
                                    <asp:Label ID="lblUndergrad_PassingYear" runat="server"></asp:Label>
                                </td>

                            </tr>
                        </table>
                    </div>
                </div>
                <%-- ---------------------------------------------------------- --%>
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <strong>Postgraduate/Masters Information</strong>
                    </div>
                    <div class="panel-body">
                        <table style="width: 100%" class="table table-condensed table-bordered">
                            <tr>
                                <td style="width: 15%" class="style_td">Institute <span class="spanAsterisk">*</span></td>
                                <td colspan="3">
                                    <asp:Label ID="lblGrad_Institute" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 15%" class="style_td">Program <span class="spanAsterisk">*</span></td>
                                <td style="width: 35%">
                                    <asp:Label ID="lblGrad_Program" runat="server"></asp:Label>
                                </td>
                                <td style="width: 15%" class="style_td">Other Program</td>
                                <td style="width: 35%">
                                    <asp:Label ID="lblGrad_ProgramOther" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <%--<td class="style_td">Group Or Subject <span class="spanAsterisk">*</span></td>
                                <td>
                                    <asp:Label ID="lblGrad_GrpOrSub" runat="server"></asp:Label>
                                </td>--%>
                                <td class="style_td">Division/Class <span class="spanAsterisk">*</span></td>
                                <td>
                                    <asp:Label ID="lblGrad_DivClass" runat="server"></asp:Label>
                                </td>

                                <td class="style_td">CGPA/Score <span class="spanAsterisk">*</span></td>
                                <td>
                                    <asp:Label ID="lblGrad_CgpaScore" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="style_td">Grade <span class="spanAsterisk">*</span></td>
                                <td>
                                    <asp:Label ID="lblGrad_Grade" runat="server"></asp:Label>
                                </td>
                                <td class="style_td">Exam Year <span class="spanAsterisk">*</span></td>
                                <td>
                                    <asp:Label ID="lblGrad_PassingYear" runat="server"></asp:Label>
                                </td>

                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
    <%-- END EDUCATION INFO ROW --%>
    <%-- -------------------------------------------------------------------------------------------------------------- --%>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Present Address</strong>
                </div>
                <div class="panel-body">
                    <table style="width: 100%" class="table table-condensed table-bordered">
                        <tr>
                            <td style="width: 15%" class="style_td">Address</td>
                            <td style="width: 35%">
                                <asp:Label ID="lblPresentAddress" runat="server"></asp:Label>
                            </td>
                            <td style="width: 15%" class="style_td">Division</td>
                            <td style="width: 35%">
                                <asp:Label ID="lblPresentDivision" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">District</td>
                            <td>
                                <asp:Label ID="lblPresentDistrict" runat="server"></asp:Label>
                            </td>
                            <td class="style_td">Upozela/Thana</td>
                            <td>
                                <asp:Label ID="lblPresentUpozela" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Postal Code</td>
                            <td>
                                <asp:Label ID="lblPresentPostalCode" runat="server"></asp:Label>
                            </td>
                            <td class="style_td">Country</td>
                            <td>
                                <asp:Label ID="lblPresentCountry" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <%--<tr>
                        <td class="style_td">Telephone</td>
                        <td colspan="3">
                            <asp:Label ID="lblPresentTelephone" runat="server"></asp:Label>
                        </td>
                    </tr>--%>
                    </table>
                </div>
            </div>
            <%-- -------------------------------------------------------------- --%>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Permanent Address</strong>
                </div>
                <div class="panel-body">
                    <table style="width: 100%" class="table table-condensed table-bordered">
                        <tr>
                            <td style="width: 15%" class="style_td">Address</td>
                            <td style="width: 35%">
                                <asp:Label ID="lblPermanentAddress" runat="server"></asp:Label>
                            </td>
                            <td style="width: 15%" class="style_td">Division</td>
                            <td style="width: 35%">
                                <asp:Label ID="lblPermanentDivision" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">District</td>
                            <td>
                                <asp:Label ID="lblPermanentDistrict" runat="server"></asp:Label>
                            </td>
                            <td class="style_td">Upozela/Thana</td>
                            <td>
                                <asp:Label ID="lblPermanentUpozela" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Postal Code</td>
                            <td>
                                <asp:Label ID="lblPermanentPostalCode" runat="server"></asp:Label>
                            </td>
                            <td class="style_td">Country</td>
                            <td>
                                <asp:Label ID="lblPermanentCountry" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <%-- <tr>
                        <td class="style_td">Telephone</td>
                        <td colspan="3">
                            <asp:Label ID="lblPermanentTelephone" runat="server"></asp:Label>
                        </td>
                    </tr>--%>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <%-- END ADDRESS ROW --%>
    <%-- -------------------------------------------------------------------------------------------------------------- --%>

    <asp:Panel ID="panel_ProgramPriority" runat="server">
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <strong>Program Priority</strong>
                    </div>
                    <div class="panel-body">
                        <asp:Repeater ID="rptProgramPriority" runat="server">
                            <HeaderTemplate>
                                <table style="width: 100%" class="table table-condensed table-bordered">
                                    <thead>
                                        <tr class="active">
                                            <th class="text-center">Priority</th>
                                            <th class="text-center">Program Name</th>
                                            <th class="text-center">Faculty</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="text-center" style="width: 15%">
                                        <%# Eval("cP_Priority") %>
                                    </td>
                                    <td style="width: 35%">
                                        <%# Eval("admUnitProgZ_ProgramName") %>
                                    </td>
                                    <td style="width: 25%">
                                        <%# Eval("admUnit_Name") %>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                        </table>
                   
                            </FooterTemplate>
                        </asp:Repeater>
                        <asp:Label ID="lblNoData" runat="server" Text="No program priorities found."
                            Visible="false" CssClass="text-center"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Parent/Guardian Information</strong>
                </div>
                <div class="panel-body">
                    <table style="width: 100%" class="table table-condensed table-bordered">
                        <tr>
                            <td colspan="4" class="active text-center">
                                <strong>Father</strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" class="style_td">Father's Name <span class="spanAsterisk">*</span></td>
                            <td style="width: 35%">
                                <asp:Label ID="lblFatherName" runat="server"></asp:Label>
                            </td>
                            <td style="width: 15%" class="style_td">Occupation</td>
                            <td style="width: 35%">
                                <asp:Label ID="lblFatherOccupation" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <%--<tr>
                            <td class="style_td">Business Address</td>
                            <td>
                                <asp:Label ID="lblFatherOrgAddress" runat="server" 
                                    TextMode="MultiLine"></asp:Label>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="style_td">Mobile</td>
                            <td>
                                <asp:Label ID="lblFatherMobile" runat="server"></asp:Label>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>

                    <table style="width: 100%" class="table table-condensed table-bordered">
                        <tr>
                            <td colspan="4" class="active text-center">
                                <strong>Mother</strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" class="style_td">Mother's Name <span class="spanAsterisk">*</span></td>
                            <td style="width: 35%">
                                <asp:Label ID="lblMotherName" runat="server"></asp:Label>
                            </td>
                            <td class="style_td" style="width: 15%">Occupation</td>
                            <td style="width: 35%">
                                <asp:Label ID="lblMotherOccupation" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <%--<tr>
                            <td class="style_td">Mailing Address<br />
                                (Postal)</td>
                            <td>
                                <asp:Label ID="lblMotherMailingAddress" runat="server" 
                                    TextMode="MultiLine"></asp:Label>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="style_td">Mobile</td>
                            <td>
                                <asp:Label ID="lblMotherMobile" runat="server"></asp:Label>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>

                    <table style="width: 100%" class="table table-condensed table-bordered">
                        <tr>
                            <td colspan="4" class="active text-center">
                                <strong>Guardian</strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" class="style_td">Guardian's Name <span class="spanAsterisk">*</span></td>
                            <td style="width: 35%">
                                <asp:Label ID="lblGuardian_Name" runat="server"></asp:Label>
                            </td>

                            <td class="style_td">Other</td>
                            <td>
                                <asp:Label ID="lblGuardianOtherRelation" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Occupation <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblGuardianOccupation" runat="server"></asp:Label>
                            </td>

                            <td class="style_td">Business Address <span class="spanAsterisk">*</span><br />
                                (Postal)</td>
                            <td>
                                <asp:Label ID="lblGuardianMailingAddress" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Mobile <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:Label ID="lblGuardianMobile" runat="server"></asp:Label>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <%--<tr>
                            <td class="style_td">Email</td>
                            <td>
                                <asp:Label ID="lblGuardianEmail" runat="server" Width="100%" CssClass="form-control"
                                    TextMode="Email"></asp:Label>
                            </td>
                        </tr>--%>
                        <%--<tr>
                            <td class="style_td">Phone(Res)</td>
                            <td>
                                <asp:Label ID="lblGuardianPhoneRes" runat="server" Width="100%" CssClass="form-control"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Phone(Office)</td>
                            <td>
                                <asp:Label ID="lblGuardianPhoneOffice" runat="server" Width="100%" CssClass="form-control"></asp:Label>
                            </td>
                        </tr>--%>
                    </table>

                    <table style="width: 100%; display: none" class="table table-condensed table-bordered">
                        <tr>
                            <td colspan="4" class="active text-center">
                                <strong>Spouse</strong>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" class="style_td"></td>
                            <td style="width: 35%">
                                <asp:Label ID="Label1" runat="server"></asp:Label>
                            </td>
                            <td style="width: 15%" class="style_td"></td>
                            <td style="width: 35%">
                                <asp:Label ID="Label2" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <%--<tr>
                            <td class="style_td">Business Address</td>
                            <td>
                                <asp:Label ID="lblFatherOrgAddress" runat="server" 
                                    TextMode="MultiLine"></asp:Label>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="style_td"></td>
                            <td>
                                <asp:Label ID="Label3" runat="server"></asp:Label>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>

                </div>
            </div>

        </div>
    </div>
    <%-- END PARENT/GUARDIAN/SPOUCE ROW --%>
    <%-- -------------------------------------------------------------------------------------------------------------- --%>

    <%--    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Financial Guarantor Information</strong>
                </div>
                <div class="panel-body">
                    <table style="width: 100%" class="table table-condensed table-bordered">
                        <tr>
                            <td style="width: 15%" class="style_td">Name (Full Name)</td>
                            <td style="width: 35%">
                                <asp:Label ID="lblFinGuarantorName" runat="server"></asp:Label>
                            </td>
                            <td style="width: 15%" class="style_td">Relationship with the applicant</td>
                            <td style="width: 35%">
                                <asp:Label ID="lblRelationWithGuarantor" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Occupation</td>
                            <td>
                                <asp:Label ID="lblFinGuarantorOccupation" runat="server"></asp:Label>
                            </td>
                            <td class="style_td">Organization</td>
                            <td>
                                <asp:Label ID="lblFinGuarantorOrganization" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Position</td>
                            <td>
                                <asp:Label ID="lblFinGuarantorPosition" runat="server"></asp:Label>
                            </td>

                            <td class="style_td">
                            Mailing Address<td>
                                <asp:Label ID="lblFinGuarantorAddress" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Email (if any)</td>
                            <td>
                                <asp:Label ID="lblFinGuarantorEmail" runat="server"></asp:Label>
                            </td>

                            <td class="style_td">Mobile</td>
                            <td>
                                <asp:Label ID="lblFinGuarantorMobile" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Source of Fund</td>
                            <td>
                                <asp:Label ID="lblFinGuarantorSourceFund" runat="server"></asp:Label>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>--%>
    <%-- END FINANCIAL GUARANTOR ROW --%>
    <%-- -------------------------------------------------------------------------------------------------------------- --%>
</asp:Content>
