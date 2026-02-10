<%@ Page Title="Application Form - Education" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApplicationEducation.aspx.cs" Inherits="Admission.Admission.Candidate.ApplicationEducation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script src="Scripts/EducationValidation.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.css" rel="stylesheet" type="text/css" />

    <style>
        /* Modern Education Form Styling */
        .application-container {
            background: rgba(255, 255, 255, 0.95);
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-lg);
            padding: 1rem;
            margin-bottom: 2rem;
            backdrop-filter: blur(10px);
            -webkit-backdrop-filter: blur(10px);
        }

        .progress-nav {
            background: rgba(255, 255, 255, 0.95);
            border-radius: var(--radius-lg);
            padding: 1.5rem;
            margin-bottom: 2rem;
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

            .progress-nav .cd-breadcrumb {
                display: flex;
                align-items: center;
                justify-content: space-between;
                margin: 0;
                padding: 0;
                list-style: none;
                position: relative;
                z-index: 1;
            }

                .progress-nav .cd-breadcrumb::before {
                    content: '';
                    position: absolute;
                    top: 50%;
                    left: 0;
                    right: 0;
                    height: 3px;
                    background: linear-gradient(to right, #e2e8f0, var(--accent-light), #e2e8f0);
                    border-radius: 2px;
                    z-index: -1;
                }

                .progress-nav .cd-breadcrumb li {
                    position: relative;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    min-width: 60px;
                    min-height: 60px;
                    transition: var(--transition);
                    z-index: 2;
                }

                    .progress-nav .cd-breadcrumb li a {
                        position: relative;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        width: 60px;
                        height: 60px;
                        background: white;
                        border-radius: 50%;
                        border: 3px solid #e2e8f0;
                        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
                        text-decoration: none;
                        transition: var(--transition);
                        cursor: pointer;
                        color: var(--gray);
                        font-weight: 700;
                        font-size: 0.85rem;
                    }

                        .progress-nav .cd-breadcrumb li a::before {
                            content: attr(data-step);
                            position: absolute;
                            top: 50%;
                            left: 50%;
                            transform: translate(-50%, -50%);
                            width: 30px;
                            height: 30px;
                            border-radius: 50%;
                            background: #e2e8f0;
                            color: var(--gray);
                            font-weight: 700;
                            font-size: 0.85rem;
                            display: flex;
                            align-items: center;
                            justify-content: center;
                            transition: var(--transition);
                        }

                        .progress-nav .cd-breadcrumb li a::after {
                            content: attr(data-label);
                            position: absolute;
                            top: 100%;
                            left: 50%;
                            transform: translateX(-50%);
                            color: var(--dark);
                            font-weight: 500;
                            font-size: 0.75rem;
                            white-space: nowrap;
                            margin-top: 0.5rem;
                            transition: var(--transition);
                            text-align: center;
                            max-width: 80px;
                            overflow: hidden;
                            text-overflow: ellipsis;
                        }

                    .progress-nav .cd-breadcrumb li.current a {
                        border-color: var(--accent);
                        transform: scale(1.1);
                        box-shadow: 0 6px 20px rgba(59, 130, 246, 0.3);
                    }

                        .progress-nav .cd-breadcrumb li.current a::before {
                            background: var(--accent);
                            color: white;
                        }

                        .progress-nav .cd-breadcrumb li.current a::after {
                            color: var(--accent);
                            font-weight: 600;
                        }

                    .progress-nav .cd-breadcrumb li.completed a {
                        border-color: var(--success);
                    }

                        .progress-nav .cd-breadcrumb li.completed a::before {
                            background: var(--success);
                            color: white;
                            content: '✓';
                            font-size: 1rem;
                        }

                    .progress-nav .cd-breadcrumb li:hover:not(.current) a {
                        transform: scale(1.05);
                        border-color: var(--accent-light);
                    }

                        .progress-nav .cd-breadcrumb li:hover:not(.current) a::before {
                            background: var(--accent-light);
                            color: white;
                        }

        /* Education specific sections */
        .education-section {
            background: white;
            border-radius: var(--radius-lg);
            padding: 2rem;
            margin-bottom: 2rem;
            box-shadow: var(--shadow-md);
            border: 1px solid rgba(59, 130, 246, 0.1);
            position: relative;
            overflow: hidden;
        }

            .education-section::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                height: 4px;
                background: linear-gradient(90deg, var(--accent), var(--secondary));
                border-radius: var(--radius-lg) var(--radius-lg) 0 0;
            }

            .education-section h4 {
                color: var(--primary);
                margin-bottom: 1.5rem;
                font-weight: 600;
                font-size: 1.25rem;
                display: flex;
                align-items: center;
                gap: 0.75rem;
            }

                .education-section h4::before {
                    content: '';
                    width: 40px;
                    height: 40px;
                    background: linear-gradient(135deg, var(--accent), var(--secondary));
                    border-radius: 50%;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    flex-shrink: 0;
                }

            .education-section.secondary h4::before {
                background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='white' viewBox='0 0 24 24'%3E%3Cpath d='M12 3L1 9l4 2.18v6L12 21l7-3.82v-6l2-1.09V17h2V9L12 3z'/%3E%3C/svg%3E");
                background-size: 20px;
                background-repeat: no-repeat;
                background-position: center;
            }

            .education-section.higher h4::before {
                background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='white' viewBox='0 0 24 24'%3E%3Cpath d='M5 13.18v4L12 21l7-3.82v-4L12 17l-7-3.82zM12 3L1 9l11 6 9-4.909V17h2V9L12 3z'/%3E%3C/svg%3E");
                background-size: 20px;
                background-repeat: no-repeat;
                background-position: center;
            }

            .education-section.bachelor h4::before {
                background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='white' viewBox='0 0 24 24'%3E%3Cpath d='M12 3L1 9l4 2.18v6L12 21l7-3.82v-6l2-1.09V17h2V9L12 3zm2.82 12L12 16.72 9.18 15l1.41-1.41L12 14.41l1.41-.82L15.82 15z'/%3E%3C/svg%3E");
                background-size: 20px;
                background-repeat: no-repeat;
                background-position: center;
            }

            .education-section.masters h4::before {
                background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='white' viewBox='0 0 24 24'%3E%3Cpath d='M12,3L1,9L12,15L21,10.09V17H23V9M5,13.18V17.18L12,21L19,17.18V13.18L12,17L5,13.18Z'/%3E%3C/svg%3E");
                background-size: 20px;
                background-repeat: no-repeat;
                background-position: center;
            }

        .education-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(450px, 1fr));
            gap: 2rem;
            margin-bottom: 2rem;
        }

        .form-table {
            width: 100%;
            border-collapse: separate;
            border-spacing: 0;
        }

            .form-table tr {
                transition: var(--transition);
            }

                .form-table tr:hover {
                    background: rgba(59, 130, 246, 0.02);
                }

            .form-table td {
                padding: 0.75rem;
                border-bottom: 1px solid rgba(226, 232, 240, 0.5);
                vertical-align: middle;
            }

                .form-table td:first-child {
                    font-weight: 500;
                    color: var(--dark);
                    width: 40%;
                }

        .form-control {
            width: 100%;
            padding: 0.75rem;
            border: 2px solid #e2e8f0;
            border-radius: var(--radius-md);
            font-size: 0.95rem;
            transition: var(--transition);
            background: white;
        }

            .form-control:focus {
                outline: none;
                border-color: var(--accent);
                box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
                transform: translateY(-1px);
            }

            .form-control:disabled {
                background: #f8fafc;
                color: var(--gray);
            }

        .btn {
            padding: 0.75rem 1.5rem;
            border: none;
            border-radius: var(--radius-md);
            font-weight: 600;
            font-size: 0.95rem;
            cursor: pointer;
            transition: var(--transition);
            position: relative;
            overflow: hidden;
            display: inline-flex;
            align-items: center;
            gap: 0.5rem;
        }

            .btn::before {
                content: '';
                position: absolute;
                top: 0;
                left: -100%;
                width: 100%;
                height: 100%;
                background: rgba(255, 255, 255, 0.2);
                transition: var(--transition);
            }

            .btn:hover::before {
                left: 100%;
            }

        .btn-primary {
            background: linear-gradient(145deg, var(--accent), var(--secondary));
            color: white;
            box-shadow: var(--shadow-sm);
        }

            .btn-primary:hover {
                transform: translateY(-2px);
                box-shadow: var(--shadow-md);
            }

        .btn-info {
            background: linear-gradient(145deg, #0ea5e9, #06b6d4);
            color: white;
        }

            .btn-info:hover {
                transform: translateY(-1px);
                box-shadow: 0 4px 12px rgba(14, 165, 233, 0.3);
            }

        .btn-success {
            background: linear-gradient(145deg, var(--success), #10b981);
            color: white;
        }

            .btn-success:hover {
                transform: translateY(-1px);
                box-shadow: 0 4px 12px rgba(34, 197, 94, 0.3);
            }

        .btn-danger {
            background: linear-gradient(145deg, var(--danger), #dc2626);
            color: white;
        }

            .btn-danger:hover {
                transform: translateY(-1px);
                box-shadow: 0 4px 12px rgba(239, 68, 68, 0.3);
            }

        .spanAsterisk {
            color: red;
            font-weight: bold;
            margin-left: 3px;
        }

        .validationErrorMsg {
            color: var(--danger);
            font-size: 0.9rem;
            display: block;
            margin-top: 0.5rem;
        }

        /* Modal Styling */
        .modalBackground {
            background-color: rgba(0, 0, 0, 0.6);
            filter: alpha(opacity=60);
            opacity: 0.6;
            z-index: 10000;
            backdrop-filter: blur(5px);
        }

        .modal-panel {
            background: white;
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-xl);
            border: none;
            overflow: hidden;
        }

            .modal-panel .panel-body {
                padding: 2rem;
            }

        .modal-header {
            background: linear-gradient(145deg, var(--accent), var(--secondary));
            color: white;
            padding: 1.5rem 2rem;
            font-weight: 600;
            font-size: 1.1rem;
            text-align: center;
        }

        .table-bordered {
            border: none;
            border-radius: var(--radius-md);
            overflow: hidden;
            box-shadow: var(--shadow-sm);
        }

        .table thead th {
            background: linear-gradient(145deg, var(--primary), var(--secondary));
            color: white;
            border: none;
            padding: 1rem;
            font-weight: 600;
        }

        .table tbody td {
            border: none;
            border-bottom: 1px solid #e2e8f0;
            padding: 1rem;
            vertical-align: middle;
        }

        .table tbody tr:hover {
            background: var(--light);
        }

        /* Loading Animation */
        #divProgress {
            display: none;
            z-index: 1000000;
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background: rgba(255, 255, 255, 0.95);
            border-radius: var(--radius-lg);
            padding: 2rem;
            box-shadow: var(--shadow-xl);
            backdrop-filter: blur(10px);
        }

        /* Action buttons container */
        .action-buttons {
            text-align: center;
            padding: 2rem;
            background: white;
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-md);
            margin-top: 2rem;
        }

        /* Animation for form elements */
        .education-section {
            animation: fadeInUp 0.6s ease forwards;
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

        /* Responsive adjustments */
        @media (max-width: 768px) {
            .education-grid {
                grid-template-columns: 1fr;
                gap: 1rem;
            }

            .education-section {
                padding: 1.5rem;
            }

            .form-table td {
                padding: 0.5rem;
                display: block;
                width: 100% !important;
            }

                .form-table td:first-child {
                    width: 100% !important;
                    font-weight: 600;
                    border-bottom: none;
                    padding-bottom: 0.25rem;
                }

            .progress-nav {
                padding: 1rem;
                overflow-x: auto;
            }

                .progress-nav .cd-breadcrumb {
                    flex-wrap: nowrap;
                    gap: 0.5rem;
                    min-width: max-content;
                }

                    .progress-nav .cd-breadcrumb li {
                        min-width: 50px;
                        min-height: 50px;
                    }

                        .progress-nav .cd-breadcrumb li a {
                            width: 50px;
                            height: 50px;
                        }

                            .progress-nav .cd-breadcrumb li a::before {
                                width: 25px;
                                height: 25px;
                                font-size: 0.75rem;
                            }

                            .progress-nav .cd-breadcrumb li a::after {
                                font-size: 0.7rem;
                                max-width: 60px;
                            }
        }

        /* Helper message styling */
        .helper-message {
            background: linear-gradient(145deg, #dbeafe, #bfdbfe);
            color: var(--primary);
            padding: 1rem;
            border-radius: var(--radius-md);
            border-left: 4px solid var(--accent);
            margin-bottom: 1.5rem;
            font-size: 0.9rem;
        }

        /* Small text styling */
        .education-section small {
            color: var(--gray);
            font-style: italic;
            display: block;
            margin-top: 0.25rem;
        }
    </style>

    <script type="text/javascript">
        function InProgress() {
            var panelProg = $get('divProgress');
            panelProg.style.display = '';
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divProgress" style="display: none;">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="150px" Width="150px" />
        <div style="text-align: center; margin-top: 1rem; color: var(--primary); font-weight: 600;">
            Processing your information...
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="application-container">

                <!-- Progress Navigation -->
                <div class="progress-nav">
                    <!-- Bachelor's Breadcrumb -->
                    <div id="bachelorsBreadcrumb" runat="server">
                        <nav>
                            <ol class="cd-breadcrumb">
                                <li>
                                    <a href="ApplicationBasic.aspx" data-step="1" data-label="Basic" title="Basic"></a>
                                </li>
                                <li class="current">
                                    <a href="#" data-step="2" data-label="Education" title="Education"></a>
                                </li>
                                <li>
                                    <a href="ApplicationPriorityS.aspx" data-step="3" data-label="Priority" title="Priority"></a>
                                </li>
                                <li>
                                    <a href="ApplicationRelation.aspx" data-step="4" data-label="Parent/Guardian" title="Parent/Guardian"></a>
                                </li>
                                <li>
                                    <a href="ApplicationAddress.aspx" data-step="5" data-label="Address" title="Address"></a>
                                </li>
                                <li>
                                    <a href="ApplicationAttachment.aspx" data-step="6" data-label="Photo" title="Photo"></a>
                                </li>
                                <li>
                                    <a href="ApplicationDeclaration.aspx" data-step="7" data-label="Declaration" title="Declaration"></a>
                                </li>
                            </ol>
                        </nav>
                    </div>

                    <!-- Master's Breadcrumb -->
                    <div id="mastersBreadcrumb" runat="server">
                        <nav>
                            <ol class="cd-breadcrumb">
                                <li>
                                    <a href="ApplicationBasic.aspx" data-step="1" data-label="Basic Masters" title="Basic Masters"></a>
                                </li>
                                <li class="current">
                                    <a href="#" data-step="2" data-label="Education" title="Education"></a>
                                </li>
                                <li>
                                    <a href="ApplicationRelation.aspx" data-step="3" data-label="Parent/Guardian" title="Parent/Guardian"></a>
                                </li>
                                <li>
                                    <a href="ApplicationAddress.aspx" data-step="4" data-label="Address" title="Address"></a>
                                </li>
                                <li>
                                    <a href="ApplicationAdditional.aspx" data-step="5" data-label="Additional Info" title="Additional Info"></a>
                                </li>
                                <li>
                                    <a href="ApplicationAttachment.aspx" data-step="6" data-label="Photo" title="Photo"></a>
                                </li>
                                <li>
                                    <a href="ApplicationDeclaration.aspx" data-step="7" data-label="Declaration" title="Declaration"></a>
                                </li>
                            </ol>
                        </nav>
                    </div>
                </div>

                <asp:HiddenField ID="hfEduCat" runat="server" />

                <asp:UpdatePanel ID="updatePanelEducation" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <!-- Secondary and Higher Secondary Education -->
                        <div class="education-grid">
                            <!-- Secondary Education -->
                            <div class="education-section secondary">
                                <h4>
                                    <i class="fas fa-graduation-cap"></i>
                                    Secondary / O-Level / Equivalent
                                </h4>

                                <table class="form-table">
                                    <tr>
                                        <td>Exam Type <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlSec_ExamType" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Education Board <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlSec_EducationBrd" runat="server" CssClass="form-control"></asp:DropDownList>
                                            <asp:TextBox ID="txtSec_EducationBrd" runat="server" Visible="false" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Institute <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtSec_Institute" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Roll Number <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtSec_RollNo" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td>Registration Number</td>
                                        <td>
                                            <asp:TextBox ID="txtSec_RegNo" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trGroupOrSubjectSSC" runat="server">
                                        <td>Group Or Subject <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlSec_GrpOrSub" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trDivisionClassSSC" runat="server">
                                        <td>Division/Class <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlSec_DivClass" runat="server" CssClass="form-control"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlSec_DivClass_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>GPA <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtSec_CgpaScore" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            <asp:RangeValidator ID="txtSec_CgpaScore_RV" runat="server" ControlToValidate="txtSec_CgpaScore"
                                                ErrorMessage="GPA must be between 1.0 - 5.0" ForeColor="Crimson" Display="Dynamic"
                                                ValidationGroup="education1" MaximumValue="5" MinimumValue="0" Type="Double"></asp:RangeValidator>
                                        </td>
                                    </tr>
                                    <tr id="trGPA4thSubjectSSC" runat="server">
                                        <td>Biology GPA <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtSec_CgpaW4S" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RangeValidator ID="txtSec_CgpaW4S_RV" runat="server" ControlToValidate="txtSec_CgpaW4S"
                                                ErrorMessage="GPA (without 4th subject) must be between 1.0 - 5.0" ForeColor="Crimson" Display="Dynamic"
                                                ValidationGroup="education1" MaximumValue="5" MinimumValue="0" Type="Double"></asp:RangeValidator>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Exam Year <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlSec_PassingYear" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Student Category <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlSec_StudentCat" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Select" Value="0" Selected="True" />
                                                <asp:ListItem Text="Regular" Value="1" />
                                                <asp:ListItem Text="Improvement" Value="2" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                </table>

                                <div class="education-section secondary mt-5" runat="server" id="divSubjectWiseGradeOLevel">

                                    <asp:Panel runat="server" ID="Panel2" CssClass="modal-panel"
                                        Style="width: 100%; max-height: 80vh; overflow-y: auto;">

                                        <div class="modal-header" style="text-align: center; background-color: lightgray !important">
                                            <strong>Subject wise result entry (O-Level) &nbsp; <span class="text-danger">*</span> </strong>
                                        </div>

                                        <div class="panel-body">
                                            <div style="margin-bottom: 1.5rem;">
                                                <div style="display: flex; align-items: end; gap: 1rem; margin-bottom: 1rem;">
                                                    <div style="flex: 1;">
                                                        <label style="display: block; margin-bottom: 0.5rem; font-weight: 600;">Number of subjects appeared</label>
                                                        <asp:TextBox ID="txtOlevelSubjetNo" runat="server" CssClass="form-control" Text="5" TextMode="Number" min="5" placeholder="minimum 5 subjects"></asp:TextBox>
                                                    </div>
                                                    <div>
                                                        <asp:LinkButton ID="lnkGenerateOlevel" runat="server" OnClick="lnkGenerateOlevel_Click"
                                                            CssClass="btn btn-info">
                                                        <i class="fas fa-plus"></i> Generate
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>

                                            <asp:Panel ID="Panel1" runat="server">
                                                <asp:GridView runat="server" ID="gvOLevelSubjectResult" AutoGenerateColumns="False"
                                                    AllowPaging="false" CellPadding="4" Width="100%"
                                                    ShowHeader="true" GridLines="None">

                                                    <HeaderStyle BackColor="#0D2D62" ForeColor="White" />
                                                    <RowStyle BackColor="#ecf0f0" />
                                                    <AlternatingRowStyle BackColor="#ffffff" />

                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SL#">
                                                            <ItemTemplate>
                                                                <b><%# Container.DataItemIndex + 1 %></b>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
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
                                                                <asp:TextBox ID="txtOLevelMark" runat="server" TextMode="Number" min="1" CssClass="form-control"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="left" Width="30%" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <div style="text-align: center">

                                                                    <span style="font-weight: 600;">Obtained Grade</span>
                                                                </div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlOLevelGrade" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="A*" Value="95"></asp:ListItem>
                                                                    <asp:ListItem Text="A" Value="85"></asp:ListItem>
                                                                    <asp:ListItem Text="B" Value="75"></asp:ListItem>
                                                                    <asp:ListItem Text="C" Value="65"></asp:ListItem>
                                                                    <asp:ListItem Text="D" Value="55"></asp:ListItem>
                                                                    <asp:ListItem Text="E" Value="45"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="left" Width="25%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>

                                                <div class="row" style="float: right" runat="server" id="divCalculateOLevel">
                                                    <div class="col=lg-6 col-md-6 col-sm-6">
                                                        <asp:LinkButton ID="lnkOLevelSave" runat="server" OnClick="lnkOLevelSave_Click"
                                                            CssClass="btn btn-success" Style="margin-bottom: 0.5rem;">
                                                            <i class="fas fa-calculator"></i> Calculate
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>

                                            </asp:Panel>

                                        </div>
                                    </asp:Panel>
                                </div>

                                <div class="row">
                                    <table class="form-table" runat="server">
                                        <tr id="trMarksForOLevelSSC" runat="server">
                                            <td>Total Obtained Marks<asp:Label ID="lblSSCTotalMark" runat="server" Text="  *" ForeColor="Red" Font-Bold="true"></asp:Label></td>
                                            <td>
                                                <asp:TextBox ID="txtSec_Marks" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trOutofMarksForOLevelSSC" runat="server">
                                            <td>Out of<asp:Label ID="Label2" runat="server" Text="  *" ForeColor="Red" Font-Bold="true"></asp:Label></td>
                                            <td>
                                                <asp:TextBox ID="txtOutofSec_Marks" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <div runat="server" visible="false">
                                        <asp:LinkButton ID="lnkAddResult" runat="server" OnClick="lnkAddResult_Click" CssClass="btn btn-info" Style="margin-top: 0.5rem;">
                                    <i class="fas fa-calculator"></i> Calculate Marks
                                        </asp:LinkButton>
                                    </div>

                                </div>

                            </div>



                            <!-- Higher Secondary Education -->
                            <div class="education-section higher">
                                <h4>
                                    <i class="fas fa-user-graduate"></i>
                                    Higher Secondary / A-Level / Equivalent
                                </h4>

                                <table class="form-table">
                                    <tr>
                                        <td>Exam Type <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlHigherSec_ExamType" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Education Board <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlHigherSec_EducationBrd" runat="server" CssClass="form-control"></asp:DropDownList>
                                            <asp:TextBox ID="txtHigherSec_EducationBrd" runat="server" Visible="false" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Institute <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtHigherSec_Institute" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Roll Number <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtHigherSec_RollNo" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td>Registration Number</td>
                                        <td>
                                            <asp:TextBox ID="txtHigherSec_RegNo" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="trGroupOrSubjectHSC" runat="server">
                                        <td>Group Or Subject <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlHigherSec_GrpOrSub" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trDivisionClassHSC" runat="server">
                                        <td>Division/Class <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlHigherSec_DivClass" runat="server" CssClass="form-control"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlHigherSec_DivClass_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>GPA <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtHigherSec_CgpaScore" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            <asp:RangeValidator ID="txtHigherSec_CgpaScore_RV" runat="server" ControlToValidate="txtHigherSec_CgpaScore"
                                                ErrorMessage="GPA must be between 1.0 - 5.0" ForeColor="Crimson" Display="Dynamic"
                                                ValidationGroup="education1" MaximumValue="5" MinimumValue="0" Type="Double"></asp:RangeValidator>
                                        </td>
                                    </tr>
                                    <tr id="trGPA4thSubjectHSC" runat="server">
                                        <td>Biology GPA<span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtHigherSec_GpaW4S" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RangeValidator ID="txtHigherSec_GpaW4S_RV" runat="server" ControlToValidate="txtHigherSec_GpaW4S"
                                                ErrorMessage="GPA (without 4th subject) must be between 1.0 - 5.0" ForeColor="Crimson" Display="Dynamic"
                                                ValidationGroup="education1" MaximumValue="5" MinimumValue="0" Type="Double"></asp:RangeValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Exam Year <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlHigherSec_PassingYear" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Student Category <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlhsc_StudentCat" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Select" Value="0" Selected="True" />
                                                <asp:ListItem Text="Regular" Value="1" />
                                                <asp:ListItem Text="Improvement" Value="2" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                </table>

                                <div class="education-section secondary mt-5" runat="server" id="divSubjectWiseGradeALevel">

                                    <asp:Panel runat="server" ID="Panel3" CssClass="modal-panel"
                                        Style="width: 100%; max-height: 80vh; overflow-y: auto;">

                                        <div class="modal-header" style="text-align: center; background-color: lightgray !important">
                                            <strong>Subject wise result entry (A-Level) &nbsp; <span class="text-danger">*</span> </strong>
                                        </div>

                                        <div class="panel-body">
                                            <div style="margin-bottom: 1.5rem;">
                                                <div style="display: flex; align-items: end; gap: 1rem; margin-bottom: 1rem;">
                                                    <div style="flex: 1;">
                                                        <label style="display: block; margin-bottom: 0.5rem; font-weight: 600;">Number of subjects appeared</label>
                                                        <asp:TextBox ID="txtAlevelSubjetNo" runat="server" CssClass="form-control" Text="2" TextMode="Number" min="2" placeholder="minimum 2 subjects"></asp:TextBox>
                                                    </div>
                                                    <div>
                                                        <asp:LinkButton ID="lnkGenerateAlevel" runat="server" OnClick="lnkGenerateAlevel_Click"
                                                            CssClass="btn btn-info">
                                    <i class="fas fa-plus"></i> Generate
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>

                                            <asp:Panel ID="Panel4" runat="server">
                                                <asp:GridView runat="server" ID="gvALevelSubjectResult" AutoGenerateColumns="False"
                                                    AllowPaging="false" CellPadding="4" Width="100%"
                                                    ShowHeader="true" GridLines="None">

                                                    <HeaderStyle BackColor="#0D2D62" ForeColor="White" />
                                                    <RowStyle BackColor="#ecf0f0" />
                                                    <AlternatingRowStyle BackColor="#ffffff" />

                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SL#">
                                                            <ItemTemplate>
                                                                <b><%# Container.DataItemIndex + 1 %></b>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
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
                                                                <asp:TextBox ID="txtALevelMark" runat="server" TextMode="Number" min="1" CssClass="form-control"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="left" Width="30%" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <div style="text-align: center">

                                                                    <span style="font-weight: 600;">Obtained Grade</span>
                                                                </div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlALevelGrade" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="A*" Value="95"></asp:ListItem>
                                                                    <asp:ListItem Text="A" Value="85"></asp:ListItem>
                                                                    <asp:ListItem Text="B" Value="75"></asp:ListItem>
                                                                    <asp:ListItem Text="C" Value="65"></asp:ListItem>
                                                                    <asp:ListItem Text="D" Value="55"></asp:ListItem>
                                                                    <asp:ListItem Text="E" Value="45"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="left" Width="25%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>

                                                <div class="row" style="float: right" runat="server" id="divCalculateALevel">
                                                    <div class="col=lg-6 col-md-6 col-sm-6">
                                                        <asp:LinkButton ID="lnkALevelSave" runat="server" OnClick="lnkALevelSave_Click"
                                                            CssClass="btn btn-success" Style="margin-bottom: 0.5rem;">
<i class="fas fa-calculator"></i> Calculate
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>

                                            </asp:Panel>

                                        </div>
                                    </asp:Panel>

                                </div>

                                <div>
                                    <table class="form-table" runat="server">

                                        <tr id="trMarksForOLevelHSC" runat="server">
                                            <td>Total Obtained Marks<asp:Label ID="lblHSCTotalMark" runat="server" Text="  *" ForeColor="Red" Font-Bold="true"></asp:Label></td>
                                            <td>
                                                <asp:TextBox ID="txtHigherSec_Marks" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trOutofMarksForOLevelHSC" runat="server">
                                            <td>Out of<asp:Label ID="Label1" runat="server" Text="  *" ForeColor="Red" Font-Bold="true"></asp:Label></td>
                                            <td>
                                                <asp:TextBox ID="txtOutofHigherSec_Marks" runat="server" CssClass="form-control"></asp:TextBox>

                                            </td>
                                        </tr>

                                    </table>
                                    <div runat="server" visible="false">
                                        <asp:LinkButton ID="lnkAddALevelResult" runat="server" OnClick="lnkAddALevelResult_Click" CssClass="btn btn-info" Style="margin-top: 0.5rem;">
                                            <i class="fas fa-calculator"></i> Calculate Marks
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Bachelor's and Master's Education (for Masters candidates) -->
                        <asp:Panel ID="panel_isUndergrad" runat="server">
                            <div class="education-grid">
                                <!-- Bachelor's Education -->
                                <div class="education-section bachelor">
                                    <h4>
                                        <i class="fas fa-university"></i>
                                        Bachelor
                                        <small>(Only for Candidates applying for Masters Program)</small>
                                    </h4>

                                    <table class="form-table">
                                        <tr>
                                            <td>Institute <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:TextBox ID="txtUndergrad_Institute" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Program/Degree <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlUndergrad_ProgramDegree" runat="server" CssClass="form-control"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlUndergrad_ProgramDegree_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Others <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:TextBox ID="txtUndergrad_ProgOthers" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Division/Class <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlUndergrad_DivClass" runat="server" CssClass="form-control"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlUndergrad_DivClass_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>CGPA/Score <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:TextBox ID="txtUndergrad_CgpaScore" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                <asp:RangeValidator ID="txtUndergrad_CgpaScore_RV" runat="server" ControlToValidate="txtUndergrad_CgpaScore"
                                                    ErrorMessage="GPA must be between 1.0 - 4.0" ForeColor="Crimson" Display="Dynamic"
                                                    ValidationGroup="education1" MaximumValue="4" MinimumValue="0" Type="Double"></asp:RangeValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Exam Year <span class="spanAsterisk">*</span></td>
                                            <td>
                                                <asp:DropDownList ID="ddlUndergrad_PassingYear" runat="server" CssClass="form-control"></asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                                <!-- Master's Education -->
                                <div class="education-section masters">
                                    <h4>
                                        <i class="fas fa-award"></i>
                                        Masters
                                        <small>(Only for Candidates applying for Masters Program)</small>
                                    </h4>

                                    <table class="form-table">
                                        <tr>
                                            <td>Institute</td>
                                            <td>
                                                <asp:TextBox ID="txtGraduate_Institute" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Program/Degree</td>
                                            <td>
                                                <asp:DropDownList ID="ddlGraduate_ProgramDegree" runat="server" CssClass="form-control"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlGraduate_ProgramDegree_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Others</td>
                                            <td>
                                                <asp:TextBox ID="txtGraduate_ProgOthers" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Division/Class</td>
                                            <td>
                                                <asp:DropDownList ID="ddlGraduate_DivClass" runat="server" CssClass="form-control"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlGraduate_DivClass_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>CGPA/Score</td>
                                            <td>
                                                <asp:TextBox ID="txtGraduate_CgpaScore" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                <asp:RangeValidator ID="txtGraduate_CgpaScore_RV" runat="server" ControlToValidate="txtGraduate_CgpaScore"
                                                    MinimumValue="1" MaximumValue="4" Display="Dynamic" ErrorMessage="Invalid number"
                                                    ForeColor="Crimson" Type="Double" ValidationGroup="education1"></asp:RangeValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Exam Year</td>
                                            <td>
                                                <asp:DropDownList ID="ddlGraduate_PassingYear" runat="server" CssClass="form-control"></asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>

                        <!-- Message Panel -->
                        <asp:Panel ID="messagePanel_Education" runat="server" Visible="false">
                            <div class="helper-message">
                                <asp:Label ID="lblMessageEducation" runat="server" Text=""></asp:Label>
                            </div>
                        </asp:Panel>

                        <!-- Action Buttons -->
                        <div class="action-buttons">
                            <asp:Button ID="btnSave_Education" runat="server" Text="Save & Next"
                                CssClass="btn btn-primary" OnClick="btnSave_Education_Click"
                                OnClientClick="return validateEducationForm();"
                                ValidationGroup="education1" Style="margin-right: 1rem;"></asp:Button>

                            <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                                CssClass="btn btn-primary" OnClick="btnNext_Click"></asp:Button>

                            <span id="validationMsg" class="validationErrorMsg"></span>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave_Education" />
        </Triggers>
    </asp:UpdatePanel>

    <!-- Modal Popups for Subject-wise Results -->


    <!-- Animation Extensions -->
    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1"
        TargetControlID="UpdatePanel2" runat="server">
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
