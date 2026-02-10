<%@ Page Title="Application Form - Basic" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApplicationBasic.aspx.cs" Inherits="Admission.Admission.Candidate.ApplicationBasic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script src="Scripts/BasicInfoValidation.js"></script>

    <script type="text/javascript">
        function handleQuotaSelection() {
            var ddlQuota = document.getElementById('<%= ddlQuota.ClientID %>');
            var divQuotaDetails = document.getElementById('<%= divQuotaDetails.ClientID %>');

            if (ddlQuota && divQuotaDetails && window.innerWidth <= 768) {
                if (ddlQuota.selectedIndex === 0) {
                    divQuotaDetails.style.display = 'block';
                } else {
                    divQuotaDetails.style.display = 'none';
                }
            } else if (divQuotaDetails) {
                divQuotaDetails.style.display = 'block';
            }
        }

        window.addEventListener('DOMContentLoaded', handleQuotaSelection);
        window.addEventListener('resize', handleQuotaSelection);

        // For UpdatePanel postbacks
        if (typeof Sys !== 'undefined') {
            Sys.Application.add_load(function () {
                handleQuotaSelection();
            });
        }
    </script>

    <script type="text/javascript">
        function InProgress() {
            var panelProg = $get('divProgress');
            panelProg.style.display = '';
            document.getElementById("blurOverlay").style.display = "block";
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
            document.getElementById("blurOverlay").style.display = "none";
        }
    </script>

    <style>
        /* Modern Application Form Styling */
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

                    .progress-nav .cd-breadcrumb li:hover a::after {
                        color: var(--accent);
                    }

                    .progress-nav .cd-breadcrumb li.current a {
                        color: var(--accent);
                        font-weight: 600;
                    }

                    .progress-nav .cd-breadcrumb li:hover a {
                        color: var(--accent);
                    }

                    /* Step connector animation */
                    .progress-nav .cd-breadcrumb li.current ~ li::after {
                        content: '';
                        position: absolute;
                        top: 50%;
                        left: -50%;
                        width: 100%;
                        height: 3px;
                        background: var(--accent);
                        border-radius: 2px;
                        z-index: -1;
                        animation: progressFlow 1s ease-in-out;
                    }

        @keyframes progressFlow {
            0% {
                width: 0%;
                opacity: 0;
            }

            100% {
                width: 100%;
                opacity: 1;
            }
        }

        /* Pulse animation for current step */
        .progress-nav .cd-breadcrumb li.current::after {
            content: '';
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            width: 100%;
            height: 100%;
            border-radius: 50%;
            border: 2px solid var(--accent);
            animation: pulse 2s infinite;
        }

        @keyframes pulse {
            0% {
                transform: translate(-50%, -50%) scale(1);
                opacity: 1;
            }

            50% {
                transform: translate(-50%, -50%) scale(1.2);
                opacity: 0.5;
            }

            100% {
                transform: translate(-50%, -50%) scale(1);
                opacity: 1;
            }
        }

        .form-section {
            background: white;
            border-radius: var(--radius-lg);
            padding: 2rem;
            margin-bottom: 2rem;
            box-shadow: var(--shadow-md);
            border: 1px solid rgba(59, 130, 246, 0.1);
        }

            .form-section h4 {
                color: var(--primary);
                margin-bottom: 1.5rem;
                font-weight: 600;
                font-size: 1.25rem;
            }

        .quota-info-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 1rem;
            margin-bottom: 2rem;
        }

        .quota-card {
            background: linear-gradient(145deg, #f8fafc, #e2e8f0);
            border-radius: var(--radius-md);
            padding: 1.5rem;
            box-shadow: var(--shadow-sm);
            border: 1px solid rgba(59, 130, 246, 0.1);
        }

            .quota-card h5 {
                background: var(--danger);
                color: white;
                padding: 0.75rem;
                border-radius: var(--radius-sm);
                margin: -0.5rem -0.5rem 1rem -0.5rem;
                text-align: center;
                font-weight: 600;
                font-size: 0.9rem;
            }

            .quota-card ul {
                margin: 0;
                padding-left: 1.25rem;
                color: var(--dark);
            }

            .quota-card li {
                margin-bottom: 0.5rem;
                font-size: 0.85rem;
                line-height: 1.4;
            }

        .form-group {
            margin-bottom: 1.5rem;
        }

            .form-group label {
                display: block;
                margin-bottom: 0.5rem;
                color: var(--dark);
                font-weight: 500;
                font-size: 0.95rem;
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
            }

        .btn {
            padding: 0.75rem 2rem;
            border: none;
            border-radius: var(--radius-md);
            font-weight: 600;
            font-size: 0.95rem;
            cursor: pointer;
            transition: var(--transition);
            position: relative;
            overflow: hidden;
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

        .btn-default {
            background: white;
            color: var(--dark);
            border: 2px solid #e2e8f0;
        }

            .btn-default:hover {
                background: var(--light);
                border-color: var(--accent);
            }

        .btn-danger {
            background: var(--danger);
            color: white;
        }

            .btn-danger:hover {
                background: #dc2626;
                transform: translateY(-1px);
            }

        .alert {
            padding: 1.25rem;
            border-radius: var(--radius-md);
            margin-bottom: 1.5rem;
            border: none;
            position: relative;
        }

        .alert-info {
            background: linear-gradient(145deg, #dbeafe, #bfdbfe);
            color: var(--primary);
            border-left: 4px solid var(--accent);
        }

        .panel {
            background: white;
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-md);
            margin-bottom: 2rem;
            overflow: hidden;
        }

        .panel-heading {
            background: linear-gradient(145deg, var(--primary), var(--secondary));
            color: white;
            padding: 1.25rem;
            font-weight: 600;
            text-align: center;
        }

        .panel-body {
            padding: 2rem;
        }

        .panel-info .panel-heading {
            background: linear-gradient(145deg, var(--accent), var(--secondary));
        }

        .panel-warning .panel-heading {
            background: linear-gradient(145deg, var(--warning), #f59e0b);
        }

        .table {
            width: 100%;
            margin-bottom: 1rem;
            border-collapse: collapse;
            background: white;
            border-radius: var(--radius-md);
            overflow: hidden;
            box-shadow: var(--shadow-sm);
        }

            .table th {
                background: linear-gradient(145deg, var(--primary), var(--secondary));
                color: white;
                padding: 1rem;
                font-weight: 600;
                text-align: left;
            }

            .table td {
                padding: 1rem;
                border-bottom: 1px solid #e2e8f0;
                vertical-align: middle;
            }

            .table tr:hover {
                background: var(--light);
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

        .navbar-toggle {
            display: none;
        }


        /* Mobile Progress Navigation */
        @media (max-width: 768px) {
            .progress-nav {
                padding: 1rem;
                overflow-x: auto; /* Allows horizontal scrolling if needed */
                -webkit-overflow-scrolling: touch; /* Smooth scrolling on iOS */
            }

                .progress-nav .cd-breadcrumb {
                    display: flex;
                    flex-wrap: nowrap; /* Prevent wrapping */
                    gap: 0.5rem;
                    padding-bottom: 1rem; /* Space for scroll if needed */
                    width: max-content; /* Allow content to expand beyond viewport */
                    min-width: 100%; /* Ensure it takes full width */
                }

                    .progress-nav .cd-breadcrumb::before {
                        display: none;
                    }

                    .progress-nav .cd-breadcrumb li {
                        min-width: 50px;
                        min-height: 50px;
                        flex-shrink: 0; /* Prevent items from shrinking */
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
                                white-space: normal; /* Allow text to wrap */
                                text-align: center;
                                top: calc(100% + 0.5rem);
                            }

                        /* Remove the vertical connector lines */
                        .progress-nav .cd-breadcrumb li::after {
                            display: none;
                        }

                /* Horizontal scrollbar styling */
                .progress-nav::-webkit-scrollbar {
                    height: 5px;
                }

                .progress-nav::-webkit-scrollbar-track {
                    background: #f1f1f1;
                }

                .progress-nav::-webkit-scrollbar-thumb {
                    background: var(--accent);
                    border-radius: 10px;
                }
        }

        /* Animation for form elements */
        .form-group {
            animation: fadeInUp 0.5s ease forwards;
        }

        @keyframes fadeInUp {
            from {
                opacity: 0;
                transform: translateY(20px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        /* Custom file upload styling */
        input[type="file"] {
            padding: 0.5rem;
            border: 2px dashed #e2e8f0;
            border-radius: var(--radius-md);
            background: var(--light);
            width: 100%;
            transition: var(--transition);
        }

            input[type="file"]:hover {
                border-color: var(--accent);
                background: white;
            }

        /* Grid layout improvements */
        .row {
            display: flex;
            flex-wrap: wrap;
            margin: 0 -0.75rem;
        }

        .col-md-12, .col-md-6, .col-md-4, .col-md-8,
        .col-lg-12, .col-lg-6, .col-lg-4, .col-lg-8,
        .col-sm-12, .col-sm-6, .col-sm-4, .col-sm-8 {
            padding: 0 0.75rem;
        }

        .col-md-12, .col-lg-12, .col-sm-12 {
            flex: 0 0 100%;
            max-width: 100%;
        }

        .col-md-8, .col-lg-8, .col-sm-8 {
            flex: 0 0 66.666667%;
            max-width: 66.666667%;
        }

        .col-md-6, .col-lg-6, .col-sm-6 {
            flex: 0 0 50%;
            max-width: 50%;
        }

        .col-md-4, .col-lg-4, .col-sm-4 {
            flex: 0 0 33.333333%;
            max-width: 33.333333%;
        }

        @media (max-width: 768px) {
            .col-md-12, .col-md-6, .col-md-4, .col-md-8 {
                flex: 0 0 100%;
                max-width: 100%;
            }
        }
    </style>


    <style type="text/css">
        .radio-list input[type="radio"] {
            transform: scale(1.5);
            margin-right: 10px;
            vertical-align: middle;
        }

        .radio-list label {
            font-size: 1.1em;
            display: inline-block;
            vertical-align: middle;
        }

        .radio-list td {
            padding: 6px 0;
        }

        .checkbox input[type="checkbox"] {
            transform: scale(1.5);
            margin-right: 4px;
        }

        .checkbox-list label {
            font-size: 1.2em;
            margin-right: 30px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="blurOverlay" style="display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; backdrop-filter: blur(5px); background-color: rgba(255, 255, 255, 0.3); z-index: 999999;">
    </div>
    <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="250px" Width="250px" />
    </div>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="application-container">

                <!-- Progress Navigation -->
                <div class="progress-nav">
                    <!-- Bachelor's Breadcrumb -->
                    <div id="bachelorsBreadcrumb" runat="server">
                        <nav>
                            <ol class="cd-breadcrumb" id="bachelorNav">
                                <li class="current">
                                    <a href="#" data-step="1" data-label="Basic" title="Basic"></a>
                                </li>
                                <li>
                                    <a href="ApplicationEducation.aspx" data-step="2" data-label="Education" title="Education"></a>
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
                            <ol class="cd-breadcrumb" id="mastersNav">
                                <li class="current">
                                    <a href="#" data-step="1" data-label="Basic Masters"></a>
                                </li>
                                <li>
                                    <a href="ApplicationEducation.aspx" data-step="2" data-label="Education"></a>
                                </li>
                                <li>
                                    <a href="ApplicationRelation.aspx" data-step="3" data-label="Parent/Guardian"></a>
                                </li>
                                <li>
                                    <a href="ApplicationAddress.aspx" data-step="4" data-label="Address"></a>
                                </li>
                                <li>
                                    <a href="ApplicationAdditional.aspx" data-step="5" data-label="Additional Info/Work Experience"></a>
                                </li>
                                <li>
                                    <a href="ApplicationAttachment.aspx" data-step="6" data-label="Upload Photo"></a>
                                </li>
                                <li>
                                    <a href="ApplicationDeclaration.aspx" data-step="7" data-label="Declaration"></a>
                                </li>
                            </ol>
                        </nav>
                    </div>
                </div>

                <!-- Main Form Section -->
                <div class="form-section">
                    <h4><i class="fas fa-info-circle"></i>Basic Information</h4>
                    <p style="color: var(--danger); font-weight: 500;">
                        <i class="fas fa-exclamation-triangle"></i>
                        <span class="spanAsterisk">*</span> indicate required fields.
                    </p>

                    <div class="row">
                        <!-- Personal Information Panel -->
                        <div class="col-lg-6 col-md-6 col-sm-12">
                            <div class="form-group">
                                <label>Religion<span class="spanAsterisk">*</span></label>
                                <asp:DropDownList ID="ddlReligion" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>


                            <div class="form-group">
                                <label>ID Type<span class="spanAsterisk">*</span></label>
                                <asp:DropDownList ID="ddlNationalIdOrBirthRegistration" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlNationalIdOrBirthRegistration_SelectedIndexChanged">
                                    <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                    <asp:ListItem Value="1">NID</asp:ListItem>
                                    <asp:ListItem Value="2">Birth Reg. No.</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="form-group" runat="server" id="divNidBirth">
                                <label>ID Number</label>
                                <asp:TextBox ID="txtNationalIdOrBirthRegistration" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>


                            <div runat="server" visible="false">

                                <div class="form-group" runat="server" id="divQuota">
                                    <label>Quota<span class="spanAsterisk">*</span></label>
                                    <asp:DropDownList ID="ddlQuota" runat="server" CssClass="form-control"
                                        OnSelectedIndexChanged="ddlQuota_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>

                            </div>
                        </div>


                        <div class="col-lg-6 col-md-6 col-sm-12">

                            <div class="form-group">
                                <label>Blood Group<span class="spanAsterisk">*</span></label>
                                <asp:DropDownList ID="ddlBloodGroup" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>


                            <div class="form-group" runat="server" id="divHall">
                                <label>Do you want to avail hall accommodation?<span class="spanAsterisk">*</span></label>
                                <asp:RadioButtonList ID="rblHallAccomodation" runat="server"
                                    CssClass="form-control radio-list" RepeatDirection="Vertical">
                                    <asp:ListItem Value="1">Yes, intersted</asp:ListItem>
                                    <asp:ListItem Value="0">No, not intersted</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>

                        </div>

                        <!-- Quota Information Cards -->
                        <div runat="server" visible="false">
                            <div class="col-lg-8 col-md-6 col-sm-12" runat="server" id="divQuotaDetails">
                                <div class="quota-info-grid">
                                    <div class="quota-card">
                                        <b>Freedom Fighter Quota</b>
                                        <ul>
                                            <li>Children of Freedom Fighter</li>
                                        </ul>
                                    </div>

                                    <div class="quota-card">
                                        <b>Special Quota</b>
                                        <ul>
                                            <li>Children of Armed Forces Personnel (Serving and Retired)</li>
                                            <li>Children of BUP Permanent Teacher, Officers, and Staffs (Serving and Retired)</li>
                                            <li>Children of Sitting members of BUP Governing Bodies (Senate, Syndicate, Academic Council and Finance Committee)</li>
                                        </ul>
                                    </div>

                                    <div class="quota-card">
                                        <b>Ethnic Minority</b>
                                        <ul>
                                            <li>Certificate issued by local Upazilla Nirbahi Officer (UNO)</li>
                                        </ul>
                                    </div>

                                    <div class="quota-card">
                                        <b>Disable</b>
                                        <ul>
                                            <li>Disable</li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Hidden Personal Details (keeping original structure) -->
                <div class="col-md-6" runat="server" visible="false">
                    <div class="form-section">
                        <div class="form-group">
                            <label>Name in FULL<span class="spanAsterisk">*</span></label>
                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label>Date of Birth<span class="spanAsterisk">*</span></label>
                            <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="form-control" placeholder="dd/MM/yyyy"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalenderExtender_DOB" runat="server"
                                TargetControlID="txtDateOfBirth" Format="dd/MM/yyyy" />
                        </div>

                        <div class="form-group">
                            <label>Gender<span class="spanAsterisk">*</span></label>
                            <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="col-md-6" runat="server" visible="false">
                    <div class="form-section">
                        <div class="form-group">
                            <label>Email<span class="spanAsterisk">*</span></label>
                            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label>Mobile<span class="spanAsterisk">*</span></label>
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="Format: +8801XXXXXXXXX"></asp:TextBox>
                            <asp:RegularExpressionValidator runat="server" ID="mobileReg"
                                ValidationGroup="basic1" ForeColor="Crimson" ErrorMessage="Invalid format."
                                ControlToValidate="txtMobile" ValidationExpression="^(\+88)\d{11}$"></asp:RegularExpressionValidator>
                        </div>

                        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                    </div>
                </div>

                <!-- Hidden Fields Section -->
                <div runat="server" id="notRequired" visible="false">
                    <div class="form-section">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Place of Birth<span class="spanAsterisk">*</span></label>
                                    <asp:TextBox ID="txtPlaceOfBirth" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Nationality<span class="spanAsterisk">*</span></label>
                                    <asp:DropDownList ID="ddlNationality" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Mother Tongue<span class="spanAsterisk">*</span></label>
                                    <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Marital Status</label>
                                    <asp:DropDownList ID="ddlMaritalStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Quota Information Panels -->
                <asp:Panel ID="panelQuotaNote" runat="server" Visible="false">
                    <div class="alert alert-info">
                        <strong style="color: var(--danger); font-size: 1.1rem;">
                            <i class="fas fa-exclamation-triangle"></i>Caution!
                        </strong>
                        <p style="color: var(--danger); font-size: 1.1rem; margin-top: 0.5rem;">
                            Your application may be cancelled, if provided information is found wrong.
                        </p>

                        <asp:Panel ID="panelQuotaNoteSpecialQuota" runat="server" Visible="false">
                            <strong>Eligibility:</strong>
                            <ul>
                                <li>Children of Armed Forces Personnel (Serving and Retired)</li>
                                <li>Children of BUP Permanent Teacher, Officers, and Staffs (Serving and Retired)</li>
                                <li>Children of Sitting members of BUP Governing Bodies (Senate, Syndicate, Academic Council and Finance Committee)</li>
                            </ul>
                        </asp:Panel>

                        <asp:Panel ID="panelQuotaNoteFreedomFighter" runat="server" Visible="false">
                            <strong>Eligibility:</strong>
                            <ul>
                                <li>Children of Freedom Fighter</li>
                            </ul>
                        </asp:Panel>

                        <asp:Panel ID="panelQuotaNotePersonWithDisability" runat="server" Visible="false">
                            <strong>Eligibility:</strong>
                            <ul>
                                <li>Disable</li>
                            </ul>
                        </asp:Panel>
                    </div>
                </asp:Panel>

                <!-- Special Quota Panel -->
                <asp:Panel ID="panelQuotaInfo" runat="server" Visible="false">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <i class="fas fa-info-circle"></i>Quota Information
                        </div>
                        <div class="panel-body">
                            <div class="form-group">
                                <label>Type of Special Quota<span class="spanAsterisk">*</span></label>
                                <asp:DropDownList ID="ddlSQQuotaType" runat="server" CssClass="form-control"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlSQQuotaType_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator10" runat="server"
                                    ControlToValidate="ddlSQQuotaType" ErrorMessage="Required" Display="Dynamic"
                                    ForeColor="Crimson" ValueToCompare="-1" Operator="NotEqual" ValidationGroup="basic1">
                                </asp:CompareValidator>
                            </div>

                            <asp:Panel ID="panelChildrenOfMilitaryPersonnel" runat="server" Visible="false">
                                <div class="form-group">
                                    <label>Serving / Retired<span class="spanAsterisk">*</span></label>
                                    <asp:DropDownList ID="rblServingRetired" runat="server" CssClass="form-control"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblServingRetired_SelectedIndexChanged">
                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1">Serving</asp:ListItem>
                                        <asp:ListItem Value="2">Retired</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="panelChildrenOfMilitaryPersonnelServingRetired" runat="server" Visible="false">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>
                                                <asp:Label ID="lblName1" runat="server"></asp:Label>
                                                <span class="spanAsterisk">*</span>
                                            </label>
                                            <asp:TextBox ID="txtInput1" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5"
                                                ValidationGroup="basic1" ControlToValidate="txtInput1" Display="Dynamic"
                                                ForeColor="Crimson" ErrorMessage="Required" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>
                                                <asp:Label ID="lblName2" runat="server"></asp:Label>
                                                <span class="spanAsterisk">*</span>
                                            </label>
                                            <asp:TextBox ID="txtInput2" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6"
                                                ValidationGroup="basic1" ControlToValidate="txtInput2" Display="Dynamic"
                                                ForeColor="Crimson" ErrorMessage="Required" />
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>
                                                Father's/Mother's Name
                                                <asp:Label ID="lblshortnoteFM" runat="server" Style="font-size: 0.8rem; color: var(--warning);"></asp:Label>
                                            </label>
                                            <asp:TextBox ID="txtFatherName" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Father's/Mother's (Rank/Designation)</label>
                                            <asp:TextBox ID="txtFatherRankDesignation" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="panelChildrenOfSittingMembersOfBUPGoverningBodies" runat="server" Visible="false">
                                <div class="form-group">
                                    <label>Committee Member</label>
                                    <asp:DropDownList ID="rblGoverningBodie" runat="server" CssClass="form-control"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblGoverningBodie_SelectedIndexChanged">
                                        <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1">Senate Committee Member</asp:ListItem>
                                        <asp:ListItem Value="2">Syndicate Committee Member</asp:ListItem>
                                        <asp:ListItem Value="3">Academic Council Member</asp:ListItem>
                                        <asp:ListItem Value="4">Finance Committee Member</asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                                <div class="form-group">
                                    <label>Committee Member Name</label>
                                    <asp:DropDownList ID="ddlGoverningBodie" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Freedom Fighter Panel -->
                <asp:Panel ID="panelFreedomFighterInfo" runat="server" Visible="false">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <i class="fas fa-flag"></i>Freedom Fighter Quota Information
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <label>Name of Freedom Fighter<span class="spanAsterisk">*</span></label>
                                        <asp:TextBox ID="txtFFName" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3"
                                            ValidationGroup="basic1" ControlToValidate="txtFFName" Display="Dynamic"
                                            ForeColor="Crimson" ErrorMessage="Required" />
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>Relation With Applicant<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlFFQuotaType" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:CompareValidator ID="CompareValidator3" runat="server"
                                            ControlToValidate="ddlFFQuotaType" ErrorMessage="Required" Display="Dynamic"
                                            ForeColor="Crimson" ValueToCompare="-1" Operator="NotEqual" ValidationGroup="basic1">
                                        </asp:CompareValidator>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>Freedom Fighter No<span class="spanAsterisk">*</span></label>
                                        <asp:TextBox ID="txtFFQFreedomFighterNo" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2"
                                            ValidationGroup="basic1" ControlToValidate="txtFFQFreedomFighterNo" Display="Dynamic"
                                            ForeColor="Crimson" ErrorMessage="Required" />
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <label>Gazette Reference No<span class="spanAsterisk">*</span></label>
                                <asp:TextBox ID="txtFFQGazetteReferenceNo" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4"
                                    ValidationGroup="basic1" ControlToValidate="txtFFQGazetteReferenceNo" Display="Dynamic"
                                    ForeColor="Crimson" ErrorMessage="Required" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Person With Disability Panel -->
                <asp:Panel ID="panelPersonWithDisabilityInfo" runat="server" Visible="false">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <i class="fas fa-universal-access"></i>Person With Disability Information
                        </div>
                        <div class="panel-body">
                            <div class="form-group">
                                <label>Type of Disability<span class="spanAsterisk">*</span></label>
                                <asp:TextBox ID="txtPWDDisabilityName" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator9"
                                    ValidationGroup="basic1" ControlToValidate="txtPWDDisabilityName" Display="Dynamic"
                                    ForeColor="Crimson" ErrorMessage="Required" />
                            </div>

                            <%-- Radio button list for Special assistant required--%>
                            <%--if rblSpecialAssistantRequired selected value is Yes then show checkbox list for special assistant types--%>

                            <div class="form-group">
                                <label>Special Assistance Required?<span class="spanAsterisk">*</span></label>
                                <asp:RadioButtonList ID="rblSpecialAssistantRequired" runat="server"
                                    AutoPostBack="True" CssClass="form-control radio-list" RepeatDirection="Vertical"
                                    OnSelectedIndexChanged="rblSpecialAssistantRequired_SelectedIndexChanged">
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:RadioButtonList>

                            </div>

                            <%--check multiple checkbox i. wheel chair ii. exam hall iii. sruti likhok iv. others text box--%>
                            <div class="form-group" runat="server" id="divDisabledAssistant">
                                <label>Type of Special Assistance Required (Select all that apply):<span class="spanAsterisk">*</span></label>
                                <asp:CheckBoxList ID="cblSpecialAssistantTypes" runat="server" CssClass="form-control checkbox-list" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="cblSpecialAssistantTypes_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </div>

                            <div class="form-group" runat="server" id="divSpecialAssistantOther">
                                <label>If Others, please specify:</label>
                                <asp:TextBox ID="txtOtherSpecialAssistant" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>


                        </div>
                </asp:Panel>

                <!-- Document Upload Panel -->
                <asp:Panel ID="panelQuotaDocUpload" runat="server" Visible="false">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <i class="fas fa-upload"></i>Upload Relevant Certificates/Documents
                        </div>
                        <div class="panel-body">
                            <!-- Upload Notes -->
                            <asp:Panel ID="panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Serving_Note" runat="server" Visible="false">
                                <div class="alert alert-info">
                                    <strong><i class="fas fa-info-circle"></i>Note:</strong>
                                    <ul>
                                        <li>Upload a pdf/image copy of certificate from the Unit/HQ/Area Commander</li>
                                    </ul>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="panel_QuotaDocUpload_ChildrenOfMilitaryPersonnel_Retired_Note" runat="server" Visible="false">
                                <div class="alert alert-info">
                                    <strong><i class="fas fa-info-circle"></i>Note:</strong>
                                    <ul>
                                        <li>Upload a pdf/image copy of certificate provided from CORO/Records</li>
                                    </ul>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Serving_Note" runat="server" Visible="false">
                                <div class="alert alert-info">
                                    <strong><i class="fas fa-info-circle"></i>Note:</strong>
                                    <ul>
                                        <li>Upload a pdf/image copy of certificate signed by concerned Office/Department Head</li>
                                    </ul>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="panel_QuotaDocUpload_ChildrenOfBUPPersonnel_Retired_Note" runat="server" Visible="false">
                                <div class="alert alert-info">
                                    <strong><i class="fas fa-info-circle"></i>Note:</strong>
                                    <ul>
                                        <li>Upload a pdf/image copy of certificate signed by concerned Office/Department Head</li>
                                    </ul>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="panel_QuotaDocUpload_PersonWithDisability_Note" runat="server" Visible="false">
                                <div class="alert alert-info">
                                    <strong><i class="fas fa-info-circle"></i>Note:</strong>
                                    <ul>
                                        <li>Upload medical certificate</li>
                                    </ul>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="panel_QuotaDocUpload_Tribal_Note" runat="server" Visible="false">
                                <div class="alert alert-info">
                                    <strong><i class="fas fa-info-circle"></i>Note:</strong>
                                    <ul>
                                        <li>Certificate issued by local Upazilla Nirbahi Officer (UNO)</li>
                                    </ul>
                                </div>
                            </asp:Panel>

                            <div class="form-group">
                                <label>
                                    <strong><i class="fas fa-file-upload"></i>File Upload:</strong>
                                    <small style="color: var(--danger); font-style: italic;">*Only (.pdf, .jpg, .jpeg, .png)</small>
                                </label>
                                <asp:FileUpload ID="FileUploadDocument" runat="server" />
                            </div>

                            <div class="form-group">
                                <asp:Button ID="btnUploadFile" Text="Upload Document" runat="server"
                                    OnClick="btnUploadFile_Click" CssClass="btn btn-primary"></asp:Button>
                            </div>

                            <%--New Added--%>

                            <asp:Panel ID="panel_QuotaDocUpload_PersonWithDisability_Note2" runat="server" Visible="false">
                                <div class="alert alert-info">
                                    <strong><i class="fas fa-info-circle"></i>Note:</strong>
                                    <ul>
                                        <li>Upload suborno nagorik card</li>
                                    </ul>
                                </div>

                                <div class="form-group">
                                    <label>
                                        <strong><i class="fas fa-file-upload"></i>File Upload:</strong>
                                        <small style="color: var(--danger); font-style: italic;">*Only (.pdf, .jpg, .jpeg, .png)</small>
                                    </label>
                                    <asp:FileUpload ID="FileUploadDocumentSuborno" runat="server" />
                                </div>

                                <div class="form-group">
                                    <asp:Button ID="btnUplaodSuborno" Text="Upload Document" runat="server"
                                        OnClick="btnUplaodSuborno_Click" CssClass="btn btn-primary"></asp:Button>
                                </div>

                            </asp:Panel>

                            <hr style="margin: 2rem 0; border: 1px solid #e2e8f0;" />

                            <asp:GridView ID="gvQuotaDoc" runat="server" CssClass="table table-hover"
                                AutoGenerateColumns="false" GridLines="none" Width="100%">
                                <HeaderStyle BackColor="#1387de" ForeColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SL" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                        <ItemTemplate><%# Container.DataItemIndex + 1 %>.</ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="QuotaDocID" ItemStyle-HorizontalAlign="Center" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblQuotaDocID" Text='<%#Eval("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="CandidateId" ItemStyle-HorizontalAlign="Center" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCandidateId" Text='<%#Eval("CandidateId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="QuotaId" ItemStyle-HorizontalAlign="Center" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblQuotaId" Text='<%#Eval("QuotaId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Document Name">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblName" Text='<%#Eval("Name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Actions">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlDownload" runat="server" Text="View" CssClass="btn btn-primary btn-sm"
                                                NavigateUrl='<%# "~/Upload/Candidate/QuotaDoc/" + Eval("Name") %>' Target="_blank">
                                                <i class="fas fa-eye"></i> View
                                            </asp:HyperLink>
                                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger btn-sm"
                                                OnClientClick="return confirm('Are you sure, you want to Delete Document!')"
                                                OnClick="btnDelete_Click" Style="margin-left: 0.5rem;"></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Exam Venue Selection Panel -->
                <asp:Panel ID="PanelExamSeatInformation" runat="server" Visible="false">
                    <div class="panel panel-warning">
                        <div class="panel-heading">
                            <%--<i class="fas fa-map-marker-alt"></i>Exam Venue Selection--%>
                            If you wish to remove a faculty, please click the 'Remove' button.
                        </div>
                        <div class="panel-body">
                            <asp:Panel ID="panel_Massage" runat="server" Visible="false">
                                <div class="alert alert-info">
                                    <asp:Label ID="districtMassage" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label>
                                </div>
                            </asp:Panel>

                            <asp:GridView ID="gvFacultyList" runat="server" CssClass="table table-hover"
                                AutoGenerateColumns="false" GridLines="none" Width="100%"
                                OnRowDataBound="gvFacultyList_RowDataBound">
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

                                    <asp:TemplateField HeaderText="District" HeaderStyle-Width="30%" Visible="false">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="form-control" Style="width: 80%;"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="ddlDistrictComV" runat="server"
                                                ControlToValidate="ddlDistrict" ErrorMessage="Required" ForeColor="Crimson"
                                                ValueToCompare="-1" Operator="NotEqual" ValidationGroup="basic1"></asp:CompareValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Actions">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="DeleteForm" ToolTip="Remove Form" CommandArgument='<%#Eval("ID")%>'
                                                runat="server" OnClientClick="return confirm('Are you sure you want to remove this faculty? Program priorities associated with this will also be deleted. This action cannot be undone.');"
                                                CssClass="btn btn-danger btn-sm" OnClick="DeleteForm_Click">                         
                                                <i class="fas fa-trash"></i> Remove
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Action Buttons -->
                <div class="form-section" style="text-align: center; padding: 2rem;">
                    <asp:Button ID="btnSave_Basic" runat="server" Text="Save & Next"
                        CssClass="btn btn-primary" ValidationGroup="basic1"
                        OnClientClick="return validateBasicInfoForm();"
                        OnClick="btnSave_Basic_Click" Style="margin-right: 1rem;"></asp:Button>

                    <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                        CssClass="btn btn-primary"
                        OnClientClick="return validateBasicInfoFormOnNext();"
                        OnClick="btnNext_Click"></asp:Button>

                    <span id="validationMsg" class="validationErrorMsg"></span>
                </div>

                <!-- Message Panel -->
                <asp:Panel ID="messagePanel_Basic" runat="server" Visible="false">
                    <div class="alert alert-info">
                        <asp:Label ID="lblMessageBasic" runat="server" Text=""></asp:Label>
                    </div>
                </asp:Panel>

            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUploadFile" />
            <asp:PostBackTrigger ControlID="btnSave_Basic" />
            <asp:PostBackTrigger ControlID="btnUplaodSuborno" />

        </Triggers>
    </asp:UpdatePanel>

    <%--======================== Animation Extender ========================--%>
    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="UpdatePanel2" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnSave_Basic" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction AnimationTarget="btnSave_Basic" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
