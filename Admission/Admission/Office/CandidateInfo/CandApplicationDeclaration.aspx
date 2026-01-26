<%@ Page Title="Application Declaration" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CandApplicationDeclaration.aspx.cs" Inherits="Admission.Admission.Office.CandidateInfo.CandApplicationDeclaration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <style type="text/css">
        input[type="checkbox"]{
            width: 20px !important;
            height: 20px !important;
        }
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
         /* Declaration section */
        .declaration-section {
            background: white;
            border-radius: var(--radius-lg);
            padding: 2rem;
            margin-bottom: 2rem;
            box-shadow: var(--shadow-md);
            border: 1px solid rgba(59, 130, 246, 0.1);
            position: relative;
            overflow: hidden;
        }

            .declaration-section::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                height: 4px;
                background: linear-gradient(90deg, var(--accent), var(--secondary));
                border-radius: var(--radius-lg) var(--radius-lg) 0 0;
            }

            .declaration-section h4 {
                color: var(--primary);
                margin-bottom: 1.5rem;
                font-weight: 600;
                font-size: 1.25rem;
                display: flex;
                align-items: center;
                gap: 0.75rem;
            }

                .declaration-section h4::before {
                    content: '';
                    width: 40px;
                    height: 40px;
                    background: linear-gradient(135deg, var(--accent), var(--secondary));
                    border-radius: 50%;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    flex-shrink: 0;
                    background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='white' viewBox='0 0 24 24'%3E%3Cpath d='M14,2H6A2,2 0 0,0 4,4V20A2,2 0 0,0 6,22H18A2,2 0 0,0 20,20V8L14,2M18,20H6V4H13V9H18V20Z'/%3E%3C/svg%3E");
                    background-size: 20px;
                    background-repeat: no-repeat;
                    background-position: center;
                }

        .declaration-content {
            line-height: 1.7;
            color: var(--dark);
        }

            .declaration-content ol {
                padding-left: 1.5rem;
                margin: 0;
            }

                .declaration-content ol li {
                    margin-bottom: 1rem;
                    text-align: justify;
                    font-size: 0.95rem;
                    position: relative;
                }

                    .declaration-content ol li::marker {
                        font-weight: 600;
                        color: var(--accent);
                    }

        /* Agreement section */
        .agreement-section {
            background: linear-gradient(145deg, #f0f9ff, #e0f2fe);
            border: 2px solid var(--accent-light);
            border-radius: var(--radius-lg);
            padding: 1.5rem;
            margin: 2rem 0;
            display: flex;
            align-items: center;
            gap: 1rem;
            /*cursor: pointer;*/
            transition: var(--transition);
        }

            .agreement-section:hover {
                background: linear-gradient(145deg, #e0f2fe, #bae6fd);
                transform: translateY(-1px);
                box-shadow: var(--shadow-sm);
            }

            .agreement-section input[type="checkbox"] {
                width: 24px !important;
                height: 24px !important;
                margin: 0;
                cursor: pointer;
                accent-color: var(--accent);
                border-radius: 4px;
            }

            .agreement-section label {
                font-weight: 600;
                color: var(--primary);
                cursor: pointer;
                margin: 0;
                flex: 1;
                font-size: 1rem;
            }

        /* Important note section */
        .important-note {
            background: linear-gradient(145deg, #fef2f2, #fee2e2);
            border-left: 4px solid var(--danger);
            border-radius: var(--radius-md);
            padding: 1.5rem;
            margin: 2rem 0;
            position: relative;
        }

            .important-note::before {
                content: '⚠️';
                position: absolute;
                top: 1rem;
                left: 1rem;
                font-size: 1.2rem;
            }

            .important-note .note-content {
                color: var(--danger);
                font-weight: 600;
                font-size: 1rem;
                margin-left: 2rem;
                line-height: 1.6;
            }

        /* Submit section */
        .submit-section {
            text-align: center;
            padding: 2rem;
            background: white;
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-md);
            margin-top: 2rem;
            border: 1px solid rgba(59, 130, 246, 0.1);
        }

        .btn-final-submit {
            background: linear-gradient(145deg, var(--danger), #dc2626);
            color: white;
            border: none;
            padding: 1rem 3rem;
            border-radius: var(--radius-md);
            font-weight: 600;
            font-size: 1.1rem;
            cursor: pointer;
            transition: var(--transition);
            position: relative;
            overflow: hidden;
            display: inline-flex;
            align-items: center;
            gap: 0.75rem;
            box-shadow: var(--shadow-md);
        }

            .btn-final-submit::before {
                content: '';
                position: absolute;
                top: 0;
                left: -100%;
                width: 100%;
                height: 100%;
                background: rgba(255, 255, 255, 0.2);
                transition: var(--transition);
            }

            .btn-final-submit:hover::before {
                left: 100%;
            }

            .btn-final-submit:hover {
                transform: translateY(-2px);
                box-shadow: var(--shadow-lg);
            }

            .btn-final-submit:disabled {
                opacity: 0.7;
                cursor: not-allowed;
                transform: none;
            }

                .btn-final-submit:disabled::before {
                    display: none;
                }

        .submit-icon {
            font-size: 1.2rem;
        }

        /* Message styling */
        .message-area {
            margin: 1rem 0;
            min-height: 1.5rem;
        }

        .message-error {
            background: rgba(239, 68, 68, 0.1);
            color: var(--danger);
            padding: 1rem;
            border-radius: var(--radius-md);
            border-left: 4px solid var(--danger);
            font-weight: 500;
            display: flex;
            align-items: center;
            gap: 0.75rem;
        }

            .message-error::before {
                content: '⚠️';
                font-size: 1.1rem;
            }

        /* Responsive adjustments */
        @media (max-width: 768px) {
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

            .declaration-section {
                padding: 1.5rem;
            }

            .agreement-section {
                flex-direction: column;
                align-items: flex-start;
                text-align: left;
                gap: 0.75rem;
            }

            .btn-final-submit {
                padding: 0.875rem 2rem;
                font-size: 1rem;
            }

            .important-note .note-content {
                margin-left: 1.5rem;
                font-size: 0.9rem;
            }
        }

        /* Animation for form elements */
        .declaration-section, .submit-section {
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

        /* Loading state */
        .btn-final-submit.loading {
            position: relative;
        }

            .btn-final-submit.loading::after {
                content: '';
                position: absolute;
                width: 16px;
                height: 16px;
                margin: auto;
                border: 2px solid transparent;
                border-top-color: #ffffff;
                border-radius: 50%;
                animation: button-loading-spinner 1s ease infinite;
            }

        @keyframes button-loading-spinner {
            from {
                transform: rotate(0turn);
            }

            to {
                transform: rotate(1turn);
            }
        }

        .preview-section {
            text-align: center;
        }

        .btn-preview {
            background-color: black; /* light neutral background */
            border: 1px solid #ccc;
            color: white;
            padding: 10px 20px;
            border-radius: 6px;
            font-size: 14px;
            cursor: pointer;
            transition: all 0.3s ease;
        }

            .btn-preview:hover {
                background-color: #e2e2e2;
                border-color: #999;
            }
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

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
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
                            <li>
                                <asp:HyperLink ID="hrefAppAdditional" runat="server" data-step="6" data-label="Additional/Work" title="Additional/Work"></asp:HyperLink>
                            </li>
                            <li>
                                <asp:HyperLink ID="hrefAppAttachment" runat="server" data-step="7" data-label="Upload Photo" title="Upload Photo"></asp:HyperLink>
                            </li>
                            <li class="active">
                                <asp:HyperLink ID="hrefAppDeclaration" runat="server" data-step="8" data-label="Declaration" title="Declaration"></asp:HyperLink>
                            </li>
                        </ol>
                    </nav>
                </div>
                <%-- end panel heading --%>
                <div class="panel-body">


                    <asp:UpdatePanel ID="updatePanelAdditional" runat="server">
                        <ContentTemplate>

                            <div class="panel panel-default">
                                <div class="panel-body">
                                    <p>
                                        <strong>Declaration by the Candidate:</strong>
                                    </p>
                                    <ol type="a">
                                        <li>I hereby accept that, if admitted to the
                                        <asp:Label ID="lblUniName1" runat="server"></asp:Label>
                                            &nbsp;(<asp:Label ID="lblUniShortName1" runat="server"></asp:Label>), I will abide by the rules and regulations of the university and the
                                        <asp:Label ID="lblUniShortName2" runat="server"></asp:Label>
                                            student code of conduct.</li>
                                        <li>I accept that manufacture, distribution and consumption  of tobacco products, alcohol, drugs and controlled substances are strictly prohibited on the
                                        <asp:Label ID="lblUniShortName3" runat="server"></asp:Label>
                                            premises and that I may be expelled for violating these rules or for abetting violators.</li>
                                        <li>I agree to respect the property and personal rights of all members of the
                                        <asp:Label ID="lblUniShortName4" runat="server"></asp:Label>
                                            community and truthfully represent fact and self at all times.</li>
                                        <li>I have no criminal case pending in police station (PS) or in the Court.</li>
                                        <li>I certify that all the statements mentioned in this application are correct and complete to the best of my knowledge.
                                        </li>
                                        <li>I authorize the university to release information from my application and supporting documents to the authorities and 
                                        organizations that provide financial assistance/fellowship in order to be considered for such support.</li>
                                        <li>I authorize the university to release information from my application and supporting documents to any Government authority or agency.</li>
                                    </ol>

                                    <p>
                                        <span style="color: crimson; font-weight: bold; font-size: 20px;">Note: After final submit, you will not be able to edit your application form. If you do not submit, you will not receive your admit card.</span>
                                    </p>

                                    <hr />

                                    <asp:CheckBox ID="chbxAgreed" runat="server" />
                                    <label for="MainContent_chbxAgreed">I agree To the above terms and conditions.</label>
                                    <br />
                                    <br />

                                    <asp:Button ID="btnSave_Declaration" runat="server" Text="Final Submit"
                                        CssClass="btn btn-danger" OnClick="btnSave_Declaration_Click" />

                                    <asp:Panel ID="messagePanel" runat="server" Visible="false" style="margin-top:10px;">
                                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                    </asp:Panel>

                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
                <%-- end panel body --%>
            </div>
            <%-- end panel default --%>
        </div>
        <%-- end col-md-12 --%>
    </div>
    <%-- end row --%>
</asp:Content>
