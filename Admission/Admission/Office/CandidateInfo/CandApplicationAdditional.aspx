<%@ Page Title="Admin - Application Additional" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CandApplicationAdditional.aspx.cs" Inherits="Admission.Admission.Office.CandidateInfo.CandApplicationAdditional" %>

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
            --bg-gradient: linear-gradient(145deg, var(--primary), var(--secondary));
            --transition: all 0.3s cubic-bezier(0.25, 0.8, 0.25, 1);
            --shadow-sm: 0 2px 4px rgba(0,0,0,0.12), 0 2px 3px rgba(0,0,0,0.24);
            --shadow-md: 0 6px 12px rgba(0,0,0,0.1);
            --shadow-lg: 0 12px 30px rgba(0,0,0,0.12);
            --radius-sm: 6px;
            --radius-md: 10px;
            --radius-lg: 18px;
        }

        /* Admin Container */
        .admin-container {
            background: rgba(255, 255, 255, 0.95);
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-lg);
            padding: 2rem;
            margin-bottom: 2.5rem;
            backdrop-filter: blur(10px);
            -webkit-backdrop-filter: blur(10px);
        }

        /* Progress Navigation */
        .progress-nav {
            background: rgba(255, 255, 255, 0.95);
            border-radius: var(--radius-lg);
            padding: 3.5rem;
            margin-bottom: 1.5rem;
            box-shadow: var(--shadow-lg);
            backdrop-filter: blur(10px);
            -webkit-backdrop-filter: blur(10px);
            border: 1px solid rgba(255, 255, 255, 0.2);
            position: relative;
            overflow: hidden;
        }

            .progress-nav::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                right: 0;
                bottom: 0;
                background: linear-gradient(135deg, var(--primary) 0%, var(--secondary) 50%, var(--accent) 100%);
                opacity: 0.05;
                z-index: 0;
            }

        .breadcrumb-modern {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin: 0;
            padding: 0;
            list-style: none;
            position: relative;
            z-index: 1;
            flex-wrap: wrap;
            gap: 1rem;
        }

            .breadcrumb-modern::before {
                content: '';
                position: absolute;
                top: 50%;
                left: 0;
                right: 0;
                height: 4px;
                background: linear-gradient(to right, #e2e8f0, var(--accent-light), #e2e8f0);
                border-radius: 2px;
                z-index: -1;
            }

            .breadcrumb-modern li {
                position: relative;
                display: flex;
                align-items: center;
                justify-content: center;
                min-width: 65px;
                min-height: 65px;
                transition: var(--transition);
                z-index: 2;
            }

                .breadcrumb-modern li a {
                    position: relative;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    width: 65px;
                    height: 65px;
                    background: white;
                    border-radius: 50%;
                    border: 4px solid #e2e8f0;
                    box-shadow: 0 6px 16px rgba(0, 0, 0, 0.12);
                    text-decoration: none;
                    transition: var(--transition);
                    cursor: pointer;
                    color: var(--gray);
                    font-weight: 700;
                    font-size: 1.5rem;
                }

                    .breadcrumb-modern li a::before {
                        content: attr(data-step);
                        position: absolute;
                        top: 50%;
                        left: 50%;
                        transform: translate(-50%, -50%);
                        width: 32px;
                        height: 32px;
                        border-radius: 50%;
                        background: #e2e8f0;
                        color: var(--gray);
                        font-weight: 700;
                        font-size: 1.5rem;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        transition: var(--transition);
                    }

                    .breadcrumb-modern li a::after {
                        content: attr(data-label);
                        position: absolute;
                        top: 100%;
                        left: 50%;
                        transform: translateX(-50%);
                        color: var(--dark);
                        font-weight: 600;
                        font-size: 1.2rem;
                        white-space: nowrap;
                        margin-top: 0.75rem;
                        transition: var(--transition);
                        text-align: center;
                        max-width: 90px;
                        overflow: hidden;
                        text-overflow: ellipsis;
                    }

                .breadcrumb-modern li.active a {
                    border-color: var(--accent);
                    transform: scale(1.15);
                    box-shadow: 0 8px 24px rgba(59, 130, 246, 0.35);
                }

                    .breadcrumb-modern li.active a::before {
                        background: var(--accent);
                        color: white;
                    }

                    .breadcrumb-modern li.active a::after {
                        color: var(--accent);
                        font-weight: 700;
                    }

                .breadcrumb-modern li:not(.active):hover a {
                    transform: scale(1.08);
                    border-color: var(--accent-light);
                }

                    .breadcrumb-modern li:not(.active):hover a::before {
                        background: var(--accent-light);
                        color: white;
                    }

        /* Additional Sections */
        .additional-section {
            background: white;
            border-radius: var(--radius-lg);
            padding: 2.5rem;
            margin-bottom: 2rem;
            box-shadow: var(--shadow-md);
            border: 1px solid rgba(59, 130, 246, 0.1);
            position: relative;
        }

            .additional-section::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                height: 5px;
                background: linear-gradient(90deg, var(--accent), var(--secondary));
                border-radius: var(--radius-lg) var(--radius-lg) 0 0;
            }

        .section-title {
            background: linear-gradient(145deg, var(--accent), var(--secondary));
            color: white;
            padding: 1rem 1.5rem;
            border-radius: var(--radius-md);
            font-weight: 700;
            font-size: 2rem;
            margin-bottom: 1.5rem;
            text-align: center;
        }

        /* Form Table */
        .form-table-modern {
            width: 100%;
            border-collapse: separate;
            border-spacing: 0;
        }

            .form-table-modern tr {
                transition: var(--transition);
            }

                .form-table-modern tr:hover {
                    background: rgba(59, 130, 246, 0.03);
                }

            .form-table-modern td {
                padding: 1rem;
                border-bottom: 1px solid rgba(226, 232, 240, 0.5);
                vertical-align: middle;
                font-size: 1.05rem;
            }

                .form-table-modern td:first-child {
                    font-weight: 600;
                    color: var(--dark);
                    width: 30%;
                    background: rgba(248, 250, 252, 0.5);
                    font-size: 1.4rem;
                }

        /* Grid Layout for Multi-column Forms */
        .form-grid {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 1rem;
            margin-bottom: 1rem;
        }

        .form-group {
            display: flex;
            flex-direction: column;
        }

            .form-group label {
                font-weight: 600;
                color: var(--dark);
                margin-bottom: 0.5rem;
                font-size: 1.4rem;
            }

        /* Form Controls */
        .form-control-modern {
            width: 100%;
            padding: 0.75rem 1rem;
            border: 2px solid #e2e8f0;
            border-radius: var(--radius-md);
            font-size: 1.4rem;
            transition: var(--transition);
            background: white;
            font-family: inherit;
        }

            .form-control-modern:focus {
                outline: none;
                border-color: var(--accent);
                box-shadow: 0 0 0 4px rgba(59, 130, 246, 0.12);
                transform: translateY(-1px);
            }

            .form-control-modern:disabled {
                background: #f8fafc;
                color: var(--gray);
            }

        /* Textarea */
        textarea.form-control-modern {
            min-height: 80px;
            resize: vertical;
        }

        /* Helper Text */
        .helper-text {
            font-size: 0.9rem;
            color: #0ea5e9;
            margin-top: 0.25rem;
            font-style: italic;
        }

        /* Alert Modern */
        .alert-modern {
            background: linear-gradient(145deg, #dbeafe, #bfdbfe);
            color: var(--primary);
            padding: 1.5rem;
            border-radius: var(--radius-md);
            border-left: 5px solid var(--accent);
            margin-bottom: 2rem;
            border: none;
            font-size: 1.5rem;
            line-height: 1.6;
        }

        /* Buttons Modern */
        .btn-modern {
            padding: 0.875rem 1.75rem;
            border: none;
            border-radius: var(--radius-md);
            font-weight: 700;
            font-size: 1rem;
            cursor: pointer;
            transition: var(--transition);
            position: relative;
            overflow: hidden;
            display: inline-flex;
            align-items: center;
            gap: 0.75rem;
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
            background: linear-gradient(145deg, var(--accent), var(--secondary));
            color: white;
            box-shadow: var(--shadow-sm);
            border-radius: 8px;
        }

            .btn-primary-modern:hover {
                transform: translateY(-2px);
                box-shadow: var(--shadow-md);
            }

        /* Validation */
        .spanAsterisk {
            color: var(--danger);
            font-weight: bold;
            margin-left: 4px;
        }

        .validationErrorMsg {
            color: var(--danger);
            font-size: 0.95rem;
            display: block;
            margin-top: 0.5rem;
            font-weight: 600;
        }

        /* Animation */
        @keyframes fadeInUp {
            from {
                opacity: 0;
                transform: translateY(30px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        .additional-section {
            animation: fadeInUp 0.6s ease forwards;
        }

        /* Responsive Design */
        @media (max-width: 1200px) {
            .breadcrumb-modern li {
                min-width: 60px;
                min-height: 60px;
            }

                .breadcrumb-modern li a {
                    width: 60px;
                    height: 60px;
                }
        }

        @media (max-width: 992px) {
            .form-grid {
                grid-template-columns: repeat(2, 1fr);
            }

            .form-table-modern td:first-child {
                width: 35%;
            }
        }

        @media (max-width: 768px) {
            .admin-container {
                padding: 1.5rem;
            }

            .progress-nav {
                padding: 2.5rem;
                overflow-x: auto;
            }

            .breadcrumb-modern {
                flex-wrap: nowrap;
                gap: 0.75rem;
                min-width: max-content;
            }

                .breadcrumb-modern li {
                    min-width: 55px;
                    min-height: 55px;
                }

                    .breadcrumb-modern li a {
                        width: 55px;
                        height: 55px;
                    }

                        .breadcrumb-modern li a::before {
                            width: 28px;
                            height: 28px;
                            font-size: 1.2rem;
                        }

                        .breadcrumb-modern li a::after {
                            font-size: 1rem;
                            max-width: 70px;
                        }

            .form-grid {
                grid-template-columns: 1fr;
            }

            .form-table-modern td {
                display: block;
                width: 100% !important;
                padding: 0.75rem;
            }

                .form-table-modern td:first-child {
                    width: 100% !important;
                    font-weight: 700;
                    border-bottom: none;
                    padding-bottom: 0.5rem;
                    background: transparent;
                }

            .additional-section {
                padding: 1.5rem;
            }

            .btn-modern {
                padding: 0.75rem 1.5rem;
                font-size: 0.95rem;
            }
        }

        @media (max-width: 576px) {
            .admin-container {
                padding: 1rem;
            }

            .additional-section {
                padding: 1rem;
            }

            .breadcrumb-modern li {
                min-width: 50px;
                min-height: 50px;
            }

                .breadcrumb-modern li a {
                    width: 50px;
                    height: 50px;
                    border-width: 3px;
                }

                    .breadcrumb-modern li a::before {
                        width: 24px;
                        height: 24px;
                        font-size: 1rem;
                    }

                    .breadcrumb-modern li a::after {
                        font-size: 0.9rem;
                        max-width: 60px;
                    }

            .btn-modern {
                width: 100%;
                justify-content: center;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="updatePanelAdditional" runat="server">
        <ContentTemplate>

            <div class="admin-container">

                <!-- Progress Navigation -->
                <div class="progress-nav">
                    <nav>
                        <ol class="breadcrumb-modern">
                            <li>
                                <asp:HyperLink ID="hrefAppBasic" runat="server" data-step="1" data-label="Basic Info" title="Basic Info"></asp:HyperLink>
                            </li>
                            <li>
                                <asp:HyperLink ID="hrefAppPriority" runat="server" data-step="2" data-label="Program Priority" title="Program Priority"></asp:HyperLink>
                            </li>
                            <li>
                                <asp:HyperLink ID="hrefAppEducation" runat="server" data-step="3" data-label="Education" title="Education"></asp:HyperLink>
                            </li>
                            <li>
                                <asp:HyperLink ID="hrefAppRelation" runat="server" data-step="4" data-label="Parent/Guardian" title="Parent/Guardian"></asp:HyperLink>
                            </li>
                            <li>
                                <asp:HyperLink ID="hrefAppAddress" runat="server" data-step="5" data-label="Address" title="Address"></asp:HyperLink>
                            </li>
                            <li class="active">
                                <asp:HyperLink ID="hrefAppAdditional" runat="server" data-step="6" data-label="Additional/Work" title="Additional/Work"></asp:HyperLink>
                            </li>
                            <li>
                                <asp:HyperLink ID="hrefAppAttachment" runat="server" data-step="7" data-label="Upload Photo" title="Upload Photo"></asp:HyperLink>
                            </li>
                            <li>
                                <asp:HyperLink ID="hrefAppDeclaration" runat="server" data-step="8" data-label="Declaration" title="Declaration"></asp:HyperLink>
                            </li>
                        </ol>
                    </nav>
                </div>

                <div class="alert-modern">
                    <span class="spanAsterisk">*</span> indicate required fields.<br />
                    <span class="spanAsterisk">Please note that there is no validation for Admin in this form.</span>
                </div>

                <!-- General Information Section -->
                <div class="additional-section">
                    <table class="form-table-modern">
                        <tr>
                            <td>Have you ever been admitted to 
                                <asp:Label ID="lblUniShortName" runat="server"></asp:Label>? 
                                <span class="spanAsterisk">*</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAdmittedBefore" runat="server" CssClass="form-control-modern"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlAdmittedBefore_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>If yes, state Student ID No. <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:TextBox ID="txtCurrentStudentId" runat="server" CssClass="form-control-modern" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Candidate Annual Income</td>
                            <td>
                                <asp:TextBox ID="txtCandidateAnnualIncome" runat="server" CssClass="form-control-modern"
                                    TextMode="Number"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Father's Annual Income <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:TextBox ID="txtFatherAnnualIncome" runat="server" CssClass="form-control-modern"
                                    TextMode="Number"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Mother's Annual Income</td>
                            <td>
                                <asp:TextBox ID="txtMotherAnnualIncome" runat="server" CssClass="form-control-modern"
                                    TextMode="Number"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>

                <!-- Occupation Details Section -->
                <asp:Panel ID="panel_Occupation" runat="server" Style="display: none">
                    <div class="additional-section">
                        <div class="section-title">
                            Occupation details (if any)
                        </div>

                        <div class="form-grid">
                            <div class="form-group">
                                <label>Designation</label>
                                <asp:TextBox ID="txtWorkDesignation" runat="server" CssClass="form-control-modern"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label>Organization</label>
                                <asp:TextBox ID="txtWorkOrganization" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                <span class="helper-text">Please provide the exact name of the organization.</span>
                            </div>
                            <div class="form-group">
                                <label>Office Address</label>
                                <asp:TextBox ID="txtWorkAddress" runat="server" CssClass="form-control-modern" TextMode="MultiLine" Rows="2"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-grid" style="grid-template-columns: 1fr 1fr;">
                            <div class="form-group">
                                <label>Start Date</label>
                                <asp:TextBox ID="txtStartDateWE" runat="server" CssClass="form-control-modern" placeholder="dd/MM/yyyy"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="txtStartDateWECE" runat="server" Format="dd/MM/yyyy" TargetControlID="txtStartDateWE" />
                            </div>
                            <div class="form-group">
                                <label>End Date</label>
                                <asp:TextBox ID="txtEndDateWE" runat="server" CssClass="form-control-modern" placeholder="dd/MM/yyyy"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="txtEndDateWECE" runat="server" Format="dd/MM/yyyy" TargetControlID="txtEndDateWE" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Extracurricular Activity Section -->
                <div class="additional-section">
                    <div class="section-title">
                        Extracurricular Activity (if any)
                    </div>

                    <div class="form-grid">
                        <div class="form-group">
                            <label>Activity</label>
                            <asp:TextBox ID="txtActivity1" runat="server" CssClass="form-control-modern"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Award</label>
                            <asp:TextBox ID="txtAward1" runat="server" CssClass="form-control-modern"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Date</label>
                            <asp:TextBox ID="txtEcaDate1" runat="server" CssClass="form-control-modern" placeholder="dd/MM/yyyy"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtEcaDate1_CE" runat="server" Format="dd/MM/yyyy" TargetControlID="txtEcaDate1" />
                        </div>
                    </div>

                    <div class="form-grid">
                        <div class="form-group">
                            <label>Activity</label>
                            <asp:TextBox ID="txtActivity2" runat="server" CssClass="form-control-modern"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Award</label>
                            <asp:TextBox ID="txtAward2" runat="server" CssClass="form-control-modern"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Date</label>
                            <asp:TextBox ID="txtEcaDate2" runat="server" CssClass="form-control-modern" placeholder="dd/MM/yyyy"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtEcaDate2_CE" runat="server" Format="dd/MM/yyyy" TargetControlID="txtEcaDate2" />
                        </div>
                    </div>
                </div>

                <asp:Panel ID="messagePanel_Additional" runat="server">
                    <asp:Label ID="lblMessageAdditional" runat="server" Text=""></asp:Label>
                </asp:Panel>

                <div style="text-align: center; padding: 2rem; background: white; border-radius: var(--radius-lg); box-shadow: var(--shadow-md); margin-top: 2rem;">
                    <asp:Button ID="btnSave_Additional" runat="server" Text="Save"
                        CssClass="btn btn-primary-modern" OnClick="btnSave_Additional_Click" />

                    <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                        CssClass="btn btn-primary-modern" />

                    <span id="validationMsg" class="validationErrorMsg"></span>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>