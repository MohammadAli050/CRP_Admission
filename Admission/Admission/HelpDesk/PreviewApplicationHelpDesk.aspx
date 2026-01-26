<%@ Page Title="Preview Application" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="PreviewApplicationHelpDesk.aspx.cs" Inherits="Admission.Admission.HelpDesk.PreviewApplicationHelpDesk" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Search using Mobile number or Payment ID</strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanelFilterSSL" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:Panel ID="messagePanelFilter" runat="server">
                                <asp:Label ID="lblMessageFilter" runat="server" Text=""></asp:Label>
                            </asp:Panel>

                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td style="width: 15%">Mobile No. / Payment ID:</td>
                                    <td style="width: 50%">
                                        <asp:TextBox ID="txtSearch" runat="server" Width="100%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="txtSearchREV" runat="server"
                                            ControlToValidate="txtSearch" ErrorMessage="Required" Font-Size="Small"
                                            ForeColor="Crimson" ValidationGroup="gr1" Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td style="width: 35%">
                                        <asp:Button ID="btnLoad" runat="server" Text="Search" ValidationGroup="gr1"
                                            OnClick="btnLoad_Click" />
                                    </td>
                                </tr>
                            </table>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%-- BASIC INFO --%>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>BASIC INFO</strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanel" runat="server" UpdateMode="Always">
                        <ContentTemplate>

                            <asp:Panel ID="messagePanelBasic" runat="server">
                                <asp:Label ID="lblMessageBasic" runat="server" Text=""></asp:Label>
                            </asp:Panel>

                            <asp:GridView ID="gvBasicInfo" runat="server"
                                CssClass="table table-responsive table-hover table-bordered"
                                AutoGenerateColumns="false" Width="100%" ShowHeader="false"
                                OnRowDataBound="gvBasicInfo_RowDataBound">
                                <%--<HeaderStyle BackColor="#1387de" ForeColor="White" Font-Size="Smaller" />--%>
                                <Columns>
                                    <asp:BoundField DataField="FirstName" ItemStyle-Font-Bold="true" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <strong>Email:</strong>
                                            <asp:Label runat="server" ID="lblEmail" Text='<%#Eval("Email") %>'></asp:Label><br />
                                            <strong>SMS Phone:</strong>
                                            <asp:Label runat="server" ID="lblSmsPhone" Text='<%#Eval("SMSPhone") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Small" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <strong>DOB:</strong>
                                            <asp:Label runat="server" ID="lblDOB" Text='<%#Eval("DateOfBirth") %>'></asp:Label><br />
                                            <strong>Place Of Birth:</strong>
                                            <asp:Label runat="server" ID="lblPlaceOfBirth" Text='<%#Eval("PlaceOfBirth") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Small" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <strong>Nationality:</strong>
                                            <asp:Label runat="server" ID="lblNationality" Text='<%#Eval("CountryName") %>'></asp:Label><br />
                                            <strong>National ID:</strong>
                                            <asp:Label runat="server" ID="lblNationalId" Text='<%#Eval("NationalIdNumber") %>'></asp:Label><br />
                                            <strong>Mother Tongue:</strong>
                                            <asp:Label runat="server" ID="lblMotherTongue" Text='<%#Eval("MotherTongueLang") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Small" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <strong>Religion:</strong>
                                            <asp:Label runat="server" ID="lblReligion" Text='<%#Eval("ReligionName") %>'></asp:Label><br />
                                            <strong>Quota:</strong>
                                            <asp:Label runat="server" ID="lblQuota" Text='<%#Eval("QuotaName") %>'></asp:Label><br />
                                            <strong>Blood Group:</strong>
                                            <asp:Label runat="server" ID="lblBloodGroup" Text='<%#Eval("BloodGroupName") %>'></asp:Label><br />
                                            <strong>Gender:</strong>
                                            <asp:Label runat="server" ID="lblGender" Text='<%#Eval("GenderName") %>'></asp:Label><br />
                                        </ItemTemplate>
                                        <ItemStyle Font-Size="Small" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="alert alert-warning" role="alert" style="text-align: center">⚠️ Candidate Basic Info Not Found ❗</div>
                                </EmptyDataTemplate>
                            </asp:GridView>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%-- DOCUMENTS --%>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>PHOTO & SIGNATURE</strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanelPhotoSign" runat="server" UpdateMode="Always">
                        <ContentTemplate>

                            <asp:Panel ID="messagePanelImage" runat="server">
                                <asp:Label ID="lblMessageImage" runat="server" Text=""></asp:Label>
                            </asp:Panel>

                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 25%"></td>
                                    <td style="width: 25%">
                                        <img runat="server" id="imgCtrl" src="" height="155" width="145" alt="Photo not uploaded."
                                            style="font-size: x-small; color: crimson; text-align: justify;" />
                                    </td>
                                    <td style="width: 25%">
                                        <img runat="server" id="signCtrl" src="" height="155" width="145" alt="Signature not uploaded."
                                            style="font-size: x-small; color: crimson; text-align: justify;" />
                                    </td>
                                    <td style="width: 25%"></td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%-- FORM SERIAL / PAYMENT --%>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>FORM SERIAL & PAYMENT</strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanelFormPay" runat="server" UpdateMode="Always">
                        <ContentTemplate>

                            <asp:Panel ID="messagePanelFormPay" runat="server">
                                <asp:Label ID="lblMessageFormPay" runat="server" Text=""></asp:Label>
                            </asp:Panel>

                            <asp:GridView ID="gvFormPay" runat="server"
                                CssClass="table table-responsive table-hover table-bordered"
                                AutoGenerateColumns="false" Width="100%" ShowHeader="true" HeaderStyle-Font-Bold="true"
                                OnRowDataBound="gvFormPay_RowDataBound">
                                <HeaderStyle BackColor="#1387de" ForeColor="White" />
                                <Columns>
                                    <asp:BoundField HeaderText="Form Serial" DataField="CFormFormSerial"/>
                                    <asp:BoundField HeaderText="PaymentID" DataField="CPaymentPaymentId" ItemStyle-ForeColor="#000066" ItemStyle-Font-Bold="true" />
                                    <asp:TemplateField HeaderText="Paid?">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIsPaid" runat="server" Text='<%#Eval("CPaymentIsPaid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Session" DataField="FullCode" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="alert alert-warning" role="alert" style="text-align: center">⚠️ Candidate Form And Payment Info Not Found ❗</div>
                                </EmptyDataTemplate>
                            </asp:GridView>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%-- EDUCATION --%>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>EDUCATION</strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanelEdu" runat="server" UpdateMode="Always">
                        <ContentTemplate>

                            <asp:Panel ID="messagePanelEdu" runat="server">
                                <asp:Label ID="lblMessageEdu" runat="server" Text=""></asp:Label>
                            </asp:Panel>

                            <asp:GridView ID="gvEdu" runat="server"
                                CssClass="table table-responsive table-hover table-bordered"
                                AutoGenerateColumns="false" Width="100%" ShowHeader="true" HeaderStyle-Font-Bold="true">
                                <HeaderStyle BackColor="#1387de" ForeColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SSC/O-Level/Dakhil">
                                        <ItemTemplate>
                                            <strong>Institute: </strong><asp:Label ID="lblSscInstitute" runat="server" Text='<%#Eval("SscInstitute") %>'></asp:Label><br />
                                            <strong>Board: </strong><asp:Label ID="lblSscEduBoard" runat="server" Text='<%#Eval("SscEduBoard") %>'></asp:Label><br />
                                            <strong>Type: </strong><asp:Label ID="lblSscExamType" runat="server" Text='<%#Eval("SscExamTypeCode") %>'></asp:Label><br />
                                            <strong>Roll: </strong><asp:Label ID="lblSscRoll" runat="server" Text='<%#Eval("SscRoll") %>'></asp:Label><br />
                                            <strong>Reg. No: </strong><asp:Label ID="lblSscRegNo" runat="server" Text='<%#Eval("SscRegNo") %>'></asp:Label><br />
                                            <strong>Group/Subject: </strong><asp:Label ID="lblSscGrpSub" runat="server" Text='<%#Eval("SscGroupSubject") %>'></asp:Label><br />
                                            <strong>Result/Division: </strong><asp:Label ID="lblSscResultDiv" runat="server" Text='<%#Eval("SscResultDiv") %>'></asp:Label><br />
                                            <strong>GPA: </strong><asp:Label ID="lblSscGpa" runat="server" Text='<%#Eval("SscGpa") %>'></asp:Label><br />
                                            <strong>GPA W4S: </strong><asp:Label ID="lblSscGpaW4s" runat="server" Text='<%#Eval("SscGpaW4S") %>'></asp:Label><br />
                                            <strong>Marks: </strong><asp:Label ID="lblSscMarks" runat="server" Text='<%#Eval("SscMarks") %>'></asp:Label><br />
                                            <strong>Year: </strong><asp:Label ID="lblSscYear" runat="server" Text='<%#Eval("SscYear") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="25%" Font-Size="Small"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HSC/A-Level/Alim/Diploma">
                                        <ItemTemplate>
                                            <strong>Institute: </strong><asp:Label ID="lblHscInstitute" runat="server" Text='<%#Eval("HscInstitute") %>'></asp:Label><br />
                                            <strong>Board: </strong><asp:Label ID="lblHscEduBoard" runat="server" Text='<%#Eval("HscBoard") %>'></asp:Label><br />
                                            <strong>Type: </strong><asp:Label ID="lblHscExamType" runat="server" Text='<%#Eval("HscExamTypeCode") %>'></asp:Label><br />
                                            <strong>Roll: </strong><asp:Label ID="lblHscRoll" runat="server" Text='<%#Eval("HscRoll") %>'></asp:Label><br />
                                            <strong>Reg. No: </strong><asp:Label ID="lblHscRegNo" runat="server" Text='<%#Eval("HscRegNo") %>'></asp:Label><br />
                                            <strong>Group/Subject: </strong><asp:Label ID="lblHscGrpSub" runat="server" Text='<%#Eval("HscGroupSubject") %>'></asp:Label><br />
                                            <strong>Result/Division: </strong><asp:Label ID="lblHscResultDiv" runat="server" Text='<%#Eval("HscResultDiv") %>'></asp:Label><br />
                                            <strong>GPA: </strong><asp:Label ID="lblHscGpa" runat="server" Text='<%#Eval("HscGpa") %>'></asp:Label><br />
                                            <strong>GPA W4S: </strong><asp:Label ID="lblHscGpaW4s" runat="server" Text='<%#Eval("HscGpaW4S") %>'></asp:Label><br />
                                            <strong>Marks: </strong><asp:Label ID="lblHscMarks" runat="server" Text='<%#Eval("HscMarks") %>'></asp:Label><br />
                                            <strong>Year: </strong><asp:Label ID="lblHscYear" runat="server" Text='<%#Eval("HscYear") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="25%" Font-Size="Small"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Bachelors">
                                        <ItemTemplate>
                                            <strong>Institute: </strong><asp:Label ID="lblUndInstitute" runat="server" Text='<%#Eval("UndInstitute") %>'></asp:Label><br />
                                            <strong>Program: </strong><asp:Label ID="lblUndEduBoard" runat="server" Text='<%#Eval("UndProgram") %>'></asp:Label><br />
                                            <strong>Other Program: </strong><asp:Label ID="lblUndExamType" runat="server" Text='<%#Eval("UndProgramOther") %>'></asp:Label><br />
                                            <strong>Roll: </strong><asp:Label ID="lblUndRoll" runat="server" Text='<%#Eval("UndRoll") %>'></asp:Label><br />
                                            <strong>Group/Subject: </strong><asp:Label ID="lblUndGrpSub" runat="server" Text='<%#Eval("UndGroupSubject") %>'></asp:Label><br />
                                            <strong>Result/Division: </strong><asp:Label ID="lblUndResultDiv" runat="server" Text='<%#Eval("UndResultDiv") %>'></asp:Label><br />
                                            <strong>CGPA: </strong><asp:Label ID="lblUndGpa" runat="server" Text='<%#Eval("UndCGPA") %>'></asp:Label><br />
                                            <strong>Year: </strong><asp:Label ID="lblUndYear" runat="server" Text='<%#Eval("UndYear") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="25%" Font-Size="Small"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Masters">
                                        <ItemTemplate>
                                            <strong>Institute: </strong><asp:Label ID="lblGrdInstitute" runat="server" Text='<%#Eval("GrdInstitute") %>'></asp:Label><br />
                                            <strong>Program: </strong><asp:Label ID="lblGrdEduBoard" runat="server" Text='<%#Eval("GrdProgram") %>'></asp:Label><br />
                                            <strong>Other Program: </strong><asp:Label ID="lblGrdExamType" runat="server" Text='<%#Eval("GrdProgramOther") %>'></asp:Label><br />
                                            <strong>Roll: </strong><asp:Label ID="lblGrdRoll" runat="server" Text='<%#Eval("GrdRoll") %>'></asp:Label><br />
                                            <strong>Group/Subject: </strong><asp:Label ID="lblGrdGrpSub" runat="server" Text='<%#Eval("GrdGroupSubject") %>'></asp:Label><br />
                                            <strong>Result/Division: </strong><asp:Label ID="lblGrdResultDiv" runat="server" Text='<%#Eval("GrdResultDiv") %>'></asp:Label><br />
                                            <strong>CGPA: </strong><asp:Label ID="lblGrdGpa" runat="server" Text='<%#Eval("GrdCGPA") %>'></asp:Label><br />
                                            <strong>Year: </strong><asp:Label ID="lblGrdYear" runat="server" Text='<%#Eval("GrdYear") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="25%" Font-Size="Small"/>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="alert alert-warning" role="alert" style="text-align: center">⚠️ Candidate Education Info Not Found ❗</div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%-- RELATION --%>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>RELATION</strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanelRelation" runat="server" UpdateMode="Always">
                        <ContentTemplate>

                            <asp:Panel ID="messagePanelRelation" runat="server">
                                <asp:Label ID="lblMessageRelation" runat="server" Text=""></asp:Label>
                            </asp:Panel>

                            <asp:GridView ID="gvRelation" runat="server"
                                CssClass="table table-responsive table-hover table-bordered"
                                AutoGenerateColumns="false" Width="100%" ShowHeader="true" HeaderStyle-Font-Bold="true">
                                <HeaderStyle BackColor="#1387de" ForeColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Father">
                                        <ItemTemplate>
                                            <strong>Name: </strong><asp:Label ID="lblFatherName" runat="server" Text='<%#Eval("FatherName") %>'></asp:Label><br />
                                            <strong>Occupation: </strong><asp:Label ID="lblFatherOccupation" runat="server" Text='<%#Eval("FatherOccupation") %>'></asp:Label><br />
                                            <strong>Designation: </strong><asp:Label ID="lblFatherDesignation" runat="server" Text='<%#Eval("FatherDesignation") %>'></asp:Label><br />
                                            <strong>Mobile: </strong><asp:Label ID="lblFatherMobile" runat="server" Text='<%#Eval("FatherMobile") %>'></asp:Label><br />
                                            <strong>Email: </strong><asp:Label ID="lblFatherEmail" runat="server" Text='<%#Eval("FatherEmail") %>'></asp:Label><br />
                                            <strong>National Id: </strong><asp:Label ID="lblFatherNationalId" runat="server" Text='<%#Eval("FatherNationalId") %>'></asp:Label><br />
                                            <strong>Nationality: </strong><asp:Label ID="lblFatherNationality" runat="server" Text='<%#Eval("FatherNationality") %>'></asp:Label><br />
                                            <strong>Is Late?: </strong><asp:Label ID="lblFatherIsLate" runat="server" Text=''></asp:Label><br />
                                            <strong>Occupation Type: </strong><asp:Label ID="lblFatherOccupationType" runat="server" Text=''></asp:Label><br />
                                        </ItemTemplate>
                                        <ItemStyle Width="33.33%" Font-Size="Small"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mother">
                                        <ItemTemplate>
                                            <strong>Name: </strong><asp:Label ID="lblMotherName" runat="server" Text='<%#Eval("MotherName") %>'></asp:Label><br />
                                            <strong>Occupation: </strong><asp:Label ID="lblMotherOccupation" runat="server" Text='<%#Eval("MotherOccupation") %>'></asp:Label><br />
                                            <strong>Designation: </strong><asp:Label ID="lblMotherDesignation" runat="server" Text='<%#Eval("MotherDesignation") %>'></asp:Label><br />
                                            <strong>Mobile: </strong><asp:Label ID="lblMotherMobile" runat="server" Text='<%#Eval("MotherMobile") %>'></asp:Label><br />
                                            <strong>National Id: </strong><asp:Label ID="lblMotherNationalId" runat="server" Text='<%#Eval("MotherNationalId") %>'></asp:Label><br />
                                            <strong>Nationality: </strong><asp:Label ID="lblMotherNationality" runat="server" Text='<%#Eval("motherNationality") %>'></asp:Label><br />
                                            <strong>Is Late?: </strong><asp:Label ID="lblMotherIsLate" runat="server" Text=''></asp:Label><br />
                                            <strong>Occupation Type: </strong><asp:Label ID="lblMotherOccupationType" runat="server" Text=''></asp:Label><br />
                                        </ItemTemplate>
                                        <ItemStyle Width="33.33%" Font-Size="Small"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Guardian">
                                        <ItemTemplate>
                                            <strong>Name: </strong><asp:Label ID="lblGuardianName" runat="server" Text='<%#Eval("GuardianName") %>'></asp:Label><br />
                                            <strong>Relation: </strong><asp:Label ID="lblGuardianRelation" runat="server" Text='<%#Eval("GuardianRelation") %>'></asp:Label><br />
                                            <strong>Designation: </strong><asp:Label ID="lblGuardianDesignation" runat="server" Text='<%#Eval("GuardianDesignation") %>'></asp:Label><br />
                                            <strong>Mobile: </strong><asp:Label ID="lblGuardianMobile" runat="server" Text='<%#Eval("GuardianMobile") %>'></asp:Label><br />
                                            <strong>Email: </strong><asp:Label ID="lblGuardianEmail" runat="server" Text='<%#Eval("GuardianEmail") %>'></asp:Label><br />
                                            <strong>National Id: </strong><asp:Label ID="lblGuardianNationalId" runat="server" Text='<%#Eval("GuardianNationalId") %>'></asp:Label><br />
                                            <strong>Nationality: </strong><asp:Label ID="lblGuardianNationality" runat="server" Text='<%#Eval("GuardianNationality") %>'></asp:Label><br />
                                        </ItemTemplate>
                                        <ItemStyle Width="33.33%" Font-Size="Small"/>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--  --%>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong></strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanel2" runat="server" UpdateMode="Always">
                        <ContentTemplate>

                            <asp:Panel ID="Panel2" runat="server">
                                <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                            </asp:Panel>



                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
