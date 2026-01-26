<%@ Page Title="Admin - Application Education" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CandApplicationEducation.aspx.cs" Inherits="Admission.Admission.Office.CandidateInfo.CandApplicationEducation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.css" rel="stylesheet" type="text/css" />

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

        /* Education Sections */
        .education-section {
            background: white;
            border-radius: var(--radius-lg);
            padding: 0;
            margin-bottom: 2rem;
            box-shadow: var(--shadow-md);
            border: 1px solid rgba(59, 130, 246, 0.1);
            overflow: hidden;
            position: relative;
        }

            .education-section::before {
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

            .section-header small {
                display: block;
                font-size: 1.2rem;
                font-weight: 400;
                margin-top: 0.25rem;
                opacity: 0.9;
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

        .btn-info-modern {
            background: linear-gradient(145deg, #0ea5e9, #0284c7);
            color: white;
            box-shadow: var(--shadow-sm);
        }

            .btn-info-modern:hover {
                transform: translateY(-2px);
                box-shadow: var(--shadow-md);
            }

        .btn-success-modern {
            background: linear-gradient(145deg, var(--success), #047857);
            color: white;
            box-shadow: var(--shadow-sm);
        }

            .btn-success-modern:hover {
                transform: translateY(-2px);
                box-shadow: var(--shadow-md);
            }

        .btn-danger-modern {
            background: linear-gradient(145deg, var(--danger), #b91c1c);
            color: white;
            box-shadow: var(--shadow-sm);
        }

            .btn-danger-modern:hover {
                transform: translateY(-2px);
                box-shadow: var(--shadow-md);
            }

        /* GridView Modern */
        .table-modern {
            border: none;
            border-radius: var(--radius-md);
            overflow: hidden;
            box-shadow: var(--shadow-sm);
            width: 100%;
        }

            .table-modern thead th {
                background: linear-gradient(145deg, var(--primary), var(--secondary));
                color: white;
                border: none;
                padding: 1rem;
                font-weight: 700;
                font-size: 1rem;
            }

            .table-modern tbody td {
                border: none;
                border-bottom: 1px solid #e2e8f0;
                padding: 1rem;
                vertical-align: middle;
                font-size: 0.95rem;
            }

            .table-modern tbody tr:hover {
                background: var(--light);
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

        /* Modal */
        .modalBackground {
            background-color: rgba(17, 24, 39, 0.8);
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .modal-panel {
            background: white;
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-lg);
            max-width: 900px;
            margin: 2rem auto;
        }

        /* Animation */
        .blink {
            animation: blinker 0.6s linear infinite;
            color: var(--accent);
            font-size: 1rem;
            font-weight: bold;
        }

        @keyframes blinker {
            50% {
                opacity: 0;
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

        .education-section {
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

    <div id="divProgress" style="display: none; z-index: 1000000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="250px" Width="250px" />
    </div>

    <asp:UpdatePanel ID="updatePanelEducation" runat="server" ChildrenAsTriggers="true">
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
                            <li class="active">
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
                            <li>
                                <asp:HyperLink ID="hrefAppAttachment" runat="server" data-step="7" data-label="Upload Photo" title="Upload Photo"></asp:HyperLink>
                            </li>
                            <li>
                                <asp:HyperLink ID="hrefAppDeclaration" runat="server" data-step="8" data-label="Declaration" title="Declaration"></asp:HyperLink>
                            </li>
                        </ol>
                    </nav>
                </div>

                <asp:HiddenField ID="hfEduCat" runat="server" />

                <div class="alert-modern">
                    <span class="spanAsterisk">*</span> indicate required fields.<br />
                    <span class="spanAsterisk">Please note that there is no validation for Admin in this form.</span>
                </div>

                <!-- Secondary & Higher Secondary Education -->
                <div class="row">
                    <div class="col-md-6">
                        <div class="education-section">
                            <div class="section-header">
                                Secondary School / O-Level
                            </div>
                            <div class="section-body">
                                <table class="form-table-modern">
                                    <tr>
                                        <td>Exam Type <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlSec_ExamType" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Education Board <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlSec_EducationBrd" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Institute <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtSec_Institute" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Roll Number <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtSec_RollNo" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td>Registration Number</td>
                                        <td>
                                            <asp:TextBox ID="txtSec_RegNo" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Group Or Subject <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlSec_GrpOrSub" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Division/Class <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlSec_DivClass" runat="server" CssClass="form-control-modern"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlSec_DivClass_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>GPA/Score <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtSec_CgpaScore" runat="server" CssClass="form-control-modern" Enabled="false"></asp:TextBox>
                                            <asp:RangeValidator ID="txtSec_CgpaScore_RV" runat="server" ControlToValidate="txtSec_CgpaScore"
                                                ErrorMessage="GPA must be between 1.0 - 5.0" ForeColor="Crimson" Display="Dynamic"
                                                ValidationGroup="education1" MaximumValue="5" MinimumValue="0" Type="Double"></asp:RangeValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>GPA Without 4th Subject</td>
                                        <td>
                                            <asp:TextBox ID="txtSec_CgpaW4S" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                            <asp:RangeValidator ID="txtSec_CgpaW4S_RV" runat="server" ControlToValidate="txtSec_CgpaW4S"
                                                ErrorMessage="GPA (without 4th subject) must be between 1.0 - 5.0" ForeColor="Crimson" Display="Dynamic"
                                                ValidationGroup="education1" MaximumValue="5" MinimumValue="0" Type="Double"></asp:RangeValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Total Obtained Marks<span class="spanAsterisk"></span></td>
                                        <td>
                                            <asp:TextBox ID="txtSec_Marks" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trOutofMarksForOLevelSSC" runat="server">
                                        <td>Out of<asp:Label ID="Label2" runat="server" Text="  *" ForeColor="Red" Font-Bold="true"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtOutofSec_Marks" runat="server" CssClass="form-control-modern" placeholder="Required for Masters candidates"></asp:TextBox>
                                            <asp:LinkButton ID="lnkAddResult" runat="server" OnClick="lnkAddResult_Click" CssClass="btn btn-info-modern" Style="margin-top: 0.5rem;">Calculate Marks</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Exam Year <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlSec_PassingYear" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="education-section">
                            <div class="section-header">
                                Higher Secondary School / A-Level
                            </div>
                            <div class="section-body">
                                <table class="form-table-modern">
                                    <tr>
                                        <td>Exam Type <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlHigherSec_ExamType" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Education Board <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlHigherSec_EducationBrd" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Institute <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtHigherSec_Institute" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Roll Number <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtHigherSec_RollNo" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td>Registration Number</td>
                                        <td>
                                            <asp:TextBox ID="txtHigherSec_RegNo" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Group Or Subject <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlHigherSec_GrpOrSub" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Division/Class <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlHigherSec_DivClass" runat="server" CssClass="form-control-modern"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlHigherSec_DivClass_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>GPA/Score <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtHigherSec_CgpaScore" runat="server" CssClass="form-control-modern" Enabled="false"></asp:TextBox>
                                            <asp:RangeValidator ID="txtHigherSec_CgpaScore_RV" runat="server" ControlToValidate="txtHigherSec_CgpaScore"
                                                ErrorMessage="GPA must be between 1.0 - 5.0" ForeColor="Crimson" Display="Dynamic"
                                                ValidationGroup="education1" MaximumValue="5" MinimumValue="0" Type="Double"></asp:RangeValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>GPA Without 4th Subject</td>
                                        <td>
                                            <asp:TextBox ID="txtHigherSec_GpaW4S" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                            <asp:RangeValidator ID="txtHigherSec_GpaW4S_RV" runat="server" ControlToValidate="txtHigherSec_GpaW4S"
                                                ErrorMessage="GPA (without 4th subject) must be between 1.0 - 5.0" ForeColor="Crimson" Display="Dynamic"
                                                ValidationGroup="education1" MaximumValue="5" MinimumValue="0" Type="Double"></asp:RangeValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Total Obtained Marks <span class="spanAsterisk"></span></td>
                                        <td>
                                            <asp:TextBox ID="txtHigherSec_Marks" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trOutofMarksForOLevelHSC" runat="server">
                                        <td>Out of<asp:Label ID="Label1" runat="server" Text="  *" ForeColor="Red" Font-Bold="true"></asp:Label></td>
                                        <td>
                                            <asp:TextBox ID="txtOutofHigherSec_Marks" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                            <asp:LinkButton ID="lnkAddALevelResult" runat="server" OnClick="lnkAddALevelResult_Click" CssClass="btn btn-info-modern" Style="margin-top: 0.5rem;">Calculate Marks</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Exam Year <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlHigherSec_PassingYear" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Bachelor & Masters Education -->
                <asp:HiddenField ID="hfIsUndergrad" runat="server" />
                <asp:Panel ID="panel_isUndergrad" runat="server">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="education-section">
                                <div class="section-header">
                                    Bachelor
                                    <small>(Only for Candidates applying for Masters Program)</small>
                                </div>
                                <div class="section-body">
                                    <table class="form-table-modern">
                                        <tr>
                                            <td>Institute <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:TextBox ID="txtUndergrad_Institute" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Program/Degree <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlUndergrad_ProgramDegree" runat="server" CssClass="form-control-modern"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlUndergrad_ProgramDegree_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Others <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:TextBox ID="txtUndergrad_ProgOthers" runat="server" CssClass="form-control-modern" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="display: none">
                                            <td>Group Or Subject <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlUndergrad_GrpOrSub" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Division/Class <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlUndergrad_DivClass" runat="server" CssClass="form-control-modern"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlUndergrad_DivClass_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>CGPA/Score <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:TextBox ID="txtUndergrad_CgpaScore" runat="server" CssClass="form-control-modern" Enabled="false"></asp:TextBox>
                                                <asp:RangeValidator ID="txtUndergrad_CgpaScore_RV" runat="server" ControlToValidate="txtUndergrad_CgpaScore"
                                                    MinimumValue="1" MaximumValue="4" Display="Dynamic" ErrorMessage="Invalid number"
                                                    ForeColor="Crimson" Type="Double" ValidationGroup="education1"></asp:RangeValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Exam Year <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlUndergrad_PassingYear" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="education-section">
                                <div class="section-header">
                                    Masters
                                    <small>(Only for Candidates applying for Masters Program)</small>
                                </div>
                                <div class="section-body">
                                    <table class="form-table-modern">
                                        <tr>
                                            <td>Institute <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:TextBox ID="txtGraduate_Institute" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Program/Degree <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlGraduate_ProgramDegree" runat="server" CssClass="form-control-modern"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlGraduate_ProgramDegree_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Others <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:TextBox ID="txtGraduate_ProgOthers" runat="server" CssClass="form-control-modern" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="display: none">
                                            <td>Group Or Subject <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlGraduate_GrpOrSub" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Division/Class <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlGraduate_DivClass" runat="server" CssClass="form-control-modern"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlGraduate_DivClass_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>CGPA/Score</td>
                                            <td>
                                                <asp:TextBox ID="txtGraduate_CgpaScore" runat="server" CssClass="form-control-modern" Enabled="false"></asp:TextBox>
                                                <asp:RangeValidator ID="txtGraduate_CgpaScore_RV" runat="server" ControlToValidate="txtGraduate_CgpaScore"
                                                    MinimumValue="1" MaximumValue="4" Display="Dynamic" ErrorMessage="Invalid number"
                                                    ForeColor="Crimson" Type="Double" ValidationGroup="education1"></asp:RangeValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Exam Year <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlGraduate_PassingYear" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="messagePanel_Education" runat="server">
                    <asp:Label ID="lblMessageEducation" runat="server" Text=""></asp:Label>
                </asp:Panel>

                <div style="text-align: center; padding: 2rem; background: white; border-radius: var(--radius-lg); box-shadow: var(--shadow-md); margin-top: 2rem;">
                    <asp:Button ID="btnSave_Education" runat="server" Text="Save"
                        CssClass="btn btn-primary-modern" OnClick="btnSave_Education_Click"
                        ValidationGroup="education1" />

                    <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                        CssClass="btn btn-primary-modern" />

                    <span id="validationMsg" class="validationErrorMsg"></span>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- Modal Popup for O-Level Subject Wise Result -->
    <div class="col-md-15 col-lg-12">
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>
                <asp:Button ID="Button1" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="modalPopupSubjectWiseResult" runat="server" TargetControlID="Button1" PopupControlID="Panel2"
                    BackgroundCssClass="modalBackground" CancelControlID="btnCancel">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="Panel2" Style="display: none; padding: 20px; max-height: 600px; overflow-y: auto"
                    CssClass="modal-panel" BackColor="White">

                    <div class="education-section">
                        <div class="section-header">
                            Subject wise result entry
                        </div>
                        <div class="section-body">

                            <div class="row" style="margin-bottom: 1.5rem;">
                                <div class="col-lg-6 col-md-6 col-sm-12">
                                    <label style="font-weight: 600; margin-bottom: 0.5rem; display: block;">Number of subjects appeared</label>
                                    <asp:TextBox ID="txtOlevelSubjetNo" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                </div>
                                <div class="col-lg-3 col-md-3 col-sm-12" style="display: flex; align-items: flex-end;">
                                    <asp:LinkButton ID="lnkGenerateOlevel" runat="server" OnClick="lnkGenerateOlevel_Click" CssClass="btn btn-info-modern" Style="width: 100%;">Generate</asp:LinkButton>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <asp:Panel ID="Panel1" runat="server">
                                        <asp:GridView runat="server" ID="gvOLevelSubjectResult" AutoGenerateColumns="False"
                                            AllowPaging="false" PageSize="20" CellPadding="4" Width="100%"
                                            ShowHeader="true" CssClass="table-modern" GridLines="None">

                                            <HeaderStyle BackColor="#4781f0" ForeColor="White" Height="30" Font-Bold="True" />
                                            <AlternatingRowStyle BackColor="White" />

                                            <Columns>
                                                <asp:TemplateField HeaderText="SL#">
                                                    <ItemTemplate>
                                                        <b><%# Container.DataItemIndex + 1 %></b>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Subject">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSubjectNo" runat="server" Text='<%#Eval("SubjectNo") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblOLevelSubject" runat="server" Text='<%# "Subject " +Eval("SubjectNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Subject Total">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtOLevelMark" runat="server" TextMode="Number" min="1" CssClass="form-control-modern"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" Width="10%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <div style="text-align: center">
                                                            <asp:LinkButton ID="lnkOLevelSave" runat="server" OnClick="lnkOLevelSave_Click"
                                                                CssClass="btn btn-success-modern" Style="margin-bottom: 0.5rem;">Calculate</asp:LinkButton>
                                                            <br />
                                                            <asp:Label ID="lblTitle" runat="server" Text="Obtained Grade"></asp:Label>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlOLevelGrade" runat="server" CssClass="form-control-modern">
                                                            <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="A*" Value="95"></asp:ListItem>
                                                            <asp:ListItem Text="A" Value="85"></asp:ListItem>
                                                            <asp:ListItem Text="B" Value="75"></asp:ListItem>
                                                            <asp:ListItem Text="C" Value="65"></asp:ListItem>
                                                            <asp:ListItem Text="D" Value="55"></asp:ListItem>
                                                            <asp:ListItem Text="E" Value="45"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" Width="15%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </div>

                            <div class="row" style="margin-top: 1.5rem; text-align: center;">
                                <div class="col-lg-12">
                                    <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-danger-modern">Close</asp:LinkButton>
                                </div>
                            </div>

                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <!-- Modal Popup for A-Level Subject Wise Result -->
    <div class="col-md-15 col-lg-12">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Button ID="Button2" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="modalPopupSubjectWiseResultALevel" runat="server" TargetControlID="Button2" PopupControlID="Panel3"
                    BackgroundCssClass="modalBackground" CancelControlID="LinkButton2">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="Panel3" Style="display: none; padding: 20px; max-height: 600px; overflow-y: auto"
                    CssClass="modal-panel" BackColor="White">

                    <div class="education-section">
                        <div class="section-header">
                            Subject wise result entry
                        </div>
                        <div class="section-body">

                            <div class="row" style="margin-bottom: 1.5rem;">
                                <div class="col-lg-6 col-md-6 col-sm-12">
                                    <label style="font-weight: 600; margin-bottom: 0.5rem; display: block;">Number of subjects appeared</label>
                                    <asp:TextBox ID="txtAlevelSubjetNo" runat="server" CssClass="form-control-modern"></asp:TextBox>
                                </div>
                                <div class="col-lg-3 col-md-3 col-sm-12" style="display: flex; align-items: flex-end;">
                                    <asp:LinkButton ID="lnkGenerateAlevel" runat="server" OnClick="lnkGenerateAlevel_Click" CssClass="btn btn-info-modern" Style="width: 100%;">Generate</asp:LinkButton>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12">
                                    <asp:Panel ID="Panel4" runat="server">
                                        <asp:GridView runat="server" ID="gvALevelSubjectResult" AutoGenerateColumns="False"
                                            AllowPaging="false" PageSize="20" CellPadding="4" Width="100%"
                                            ShowHeader="true" CssClass="table-modern" GridLines="None">

                                            <HeaderStyle BackColor="#4781f0" ForeColor="White" Height="30" Font-Bold="True" />
                                            <AlternatingRowStyle BackColor="White" />

                                            <Columns>
                                                <asp:TemplateField HeaderText="SL#">
                                                    <ItemTemplate>
                                                        <b><%# Container.DataItemIndex + 1 %></b>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Subject">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblASubjectNo" runat="server" Text='<%#Eval("SubjectNo") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblALevelSubject" runat="server" Text='<%# "Subject " +Eval("SubjectNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Subject Total">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtALevelMark" runat="server" TextMode="Number" min="1" CssClass="form-control-modern"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" Width="10%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <div style="text-align: center">
                                                            <asp:LinkButton ID="lnkALevelSave" runat="server" OnClick="lnkALevelSave_Click"
                                                                CssClass="btn btn-success-modern" Style="margin-bottom: 0.5rem;">Calculate</asp:LinkButton>
                                                            <br />
                                                            <asp:Label ID="lblATitle" runat="server" Text="Obtained Grade"></asp:Label>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlALevelGrade" runat="server" CssClass="form-control-modern">
                                                            <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="A*" Value="95"></asp:ListItem>
                                                            <asp:ListItem Text="A" Value="85"></asp:ListItem>
                                                            <asp:ListItem Text="B" Value="75"></asp:ListItem>
                                                            <asp:ListItem Text="C" Value="65"></asp:ListItem>
                                                            <asp:ListItem Text="D" Value="55"></asp:ListItem>
                                                            <asp:ListItem Text="E" Value="45"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" Width="15%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </div>

                            <div class="row" style="margin-top: 1.5rem; text-align: center;">
                                <div class="col-lg-12">
                                    <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-danger-modern">Close</asp:LinkButton>
                                </div>
                            </div>

                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="updatePanelEducation" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnSave_Education" Enabled="false" />
                    <EnableAction AnimationTarget="btnNext" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction AnimationTarget="btnSave_Education" Enabled="true" />
                    <EnableAction AnimationTarget="btnNext" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>