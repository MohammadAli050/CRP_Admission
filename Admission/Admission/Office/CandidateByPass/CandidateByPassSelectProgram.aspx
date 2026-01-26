<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CandidateByPassSelectProgram.aspx.cs" Inherits="Admission.Admission.Office.CandidateByPass.CandidateByPassSelectProgram" %>

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
            --border-color: #E5E7EB;
            --transition: all 0.3s cubic-bezier(0.25, 0.8, 0.25, 1);
            --shadow-sm: 0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24);
            --shadow-md: 0 4px 6px rgba(0,0,0,0.1);
            --shadow-lg: 0 10px 25px rgba(0,0,0,0.1);
            --radius-sm: 4px;
            --radius-md: 8px;
            --radius-lg: 16px;
        }

        /* Modern Card Design */
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
            transform: translateY(-2px);
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

        .card-header-modern strong {
            display: block;
            line-height: 1.6;
        }

        .card-body-modern {
            padding: 2rem;
        }

        /* Table Styling */
        .modern-table {
            width: 100%;
            margin-bottom: 0;
            background: white;
            border-radius: var(--radius-md);
            overflow: auto;
        }

        .modern-table thead th {
            background: var(--light);
            color: var(--dark);
            font-weight: 600;
            padding: 1rem 0.75rem;
            border-bottom: 2px solid var(--border-color);
            text-align: center;
            font-size: 0.9rem;
            vertical-align: middle;
        }

        .modern-table tbody tr {
            transition: var(--transition);
            border-bottom: 1px solid var(--border-color);
        }

        .modern-table tbody tr:hover {
            background: var(--light);
            transform: scale(1.01);
        }

        .modern-table tbody td {
            padding: 1rem 0.75rem;
            vertical-align: middle;
            font-size: 0.9rem;
            color: var(--dark);
        }

        /* Checkbox Styling */
        input[type="checkbox"] {
            width: 20px;
            height: 20px;
            cursor: pointer;
            accent-color: var(--accent);
        }

        /* Form Controls */
        .form-control {
            border: 2px solid var(--border-color);
            border-radius: var(--radius-md);
            padding: 0.75rem 1rem;
            transition: var(--transition);
            font-size: 0.95rem;
        }

        .form-control:focus {
            border-color: var(--accent);
            box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
            outline: none;
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

        .btn-info-modern {
            background: var(--accent);
            color: white;
        }

        .btn-info-modern:hover {
            background: var(--secondary);
            transform: translateY(-2px);
            box-shadow: var(--shadow-md);
            color: white;
        }

        /* Summary Panel */
        .summary-panel {
            background: linear-gradient(135deg, var(--light), white);
            border: 2px solid var(--border-color);
            border-radius: var(--radius-lg);
            padding: 1.5rem;
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-top: 1rem;
            box-shadow: var(--shadow-sm);
        }

        .summary-info {
            display: flex;
            flex-direction: column;
            gap: 0.5rem;
        }

        .summary-label {
            color: var(--gray);
            font-size: 0.9rem;
            font-weight: 500;
        }

        .summary-value {
            color: var(--primary);
            font-size: 1.25rem;
            font-weight: 700;
        }

        /* Alert Styling */
        .alert-modern {
            border-radius: var(--radius-md);
            padding: 1rem 1.5rem;
            border: none;
            box-shadow: var(--shadow-sm);
        }

        .alert-warning-modern {
            background: #FEF3C7;
            color: var(--warning);
            border-left: 4px solid var(--warning);
        }

        /* Form Row Styling */
        .form-row-modern {
            display: flex;
            flex-direction: column;
            gap: 0.5rem;
            margin-bottom: 1rem;
        }

        .form-row-modern label {
            font-weight: 600;
            color: var(--dark);
            font-size: 0.9rem;
        }

        /* Payment ID Section */
        .payment-section {
            background: white;
            border-radius: var(--radius-lg);
            padding: 1.5rem;
        }

        /* Info Footer */
        .info-footer {
            background: var(--light);
            border-radius: var(--radius-lg);
            padding: 1.25rem;
            margin-top: 2rem;
            border: 2px solid var(--border-color);
            text-align: center;
            color: var(--gray);
        }

        .info-footer a {
            color: var(--accent);
            font-weight: 600;
            text-decoration: none;
            transition: var(--transition);
        }

        .info-footer a:hover {
            color: var(--secondary);
            text-decoration: underline;
        }

        /* Validator Styling */
        span[id*="Validator"] {
            display: block;
            margin-top: 0.25rem;
            font-size: 0.85rem;
        }

        /* Empty State */
        .empty-state {
            text-align: center;
            padding: 3rem 1rem;
        }

        .empty-state i {
            font-size: 3rem;
            color: var(--gray);
            margin-bottom: 1rem;
        }

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

        .spanAsterisk {
            color: crimson;
            font-size: 12pt;
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

        /* Responsive Design */
        @media (max-width: 768px) {
            .card-body-modern {
                padding: 1rem;
            }

            .summary-panel {
                flex-direction: column;
                gap: 1rem;
                text-align: center;
            }

            .modern-table {
                font-size: 0.85rem;
            }

            .modern-table thead th,
            .modern-table tbody td {
                padding: 0.5rem;
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="upanel1">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12">
                    <!-- Main Program Selection Card -->
                    <div class="">
                        <div class="modern-card">
                            <div class="card-header-modern">
                                <strong>
                                    <%-- I do not have a Payment ID <br />
                                    Select --%>
                                    <asp:Label ID="lblEducationCat" runat="server"></asp:Label>
                                    <%--<br />
                                    <asp:Label ID="Label1" runat="server" Text="These forms are only for testing. Please do not apply." ForeColor="Crimson"></asp:Label>--%>
                                </strong>
                            </div>
                            <div class="card-body-modern panelBody_edu_marginBottom">
                                <div style="overflow:auto;">
                                <asp:ListView ID="lvAdmSetup" runat="server"
                                    OnItemDataBound="lvAdmSetup_ItemDataBound"> <%--Hidden by Ariq : OnItemCommand="lvAdmSetup_ItemCommand"--%>
                                    <LayoutTemplate>
                                        <table id="tblAdmSetup" class="modern-table">
                                            <thead>
                                                <tr>
                                                    <th style="width: 5%"></th>
                                                    <th style="width: 5%"></th>
                                                    <th style="width: 30%">Faculty</th>
                                                    <th style="width: 20%">Application<br />Start Date</th>
                                                    <th style="width: 20%">Application<br />End Date</th>
                                                    <th style="width: 20%">Fee</th>
                                                    <%--<th></th>--%>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr runat="server" id="itemPlaceholder" />
                                            </tbody>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="text-align: center;">
                                                <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" OnCheckedChanged="ckbxSelectedSchool_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>'/>
                                                <asp:HiddenField ID="HiddenField2" runat="server" Value='<%# Eval("AdmissionUnitID") %>'/>
                                                <asp:HiddenField ID="HiddenField3" runat="server" Value='<%# Eval("AcaCalID") %>'/>
                                                <asp:HiddenField ID="HiddenField4" runat="server" Value='<%# Eval("Fee") %>'/>
                                                <%--<asp:Label ID="lblSchoolId" runat="server" Visible="false" Value='<%# Eval("ID") %>'/>--%>
                                                <%--<asp:HiddenFieldID="hdnfldCurrentDateTime" runat="server"  /> --%>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblUnitName" runat="server" />
                                            </td>
                                            <td style="text-align: center;">
                                                <asp:Label ID="lblStartDate" runat="server" />
                                            </td>
                                            <td style="text-align: center;">
                                                <asp:Label ID="lblEndDate" runat="server" />
                                            </td>
                                            <td style="text-align: center; font-weight: 600;">
                                                <span style="color: var(--gray); font-weight: 400;">BDT </span>
                                                <asp:Label ID="lblFee" runat="server" />
                                            </td>
                                            <%--<td style="text-align: right;">
                                                <asp:LinkButton ID="lnkViewDetails" runat="server" CssClass="btn btn-info-modern btn-sm">APPLY</asp:LinkButton>
                                            </td>--%>
                                        </tr>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <div class="empty-state">
                                            <i class="fas fa-inbox"></i>
                                            <div class="alert-modern alert-warning-modern">
                                                No program(s) opened for admission.
                                            </div>
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                                </div>
                                <%--<asp:Button ID="btnApply" runat="server" Text="APPLY" OnClick="btnApply_Click" CssClass="btn btn-info-modern"/>--%>

                                <!-- Summary Panel -->
                                <div class="summary-panel">
                                    <div class="summary-info">
                                        <span class="summary-label">Number of Selected Faculty(s):</span>
                                        <span class="summary-value">
                                            <asp:Label ID="lblNoOfSelSchool" runat="server" Text="0"></asp:Label>
                                        </span>
                                    </div>
                                    <div class="summary-info">
                                        <span class="summary-label">Total Fees:</span>
                                        <span class="summary-value">
                                            BDT <asp:Label ID="lblTotal" runat="server" Text="0"></asp:Label>
                                        </span>
                                    </div>
                                    <%-- <div></div>--%>
                                    <div>
                                        <asp:Button ID="btnApply1" runat="server" Text="APPLY" OnClick="btnApply_Click" CssClass="btn-modern btn-info-modern"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Payment ID Card -->
                    <div class="">
                        <div class="modern-card">
                            <div class="card-header-modern">
                                I have a Payment ID<br />
                                but did not complete my payment
                            </div>
                            <div class="card-body-modern panelBody_edu_marginBottom">
                                <div class="payment-section">
                                    <div class="form-row-modern">
                                        <label for="txtPaymentId">Payment ID</label>
                                        <asp:TextBox ID="txtPaymentId" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);" placeholder="Enter your Payment ID"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" Display="Dynamic"
                                            ControlToValidate="txtPaymentId" ErrorMessage="Payment ID is required" ForeColor="Crimson" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-row-modern">
                                        <asp:Button ID="Button1" runat="server" Text="Go For Payment" OnClick="btnNext_Click" ValidationGroup="gr1" CssClass="btn-modern btn-primary-modern" style="width: 100%;" />
                                        <asp:Label ID="lblOLevelResult" runat="server" Text="" Style="font-weight: bold; color: crimson; margin-top: 0.5rem;"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%-- END COL-MD-12 --%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%-- COMMENTED OUT SECTIONS - KEPT FOR REFERENCE
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Select 
                        <asp:Label ID="lblEducationCat" runat="server"></asp:Label>
                    </strong>
                </div>
                <div class="panel-body">
                </div>
            </div>
        </div>
    </div>
    --%>

    <%-- OFFLINE MASTERS PANEL - COMMENTED OUT
    <asp:Panel ID="panelOfflineMasters" runat="server" Visible="false">
        <div class="row">
            <div class="col-md-12">
                <div class="modern-card">
                    <div class="card-header-modern">
                        <strong>MPCHRS & MDHSM</strong>
                    </div>
                    <div class="card-body-modern">
                        <table class="modern-table">
                            <tbody>
                                <tr>
                                    <td>
                                        <strong>Master of Peace, Conflict and Human Rights Studies (MPCHRS)</strong>
                                    </td>
                                    <td>
                                        <a href="../../ApplicationDocs/MPCHRS.pdf" target="_blank">View Details</a>
                                    </td>
                                    <td>
                                        <a href="../../ApplicationDocs/Application Form- MPCHRS 2017-2018.pdf" target="_blank">Download Application Form</a>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <p><strong>Masters in Disaster and Human Security Management (MDHSM)</strong></p>
                                        <p>
                                            <u>Students admission fee</u> : 1000 tk <br />
                                            <u>Bank Acc Name</u> : DDHSM, BUP <br />
                                            <u>Bank Acc Number</u> : 0028-0210007542 <br />
                                            <i>After depositing 1000 tk to bank, student will fill up the application form and will 
                                            submit attached copies of all certificate, transcript, testimonial with the application 
                                            form to the admission office.</i>
                                        </p>
                                    </td>
                                    <td>
                                        <a href="../../ApplicationDocs/MDHSM_circular.pdf" target="_blank">View Details</a><br /><br />
                                        <a href="../../ApplicationDocs/MDHSM_Jan2018_AdmissionNotice.pdf" target="_blank">View Admission Notice</a>
                                    </td>
                                    <td>
                                        <a href="../../ApplicationDocs/MDHSM_Application_Form.pdf" target="_blank">Download Application Form</a>
                                    </td>
                                    <td>
                                        <a href="../../ApplicationDocs/MDHSM_Admit card_2.pdf" target="_blank">Admit Card</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    --%>

    <!-- Info Footer -->
    <div class="row">
        <div class="col-md-12">
                <div class="info-footer">
                    Please visit
                    <asp:HyperLink NavigateUrl="http://www.bup.edu.bd" Target="_blank" runat="server">BUP Official Website</asp:HyperLink>
                    for more information about Undergraduate and Graduate programs.
                </div>
        </div>
    </div>
</asp:Content>