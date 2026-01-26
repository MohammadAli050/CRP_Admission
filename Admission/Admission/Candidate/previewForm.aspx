<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="previewForm.aspx.cs" Inherits="Admission.Admission.Candidate.previewForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .header-center {
            padding: 5px;
        }

        #MainContent_RadioButtonList1 tr td label {
            margin-left:10px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-sm-12 col-md-12 col-lg-12">
            <div class="panel panel-info">
                <div class="panel-heading  text-center" style="background-color: black">
                    <h4 style="color: #c4bfbf">Personal Information</h4>
                </div>
                <div class="panel-body">
                    <%--======================== Full Name ========================--%>
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>First Name<span class="spanAsterisk">*</span> <span style="color: #ff6c00; font-size: 8pt; font-weight: bold;"></span></label>
                                <asp:RequiredFieldValidator runat="server"
                                    ID="NameReq"
                                    ValidationGroup="SUBMIT"
                                    ControlToValidate="txtName"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="Name Required"
                                    ValidationExpression="^[A-Za-z-._ ]*$" />
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="100%" placeholder="Full Name" onkeydown="return (event.keyCode!=13);"></asp:TextBox>

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Middle Name<span style="color: #ff6c00; font-size: 8pt; font-weight: bold;"></span></label>
                                <asp:TextBox ID="txtMiddleName" runat="server" CssClass="form-control" Width="100%" placeholder="Middle Name" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <label>Last Name <span style="color: #ff6c00; font-size: 8pt; font-weight: bold;"></span></label>
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" Width="100%" placeholder="Last Name" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            </div>
                        </div>

                        <label>Father's Name<span class="spanAsterisk">*</span> <span style="color: #ff6c00; font-size: 8pt; font-weight: bold;"></span></label>
                        <asp:RequiredFieldValidator runat="server"
                            ID="RequiredFieldValidator4"
                            ValidationGroup="SUBMIT"
                            ControlToValidate="txtName"
                            Display="Dynamic"
                            Font-Size="9pt"
                            ForeColor="Crimson"
                            Font-Bold="false"
                            ErrorMessage="Name Required"
                            ValidationExpression="^[A-Za-z-._ ]*$" />
                        <asp:TextBox ID="txtFatherName" runat="server" CssClass="form-control" Width="100%" placeholder="Full Name" onkeydown="return (event.keyCode!=13);"></asp:TextBox>

                        <label>Mother's Name<span class="spanAsterisk">*</span> <span style="color: #ff6c00; font-size: 8pt; font-weight: bold;"></span></label>
                        <asp:RequiredFieldValidator runat="server"
                            ID="RequiredFieldValidator5"
                            ValidationGroup="SUBMIT"
                            ControlToValidate="txtName"
                            Display="Dynamic"
                            Font-Size="9pt"
                            ForeColor="Crimson"
                            Font-Bold="false"
                            ErrorMessage="Name Required"
                            ValidationExpression="^[A-Za-z-._ ]*$" />
                        <asp:TextBox ID="txtMotherName" runat="server" CssClass="form-control" Width="100%" placeholder="Full Name" onkeydown="return (event.keyCode!=13);"></asp:TextBox>

                    </div>


                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label>Date of Birth<span class="spanAsterisk">*</span></label>
                                <asp:TextBox ID="txtDateOfBirth" runat="server" Width="100%" CssClass="form-control" placeholder="dd/MM/yyyy"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalenderExtender_DOB" runat="server"
                                    TargetControlID="txtDateOfBirth" Format="dd/MM/yyyy" />


                                <span id="txtDateOfBirthValidateMassage" runat="server" style="font-weight: bold; color: crimson;"></span>

                            </div>
                        </div>


                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label>Gender<span class="spanAsterisk">*</span></label>
                                <asp:DropDownList ID="ddlGender" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator3" runat="server"
                                    ControlToValidate="ddlGender" ErrorMessage="required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </div>
                        </div>
                    </div>

                    <%--======================== END Date of Birth ========================--%>
                    <%--======================== country religion ========================--%>
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label>Country<span class="spanAsterisk">*</span></label>
                                <asp:DropDownList ID="ddlCountry" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator1" runat="server"
                                    ControlToValidate="ddlCountry" ErrorMessage="required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </div>
                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label>Religion<span class="spanAsterisk">*</span></label>
                                <asp:DropDownList ID="ddlReligion" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator2" runat="server"
                                    ControlToValidate="ddlReligion" ErrorMessage="required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </div>
                        </div>

                    </div>
                    <%--======================== email passport ========================--%>

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">

                            <div class="form-group">
                                <label>Email<span class="spanAsterisk">*</span></label>
                                <asp:TextBox ID="txtEmail" runat="server" Width="100%" CssClass="form-control"
                                    placeholder="Email"
                                    TextMode="Email"
                                    onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                <span style="color: darkorange; font-size: 9pt;">Please provide a valid email address.</span>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtEmail"
                                    ValidationGroup="SUBMIT"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="Please enter valid email. (Ex: xyz@zyz.com)">   
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator runat="server"
                                    ID="RequiredFieldValidator1"
                                    ValidationGroup="SUBMIT"
                                    ControlToValidate="txtEmail"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="Email Required" />
                            </div>

                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label><strong>Passport Number <span style="color: crimson; font-weight: bold;">*</span></strong></label>


                                <asp:TextBox ID="txtPassportNo" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="txtPlaceOfBirth_ReqV" runat="server"
                                    ControlToValidate="txtPassportNo"
                                    ErrorMessage="Required"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    Font-Bold="true"
                                    ValidationGroup="SUBMIT">
                                </asp:RequiredFieldValidator>

                            </div>
                        </div>
                    </div>

                    <%--phone--%>

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">

                            <label>Mobile No.(International)<span class="spanAsterisk">*</span></label>
                            <div>
                                <%--  <div style="float: left;">
                                    <asp:TextBox ID="txtCountryCodeGuardianMobile" runat="server" CssClass="form-control" Text="+" Style="width: 54px;" ReadOnly="true"></asp:TextBox>
                                </div>--%>
                                <div>
                                    <asp:TextBox ID="txtIntMobile" runat="server" Width="100%" type="number" CssClass="form-control" placeholder="" MaxLength="11" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    <%--<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1"
                                        ValidationGroup="SUBMIT"
                                        Font-Size="9pt"
                                        ForeColor="Crimson"
                                        Display="Dynamic"
                                        Font-Bold="False"
                                        ErrorMessage="Invalid format"
                                        ControlToValidate="txtIntMobile"
                                        ValidationExpression="^(?:)?01[13-9]\d{8}$"></asp:RegularExpressionValidator>
                                    <span style="color: darkorange; font-size: 9pt; display: none;">Guardian will not recieve information,
                                                                <br />
                                        If number is in wrong format.
                                    </span>--%>
                                </div>
                            </div>
                            <%--<asp:RequiredFieldValidator runat="server"
                                ID="RequiredFieldValidator3"
                                ValidationGroup="SUBMIT"
                                ControlToValidate="txtIntMobile"
                                Display="Dynamic"
                                Font-Size="9pt"
                                ForeColor="Crimson"
                                Font-Bold="false"
                                ErrorMessage="valid Number Required" />--%>
                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-6">

                            <label>Mobile No.(If you have any bangladeshi Number)<span class="spanAsterisk">*</span></label>
                            <div>
                                <%--<div style="float: left;">
                                    <asp:TextBox ID="txtSmsPhone" runat="server" CssClass="form-control" Text="+88" Style="width: 54px;" ReadOnly="true"></asp:TextBox>
                                </div>--%>
                                <div>
                                    <asp:TextBox ID="txtPhone" runat="server" Width="100%" type="number" CssClass="form-control" placeholder="017XXXXXXXX" MaxLength="11" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    <%-- <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator2"
                                        ValidationGroup="SUBMIT"
                                        Font-Size="9pt"
                                        ForeColor="Crimson"
                                        Display="Dynamic"
                                        Font-Bold="False"
                                        ErrorMessage="Invalid format"
                                        ControlToValidate="txtPhone"
                                        ValidationExpression="^(?:)?01[13-9]\d{8}$"></asp:RegularExpressionValidator>
                                    <span style="color: darkorange; font-size: 9pt; display: none;">Guardian will not recieve information,
                                                                <br />
                                        If number is in wrong format.
                                    </span>--%>
                                </div>
                            </div>
                            <%-- <asp:RequiredFieldValidator runat="server"
                                ID="RequiredFieldValidator7"
                                ValidationGroup="SUBMIT"
                                ControlToValidate="txtPhone"
                                Display="Dynamic"
                                Font-Size="9pt"
                                ForeColor="Crimson"
                                Font-Bold="false"
                                ErrorMessage="valid Number Required" />--%>
                        </div>
                    </div>

                    <%--address--%>

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label><strong>Corresponding Address<span style="color: crimson; font-weight: bold;">*</span></strong></label>


                                <asp:TextBox ID="txtPresentAddress" runat="server" CssClass="form-control" Width="100%" TextMode="MultiLine"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txtPresentAddress"
                                    ErrorMessage="Required"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    Font-Bold="true"
                                    ValidationGroup="SUBMIT">
                                </asp:RequiredFieldValidator>

                            </div>
                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label><strong>Permanent Address at home country<span style="color: crimson; font-weight: bold;">*</span></strong></label>


                                <asp:TextBox ID="txtParmamentAddress" runat="server" CssClass="form-control" Width="100%" TextMode="MultiLine"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                    ControlToValidate="txtParmamentAddress"
                                    ErrorMessage="Required"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    Font-Bold="true"
                                    ValidationGroup="SUBMIT">
                                </asp:RequiredFieldValidator>

                            </div>
                        </div>


                    </div>


                    <div class="row col-lg-12 col-md-12 col-sm-12">
                        <label><strong>Source of Fund<span style="color: crimson; font-weight: bold;">*</span></strong></label>
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" style="margin-left:7px;" AutoPostBack="true">
                            <asp:ListItem Text="Private/Family" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Scholarship/Grant" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>



                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-6" id="divSc" runat="server">
                            <div class="form-group">
                                <label><strong>If Scholarship/Grant in (ii) please write the name of the donor (Use separate sheet if needed for details)<span style="color: crimson; font-weight: bold;">*</span></strong></label>

                                <asp:TextBox ID="lbldonorname" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>
                        </div>

                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <div class="form-group">
                                <label><strong>Reference in Bangladesh (If any)<span style="color: crimson; font-weight: bold;">*</span></strong></label>

                                <asp:TextBox ID="lblreference" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>
                        </div>
                    </div>


                </div>
            </div>



        </div>
    </div>



    <div class="row">
        <div class="col-sm-12 col-md-12 col-lg-12">
            <div class="panel panel-info">
                <div class="panel-heading  text-center" style="background-color: black">
                    <h4 style="color: #c4bfbf">Academic Information</h4>
                </div>
                <div class="panel-body">
                    <%--<div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label>Name of Desired Faculty<span class="spanAsterisk">*</span></label>
                                <asp:DropDownList ID="ddlAdmissionUnit" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlAdmissionUnitCompare" runat="server"
                                    ControlToValidate="ddlAdmissionUnit" ErrorMessage="required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label>Name of Desired Program<span class="spanAsterisk">*</span></label>
                                <asp:DropDownList ID="ddlProgram" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlProgramCompare" runat="server"
                                    ControlToValidate="ddlProgram" ErrorMessage="required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label>Session<span class="spanAsterisk">*</span></label>
                                <asp:DropDownList ID="ddlSession" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlSessionCompare" runat="server"
                                    ControlToValidate="ddlSession" ErrorMessage="required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </div>
                        </div>
                    </div>--%>

                    <asp:GridView runat="server" ID="GridViewProgramList" AutoGenerateColumns="False" AllowPaging="false" GridLines="None"
                        PagerSettings-Mode="NumericFirstLast" Width="100%"
                        PagerStyle-Font-Bold="true" PagerStyle-Font-Size="Larger"
                        ShowHeader="true">
                        <HeaderStyle BackColor="#91CDE0" ForeColor="Black" />
                        <RowStyle BackColor="#ecf0f0" />
                        <AlternatingRowStyle BackColor="#ffffff" />
                        <Columns>

                            <asp:TemplateField HeaderText="SL" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblSL" Text='<%#Eval("SL") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Program Name" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblName" Text='<%#Eval("ProgramName") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Priority" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblPriority" Text='<%#Eval("Priority") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Session" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblSession" Text='<%#Eval("Session") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Payment Status" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblPaymentStatus" Text='<%#Eval("PaymentStatus") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="File Information" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:HyperLink runat="server" CssClass="btn btn-primary" Text="View" ID="lblFileView" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank" NavigateUrl='<%#Eval("FilePath") %>' Visible='<%#Eval("FilePath")==null ? false : true %>'> Deposit Slip </asp:HyperLink>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Bank Name" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblBankName" Text='<%#Eval("BankName") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Transaction Id" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblTransactionId" Text='<%#Eval("TransactionId") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Payment Date" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblDate" Text='<%#Eval("PaymentDate") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                        </Columns>
                        <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" />

                    </asp:GridView>

                    <br />
                    <br />

                    <asp:GridView runat="server" ID="gvEducationList" AutoGenerateColumns="False" AllowPaging="false" OnRowDataBound="gvEducationList_RowDataBound" GridLines="None"
                        PagerSettings-Mode="NumericFirstLast" Width="100%"
                        PagerStyle-Font-Bold="true" PagerStyle-Font-Size="Larger"
                        ShowHeader="true">
                        <HeaderStyle BackColor="#91CDE0" ForeColor="Black" />
                        <RowStyle BackColor="#ecf0f0" />
                        <AlternatingRowStyle BackColor="#ffffff" />
                        <Columns>

                            <asp:TemplateField HeaderText="SL" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">

                                        <asp:Label runat="server" ID="lblSL" Text='<%#Eval("SL") %>' ForeColor="Black" Width="100%" Font-Bold="true"></asp:Label>
                                        <asp:HiddenField ID="ExamTypeId" runat="server" Value='<%#Eval("ExamId") %>' />

                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Name of the Examination" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:TextBox runat="server" ID="lblExam" Text='<%#Eval("ExamName") %>' ForeColor="Black" Font-Bold="false" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Year of Passing" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:HiddenField ID="hdnYear" runat="server" Value='<%#Eval("PassingYear") %>' />

                                        <asp:DropDownList ID="ddlPassingYear" runat="server" Font-Bold="false" CssClass="form-control" Width="90%">
                                        </asp:DropDownList>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Name of the Institution" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:TextBox runat="server" ID="txtInst" ForeColor="Black" Text='<%#Eval("InstituteName") %>' Font-Bold="false" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Letter Grade Obtained" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:TextBox runat="server" ID="txtLgD" ForeColor="Black" Text='<%#Eval("Grade") %>' Font-Bold="false" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Division" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">

                                        <asp:HiddenField ID="hdnDivisionId" runat="server" Value='<%#Eval("Division") %>' />

                                        <asp:DropDownList ID="ddlDvision" runat="server" Font-Bold="true" CssClass="form-control" Width="90%">
                                            <asp:ListItem Text="Select Division" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="1st Division" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="2nd Division" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="3rd Division" Value="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="15%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="GPA/%Marks" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:TextBox runat="server" ID="txtGpa" Text='<%#Eval("GpaMarks") %>' ForeColor="Black" CssClass="form-control" Font-Bold="false"></asp:TextBox>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>

                        </Columns>
                        <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" />

                    </asp:GridView>

                </div>


            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-sm-12 col-md-12 col-lg-12">
            <div class="panel panel-info">
                <div class="panel-heading  text-center" style="background-color: black">
                    <h4 style="color: #c4bfbf">Upload Documents</h4>
                </div>
                <div class="panel-body">


                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">1. Upload Your Recent Photo (Passport size JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadPhoto" runat="server" accept=".jpg,.jpeg,.png" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />
                            </div>

                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnUploadPhoto" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnUploadPhoto_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblPhotoURL" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>
                            </div>
                        </div>


                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">8. Upload a certified copy of valid passport(PDF, JPEG)</p>

                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadPassport" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnpassport" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnpassport_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblPassport" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>
                        </div>


                    </div>

                    <br />
                    <br />
                    <br />

                    <div class="row">




                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">2. Upload Your Signature (JPEG, PNG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadSignature" runat="server" accept=".jpg,.jpeg,.png" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnSignature" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnSignature_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblupsign" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>
                            </div>

                        </div>


                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">9. Upload National ID Card/Equivalent (PDF, JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadNID" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnNID" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnNID_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblnational" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>

                        </div>
                    </div>

                    <br />
                    <br />
                    <br />

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">3. Upload Secondary School Certificate/O-Level/ Equivalent  Diploma (PDF, JPEG)</p>

                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadAcademic" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnAcademic" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnAcademic_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblacademic" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>

                        </div>



                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">10. Upload Police Clearance Certificate (JPEG, PDF)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadPolice" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnPolice" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnPolice_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblpolice" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>

                        </div>

                    </div>

                    <br />
                    <br />
                    <br />

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">4. Upload Bachelor/honors certificate (for masters only)(PDF, JPEG)</p>

                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadBechlor" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnbachelor" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnbachelor_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblbachelorcer" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>

                        </div>


                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">11. Upload Scholarship/Grant Application (PDF, JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadGrant" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnGrant" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnGrant_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblscholarhip" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>

                        </div>

                    </div>

                    <br />
                    <br />
                    <br />

                    <div class="row">


                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">5. Upload Curriculum Vitae (PDF, JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadCV" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnCVUpload" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnCVUpload_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblcurriculamURL" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>
                            </div>

                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">12. Upload Health Certificate (PDF,JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadHealth" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnHealth" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnHealth_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblhealth" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>


                        </div>

                    </div>

                    <br />
                    <br />
                    <br />

                    <div class="row">


                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">6. Upload English Language Proficiency Certificate (PDF, JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadEnglishCertificate" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnEnglishCertificate" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnEnglishCertificate_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblEnglish" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>

                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">13. Upload Application for scholarship/financial aid (PDF, JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadDeposit" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnDeposit" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnDeposit_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lbldeposite" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>
                        </div>

                    </div>


                    <br />
                    <br />
                    <br />

                    <div class="row">


                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">7. Upload Higher Secondary Certificate/A-Level/ Equivalent  Diploma (PDF, JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadMedical" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />
                                <br />

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnHsc" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnHsc_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>


                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblmedicalcertificate" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>

                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">14. Upload Testimonial from your School and College (PDF, JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadTestimonial" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />


                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnTestimonial" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnTestimonial_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lbltestimonial" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>
                        </div>

                    </div>


                    <div class="row">
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <div class="button" style="margin-top: 27px;">
                                <asp:HyperLink ID="lblbtn" Style="border-radius: 4px; background-color: #000000a6; color: white;" NavigateUrl="~/Admission/Candidate/foreignStudentApplicationDeclaration.aspx?ecat=4"
                                    runat="server" CssClass="btn btn-light">
                                        <span class="glyphicon glyphicon-chevron-left"></span> Back &nbsp;
                                </asp:HyperLink>
                            </div>
                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <div style="text-align: right">

                                <asp:Button ID="lblbtnSave" ValidationGroup="SUBMIT" Style="border-radius: 4px; width: 85px; margin-top: 27px;"
                                    runat="server" Text="Save" CssClass="btn btn-primary" OnClick="lblbtnSave_Click"></asp:Button>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

</asp:Content>
