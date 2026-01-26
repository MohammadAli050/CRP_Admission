<%@ Page Title="Report : Candidate Information" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="RptCandidateInfoSelParam.aspx.cs" Inherits="Admission.Admission.Office.Reports.RptCandidateInfoSelParam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading text-center" style="color: cadetblue">
                    <h4><strong>REPORT - Candidate Information</strong></h4>
                </div>
                <div class="panel-body">
                    <table class="table_form table_fullwidth">
                        <tr>
                            <td class="style_td" style="width: 6%">School</td>
                            <td style="width: 26%">
                                <asp:DropDownList ID="ddlAdmissionUnit" runat="server" Width="85%"></asp:DropDownList>
                            </td>
                            <td class=" style_td style_td_secondCol" style="width: 6%">Session</td>
                            <td style="width: 26%">
                                <asp:DropDownList ID="ddlSession" runat="server" Width="85%"></asp:DropDownList>
                            </td>
                            <td class=" style_td style_td_secondCol" style="width: 6%">Sort By</td>
                            <td style="width: 26%">
                                <asp:DropDownList ID="ddlSortBy" runat="server" Width="85%"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table class="table_form table_fullwidth">
                        <tr>
                            <td class="style_td" style="width: 15%">Admission Date Range:</td>
                            <td class="style_td" style="width: 7%">Start Date</td>
                            <td style="width: 20%">
                                <asp:TextBox ID="txtStartDate" runat="server" Width="85%"></asp:TextBox>
                            </td>
                            <td class=" style_td style_td_secondCol" style="width: 7%">End Date</td>
                            <td style="width: 20%">
                                <asp:TextBox ID="txtEndDate" runat="server" Width="85%"></asp:TextBox>
                            </td>
                            <td style="width: 30%"></td>
                        </tr>
                    </table>
                    <div class="row">
                        Select All &nbsp;
                        <asp:CheckBox ID="chbxSelectAll" runat="server" />
                        <table class="table table-condensed table-bordered table-hover" style="margin-bottom: 0.1%;">
                            <%-- BASIC INFO --%>
                            <tr>
                                <td style="width: 6%"><span class="badge">Basic :</span>&nbsp;</td>
                                <td>Mobile
                                    <asp:CheckBox ID="CheckBoxBasic_Mobile" runat="server" />
                                    &nbsp;
                                    Email
                                    <asp:CheckBox ID="CheckBoxBasic_Email" runat="server" />
                                    &nbsp;
                                    DOB
                                    <asp:CheckBox ID="CheckBoxBasic_Dob" runat="server" />
                                    &nbsp;
                                    Place of Birth
                                    <asp:CheckBox ID="CheckBoxBasic_PlaceBirth" runat="server" />
                                    &nbsp;
                                    Nationality
                                    <asp:CheckBox ID="CheckBoxBasic_Nationality" runat="server" />
                                    &nbsp;
                                    National ID
                                    <asp:CheckBox ID="CheckBoxBasic_NationalId" runat="server" />
                                    &nbsp;
                                    Birth Reg.
                                    <asp:CheckBox ID="CheckBoxBasic_BirthReg" runat="server" />
                                    &nbsp;
                                    Marital Status
                                    <asp:CheckBox ID="CheckBoxBasic_MaritalSt" runat="server" />
                                    &nbsp;
                                    Religion
                                    <asp:CheckBox ID="CheckBoxBasic_Religion" runat="server" />
                                    &nbsp;
                                    Blood Group
                                    <asp:CheckBox ID="CheckBoxBasic_BloodGrp" runat="server" />
                                    &nbsp;
                                    Gender
                                    <asp:CheckBox ID="CheckBoxBasic_Gender" runat="server" />
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <%--FORM --%>
                        <table class="table table-condensed table-bordered table-hover" style="margin-bottom: 0.1%;">
                            <tr>
                                <td style="width: 1%"><span class="badge">Form :</span></td>
                                <td>Form Serial
                                    <asp:CheckBox ID="CheckBoxForm_FormSl" runat="server" />
                                    &nbsp;
                                    Payment ID
                                    <asp:CheckBox ID="CheckBoxForm_PaymentID" runat="server" />
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <%-- EDUCATION SSC --%>
                        <table class="table table-condensed table-bordered table-hover" style="margin-bottom: 0.1%;">
                            <tr>
                                <td style="width: 13%"><span class="badge">SSC/O-Level/Dakhil :</span>&nbsp;</td>
                                <td>Type
                                    <asp:CheckBox ID="CheckBoxSSC_Type" runat="server" />
                                    &nbsp;
                                    Board
                                    <asp:CheckBox ID="CheckBoxSSC_Board" runat="server" />
                                    &nbsp;
                                    Institute
                                    <asp:CheckBox ID="CheckBoxSSC_Institute" runat="server" />
                                    &nbsp;
                                    Roll
                                    <asp:CheckBox ID="CheckBoxSSC_Roll" runat="server" />
                                    &nbsp;
                                    Registraion No.
                                    <asp:CheckBox ID="CheckBoxSSC_RegNo" runat="server" />
                                    &nbsp;
                                    Group or Subject
                                    <asp:CheckBox ID="CheckBoxSSC_GrpSub" runat="server" />
                                    &nbsp;
                                    Division
                                    <asp:CheckBox ID="CheckBoxSSC_ResultDiv" runat="server" />
                                    &nbsp;
                                    GPA/CGPA
                                    <asp:CheckBox ID="CheckBoxSSC_GPA" runat="server" />
                                    &nbsp;
                                    Marks
                                    <asp:CheckBox ID="CheckBoxSSC_Marks" runat="server" />
                                    &nbsp;
                                    Passing Year
                                    <asp:CheckBox ID="CheckBoxSSC_PassingYear" runat="server" />
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <%-- EDUCATION HSC --%>
                        <table class="table table-condensed table-bordered table-hover" style="margin-bottom: 0.1%;">
                            <tr>
                                <td style="width: 16%"><span class="badge">HSC/A-Level/Alim/Diploma :</span>&nbsp;</td>
                                <td>Type
                                    <asp:CheckBox ID="CheckBoxHSC_Type" runat="server" />
                                    &nbsp;
                                    Board
                                    <asp:CheckBox ID="CheckBoxHSC_Board" runat="server" />
                                    &nbsp;
                                    Institute
                                    <asp:CheckBox ID="CheckBoxHSC_Institute" runat="server" />
                                    &nbsp;
                                    Roll
                                    <asp:CheckBox ID="CheckBoxHSC_Roll" runat="server" />
                                    &nbsp;
                                    Registraion No.
                                    <asp:CheckBox ID="CheckBoxHSC_RegNo" runat="server" />
                                    &nbsp;
                                    Group or Subject
                                    <asp:CheckBox ID="CheckBoxHSC_GrpSub" runat="server" />
                                    &nbsp;
                                    Division
                                    <asp:CheckBox ID="CheckBoxHSC_ResultDiv" runat="server" />
                                    &nbsp;
                                    GPA/CGPA
                                    <asp:CheckBox ID="CheckBoxHSC_GPA" runat="server" />
                                    &nbsp;
                                    Marks
                                    <asp:CheckBox ID="CheckBoxHSC_Marks" runat="server" />
                                    &nbsp;
                                    Passing Year
                                    <asp:CheckBox ID="CheckBoxHSC_PassingYear" runat="server" />
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <%-- EDUCATION UNDERGRAD --%>
                        <table class="table table-condensed table-bordered table-hover" style="margin-bottom: 0.1%;">
                            <tr>
                                <td style="width: 1%"><span class="badge">Undergrad :</span></td>
                                <td>Type
                                    <asp:CheckBox ID="CheckBoxUndGrd_Type" runat="server" />
                                    &nbsp;
                                    Institute
                                    <asp:CheckBox ID="CheckBoxUndGrd_Institute" runat="server" />
                                    &nbsp;
                                    Program
                                    <asp:CheckBox ID="CheckBoxUndGrd_Program" runat="server" />
                                    &nbsp;
                                    Roll
                                    <asp:CheckBox ID="CheckBoxUndGrd_Roll" runat="server" />
                                    &nbsp;
                                    Group or Subject
                                    <asp:CheckBox ID="CheckBoxUndGrd_GrpSub" runat="server" />
                                    &nbsp;
                                    GPA/CGPA
                                    <asp:CheckBox ID="CheckBoxUndGrd_GPA" runat="server" />
                                    &nbsp;
                                    Grade
                                    <asp:CheckBox ID="CheckBoxUndGrd_Grade" runat="server" />
                                    &nbsp;
                                    Marks
                                    <asp:CheckBox ID="CheckBoxUndGrd_Marks" runat="server" />
                                    &nbsp;
                                    Passing Year
                                    <asp:CheckBox ID="CheckBoxUndGrd_PassingYear" runat="server" />
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <%-- EDUCATION POSTGRAD --%>
                        <table class="table table-condensed table-bordered table-hover" style="margin-bottom: 0.1%;">
                            <tr>
                                <td style="width: 1%"><span class="badge">Masters :</span></td>
                                <td>Type
                                    <asp:CheckBox ID="CheckBoxMasters_Type" runat="server" />
                                    &nbsp;
                                    Institute
                                    <asp:CheckBox ID="CheckBoxMasters_Institute" runat="server" />
                                    &nbsp;
                                    Program
                                    <asp:CheckBox ID="CheckBoxMasters_Program" runat="server" />
                                    &nbsp;
                                    Roll
                                    <asp:CheckBox ID="CheckBoxMasters_Roll" runat="server" />
                                    &nbsp;
                                    Group or Subject
                                    <asp:CheckBox ID="CheckBoxMasters_GrpSub" runat="server" />
                                    &nbsp;
                                    GPA/CGPA
                                    <asp:CheckBox ID="CheckBoxMasters_GPA" runat="server" />
                                    &nbsp;
                                    Grade
                                    <asp:CheckBox ID="CheckBoxMasters_Grade" runat="server" />
                                    &nbsp;
                                    Marks
                                    <asp:CheckBox ID="CheckBoxMasters_Marks" runat="server" />
                                    &nbsp;
                                    Passing Year
                                    <asp:CheckBox ID="CheckBoxMasters_PassingYear" runat="server" />
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <%-- RELATION --%>
                        <table class="table table-condensed table-bordered table-hover" style="margin-bottom: 0.1%;">
                            <tr>
                                <td style="width: 1%" rowspan="4"><span class="badge">Parent/Guardian :</span>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Father:</strong> Name
                                    <asp:CheckBox ID="CheckBoxFather_Name" runat="server" />
                                    &nbsp;
                                    Occupation
                                    <asp:CheckBox ID="CheckBoxFather_Occupation" runat="server" />
                                    &nbsp;
                                    Mobile
                                    <asp:CheckBox ID="CheckBoxFather_Mobile" runat="server" />
                                    &nbsp;&nbsp;&nbsp;
                                    <strong>Mother:</strong> Name
                                    <asp:CheckBox ID="CheckBoxMother_Name" runat="server" />
                                    &nbsp;
                                    Occupation
                                    <asp:CheckBox ID="CheckBoxMother_Occupation" runat="server" />
                                    &nbsp;
                                    Mobile
                                    <asp:CheckBox ID="CheckBoxMother_Mobile" runat="server" />
                                    &nbsp;
                                </td>
                            </tr>
                            <%--<tr>
                                <td>
                                    <strong>Mother:</strong> Name
                                    <asp:CheckBox ID="CheckBoxMother_Name" runat="server" />
                                    &nbsp;
                                    Occupation
                                    <asp:CheckBox ID="CheckBoxMother_Occupation" runat="server" />
                                    &nbsp;
                                    Mobile
                                    <asp:CheckBox ID="CheckBoxMother_Mobile" runat="server" />
                                    &nbsp;
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    <strong>Guardian:</strong> Name
                                    <asp:CheckBox ID="CheckBoxGuardian_Name" runat="server" />
                                    &nbsp;
                                    Relation
                                    <asp:CheckBox ID="CheckBoxGuardian_Relation" runat="server" />
                                    &nbsp;
                                    Occupation
                                    <asp:CheckBox ID="CheckBoxGuardian_Occupation" runat="server" />
                                    &nbsp;
                                    Business Address
                                    <asp:CheckBox ID="CheckBoxGuardian_Address" runat="server" />
                                    &nbsp;
                                    Mobile
                                    <asp:CheckBox ID="CheckBoxGuardian_Mobile" runat="server" />
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <%--OTHERS --%>
                        <table class="table table-condensed table-bordered table-hover" style="margin-bottom: 0.1%;">
                            <tr>
                                <td style="width: 1%"><span class="badge">Others :</span></td>
                                <td>Work Experience
                                    <asp:CheckBox ID="CheckBoxWorkExp" runat="server" />
                                    &nbsp;
                                    Extracurricular Activities
                                    <asp:CheckBox ID="CheckBoxExtraCurrAct" runat="server" />
                                    &nbsp;
                                    Photo
                                    <asp:CheckBox ID="CheckBoxPhoto" runat="server" />
                                    &nbsp;
                                    Signature
                                    <asp:CheckBox ID="CheckBoxSignature" runat="server" />
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <%-- END PANEL-DEFAULT --%>
        </div>
        <%-- END COL-MD-12 --%>
    </div>
    <%-- END ROW --%>
</asp:Content>
