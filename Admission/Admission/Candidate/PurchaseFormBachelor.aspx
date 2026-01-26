<%@ Page Title="Purchase Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PurchaseFormBachelor.aspx.cs" Inherits="Admission.Admission.Candidate.PurchaseFormBachelor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
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
        }

        /* Card Styles */
        .modern-card {
            background: white;
            border-radius: 12px;
            box-shadow: 0 4px 6px rgba(0,0,0,0.07);
            padding: 2rem;
            margin-bottom: 1.5rem;
            transition: all 0.3s ease;
        }

            .modern-card:hover {
                box-shadow: 0 8px 15px rgba(0,0,0,0.1);
            }

        .card-header-custom {
            background: linear-gradient(145deg, var(--primary), var(--secondary));
            color: white;
            padding: 1.5rem;
            border-radius: 12px 12px 0 0;
            margin: -2rem -2rem 2rem -2rem;
        }

            .card-header-custom h4 {
                margin: 0;
                font-weight: 600;
                font-size: 1.5rem;
            }

        /* Info Alert */
        .info-box {
            background: linear-gradient(135deg, #EFF6FF 0%, #DBEAFE 100%);
            border-left: 4px solid var(--accent);
            border-radius: 8px;
            padding: 1.25rem;
            margin-bottom: 1.5rem;
        }

            .info-box strong {
                color: var(--accent);
                font-size: 1.1rem;
            }

            .info-box ul {
                margin: 0.75rem 0 0 0;
                padding-left: 1.5rem;
            }

            .info-box li {
                margin: 0.5rem 0;
                line-height: 1.6;
            }

        /* Education Type Section */
        .education-section {
            border-radius: 12px;
            padding: 1.5rem;
            border: 2px solid #FCD34D;
        }

        .education-buttons {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 1rem;
            margin-top: 1rem;
        }

        .btn-education {
            color: white;
            padding: 1rem;
            border: none;
            border-radius: 8px;
            font-weight: 600;
            transition: all 0.3s ease;
            cursor: pointer;
            position: relative;
            overflow: hidden;
        }

            .btn-education::before {
                content: '';
                position: absolute;
                top: 0;
                left: -100%;
                width: 100%;
                height: 100%;
                background: rgba(255,255,255,0.2);
                transition: left 0.3s ease;
            }

            .btn-education:hover::before {
                left: 100%;
            }

            .btn-education:hover {
                transform: translateY(-2px);
                box-shadow: 0 6px 12px rgba(0,0,0,0.15);
            }

        /* Form Groups */
        .form-group {
            margin-bottom: 1.5rem;
        }

            .form-group label {
                display: block;
                margin-bottom: 0.5rem;
                font-weight: 600;
                color: var(--dark);
                font-size: 0.95rem;
            }

        .form-control {
            width: 100%;
            padding: 0.75rem;
            border: 2px solid #E5E7EB;
            border-radius: 8px;
            transition: all 0.3s ease;
            font-size: 0.95rem;
        }

            .form-control:focus {
                outline: none;
                border-color: var(--accent);
                box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
            }

        /* Section Headers */
        .section-header {
            background: linear-gradient(135deg, var(--accent-light), var(--accent));
            color: white;
            padding: 1rem;
            border-radius: 8px;
            text-align: center;
            font-weight: 600;
            margin-bottom: 1.5rem;
        }

        /* Two Column Layout */
        .two-col-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
            gap: 1.5rem;
        }

        /* Panel Styles */
        .panel-modern {
            background: white;
            border: 2px solid #E5E7EB;
            border-radius: 12px;
            overflow: hidden;
        }

        .panel-modern-header {
            background: linear-gradient(135deg, #F3F4F6, #E5E7EB);
            padding: 1rem;
            font-weight: 600;
            color: var(--dark);
            border-bottom: 2px solid #D1D5DB;
        }

        .panel-modern-body {
            padding: 1.5rem;
        }

        /* Grade Table */
        .grade-table {
            width: 100%;
            border-collapse: separate;
            border-spacing: 0;
            margin: 1rem 0;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }

            .grade-table th {
                background: var(--danger);
                color: white;
                padding: 0.75rem;
                font-weight: 600;
            }

            .grade-table td {
                background: #FEE2E2;
                color: var(--danger);
                padding: 0.75rem;
                text-align: center;
                font-weight: 600;
            }

        /* Result Display */
        .result-box {
            background: #FEF2F2;
            border: 2px solid #FCA5A5;
            border-radius: 8px;
            padding: 1rem;
            margin-top: 1rem;
        }

        .result-label {
            font-weight: bold;
            color: var(--danger);
            font-size: 1.1rem;
        }

        /* Submit Section */
        .submit-section {
            border-radius: 12px;
            padding: 2rem;
            text-align: center;
        }

        .captcha-container {
            display: inline-block;
            margin: 1rem 0;
        }

        .btn-submit {
            background: linear-gradient(145deg, var(--accent), var(--secondary));
            color: white;
            border: none;
            padding: 1rem 3rem;
            border-radius: 8px;
            font-size: 1.1rem;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
        }

            .btn-submit:hover {
                transform: translateY(-2px);
                box-shadow: 0 6px 12px rgba(0,0,0,0.15);
            }

        .btn-calculate {
            background: var(--accent);
            color: white;
            border: none;
            padding: 0.75rem 2rem;
            border-radius: 8px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
        }

            .btn-calculate:hover {
                background: var(--secondary);
                transform: translateY(-2px);
            }

        .btn-verify {
            display: block;
            margin: 0 auto;
            background: var(--danger);
            color: white;
            border: none;
            padding: 0.75rem 2rem;
            border-radius: 8px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
        }


            .btn-verify:hover {
                opacity: 0.9;
                transform: translateY(-2px);
            }

        /* Asterisk */
        .spanAsterisk {
            color: var(--danger);
            font-size: 1.1rem;
            margin-left: 2px;
        }

        /* Loading Overlay */
        #divProgress {
            background: rgba(0,0,0,0.5);
            backdrop-filter: blur(4px);
        }

        /* Messages */
        .message-panel {
            border-radius: 8px;
            padding: 1rem;
            margin-bottom: 1rem;
            position: relative;
        }

            .message-panel .close {
                position: absolute;
                right: 1rem;
                top: 50%;
                transform: translateY(-50%);
            }

        .modalBackground {
            position: fixed !important;
            background-color: rgba(0, 0, 0, 0.75) !important;
            backdrop-filter: blur(5px);
            top: 0 !important;
            left: 0 !important;
            width: 100% !important;
            height: 100% !important;
            z-index: 99999 !important;
        }

        .modalPopup {
            background-color: #FFFFFF !important;
            border-radius: 16px !important;
            box-shadow: 0 25px 50px rgba(0,0,0,0.3) !important;
            padding: 0 !important;
            width: 90% !important;
            max-width: 600px !important;
            position: fixed !important;
            top: 12% !important;
            left: 50% !important;
            transform: translate(-50%, -50%) !important;
            z-index: 100000 !important;
            overflow: hidden;
        }

        @media (max-width: 768px) {
            .modern-card {
                padding: 1rem;
            }

            .card-header-custom {
                margin: -1rem -1rem 1rem -1rem;
                padding: 1rem;
            }

            .education-buttons {
                grid-template-columns: 1fr;
            }

            .two-col-grid {
                grid-template-columns: 1fr;
            }

            .btn-submit {
                width: 100%;
                padding: 1rem;
            }

            .btn-verify {
                float: none;
                width: 100%;
                margin-top: 1rem;
            }
        }

        /* Old styles for compatibility */
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
            font-family: Calibri;
            font-size: 12pt;
            font-weight: bold;
        }

        .panelBody_edu_marginBottom {
            margin-bottom: -3%;
        }

        .auto-style2 {
            display: block;
            font-size: 14px;
            line-height: 1.42857143;
            color: #555;
            border-radius: 8px;
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            border: 2px solid #E5E7EB;
            padding: 0.75rem;
            background-color: #fff;
            background-image: none;
        }

        /* Mobile Responsive */
        @media screen and (max-width: 600px) {
            #MainContent_btnSSCHSC {
                font-size: smaller;
                margin-bottom: 5px;
            }

            .captcha {
                width: 100% !important;
            }

            #MainContent_btnSubmit {
                width: 100% !important;
            }

            .grade-table {
                font-size: 0.85rem;
            }
        }

        /* Animation */
        @keyframes slideUp {
            from {
                opacity: 0;
                transform: translateY(20px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        .modern-card {
            animation: slideUp 0.4s ease;
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
            document.getElementById("blurOverlay").style.display = "block";
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
            document.getElementById("blurOverlay").style.display = "none";
        }
    </script>
    <script type="text/javascript">
        function scrollToModalTop() {
            setTimeout(function () {
                window.scrollTo({ top: 0, behavior: 'auto' });
            }, 100);
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="blurOverlay" style="display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; backdrop-filter: blur(5px); background-color: rgba(255, 255, 255, 0.3); z-index: 999999;">
    </div>
    <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="250px" Width="250px" />
    </div>

    <div class="modern-card">
        <div class="card-header-custom">
            <h4><i class="fas fa-shopping-cart"></i>Purchase Application Form</h4>
            <div class="row" style="display: none;">
                <div class="col-lg-12" style="text-align: right;">
                    <asp:Button ID="btnAdminAssignTestValue" runat="server" Text="Assign Test Value" CssClass="btn btn-danger" OnClick="btnAdminAssignTestValue_Click" />
                </div>
            </div>
        </div>

        <%--======================== Info ========================--%>
        <div class="info-box">
            <strong><i class="fas fa-info-circle"></i>Important Information!</strong>
            <ul>
                <li>
                    <span style="font-weight: bold; color: crimson;">(*) Indicates required field.</span>
                </li>
                <%--<li>
                    <span style="font-weight: bold; color: orangered">Please note that this is not the final application. After successful payment candidate has to fill up further information form to get Admit Card.</span>
                </li>--%>
              <%--  <li>
                    <span style="font-weight: bold; color: black">Candidates having combination of SSC/HSC and O/A Level (for example, SSC &amp; A level or O level &amp; HSC) and International Baccalaureate are requested to contact with BUP admission helpline.</span>
                </li>--%>
            </ul>
        </div>
        <%--======================== END Info ========================--%>

        <asp:UpdatePanel ID="UpdatePanelAll" runat="server">
            <ContentTemplate>

                <%--======================== Select Education ========================--%>
                <div class="education-section" runat="server" visible="false">
                    <div style="margin-bottom: 1rem;">
                        <strong style="font-size: 1.1rem;"><i class="fas fa-graduation-cap"></i>Select Education Type</strong>
                        <span class="spanAsterisk">*</span>
                    </div>
                    <hr style="margin: 10px 0; border-color: #FCD34D;" />

                    <div class="education-buttons">
                        <asp:Button ID="btnSSCHSC" runat="server" class="btn-education"
                            Style="background: linear-gradient(145deg, #10B981, #059669);"
                            Text="SSC/Equivalent & HSC/Equivalent"
                            OnClick="btnSSCHSC_Click" />

                        <asp:Button ID="btnOALevel" runat="server" class="btn-education"
                            Style="background: linear-gradient(145deg, #3B82F6, #2563EB);"
                            Text="O-Level & A-Level"
                            OnClick="btnOALevel_Click" />

                        <asp:Button ID="btnInternationalBaccalaureate" runat="server" class="btn-education" Visible="false"
                            Style="background: linear-gradient(145deg, #06B6D4, #0891B2);"
                            Text="International Baccalaureate"
                            OnClick="btnInternationalBaccalaureate_Click" />

                        <asp:Button ID="btnOALevelAppeared" runat="server" class="btn-education" Visible="false"
                            Style="background: linear-gradient(145deg, #F59E0B, #D97706);"
                            Text="A Level Appeared Only"
                            OnClick="btnOALevelAppeared_Click" />
                    </div>

                    <div style="display: none;">
                        <asp:RadioButtonList ID="rblEducationChoice" runat="server" OnSelectedIndexChanged="rblEducationChoice_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Value="1">SSC, SSC (Vocational), HSC, HSC (Vocational), Dakhil, Alim</asp:ListItem>
                            <asp:ListItem Value="2">O-Level, A-Level</asp:ListItem>
                            <asp:ListItem Value="3">International Baccalaureate</asp:ListItem>
                            <asp:ListItem Value="4">O-Level, A-Level (2023 Appeared)</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="ReqiredFieldValidator1" runat="server"
                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                            ControlToValidate="rblEducationChoice" ErrorMessage="Required"
                            ValidationGroup="SUBMIT">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <%--======================== END Select Education ========================--%>

                <%--======================== MessageTop ========================--%>
                <asp:UpdatePanel ID="UpdatePanelMassageTop" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblErrormsg" runat="server" Text="" CssClass="message-panel"></asp:Label>
                        <asp:Panel ID="messagePanelTop" runat="server" Visible="false" CssClass="message-panel">
                            <asp:Label ID="lblMessageTop" runat="server" Text=""></asp:Label>
                            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--======================== END MessageTop ========================--%>

                <%--======================== Message (View Eligible & Not-Eligible List) ========================--%>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="PanelMessageEligibleNotEligibleList" runat="server" Visible="false">
                            <asp:Label ID="LabelMessageEligibleNotEligible" runat="server" Text=""></asp:Label>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--======================== END Message (View Eligible & Not-Eligible List) ========================--%>

                <%--======================== SSC HSC Input Section ========================--%>
                <asp:Panel ID="panelSSCHSCInputSection" runat="server" Visible="false">
                    <div class="modern-card" style="border: 2px solid #FCD34D;">

                        <!-- SSC Information -->
                        <div class="panel-modern">
                            <div class="panel-modern-header">
                                <i class="fas fa-school"></i>SSC Information
                            </div>
                            <div class="panel-modern-body">
                                <div class="row">
                                    <div class="col-sm-12 col-md-4">
                                        <div class="form-group">
                                            <label>Examination<span class="spanAsterisk">*</span></label>
                                            <asp:DropDownList ID="ddlExamTypeSSC" runat="server" CssClass="form-control">
                                                <asp:ListItem Enabled="true" Text="--Select Exam--" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="SSC, SSC (Vocational), Dakhil" Value="ssc" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareValidator1" runat="server"
                                                ControlToValidate="ddlExamTypeSSC" ErrorMessage="Required"
                                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMITVerifyInformation"></asp:CompareValidator>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-2">
                                        <div class="form-group">
                                            <label>Roll Number<span class="spanAsterisk">*</span></label>
                                            <asp:TextBox ID="txtRollSSC" runat="server" CssClass="form-control" type="number"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="txtRollSSCValidator"
                                                ValidationGroup="SUBMITVerifyInformation" ControlToValidate="txtRollSSC"
                                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                ErrorMessage="Required" />
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-2">
                                        <div class="form-group">
                                            <label>Reg. Number<span class="spanAsterisk">*</span></label>
                                            <asp:TextBox ID="txtRegSSC" runat="server" CssClass="form-control" type="number"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator10"
                                                ValidationGroup="SUBMITVerifyInformation" ControlToValidate="txtRegSSC"
                                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                ErrorMessage="Required" />
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-2">
                                        <div class="form-group">
                                            <label>Passing Year<span class="spanAsterisk">*</span></label>
                                            <asp:DropDownList ID="ddlPassYearSSC" runat="server" CssClass="form-control"></asp:DropDownList>
                                            <asp:CompareValidator ID="PassYearSSCvalidator" runat="server"
                                                ControlToValidate="ddlPassYearSSC" ErrorMessage="Required"
                                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMITVerifyInformation"></asp:CompareValidator>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-2">
                                        <div class="form-group">
                                            <label>Board<span class="spanAsterisk">*</span></label>
                                            <asp:DropDownList ID="ddlBoardSSC" runat="server" CssClass="form-control"></asp:DropDownList>
                                            <asp:CompareValidator ID="BoardSSCvalidator" runat="server"
                                                ControlToValidate="ddlBoardSSC" ErrorMessage="Required"
                                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMITVerifyInformation"></asp:CompareValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- HSC Information -->
                        <div class="panel-modern" style="margin-top: 1.5rem;">
                            <div class="panel-modern-header">
                                <i class="fas fa-university"></i>HSC Information
                            </div>
                            <div class="panel-modern-body">
                                <div class="row">
                                    <div class="col-sm-12 col-md-4">
                                        <div class="form-group">
                                            <label>Examination<span class="spanAsterisk">*</span></label>
                                            <asp:DropDownList ID="ddlExamTypeHSC" runat="server" CssClass="form-control">
                                                <asp:ListItem Enabled="true" Text="--Select Exam--" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="HSC, HSC (Vocational), Alim" Value="hsc" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareValidator4" runat="server"
                                                ControlToValidate="ddlExamTypeHSC" ErrorMessage="Required"
                                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMITVerifyInformation"></asp:CompareValidator>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-2">
                                        <div class="form-group">
                                            <label>Roll Number<span class="spanAsterisk">*</span></label>
                                            <asp:TextBox ID="txtRollHSC" runat="server" CssClass="form-control" type="number"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6"
                                                ValidationGroup="SUBMITVerifyInformation" ControlToValidate="txtRollHSC"
                                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                ErrorMessage="Required" />
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-2">
                                        <div class="form-group">
                                            <label>Reg. Number<span class="spanAsterisk">*</span></label>
                                            <asp:TextBox ID="txtRegHSC" runat="server" CssClass="form-control" type="number"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator9"
                                                ValidationGroup="SUBMITVerifyInformation" ControlToValidate="txtRegHSC"
                                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                ErrorMessage="Required" />
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-2">
                                        <div class="form-group">
                                            <label>Passing Year<span class="spanAsterisk">*</span></label>
                                            <asp:DropDownList ID="ddlPassYearHSC" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="-1">--Select Year--</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareValidator5" runat="server"
                                                ControlToValidate="ddlPassYearHSC" ErrorMessage="Required"
                                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMITVerifyInformation"></asp:CompareValidator>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-2">
                                        <div class="form-group">
                                            <label>Board<span class="spanAsterisk">*</span></label>
                                            <asp:DropDownList ID="ddlBoardHSC" runat="server" CssClass="form-control"></asp:DropDownList>
                                            <asp:CompareValidator ID="CompareValidator6" runat="server"
                                                ControlToValidate="ddlBoardHSC" ErrorMessage="Required"
                                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                                ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMITVerifyInformation"></asp:CompareValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <%--<asp:Button ID="Button2" runat="server" Text="Modal test"
                            class="btn btn-danger pull-right" OnClick="Button2_Click" />--%>
                    </div>

                    <asp:Button ID="btnVerifyInformation" runat="server" Text="Verify Information"
                        class="btn-verify" ValidationGroup="SUBMITVerifyInformation" OnClientClick="scrollToModalTop()" OnClick="btnVerifyInformation_Click" />
                </asp:Panel>
                <%--======================== END SSC HSC Input Section ========================--%>

                <%--======================== Basic Info Panel ========================--%>
                <asp:Panel ID="panelBasicInfo" runat="server" Visible="false">
                    <div class="modern-card" style="border: 2px solid #FCD34D;">

                        <div class="form-group">
                            <label><i class="fas fa-user"></i>Full Name<span class="spanAsterisk">*</span></label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter your full name" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="NameReq"
                                ValidationGroup="SUBMIT" ControlToValidate="txtName"
                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                ErrorMessage="Name Required" ValidationExpression="^[A-Za-z-._ ]*$" />
                        </div>

                        <div class="form-group">
                            <label><i class="fas fa-calendar"></i>Date of Birth<span class="spanAsterisk">*</span></label>
                            <div class="row">
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlDay" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlBirthDate_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator11" runat="server"
                                        ValidationGroup="SUBMIT" ControlToValidate="ddlDay"
                                        ErrorMessage="Day required" Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual"></asp:CompareValidator>
                                </div>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlMonth" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlBirthDate_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator12" runat="server"
                                        ValidationGroup="SUBMIT" ControlToValidate="ddlDay"
                                        ErrorMessage="Month required" Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual"></asp:CompareValidator>
                                </div>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlYear" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlBirthDate_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator13" runat="server"
                                        ValidationGroup="SUBMIT" ControlToValidate="ddlMonth"
                                        ErrorMessage="Year required" Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual"></asp:CompareValidator>
                                </div>
                            </div>
                            <span id="txtDateOfBirthValidateMassage" runat="server" style="font-weight: bold; color: crimson;"></span>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label><i class="fas fa-envelope"></i>Email<span class="spanAsterisk">*</span></label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"
                                        placeholder="example@email.com" TextMode="Email"
                                        onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                    <small style="color: #D97706;">Please provide a valid email address.</small>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtEmail"
                                        ValidationGroup="SUBMIT"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ErrorMessage="Please enter valid email. (Ex: xyz@zyz.com)">   
                                    </asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1"
                                        ValidationGroup="SUBMIT" ControlToValidate="txtEmail"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ErrorMessage="Email Required" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label><i class="fas fa-venus-mars"></i>Gender<span class="spanAsterisk">*</span></label>
                                    <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <asp:CompareValidator ID="ddlGenderComV" runat="server"
                                        ControlToValidate="ddlGender" ErrorMessage="Gender required"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label><i class="fas fa-mobile-alt"></i>Mobile No. (for SMS)<span class="spanAsterisk">*</span></label>
                                    <div style="display: flex; gap: 0.5rem;">
                                        <asp:TextBox ID="txtCountryCodeSMSMobile" runat="server" CssClass="form-control" Text="+88" Style="width: 70px;" ReadOnly="true"></asp:TextBox>
                                        <div style="flex: 1;">
                                            <asp:TextBox ID="txtSmsMobile" runat="server" type="number" CssClass="form-control" placeholder="017XXXXXXXX" MaxLength="11" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </div>
                                    </div>
                                    <asp:RegularExpressionValidator runat="server" ID="mobileReg"
                                        ValidationGroup="SUBMIT" Font-Size="9pt" ForeColor="Crimson"
                                        Display="Dynamic" ErrorMessage="Invalid format"
                                        ControlToValidate="txtSmsMobile"
                                        ValidationExpression="^(?:)?01[13-9]\d{8}$"></asp:RegularExpressionValidator>
                                    <small style="color: #D97706;">Candidate will not receive Username and Password if number is in wrong format.</small>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2"
                                        ValidationGroup="SUBMIT" ControlToValidate="txtSmsMobile"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ErrorMessage="Mobile Number Required" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label><i class="fas fa-phone"></i>Guardian Mobile No.<span class="spanAsterisk">*</span></label>
                                    <div style="display: flex; gap: 0.5rem;">
                                        <asp:TextBox ID="txtCountryCodeGuardianMobile" runat="server" CssClass="form-control" Text="+88" Style="width: 70px;" ReadOnly="true"></asp:TextBox>
                                        <div style="flex: 1;">
                                            <asp:TextBox ID="txtGuardianMobile" runat="server" type="number" CssClass="form-control" placeholder="017XXXXXXXX" MaxLength="11" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        </div>
                                    </div>
                                    <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1"
                                        ValidationGroup="SUBMIT" Font-Size="9pt" ForeColor="Crimson"
                                        Display="Dynamic" ErrorMessage="Invalid format"
                                        ControlToValidate="txtGuardianMobile"
                                        ValidationExpression="^(?:)?01[13-9]\d{8}$"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3"
                                        ValidationGroup="SUBMIT" ControlToValidate="txtGuardianMobile"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ErrorMessage="Guardian Number Required" />
                                </div>
                            </div>
                        </div>

                        <%--<div class="form-group">
                            <label>Quota<span class="spanAsterisk">*</span></label>
                            <asp:DropDownList ID="ddlQuota" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:CompareValidator ID="ddlQuotaComV" runat="server"
                                ControlToValidate="ddlQuota" ErrorMessage="Quota Required"
                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                        </div>--%>
                    </div>
                </asp:Panel>
                <%--======================== END Basic Info Panel ========================--%>

                <%--======================== O/A Level Input Section ========================--%>
                <asp:Panel ID="panelOALevelInputSection" runat="server" Visible="false">
                    <div class="modern-card" style="border: 2px solid #FCD34D;">
                        <h4 style="margin-bottom: 1rem;"><i class="fas fa-book"></i>O-Level and A-Level Information</h4>
                        <hr style="margin: 0.5rem 0 1.5rem 0; border-color: #FCD34D;" />

                        <div class="info-box" style="margin-bottom: 1.5rem;">
                            <strong><i class="fas fa-calculator"></i>Grade Conversion Info!</strong>
                            <p style="margin-top: 0.5rem;">Grade Points Calculation process for <strong>English Medium Students</strong> - Calculate the Average Grade Points using the table considering all subjects.</p>
                            <div style="overflow-x: auto;">
                                <table class="grade-table" style="min-width: 300px;">
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
                                    </tr>
                                </table>
                            </div>
                        </div>

                        <div class="two-col-grid">
                            <!-- O-Level Panel -->
                            <div class="panel-modern">
                                <div class="panel-modern-header">
                                    <i class="fas fa-certificate"></i>O-Level
                                </div>
                                <div class="panel-modern-body">
                                    <div class="form-group">
                                        <label>Institute<span class="spanAsterisk">*</span></label>
                                        <asp:TextBox ID="txtOLevelInstitute" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7"
                                            ValidationGroup="btnOALevelCalculate" ControlToValidate="txtOLevelInstitute"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ErrorMessage="Required" />
                                    </div>

                                    <div class="form-group">
                                        <label>Education Board<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlOLevelEducationBoard" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator9" runat="server"
                                            ControlToValidate="ddlOLevelEducationBoard" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Passing Year<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlPassYearOLevel" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator7" runat="server"
                                            ControlToValidate="ddlPassYearOLevel" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Subject-1<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlOLevelSubject1" runat="server" CssClass="form-control">
                                            <asp:ListItem Enabled="true" Text="--Select Grade--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                            <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                            <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                            <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                            <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="ddlOLevelSubject1_CV" runat="server"
                                            ControlToValidate="ddlOLevelSubject1" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Subject-2<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlOLevelSubject2" runat="server" CssClass="form-control">
                                            <asp:ListItem Enabled="true" Text="--Select Grade--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                            <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                            <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                            <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                            <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="ddlOLevelSubject2_CV" runat="server"
                                            ControlToValidate="ddlOLevelSubject2" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Subject-3<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlOLevelSubject3" runat="server" CssClass="form-control">
                                            <asp:ListItem Enabled="true" Text="--Select Grade--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                            <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                            <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                            <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                            <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="ddlOLevelSubject3_CV" runat="server"
                                            ControlToValidate="ddlOLevelSubject3" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Subject-4<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlOLevelSubject4" runat="server" CssClass="form-control">
                                            <asp:ListItem Enabled="true" Text="--Select Grade--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                            <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                            <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                            <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                            <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="ddlOLevelSubject4_CV" runat="server"
                                            ControlToValidate="ddlOLevelSubject4" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Subject-5<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlOLevelSubject5" runat="server" CssClass="form-control">
                                            <asp:ListItem Enabled="true" Text="--Select Grade--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                            <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                            <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                            <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                            <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="ddlOLevelSubject5_CV" runat="server"
                                            ControlToValidate="ddlOLevelSubject5" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <hr />

                                    <div class="result-box">
                                        <span style="font-weight: 600;">O-Level Points</span><br />
                                        <asp:Label ID="lblOLevelResult" runat="server" Text="" CssClass="result-label"></asp:Label>
                                        <asp:HiddenField ID="hfOLevelConvertedSscGPA" runat="server" />
                                    </div>
                                </div>
                            </div>

                            <!-- A-Level Panel -->
                            <div class="panel-modern">
                                <div class="panel-modern-header">
                                    <i class="fas fa-award"></i>A-Level
                                </div>
                                <div class="panel-modern-body">
                                    <div class="form-group">
                                        <label>Institute<span class="spanAsterisk">*</span></label>
                                        <asp:TextBox ID="txtALevelInstitute" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8"
                                            ValidationGroup="btnOALevelCalculate" ControlToValidate="txtALevelInstitute"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ErrorMessage="Required" />
                                    </div>

                                    <div class="form-group">
                                        <label>Education Board<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlALevelEducationBoard" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator10" runat="server"
                                            ControlToValidate="ddlALevelEducationBoard" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Passing Year<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlPassYearALevel" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator8" runat="server"
                                            ControlToValidate="ddlPassYearALevel" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Subject-1<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlALevelSubject1" runat="server" CssClass="form-control">
                                            <asp:ListItem Enabled="true" Text="--Select Grade--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                            <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                            <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                            <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                            <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="ddlALevelSubject1_CV" runat="server"
                                            ControlToValidate="ddlALevelSubject1" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Subject-2<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlALevelSubject2" runat="server" CssClass="form-control">
                                            <asp:ListItem Enabled="true" Text="--Select Grade--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                            <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                            <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                            <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                            <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="ddlALevelSubject2_CV" runat="server"
                                            ControlToValidate="ddlALevelSubject2" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <hr />

                                    <div class="result-box">
                                        <span style="font-weight: 600;">A-Level Points</span><br />
                                        <asp:Label ID="lblALevelResult" runat="server" Text="" CssClass="result-label"></asp:Label>
                                        <asp:HiddenField ID="hfALevelConvertedHscGPA" runat="server" />
                                    </div>

                                    <div class="panel-modern" style="margin-top: 1rem; background: #F3F4F6;">
                                        <div class="panel-modern-body">
                                            <div>
                                                <b><i class="fas fa-calculator"></i>Total Points : </b>
                                                <asp:Label ID="lblTotalPoints" runat="server" CssClass="result-label"></asp:Label>
                                            </div>
                                            <div style="margin-top: 1rem;">
                                                <asp:Button ID="btnCalculateALevel" runat="server" Text="Calculate"
                                                    OnClick="btnCalculateOAndALevel_Click" ValidationGroup="btnOALevelCalculate"
                                                    CssClass="btn-calculate" />
                                            </div>
                                            <asp:Panel ID="messagePanelOALevel" runat="server" Style="margin-top: 1rem;">
                                                <asp:Label ID="lblMassageOALevel" runat="server" Text=""></asp:Label>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <%--======================== END O/A Level Input Section ========================--%>

                <%--======================== IB Input Section ========================--%>
                <asp:Panel ID="panelIBInputSection" runat="server" Visible="false">
                    <div class="modern-card" style="border: 2px solid #FCD34D;">
                        <h4 style="margin-bottom: 1rem;"><i class="fas fa-globe"></i>International Baccalaureate</h4>
                        <hr style="margin: 0.5rem 0 1.5rem 0; border-color: #FCD34D;" />

                        <div class="panel-modern">
                            <div class="panel-modern-header">
                                <i class="fas fa-certificate"></i>International Baccalaureate
                            </div>
                            <div class="panel-modern-body">
                                <div class="form-group">
                                    <label>Institute<span class="spanAsterisk">*</span></label>
                                    <asp:TextBox ID="txtIBInstitute" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4"
                                        ValidationGroup="btnIBCalculate" ControlToValidate="txtIBInstitute"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ErrorMessage="Required" />
                                </div>

                                <div class="form-group">
                                    <label>Education Board<span class="spanAsterisk">*</span></label>
                                    <asp:TextBox ID="txtIBEducationBoard" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5"
                                        ValidationGroup="btnIBCalculate" ControlToValidate="txtIBEducationBoard"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ErrorMessage="Required" />
                                </div>

                                <div class="form-group">
                                    <label>Passing Year<span class="spanAsterisk">*</span></label>
                                    <asp:DropDownList ID="ddlIBPassingYear" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator2" runat="server"
                                        ControlToValidate="ddlIBPassingYear" ErrorMessage="Required"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual"
                                        ValidationGroup="btnIBCalculate">
                                    </asp:CompareValidator>
                                </div>

                                <div class="form-group">
                                    <label>Subject-1<span class="spanAsterisk">*</span></label>
                                    <asp:DropDownList ID="ddlIBSubject1" runat="server" CssClass="form-control">
                                        <asp:ListItem Enabled="true" Text="--Select Rating--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator3" runat="server"
                                        ControlToValidate="ddlIBSubject1" ErrorMessage="Required"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual"
                                        ValidationGroup="btnIBCalculate">
                                    </asp:CompareValidator>
                                </div>

                                <div class="form-group">
                                    <label>Subject-2<span class="spanAsterisk">*</span></label>
                                    <asp:DropDownList ID="ddlIBSubject2" runat="server" CssClass="form-control">
                                        <asp:ListItem Enabled="true" Text="--Select Rating--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator14" runat="server"
                                        ControlToValidate="ddlIBSubject2" ErrorMessage="Required"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual"
                                        ValidationGroup="btnIBCalculate">
                                    </asp:CompareValidator>
                                </div>

                                <div class="form-group">
                                    <label>Subject-3<span class="spanAsterisk">*</span></label>
                                    <asp:DropDownList ID="ddlIBSubject3" runat="server" CssClass="form-control">
                                        <asp:ListItem Enabled="true" Text="--Select Rating--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator15" runat="server"
                                        ControlToValidate="ddlIBSubject3" ErrorMessage="Required"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual"
                                        ValidationGroup="btnIBCalculate">
                                    </asp:CompareValidator>
                                </div>

                                <div class="form-group">
                                    <label>Subject-4<span class="spanAsterisk">*</span></label>
                                    <asp:DropDownList ID="ddlIBSubject4" runat="server" CssClass="form-control">
                                        <asp:ListItem Enabled="true" Text="--Select Rating--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator16" runat="server"
                                        ControlToValidate="ddlIBSubject4" ErrorMessage="Required"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual"
                                        ValidationGroup="btnIBCalculate">
                                    </asp:CompareValidator>
                                </div>

                                <div class="form-group">
                                    <label>Subject-5<span class="spanAsterisk">*</span></label>
                                    <asp:DropDownList ID="ddlIBSubject5" runat="server" CssClass="form-control">
                                        <asp:ListItem Enabled="true" Text="--Select Rating--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator17" runat="server"
                                        ControlToValidate="ddlIBSubject5" ErrorMessage="Required"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual"
                                        ValidationGroup="btnIBCalculate">
                                    </asp:CompareValidator>
                                </div>

                                <div class="form-group">
                                    <label>Subject-6<span class="spanAsterisk">*</span></label>
                                    <asp:DropDownList ID="ddlIBSubject6" runat="server" CssClass="form-control">
                                        <asp:ListItem Enabled="true" Text="--Select Rating--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator18" runat="server"
                                        ControlToValidate="ddlIBSubject5" ErrorMessage="Required"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual"
                                        ValidationGroup="btnIBCalculate">
                                    </asp:CompareValidator>
                                </div>

                                <hr />

                                <div class="result-box">
                                    <span style="font-weight: 600;">International Baccalaureate Points</span><br />
                                    <asp:Label ID="lblIBResult" runat="server" Text="" CssClass="result-label"></asp:Label>
                                    <asp:HiddenField ID="hfIBConvertedSscGPA" runat="server" />

                                    <div style="margin-top: 1rem;">
                                        <asp:Button ID="btnCalculatIB" runat="server" Text="Calculate"
                                            OnClick="btnCalculatIB_Click" ValidationGroup="btnIBCalculate"
                                            CssClass="btn-calculate" />
                                    </div>

                                    <asp:Panel ID="messagePanelIB" runat="server" Style="margin-top: 1rem;">
                                        <asp:Label ID="lblMassageIB" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <%--======================== END IB Input Section ========================--%>

                <%--======================== O/A Level Appeared Input Section ========================--%>
                <asp:Panel ID="panelOALevelAppearedInputSection" runat="server" Visible="false">
                    <div class="modern-card" style="border: 2px solid #FCD34D;">

                        <div class="two-col-grid">
                            <!-- SSC/Equivalent Panel -->
                            <div class="panel-modern">
                                <div class="panel-modern-header">
                                    <i class="fas fa-school"></i>SSC/Equivalent
                                </div>
                                <div class="panel-modern-body">
                                    <div class="form-group">
                                        <label>Institute<span class="spanAsterisk">*</span></label>
                                        <asp:TextBox ID="txtOLevelAppearedInstitute" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator12"
                                            ValidationGroup="btnOALevelAppearedCalculate" ControlToValidate="txtOLevelAppearedInstitute"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ErrorMessage="Required" />
                                    </div>

                                    <div class="form-group">
                                        <label>Education Board<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlOLevelAppearedEducationBoard" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator19" runat="server"
                                            ControlToValidate="ddlOLevelAppearedEducationBoard" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelAppearedCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Passing Year<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlPassYearOLevelAppeared" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator20" runat="server"
                                            ControlToValidate="ddlPassYearOLevelAppeared" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelAppearedCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Subject-1<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlOLevelAppearedSubject1" runat="server" CssClass="form-control">
                                            <asp:ListItem Enabled="true" Text="--Select Grade--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                            <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                            <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                            <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                            <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator21" runat="server"
                                            ControlToValidate="ddlOLevelAppearedSubject1" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelAppearedCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Subject-2<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlOLevelAppearedSubject2" runat="server" CssClass="form-control">
                                            <asp:ListItem Enabled="true" Text="--Select Grade--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                            <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                            <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                            <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                            <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator22" runat="server"
                                            ControlToValidate="ddlOLevelAppearedSubject2" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelAppearedCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Subject-3<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlOLevelAppearedSubject3" runat="server" CssClass="form-control">
                                            <asp:ListItem Enabled="true" Text="--Select Grade--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                            <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                            <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                            <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                            <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator23" runat="server"
                                            ControlToValidate="ddlOLevelAppearedSubject3" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelAppearedCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Subject-4<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlOLevelAppearedSubject4" runat="server" CssClass="form-control">
                                            <asp:ListItem Enabled="true" Text="--Select Grade--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                            <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                            <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                            <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                            <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator24" runat="server"
                                            ControlToValidate="ddlOLevelAppearedSubject4" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelAppearedCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Subject-5<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlOLevelAppearedSubject5" runat="server" CssClass="form-control">
                                            <asp:ListItem Enabled="true" Text="--Select Grade--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                            <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                            <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                            <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                            <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator25" runat="server"
                                            ControlToValidate="ddlOLevelAppearedSubject5" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelAppearedCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <hr />

                                    <div class="result-box">
                                        <span style="font-weight: 600;">SSC/Equivalent Points</span><br />
                                        <asp:Label ID="lblOLevelAppearedResult" runat="server" Text="" CssClass="result-label"></asp:Label>
                                        <asp:HiddenField ID="hfOLevelAppearedConvertedSscGPA" runat="server" />
                                    </div>
                                </div>
                            </div>

                            <!-- HSC/Equivalent Panel -->
                            <div class="panel-modern">
                                <div class="panel-modern-header">
                                    <i class="fas fa-university"></i>HSC/Equivalent
                                </div>
                                <div class="panel-modern-body">
                                    <div class="form-group">
                                        <label>Institute</label>
                                        <asp:TextBox ID="txtALevelAppearedInstitute" runat="server" CssClass="form-control" Text="Institute" Enabled="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator13"
                                            ValidationGroup="btnOALevelAppearedCalculate" ControlToValidate="txtALevelAppearedInstitute"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ErrorMessage="Required" />
                                    </div>

                                    <div class="form-group">
                                        <label>Education Board</label>
                                        <asp:DropDownList ID="ddlALevelAppearedEducationBoard" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator26" runat="server"
                                            ControlToValidate="ddlALevelAppearedEducationBoard" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelAppearedCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Passing Year</label>
                                        <asp:DropDownList ID="ddlPassYearALevelAppeared" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator27" runat="server"
                                            ControlToValidate="ddlPassYearALevelAppeared" ErrorMessage="Required"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ValueToCompare="-1" Operator="NotEqual"
                                            ValidationGroup="btnOALevelAppearedCalculate">
                                        </asp:CompareValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Subject-1</label>
                                        <asp:DropDownList ID="ddlALevelAppearedSubject1" runat="server" CssClass="form-control" Enabled="false">
                                            <asp:ListItem Enabled="true" Text="--Select Grade--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                            <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                            <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                            <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                            <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <div class="form-group">
                                        <label>Subject-2</label>
                                        <asp:DropDownList ID="ddlALevelAppearedSubject2" runat="server" CssClass="form-control" Enabled="false">
                                            <asp:ListItem Enabled="true" Text="--Select Grade--" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="A*/A" Value="5.00"></asp:ListItem>
                                            <asp:ListItem Text="B" Value="4.00"></asp:ListItem>
                                            <asp:ListItem Text="C" Value="3.50"></asp:ListItem>
                                            <asp:ListItem Text="D" Value="3.00"></asp:ListItem>
                                            <asp:ListItem Text="E" Value="0.00"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <hr />

                                    <div class="result-box">
                                        <span style="font-weight: 600;">HSC/Equivalent Points</span><br />
                                        <asp:Label ID="lblALevelAppearedResult" runat="server" Text="" CssClass="result-label"></asp:Label>
                                        <asp:HiddenField ID="hfALevelAppearedConvertedHscGPA" runat="server" />
                                    </div>

                                    <div class="panel-modern" style="margin-top: 1rem; background: #F3F4F6;">
                                        <div class="panel-modern-body">
                                            <div>
                                                <b><i class="fas fa-calculator"></i>Total Points : </b>
                                                <asp:Label ID="lblTotalPointsAppeared" runat="server" CssClass="result-label"></asp:Label>
                                            </div>
                                            <div style="margin-top: 1rem;">
                                                <asp:Button ID="btnCalculateALevelAppeared" runat="server" Text="Calculate"
                                                    OnClick="btnCalculateALevelAppeared_Click" ValidationGroup="btnOALevelAppearedCalculate"
                                                    CssClass="btn-calculate" />
                                            </div>
                                            <asp:Panel ID="messagePanelOALevelAppeared" runat="server" Style="margin-top: 1rem;">
                                                <asp:Label ID="lblMassageOALevelAppeared" runat="server" Text=""></asp:Label>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <%--======================== END O/A Level Appeared Input Section ========================--%>

                <%--======================== Submit Button Section ========================--%>
                <asp:Panel ID="panelSubmitButtonSection" runat="server" Visible="false">
                    <div class="submit-section">
                        <div class="captcha-container">
                            <img runat="server" id="imgCtrl" style="border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);" />
                        </div>

                        <asp:Panel ID="captchaMsg" runat="server" Visible="false">
                            <div style="background: #FEE2E2; color: #DC2626; padding: 0.75rem; border-radius: 8px; margin: 1rem auto; max-width: 400px;">
                                <asp:Label ID="lblCaptcha" runat="server"
                                    Text="Sorry your text and image didn't match. Please try again."></asp:Label>
                            </div>
                        </asp:Panel>

                        <div style="margin-top: 1rem;">
                            <label style="font-weight: 600; display: block; margin-bottom: 0.5rem;">
                                <i class="fas fa-shield-alt"></i>Enter the code shown above
                            </label>
                            <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control captcha"
                                Style="max-width: 300px; margin: 0 auto; text-align: center;" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator11"
                                ValidationGroup="SUBMIT" ControlToValidate="txtCaptcha"
                                Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                ErrorMessage="Captcha Required" />
                        </div>

                        <div style="margin-top: 2rem;">
                            <asp:Button ID="btnSubmit" runat="server" Text="Next →"
                                CssClass="btn-submit" ValidationGroup="SUBMIT"
                                OnClick="btnSubmit_Click" />
                        </div>
                    </div>

                    <asp:Panel ID="messagePanelBottom" runat="server" CssClass="message-panel" Style="margin-top: 1rem;">
                        <asp:Label ID="lblMessageBottom" runat="server" Text=""></asp:Label>
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </asp:Panel>
                </asp:Panel>
                <%--======================== END Submit Button Section ========================--%>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    <!-- END Main Card -->

    <%--======================== Modal Popup ========================--%>
    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
        <ContentTemplate>
            <asp:Button ID="Button1" runat="server" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender ID="modalPopupAlert" runat="server"
                TargetControlID="Button1"
                PopupControlID="Panel2"
                BackgroundCssClass="modalBackground"
                DropShadow="true">
            </ajaxToolkit:ModalPopupExtender>

            <asp:Panel runat="server" ID="Panel2"
                Style="display: none;"
                CssClass="modalPopup">

                <asp:HiddenField ID="hdnCandidateUserId" runat="server" />

                <!-- Modal Header -->
                <div style="background: linear-gradient(145deg, var(--primary), var(--secondary)); color: white; padding: 1.5rem; border-radius: 16px 16px 0 0; text-align: center;">
                    <h3 style="margin: 0; font-weight: 600;">Form Submitted Successfully!</h3>
                </div>

                <!-- Modal Body -->
                <div style="padding: 2rem; overflow-y: auto;">
                    <div style="background: linear-gradient(135deg, #EFF6FF, #DBEAFE); border-radius: 12px; padding: 1.5rem; margin-bottom: 1.5rem; border: 2px solid #93C5FD;">

                        <div style="margin-bottom: 1.25rem; padding-bottom: 1rem; border-bottom: 2px dashed #93C5FD;">
                            <div style="color: #6B7280; font-size: 0.9rem; margin-bottom: 0.25rem;">Candidate Name</div>
                            <asp:Label ID="lblAlertName" runat="server" Text=""
                                Style="font-size: 1.25rem; font-weight: 600; color: var(--dark); display: block;"></asp:Label>
                        </div>

                        <div style="margin-bottom: 1.25rem; text-align: center; padding: 1rem; background: white; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);">
                            <div style="color: #6B7280; font-size: 0.9rem; margin-bottom: 0.5rem;">
                                <i class="fas fa-credit-card"></i>Payment ID
                            </div>
                            <asp:Label ID="lblAlertPaymentId" runat="server" Text=""
                                Style="font-size: 1.75rem; color: var(--accent); font-weight: bold; letter-spacing: 1px; display: block; font-family: monospace;"></asp:Label>
                        </div>

                        <div style="background: linear-gradient(135deg, #FEF2F2, #FEE2E2); border-left: 4px solid var(--danger); padding: 1.25rem; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.05);">
                            <div style="display: flex; align-items: flex-start; gap: 0.75rem;">
                                <i class="fas fa-exclamation-triangle"
                                    style="color: var(--danger); font-size: 1.5rem; margin-top: 0.25rem;"></i>
                                <asp:Label ID="lblAlertMessage" runat="server"
                                    Text="Please note down your payment ID and save this number for future reference. Please fill up your all information and make your payment"
                                    Font-Bold="true"
                                    ForeColor="#DC2626"
                                    Style="line-height: 1.6;"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <!-- Action Button -->
                    <div style="text-align: center; padding-top: 1rem;">
                        <asp:LinkButton ID="btnOk" runat="server" OnClick="btnOk_Click"
                            Style="display: inline-block; background: linear-gradient(145deg, #10B981, #059669); color: white; padding: 1rem 3rem; border-radius: 10px; text-decoration: none; font-weight: 600; font-size: 1.1rem; transition: all 0.3s ease; box-shadow: 0 4px 6px rgba(16, 185, 129, 0.3); border: none; cursor: pointer;">
                            <i class="fas fa-check-circle"></i> Okay
                        </asp:LinkButton>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--======================== END Modal Popup ========================--%>

    <%--======================== COMMENTED OUT OLD CODE SECTION ========================--%>
    <%--    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h4>Purchase Form</h4>
                </div>
                <div class="panel-body">--%>

    <%--<div class="row">
                        <div class="col-lg-4"></div>
                        <div class="col-lg-4">
                        </div>
                        <div class="col-lg-4"></div>
                    </div>

                    <div class="row">
                        <div class="col-lg-12" style="text-align: center; font-weight: bold; color: orangered">
                        </div>
                    </div>

                    <br />--%>

    <%------------------------------------------------ Message Top -----------------------------------------%>

    <%--
                    <asp:UpdatePanel runat="server" ID="upanel">
                        <ContentTemplate>
                            <div class="panel panel-warning">
                                <div class="panel-body">--%>



    <%----------------------------- Full Name --------------------------------------%>
    <%--<div class="row">
                                        <div class="col-lg-3">
                                        </div>
                                        <div class="col-lg-6">
                                        </div>
                                        <div class="col-lg-3">
                                        </div>
                                    </div>

                                    <br />--%>

    <%----------------------------- Date of Birth --------------------------------------%>
    <%--<div class="row">
                                        <div class="col-lg-3">
                                            <strong>Date of Birth</strong> <span class="spanAsterisk">*</span>
                                        </div>
                                        <div class="col-lg-6">
                                            <asp:TextBox ID="txtDateOfBirth" runat="server" Width="65%" CssClass="form-control"
                                                placeholder="Date of Birth (DD/MM/YYYY)"
                                                onkeydown="return (event.keyCode!=13);"
                                                MaxLength="10"
                                                AutoPostBack="True"
                                                OnTextChanged="DateOfBirth_TextChanged"></asp:TextBox>

                                            <span id="txtDateOfBirthValidateMassage" runat="server" style="font-weight: bold; color: crimson;"></span>
                                        </div>
                                        <div class="col-lg-3">
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

                                            <br />

                                            <asp:RegularExpressionValidator runat="server"
                                                ControlToValidate="txtDateOfBirth"
                                                ValidationExpression="(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((18|19|20)\d\d))$"
                                                Display="Dynamic"
                                                Font-Size="9pt"
                                                ForeColor="Crimson"
                                                Font-Bold="true"
                                                ErrorMessage="Invalid date format. Ex: dd/MM/yyyy"
                                                ValidationGroup="SUBMIT" />

                                        </div>
                                    </div>

                                    <br />--%>

    <%----------------------------- Email --------------------------------------%>
    <%--<div class="row">
                                        <div class="col-lg-3">
                                            <strong>Email</strong> <span class="spanAsterisk">*</span>
                                        </div>
                                        <div class="col-lg-6">
                                            <asp:TextBox ID="txtEmail" runat="server" Width="65%" CssClass="form-control"
                                                placeholder="Email Address"
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
                                        </div>
                                        <div class="col-lg-3">
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

                                    <br />--%>

    <%----------------------------- Mobile No --------------------------------------%>
    <%--<div class="row">
                                        <div class="col-lg-3">
                                            <strong></strong> <span class="spanAsterisk">*</span>
                                        </div>
                                        <div class="col-lg-6">
                                        </div>
                                        <div class="col-lg-3">
                                            
                                        </div>
                                    </div>

                                    <br />--%>

    <%----------------------------- Guardian Mobile No --------------------------------------%>
    <%--<div class="row">
                                        <div class="col-lg-3">
                                            <strong></strong><span class="spanAsterisk">*</span>
                                        </div>
                                        <div class="col-lg-6">
                                        </div>
                                        <div class="col-lg-3">
                                            
                                        </div>
                                    </div>

                                    <br />--%>

    <%----------------------------- Gender --------------------------------------%>
    <%--<div class="row">
                                        <div class="col-lg-3">
                                            <strong></strong><span class="spanAsterisk">*</span>
                                        </div>
                                        <div class="col-lg-6">
                                            
                                        </div>
                                        <div class="col-lg-3">
                                            
                                        </div>
                                    </div>

                                    <br />--%>

    <%----------------------------- Quota --------------------------------------%>
    <%--<div class="row">
                                        <div class="col-lg-3">
                                            <strong></strong><span class="spanAsterisk">*</span>
                                        </div>
                                        <div class="col-lg-6">
                                            
                                        </div>
                                        <div class="col-lg-3">
                                            
                                        </div>
                                    </div>--%>
    <%--</div>
                            </div>


                            <div class="panel panel-warning">
                                <div class="panel-body">--%>

    <%----------------------------- Education Choice --------------------------------------%>


    <br />

    <%----------------------------- Education Input Section (SSC/HSC) --------------------------------------%>


    <%----------------------------- Education Input Section (O/A Level) --------------------------------------%>

    <%--<div id="educationInputSectionOALevel" runat="server" visible="false">
                                        <div class="panel panel-default" style="margin-bottom: 0px;">
                                            <div class="panel-body" style="padding-bottom: 0px;">

                                                <div class="panel panel-default">
                                                    <div class="panel-heading" style="text-align: center;"><strong>O/A-Level Grade Conversion</strong></div>
                                                    <div class="panel-body" style="padding-bottom: 0px;">--%>

    <%----------------------------- Grade Points Calculation Info --------------------------------------%>
    <%--<div class="alert alert-warning" style="margin-bottom: 0px; padding-bottom: 0px;">
                                                            <strong>Information:</strong> Grade Points Calculation process for <strong>English Medium Students</strong> - Calculate the Average Grade Points using the table considering all subjects.</span>
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
                                                                </tr>
                                                            </table>
                                                        </div>

                                                        <br />--%>
    <%--</div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>--%>
    <%--</div>
                            </div>


                        </ContentTemplate>
                    </asp:UpdatePanel>--%>



    <%------------------------------------------------ Submit Button, Captcha -----------------------------------------%>
    <%--<asp:UpdatePanel runat="server" ID="UpdatePanel1">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td>

                                        <br />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </ContentTemplate>

                    </asp:UpdatePanel>--%>

    <%--  <br />--%>

    <%------------------------------------------------ Message Bottom -----------------------------------------%>
    <%--</div>
            </div>--%>
    <%-- ROW --%>
    <%--</div>
    </div>--%>

    <%--======================== END COMMENTED OUT OLD CODE SECTION ========================--%>

    <%--======================== Animation Extender ========================--%>
    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="UpdatePanelAll" runat="server">
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
                    <EnableAction AnimationTarget="btnSubmit" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
