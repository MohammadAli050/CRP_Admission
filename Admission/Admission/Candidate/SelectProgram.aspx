<%@ Page Title="Select School/Program" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SelectProgram.aspx.cs" Inherits="Admission.Admission.Candidate.SelectProgram" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        :root {
            /* BHPI Institutional Theme Colors */
            --primary: #8a151b;        /* BHPI Red */
            --secondary: #006a4e;      /* BHPI Green */
            --accent: #00897b;         
            --accent-light: #e0f2f1;
            --success: #10b981;
            --warning: #f59e0b;
            --danger: #dc2626;
            --light: #f4f7f6;
            --dark: #1a1a1a;
            --gray: #6B7280;
            --gray-light: #F3F4F6;
            --gray-medium: #E5E7EB;
            --white: #FFFFFF;
            --transition: all 0.3s ease;
            --shadow-sm: 0 2px 4px rgba(0,0,0,0.05);
            --shadow-md: 0 4px 15px rgba(0,0,0,0.1);
            --shadow-lg: 0 10px 25px rgba(0,0,0,0.15);
            --radius-sm: 4px;
            --radius-md: 8px;
            --radius-lg: 12px;
        }

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

        .modern-card:hover {
            box-shadow: var(--shadow-md);
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

        .card-header.payment-header {
            background: var(--primary);
        }

        .card-header.info-header {
            background: #444;
        }

        .card-body {
            padding: 1.5rem;
        }

        /* Program Table */
        .program-table table {
            margin: 0;
            border: none;
        }

        .program-table thead th {
            background: #f8f9fa;
            color: var(--secondary);
            font-weight: 700;
            border-bottom: 2px solid var(--gray-medium);
            padding: 1.2rem 1rem;
            font-size: 0.85rem;
            text-transform: uppercase;
        }

        .program-table tbody tr:hover {
            background: #fffcfc;
        }

        .program-table tbody td {
            padding: 1.2rem 1rem;
            border-bottom: 1px solid var(--gray-medium);
            vertical-align: middle;
        }

        /* Fee Display */
        .fee-amount {
            font-weight: 700;
            color: var(--secondary);
            font-size: 1.1rem;
        }

        /* Apply Button */
        .btn-apply {
            background: var(--primary);
            color: var(--white) !important;
            border: none;
            padding: 0.7rem 1.5rem;
            font-weight: 600;
            font-size: 0.85rem;
            border-radius: var(--radius-sm);
            transition: var(--transition);
            text-decoration: none;
            display: inline-block;
            text-align: center;
        }

        .btn-apply:hover {
            background: #6d0f14;
            transform: translateY(-2px);
            box-shadow: var(--shadow-md);
        }

        /* Payment Form */
        .payment-form {
            background: #f9f9f9;
            border-radius: var(--radius-sm);
            padding: 1.5rem;
            border: 1px solid #eee;
        }

        .form-label {
            font-weight: 700;
            color: #444;
            font-size: 0.9rem;
        }

        .required-asterisk {
            color: var(--danger);
        }

        .form-control {
            border: 1px solid #ccc;
            border-radius: var(--radius-sm);
            padding: 0.8rem;
        }

        .form-control:focus {
            border-color: var(--secondary);
            box-shadow: 0 0 0 3px rgba(0, 106, 78, 0.1);
        }

        .btn-payment {
            background: var(--secondary);
            color: var(--white);
            border: none;
            padding: 0.8rem;
            font-weight: 700;
            border-radius: var(--radius-sm);
            width: 100%;
            text-transform: uppercase;
        }

        .btn-payment:hover {
            background: #00563f;
            color: white;
        }

        /* Info Section */
        .info-section {
            background: #fff;
            border-left: 5px solid var(--primary);
            padding: 1.5rem;
            margin-top: 2rem;
            box-shadow: var(--shadow-sm);
        }

        .info-section a {
            color: var(--secondary);
            font-weight: 700;
        }

        @media (max-width: 768px) {
            .page-header h1 { font-size: 1.6rem; }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h1>
            <i class="fas fa-graduation-cap me-3"></i>
            Graduate Admission Programs
        </h1>
        <small class="text-muted">Bangladesh Health Professions Institute (BHPI)</small>
    </div>

    <div class="row">
        <div class="col-lg-8 col-md-7">
            <div class="modern-card">
                <div class="card-header">
                    <i class="fas fa-list-alt me-2"></i>
                    Available Courses
                </div>
                <div class="card-body">
                    <div class="program-table">
                        <asp:ListView ID="lvAdmSetup" runat="server"
                            OnItemDataBound="lvAdmSetup_ItemDataBound"
                            OnItemCommand="lvAdmSetup_ItemCommand">
                            <LayoutTemplate>
                                <table class="table table-hover mb-0">
                                    <thead>
                                        <tr>
                                            <th>Program Name</th>
                                            <th>Application Start</th>
                                            <th>Deadline</th>
                                            <th>Fee (BDT)</th>
                                            <th style="width: 120px;">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody id="itemPlaceholder" runat="server">
                                    </tbody>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <span class="program-name">
                                            <asp:Label ID="lblUnitName" runat="server" />
                                        </span>
                                    </td>
                                    <td><asp:Label ID="lblStartDate" runat="server" /></td>
                                    <td><asp:Label ID="lblEndDate" runat="server" /></td>
                                    <td>
                                        <span class="fee-amount">
                                            <asp:Label ID="lblFee" runat="server" />
                                        </span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkViewDetails" runat="server" CssClass="btn-apply">
                                            APPLY NOW
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <div class="text-center py-5">
                                    <i class="fas fa-folder-open fa-3x text-muted mb-3"></i>
                                    <p class="text-muted">No programs are currently open for admission.</p>
                                </div>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-lg-4 col-md-5">
            <div class="modern-card">
                <div class="card-header payment-header">
                    <i class="fas fa-receipt me-2"></i>
                    Complete Application
                </div>
                <div class="card-body">
                    <div class="payment-form">
                        <p class="small text-muted mb-3">If you have already started an application and have a Payment ID, enter it here to continue.</p>
                        <div class="form-group">
                            <label class="form-label">
                                Payment ID <span class="required-asterisk">*</span>
                            </label>
                            <asp:TextBox ID="txtPaymentId" runat="server" 
                                placeholder="e.g., BHPI-2025-XXXX"
                                CssClass="form-control" 
                                onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" 
                                Display="Dynamic" ControlToValidate="txtPaymentId" 
                                ErrorMessage="Please enter your Payment ID" 
                                CssClass="text-danger small fw-bold" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Button ID="Button1" runat="server" Text="CONTINUE PAYMENT" 
                            OnClick="btnNext_Click" ValidationGroup="gr1" 
                            CssClass="btn-payment" />
                        <asp:Label ID="lblOLevelResult" runat="server" Text="" 
                            CssClass="text-danger d-block mt-2 small"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="updatePanelAll" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panelOtherPrograms" runat="server" Visible="false">
                <div class="modern-card">
                    <div class="card-header info-header">
                        <i class="fas fa-plus-circle me-2"></i>
                        Additional Selection Options
                    </div>
                    <div class="card-body">
                        <asp:Panel ID="listViewPanel" runat="server">
                            <asp:ListView ID="lvProgramPriority" runat="server"
                                OnItemDataBound="lvProgramPriority_ItemDataBound"
                                OnItemCommand="lvProgramPriority_ItemCommand"
                                GroupItemCount="1"
                                GroupPlaceholderID="groupPlaceHolder"
                                ItemPlaceholderID="itemPlaceHolder">
                                <LayoutTemplate>
                                    <table class="table table-hover mb-0">
                                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder"></asp:PlaceHolder>
                                    </table>
                                </LayoutTemplate>
                                <GroupTemplate>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                </GroupTemplate>
                                <ItemTemplate>
                                    <%# AddGroupingHeader() %>
                                    <tr>
                                        <td style="display: none;">
                                            <asp:Label ID="lblSerial" runat="server" Visible="false" />                                            
                                        </td>
                                        <td>
                                            <span class="fw-bold text-dark">
                                                <asp:Label ID="lblFileTypeName" runat="server" />
                                            </span>
                                        </td>
                                        <td style="width: 150px; text-align: right;">
                                            <asp:HyperLink ID="hlBtn" runat="server" CssClass="btn-apply w-100">SELECT</asp:HyperLink>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </asp:Panel>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="info-section">
        <p>
            <i class="fas fa-info-circle me-2 text-danger"></i>
            <strong>Need Assistance?</strong> Please visit the 
            <asp:HyperLink NavigateUrl="https://www.bhpi.edu.bd/" Target="_blank" runat="server">
                BHPI Official Website
            </asp:HyperLink>
            or contact the admission office for comprehensive details about our medical and health profession programs.
        </p>
    </div>
</asp:Content>