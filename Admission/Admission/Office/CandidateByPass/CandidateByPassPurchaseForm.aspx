<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CandidateByPassPurchaseForm.aspx.cs" Inherits="Admission.Admission.Office.CandidateByPass.CandidateByPassPurchaseForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .modern-card {
            background: white;
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-md);
            overflow: hidden;
            transition: var(--transition);
            border: 1px solid var(--border-color);
            margin-bottom: 1.5rem;
        }

        .modern-card:hover {
            box-shadow: var(--shadow-lg);
        }

        .card-header-modern {
            background: linear-gradient(145deg, var(--primary), var(--secondary));
            color: white;
            padding: 1.5rem;
            font-weight: 600;
            font-size: 1.1rem;
            text-align: center;
            border: none;
        }

        .card-body-modern {
            padding: 2rem;
        }

        /* Form Styling */
        .form-group-modern {
            margin-bottom: 1.5rem;
        }

        .form-group-modern label {
            display: block;
            font-weight: 600;
            color: var(--dark);
            margin-bottom: 0.5rem;
            font-size: 0.9rem;
        }

        .form-control {
            border: 2px solid var(--border-color);
            border-radius: var(--radius-md);
            padding: 0.75rem 1rem;
            transition: var(--transition);
            font-size: 0.95rem;
            width: 100%;
        }

        .form-control:focus {
            border-color: var(--accent);
            box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
            outline: none;
        }

        /* Alert Styling */
        .alert-modern {
            border-radius: var(--radius-md);
            padding: 1rem 1.5rem;
            border: none;
            box-shadow: var(--shadow-sm);
            margin-bottom: 1.5rem;
        }

        .alert-danger-modern {
            background: #FEE2E2;
            color: var(--danger);
            border-left: 4px solid var(--danger);
        }

        .alert-info-modern {
            background: #DBEAFE;
            color: var(--accent);
            border-left: 4px solid var(--accent);
        }

        .alert-warning-modern {
            background: #FEF3C7;
            color: var(--warning);
            border-left: 4px solid var(--warning);
        }

        /* Required Field Asterisk */
        .text-danger, .spanAsterisk {
            color: var(--danger);
            font-size: 1rem;
            margin-left: 0.25rem;
        }

        /* Button Styling */
        .btn-modern {
            border-radius: var(--radius-md);
            font-weight: 500;
            padding: 0.75rem 1.5rem;
            transition: var(--transition);
            border: none;
            cursor: pointer;
            display: inline-flex;
            align-items: center;
            gap: 0.5rem;
            position: relative;
            overflow: hidden;
        }

        .btn-modern::before {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: rgba(255, 255, 255, 0.2);
            transition: var(--transition);
        }

        .btn-modern:hover::before {
            left: 100%;
        }

        .btn-primary-modern {
            background: linear-gradient(135deg, var(--accent), var(--secondary));
            color: white;
        }

        .btn-primary-modern:hover {
            transform: translateY(-2px);
            box-shadow: var(--shadow-md);
            color: white;
        }

        /* Education Section Layout */
        .education-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 1.5rem;
            margin-bottom: 1.5rem;
        }

        .education-card {
            background: white;
            border: 2px solid var(--border-color);
            border-radius: var(--radius-lg);
            padding: 1.5rem;
            transition: var(--transition);
        }

        .education-card:hover {
            border-color: var(--accent-light);
            box-shadow: var(--shadow-md);
        }

        .education-header {
            background: linear-gradient(135deg, var(--light), white);
            padding: 1rem;
            border-radius: var(--radius-md);
            margin-bottom: 1rem;
            font-weight: 600;
            color: var(--primary);
            text-align: center;
            border: 1px solid var(--border-color);
        }

        .form-field {
            margin-bottom: 1rem;
        }

        .form-field label {
            display: block;
            font-weight: 500;
            color: var(--dark);
            margin-bottom: 0.5rem;
            font-size: 0.85rem;
        }

        /* Captcha Section */
        .captcha-section {
            background: var(--light);
            border-radius: var(--radius-lg);
            padding: 1.5rem;
            margin-top: 2rem;
            border: 2px solid var(--border-color);
        }

        .captcha-image {
            display: flex;
            align-items: center;
            gap: 1rem;
            margin-bottom: 1rem;
        }

        /* Modal Styling */
        .modalBackground {
            background-color: rgba(0, 0, 0, 0.5);
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border-radius: var(--radius-lg);
            border: none;
            box-shadow: var(--shadow-lg);
            padding: 0;
            overflow: hidden;
        }

        .modal-header-custom {
            background: linear-gradient(145deg, var(--primary), var(--secondary));
            color: white;
            padding: 1rem 1.5rem;
            font-weight: 600;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .modal-body-custom {
            padding: 1.5rem;
            max-height: 500px;
            overflow-y: auto;
        }

        .grade-conversion-table {
            width: 100%;
            border-collapse: separate;
            border-spacing: 0;
            border-radius: var(--radius-md);
            overflow: hidden;
            box-shadow: var(--shadow-sm);
            margin-bottom: 1rem;
        }

        .grade-conversion-table th,
        .grade-conversion-table td {
            padding: 0.75rem;
            text-align: center;
            border: 1px solid var(--border-color);
        }

        .grade-conversion-table th {
            background: var(--primary);
            color: white;
            font-weight: 600;
        }

        .grade-conversion-table td {
            background: white;
        }

        /* Helper Text */
        .helper-text {
            font-size: 0.75rem;
            color: var(--warning);
            margin-top: 0.25rem;
            display: block;
        }

        /* Validator Styling */
        span[id*="Validator"],
        span[id*="CV"],
        span[id*="RFV"] {
            display: block;
            margin-top: 0.25rem;
            font-size: 0.85rem;
            color: var(--danger);
        }

        /* Legacy styles - kept for compatibility */
        .style_td {
            font-weight: bold;
            text-align: left;
            font-size: 9pt;
        }

        .style_td1 {
            font-weight: bold;
            text-align: right;
            font-size: 9pt;
        }

        .style_td_secondCol {
            border-left: dotted;
            border-color: gray;
            border-width: 1px;
        }

        .style_thead {
            text-align: center;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 12pt;
            font-weight: bold;
        }

        .panelBody_edu_marginBottom {
            margin-bottom: 0;
        }

        .auto-style1 {
            height: 32px;
        }

        /* Responsive Design */
        @media (max-width: 768px) {
            .education-grid {
                grid-template-columns: 1fr;
            }

            .card-body-modern {
                padding: 1rem;
            }

            .btn-modern {
                width: 100%;
                justify-content: center;
            }
        }

        /* Animation */
        @keyframes fadeIn {
            from {
                opacity: 0;
                transform: translateY(10px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        .modern-card {
            animation: fadeIn 0.5s ease forwards;
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="modern-card">
                <div class="card-header-modern">
                    <h4 style="margin: 0;">Purchase Form</h4>
                </div>
                <div class="card-body-modern">
                    <asp:UpdatePanel runat="server" ID="upanel">
                        <ContentTemplate>
                            <!-- Alert Messages -->
                            <asp:Panel ID="massageHiddenTopId" runat="server" Visible="false">
                                <div class="alert-modern alert-danger-modern">
                                    <asp:Label ID="lblEligibleMsg" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                            </asp:Panel>

                            <!-- Important Notice -->
                            <div class="alert-modern alert-warning-modern">
                                <strong>Please note:</strong> This is not the final application. Candidate has to fill up application form after successful payment to get Admit Card.</br>
                                <span class="spanAsterisk">( * )</span> <strong>Indicates required field.</strong>
                            </div>

                            <%-- 
                            <tr>
                                <td class="style_td">Quota <span class="spanAsterisk">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddlQuota" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
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
                                <td>
                                    <asp:DropDownList ID="ddlPassingYear" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CompareValidator ID="ddlPassingYearComV" runat="server"
                                        ControlToValidate="ddlPassingYear" ErrorMessage="Passing Year Required."
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                </td>
                            </tr>
                            --%>

                            <!-- Personal Information Card -->
                            <div class="modern-card" style="animation-delay: 0.1s;">
                                <div class="card-header-modern">
                                    Personal Information
                                </div>
                                <div class="card-body-modern">
                                    <div class="row">
                                        <div class="col-lg-6 col-md-6 col-sm-12">
                                            <div class="form-group-modern">
                                                <label>Name <span class="text-danger">*</span></label>
                                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter your full name" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server"
                                                    ID="NameReq"
                                                    ValidationGroup="SUBMIT"
                                                    ControlToValidate="txtName"
                                                    Display="Dynamic"
                                                    Font-Size="9pt"
                                                    ForeColor="Crimson"
                                                    Font-Bold="false"
                                                    ErrorMessage="Name Required" />
                                            </div>
                                        </div>
                                        <div class="col-lg-6 col-md-6 col-sm-12">
                                            <div class="form-group-modern">
                                                <label>Date of Birth <span class="text-danger">*</span></label>
                                                <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="form-control" placeholder="dd/MM/yyyy" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
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
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-lg-6 col-md-6 col-sm-12">
                                            <div class="form-group-modern">
                                                <label>Email <span class="text-danger">*</span></label>
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"
                                                    TextMode="Email" placeholder="your.email@example.com" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                <span class="helper-text">Please provide a valid email address.</span>
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
                                        <div class="col-lg-6 col-md-6 col-sm-12">
                                            <div class="form-group-modern">
                                                <label>Gender <span class="text-danger">*</span></label>
                                                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control"></asp:DropDownList>
                                                <asp:CompareValidator ID="ddlGenderComV" runat="server"
                                                    ControlToValidate="ddlGender" ErrorMessage="Gender required"
                                                    Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-lg-6 col-md-6 col-sm-12">
                                            <div class="form-group-modern">
                                                <label>Mobile No. for SMS <span class="text-danger">*</span></label>
                                                <asp:TextBox ID="txtSmsMobile" runat="server" CssClass="form-control" placeholder="+88017XXXXXXXX" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                <span class="helper-text">Please include country code, e.g.: +8801700000000. Candidate will not receive Username and Password if number is in wrong format.</span>
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
                                                    ValidationExpression="^(\+88)\d{11}$"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 col-md-6 col-sm-12">
                                            <div class="form-group-modern">
                                                <label>Guardian's Mobile No. <span class="text-danger">*</span></label>
                                                <asp:TextBox ID="txtGuardianMobile" runat="server" CssClass="form-control" placeholder="+88017XXXXXXXX" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                                <span class="helper-text">Please include country code, e.g.: +8801700000000. Guardian will not receive information if number is in wrong format.</span>
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
                                                    ValidationExpression="^(\+88)\d{11}$"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Education Information Grid -->
                            <div class="education-grid">
                                <!-- SSC Section -->
                                <asp:Panel ID="hideSSC" runat="server" CssClass="education-card">
                                    <div class="education-header">
                                        <i class="fas fa-graduation-cap"></i> SSC/O-Level/Equivalent
                                    </div>
                                    
                                    <div class="form-field">
                                        <label>Institute Name</label>
                                        <asp:TextBox ID="txtInstituteSSC" runat="server" CssClass="form-control" placeholder="Enter institute name"></asp:TextBox>
                                    </div>

                                    <div class="form-field">
                                        <label>Board</label>
                                        <asp:DropDownList ID="ddlBoardSSC" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>

                                    <div class="form-field">
                                        <label>Exam Type <span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlExamTypeSSC" runat="server" CssClass="form-control" OnSelectedIndexChanged="myListDropDown_Change" AutoPostBack="true">
                                            <asp:ListItem Enabled="true" Text="Exam Type" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="SSC" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="O-Level" Value="5"></asp:ListItem>
                                            <asp:ListItem Text="Dakhil" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="SSC (Vocational)" Value="12"></asp:ListItem>
                                            <asp:ListItem Text="International Baccalaureate" Value="14"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="ddlExamTypeSSCComV" runat="server"
                                            ControlToValidate="ddlExamTypeSSC" ErrorMessage="SSC Exam Type Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                    </div>

                                    <div class="form-field">
                                        <label>Group <span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlGroupSSC" runat="server" CssClass="form-control">
                                            <asp:ListItem Enabled="true" Text="Group" Value="-1"></asp:ListItem>
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

                                    <div class="form-field">
                                        <label>GPA <span class="spanAsterisk">*</span></label>
                                        <asp:TextBox ID="txtGPASSC" runat="server" placeholder="GPA (x.xx or x)" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ValidationGroup="SUBMIT"
                                            ControlToValidate="txtGPASSC"
                                            Display="Dynamic"
                                            Font-Size="9pt"
                                            ForeColor="Crimson"
                                            Font-Bold="false"
                                            ErrorMessage="GPA is in wrong format. Correct format (x.xx or x)"
                                            ValidationExpression="^\d{0,2}(\.\d{1,2})?$"> </asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="txtGPASSC_ReqV" runat="server"
                                            ControlToValidate="txtGPASSC"
                                            ErrorMessage="SSC GPA Required"
                                            ForeColor="Crimson"
                                            Display="Dynamic"
                                            Font-Size="9pt"
                                            Font-Bold="false"
                                            ValidationGroup="SUBMIT"> </asp:RequiredFieldValidator>
                                    </div>

                                    <div class="form-field">
                                        <label>Passing Year</label>
                                        <asp:DropDownList ID="ddlPassYearSSC" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <%--<asp:TextBox ID="txtPassYearSSC" runat="server" placeholder="Passing Year" CssClass="form-control" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>--%>
                                        <%--<asp:TextBox ID="ddlPassYearSSC" runat="server" placeholder="Passing Year" CssClass="form-control" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>--%>
                                        <%--<asp:DropDownList ID="ddlPassYearSSC" runat="server" CssClass="form-control">
                                            <asp:ListItem Enabled="true" Text="Passing Year" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                                            <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                                            <asp:ListItem Text="2014" Value="2014"></asp:ListItem>
                                            <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
                                        </asp:DropDownList>--%>
                                        <%--<asp:RequiredFieldValidator ID="txtPassYearSSCComV" runat="server"
                                            ControlToValidate="txtPassYearSSC"
                                            ErrorMessage="SSC Passing Year Required"
                                            ForeColor="Crimson"
                                            Display="Dynamic"
                                            Font-Size="9pt"
                                            Font-Bold="false"
                                            ValidationGroup="SUBMIT"> </asp:RequiredFieldValidator>--%>
                                        <%--<asp:CompareValidator ID="ddlPassYearSSCComV" runat="server"
                                            ControlToValidate="ddlPassYearSSC" ErrorMessage="SSC Passing Year Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>--%>
                                    </div>
                                </asp:Panel>

                                <!-- HSC Section -->
                                <asp:Panel ID="hideHSC" runat="server" CssClass="education-card">
                                    <div class="education-header">
                                        <i class="fas fa-user-graduate"></i> HSC/A-Level/Equivalent
                                    </div>
                                    
                                    <div class="form-field">
                                        <label>Institute Name</label>
                                        <asp:TextBox ID="txtInstituteHSC" runat="server" CssClass="form-control" placeholder="Enter institute name"></asp:TextBox>
                                    </div>

                                    <div class="form-field">
                                        <label>Board</label>
                                        <asp:DropDownList ID="ddlBoardHSC" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>

                                    <div class="form-field">
                                        <label>Exam Type <span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlExamTypeHSC" runat="server" CssClass="form-control">
                                            <asp:ListItem Enabled="true" Text="Exam Type" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="HSC" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="A-Level" Value="7"></asp:ListItem>
                                            <asp:ListItem Text="Alim" Value="8"></asp:ListItem>
                                            <asp:ListItem Text="HSC (Vocational)" Value="13"></asp:ListItem>
                                            <asp:ListItem Text="International Baccalaureate" Value="15"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="ddlExamTypeHSCComV" runat="server"
                                            ControlToValidate="ddlExamTypeHSC" ErrorMessage="HSC Exam Type Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                    </div>

                                    <div class="form-field">
                                        <label>Group <span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlGroupHSC" runat="server" CssClass="form-control">
                                            <asp:ListItem Enabled="true" Text="Group" Value="-1"></asp:ListItem>
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

                                    <div class="form-field">
                                        <label>GPA <span class="spanAsterisk">*</span></label>
                                        <asp:TextBox ID="txtGPAHSC" runat="server" placeholder="GPA (x.xx or x)" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="YourRegularExpressionValidator" runat="server"
                                            ValidationGroup="SUBMIT"
                                            ControlToValidate="txtGPAHSC"
                                            Display="Dynamic"
                                            Font-Size="9pt"
                                            ForeColor="Crimson"
                                            Font-Bold="false"
                                            ErrorMessage="GPA is in wrong format. Correct format (x.xx or x)"
                                            ValidationExpression="^\d{0,2}(\.\d{1,2})?$"> </asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                            ControlToValidate="txtGPAHSC"
                                            ErrorMessage="HSC GPA Required"
                                            ForeColor="Crimson"
                                            Display="Dynamic"
                                            Font-Size="9pt"
                                            Font-Bold="false"
                                            ValidationGroup="SUBMIT"> </asp:RequiredFieldValidator>
                                    </div>

                                    <div class="form-field">
                                        <label>Passing Year</label>
                                        <asp:DropDownList ID="ddlPassYearHSC" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <%--<asp:TextBox ID="txtPassYearHSC" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>--%>
                                        <%--<asp:DropDownList ID="ddlPassYearHSC" runat="server" CssClass="form-control">
                                            <asp:ListItem Enabled="true" Text="Passing Year" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                                            <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
                                        </asp:DropDownList>--%>
                                        <%--<asp:RequiredFieldValidator ID="txtPassYearHSCComV" runat="server"
                                            ControlToValidate="txtPassYearHSC"
                                            ErrorMessage="HSC Passing Year Required"
                                            ForeColor="Crimson"
                                            Display="Dynamic"
                                            Font-Size="9pt"
                                            Font-Bold="false"
                                            ValidationGroup="SUBMIT"> </asp:RequiredFieldValidator>--%>
                                        <%--<asp:CompareValidator ID="ddlPassYearHSCComV" runat="server"
                                            ControlToValidate="ddlPassYearHSC" ErrorMessage="HSC Passing Year Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>--%>
                                    </div>
                                </asp:Panel>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <%--
                    <div class="row">
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
                    </div>
                    --%>
                    <%-- END SECONDARY / O-Level --%>

                    <%-- 
                    <div class="row">
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
                    </div>
                    --%>
                    <%-- END HIGHER SECONDARY / A-Level --%>
                    
                    <%--<asp:HiddenField ID="hiddenFieldUndergrad" runat="server" />--%>

                    <%-- 
                    <asp:Panel ID="undergraduateInfoPanel" runat="server" Visible="false">
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
                    </asp:Panel>
                    --%>
                    <%-- END UNDERGRAD --%>

                    <!-- Captcha Section -->
                     <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                        <ContentTemplate>
                            <div class="captcha-section">
                                <div class="captcha-image" style="justify-content: center;">
                                    <img runat="server" id="imgCtrl" style="border-radius: var(--radius-md); box-shadow: var(--shadow-sm);" />
                                    <asp:ImageButton ID="btnReLoadCaptcha" runat="server"
                                        Height="40"
                                        Width="40"
                                        ToolTip="Reload captcha"
                                        ImageUrl="~/Images/AppImg/reload6.png"
                                        style="cursor: pointer; transition: var(--transition); border-radius: var(--radius-md);" />
                                </div>

                                <asp:Panel ID="captchaMsg" runat="server" Visible="false">
                                    <div class="alert-modern alert-warning-modern" style="margin-bottom: 1rem; text-align: center;">
                                        <asp:Label ID="lblCaptcha" runat="server"
                                            Text="Sorry your text and image didn't match. Please try again."></asp:Label>
                                    </div>
                                </asp:Panel>

                                <div class="form-group-modern" style="display: flex; flex-direction: column; align-items: center;">
                                    <label style="text-align: center;">Enter the code shown</label>
                                    <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control"
                                        style="max-width: 300px; text-align: center;" placeholder="Enter captcha code" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                </div>
                            </div>

                            <!-- Bottom Alert Message -->
                            <asp:Panel ID="massageHiddenBottomId" runat="server" Visible="false">
                                <div class="alert-modern alert-danger-modern">
                                    <asp:Label ID="lblEligibleMsgBottom" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                            </asp:Panel>

                            <!-- Submit Button -->
                            <div style="margin-top: 2rem; text-align: center;">
                                <asp:Button ID="btnSubmit" runat="server" Text="Next"
                                    CssClass="btn-modern btn-primary-modern" ValidationGroup="SUBMIT"
                                    OnClick="btnSubmit_Click" style="min-width: 200px;" />
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
                                <div class="modalPopup" style="max-width: 900px; width: 90vw;">
                                    <div class="modal-header-custom">
                                        <span style="font-size: 1.2rem;">O/A-Level Grade Conversion</span>
                                        <asp:LinkButton ID="LinkButton2" runat="server" OnClick="btnCancleModel_Click" style="color: white; text-decoration: none; font-size: 1.5rem; line-height: 1;">
                                            <i class="fas fa-times"></i>
                                        </asp:LinkButton>
                                    </div>
                                    
                                    <div class="modal-body-custom">
                                        <!-- Instruction Section -->
                                        <div class="alert-modern alert-info-modern" style="margin-bottom: 1.5rem;">
                                            <strong>Grade Points Calculation for English Medium Students:</strong> Calculate the Average Grade Points using the table considering all subjects.
                                        </div>

                                        <!-- Grade Conversion Table -->
                                        <table class="grade-conversion-table" style="max-width: 500px; margin: 0 auto 2rem;">
                                            <thead>
                                                <tr>
                                                    <th>Grade</th>
                                                    <th>A*/A</th>
                                                    <th>B</th>
                                                    <th>C</th>
                                                    <th>D</th>
                                                    <th>E</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <th style="background: var(--light); color: var(--dark);">Point</th>
                                                    <td>5.00</td>
                                                    <td>4.00</td>
                                                    <td>3.50</td>
                                                    <td>3.00</td>
                                                    <td>0.00</td>
                                                </tr>
                                            </tbody>
                                        </table>

                                        <!-- O-Level and A-Level Grid -->
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="modern-card" style="animation: none;">
                                                    <div class="card-header-modern" style="font-size: 1rem; padding: 1rem;">
                                                        O-Level
                                                    </div>
                                                    <div class="card-body-modern" style="padding: 1.5rem;">
                                                        <div class="form-field">
                                                            <label>Subject-1:</label>
                                                            <asp:DropDownList ID="ddlOLevelSubject1" runat="server" CssClass="form-control">
                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlOLevelSubject1_CV" runat="server"
                                                                ControlToValidate="ddlOLevelSubject1"
                                                                ErrorMessage="Subject-1 Grade Required"
                                                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                                ValueToCompare="-1" Operator="NotEqual"
                                                                ValidationGroup="btnOALevelCalculate"></asp:CompareValidator>
                                                        </div>

                                                        <div class="form-field">
                                                            <label>Subject-2:</label>
                                                            <asp:DropDownList ID="ddlOLevelSubject2" runat="server" CssClass="form-control">
                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlOLevelSubject2_CV" runat="server"
                                                                ControlToValidate="ddlOLevelSubject2"
                                                                ErrorMessage="Subject-2 Grade Required"
                                                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                                ValueToCompare="-1" Operator="NotEqual"
                                                                ValidationGroup="btnOALevelCalculate"></asp:CompareValidator>
                                                        </div>

                                                        <div class="form-field">
                                                            <label>Subject-3:</label>
                                                            <asp:DropDownList ID="ddlOLevelSubject3" runat="server" CssClass="form-control">
                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlOLevelSubject3_CV" runat="server"
                                                                ControlToValidate="ddlOLevelSubject3"
                                                                ErrorMessage="Subject-3 Grade Required"
                                                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                                ValueToCompare="-1" Operator="NotEqual"
                                                                ValidationGroup="btnOALevelCalculate"></asp:CompareValidator>
                                                        </div>

                                                        <div class="form-field">
                                                            <label>Subject-4:</label>
                                                            <asp:DropDownList ID="ddlOLevelSubject4" runat="server" CssClass="form-control">
                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlOLevelSubject4_CV" runat="server"
                                                                ControlToValidate="ddlOLevelSubject4"
                                                                ErrorMessage="Subject-4 Grade Required"
                                                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                                ValueToCompare="-1" Operator="NotEqual"
                                                                ValidationGroup="btnOALevelCalculate"></asp:CompareValidator>
                                                        </div>

                                                        <div class="form-field">
                                                            <label>Subject-5:</label>
                                                            <asp:DropDownList ID="ddlOLevelSubject5" runat="server" CssClass="form-control">
                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlOLevelSubject5_CV" runat="server"
                                                                ControlToValidate="ddlOLevelSubject5"
                                                                ErrorMessage="Subject-5 Grade Required"
                                                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                                ValueToCompare="-1" Operator="NotEqual"
                                                                ValidationGroup="btnOALevelCalculate"></asp:CompareValidator>
                                                        </div>

                                                        <div style="background: var(--light); padding: 1rem; border-radius: var(--radius-md); margin-top: 1rem;">
                                                            <div style="margin-bottom: 0.5rem;">
                                                                <strong>O-Level Points:</strong>
                                                                <asp:Label ID="lblOLevelResult" runat="server" Text="" Style="font-weight: bold; color: var(--accent); margin-left: 0.5rem;"></asp:Label>
                                                                <asp:HiddenField ID="hfOLevelConvertedSscGPA" runat="server" />
                                                            </div>
                                                            <div>
                                                                <strong>Converted GPA:</strong>
                                                                <asp:Label ID="lblOLevelConvertedSscGPA" runat="server" Text="" Style="font-weight: bold; color: var(--success); margin-left: 0.5rem;"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div class="modern-card" style="animation: none;">
                                                    <div class="card-header-modern" style="font-size: 1rem; padding: 1rem;">
                                                        A-Level
                                                    </div>
                                                    <div class="card-body-modern" style="padding: 1.5rem;">
                                                        <div class="form-field">
                                                            <label>Subject-1:</label>
                                                            <asp:DropDownList ID="ddlALevelSubject1" runat="server" CssClass="form-control">
                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlALevelSubject1_CV" runat="server"
                                                                ControlToValidate="ddlALevelSubject1"
                                                                ErrorMessage="Subject-1 Grade Required"
                                                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                                ValueToCompare="-1" Operator="NotEqual"
                                                                ValidationGroup="btnOALevelCalculate"></asp:CompareValidator>
                                                        </div>

                                                        <div class="form-field">
                                                            <label>Subject-2:</label>
                                                            <asp:DropDownList ID="ddlALevelSubject2" runat="server" CssClass="form-control">
                                                                <asp:ListItem Enabled="true" Text="Select Grade" Value="-1"></asp:ListItem>
                                                                <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                                                <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                                                <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                                                <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                                                <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:CompareValidator ID="ddlALevelSubject2_CV" runat="server"
                                                                ControlToValidate="ddlALevelSubject2"
                                                                ErrorMessage="Subject-1 Grade Required"
                                                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                                ValueToCompare="-1" Operator="NotEqual"
                                                                ValidationGroup="btnOALevelCalculate"></asp:CompareValidator>
                                                        </div>

                                                        <div style="background: var(--light); padding: 1rem; border-radius: var(--radius-md); margin-top: 1rem;">
                                                            <div style="margin-bottom: 0.5rem;">
                                                                <strong>A-Level Points:</strong>
                                                                <asp:Label ID="lblALevelResult" runat="server" Text="" Style="font-weight: bold; color: var(--accent); margin-left: 0.5rem;"></asp:Label>
                                                                <asp:HiddenField ID="hfALevelConvertedHscGPA" runat="server" />
                                                            </div>
                                                            <div>
                                                                <strong>Converted GPA:</strong>
                                                                <asp:Label ID="lblALevelConvertedHscGPA" runat="server" Text="" Style="font-weight: bold; color: var(--success); margin-left: 0.5rem;"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <!-- Total Points Section -->
                                                <div class="modern-card" style="animation: none; margin-top: 1rem;">
                                                    <div class="card-body-modern" style="padding: 1.5rem;">
                                                        <div style="margin-bottom: 1rem;">
                                                            <strong style="font-size: 1.1rem;">Total Points:</strong>
                                                            <asp:Label ID="lblTotalPoints" runat="server" Style="font-weight: bold; color: var(--primary); font-size: 1.2rem; margin-left: 0.5rem;"></asp:Label>
                                                        </div>
                                                        <asp:Label ID="lblMassage" runat="server" ForeColor="Crimson" style="display: block; margin-bottom: 1rem;"></asp:Label>
                                                        <asp:HiddenField ID="HiddenField1" runat="server" />
                                                        <asp:Button ID="btnCalculateALevel" runat="server" Text="Calculate" 
                                                            OnClick="btnCalculateOAndALevel_Click" ValidationGroup="btnOALevelCalculate"
                                                            CssClass="btn-modern btn-primary-modern" style="width: 100%;" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <!-- Close Button -->
                                        <div style="text-align: right; margin-top: 1.5rem; padding-top: 1rem; border-top: 2px solid var(--border-color);">
                                            <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnCancleModel_Click"
                                                style="background: var(--gray); color: white; border: none; padding: 0.75rem 2rem; border-radius: var(--radius-md); cursor: pointer; transition: var(--transition);" />
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <%------------------------------------------------ END POPUP Modal -----------------------------------------%>

                </div>
            </div>
        </div>
    </div>
</asp:Content>