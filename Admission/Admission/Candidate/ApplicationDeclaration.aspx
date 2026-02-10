<%@ Page Title="Application Form - Declaration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApplicationDeclaration.aspx.cs" Inherits="Admission.Admission.Candidate.ApplicationDeclaration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style>
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
                    background: linear-gradient(to right, var(--success), var(--success), var(--accent));
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
            background: red;
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
    </style>
    <script>
        window.onload = function () {
            var messageLabel = document.getElementById('<%= lblMessage.ClientID %>');
            var messageArea = document.getElementById('messageArea');

            if (messageLabel && messageArea) {
                if (messageLabel.innerText.trim() === "") {
                    messageArea.style.display = "none";
                } else {
                    messageArea.style.display = "block";
                }
            }
        };
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
                                <li>
                                    <a href="ApplicationRelation.aspx" data-step="4" data-label="Parent/Guardian" title="Parent/Guardian"></a>
                                </li>
                                <li>
                                    <a href="ApplicationAddress.aspx" data-step="5" data-label="Address" title="Address"></a>
                                </li>
                                <li>
                                    <a href="ApplicationAttachment.aspx" data-step="6" data-label="Photo" title="Photo"></a>
                                </li>
                                <li class="current">
                                    <a href="#" data-step="7" data-label="Declaration" title="Declaration"></a>
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
                                <li class="current">
                                    <a href="#" data-step="7" data-label="Declaration" title="Declaration"></a>
                                </li>
                            </ol>
                        </nav>
                    </div>
                </div>
                <!-- Message Area -->
                <div class="message-area" id="messageArea">
                    <asp:Label ID="lblMessage" runat="server" CssClass="message-error"></asp:Label>
                </div>
                <!-- Declaration Section -->
                <div class="declaration-section">
                    <h4>
                        <i class="fas fa-file-signature"></i>
                        Declaration by the Candidate
                    </h4>

                    <div class="declaration-content">
                        <ol type="a">
                            <li>I hereby accept that, if admitted to the
                                <asp:Label ID="lblUniName1" runat="server"></asp:Label>
                                &nbsp;(<asp:Label ID="lblUniShortName1" runat="server"></asp:Label>), I will abide by the rules and regulations of the university and the
                                <asp:Label ID="lblUniShortName2" runat="server"></asp:Label>
                                student code of conduct.
                            </li>
                            <li>I accept that manufacture, distribution and consumption of tobacco products, alcohol, drugs and controlled substances are strictly prohibited on the
                                <asp:Label ID="lblUniShortName3" runat="server"></asp:Label>
                                premises and that I may be expelled for violating these rules or for abetting violators.
                            </li>
                            <li>I agree to respect the property and personal rights of all members of the
                                <asp:Label ID="lblUniShortName4" runat="server"></asp:Label>
                                community and truthfully represent fact and self at all times.
                            </li>
                            <li>I have no criminal case pending in police station (PS) or in the Court.</li>
                            <li>I certify that all the statements mentioned in this application are correct and complete to the best of my knowledge.</li>
                            <li>I authorize the university to release information from my application and supporting documents to the authorities and 
                                organizations that provide financial assistance/fellowship in order to be considered for such support.
                            </li>
                            <li>I authorize the university to release information from my application and supporting documents to any Government authority or agency.</li>
                        </ol>
                    </div>
                </div>

                <!-- Preview Application Button -->
                <div class="preview-section">
                    <asp:Button ID="btnPreview" runat="server" Text="Preview Application"
                        CssClass="btn-preview"
                        OnClientClick="window.open('ReviewApplication.aspx', '_blank'); return false;" />

                </div>

                <!-- Agreement Section -->
                <div class="agreement-section">
                    <asp:CheckBox ID="chbxAgreed" runat="server" />
                    <label for="<%= chbxAgreed.ClientID %>">
                        I agree to the above terms and conditions.
                    </label>
                </div>


                <!-- Important Note -->
                <div class="important-note">
                    <div class="note-content">
                        Note: After final submit, you will not be able to edit your application form. If you do not submit, you will not receive your admit card.
                    </div>
                </div>



                <!-- Submit Section -->
                <div class="submit-section">
                    <asp:Button ID="btnSave_Declaration" runat="server" Text="Final Submit"
                        OnClientClick="this.value = 'Please wait....'; this.disabled = true;" UseSubmitBehavior="false"
                        CssClass="btn-final-submit" OnClick="btnSave_Declaration_Click"></asp:Button>
                </div>

            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave_Declaration" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
