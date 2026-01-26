<%@ Page Title="Admin - Application Relation" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CandApplicationRelation.aspx.cs" Inherits="Admission.Admission.Office.CandidateInfo.CandApplicationRelation" %>

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

        /* Relation Sections */
        .relation-section {
            background: white;
            border-radius: var(--radius-lg);
            padding: 0;
            margin-bottom: 2rem;
            box-shadow: var(--shadow-md);
            border: 1px solid rgba(59, 130, 246, 0.1);
            overflow: hidden;
            position: relative;
        }

            .relation-section::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                height: 5px;
                background: linear-gradient(90deg, var(--accent), var(--secondary));
                border-radius: var(--radius-lg) var(--radius-lg) 0 0;
            }

        .section-header {
            background: linear-gradient(145deg, var(--accent), var(--secondary));
            color: white;
            padding: 1.75rem 2rem;
            font-weight: 700;
            font-size: 2rem;
            text-align: center;
            border-bottom: none;
        }

        .section-body {
            padding: 2rem;
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

        .relation-section {
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
            .section-body {
                padding: 1.5rem;
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

            .section-body {
                padding: 1.25rem;
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

            .section-body {
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

    <asp:UpdatePanel ID="updatePanelParent" runat="server" ChildrenAsTriggers="true">
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
                            <li class="active">
                                <asp:HyperLink ID="hrefAppRelation" runat="server" data-step="4" data-label="Parent/Guardian" title="Parent/Guardian"></asp:HyperLink>
                            </li>
                            <li>
                                <asp:HyperLink ID="hrefAppAddress" runat="server" data-step="5" data-label="Address" title="Address"></asp:HyperLink>
                            </li>
                            <li>
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

                <!-- Father & Mother Information -->
                <div class="row">
                    <div class="col-md-6">
                        <div class="relation-section">
                            <div class="section-header">
                                Father
                            </div>
                            <div class="section-body">
                                <table class="form-table-modern">
                                    <tr>
                                        <td>Father's Name <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtFatherName" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Late?</td>
                                        <td>
                                            <asp:DropDownList ID="ddlIsLateFather" runat="server" CssClass="form-control-modern" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlIsLateFather_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td>Occupation <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtFatherOccupation" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Occupation Type<span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlFatherOccupationType" runat="server" CssClass="form-control-modern" AutoPostBack="true" OnSelectedIndexChanged="ddlFatherOccupationType_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="fatherService" runat="server" visible="false">
                                        <td>Service Type <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlFatherServiceType" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="fatherOrganization" runat="server" visible="false">
                                        <td>Organization <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtFatherOrganization" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="fatherDesignation" runat="server" visible="false">
                                        <td>Designation <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtFatherDesignation" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Mobile <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtFatherMobile" runat="server" CssClass="form-control-modern"
                                                placeholder="Format: +8801XXXXXXXXX"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Email</td>
                                        <td>
                                            <asp:TextBox ID="txtFatherEmail" runat="server" CssClass="form-control-modern"
                                                TextMode="Email"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>National ID / Birth Cert No. / Passport No. <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtFatherNationalId" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Nationality <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlFatherNationality" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="relation-section">
                            <div class="section-header">
                                Mother
                            </div>
                            <div class="section-body">
                                <table class="form-table-modern">
                                    <tr>
                                        <td>Mother's Name <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtMotherName" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Late?</td>
                                        <td>
                                            <asp:DropDownList ID="ddlIsLateMother" runat="server" CssClass="form-control-modern" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlIsLateMother_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td>Occupation <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtMotherOccupation" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Occupation Type<span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlMotherOccupationType" runat="server" CssClass="form-control-modern" AutoPostBack="true" OnSelectedIndexChanged="ddlMotherOccupationType_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="motherService" runat="server" visible="false">
                                        <td>Service Type <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlMotherServiceType" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="motherOrganization" runat="server" visible="false">
                                        <td>Organization <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtMotherOrganization" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="motherDesignation" runat="server" visible="false">
                                        <td>Designation <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtMotherDesignation" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Mobile <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtMotherMobile" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>National ID / Birth Cert No. / Passport No. <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtMotherNationalId" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Nationality <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlMotherNationality" runat="server" CssClass="form-control-modern">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Guardian Information -->
                <div class="row">
                    <div class="col-md-6">
                        <div class="relation-section">
                            <div class="section-header">
                                Guardian
                            </div>
                            <div class="section-body">
                                <asp:UpdatePanel ID="updatePanel_Guardian" runat="server">
                                    <ContentTemplate>
                                        <table class="form-table-modern">
                                            <tr>
                                                <td>Relationship with the applicant <span class="spanAsterisk">*</span></td>
                                                <td>
                                                    <asp:DropDownList ID="ddlGuardianRelation" runat="server" CssClass="form-control-modern" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ddlGuardianRelation_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Guardian's Name <span class="spanAsterisk">*</span></td>
                                                <td>
                                                    <asp:TextBox ID="txtGuardian_Name" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Other</td>
                                                <td>
                                                    <asp:TextBox ID="txtGuardianOtherRelation" runat="server" CssClass="form-control-modern" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr style="display: none">
                                                <td>Occupation</td>
                                                <td>
                                                    <asp:TextBox ID="txtGuardianOccupation" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Occupation Type<span class="spanAsterisk">*</span></td>
                                                <td>
                                                    <asp:DropDownList ID="ddlGuardianOccupationType" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Organization</td>
                                                <td>
                                                    <asp:TextBox ID="txtGuardianOrganization" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Address</td>
                                                <td>
                                                    <asp:TextBox ID="txtGuardianMailingAddress" runat="server" CssClass="form-control-modern"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Email</td>
                                                <td>
                                                    <asp:TextBox ID="txtGuardianEmail" runat="server" CssClass="form-control-modern"
                                                        TextMode="Email"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Mobile <span class="spanAsterisk">*</span></td>
                                                <td>
                                                    <asp:TextBox ID="txtGuardianMobile" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>National ID / Birth Cert No. / Passport No. <span class="spanAsterisk">*</span></td>
                                                <td>
                                                    <asp:TextBox ID="txtGuardianNationalId" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Nationality <span class="spanAsterisk">*</span></td>
                                                <td>
                                                    <asp:DropDownList ID="ddlGuardianNationality" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>

                <asp:Panel ID="messagePanel_Parent" runat="server">
                    <asp:Label ID="lblMessageParent" runat="server" Text=""></asp:Label>
                </asp:Panel>

                <div style="text-align: center; padding: 2rem; background: white; border-radius: var(--radius-lg); box-shadow: var(--shadow-md); margin-top: 2rem;">
                    <asp:Button ID="btnSave_Parent" runat="server" Text="Save"
                        CssClass="btn btn-primary-modern" OnClick="btnSave_Parent_Click"
                        ValidationGroup="parent1" />

                    <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                        CssClass="btn btn-primary-modern" />

                    <span id="validationMsg" class="validationErrorMsg"></span>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>