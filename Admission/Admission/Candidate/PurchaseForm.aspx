<%@ Page Title="Purchase Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseForm.aspx.cs" Inherits="Admission.Admission.Candidate.PurchaseForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">


    <style type="text/css">
        /*.style_td {
            font-weight: bold;
            text-align: left;
            font-size: 9pt;
        }

            text-align: right;
            font-size: 9pt;
        }

        .style_td_secondCol {
            border-left: dotted;
            border-color: gray;
            border-width: 1px;
        }

        .spanAsterisk {
            color: crimson;
            font-size: 12pt;
        }

        .style_thead {
            text-align: center;
            font-family: Calibri;
            font-size: 12pt;
            font-weight: bold;
        }

        .panelBody_edu_marginBottom {
            margin-bottom: -3%;
        }



        .modalBackground {
            background-color: #2a2d2a;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
        }

        .auto-style1 {
            height: 32px;
        }

        .auto-style2 {
            width: 29%;
        }


        @media screen and (max-width: 768px) {
            .auto-style2 {
                width: 100% !important;
            }
            .remarks{
                width: 100% !important
                width: 100% !important
            }
            .button-blue{
                width: 100% !important
            }
        }*/

        #MainContent_rbBachelorOrBachelorPass label {
            font-size: 20px;
            margin-right: 40px;
        }

        #MainContent_rbBachelorOrBachelorPass input {
            width: 20px;
            height: 20px;
            margin-right: 8px;
        }

        .modalBackground {
            background-color: #2a2d2a;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }
    </style>


    <script type="text/javascript">

        function onlyDotsAndNumbers(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode == 46) {
                return true;
            }
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

    </script>

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


    <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="250px" Width="250px" />
    </div>

    <br />


    <asp:UpdatePanel ID="upAll" runat="server">
        <ContentTemplate>
            <div class="row" style="background: white; border-radius:20px;">
                <div class="col-md-12">
                    <div class="panel panel-primary">
                        <div class="panel-heading" style="text-align: center;">
                            <h4><b>Purchase Form</b></h4>
                            <asp:Button ID="btnAdminAssignTestValue" runat="server" Text="Assign Test Value" CssClass="btn btn-danger" OnClick="btnAdminAssignTestValue_Click" Visible="false" />
                        </div>
                        <div class="panel-body">

                            <%--======================== Info ========================--%>
                            <div class="row">
                                <div class="col-sm-12 col-md-12 col-lg-12">
                                    <div class="alert alert-warning" style="padding-top: 10px; padding-bottom: 0px; margin-bottom: 2px;">
                                        <strong>Info!</strong>
                                        <ul style="padding-left: 20px; padding-bottom: 10px;">
                                            <li>
                                                <span style="font-weight: bold; color: crimson;">(*) Indicates required field.</span>
                                            </li>
                                            <li>
                                                <span style="font-weight: bold; color: orangered">Please note that this is not the final application. After successful payment candidate has to fill up further information form to get Admit Card.
                                                </span>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <%--======================== END Info ========================--%>
                             <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                            <%--======================== MessageTop ========================--%>
                            <div class="row">
                                <div class="col-sm-12 col-md-12 col-lg-12">
                                    <asp:Panel ID="messagePanelTop" runat="server" Visible="false">
                                        <asp:Label ID="lblMessageTop" runat="server" Text=""></asp:Label>

                                    </asp:Panel>
                                </div>
                            </div>
                            <%--======================== END MessageTop ========================--%>


                            <%--======================== Basic Info ========================--%>
                            <div class="row">
                                <div class="col-sm-12 col-md-12 col-lg-12">
                                    <div class="panel panel-info">
                                        <div class="panel-heading  text-center" style="margin-top: 15px;">
                                            <h4>Basic Information</h4>
                                        </div>
                                        <div class="panel-body">
                                            <%--======================== Full Name ========================--%>
                                            <div class="form-group">
                                                <label>Full Name<span class="spanAsterisk">*</span> <span style="color: #ff6c00; font-size: 10pt; font-weight: bold;">(According to SSC / O-Level / Equivalent Certificate)</span></label>
                                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="100%" placeholder="Full Name" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
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
                                            </div>
                                            <%--======================== END Full Name ========================--%>


                                            <%--======================== Date of Birth ========================--%>
                                            <div class="form-group">
                                                <label>Date of Birth<span class="spanAsterisk">*</span></label>
                                                <div class="row">
                                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                                        <asp:DropDownList ID="ddlDay" CssClass="form-control" Width="100%" runat="server" OnSelectedIndexChanged="ddlBirthDate_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        <asp:CompareValidator ID="CompareValidator11" runat="server"
                                                            ValidationGroup="SUBMIT"
                                                            ControlToValidate="ddlDay"
                                                            ErrorMessage="Day required"
                                                            Display="Dynamic"
                                                            Font-Size="9pt"
                                                            ForeColor="Crimson"
                                                            ValueToCompare="-1"
                                                            Operator="NotEqual"></asp:CompareValidator>
                                                    </div>
                                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                                        <asp:DropDownList ID="ddlMonth" CssClass="form-control" Width="100%" runat="server" OnSelectedIndexChanged="ddlBirthDate_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        <asp:CompareValidator ID="CompareValidator12" runat="server"
                                                            ValidationGroup="SUBMIT"
                                                            ControlToValidate="ddlDay"
                                                            ErrorMessage="Month required"
                                                            Display="Dynamic"
                                                            Font-Size="9pt"
                                                            ForeColor="Crimson"
                                                            ValueToCompare="-1"
                                                            Operator="NotEqual"></asp:CompareValidator>
                                                    </div>
                                                    <div class="col-sm-4 col-md-4 col-lg-4">
                                                        <asp:DropDownList ID="ddlYear" CssClass="form-control" Width="100%" runat="server" OnSelectedIndexChanged="ddlBirthDate_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        <asp:CompareValidator ID="CompareValidator13" runat="server"
                                                            ValidationGroup="SUBMIT"
                                                            ControlToValidate="ddlMonth"
                                                            ErrorMessage="Year required"
                                                            Display="Dynamic"
                                                            Font-Size="9pt"
                                                            ForeColor="Crimson"
                                                            ValueToCompare="-1"
                                                            Operator="NotEqual"></asp:CompareValidator>
                                                    </div>
                                                </div>

                                                <span id="txtDateOfBirthValidateMassage" runat="server" style="font-weight: bold; color: crimson;"></span>

                                            </div>
                                            <%--======================== END Date of Birth ========================--%>


                                            <%--======================== Email & Gender & SMS Phone ========================--%>
                                            <div class="row">
                                                <div class="col-sm-6 col-md-6 col-lg-6">

                                                    <div class="form-group">
                                                        <label>Email<span class="spanAsterisk">*</span></label>
                                                        <asp:TextBox ID="txtEmail" runat="server" Width="100%" CssClass="form-control"
                                                            placeholder="Email"
                                                            TextMode="Email"
                                                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                        <span style="color: darkorange; font-size: 10pt;">Please provide a valid email address.</span>
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
                                                        <label>Gender<span class="spanAsterisk">*</span></label>
                                                        <asp:DropDownList ID="ddlGender" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                                        <asp:CompareValidator ID="ddlGenderComV" runat="server"
                                                            ControlToValidate="ddlGender" ErrorMessage="Gender required"
                                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </div>

                                                </div>
                                                <div class="col-sm-4 col-md-4 col-lg-4" style="display:none">
                                                    <div class="form-group">
                                                        <label><strong>Quota <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                                        <asp:DropDownList ID="ddlQuota" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--======================== End Email & Gender & SMS Phone ========================--%>



                                            <%--======================== Phone & Guardian Phone ========================--%>
                                            <div class="row">
                                                <div class="col-sm-6 col-md-6 col-lg-6">
                                                    <div class="form-group">
                                                        <label>Mobile No. (for SMS)<span class="spanAsterisk">*</span></label>
                                                        <div>
                                                            <div style="float: left;">
                                                                <asp:TextBox ID="txtCountryCodeSMSMobile" runat="server" CssClass="form-control" Text="+88" Style="width: 54px;" ReadOnly="true"></asp:TextBox>
                                                            </div>
                                                            <div>
                                                                <asp:TextBox ID="txtSmsMobile" runat="server" Width="89%" type="number" CssClass="form-control" placeholder="017XXXXXXXX" MaxLength="11" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                                <asp:RegularExpressionValidator runat="server" ID="mobileReg"
                                                                    ValidationGroup="SUBMIT"
                                                                    Font-Size="9pt"
                                                                    ForeColor="Crimson"
                                                                    Display="Dynamic"
                                                                    Font-Bold="False"
                                                                    ErrorMessage="Invalid format"
                                                                    ControlToValidate="txtSmsMobile"
                                                                    ValidationExpression="^(?:)?01[13-9]\d{8}$"></asp:RegularExpressionValidator>
                                                                <span style="color: darkorange; font-size: 10pt;">Candidate will not recieve Username and Password,
                                                        <br />
                                                                    If number is in wrong format.
                                                                </span>
                                                            </div>
                                                        </div>
                                                        <asp:RequiredFieldValidator runat="server"
                                                            ID="RequiredFieldValidator2"
                                                            ValidationGroup="SUBMIT"
                                                            ControlToValidate="txtSmsMobile"
                                                            Display="Dynamic"
                                                            Font-Size="9pt"
                                                            ForeColor="Crimson"
                                                            Font-Bold="false"
                                                            ErrorMessage="Mobile Number Required" />
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6">
                                                    <div class="form-group">
                                                        <label>Guardian Mobile No.<span class="spanAsterisk">*</span></label>
                                                        <div>
                                                            <div style="float: left;">
                                                                <asp:TextBox ID="txtCountryCodeGuardianMobile" runat="server" CssClass="form-control" Text="+88" Style="width: 54px;" ReadOnly="true"></asp:TextBox>
                                                            </div>
                                                            <div>
                                                                <asp:TextBox ID="txtGuardianMobile" runat="server" Width="89%" type="number" CssClass="form-control" placeholder="017XXXXXXXX" MaxLength="11" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1"
                                                                    ValidationGroup="SUBMIT"
                                                                    Font-Size="9pt"
                                                                    ForeColor="Crimson"
                                                                    Display="Dynamic"
                                                                    Font-Bold="False"
                                                                    ErrorMessage="Invalid format"
                                                                    ControlToValidate="txtGuardianMobile"
                                                                    ValidationExpression="^(?:)?01[13-9]\d{8}$"></asp:RegularExpressionValidator>
                                                                <span style="color: darkorange; font-size: 10pt; display: none;">Guardian will not recieve information,
                                                                <br />
                                                                    If number is in wrong format.
                                                                </span>
                                                            </div>
                                                        </div>
                                                        <asp:RequiredFieldValidator runat="server"
                                                            ID="RequiredFieldValidator3"
                                                            ValidationGroup="SUBMIT"
                                                            ControlToValidate="txtGuardianMobile"
                                                            Display="Dynamic"
                                                            Font-Size="9pt"
                                                            ForeColor="Crimson"
                                                            Font-Bold="false"
                                                            ErrorMessage="Guardian Number Required" />
                                                    </div>
                                                </div>
                                            </div>
                                            <%--======================== END Phone & Guardian Phone ========================--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--======================== END Basic Info ========================--%>

                            <%--======================== SSC / O-Level / Equivalent ========================--%>
                            <div class="row">
                                <div class="col-sm-12 col-md-12 col-lg-12">
                                    <div class="panel panel-info">
                                        <div class="panel-heading  text-center style="margin-top: 15px;"">
                                            <h4>SSC / O-Level / Equivalent</h4>
                                        </div>
                                        <div class="panel-body">

                                            <%--======================== Institute & Board ========================--%>
                                            <div class="row">
                                                <div class="col-sm-6 col-md-6 col-lg-6">
                                                    <div class="form-group">
                                                        <label>Institute<span class="spanAsterisk">*</span></label>
                                                        <asp:TextBox ID="txtInstituteSSC" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                                            ControlToValidate="txtInstituteSSC"
                                                            ErrorMessage="Required"
                                                            ForeColor="Crimson"
                                                            Display="Dynamic"
                                                            Font-Size="9pt"
                                                            Font-Bold="false"
                                                            ValidationGroup="SUBMIT"> </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6">
                                                    <div class="form-group">
                                                        <label>Board<span class="spanAsterisk">*</span></label>
                                                        <asp:DropDownList ID="ddlBoardSSC" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                                        <asp:CompareValidator ID="CompareValidator1" runat="server"
                                                            ControlToValidate="ddlBoardSSC" ErrorMessage="Required"
                                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--======================== END Institute & Board ========================--%>

                                            <%--======================== Exam Type & Group ========================--%>
                                            <div class="row">
                                                <div class="col-sm-6 col-md-6 col-lg-6">
                                                    <div class="form-group">
                                                        <label>Exam Type<span class="spanAsterisk">*</span></label>
                                                        <asp:DropDownList ID="ddlExamTypeSSC" runat="server" Width="100%" CssClass="form-control"
                                                            OnSelectedIndexChanged="ddlExamTypeSSC_SelectedIndexChanged" AutoPostBack="true">
                                                            <asp:ListItem Enabled="true" Text="--Select Exam Type--" Value="-1"></asp:ListItem>
                                                            <asp:ListItem Text="SSC" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="O-Level" Value="5"></asp:ListItem>
                                                            <asp:ListItem Text="Dakhil" Value="6"></asp:ListItem>
                                                            <asp:ListItem Text="SSC (Vocational)" Value="12"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="ddlExamTypeSSCComV" runat="server"
                                                            ControlToValidate="ddlExamTypeSSC" ErrorMessage="SSC Exam Type Required"
                                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6">
                                                    <div class="form-group">
                                                        <label>Group<span class="spanAsterisk">*</span></label>
                                                        <asp:DropDownList ID="ddlGroupSSC" runat="server" Width="100%" CssClass="form-control">
                                                            <asp:ListItem Enabled="true" Text="--Select Group--" Value="-1"></asp:ListItem>
                                                            <asp:ListItem Text="Other" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Science" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="Humanities" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="Business" Value="5"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="ddlGroupSSCComV" runat="server"
                                                            ControlToValidate="ddlGroupSSC" ErrorMessage="SSC Group Required"
                                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--======================== END Exam Type & Group ========================--%>

                                            <%--======================== GPA & Passing Year ========================--%>
                                            <div class="row">
                                                <div class="col-sm-6 col-md-6 col-lg-6">
                                                    <div class="form-group">
                                                        <label>Division/Class<span class="spanAsterisk">*</span></label>
                                                        <asp:DropDownList ID="ddlSSCDivisionClass" runat="server" Width="100%" CssClass="form-control"
                                                            OnSelectedIndexChanged="ddlSSCDivisionClass_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="CompareValidator7" runat="server"
                                                            ControlToValidate="ddlSSCDivisionClass" ErrorMessage="Required"
                                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6">
                                                    <div class="form-group">
                                                        <label>GPA<span class="spanAsterisk">*</span></label>
                                                        <asp:TextBox ID="txtGPASSC" runat="server" type="number"
                                                            step="0.01" placeholder="GPA (x.xx or x)" Width="100%" CssClass="form-control"
                                                            onkeydown="return (event.keyCode!=13);" MaxLength="4" AutoCompleteType="Disabled"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="revGPASSC" runat="server"
                                                            ValidationGroup="SUBMIT"
                                                            ControlToValidate="txtGPASSC"
                                                            Display="Dynamic"
                                                            Font-Size="9pt"
                                                            ForeColor="Crimson"
                                                            Font-Bold="false"
                                                            ErrorMessage="GPA is in wrong format. Correct formate (x.xx or x)"
                                                            ValidationExpression="^\d{0,2}(\.\d{1,2})?$"
                                                            Enabled="false"></asp:RegularExpressionValidator>
                                                        <asp:RequiredFieldValidator ID="rfvGPASSC" runat="server"
                                                            ControlToValidate="txtGPASSC"
                                                            ErrorMessage="SSC GPA Required"
                                                            ForeColor="Crimson"
                                                            Display="Dynamic"
                                                            Font-Size="9pt"
                                                            Font-Bold="false"
                                                            ValidationGroup="SUBMIT"
                                                            Enabled="false"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--======================== END GPA & Passing Year ========================--%>


                                            <%--======================== Passing Year ========================--%>
                                            <div class="row">
                                                <div class="col-sm-12 col-md-12 col-lg-12">
                                                    <div class="form-group">
                                                        <label>Passing Year<span class="spanAsterisk">*</span></label>
                                                        <asp:DropDownList ID="ddlPassYearSSC" runat="server" Width="100%" CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="ddlPassYearSSCComV" runat="server"
                                                            ControlToValidate="ddlPassYearSSC" ErrorMessage="SSC Passing Year Required"
                                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--======================== END Passing Year ========================--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--======================== END SSC / O-Level / Equivalent ========================--%>


                            <%--======================== HSC / A-Level / Equivalent ========================--%>
                            <div class="row">
                                <div class="col-sm-12 col-md-12 col-lg-12">
                                    <div class="panel panel-info">
                                        <div class="panel-heading  text-center" style="margin-top: 15px;">
                                            <h4>HSC / A-Level / Equivalent</h4>
                                        </div>
                                        <div class="panel-body">

                                            <%--======================== Institute & Board ========================--%>
                                            <div class="row">
                                                <div class="col-sm-6 col-md-6 col-lg-6">
                                                    <div class="form-group">
                                                        <label>Institute<span class="spanAsterisk">*</span></label>
                                                        <asp:TextBox ID="txtInstituteHSC" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                            ControlToValidate="txtInstituteHSC"
                                                            ErrorMessage="Required"
                                                            ForeColor="Crimson"
                                                            Display="Dynamic"
                                                            Font-Size="9pt"
                                                            Font-Bold="false"
                                                            ValidationGroup="SUBMIT"> </asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6">
                                                    <div class="form-group">
                                                        <label>Board<span class="spanAsterisk">*</span></label>
                                                        <asp:DropDownList ID="ddlBoardHSC" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                                        <asp:CompareValidator ID="CompareValidator2" runat="server"
                                                            ControlToValidate="ddlBoardHSC" ErrorMessage="Required"
                                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--======================== END Institute & Board ========================--%>



                                            <%--======================== Exam Type & Group ========================--%>
                                            <div class="row">
                                                <div class="col-sm-6 col-md-6 col-lg-6">
                                                    <div class="form-group">
                                                        <label>Exam Type<span class="spanAsterisk">*</span></label>
                                                        <asp:DropDownList ID="ddlExamTypeHSC" runat="server" Width="100%" CssClass="form-control">
                                                            <asp:ListItem Enabled="true" Text="--Select Exam Type--" Value="-1"></asp:ListItem>
                                                            <asp:ListItem Text="HSC" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="A-Level" Value="7"></asp:ListItem>
                                                            <asp:ListItem Text="Alim" Value="8"></asp:ListItem>
                                                            <asp:ListItem Text="Diploma" Value="9"></asp:ListItem>
                                                            <asp:ListItem Text="HSC (Vocational)" Value="13"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="ddlExamTypeHSCComV" runat="server"
                                                            ControlToValidate="ddlExamTypeHSC" ErrorMessage="HSC Exam Type Required"
                                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6">
                                                    <div class="form-group">
                                                        <label>Group<span class="spanAsterisk">*</span></label>
                                                        <asp:DropDownList ID="ddlGroupHSC" runat="server" Width="100%" CssClass="form-control">
                                                            <asp:ListItem Enabled="true" Text="--Select Group--" Value="-1"></asp:ListItem>
                                                            <asp:ListItem Text="Other" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Science" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="Humanities" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="Business" Value="5"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="ddlGroupHSCComV" runat="server"
                                                            ControlToValidate="ddlGroupHSC" ErrorMessage="HSC Group Required"
                                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <%--======================== END Exam Type & Group ========================--%>




                                            <%--======================== GPA ========================--%>
                                            <div class="row">
                                                <div class="col-sm-6 col-md-6 col-lg-6">
                                                    <div class="form-group">
                                                        <label>Division/Class<span class="spanAsterisk">*</span></label>
                                                        <asp:DropDownList ID="ddlHSCDivisionClass" runat="server" Width="100%" CssClass="form-control"
                                                            OnSelectedIndexChanged="ddlHSCDivisionClass_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="CompareValidator6" runat="server"
                                                            ControlToValidate="ddlHSCDivisionClass" ErrorMessage="Required"
                                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6 col-lg-6">

                                                    <div class="form-group">
                                                        <label>GPA<span class="spanAsterisk">*</span></label>
                                                        <asp:TextBox ID="txtGPAHSC" runat="server" type="number"
                                                            step="0.01" placeholder="GPA (x.xx or x)" Width="100%" CssClass="form-control"
                                                            onkeydown="return (event.keyCode!=13);" MaxLength="4" AutoCompleteType="Disabled"
                                                            Enabled="false"></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="revGPAHSC" runat="server"
                                                            ValidationGroup="SUBMIT"
                                                            ControlToValidate="txtGPAHSC"
                                                            Display="Dynamic"
                                                            Font-Size="9pt"
                                                            ForeColor="Crimson"
                                                            Font-Bold="false"
                                                            ErrorMessage="GPA is in wrong format. Correct format (x.xx or x)"
                                                            ValidationExpression="^\d{0,2}(\.\d{1,2})?$"
                                                            Enabled="false"></asp:RegularExpressionValidator>
                                                        <asp:RequiredFieldValidator ID="rfvGPAHSC" runat="server"
                                                            ControlToValidate="txtGPAHSC"
                                                            ErrorMessage="HSC GPA Required"
                                                            ForeColor="Crimson"
                                                            Display="Dynamic"
                                                            Font-Size="9pt"
                                                            Font-Bold="false"
                                                            ValidationGroup="SUBMIT"
                                                            Enabled="false"></asp:RequiredFieldValidator>
                                                    </div>

                                                </div>
                                            </div>
                                            <%--======================== END GPA ========================--%>

                                            <%--======================== Passing Year ========================--%>
                                            <div class="row">
                                                <div class="col-sm-12 col-md-12 col-lg-12">
                                                    <div class="form-group">
                                                        <label>Passing Year<span class="spanAsterisk">*</span></label>
                                                        <asp:DropDownList ID="ddlPassYearHSC" runat="server" Width="100%" CssClass="form-control" Style="margin-top: 7px;">
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="ddlPassYearHSCComV" runat="server"
                                                            ControlToValidate="ddlPassYearHSC" ErrorMessage="HSC Passing Year Required"
                                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--======================== END Passing Year ========================--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--======================== END HSC / A-Level / Equivalent ========================--%>


                        <%--======================== Undergrad ========================--%>
                        <div class="row">
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <div class="panel panel-info">
                                    <div class="panel-heading  text-center" style="margin-top: 15px;">
                                        <h4>Bachelor</h4>
                                    </div>
                                    <div class="panel-body">

                                        <%--======================== Bachelor or Bachelor (Pass) ========================--%>
                                        <div class="row">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <asp:RadioButtonList ID="rbBachelorOrBachelorPass" runat="server"
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="1" Selected="True">Bachelor</asp:ListItem>
                                                    <asp:ListItem Value="2">Bachelor (Pass)</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                        <%--======================== End Bachelor or Bachelor (Pass) ========================--%>


                                        <%--======================== Institute ========================--%>
                                        <div class="row">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <div class="form-group">
                                                    <label>Institute<span class="spanAsterisk">*</span></label>
                                                    <asp:TextBox ID="txtUndergradInstitute" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                        ControlToValidate="txtUndergradInstitute"
                                                        ErrorMessage="Required"
                                                        ForeColor="Crimson"
                                                        Display="Dynamic"
                                                        Font-Size="9pt"
                                                        Font-Bold="false"
                                                        ValidationGroup="SUBMIT"> </asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                        </div>
                                        <%--======================== END Institute ========================--%>

                                        <%--======================== Program/Degree ========================--%>
                                        <div class="row">
                                            <div class="col-sm-6 col-md-6 col-lg-6">
                                                <div class="form-group">
                                                    <label>Program/Degree<span class="spanAsterisk">*</span></label>
                                                    <asp:DropDownList ID="ddlUndergradProgramDegree" runat="server" Width="100%" CssClass="form-control"
                                                        OnSelectedIndexChanged="ddlUndergradProgramDegree_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:CompareValidator ID="CompareValidator4" runat="server"
                                                        ControlToValidate="ddlUndergradProgramDegree" ErrorMessage="Required"
                                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                        ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-6">
                                                <div class="form-group">
                                                    <label>Others <span class="spanAsterisk">*</span></label>
                                                    <asp:TextBox ID="txtUndergradProgOthers" runat="server" Width="100%" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvUndergradProgOthers" runat="server" Enabled="false"
                                                        ControlToValidate="txtUndergradProgOthers" ErrorMessage="Required" ForeColor="Crimson"
                                                        Font-Size="9pt" Display="Dynamic" Font-Bold="true"
                                                        ValidationGroup="SUBMIT"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <%--======================== END Program/Degree ========================--%>

                                        <%--======================== CGPA ========================--%>
                                        <div class="row">
                                            <div class="col-sm-6 col-md-6 col-lg-6">
                                                <div class="form-group">
                                                    <label>Division/Class<span class="spanAsterisk">*</span></label>
                                                    <asp:DropDownList ID="ddlUndergradDivisionClass" runat="server" Width="100%" CssClass="form-control"
                                                        OnSelectedIndexChanged="ddlUndergradDivisionClass_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:CompareValidator ID="CompareValidator5" runat="server"
                                                        ControlToValidate="ddlUndergradProgramDegree" ErrorMessage="Required"
                                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                        ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-6">
                                                <div class="form-group">
                                                    <label>CGPA<span class="spanAsterisk">*</span></label>
                                                    <asp:TextBox ID="txtUndergradCGPA" runat="server" type="number"
                                                        step="0.01" placeholder="CGPA (x.xx or x)"
                                                        Width="100%" CssClass="form-control"
                                                        onkeydown="return (event.keyCode!=13);" MaxLength="4"
                                                        AutoCompleteType="Disabled" Enabled="false"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="revUndergradCGPA" runat="server"
                                                        ValidationGroup="SUBMIT"
                                                        ControlToValidate="txtUndergradCGPA"
                                                        Display="Dynamic"
                                                        Font-Size="9pt"
                                                        ForeColor="Crimson"
                                                        Font-Bold="false"
                                                        ErrorMessage="CGPA is in wrong format. Correct formate (x.xx or x)"
                                                        ValidationExpression="^\d{0,2}(\.\d{1,2})?$"
                                                        Enabled="false"></asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="rfvUndergradCGPA" runat="server"
                                                        ControlToValidate="txtUndergradCGPA"
                                                        ErrorMessage="Required"
                                                        ForeColor="Crimson"
                                                        Display="Dynamic"
                                                        Font-Size="9pt"
                                                        Font-Bold="false"
                                                        ValidationGroup="SUBMIT"
                                                        Enabled="false"> </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <%--======================== END CGPA ========================--%>

                                        <%--======================== Passing Year ========================--%>
                                        <div class="row">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <div class="form-group">
                                                    <label>Passing Year<span class="spanAsterisk">*</span></label>
                                                    <asp:DropDownList ID="ddlUndergradPassingYear" runat="server" Width="100%" CssClass="form-control">
                                                    </asp:DropDownList>
                                                    <asp:CompareValidator ID="CompareValidator3" runat="server"
                                                        ControlToValidate="ddlUndergradPassingYear" ErrorMessage="Required"
                                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                        ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <%--======================== END Passing Year ========================--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--======================== END Undergrad ========================--%>


                        <%--======================== Grad ========================--%>
                        <div class="row">
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <div class="panel panel-info">
                                    <div class="panel-heading  text-center" style="margin-top: 15px;">
                                        <h4>Masters</h4>
                                    </div>
                                    <div class="panel-body">

                                        <%--======================== Institute ========================--%>
                                        <div class="row">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <div class="form-group">
                                                    <label>Institute</label>
                                                    <%--<span class="spanAsterisk">*</span>--%>
                                                    <asp:TextBox ID="txtGradInstitute" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                                            ControlToValidate="txtGradInstitute"
                                                            ErrorMessage="Required"
                                                            ForeColor="Crimson"
                                                            Display="Dynamic"
                                                            Font-Size="9pt"
                                                            Font-Bold="false"
                                                            ValidationGroup="SUBMIT"> </asp:RequiredFieldValidator>--%>
                                                </div>
                                            </div>

                                        </div>
                                        <%--======================== END Institute ========================--%>

                                        <%--======================== Program/Degree ========================--%>
                                        <div class="row">
                                            <div class="col-sm-6 col-md-6 col-lg-6">
                                                <div class="form-group">
                                                    <label>Program/Degree</label>
                                                    <%--<span class="spanAsterisk">*</%--span>--%>
                                                    <asp:DropDownList ID="ddlGradProgramDegree" runat="server" Width="100%" CssClass="form-control"
                                                        OnSelectedIndexChanged="ddlGradProgramDegree_SelectedIndexChanged" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <%-- <asp:CompareValidator ID="CompareValidator6" runat="server"
                                                            ControlToValidate="ddlGradProgramDegree" ErrorMessage="Required"
                                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>--%>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-6">
                                                <div class="form-group">
                                                    <label>Others </label>
                                                    <%--<span class="spanAsterisk">*</span>--%>
                                                    <asp:TextBox ID="txtGradProgOthers" runat="server" Width="100%" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="rfvGradProgOthers" runat="server" Enabled="false"
                                                            ControlToValidate="txtGradProgOthers" ErrorMessage="Required" ForeColor="Crimson"
                                                            Font-Size="9pt" Display="Dynamic" Font-Bold="true"
                                                            ValidationGroup="SUBMIT"></asp:RequiredFieldValidator>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <%--======================== END Program/Degree ========================--%>

                                        <%--======================== CGPA ========================--%>
                                        <div class="row">
                                            <div class="col-sm-6 col-md-6 col-lg-6">
                                                <div class="form-group">
                                                    <label>Division/Class</label>
                                                    <%--<span class="spanAsterisk">*</span>--%>
                                                    <asp:DropDownList ID="ddlGradDivisionClass" runat="server" Width="100%" CssClass="form-control"
                                                        OnSelectedIndexChanged="ddlGradDivisionClass_SelectedIndexChanged1" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <%--<asp:CompareValidator ID="CompareValidator7" runat="server"
                                                            ControlToValidate="ddlGradProgramDegree" ErrorMessage="Required"
                                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>--%>
                                                </div>
                                            </div>
                                            <div class="col-sm-6 col-md-6 col-lg-6">
                                                <div class="form-group">
                                                    <label>CGPA</label>
                                                    <%--<span class="spanAsterisk">*</span>--%>
                                                    <asp:TextBox ID="txtGradCGPA" runat="server" type="number"
                                                        step="0.01" placeholder="CGPA (x.xx or x)"
                                                        Width="100%" CssClass="form-control"
                                                        onkeydown="return (event.keyCode!=13);" MaxLength="4"
                                                        AutoCompleteType="Disabled" Enabled="false"></asp:TextBox>
                                                    <%--<asp:RegularExpressionValidator ID="revGradCGPA" runat="server"
                                                            ValidationGroup="SUBMIT"
                                                            ControlToValidate="txtGradCGPA"
                                                            Display="Dynamic"
                                                            Font-Size="9pt"
                                                            ForeColor="Crimson"
                                                            Font-Bold="false"
                                                            ErrorMessage="CGPA is in wrong format. Correct formate (x.xx or x)"
                                                            ValidationExpression="^\d{0,2}(\.\d{1,2})?$"
                                                            Enabled="false"></asp:RegularExpressionValidator>
                                                        <asp:RequiredFieldValidator ID="rfvGradCGPA" runat="server"
                                                            ControlToValidate="txtGradCGPA"
                                                            ErrorMessage="Required"
                                                            ForeColor="Crimson"
                                                            Display="Dynamic"
                                                            Font-Size="9pt"
                                                            Font-Bold="false"
                                                            ValidationGroup="SUBMIT"
                                                            Enabled="false"> </asp:RequiredFieldValidator>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <%--======================== END CGPA ========================--%>

                                        <%--======================== Passing Year ========================--%>
                                        <div class="row">
                                            <div class="col-sm-12 col-md-12 col-lg-12">
                                                <div class="form-group">
                                                    <label>Passing Year</label>
                                                    <%--<span class="spanAsterisk">*</span>--%>
                                                    <asp:DropDownList ID="ddlGradPassingYear" runat="server" Width="100%" CssClass="form-control">
                                                    </asp:DropDownList>
                                                    <%-- <asp:CompareValidator ID="CompareValidator8" runat="server"
                                                            ControlToValidate="ddlGradPassingYear" ErrorMessage="Required"
                                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <%--======================== END Passing Year ========================--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--======================== END Grad ========================--%>







                        <%--======================== Submit Button ========================--%>
                        <div class="row">
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <div class="panel panel-default">
                                    <div class="panel-body">

                                        <img runat="server" id="imgCtrl" />&nbsp;
                    
                                            <br />
                                        <asp:Panel ID="captchaMsg" runat="server" Visible="false">
                                            <asp:Label ID="lblCaptcha" runat="server"
                                                CssClass="text-warning"
                                                Text="Sorry your text and image didn't match. Please try again."></asp:Label>
                                        </asp:Panel>
                                        <br />
                                        <label>Enter the code shown.</label>
                                        <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control captcha"
                                            Width="25%" onkeydown="return (event.keyCode!=13);" AutoCompleteType="Disabled"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server"
                                            ID="RequiredFieldValidator11"
                                            ValidationGroup="SUBMIT"
                                            ControlToValidate="txtCaptcha"
                                            Display="Dynamic"
                                            Font-Size="9pt"
                                            ForeColor="Crimson"
                                            Font-Bold="false"
                                            ErrorMessage="Captcha Required" />

                                        <asp:Button ID="btnSubmit" runat="server" Text="Next" Style="width: 25%; margin-top: 10px;"
                                            CssClass="btn btn-primary" ValidationGroup="SUBMIT"
                                            OnClick="btnSubmit_Click" />

                                        <%--======================== MessageBottom ========================--%>
                                        <asp:Panel ID="messagePanelBottom" runat="server" Visible="false">
                                            <asp:Label ID="lblMessageBottom" runat="server" Text=""></asp:Label>
                                            <%--<button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                                    <span aria-hidden="true">&times;</span>
                                                </button>--%>
                                        </asp:Panel>
                                        <%--======================== END MessageBottom ========================--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%--======================== END Submit Button ========================--%>



                        <%--<table style="width: 100%" class="table table-condensed table-striped">--%>


                        <%--<tr id="hideCalculater" runat="server">
                       
                            <td colspan="3" style="text-align: left; font-weight: bold; color: crimson;">For English Medium Students please Calculate the Average Grade Points :
                                <asp:Button runat="server" Text="O/A-Level Conversion" OnClick="btnShowPopupClicked" ID="btnsPopup" class="btn btn-info" />
                            </td>
                        </tr>


                        <tr>
                            <td style="width: 15%" class="style_td">Name <span class="spanAsterisk">*</span></td>
                            <td class="auto-style2">
                                <asp:TextBox ID="txtName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </td>
                            <td style="width: 50%">
                                <asp:RequiredFieldValidator runat="server"
                                    ID="NameReq"
                                    ValidationGroup="SUBMIT"
                                    ControlToValidate="txtName"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="Name Required" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%" class="style_td">Date Of Birth <span class="spanAsterisk">*</span></td>
                            <td class="auto-style2">
                                <asp:TextBox ID="txtDateOfBirth" runat="server" Width="100%" CssClass="form-control" placeholder="dd/MM/yyyy"></asp:TextBox>
                            </td>
                            <td style="width: 50%">
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server"
                                    TargetControlID="txtDateOfBirth" Format="dd/MM/yyyy" Animated="true" />
                                <asp:RequiredFieldValidator runat="server"
                                    ID="RequiredFieldValidator4"
                                    ValidationGroup="SUBMIT"
                                    ControlToValidate="txtDateOfBirth"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="Date of Birth Required" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Email <span class="spanAsterisk">*</span></td>
                            <td class="auto-style2">
                                <asp:TextBox ID="txtEmail" runat="server" Width="100%" CssClass="form-control"
                                    TextMode="Email"></asp:TextBox>
                                <span style="color: darkorange; font-size: 9pt;">Please provide a valid email address.
                                    Candidate will not receive any email containing financial or payment information.
                                </span>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator runat="server"
                                    ID="RequiredFieldValidator1"
                                    ValidationGroup="SUBMIT"
                                    ControlToValidate="txtEmail"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="Email Required" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">SMS Mobile No. <span class="spanAsterisk">*</span></td>
                           
                            <td class="auto-style2">

                                <div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtCountryCodeSMSMobile" runat="server" CssClass="form-control" Text="+88" Style="width: 55px;" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div>
                                        <asp:TextBox ID="txtSmsMobile" runat="server" Width="72%" CssClass="form-control" placeholder="01XXXXXXXXX" MaxLength="11"></asp:TextBox>
                                        <span style="color: darkorange; font-size: 9pt;"> eg: +88017XXXXXXXX.
                                            Candidate will not recieve Username and Password 
                                            if number is in wrong format.
                                        </span>
                                    </div>
                                </div>


                            </td>
                            <td>
                                <asp:RequiredFieldValidator runat="server"
                                    ID="RequiredFieldValidator2"
                                    ValidationGroup="SUBMIT"
                                    ControlToValidate="txtSmsMobile"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="SMS Mobile Number Required" />
                                <asp:RegularExpressionValidator runat="server" ID="mobileReg"
                                    ValidationGroup="SUBMIT"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Bold="False"
                                    ErrorMessage="Invalid format"
                                    ControlToValidate="txtSmsMobile"
                                    ValidationExpression="^(?:)?01[15-9]\d{8}$"></asp:RegularExpressionValidator>
                        
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Guardian Mobile No. <span class="spanAsterisk">*</span></td>
                            <td class="auto-style2">
                                <div>
                                    <div style="float: left;">
                                        <asp:TextBox ID="txtCountryCodeGuardianMobile" runat="server" CssClass="form-control" Text="+88" Style="width: 55px;" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div>
                                        <asp:TextBox ID="txtGuardianMobile" runat="server" Width="72%" CssClass="form-control" placeholder="01XXXXXXXXX" MaxLength="11"></asp:TextBox>
                                        <span style="color: darkorange; font-size: 9pt;"> eg: +88017XXXXXXXX.
                                            Guardian will not recieve information if number is 
                                            in wrong format.
                                        </span>
                                    </div>
                                </div>

                            </td>
                            <td>
                                <asp:RequiredFieldValidator runat="server"
                                    ID="RequiredFieldValidator3"
                                    ValidationGroup="SUBMIT"
                                    ControlToValidate="txtGuardianMobile"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="Guardian Mobile Number Required" />
                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1"
                                    ValidationGroup="SUBMIT"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Bold="False"
                                    ErrorMessage="Invalid format"
                                    ControlToValidate="txtGuardianMobile"
                                    ValidationExpression="^(?:)?01[15-9]\d{8}$"></asp:RegularExpressionValidator>
                         
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Gender <span class="spanAsterisk">*</span></td>
                            <td class="auto-style2">
                                <asp:DropDownList ID="ddlGender" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:CompareValidator ID="ddlGenderComV" runat="server"
                                    ControlToValidate="ddlGender" ErrorMessage="Gender required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Quota <span class="spanAsterisk">*</span></td>
                            <td class="auto-style2">
                                <asp:DropDownList ID="ddlQuota" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:CompareValidator ID="ddlQuotaComV" runat="server"
                                    ControlToValidate="ddlQuota" ErrorMessage="Quota Required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr id="hidePassingYear" runat="server">
                            <td class="style_td">HSC/A-Level or Equivalent Passing Year <span class="spanAsterisk">*</span></td>
                            <td class="auto-style2">
                                <asp:DropDownList ID="ddlPassingYear" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:CompareValidator ID="ddlPassingYearComV" runat="server"
                                    ControlToValidate="ddlPassingYear" ErrorMessage="Passing Year Required."
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                            </td>
                        </tr>--%>

                        <%------------------------------------------------ For SSC -----------------------------------------%>
                        <%-- <tr id="hideSSC" runat="server">
                            <td class="style_td">SSC/O-Level/Equivalent<span class="spanAsterisk">*</span></td>
                            <td class="auto-style2">
                                <asp:DropDownList ID="ddlExamTypeSSC" runat="server" Width="100%" CssClass="form-control">
                                    <asp:ListItem Enabled="true" Text="SSC Exam Type" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="SSC" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="O-Level" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Others" Value="5"></asp:ListItem>
                                </asp:DropDownList>



                                <asp:DropDownList ID="ddlGroupSSC" runat="server" Width="100%" CssClass="form-control" Style="margin-top: 7px;">
                                    <asp:ListItem Enabled="true" Text="SSC Group" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Science" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Business" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Humanities" Value="3"></asp:ListItem>
                                </asp:DropDownList>


                                <asp:TextBox ID="txtGPASSC" runat="server" placeholder="SSC GPA (x.xx or x)" Width="85%" CssClass="form-control" Style="margin-top: 7px;"></asp:TextBox>


                                <asp:DropDownList ID="ddlPassYearSSC" runat="server" Width="100%" CssClass="form-control" Style="margin-top: 7px;">
                                    <asp:ListItem Enabled="true" Text="SSC Passing Year" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                                    <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                                    <asp:ListItem Text="2014" Value="2014"></asp:ListItem>
                                    <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
                                </asp:DropDownList>

                            </td>

                            <td>

                                <asp:CompareValidator ID="ddlExamTypeSSCComV" runat="server"
                                    ControlToValidate="ddlExamTypeSSC" ErrorMessage="SSC Exam Type Required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>

                                <br />
                                <br />

                                <asp:CompareValidator ID="ddlGroupSSCComV" runat="server"
                                    ControlToValidate="ddlGroupSSC" ErrorMessage="SSC Group Required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>

                                <br />
                                <br />

                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                    ValidationGroup="SUBMIT"
                                    ControlToValidate="txtGPASSC"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="GPA is in wrong format. Correct formate (x.xx or x)"
                                    ValidationExpression="^\d{0,2}(\.\d{1,2})?$">
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="txtGPASSC_ReqV" runat="server"
                                    ControlToValidate="txtGPASSC"
                                    ErrorMessage="SSC GPA Required"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    Font-Bold="false"
                                    ValidationGroup="SUBMIT">
                                </asp:RequiredFieldValidator>

                                <br />
                                <br />

                                <asp:CompareValidator ID="ddlPassYearSSCComV" runat="server"
                                    ControlToValidate="ddlPassYearSSC" ErrorMessage="SSC Passing Year Required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>

                            </td>

                        </tr>--%>

                        <%------------------------------------------------ END For SSC -----------------------------------------%>




                        <%------------------------------------------------ For HSC -----------------------------------------%>

                        <%--<tr id="hideHSC" runat="server">
                            <td class="style_td">HSC/A-Level/Equivalent<span class="spanAsterisk">*</span></td>



                            <td class="auto-style2">
                                <asp:DropDownList ID="ddlExamTypeHSC" runat="server" Width="100%" CssClass="form-control">
                                    <asp:ListItem Enabled="true" Text="HSC Exam Type" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="HSC" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="A-Level" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Others" Value="5"></asp:ListItem>
                                </asp:DropDownList>



                                <asp:DropDownList ID="ddlGroupHSC" runat="server" Width="100%" CssClass="form-control" Style="margin-top: 7px;">
                                    <asp:ListItem Enabled="true" Text="HSC Group" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Science" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Business" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Humanities" Value="3"></asp:ListItem>
                                </asp:DropDownList>


                                <asp:TextBox ID="txtGPAHSC" runat="server" placeholder="HSC GPA (x.xx or x)" Width="85%" CssClass="form-control" Style="margin-top: 7px;"></asp:TextBox>


                                <asp:DropDownList ID="ddlPassYearHSC" runat="server" Width="100%" CssClass="form-control" Style="margin-top: 7px;">
                                    <asp:ListItem Enabled="true" Text="HSC Passing Year" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                                    <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
                                </asp:DropDownList>

                            </td>

                            <td>

                                <asp:CompareValidator ID="ddlExamTypeHSCComV" runat="server"
                                    ControlToValidate="ddlExamTypeHSC" ErrorMessage="HSC Exam Type Required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>

                                <br />
                                <br />

                                <asp:CompareValidator ID="ddlGroupHSCComV" runat="server"
                                    ControlToValidate="ddlGroupHSC" ErrorMessage="HSC Group Required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>

                                <br />
                                <br />

                                <asp:RegularExpressionValidator ID="YourRegularExpressionValidator" runat="server"
                                    ValidationGroup="SUBMIT"
                                    ControlToValidate="txtGPAHSC"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    ForeColor="Crimson"
                                    Font-Bold="false"
                                    ErrorMessage="GPA is in wrong format. Correct format (x.xx or x)"
                                    ValidationExpression="^\d{0,2}(\.\d{1,2})?$">
                                </asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                    ControlToValidate="txtGPAHSC"
                                    ErrorMessage="HSC GPA Required"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    Font-Bold="false"
                                    ValidationGroup="SUBMIT">
                                </asp:RequiredFieldValidator>

                                <br />
                                <br />

                                <asp:CompareValidator ID="ddlPassYearHSCComV" runat="server"
                                    ControlToValidate="ddlPassYearHSC" ErrorMessage="HSC Passing Year Required"
                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>

                            </td>



                        </tr>--%>

                        <%------------------------------------------------ END For HSC -----------------------------------------%>
                        <%--</table>--%>









                        <%------------------------------------------------ POPUP Modal -----------------------------------------%>



                        <%--<asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnPopUp" CancelControlID="btnClose" BackgroundCssClass="modalBackground">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:Panel runat="server" ID="pnPopUp" Style="display: none;">
                        <asp:UpdatePanel ID="UpdatePanel04" runat="server">
                            <ContentTemplate>
                                <div style="height: auto; width: auto; padding: 5px; margin: 5px; background-color: Window;">
                                    <fieldset style="padding: 10px; margin: 5px; border-color: lightgreen; max-height: 500px; overflow: auto;">
                                        <legend style="margin-bottom: 6px;">O/A-Level Conversion</legend>
                                        <div class="loadedArea">


                                            <div>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <span style="color: darkred">Grade Points Calculation process for <strong>English Medium Students</strong> - Calculate the Average Grade Points using the table considering all subjects.</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table style="width: 30%; color: darkred" class="table table-bordered">
                                                                <tr>
                                                                    <th>Grade</th>
                                                                    <td>A*/A</td>
                                                                    <td>B</td>
                                                                    <td>C</td>
                                                                    <td>D</td>
                                                                </tr>
                                                                <tr>
                                                                    <th>GPA</th>
                                                                    <td>5.00</td>
                                                                    <td>4.00</td>
                                                                    <td>3.50</td>
                                                                    <td>3.00</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>


                                            <div class="row">
                                                <div class="col-md-12">

                                                    <div class="col-md-6">
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading style_thead">
                                                                O-Level
                                                            </div>
                                                            <div class="panel-body panelBody_edu_marginBottom">
                                                                <table style="width: 100%" class="table table-condensed table-striped">
                                                                    <tr>
                                                                        <td style="width: 30%" class="style_td">Subject-1 : </td>
                                                                        <td style="width: 70%">
                                                                            <asp:DropDownList ID="ddlOLevelSubject1" runat="server" CssClass="form-control">
                                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:CompareValidator ID="ddlOLevelSubject1_CV" runat="server"
                                                                                ControlToValidate="ddlOLevelSubject1"
                                                                                ErrorMessage="Subject-1 Grade Required"
                                                                                Display="Dynamic"
                                                                                Font-Size="9pt"
                                                                                ForeColor="Crimson"
                                                                                ValueToCompare="-1"
                                                                                Operator="NotEqual"
                                                                                ValidationGroup="btnOLevelCalculate">
                                                                            </asp:CompareValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style_td">Subject-2 : </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlOLevelSubject2" runat="server" CssClass="form-control">
                                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:CompareValidator ID="ddlOLevelSubject2_CV" runat="server"
                                                                                ControlToValidate="ddlOLevelSubject2"
                                                                                ErrorMessage="Subject-2 Grade Required"
                                                                                Display="Dynamic"
                                                                                Font-Size="9pt"
                                                                                ForeColor="Crimson"
                                                                                ValueToCompare="-1"
                                                                                Operator="NotEqual"
                                                                                ValidationGroup="btnOLevelCalculate">
                                                                            </asp:CompareValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style_td">Subject-3 : </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlOLevelSubject3" runat="server" CssClass="form-control">
                                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:CompareValidator ID="ddlOLevelSubject3_CV" runat="server"
                                                                                ControlToValidate="ddlOLevelSubject3"
                                                                                ErrorMessage="Subject-3 Grade Required"
                                                                                Display="Dynamic"
                                                                                Font-Size="9pt"
                                                                                ForeColor="Crimson"
                                                                                ValueToCompare="-1"
                                                                                Operator="NotEqual"
                                                                                ValidationGroup="btnOLevelCalculate">
                                                                            </asp:CompareValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style_td">Subject-4 : </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlOLevelSubject4" runat="server" CssClass="form-control">
                                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:CompareValidator ID="ddlOLevelSubject4_CV" runat="server"
                                                                                ControlToValidate="ddlOLevelSubject4"
                                                                                ErrorMessage="Subject-4 Grade Required"
                                                                                Display="Dynamic"
                                                                                Font-Size="9pt"
                                                                                ForeColor="Crimson"
                                                                                ValueToCompare="-1"
                                                                                Operator="NotEqual"
                                                                                ValidationGroup="btnOLevelCalculate">
                                                                            </asp:CompareValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style_td">Subject-5 : </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlOLevelSubject5" runat="server" CssClass="form-control">
                                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:CompareValidator ID="ddlOLevelSubject5_CV" runat="server"
                                                                                ControlToValidate="ddlOLevelSubject5"
                                                                                ErrorMessage="Subject-5 Grade Required"
                                                                                Display="Dynamic"
                                                                                Font-Size="9pt"
                                                                                ForeColor="Crimson"
                                                                                ValueToCompare="-1"
                                                                                Operator="NotEqual"
                                                                                ValidationGroup="btnOLevelCalculate">
                                                                            </asp:CompareValidator>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style_td"></td>
                                                                        <td>
                                                                            <asp:Button ID="btnCalculateOLevel" runat="server" Text="Calculate" OnClick="btnCalculateOLevel_Click" ValidationGroup="btnOLevelCalculate" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style_td">Converted Result : </td>
                                                                        <td>
                                                                            <asp:Label ID="lblOLevelResult" runat="server" Text="" Style="font-weight: bold; color: crimson;"></asp:Label>
                                                                        </td>
                                                                    </tr>

                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="panel panel-default">
                                                            <div class="panel-heading style_thead">
                                                                A-Level
                                                            </div>
                                                            <div class="panel-body panelBody_edu_marginBottom">
                                                                <table style="width: 100%" class="table table-condensed table-striped">
                                                                    <tr>
                                                                        <td style="width: 30%" class="style_td">Subject-1 : </td>
                                                                        <td style="width: 70%">
                                                                            <asp:DropDownList ID="ddlALevelSubject1" runat="server" CssClass="form-control">
                                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:CompareValidator ID="ddlALevelSubject1_CV" runat="server"
                                                                                ControlToValidate="ddlALevelSubject1"
                                                                                ErrorMessage="Subject-1 Grade Required"
                                                                                Display="Dynamic"
                                                                                Font-Size="9pt"
                                                                                ForeColor="Crimson"
                                                                                ValueToCompare="-1"
                                                                                Operator="NotEqual"
                                                                                ValidationGroup="btnALevelCalculate">
                                                                            </asp:CompareValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style_td">Subject-2 : </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlALevelSubject2" runat="server" CssClass="form-control">
                                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:CompareValidator ID="ddlALevelSubject2_CV" runat="server"
                                                                                ControlToValidate="ddlALevelSubject2"
                                                                                ErrorMessage="Subject-1 Grade Required"
                                                                                Display="Dynamic"
                                                                                Font-Size="9pt"
                                                                                ForeColor="Crimson"
                                                                                ValueToCompare="-1"
                                                                                Operator="NotEqual"
                                                                                ValidationGroup="btnALevelCalculate">
                                                                            </asp:CompareValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style_td"></td>
                                                                        <td>
                                                                            <asp:Button ID="btnCalculateALevel" runat="server" Text="Calculate" OnClick="btnCalculateALevel_Click" ValidationGroup="btnALevelCalculate" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="style_td">Converted Result : </td>
                                                                        <td>
                                                                            <asp:Label ID="lblALevelResult" runat="server" Text="" Style="font-weight: bold; color: crimson;"></asp:Label>
                                                                        </td>
                                                                    </tr>

                                                                </table>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                      
                                            </div>



                                        </div>
                                        <asp:Button ID="btnClose" runat="server" Text="Close" class="btn btn-default pull-right" OnClick="btnCancleModel_Click" />
                                    </fieldset>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                        --%>


                        <%------------------------------------------------ END POPUP Modal -----------------------------------------%>







                        <%-- ---------------------------------------------------------------------------------------------------------------------------------- --%>
                        <%--<table style="width: 100%; margin-bottom: -4%;" class="table">
                        <tr>
                            <td>
                                <span style="color: darkred">Grade Points Calculation process for <strong>English Medium Students</strong> - Calculate the Average Grade Points using
                                 the table considering all subjects.
                                <br />
                                    <span class="glyphicon glyphicon-ok">&nbsp;</span>Average points required - 15 in 5 subject (O level)
                                <br />
                                    <span class="glyphicon glyphicon-ok">&nbsp;</span>Average points required - 6 in 2 subject (A level)</span>
                                <br />
                                <table style="width: 10%; color: darkred" class="table table-bordered">
                                    <tr>
                                        <th>Grade</th>
                                        <td>A</td>
                                        <td>B</td>
                                        <td>C</td>
                                        <td>D</td>
                                        <td>E</td>
                                    </tr>
                                    <tr>
                                        <th>GPA</th>
                                        <td>5</td>
                                        <td>4</td>
                                        <td>3.5</td>
                                        <td>2</td>
                                        <td>1</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <hr />--%>
                        <%-- ---------------------------------------------------------------------------------------------------------------------------------- --%>

                        <%--<div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-primary">
                                <div class="panel-heading style_thead">
                                    Secondary School / O-Level / Dakhil
                                </div>
                                <div class="panel-body panelBody_edu_marginBottom">
                                    <asp:UpdatePanel ID="updatePanelSecondary" runat="server">
                                        <ContentTemplate>
                                            <table style="width: 100%" class="table table-condensed table-striped">
                                                <tr>
                                                    <td class="style_td" style="width: 10%; vertical-align: middle">Year of Passing<span class="spanAsterisk">*</span></td>
                                                    <td style="width: 18%;">
                                                        <asp:DropDownList ID="ddlSec_PassingYear" runat="server" Width="90%" CssClass="form-control"></asp:DropDownList>
                                                        <asp:CompareValidator ID="ddlSec_PassingYearCV" runat="server" Display="Dynamic"
                                                            ControlToValidate="ddlSec_PassingYear" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </td>
                                                    <td class="style_td" style="width: 7%; vertical-align: middle">Exam Type<span class="spanAsterisk">*</span></td>
                                                    <td style="width: 18%;">
                                                        <asp:DropDownList ID="ddlSec_ExamType" runat="server" Width="90%" CssClass="form-control"></asp:DropDownList>
                                                        <asp:CompareValidator ID="ddlSec_ExamTypeCV" runat="server" Display="Dynamic"
                                                            ControlToValidate="ddlSec_ExamType" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </td>
                                                    <td class="style_td" style="width: 2%; vertical-align: middle">Board<span class="spanAsterisk">*</span></td>
                                                    <td style="width: 18%;">
                                                        <asp:DropDownList ID="ddlSec_EducationBrd" runat="server" Width="90%" CssClass="form-control"></asp:DropDownList>
                                                        <asp:CompareValidator ID="ddlSec_EducationBrdCV" runat="server" Display="Dynamic"
                                                            ControlToValidate="ddlSec_EducationBrd" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style_td" style="vertical-align: middle">Institute<span class="spanAsterisk">*</span></td>
                                                    <td>
                                                        <asp:TextBox ID="txtSecInstitute" runat="server" Width="96%" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                    <td class="style_td" style="vertical-align: middle">Roll No.<span class="spanAsterisk">*</span></td>
                                                    <td>
                                                        <asp:TextBox ID="txtSecRollNo" runat="server" Width="90%" CssClass="form-control"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="txtSecRollNoRFV" runat="server" Display="Dynamic"
                                                            ControlToValidate="txtSecRollNo" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                            ValidationGroup="SUBMIT"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td class="style_td" style="vertical-align: middle">Group/Subject<span class="spanAsterisk">*</span></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlSec_GrpOrSub" runat="server" Width="90%" CssClass="form-control"></asp:DropDownList>
                                                        <asp:CompareValidator ID="ddlSec_GrpOrSubCV" runat="server" Display="Dynamic"
                                                            ControlToValidate="ddlSec_GrpOrSub" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style_td" style="vertical-align: middle">Division/Class<span class="spanAsterisk">*</span></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlSec_DivClass" runat="server" Width="90%" CssClass="form-control"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlSec_DivClass_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="ddlSec_DivClassCV" runat="server" Display="Dynamic"
                                                            ControlToValidate="ddlSec_DivClass" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </td>
                                                    <td class="style_td" style="vertical-align: middle">CGPA<span class="spanAsterisk">*</span></td>
                                                    <td>
                                                        <asp:TextBox ID="txtSecCgpa" runat="server" Width="90%" CssClass="form-control" Enabled="false" onkeypress="return onlyDotsAndNumbers(event)"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="txtSecCgpaRFV" runat="server" Display="Dynamic"
                                                            ControlToValidate="txtSecCgpa" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                            ValidationGroup="SUBMIT"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td class="style_td" style="vertical-align: middle">Grade</td>
                                                    <td>
                                                        <asp:TextBox ID="txtSecGrade" runat="server" Width="90%" CssClass="form-control"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="txtSecGradeFTBE" runat="server"
                                                            TargetControlID="txtSecGrade" ValidChars="ABCDEF+-" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>--%>
                        <%-- END SECONDARY / O-Level --%>

                        <%--<div class="row">
                        <div class="col-md-12">
                            <div class="panel panel-primary">
                                <div class="panel-heading style_thead">
                                    Higher Secondary School / A-Level / Alim / Diploma
                                </div>
                                <div class="panel-body panelBody_edu_marginBottom">
                                    <asp:UpdatePanel ID="updatePanelHigherSecondary" runat="server">
                                        <ContentTemplate>
                                            <table style="width: 100%" class="table table-condensed table-striped">
                                                <tr>
                                                    <td class="style_td" style="width: 10%; vertical-align: middle">Year of Passing<span class="spanAsterisk">*</span></td>
                                                    <td style="width: 18%;">
                                                        <asp:DropDownList ID="ddlHigherSec_PassingYear" runat="server" Width="90%" CssClass="form-control"></asp:DropDownList>
                                                        <asp:CompareValidator ID="ddlHigherSec_PassingYearCV" runat="server" Display="Dynamic"
                                                            ControlToValidate="ddlHigherSec_PassingYear" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </td>
                                                    <td class="style_td" style="width: 7%; vertical-align: middle">Exam Type<span class="spanAsterisk">*</span></td>
                                                    <td style="width: 18%;">
                                                        <asp:DropDownList ID="ddlHigherSec_ExamType" runat="server" Width="90%" CssClass="form-control"></asp:DropDownList>
                                                        <asp:CompareValidator ID="ddlHigherSec_ExamTypeCV" runat="server" Display="Dynamic"
                                                            ControlToValidate="ddlHigherSec_ExamType" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </td>
                                                    <td class="style_td" style="width: 2%; vertical-align: middle">Board<span class="spanAsterisk">*</span></td>
                                                    <td style="width: 18%;">
                                                        <asp:DropDownList ID="ddlHigherSec_EducationBrd" runat="server" Width="90%" CssClass="form-control"></asp:DropDownList>
                                                        <asp:CompareValidator ID="ddlHigherSec_EducationBrdCV" runat="server" Display="Dynamic"
                                                            ControlToValidate="ddlHigherSec_EducationBrd" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style_td" style="vertical-align: middle">Institute<span class="spanAsterisk">*</span></td>
                                                    <td>
                                                        <asp:TextBox ID="txtHighSecInstitute" runat="server" Width="96%" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                    <td class="style_td" style="vertical-align: middle">Roll No.<span class="spanAsterisk">*</span></td>
                                                    <td>
                                                        <asp:TextBox ID="txtHighSecRollNo" runat="server" Width="90%" CssClass="form-control"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="txtHighSecRollNoRFV" runat="server" Display="Dynamic"
                                                            ControlToValidate="txtHighSecRollNo" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                            ValidationGroup="SUBMIT"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td class="style_td" style="vertical-align: middle">Group/Subject<span class="spanAsterisk">*</span></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlHigherSec_GrpOrSub" runat="server" Width="90%" CssClass="form-control"></asp:DropDownList>
                                                        <asp:CompareValidator ID="ddlHigherSec_GrpOrSubCV" runat="server" Display="Dynamic"
                                                            ControlToValidate="ddlHigherSec_GrpOrSub" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="style_td" style="vertical-align: middle">Division/Class<span class="spanAsterisk">*</span></td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlHigherSec_DivClass" runat="server" Width="90%" CssClass="form-control"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlHigherSec_DivClass_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:CompareValidator ID="ddlHigherSec_DivClassCV" runat="server" Display="Dynamic"
                                                            ControlToValidate="ddlHigherSec_DivClass" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                    </td>
                                                    <td class="style_td" style="vertical-align: middle">CGPA<span class="spanAsterisk">*</span></td>
                                                    <td>
                                                        <asp:TextBox ID="txtHigSecCgpa" runat="server" Width="90%" CssClass="form-control" Enabled="false" onkeypress="return onlyDotsAndNumbers(event)"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="txtHigSecCgpaRFV" runat="server" Display="Dynamic"
                                                            ControlToValidate="txtHigSecCgpa" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                            ValidationGroup="SUBMIT"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td class="style_td" style="vertical-align: middle">Grade</td>
                                                    <td>
                                                        <asp:TextBox ID="txtHigSecGrade" runat="server" Width="90%" CssClass="form-control"></asp:TextBox>
                                                        <ajaxToolkit:FilteredTextBoxExtender ID="txtHigSecGradeFTBE" runat="server"
                                                            TargetControlID="txtHigSecGrade" ValidChars="ABCDEF+-" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>--%>
                        <%-- END HIGHER SECONDARY / A-Level --%>
                        <%--<asp:HiddenField ID="hiddenFieldUndergrad" runat="server" />--%>

                        <%--<asp:Panel ID="undergraduateInfoPanel" runat="server" Visible="false">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="panel panel-primary">
                                    <div class="panel-heading style_thead">
                                        Undergraduate
                                    </div>
                                    <div class="panel-body panelBody_edu_marginBottom">
                                        <asp:UpdatePanel ID="updatePanelUndergraduate" runat="server">
                                            <ContentTemplate>
                                                <table style="width: 100%" class="table table-condensed table-striped">
                                                    <tr>
                                                        <td class="style_td" style="width: 10%; vertical-align: middle">Year of Passing<span class="spanAsterisk">*</span></td>
                                                        <td style="width: 18%;">
                                                            <asp:DropDownList ID="ddlUndergrad_PassingYear" runat="server" Width="90%" CssClass="form-control"></asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlUndergrad_PassingYearCV" runat="server" Display="Dynamic"
                                                                ControlToValidate="ddlUndergrad_PassingYear" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                                ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                        </td>
                                                        <td class="style_td" style="width: 7%; vertical-align: middle">Program<span class="spanAsterisk">*</span></td>
                                                        <td style="width: 18%;">
                                                            <asp:DropDownList ID="ddlUndergrad_Programs" runat="server" Width="90%" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlUndergrad_Programs_SelectedIndexChanged"></asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlUndergrad_ProgramsCV" runat="server" Display="Dynamic"
                                                                ControlToValidate="ddlUndergrad_Programs" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                                ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                        </td>
                                                        <td class="style_td" style="width: 2%; vertical-align: middle">Other</td>
                                                        <td style="width: 18%;">
                                                            <asp:TextBox ID="txtUndergrad_ProgramOther" runat="server" Width="90%" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="txtUndergrad_ProgramOtherRFV" runat="server" Display="Dynamic"
                                                                ControlToValidate="txtUndergrad_ProgramOther" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                                ValidationGroup="SUBMIT"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td" style="vertical-align: middle">Institute<span class="spanAsterisk">*</span></td>
                                                        <td colspan="3">
                                                            <asp:TextBox ID="txtUndergrad_Institute" runat="server" Width="96%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                        <td class="style_td" style="vertical-align: middle">Group/Subject<span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlUndergrad_GroupSubject" runat="server" Width="90%" CssClass="form-control"></asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlUndergrad_GroupSubjectCV" runat="server" Display="Dynamic"
                                                                ControlToValidate="ddlUndergrad_GroupSubject" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                                ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td" style="vertical-align: middle">Division/Class</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlUndergrad_DivisionClass" runat="server" Width="90%" CssClass="form-control"
                                                                AutoPostBack="true" Enabled="false">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="style_td" style="vertical-align: middle">CGPA<span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtUndergrad_Cgpa" runat="server" Width="90%" CssClass="form-control" onkeypress="return onlyDotsAndNumbers(event)"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="txtUndergrad_CgpaRFV" runat="server" Display="Dynamic"
                                                                ControlToValidate="txtUndergrad_Cgpa" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                                ValidationGroup="SUBMIT"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="style_td" style="vertical-align: middle">Grade</td>
                                                        <td>
                                                            <asp:TextBox ID="txtUndergrad_Grade" runat="server" Width="90%" CssClass="form-control"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="txtUndergrad_GradeFTBE" runat="server"
                                                                TargetControlID="txtUndergrad_Grade" ValidChars="ABCDEF+-" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table style="width: 100%" class="table table-condensed table-striped">
                                                    <td class="style_td" style="width: 10%; vertical-align: middle">Years of Work Experience<span class="spanAsterisk">*</span></td>
                                                        <td style="width: 18%;">
                                                            <asp:DropDownList ID="ddlYearsOfExp" runat="server" Width="50%" CssClass="form-control"></asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlYearsOfExpCompareValidator" runat="server" Display="Dynamic"
                                                                ControlToValidate="ddlYearsOfExp" ErrorMessage="Required" ForeColor="Crimson" Font-Size="10pt"
                                                                ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                                        </td>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>--%>
                        <%-- END UNDERGRAD --%>

                        <%--<table style="width: 100%">
                        <tr>
                            <td>
                                <img runat="server" id="imgCtrl" />&nbsp;
                                <asp:ImageButton ID="btnReLoadCaptcha" runat="server"
                                    Height="30"
                                    Width="30"
                                    ToolTip="Reload captcha"
                                    ImageUrl="~/Images/AppImg/reload6.png" />
                                <br />
                                <asp:Panel ID="captchaMsg" runat="server" Visible="false">
                                    <asp:Label ID="lblCaptcha" runat="server"
                                        CssClass="text-warning"
                                        Text="Sorry your text and image didn't match. Please try again."></asp:Label>
                                </asp:Panel>
                                <br />
                                <label>Enter the code shown.</label>
                                <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control remarks"
                                    Width="25%"></asp:TextBox>
                                <br />
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="2" class="auto-style1">
                                <asp:UpdatePanel ID="updatePanel_EligibleMessage" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblEligibleMsg" runat="server" Font-Bold="true"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSubmit" runat="server" Text="Next"
                                    CssClass="btn btn-primary button-blue" ValidationGroup="SUBMIT"
                                    OnClick="btnSubmit_Click" />
                            </td>
                            <td></td>
                        </tr>
                    </table>--%>
                    </div>
                </div>

            </div>


        </ContentTemplate>
    </asp:UpdatePanel>




    <%------------------------------------------------ POPUP Modal -----------------------------------------%>



    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnPopUp" CancelControlID="btnClose" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel runat="server" ID="pnPopUp" Style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel04" runat="server">
            <ContentTemplate>
                <div style="height: auto; width: auto; padding: 5px; margin: 5px; background-color: Window;">
                    <fieldset style="padding: 10px; margin: 5px; border-color: lightgreen; max-height: 500px; overflow: auto;">
                        <legend style="margin-bottom: 6px;">O/A-Level Grade Conversion
                        <asp:LinkButton ID="LinkButton2" runat="server" OnClick="btnCancleModel_Click"><span class="glyphicon" style="float: right;margin-right: 22px;color: slategray;font-size: 20px;">&#xe014;</span></span></asp:LinkButton>
                        </legend>
                        <div class="loadedArea">


                            <div>
                                <table>
                                    <tr>
                                        <td>
                                            <span style="color: darkred">Grade Points Calculation process for <strong>English Medium Students</strong> - Calculate the Average Grade Points using the table considering all subjects.</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 30%; color: darkred" class="table table-bordered">
                                                <tr>
                                                    <th>Grade</th>
                                                    <td>A*/A</td>
                                                    <td>B</td>
                                                    <td>C</td>
                                                    <td>D</td>
                                                    <td>E</td>
                                                </tr>
                                                <tr>
                                                    <th>Point</th>
                                                    <td>5.00</td>
                                                    <td>4.00</td>
                                                    <td>3.50</td>
                                                    <td>3.00</td>
                                                    <td>0.00</td>
                                                    <%--<td>2.50</td>--%>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>


                            <div class="row">
                                <div class="col-md-12">

                                    <div class="col-md-6">
                                        <div class="panel panel-default">
                                            <div class="panel-heading style_thead">
                                                O-Level
                                            </div>
                                            <div class="panel-body panelBody_edu_marginBottom">
                                                <table style="width: 100%" class="table table-condensed table-striped">
                                                    <tr>
                                                        <td style="width: 30%" class="style_td">Subject-1 : </td>
                                                        <td style="width: 70%">
                                                            <asp:DropDownList ID="ddlOLevelSubject1" runat="server" CssClass="form-control">
                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                                                <%--<asp:ListItem Text="E" Value="2.25"></asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlOLevelSubject1_CV" runat="server"
                                                                ControlToValidate="ddlOLevelSubject1"
                                                                ErrorMessage="Subject-1 Grade Required"
                                                                Display="Dynamic"
                                                                Font-Size="9pt"
                                                                ForeColor="Crimson"
                                                                ValueToCompare="-1"
                                                                Operator="NotEqual"
                                                                ValidationGroup="btnOALevelCalculate">
                                                            </asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Subject-2 : </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlOLevelSubject2" runat="server" CssClass="form-control">
                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                                                <%--<asp:ListItem Text="E" Value="2.25"></asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlOLevelSubject2_CV" runat="server"
                                                                ControlToValidate="ddlOLevelSubject2"
                                                                ErrorMessage="Subject-2 Grade Required"
                                                                Display="Dynamic"
                                                                Font-Size="9pt"
                                                                ForeColor="Crimson"
                                                                ValueToCompare="-1"
                                                                Operator="NotEqual"
                                                                ValidationGroup="btnOALevelCalculate">
                                                            </asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Subject-3 : </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlOLevelSubject3" runat="server" CssClass="form-control">
                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                                                <%--<asp:ListItem Text="E" Value="2.25"></asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlOLevelSubject3_CV" runat="server"
                                                                ControlToValidate="ddlOLevelSubject3"
                                                                ErrorMessage="Subject-3 Grade Required"
                                                                Display="Dynamic"
                                                                Font-Size="9pt"
                                                                ForeColor="Crimson"
                                                                ValueToCompare="-1"
                                                                Operator="NotEqual"
                                                                ValidationGroup="btnOALevelCalculate">
                                                            </asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Subject-4 : </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlOLevelSubject4" runat="server" CssClass="form-control">
                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                                                <%--<asp:ListItem Text="E" Value="2.25"></asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlOLevelSubject4_CV" runat="server"
                                                                ControlToValidate="ddlOLevelSubject4"
                                                                ErrorMessage="Subject-4 Grade Required"
                                                                Display="Dynamic"
                                                                Font-Size="9pt"
                                                                ForeColor="Crimson"
                                                                ValueToCompare="-1"
                                                                Operator="NotEqual"
                                                                ValidationGroup="btnOALevelCalculate">
                                                            </asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Subject-5 : </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlOLevelSubject5" runat="server" CssClass="form-control">
                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                                                <%--<asp:ListItem Text="E" Value="2.25"></asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlOLevelSubject5_CV" runat="server"
                                                                ControlToValidate="ddlOLevelSubject5"
                                                                ErrorMessage="Subject-5 Grade Required"
                                                                Display="Dynamic"
                                                                Font-Size="9pt"
                                                                ForeColor="Crimson"
                                                                ValueToCompare="-1"
                                                                Operator="NotEqual"
                                                                ValidationGroup="btnOALevelCalculate">
                                                            </asp:CompareValidator>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">O-Level Points : </td>
                                                        <td>
                                                            <asp:Label ID="lblOLevelResult" runat="server" Text="" Style="font-weight: bold; color: crimson;"></asp:Label>
                                                            <asp:HiddenField ID="hfOLevelConvertedSscGPA" runat="server" />
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="panel panel-default">
                                            <div class="panel-heading style_thead">
                                                A-Level
                                            </div>
                                            <div class="panel-body panelBody_edu_marginBottom">
                                                <table style="width: 100%" class="table table-condensed table-striped">
                                                    <tr>
                                                        <td style="width: 30%" class="style_td">Subject-1 : </td>
                                                        <td style="width: 70%">
                                                            <asp:DropDownList ID="ddlALevelSubject1" runat="server" CssClass="form-control">
                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                                                <%--<asp:ListItem Text="E" Value="2.25"></asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlALevelSubject1_CV" runat="server"
                                                                ControlToValidate="ddlALevelSubject1"
                                                                ErrorMessage="Subject-1 Grade Required"
                                                                Display="Dynamic"
                                                                Font-Size="9pt"
                                                                ForeColor="Crimson"
                                                                ValueToCompare="-1"
                                                                Operator="NotEqual"
                                                                ValidationGroup="btnOALevelCalculate">
                                                            </asp:CompareValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Subject-2 : </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlALevelSubject2" runat="server" CssClass="form-control">
                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                                                <%--<asp:ListItem Text="E" Value="2.25"></asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlALevelSubject2_CV" runat="server"
                                                                ControlToValidate="ddlALevelSubject2"
                                                                ErrorMessage="Subject-1 Grade Required"
                                                                Display="Dynamic"
                                                                Font-Size="9pt"
                                                                ForeColor="Crimson"
                                                                ValueToCompare="-1"
                                                                Operator="NotEqual"
                                                                ValidationGroup="btnOALevelCalculate">
                                                            </asp:CompareValidator>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td class="style_td">A-Level Points : </td>
                                                        <td>
                                                            <asp:Label ID="lblALevelResult" runat="server" Text="" Style="font-weight: bold; color: crimson;"></asp:Label>
                                                            <asp:HiddenField ID="hfALevelConvertedHscGPA" runat="server" />
                                                        </td>
                                                    </tr>

                                                </table>
                                            </div>
                                        </div>

                                        <div class="panel panel-default">
                                            <div class="panel-body panelBody_edu_marginBottom">
                                                <div>
                                                    <b>Total Points : </b>
                                                    <asp:Label ID="lblTotalPoints" runat="server" Style="font-weight: bold; color: crimson;"></asp:Label>
                                                    <br />
                                                    <asp:Label ID="lblMassage" runat="server" ForeColor="Crimson"></asp:Label>
                                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                                </div>
                                                <div style="margin-top: 15px; padding-bottom: 14px;">
                                                    <asp:Button ID="btnCalculateALevel" runat="server" Text="Calculate" OnClick="btnCalculateOAndALevel_Click" ValidationGroup="btnOALevelCalculate" />
                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                </div>
                                <%-- END COL-MD-12 --%>
                            </div>



                        </div>
                        <asp:Button ID="btnClose" runat="server" Text="Close" class="btn btn-default pull-right" OnClick="btnCancleModel_Click" />
                    </fieldset>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>



    <%------------------------------------------------ END POPUP Modal -----------------------------------------%>



    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="upAll" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnSubmit" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnSubmit" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>


</asp:Content>
