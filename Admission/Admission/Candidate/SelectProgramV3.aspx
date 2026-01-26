<%@ Page Title="Select Program" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelectProgramV3.aspx.cs" Inherits="Admission.Admission.Candidate.SelectProgramV3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        :root {
            /* BHPI Institutional Palette */
            --primary: #8a151b;        /* BHPI Red */
            --secondary: #006a4e;      /* BHPI Green */
            --accent: #00897b;         
            --accent-light: #e0f2f1;
            --success: #10b981;
            --warning: #f59e0b;
            --danger: #dc2626;
            --light: #f8faf9;
            --dark: #1a1a1a;
            --gray: #6B7280;
            --gray-light: #F3F4F6;
            --gray-medium: #E5E7EB;
            --white: #FFFFFF;
            --transition: all 0.3s ease;
            --shadow-sm: 0 2px 4px rgba(0,0,0,0.05);
            --shadow-md: 0 4px 15px rgba(0,0,0,0.1);
            --shadow-lg: 0 10px 25px rgba(0,0,0,0.15);
            --radius-md: 4px;
            --radius-lg: 8px;
        }

        /* Loading Overlay */
        .loading-overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(255, 255, 255, 0.9);
            backdrop-filter: blur(5px);
            z-index: 9999;
            display: flex;
            align-items: center;
            justify-content: center;
            flex-direction: column;
        }

        .loading-spinner {
            width: 50px;
            height: 50px;
            border: 4px solid var(--gray-medium);
            border-top: 4px solid var(--primary);
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }

        @keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }

        /* Page Header */
        .page-header {
            background: #ffffff;
            color: var(--primary);
            padding: 2.5rem 0;
            border-bottom: 4px solid var(--secondary);
            margin-bottom: 2rem;
            text-align: center;
        }

        .page-header h1 {
            margin: 0;
            font-size: 2.2rem;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 1px;
        }

        /* Modern Cards */
        .modern-card {
            background: var(--white);
            border-radius: var(--radius-md);
            box-shadow: var(--shadow-sm);
            border: 1px solid #e0e0e0;
            overflow: hidden;
            transition: var(--transition);
            margin-bottom: 1.5rem;
        }

        .card-header {
            background: var(--secondary);
            color: var(--white);
            padding: 1.2rem 1.5rem;
            border: none;
            font-weight: 600;
            font-size: 1.1rem;
            text-transform: uppercase;
        }

        .card-body { padding: 1.5rem; }

        /* Program List Table */
        .program-table thead th {
            background: #f8f9fa;
            color: var(--secondary);
            font-weight: 700;
            border-bottom: 2px solid var(--gray-medium);
            padding: 1.2rem 1rem;
            font-size: 0.85rem;
            text-transform: uppercase;
        }

        .program-table tbody td {
            padding: 1.2rem 1rem;
            border-bottom: 1px solid var(--gray-medium);
            vertical-align: middle;
        }

        /* Custom Checkbox */
        .custom-checkbox {
            position: relative;
            display: inline-block;
            width: 24px;
            height: 24px;
        }

        .custom-checkbox input[type="checkbox"] {
            opacity: 0;
            position: absolute;
            width: 100%; height: 100%;
            cursor: pointer;
            z-index: 5;
        }

        .checkbox-label {
            position: absolute;
            top: 0; left: 0;
            width: 100%; height: 100%;
            background: #fff;
            border: 2px solid var(--gray-medium);
            border-radius: 4px;
            transition: var(--transition);
        }

        .custom-checkbox input[type="checkbox"]:checked + .checkbox-label {
            background: var(--primary);
            border-color: var(--primary);
        }

        .checkbox-label::after {
            content: '\f00c';
            font-family: 'Font Awesome 5 Free';
            font-weight: 900;
            color: #fff;
            font-size: 12px;
            position: absolute;
            top: 50%; left: 50%;
            transform: translate(-50%, -50%);
            opacity: 0;
        }

        .custom-checkbox input[type="checkbox"]:checked + .checkbox-label::after { opacity: 1; }

        /* Fee Display */
        .fee-amount {
            font-weight: 700;
            color: var(--secondary);
            font-size: 1.1rem;
        }

        /* Summary Panel */
        .summary-panel {
            background: #fdfdfd;
            border: 2px solid var(--secondary);
            border-radius: var(--radius-md);
            padding: 1.5rem;
            margin-top: 1.5rem;
            box-shadow: var(--shadow-md);
        }

        .summary-label {
            font-size: 0.8rem;
            color: var(--gray);
            font-weight: 700;
            text-transform: uppercase;
        }

        .summary-value { font-size: 1.3rem; font-weight: 700; color: var(--dark); }
        .total-amount { color: var(--primary); font-size: 1.6rem; }

        /* Apply Button */
        .btn-apply {
            background: var(--primary);
            color: var(--white) !important;
            border: none;
            padding: 1rem 2.5rem;
            font-weight: 700;
            font-size: 1rem;
            border-radius: var(--radius-sm);
            transition: var(--transition);
            text-transform: uppercase;
            letter-spacing: 1px;
        }

        .btn-apply:hover:not(:disabled) {
            background: #6d0f14;
            transform: translateY(-2px);
            box-shadow: var(--shadow-md);
        }

        /* Info Section */
        .info-section {
            background: #fff;
            border-left: 5px solid var(--primary);
            padding: 1.5rem;
            margin-top: 2rem;
            box-shadow: var(--shadow-sm);
        }

        .info-section li { margin-bottom: 10px; color: #444; line-height: 1.6; }
        .info-section a { color: var(--secondary); font-weight: 700; }

        @media (max-width: 768px) {
            .page-header h1 { font-size: 1.6rem; }
            .summary-info { flex-direction: column; gap: 1rem; align-items: flex-start; }
        }
    </style>

    <script type="text/javascript">
        function InProgress() {
            var panelProg = $get('divProgress');
            if (panelProg) { panelProg.style.display = 'flex'; }
        }
        function onComplete() {
            var panelProg = $get('divProgress');
            if (panelProg) { panelProg.style.display = 'none'; }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divProgress" class="loading-overlay" style="display: none;">
        <div class="loading-spinner"></div>
        <div class="text-muted fw-bold">BHPI Admission: Updating calculations...</div>
    </div>

    <div class="page-header">
        <h1>
            <i class="fas fa-university me-3"></i>
            <asp:Label ID="lblEducationCat" runat="server"></asp:Label>
        </h1>
        <small class="text-muted">Academic Excellence in Health Sciences</small>
    </div>

    <asp:UpdatePanel runat="server" ID="upanel1">
        <ContentTemplate>
            <div class="modern-card">
                <div class="card-header">
                    <i class="fas fa-check-square me-2"></i>
                    Select Programs
                </div>
                <div class="card-body">
                    <div class="program-table">
                        <asp:ListView ID="lvAdmSetup" runat="server" OnItemDataBound="lvAdmSetup_ItemDataBound">
                            <LayoutTemplate>
                                <table class="table table-hover mb-0">
                                    <thead>
                                        <tr>
                                            <th style="width: 80px; text-align: center;">Select</th>
                                            <th>Program Name</th>
                                            <th>Application Start</th>
                                            <th>Deadline</th>
                                            <th>Fee (BDT)</th>
                                        </tr>
                                    </thead>
                                    <tbody id="itemPlaceholder" runat="server">
                                    </tbody>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align: center;">
                                        <div class="custom-checkbox">
                                            <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" 
                                                OnCheckedChanged="ckbxSelectedSchool_CheckedChanged" />
                                            <label class="checkbox-label" for="<%# Container.FindControl("CheckBox1").ClientID %>"></label>
                                        </div>
                                    </td>
                                    <td style="display: none;">
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                        <asp:HiddenField ID="HiddenField2" runat="server" Value='<%# Eval("AdmissionUnitID") %>' />
                                        <asp:HiddenField ID="HiddenField3" runat="server" Value='<%# Eval("AcaCalID") %>' />
                                        <asp:HiddenField ID="HiddenField4" runat="server" Value='<%# Eval("Fee") %>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblUnitName" runat="server" CssClass="fw-bold text-dark" />
                                    </td>
                                    <td><asp:Label ID="lblStartDate" runat="server" /></td>
                                    <td><asp:Label ID="lblEndDate" runat="server" /></td>
                                    <td>
                                        <span class="fee-amount">
                                            <asp:Label ID="lblFee" runat="server" />
                                        </span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <div class="text-center py-5">
                                    <i class="fas fa-folder-open fa-3x text-muted mb-3"></i>
                                    <p class="text-muted">No programs are currently open for selection.</p>
                                </div>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </div>

                    <div class="summary-panel">
                        <div class="row align-items-center">
                            <div class="col-md-8">
                                <div class="d-flex summary-info">
                                    <div class="me-5">
                                        <div class="summary-label">Programs Selected</div>
                                        <div class="summary-value">
                                            <asp:Label ID="lblNoOfSelSchool" runat="server" Text="0"></asp:Label>
                                        </div>
                                    </div>
                                    <div>
                                        <div class="summary-label">Total Fee</div>
                                        <div class="summary-value total-amount">
                                            BDT <asp:Label ID="lblTotal" runat="server" Text="0"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 text-md-end mt-3 mt-md-0">
                                <asp:Button ID="btnApply1" runat="server" Text="CONTINUE TO APPLY" 
                                    OnClick="btnApply_Click" CssClass="btn-apply" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modern-card" runat="server" visible="false">
                <div class="card-header">
                    <i class="fas fa-credit-card me-2"></i>
                    Continue with Payment ID
                </div>
                <div class="card-body">
                    <div class="payment-section">
                        <div class="form-group">
                            <label class="form-label">Payment ID<span class="required-asterisk">*</span></label>
                            <asp:TextBox ID="txtPaymentId" runat="server" placeholder="Enter payment ID" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" Display="Dynamic" ControlToValidate="txtPaymentId" ErrorMessage="Payment ID is required" CssClass="error-message" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Button ID="Button1" runat="server" Text="Continue Payment" OnClick="btnNext_Click" ValidationGroup="gr1" CssClass="btn-payment" />
                        <asp:Label ID="lblOLevelResult" runat="server" Text="" CssClass="error-message d-block mt-2"></asp:Label>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="info-section">
        <h5 class="text-danger fw-bold mb-3"><i class="fas fa-info-circle me-2"></i>Important Note</h5>
        <ul class="list-unstyled">
            <li>Candidates with mixed academic backgrounds (SSC/HSC and O/A Levels) should contact the **BHPI admission helpline** for guidance.</li>
            <li>Please visit the <asp:HyperLink NavigateUrl="https://www.bhpi.edu.bd" Target="_blank" runat="server">BHPI Official Website</asp:HyperLink> for eligibility details.</li>
        </ul>
    </div>

    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="upanel1" runat="server">
        <Animations>
            <OnUpdating><Parallel duration="0"><ScriptAction Script="InProgress();" /></Parallel></OnUpdating>
            <OnUpdated><Parallel duration="0"><ScriptAction Script="onComplete();" /></Parallel></OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>
</asp:Content>