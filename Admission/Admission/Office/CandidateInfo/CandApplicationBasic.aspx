<%@ Page Title="Admin - Application Basic" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CandApplicationBasic.aspx.cs" Inherits="Admission.Admission.Office.CandidateInfo.CandApplicationBasic" %>

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

        /* Form Sections */
        .basic-section {
            background: white;
            border-radius: var(--radius-lg);
            padding: 2.5rem;
            margin-bottom: 2.5rem;
            box-shadow: var(--shadow-md);
            border: 1px solid rgba(59, 130, 246, 0.1);
            position: relative;
            overflow: hidden;
        }

            .basic-section::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                height: 5px;
                background: linear-gradient(90deg, var(--accent), var(--secondary));
                border-radius: var(--radius-lg) var(--radius-lg) 0 0;
            }

            .basic-section h4 {
                color: var(--primary);
                margin-bottom: 2rem;
                font-weight: 700;
                font-size: 2.5rem;
                display: flex;
                align-items: center;
                gap: 1rem;
            }

                .basic-section h4::before {
                    width: 48px;
                    height: 48px;
                    background: linear-gradient(135deg, var(--accent), var(--secondary));
                    border-radius: 50%;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    flex-shrink: 0;
                    background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='white' viewBox='0 0 24 24'%3E%3Cpath d='M12 12c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm0 2c-2.67 0-8 1.34-8 4v2h16v-2c0-2.66-5.33-4-8-4z'/%3E%3C/svg%3E");
                    background-size: 24px;
                    background-repeat: no-repeat;
                    background-position: center;
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
                padding: 1.25rem;
                border-bottom: 1px solid rgba(226, 232, 240, 0.5);
                vertical-align: middle;
                font-size: 1.4rem;
            }

                .form-table-modern td:first-child {
                    font-weight: 600;
                    color: var(--dark);
                    width: 35%;
                    background: rgba(248, 250, 252, 0.5);
                    font-size: 1.4rem;
                }

        /* Form Controls */
        .form-control-modern {
            width: 100%;
            padding: 0.95rem 1rem;
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

        /* Quota Cards */
        .quota-info-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
            gap: 1.5rem;
            margin: 1.5rem 0;
        }

        .quota-card {
            background: white;
            border-radius: var(--radius-md);
            padding: 1.5rem;
            border: 2px solid #e2e8f0;
            transition: var(--transition);
            text-align: center;
        }

            .quota-card:hover {
                border-color: var(--accent);
                transform: translateY(-3px);
                box-shadow: var(--shadow-md);
            }

            .quota-card b {
                display: block;
                padding: 0.75rem;
                border-radius: var(--radius-sm);
                margin-bottom: 0.75rem;
                font-size: 1.5rem;
            }

            .quota-card ul {
                text-align: left;
                padding-left: 1.25rem;
                margin: 0;
                font-size: 1.2rem;
                line-height: 1.7;
            }

            .quota-card li {
                margin-bottom: 0.5rem;
            }

        /* Panel Modern */
        .panel-modern {
            background: white;
            border-radius: var(--radius-lg);
            padding: 0;
            margin-bottom: 2.5rem;
            box-shadow: var(--shadow-md);
            border: 1px solid rgba(59, 130, 246, 0.1);
            overflow: hidden;
        }

        .panel-header-modern {
            background: linear-gradient(145deg, var(--accent), var(--secondary));
            color: white;
            padding: 1.75rem 2.5rem;
            font-weight: 700;
            font-size: 1.35rem;
            text-align: center;
            border-bottom: none;
        }

        .panel-body-modern {
            padding: 2.5rem;
        }

            .panel-body-modern label {
                font-weight: 600;
                font-size: 1.05rem;
                color: var(--dark);
                margin-bottom: 0.5rem;
                display: block;
            }

        /* Alert Modern */
        .alert-modern {
            background: linear-gradient(145deg, #dbeafe, #bfdbfe);
            color: var(--primary);
            padding: 1.75rem;
            border-radius: var(--radius-md);
            border-left: 5px solid var(--accent);
            margin-bottom: 2rem;
            border: none;
            font-size: 1.5rem;
            line-height: 1.7;
        }

            .alert-modern strong {
                color: var(--danger);
                font-size: 1.1rem;
            }

            .alert-modern ul {
                margin: 0.75rem 0 0 0;
                padding-left: 1.5rem;
            }

            .alert-modern li {
                margin-bottom: 0.5rem;
            }

        /* Buttons Modern */
        .btn-modern {
            padding: 1rem 2rem;
            border: none;
            border-radius: var(--radius-md);
            font-weight: 700;
            font-size: 1.1rem;
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

        .btn-default-modern {
            background: linear-gradient(145deg, #f1f5f9, #e2e8f0);
            color: var(--dark);
        }

            .btn-default-modern:hover {
                transform: translateY(-2px);
                box-shadow: 0 6px 16px rgba(148, 163, 184, 0.3);
            }

        /* Table Modern */
        .table-modern {
            border: none;
            border-radius: var(--radius-md);
            overflow: hidden;
            box-shadow: var(--shadow-sm);
        }

            .table-modern thead th {
                background: linear-gradient(145deg, var(--primary), var(--secondary));
                color: white;
                border: none;
                padding: 1.25rem;
                font-weight: 700;
                font-size: 1.05rem;
            }

            .table-modern tbody td {
                border: none;
                border-bottom: 1px solid #e2e8f0;
                padding: 1.25rem;
                vertical-align: middle;
                font-size: 1rem;
            }

            .table-modern tbody tr:hover {
                background: var(--light);
            }

        /* Validation */
        .spanAsterisk {
            color: var(--danger);
            font-weight: bold;
            margin-left: 4px;
            font-size: 1.5rem;
        }

        .validationErrorMsg {
            color: var(--danger);
            font-size: 1rem;
            display: block;
            margin-top: 0.75rem;
            font-weight: 600;
        }

        /* Special Notice */
        .special-notice {
            color: #FFD700 !important;
            font-weight: 700 !important;
            animation: pulse 2s infinite;
        }

        @keyframes pulse {
            0% {
                opacity: 1;
            }

            50% {
                opacity: 0.7;
            }

            100% {
                opacity: 1;
            }
        }

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

        .basic-section {
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
            body {
                font-size: 15px;
            }

            .custom-navbar {
                border-radius: var(--radius-md);
            }

                .custom-navbar .navbar-nav {
                    flex-direction: column;
                    padding: 1rem 0;
                }

                    .custom-navbar .navbar-nav .nav-link {
                        padding: 1rem 1.25rem;
                        text-align: left;
                        justify-content: flex-start;
                        font-size: 1rem;
                    }

            .nav-item::before {
                display: none;
            }

            .auth-buttons {
                padding: 1rem;
                justify-content: flex-end;
                background: transparent;
            }

            .basic-section h4 {
                font-size: 1.35rem;
            }
        }

        @media (max-width: 768px) {
            .admin-container {
                padding: 1.5rem;
            }

            .basic-section {
                padding: 2rem;
            }

            .form-table-modern td {
                padding: 1rem;
                display: block;
                width: 100% !important;
                font-size: 1rem;
            }

                .form-table-modern td:first-child {
                    width: 100% !important;
                    font-weight: 700;
                    border-bottom: none;
                    padding-bottom: 0.5rem;
                    background: transparent;
                    font-size: 1.4rem;
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

            .quota-info-grid {
                grid-template-columns: 1fr;
            }

            .basic-section h4 {
                font-size: 1.25rem;
            }

            .panel-header-modern {
                padding: 1.5rem 1.75rem;
                font-size: 1.2rem;
            }

            .panel-body-modern {
                padding: 2rem;
            }

            .btn-modern {
                padding: 0.875rem 1.75rem;
                font-size: 1rem;
            }
        }

        @media (max-width: 576px) {
            body {
                font-size: 14px;
            }

            .logo-container img {
                max-height: 75px;
            }

            .admin-container {
                padding: 1rem;
            }

            .basic-section {
                padding: 1.5rem;
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
                        font-size: 1.5rem;
                    }

                    .breadcrumb-modern li a::after {
                        font-size: 1rem;
                        max-width: 60px;
                    }

            .panel-body-modern {
                padding: 1.5rem;
            }

            .btn-modern {
                padding: 0.75rem 1.5rem;
                font-size: 0.95rem;
                width: 100%;
                justify-content: center;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>

            <div class="admin-container">

                <!-- Progress Navigation -->
                <div class="progress-nav">
                    <nav>
                        <ol class="breadcrumb-modern">
                            <%--<li class="active">Basic</li>--%>
                            <li class="active">
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
                            <li>
                                <asp:HyperLink ID="hrefAppAdditional" runat="server" data-step="6" data-label="Additional/Work" title="Additional/Work"></asp:HyperLink>
                            </li>
                            <%--<li><asp:HyperLink ID="hrefAppFinGuar" runat="server">Financial Guarantor</asp:HyperLink></li>--%>
                            <li>
                                <asp:HyperLink ID="hrefAppAttachment" runat="server" data-step="7" data-label="Upload Photo" title="Upload Photo"></asp:HyperLink>
                            </li>
                            <li>
                                <asp:HyperLink ID="hrefAppDeclaration" runat="server" data-step="8" data-label="Declaration" title="Declaration"></asp:HyperLink>
                            </li>
                        </ol>
                    </nav>
                </div>

                <!-- Basic Information Section -->
                <div class="basic-section">
                    <h4>
                        <i class="fas fa-user-circle"></i>
                        Basic Information
                    </h4>

                    <div class="alert-modern">
                        <span class="spanAsterisk">*</span> indicate required fields.<br />
                        <span class="spanAsterisk">Please note that there is no validation for Admin in this form.</span>
                    </div>

                    <table class="form-table-modern">
                        <tr>
                            <td>Name in FULL <span class="spanAsterisk">*</span></td>
                            <td colspan="3">
                                <asp:TextBox ID="txtFirstName" runat="server" Width="100%" CssClass="form-control-modern"></asp:TextBox>
                            </td>
                        </tr>
                        <%--<tr>
                            <td class="style_td">Last Name</td>
                            <td>
                                <asp:TextBox ID="txtLastName" runat="server" Width="100%" CssClass="form-control-modern"></asp:TextBox>
                            </td>
                            <td class="style_td style_td_secondCol">Nick Name</td>
                            <td>
                                <asp:TextBox ID="txtNickName" runat="server" Width="100%" CssClass="form-control-modern"></asp:TextBox>
                            </td>
                        </tr>--%>

                        <%--<tr>
                            <td class="style_td" style="width: 15%">FULL Name <span class="spanAsterisk">*</span></td>
                            <td style="width: 35%">
                                
                            </td>
                            <td class="style_td style_td_secondCol" style="width: 15%">National ID / Birth Reg. No.<span class="spanAsterisk">*</span></td>
                            <td style="width: 35%">
                                
                            </td>
                        </tr>--%>

                        <tr>
                            <td>Date Of Birth <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:TextBox ID="txtDateOfBirth" runat="server" Width="100%" CssClass="form-control-modern" placeholder="dd/MM/yyyy"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalenderExtender_DOB" runat="server"
                                    TargetControlID="txtDateOfBirth" Format="dd/MM/yyyy" />
                            </td>
                            <td id="POBtdspan" runat="server">Place of Birth<span class="spanAsterisk" id="spanPOB" runat="server">*</span></td>
                            <td id="POBtdtxt" runat="server">
                                <asp:TextBox ID="txtPlaceOfBirth" runat="server" Width="100%" CssClass="form-control-modern"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="NationalityMotherTonge" runat="server">
                            <td>Nationality<span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlNationality" runat="server" Width="100%" CssClass="form-control-modern"></asp:DropDownList>
                            </td>
                            <td>Mother Tongue<span class="spanAsterisk" id="spanMotherTounge" runat="server">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlLanguage" runat="server" Width="100%" CssClass="form-control-modern"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Gender <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlGender" runat="server" Width="100%" CssClass="form-control-modern"></asp:DropDownList>
                            </td>
                            <td id="MaritalStatusspan" runat="server">Marital Status<span class="spanAsterisk" id="spanMaritalStatus" runat="server">*</span></td>
                            <td id="MaritalStatusddl" runat="server">
                                <asp:DropDownList ID="ddlMaritalStatus" runat="server" Width="100%" CssClass="form-control-modern"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>National ID / Birth Reg. No.</td>
                            <td>
                                <div style="display: flex; gap: 0.5rem;">
                                    <div style="flex: 1;">
                                        <asp:DropDownList ID="ddlNationalIdOrBirthRegistration" runat="server" Width="100%" CssClass="form-control-modern">
                                            <asp:ListItem Value="1">NID</asp:ListItem>
                                            <asp:ListItem Value="2">Birth Reg. No.</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div style="flex: 2;">
                                        <asp:TextBox ID="txtNationalIdOrBirthRegistration" runat="server" CssClass="form-control-modern" Width="100%"></asp:TextBox>
                                    </div>
                                </div>
                            </td>
                            <td>Religion <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlReligion" runat="server" Width="100%" CssClass="form-control-modern"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Blood Group <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:DropDownList ID="ddlBloodGroup" runat="server" Width="100%" CssClass="form-control-modern"></asp:DropDownList>
                            </td>
                            <td>Email <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" Width="100%" TextMode="Email" CssClass="form-control-modern"></asp:TextBox>
                            </td>
                        </tr>
                        <%--<td class="style_td style_td_secondCol">Phone<br />
                                (Res)</td>
                            <td>
                                <asp:TextBox ID="txtPhoneRes" runat="server" Width="100%" CssClass="form-control-modern"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Emergency Phone</td>
                            <td>
                                <asp:TextBox ID="txtPhoneEmergency" runat="server" Width="100%" CssClass="form-control-modern"></asp:TextBox>
                            </td>--%>
                        <tr>
                            <td>Mobile <span class="spanAsterisk">*</span></td>
                            <td>
                                <asp:TextBox ID="txtMobile" runat="server" Width="100%" CssClass="form-control-modern"
                                    placeholer="Format: +8801XXXXXXXXX"></asp:TextBox>
                                <asp:RegularExpressionValidator runat="server" ID="mobileReg"
                                    ValidationGroup="basic1"
                                    ForeColor="Crimson"
                                    ErrorMessage="Invalid format."
                                    ControlToValidate="txtMobile"
                                    ValidationExpression="^(\+88)\d{11}$"></asp:RegularExpressionValidator>
                            </td>
                            <td>Quota<span class="spanAsterisk">*</span></td>
                            <td>
                                <%--   <asp:UpdatePanel ID="UpdatePanelQuota" runat="server">
                                            <ContentTemplate>--%>
                                <asp:DropDownList ID="ddlQuota" runat="server" Width="100%" CssClass="form-control-modern" OnSelectedIndexChanged="ddlQuota_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                <%--  </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>

                    <!-- Quota Information Grid -->
                    <div runat="server" id="divQuota" style="margin-top: 2rem;">
                        <div class="quota-info-grid">
                            <div class="quota-card">
                                <b style="background-color: #d9534f; color: whitesmoke;">Freedom Fighter Quota</b>
                                <span>
                                    <ul>
                                        <li>Children/Grand Children of Freedom Fighter
                                        </li>
                                    </ul>
                                </span>
                            </div>
                            <div class="quota-card">
                                <b style="background-color: #d9534f; color: whitesmoke;">Special Quota</b>
                                <span>
                                    <ul>
                                        <li>Children of Armed Forces Personnel (Serving and Retired)</li>
                                        <li>Children of BUP Permanent Teacher, Officers, and Staffs (Serving and Retired)</li>
                                        <li>Children of Sitting members of BUP Governing Bodies (Senate, Syndicate, Academic Council and Finance Committee)</li>
                                    </ul>
                                </span>
                            </div>
                            <div class="quota-card">
                                <b style="background-color: #d9534f; color: whitesmoke;">Ethnic Minority</b>
                                <span>
                                    <ul>
                                        <li>Certificate issued by local Upazilla Nirbahi Officer (UNO)
                                        </li>
                                    </ul>
                                </span>
                            </div>
                            <div class="quota-card">
                                <b style="background-color: #d9534f; color: whitesmoke;">Disable</b>
                                <span>
                                    <ul>
                                        <li>Disable
                                        </li>
                                    </ul>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Quota Details Section -->
                <div>

                    <%--=========== Quota Info ===========--%>
                    <%--           <asp:UpdatePanel ID="UpdatePanelQuotaNote" runat="server">
                        <ContentTemplate>--%>
                    <asp:Panel ID="panelQuotaNote" runat="server" Visible="false">
                        <div class="alert-modern">
                            <strong style="color: crimson; font-size: 17px;">Caution!</strong>
                            <p style="color: crimson; font-size: 17px;">
                                Your application may be cancelled, if provided information is found wrong.
                           
                            </p>

                            <br />
                            <asp:Panel ID="panelQuotaNoteSpecialQuota" runat="server" Visible="false">
                                <strong>Eligibility:</strong>
                                <br />
                                <ul>
                                    <li>Children of Armed Forces Personnel (Serving and Retired)</li>
                                    <li>Children of BUP Permanent Teacher, Officers, and Staffs (Serving and Retired)</li>
                                    <li>Children of Sitting members of BUP Governing Bodies (Senate, Syndicate, Academic Council and Finance Committee)</li>
                                </ul>
                            </asp:Panel>
                            <asp:Panel ID="panelQuotaNoteFreedomFighter" runat="server" Visible="false">
                                <strong>Eligibility:</strong>
                                <br />
                                <ul>
                                    <li>Children/Grand Children of Freedom Fighter</li>
                                </ul>
                            </asp:Panel>
                            <asp:Panel ID="panelQuotaNotePersonWithDisability" runat="server" Visible="false">
                                <strong>Eligibility:</strong>
                                <br />
                                <ul>
                                    <li>Disable</li>
                                </ul>
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                    <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>

                    <%--  <asp:UpdatePanel ID="UpdatePanelQuotaInfo" runat="server">
                        <ContentTemplate>--%>

                    <%--=========== Special Quota ===========--%>
                    <asp:Panel ID="panelQuotaInfo" runat="server" Visible="false">
                        <div class="panel-modern">
                            <div class="panel-header-modern"><span style="font-size: 19px;"><strong>Quota Information</strong></span></div>
                            <div class="panel-body-modern">


                                <div style="margin-bottom: 1rem;">
                                    <div>

                                        <div>
                                            <label>Type of Special Quota<span class="spanAsterisk"></span></label>
                                            <asp:DropDownList ID="ddlSQQuotaType" runat="server" Width="100%" CssClass="form-control-modern"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlSQQuotaType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareValidator10" runat="server"
                                                ControlToValidate="ddlSQQuotaType"
                                                ErrorMessage="Required"
                                                Display="Dynamic"
                                                Font-Size="9pt"
                                                ForeColor="Crimson"
                                                ValueToCompare="-1"
                                                Operator="NotEqual">
                                            </asp:CompareValidator>
                                        </div>

                                    </div>
                                </div>

                                <%--====== Children of Military Personnel (Serving and Retired)
                                        ====== Children of BUP Permanent Teacher, Officers, and Staffs (Serving and Retired)--%>
                                <asp:Panel ID="panelChildrenOfMilitaryPersonnel" runat="server" Visible="false">
                                    <div style="margin-bottom: 1rem;">
                                        <div>
                                            <label>Serving / Retired<span class="spanAsterisk"></span></label>
                                            <%--<asp:DropDownList ID="rblServingRetired" runat="server" Width="100%" RepeatDirection="Horizontal"
                                                        AutoPostBack="true" OnSelectedIndexChanged="rblServingRetired_SelectedIndexChanged">
                                                        <asp:ListItem Value="1">Serving</asp:ListItem>
                                                        <asp:ListItem Value="2">Retired</asp:ListItem>
                                                    </asp:DropDownList>--%>
                                            <asp:DropDownList ID="rblServingRetired" runat="server" Width="100%" CssClass="form-control-modern"
                                                AutoPostBack="true" OnSelectedIndexChanged="rblServingRetired_SelectedIndexChanged">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                <asp:ListItem Value="1">Serving</asp:ListItem>
                                                <asp:ListItem Value="2">Retired</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </asp:Panel>


                                <asp:Panel ID="panelChildrenOfMilitaryPersonnelServingRetired" runat="server" Visible="false">

                                    <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 1rem; margin-bottom: 1rem;">
                                        <div>
                                            <div>
                                                <label>
                                                    <asp:Label ID="lblName1" runat="server"></asp:Label>
                                                    <span class="spanAsterisk"></span>
                                                </label>
                                                <asp:TextBox ID="txtInput1" runat="server" Width="100%" CssClass="form-control-modern"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server"
                                                    ID="RequiredFieldValidator5"
                                                    ControlToValidate="txtInput1"
                                                    Display="Dynamic"
                                                    Font-Size="9pt"
                                                    ForeColor="Crimson"
                                                    Font-Bold="false"
                                                    ErrorMessage="Required" />
                                            </div>
                                        </div>
                                        <div>
                                            <div>
                                                <label>
                                                    <asp:Label ID="lblName2" runat="server"></asp:Label>
                                                    <span class="spanAsterisk"></span>
                                                </label>
                                                <asp:TextBox ID="txtInput2" runat="server" Width="100%" CssClass="form-control-modern"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server"
                                                    ID="RequiredFieldValidator6"
                                                    ControlToValidate="txtInput2"
                                                    Display="Dynamic"
                                                    Font-Size="9pt"
                                                    ForeColor="Crimson"
                                                    Font-Bold="false"
                                                    ErrorMessage="Required" />
                                            </div>
                                        </div>
                                    </div>


                                    <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 1rem; margin-bottom: 1rem;">
                                        <div>
                                            <div>
                                                <label>
                                                    Father's/Mother's Name
                                                           
                                                    <asp:Label ID="lblshortnoteFM" runat="server" Style="font-size: 12px; color: #ff7800;"></asp:Label></label>
                                                <asp:TextBox ID="txtFatherName" runat="server" Width="100%" CssClass="form-control-modern"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator runat="server"
                                                            ID="RequiredFieldValidator8"
                                                            ValidationGroup="basic1"
                                                            ControlToValidate="txtFatherName"
                                                            Display="Dynamic"
                                                            Font-Size="9pt"
                                                            ForeColor="Crimson"
                                                            Font-Bold="false"
                                                            ErrorMessage="Required" />--%>
                                            </div>
                                        </div>
                                        <div>
                                            <div>
                                                <label>Father's/Mother's (Rank/Designation)</label>
                                                <asp:TextBox ID="txtFatherRankDesignation" runat="server" Width="100%" CssClass="form-control-modern"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator runat="server"
                                                            ID="RequiredFieldValidator1"
                                                            ValidationGroup="basic1"
                                                            ControlToValidate="txtFatherRankDesignation"
                                                            Display="Dynamic"
                                                            Font-Size="9pt"
                                                            ForeColor="Crimson"
                                                            Font-Bold="false"
                                                            ErrorMessage="Required" />--%>
                                            </div>
                                        </div>
                                    </div>

                                    <%--<div style="display: grid; grid-template-columns: 1fr 1fr; gap: 1rem; margin-bottom: 1rem;">
                                                <div>
                                                    <div>
                                                        <label>Mother Name</label>
                                                        <asp:TextBox ID="txtMotherName" runat="server" Width="100%" CssClass="form-control-modern"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div>
                                                    <div>
                                                        <label>Mother Rank / Designation</label>
                                                        <asp:TextBox ID="txtMotherRankDesignation" runat="server" Width="100%" CssClass="form-control-modern"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>--%>
                                </asp:Panel>
                                <%--====== END Children of Military Personnel (Serving and Retired)
                                        ====== END Children of BUP Permanent Teacher, Officers, and Staffs (Serving and Retired)--%>


                                <asp:Panel ID="panelChildrenOfSittingMembersOfBUPGoverningBodies" runat="server" Visible="false">



                                    <div style="margin-bottom: 1rem;">
                                        <div>
                                            <label>Committee Member</label>
                                            <%--<asp:RadioButtonList ID="rblGoverningBodie" runat="server" RepeatDirection="Horizontal"
                                                        AutoPostBack="true" OnSelectedIndexChanged="rblGoverningBodie_SelectedIndexChanged">
                                                        <asp:ListItem Value="1">Senate Committee Member</asp:ListItem>
                                                        <asp:ListItem Value="2">Syndicate Committee Member</asp:ListItem>
                                                        <asp:ListItem Value="3">Academic Council Member</asp:ListItem>
                                                        <asp:ListItem Value="4">Finance Committee Member</asp:ListItem>
                                                    </asp:RadioButtonList>--%>
                                            <asp:DropDownList ID="rblGoverningBodie" runat="server" Width="100%" CssClass="form-control-modern"
                                                AutoPostBack="true" OnSelectedIndexChanged="rblGoverningBodie_SelectedIndexChanged">
                                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                                <asp:ListItem Value="1">Senate Committee Member</asp:ListItem>
                                                <asp:ListItem Value="2">Syndicate Committee Member</asp:ListItem>
                                                <asp:ListItem Value="3">Academic Council Member</asp:ListItem>
                                                <asp:ListItem Value="4">Finance Committee Member</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>

                                    </div>


                                    <div style="margin-bottom: 1rem;">
                                        <div>
                                            <label>Committee Member Name</label>
                                            <asp:DropDownList ID="ddlGoverningBodie" runat="server"
                                                CssClass="form-control-modern">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                </asp:Panel>

                            </div>
                        </div>
                    </asp:Panel>

                    <%--=========== Freedom Fighter ===========--%>
                    <asp:Panel ID="panelFreedomFighterInfo" runat="server" Visible="false">
                        <div class="panel-modern">
                            <div class="panel-header-modern"><span style="font-size: 19px;"><strong>Quota Information</strong></span></div>
                            <div class="panel-body-modern">

                                <div style="margin-bottom: 1rem;">
                                    <div>
                                        <div>
                                            <label>Name of Freedom Fighter<span class="spanAsterisk"></span></label>
                                            <asp:TextBox ID="txtFFName" runat="server" Width="100%" CssClass="form-control-modern"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server"
                                                ID="RequiredFieldValidator3"
                                                ControlToValidate="txtFFName"
                                                Display="Dynamic"
                                                Font-Size="9pt"
                                                ForeColor="Crimson"
                                                Font-Bold="false"
                                                ErrorMessage="Required" />
                                        </div>
                                    </div>
                                </div>
                                <div style="margin-bottom: 1rem;">
                                    <div>
                                        <div>
                                            <label>Relation With Applicant<span class="spanAsterisk"></span></label>
                                            <asp:DropDownList ID="ddlFFQuotaType" runat="server" Width="100%" CssClass="form-control-modern"></asp:DropDownList>
                                            <asp:CompareValidator ID="CompareValidator3" runat="server"
                                                ControlToValidate="ddlFFQuotaType"
                                                ErrorMessage="Required"
                                                Display="Dynamic"
                                                Font-Size="9pt"
                                                ForeColor="Crimson"
                                                ValueToCompare="-1"
                                                Operator="NotEqual">
                                            </asp:CompareValidator>
                                        </div>
                                    </div>
                                </div>
                                <div style="margin-bottom: 1rem;">
                                    <div>
                                        <div>
                                            <label>Freedom Fighter No<span class="spanAsterisk"></span></label>
                                            <asp:TextBox ID="txtFFQFreedomFighterNo" runat="server" Width="100%" CssClass="form-control-modern"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server"
                                                ID="RequiredFieldValidator2"
                                                ControlToValidate="txtFFQFreedomFighterNo"
                                                Display="Dynamic"
                                                Font-Size="9pt"
                                                ForeColor="Crimson"
                                                Font-Bold="false"
                                                ErrorMessage="Required" />
                                        </div>
                                    </div>
                                </div>
                                <div style="margin-bottom: 1rem;">
                                    <div>
                                        <div>
                                            <label>Gazette Reference No</label>
                                            <asp:TextBox ID="txtFFQGazetteReferenceNo" runat="server" Width="100%" CssClass="form-control-modern"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator runat="server"
                                                        ID="RequiredFieldValidator1"
                                                        ValidationGroup="basic1"
                                                        ControlToValidate="txtFFQGazetteReferenceNo"
                                                        Display="Dynamic"
                                                        Font-Size="9pt"
                                                        ForeColor="Crimson"
                                                        Font-Bold="false"
                                                        ErrorMessage="Required" />--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <%--=========== Person With Disability ===========--%>
                    <asp:Panel ID="panelPersonWithDisabilityInfo" runat="server" Visible="false">
                        <div class="panel-modern">
                            <div class="panel-header-modern"><span style="font-size: 19px;"><strong>Quota Information</strong></span></div>
                            <div class="panel-body-modern">

                                <div style="margin-bottom: 1rem;">
                                    <div>
                                        <div>
                                            <label>Type of Disability<span class="spanAsterisk"></span></label>
                                            <asp:TextBox ID="txtPWDDisabilityName" runat="server" Width="100%" CssClass="form-control-modern"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server"
                                                ID="RequiredFieldValidator9"
                                                ControlToValidate="txtPWDDisabilityName"
                                                Display="Dynamic"
                                                Font-Size="9pt"
                                                ForeColor="Crimson"
                                                Font-Bold="false"
                                                ErrorMessage="Required" />
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </asp:Panel>

                    <%--=========== Quota Doc Upload ===========--%>
                    <asp:Panel ID="panelQuotaDocUpload" runat="server" Visible="false">
                        <div class="panel-modern">
                            <div class="panel-header-modern"><span style="font-size: 19px;"><strong>Upload Relevent Certificates/Documents</strong></span></div>
                            <div class="panel-body-modern">

                                <asp:Panel ID="panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Serving_Note" runat="server" Visible="false">
                                    <div class="alert-modern">
                                        <strong>Note:</strong>
                                        <br />
                                        <ul>
                                            <li>Upload a pdf/image copy of certificate from the Unit/HQ/Area Commander</li>
                                        </ul>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Retired_Note" runat="server" Visible="false">
                                    <div class="alert-modern">
                                        <strong>Note:</strong>
                                        <br />
                                        <ul>
                                            <li>Upload a pdf/image copy of certificate provided from CORO/Records</li>
                                        </ul>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Serving_Note" runat="server" Visible="false">
                                    <div class="alert-modern">
                                        <strong>Note:</strong>
                                        <br />
                                        <ul>
                                            <li>Upload a pdf/image copy of certificate signed by concerned Office/Department Head</li>
                                        </ul>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Retired_Note" runat="server" Visible="false">
                                    <div class="alert-modern">
                                        <strong>Note:</strong>
                                        <br />
                                        <ul>
                                            <li>Upload a pdf/image copy of certificate signed by concerned Office/Department Head</li>
                                        </ul>
                                    </div>
                                </asp:Panel>

                                <asp:Panel ID="panel_QuotaDocUpload_PersonWithDisability_Note" runat="server" Visible="false">
                                    <div class="alert-modern">
                                        <strong>Note:</strong>
                                        <br />
                                        <ul>
                                            <li>Upload medical certificate</li>
                                        </ul>
                                    </div>
                                </asp:Panel>

                                <asp:Panel ID="panel_QuotaDocUpload_Tribal_Note" runat="server" Visible="false">
                                    <div class="alert-modern">
                                        <strong>Note:</strong>
                                        <br />
                                        <ul>
                                            <li>Certificate issued by local Upazilla Nirbahi Officer (UNO)</li>
                                        </ul>
                                    </div>
                                </asp:Panel>


                                <div style="margin-bottom: 1rem;">

                                    <div>
                                        <strong>File Upload:</strong> <small style="color: crimson; font-style: italic;">*Only (.pdf, .jpg, .jpeg, .png)</small>
                                        <asp:FileUpload ID="FileUploadDocument" runat="server" CssClass="form-control-modern" />
                                    </div>
                                </div>

                                <div style="margin-bottom: 1rem;">
                                    <div>
                                        <asp:Button ID="btnUploadFile" Text="Upload Document" runat="server"
                                            OnClick="btnUploadFile_Click"
                                            CssClass="btn btn-default-modern" Style="width: 185px; margin-top: 5px;" />
                                    </div>
                                </div>

                                <hr />

                                <asp:GridView ID="gvQuotaDoc" runat="server" CssClass="table-modern table-responsive table-hover"
                                    AutoGenerateColumns="false" GridLines="none" Width="100%">
                                    <HeaderStyle BackColor="#1387de" ForeColor="White" />
                                    <Columns>

                                        <asp:TemplateField HeaderText="SL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate><%# Container.DataItemIndex + 1 %>.</ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="QuotaDocID" ItemStyle-HorizontalAlign="Center" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblQuotaDocID"
                                                    Text='<%#Eval("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="CandidateId" ItemStyle-HorizontalAlign="Center" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblCandidateId"
                                                    Text='<%#Eval("CandidateId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="QuotaId" ItemStyle-HorizontalAlign="Center" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblQuotaId"
                                                    Text='<%#Eval("QuotaId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblName"
                                                    Text='<%#Eval("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlDownload" runat="server" Text="View" class="btn btn-primary-modern btn-xs"
                                                    NavigateUrl='<%# "~/Upload/Candidate/QuotaDoc/" + Eval("Name") %>'
                                                    Target="_blank"></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:Button ID="btnDelete" runat="server" Style="color: crimson;"
                                                    Text="Delete" class="btn btn-link btn-xs"
                                                    OnClientClick="return confirm('Are you sure, you want to Delete Document!')"
                                                    OnClick="btnDelete_Click"></asp:Button>
                                            </ItemTemplate>
                                        </asp:TemplateField>


                                    </Columns>
                                </asp:GridView>

                            </div>
                        </div>
                    </asp:Panel>
                    <%-- </ContentTemplate>
                       
                    </asp:UpdatePanel>--%>
                    <%--=========== END Quota Info ===========--%>
                </div>




                <%--=========== Exam Venue Selection ===========--%>
                <%--    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>--%>

                <div>
                    <div>
                        <asp:Panel ID="PanelExamSeatInformation" runat="server" Visible="false">
                            <div class="panel-modern">
                                <div class="panel-header-modern"><span style="font-size: 19px;"><strong>Exam Venue Selection</strong></span></div>
                                <div class="panel-body-modern">

                                    <asp:Panel ID="panel_Massage" runat="server" Visible="false">
                                        <asp:Label ID="districtMassage" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                                    </asp:Panel>

                                    <asp:GridView ID="gvFacultyList" runat="server" CssClass="table-modern table-responsive table-hover"
                                        AutoGenerateColumns="false" GridLines="none" Width="100%"
                                        OnRowDataBound="gvFacultyList_RowDataBound">
                                        <%--OnRowCommand="gvFacultyList_RowCommand"--%>
                                        <HeaderStyle BackColor="#1387de" ForeColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="SL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                                <ItemTemplate><%# Container.DataItemIndex + 1 %>.</ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="CandidateFacultyWiseDistrictSeatId" HeaderStyle-Width="40%" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCandidateFacultyWiseDistrictSeatId"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="AdmissionUnitId" HeaderStyle-Width="40%" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblAdmissionUnitId"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="AdmissionSetupId" HeaderStyle-Width="40%" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblAdmissionSetupId"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Faculty Name" HeaderStyle-Width="40%">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblFacultyName"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="District" HeaderStyle-Width="30%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlDistrict" runat="server" Width="50%" AutoPostBack="true" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged" CssClass="form-control-modern"></asp:DropDownList>
                                                    <%-- <asp:CompareValidator ID="ddlDistrictComV" runat="server"
                                                                            ControlToValidate="ddlDistrict" ErrorMessage="Required" ForeColor="Crimson"
                                                                            ValueToCompare="-1" Operator="NotEqual" ValidationGroup="basic1"></asp:CompareValidator>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>

                <%--</ContentTemplate>
                            </asp:UpdatePanel>--%>
                <%--=========== Exam Venue Selection ===========--%>


                <asp:Panel ID="messagePanel_Basic" runat="server">
                    <asp:Label ID="lblMessageBasic" runat="server" Text=""></asp:Label>
                </asp:Panel>

                <div style="text-align: center; padding: 2rem; background: white; border-radius: var(--radius-lg); box-shadow: var(--shadow-md); margin-top: 2rem;">
                    <asp:Button ID="btnSave_Basic" runat="server" Text="Save"
                        CssClass="btn btn-primary-modern" ValidationGroup="basic1"
                        OnClick="btnSave_Basic_Click" />

                    <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                        CssClass="btn btn-primary-modern" />

                    <span id="validationMsg" class="validationErrorMsg"></span>
                </div>

            </div>
            <%-- end admin-container --%>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUploadFile" />
            <asp:PostBackTrigger ControlID="btnSave_Basic" />
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>
