<%@ Page Title="Application Form - Relation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApplicationRelation.aspx.cs" Inherits="Admission.Admission.Candidate.ApplicationRelation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script src="Scripts/RelationInfoValidation.js?v=1"></script>

    <style>
        /* Modern Relation Form Styling */
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

        /* Relation specific sections */
        .relation-section {
            background: white;
            border-radius: var(--radius-lg);
            padding: 2rem;
            margin-bottom: 2rem;
            box-shadow: var(--shadow-md);
            border: 1px solid rgba(59, 130, 246, 0.1);
            position: relative;
            overflow: hidden;
        }

            .relation-section::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                height: 4px;
                background: linear-gradient(90deg, var(--accent), var(--secondary));
                border-radius: var(--radius-lg) var(--radius-lg) 0 0;
            }

            .relation-section h4 {
                color: var(--primary);
                margin-bottom: 1.5rem;
                font-weight: 600;
                font-size: 1.25rem;
                display: flex;
                align-items: center;
                gap: 0.75rem;
            }

                .relation-section h4::before {
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

            .relation-section.father h4::before {
                background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='white' viewBox='0 0 24 24'%3E%3Cpath d='M16 4c0-1.11.89-2 2-2s2 .89 2 2-.89 2-2 2-2-.89-2-2zM20.78 7.58l-1.14-.69L18.8 8.14c-.38.5-.65.98-.84 1.43h-.22L16.8 7.43c-.38-.76-1.16-1.3-2.08-1.3h-1.44c-.92 0-1.7.54-2.08 1.3L9.46 9.57h-.22c-.19-.45-.46-.93-.84-1.43L7.36 6.89l-1.14.69c-.38.23-.62.65-.62 1.09V16h2v5.5h1.5V16h8v5.5H18V16h2V8.67c0-.44-.24-.86-.62-1.09z'/%3E%3C/svg%3E");
                background-size: 20px;
                background-repeat: no-repeat;
                background-position: center;
            }

            .relation-section.mother h4::before {
                background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='white' viewBox='0 0 24 24'%3E%3Cpath d='M13.94 8.31C13.62 7.52 12.85 7 12 7s-1.62.52-1.94 1.31l-1.31 3.25c-.11.27-.38.44-.69.44H7c-.55 0-1 .45-1 1s.45 1 1 1h1.06c.31 0 .58.17.69.44l1.31 3.25c.32.79 1.09 1.31 1.94 1.31s1.62-.52 1.94-1.31l1.31-3.25c.11-.27.38-.44.69-.44H17c.55 0 1-.45 1-1s-.45-1-1-1h-1.06c-.31 0-.58-.17-.69-.44l-1.31-3.25zM12 6c1.1 0 2-.9 2-2s-.9-2-2-2-2 .9-2 2 .9 2 2 2z'/%3E%3C/svg%3E");
                background-size: 20px;
                background-repeat: no-repeat;
                background-position: center;
            }

            .relation-section.guardian h4::before {
                background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='white' viewBox='0 0 24 24'%3E%3Cpath d='M12,1L3,5V11C3,16.55 6.84,21.74 12,23C17.16,21.74 21,16.55 21,11V5L12,1M12,7C13.4,7 14.8,8.6 14.8,10V11.5C14.8,12.3 14.3,13 13.5,13H10.5C9.7,13 9.2,12.3 9.2,11.5V10C9.2,8.6 10.6,7 12,7M10.4,10C10.4,9.2 11.2,8.5 12,8.5C12.8,8.5 13.6,9.2 13.6,10V11.5H10.4V10M12,14C14.1,14 15.8,15.7 15.8,17.8V19H8.2V17.8C8.2,15.7 9.9,14 12,14Z'/%3E%3C/svg%3E");
                background-size: 20px;
                background-repeat: no-repeat;
                background-position: center;
            }

        .relation-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(450px, 1fr));
            gap: 2rem;
            margin-bottom: 2rem;
        }

        .guardian-section {
            grid-column: 1 / -1;
            max-width: 600px;
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
                    width: 35%;
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

        .spanAsterisk {
            color: var(--danger);
            font-weight: bold;
            margin-left: 3px;
        }

        .validationErrorMsg {
            color: var(--danger);
            font-size: 0.9rem;
            display: block;
            margin-top: 0.5rem;
        }

        /* Helper text styling */
        .helper-text {
            color: #ff6c00;
            font-size: 0.8rem;
            font-weight: 600;
            display: block;
            margin-top: 0.25rem;
        }

        .format-hint {
            color: #f59e0b;
            font-size: 0.85rem;
            display: block;
            margin-top: 0.25rem;
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
        .relation-section {
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
            .relation-grid {
                grid-template-columns: 1fr;
                gap: 1rem;
            }

            .relation-section {
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
                                <li>
                                    <a href="ApplicationEducation.aspx" data-step="2" data-label="Education" title="Education"></a>
                                </li>
                                <li>
                                    <a href="ApplicationPriorityS.aspx" data-step="3" data-label="Priority" title="Priority"></a>
                                </li>
                                <li class="current">
                                    <a href="#" data-step="4" data-label="Parent/Guardian" title="Parent/Guardian"></a>
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
                                <li>
                                    <a href="ApplicationEducation.aspx" data-step="2" data-label="Education" title="Education"></a>
                                </li>
                                <li>
                                    <a href="ApplicationPriorityS.aspx" data-step="3" data-label="Priority" title="Priority"></a>
                                </li>
                                <li class="current">
                                    <a href="#" data-step="3" data-label="Parent/Guardian" title="Parent/Guardian"></a>
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

                <asp:UpdatePanel ID="updatePanelParent" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <!-- Father and Mother Information -->
                        <div class="relation-grid">
                            <!-- Father Information -->
                            <div class="relation-section father">
                                <h4>
                                    <i class="fas fa-male"></i>
                                    Father Information
                                </h4>

                                <table class="form-table">
                                    <tr>
                                        <td>Father's Name</td>
                                        <td>
                                            <asp:TextBox ID="txtFatherName" runat="server" CssClass="form-control"></asp:TextBox>
                                            <span class="helper-text">According to SSC / O-Level / Equivalent Certificate</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Late?</td>
                                        <td>
                                            <asp:DropDownList ID="ddlIsLateFather" runat="server" CssClass="form-control" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlIsLateFather_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td>Occupation</td>
                                        <td>
                                            <asp:TextBox ID="txtFatherOccupation" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Occupation</td>
                                        <td>
                                            <asp:DropDownList ID="ddlFatherOccupationType" runat="server" CssClass="form-control"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlFatherOccupationType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="fatherService" runat="server" visible="false">
                                        <td>Service Type <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlFatherServiceType" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="fatherOrganization" runat="server" visible="false">
                                        <td>Organization <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtFatherOrganization" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="fatherDesignation" runat="server" visible="false">
                                        <td>Designation <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtFatherDesignation" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="fatherServiceTxt" runat="server" visible="false">
                                        <td>Previous Occupation Details<br />
                                            <small>(if retired)</small></td>
                                        <td>
                                            <asp:TextBox ID="txtFatherPreviousService" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Mobile <span class="spanAsterisk" id="spanFatherMobile" runat="server">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtFatherMobile" runat="server" CssClass="form-control"
                                                placeholder="Format: +8801XXXXXXXXX"></asp:TextBox>
                                            <span class="format-hint">Please include country code, eg: +8801700000000.</span>
                                            <asp:RegularExpressionValidator runat="server" ID="txtFatherMobileREV"
                                                ValidationGroup="parent1" Font-Size="9pt" ForeColor="Crimson" Display="Dynamic"
                                                ErrorMessage="Invalid format." ControlToValidate="txtFatherMobile"
                                                ValidationExpression="^(\+88)\d{11}$"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Email</td>
                                        <td>
                                            <asp:TextBox ID="txtFatherEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>National ID / Birth Cert No. / Passport No.</td>
                                        <td>
                                            <asp:TextBox ID="txtFatherNationalId" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Nationality</td>
                                        <td>
                                            <asp:DropDownList ID="ddlFatherNationality" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <!-- Mother Information -->
                            <div class="relation-section mother">
                                <h4>
                                    <i class="fas fa-female"></i>
                                    Mother Information
                                </h4>

                                <table class="form-table">
                                    <tr>
                                        <td>Mother's Name</td>
                                        <td>
                                            <asp:TextBox ID="txtMotherName" runat="server" CssClass="form-control"></asp:TextBox>
                                            <span class="helper-text">According to SSC / O-Level / Equivalent Certificate</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Late?</td>
                                        <td>
                                            <asp:DropDownList ID="ddlIsLateMother" runat="server" CssClass="form-control" AutoPostBack="true"
                                                OnSelectedIndexChanged="ddlIsLateMother_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td>Occupation</td>
                                        <td>
                                            <asp:TextBox ID="txtMotherOccupation" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Occupation</td>
                                        <td>
                                            <asp:DropDownList ID="ddlMotherOccupationType" runat="server" CssClass="form-control"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlMotherOccupationType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="motherService" runat="server" visible="false">
                                        <td>Service Type <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlMotherServiceType" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="motherOrganization" runat="server" visible="false">
                                        <td>Organization <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtMotherOrganization" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="motherDesignation" runat="server" visible="false">
                                        <td>Designation <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtMotherDesignation" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr id="motherServiceTxt" runat="server" visible="false">
                                        <td>Previous Occupation Details<br />
                                            <small>(if retired)</small></td>
                                        <td>
                                            <asp:TextBox ID="txtMotherPreviousService" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Mobile <span class="spanAsterisk" id="spanMotherMobile" runat="server">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtMotherMobile" runat="server" CssClass="form-control"
                                                placeholder="Format: +8801XXXXXXXXX"></asp:TextBox>
                                            <span class="format-hint">Please include country code, eg: +8801700000000.</span>
                                            <asp:RegularExpressionValidator runat="server" ID="txtMotherMobileREV"
                                                ValidationGroup="parent1" Font-Size="9pt" ForeColor="Crimson" Display="Dynamic"
                                                ErrorMessage="Invalid format." ControlToValidate="txtMotherMobile"
                                                ValidationExpression="^(\+88)\d{11}$"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>National ID / Birth Cert No. / Passport No.</td>
                                        <td>
                                            <asp:TextBox ID="txtMotherNationalId" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Nationality</td>
                                        <td>
                                            <asp:DropDownList ID="ddlMotherNationality" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                        <!-- Guardian Information -->
                        <div class="guardian-section">
                            <div class="relation-section guardian">
                                <h4>
                                    <i class="fas fa-shield-alt"></i>
                                    Guardian Information
                                </h4>

                                <asp:UpdatePanel ID="updatePanel_Guardian" runat="server">
                                    <ContentTemplate>
                                        <table class="form-table">
                                            <tr>
                                                <td>Relationship with the applicant <span class="spanAsterisk">*</span></td>
                                                <td>
                                                    <asp:DropDownList ID="ddlGuardianRelation" runat="server" CssClass="form-control"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlGuardianRelation_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Guardian's Name <span class="spanAsterisk">*</span></td>
                                                <td>
                                                    <asp:TextBox ID="txtGuardian_Name" runat="server" CssClass="form-control"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Mention relationship<br />
                                                    <small>(If guardian is not Father/Mother)</small></td>
                                                <td>
                                                    <asp:TextBox ID="txtGuardianOtherRelation" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr style="display: none">
                                                <td>Occupation</td>
                                                <td>
                                                    <asp:TextBox ID="txtGuardianOccupation" runat="server" CssClass="form-control"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Occupation</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlGuardianOccupationType" runat="server" CssClass="form-control"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Organization</td>
                                                <td>
                                                    <asp:TextBox ID="txtGuardianOrganization" runat="server" CssClass="form-control"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Address <span class="spanAsterisk">*</span></td>
                                                <td>
                                                    <asp:TextBox ID="txtGuardianMailingAddress" runat="server" CssClass="form-control"
                                                        TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Email</td>
                                                <td>
                                                    <asp:TextBox ID="txtGuardianEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Mobile <span class="spanAsterisk">*</span></td>
                                                <td>
                                                    <asp:TextBox ID="txtGuardianMobile" runat="server" CssClass="form-control"
                                                        placeholder="Format: +8801XXXXXXXXX"></asp:TextBox>
                                                    <span class="format-hint">Please include country code, eg: +8801700000000.</span>
                                                    <asp:RegularExpressionValidator runat="server" ID="guardianMobileReg"
                                                        ValidationGroup="parent1" Font-Size="9pt" ForeColor="Crimson" Display="Dynamic"
                                                        ErrorMessage="Invalid format." ControlToValidate="txtGuardianMobile"
                                                        ValidationExpression="^(\+88)\d{11}$"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>National ID / Birth Cert No. / Passport No.</td>
                                                <td>
                                                    <asp:TextBox ID="txtGuardianNationalId" runat="server" CssClass="form-control"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Nationality</td>
                                                <td>
                                                    <asp:DropDownList ID="ddlGuardianNationality" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <!-- Message Panel -->
                        <asp:Panel ID="messagePanel_Parent" runat="server" Visible="false">
                            <div class="helper-message">
                                <asp:Label ID="lblMessageParent" runat="server" Text=""></asp:Label>
                            </div>
                        </asp:Panel>

                        <!-- Action Buttons -->
                        <div class="action-buttons">
                            <asp:Button ID="btnSave_Parent" runat="server" Text="Save & Next"
                                OnClientClick="return validateRelationInfo();" CssClass="btn btn-primary"
                                OnClick="btnSave_Parent_Click" ValidationGroup="parent1" Style="margin-right: 1rem;"></asp:Button>

                            <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                                CssClass="btn btn-primary" OnClick="btnNext_Click"></asp:Button>

                            <span id="validationMsg" class="validationErrorMsg"></span>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave_Parent" />
        </Triggers>
    </asp:UpdatePanel>

    <!-- Animation Extensions -->
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        </ContentTemplate>
    </asp:UpdatePanel>

    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1"
        TargetControlID="UpdatePanel1" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnSave_Parent" Enabled="false" />
                    <EnableAction AnimationTarget="btnNext" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction AnimationTarget="btnSave_Parent" Enabled="true" />
                    <EnableAction AnimationTarget="btnNext" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
