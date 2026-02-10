<%@ Page Title="Candidate Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CandidateHome.aspx.cs" Inherits="Admission.Admission.Candidate.CandidateHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style>
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
            --transition: all 0.3s cubic-bezier(0.25, 0.8, 0.25, 1);
            --shadow-sm: 0 1px 3px rgba(0,0,0,0.12);
            --shadow-md: 0 4px 15px rgba(0,0,0,0.08);
            --shadow-lg: 0 10px 25px rgba(0,0,0,0.1);
            --radius-md: 8px;
            --radius-lg: 12px;
        }

        /* Modern Card Design */
        .modern-card {
            background: white;
            border-radius: var(--radius-md);
            box-shadow: var(--shadow-sm);
            border: 1px solid #e5e7eb;
            transition: var(--transition);
            overflow: hidden;
            margin-bottom: 1.5rem;
        }

        .modern-card:hover {
            box-shadow: var(--shadow-md);
        }

        .card-header {
            background: var(--secondary); /* BHPI Green Header */
            color: white;
            padding: 1rem 1.5rem;
            border: none;
            font-weight: 600;
            font-size: 1rem;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            display: flex;
            align-items: center;
        }

        .card-header::after {
            content: '';
            position: absolute;
            bottom: 0;
            left: 0;
            right: 0;
            height: 3px;
            background: var(--primary); /* Red underline accent */
        }

        .card-body {
            padding: 1.5rem;
        }

        /* Action Buttons */
        .action-buttons {
            display: flex;
            flex-direction: column;
            gap: 0.75rem;
        }

        .action-btn {
            background: var(--primary); /* BHPI Red */
            border: none;
            border-radius: 4px;
            color: white !important;
            padding: 0.75rem 1rem;
            font-weight: 600;
            text-decoration: none;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 0.5rem;
            transition: var(--transition);
            text-transform: uppercase;
            font-size: 0.85rem;
        }

        .action-btn:hover {
            background: #6d0f14;
            transform: translateX(5px);
            box-shadow: var(--shadow-md);
        }

        .action-btn.info {
            background: var(--secondary); /* BHPI Green */
        }
        .action-btn.info:hover {
            background: #00563f;
        }

        .action-btn.warning {
            background: #444; /* Neutral Dark for Admit Card */
        }

        /* Info Table */
        .info-table {
            width: 100%;
            border-collapse: collapse;
        }

        .info-table td {
            padding: 0.75rem 0;
            border-bottom: 1px solid #f0f0f0;
        }

        .info-table tr:last-child td { border-bottom: none; }

        .info-table .label {
            font-weight: 700;
            color: #555;
            width: 40%;
            font-size: 0.9rem;
        }

        .info-table .value {
            color: var(--dark);
            font-weight: 500;
        }

        /* Photo Section */
        .photo-container {
            text-align: center;
        }

        .photo-frame {
            display: inline-block;
            border: 4px solid #f0f0f0;
            border-radius: 8px;
            overflow: hidden;
            background: #f9f9f9;
        }

        .photo-frame img {
            width: 130px;
            height: 150px;
            object-fit: cover;
            display: block;
        }

        .photo-placeholder {
            width: 130px;
            height: 150px;
            display: flex;
            align-items: center;
            justify-content: center;
            background: #f3f4f6;
            color: #d1d5db;
            font-size: 3rem;
        }

        .photo-warning {
            margin-top: 1rem;
            font-size: 0.8rem;
            color: var(--primary);
            font-weight: 600;
            line-height: 1.4;
        }

        /* Status Indicators */
        .status-item {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 0.85rem;
            background: #f9fafb;
            border-radius: 6px;
            border-left: 4px solid var(--secondary);
            margin-bottom: 0.5rem;
        }

        .status-badge {
            padding: 0.2rem 0.6rem;
            border-radius: 4px;
            font-size: 0.75rem;
            font-weight: 700;
            text-transform: uppercase;
        }

        .status-success { background: #dcfce7; color: #166534; }
        .status-warning { background: #fef9c3; color: #854d0e; }

        /* Program Table */
        .program-table {
            width: 100%;
            border: 1px solid #eee;
        }

        .program-table th {
            background: #f8f9fa;
            padding: 0.75rem;
            font-weight: 700;
            color: var(--secondary);
            font-size: 0.85rem;
            text-transform: uppercase;
            border-bottom: 2px solid var(--secondary);
        }

        .program-table td {
            padding: 0.75rem;
            border-bottom: 1px solid #eee;
            font-size: 0.9rem;
        }

        .choice-badge {
            background: var(--primary);
            color: white;
            padding: 0.2rem 0.5rem;
            border-radius: 3px;
            font-weight: bold;
        }

        /* Notice Alert */
        .notice-alert {
            background: #fff5f5;
            color: var(--primary);
            border: 1px solid #feb2b2;
            border-radius: 8px;
            padding: 1rem;
            margin-bottom: 1.5rem;
            font-weight: 600;
        }

        /* Animations */
        .fade-in { animation: fadeInUp 0.5s ease forwards; opacity: 0; }
        @keyframes fadeInUp {
            from { opacity: 0; transform: translateY(10px); }
            to { opacity: 1; transform: translateY(0); }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="notice-alert" style="display: none;">
                <i class="fas fa-bullhorn me-2"></i>
                <strong>Urgent:</strong> If you don't see your admit card link, please check again in one hour.
            </div>

            <div class="row g-4">
                <div class="col-lg-3 col-md-12">
                    <div class="modern-card fade-in">
                        <div class="card-header">
                            <i class="fas fa-directions me-2"></i>Navigation
                        </div>
                        <div class="card-body">
                            <div class="action-buttons">
                                <asp:Button ID="btnApplicationForm" runat="server"
                                    Text="Application Form"
                                    CssClass="action-btn"
                                    OnClick="btnApplicationForm_Click"
                                    Visible="false" />

                                <asp:HyperLink ID="hrefMultiApp" runat="server"
                                    Text="<i class='fas fa-plus-circle'></i> New Application"
                                    CssClass="action-btn info"
                                    NavigateUrl="~/Admission/Candidate/MultipleApplication.aspx"
                                    Style="display: none"></asp:HyperLink>

                                <asp:HyperLink ID="hrefPrintAdmitCard" runat="server"
                                    Text="<i class='fas fa-download'></i> Download Admit Card"
                                    CssClass="action-btn warning"
                                    Visible="false"></asp:HyperLink>

                                <asp:HyperLink ID="hrefReviewApplication" runat="server"
                                    Text="<i class='fas fa-file-invoice'></i> Review Details"
                                    CssClass="action-btn"
                                    NavigateUrl="~/Admission/Candidate/ReviewApplication.aspx"
                                    Visible="false"></asp:HyperLink>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-6 col-md-12">
                    <div class="modern-card fade-in" style="animation-delay: 0.1s;">
                        <div class="card-header">
                            <i class="fas fa-id-card me-2"></i>Candidate Profile
                        </div>
                        <div class="card-body">
                            <table class="info-table">
                                <tr>
                                    <td class="label">Payment ID</td>
                                    <td class="value">
                                        <asp:Label ID="lblPaymentId" runat="server" CssClass="text-primary fw-bold"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">Applicant Name</td>
                                    <td class="value">
                                        <asp:Label ID="lblCandidateName" runat="server" CssClass="text-uppercase"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">Date of Birth</td>
                                    <td class="value">
                                        <asp:Label ID="lblDateOfBirth" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="label">Payment Status</td>
                                    <td class="value">
                                        <asp:Label ID="lblPaymentStatus" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <%--<tr runat="server" id="quotarow">
                                    <td class="label">Quota Status</td>
                                    <td class="value">
                                        <asp:Label ID="lblQuotaStatus" runat="server" CssClass="fw-bold"></asp:Label>
                                    </td>
                                </tr>--%>
                            </table>
                        </div>
                    </div>

                    <div class="modern-card fade-in" id="panelFacultyWiseProgramPriority" runat="server" visible="false" style="animation-delay: 0.2s;">
                        <div class="card-header">
                            <i class="fas fa-list-ol me-2"></i>Course Choices
                        </div>
                        <div class="card-body">
                            <asp:Label ID="lblPrograms" runat="server" CssClass="small text-muted mb-3 d-block"></asp:Label>

                            <asp:ListView ID="lvProgramPriority" runat="server"
                                OnItemDataBound="lvProgramPriority_ItemDataBound"
                                GroupItemCount="1"
                                GroupPlaceholderID="groupPlaceHolder"
                                ItemPlaceholderID="itemPlaceHolder">
                                <LayoutTemplate>
                                    <table class="program-table">
                                        <thead>
                                            <tr>
                                                <th>Faculty/Unit</th>
                                                <th>Applied Course</th>
                                                <th style="width: 80px; text-align: center;">Priority</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:PlaceHolder runat="server" ID="groupPlaceHolder"></asp:PlaceHolder>
                                        </tbody>
                                    </table>
                                </LayoutTemplate>
                                <GroupTemplate>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                </GroupTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td class="fw-bold text-secondary">
                                            <asp:Label ID="lblUnitName" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblProgramName" runat="server" />
                                        </td>
                                        <td style="text-align: center;">
                                            <span class="choice-badge">
                                                <asp:Label ID="lblChoice" runat="server" />
                                            </span>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                </div>

                <div class="col-lg-3 col-md-12">
                    <div class="modern-card fade-in" style="animation-delay: 0.3s;">
                        <div class="card-header">
                            <i class="fas fa-camera-retro me-2"></i>Official Photo
                        </div>
                        <div class="card-body">
                            <div class="photo-container">
                                <div class="photo-frame">
                                    <img runat="server" id="imgCtrl" src="" alt="Candidate Photo" />
                                    <div class="photo-placeholder" style="display: none;">
                                        <i class="fas fa-user-circle"></i>
                                    </div>
                                </div>
                                <div class="photo-warning">
                                    <asp:Label ID="lblPhoto" runat="server"
                                        Text="<i class='fas fa-info-circle'></i> Please upload a formal photo via the Application Form to enable your Admit Card."
                                        Visible="true"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="modern-card fade-in" style="animation-delay: 0.4s;">
                        <div class="card-header">
                            <i class="fas fa-tasks me-2"></i>Tracking
                        </div>
                        <div class="card-body">
                            <div class="status-grid">
                                <div class="status-item">
                                    <span class="fw-bold small text-muted">Submission</span>
                                    <span class="status-badge status-success">
                                        <asp:Label ID="lblApplicationFormStatus" runat="server"></asp:Label>
                                    </span>
                                </div>
                                <div class="status-item" style="display: none;">
                                    <span class="fw-bold small text-muted">Approval</span>
                                    <span class="status-badge status-warning">
                                        <asp:Label ID="lblApprovalStatus" runat="server"></asp:Label>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script>
        document.addEventListener('DOMContentLoaded', function () {
            // Handle photo display logic
            const photoImg = document.querySelector('#<%= imgCtrl.ClientID %>');
            const photoPlaceholder = document.querySelector('.photo-placeholder');

            if (photoImg && photoImg.getAttribute('src') !== '') {
                photoPlaceholder.style.display = 'none';
                photoImg.style.display = 'block';
            } else {
                if (photoImg) photoImg.style.display = 'none';
                photoPlaceholder.style.display = 'flex';
            }
        });
    </script>
</asp:Content>