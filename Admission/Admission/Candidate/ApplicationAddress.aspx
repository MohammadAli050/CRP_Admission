<%@ Page Title="Application Form - Address" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApplicationAddress.aspx.cs" Inherits="Admission.Admission.Candidate.ApplicationAddress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script src="Scripts/AddressValidation.js"></script>

    <style>
        /* Modern Address Form Styling */
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

        /* Address specific sections */
        .address-section {
            background: white;
            border-radius: var(--radius-lg);
            padding: 2rem;
            margin-bottom: 2rem;
            box-shadow: var(--shadow-md);
            border: 1px solid rgba(59, 130, 246, 0.1);
            position: relative;
            overflow: hidden;
        }

            .address-section::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                height: 4px;
                background: linear-gradient(90deg, var(--accent), var(--secondary));
                border-radius: var(--radius-lg) var(--radius-lg) 0 0;
            }

            .address-section h4 {
                color: var(--primary);
                margin-bottom: 1.5rem;
                font-weight: 600;
                font-size: 1.25rem;
                display: flex;
                align-items: center;
                gap: 0.75rem;
            }

                .address-section h4::before {
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

            .address-section.present h4::before {
                background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='white' viewBox='0 0 24 24'%3E%3Cpath d='M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7zm0 9.5c-1.38 0-2.5-1.12-2.5-2.5s1.12-2.5 2.5-2.5 2.5 1.12 2.5 2.5-1.12 2.5-2.5 2.5z'/%3E%3C/svg%3E");
                background-size: 20px;
                background-repeat: no-repeat;
                background-position: center;
            }

            .address-section.permanent h4::before {
                background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='white' viewBox='0 0 24 24'%3E%3Cpath d='M10 20v-6h4v6h5v-8h3L12 3 2 12h3v8z'/%3E%3C/svg%3E");
                background-size: 20px;
                background-repeat: no-repeat;
                background-position: center;
            }

        .address-grid {
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

        /* Checkbox styling */
        .copy-address-section {
            background: linear-gradient(145deg, #f0f9ff, #e0f2fe);
            border: 2px solid var(--accent-light);
            border-radius: var(--radius-md);
            padding: 1rem;
            margin-bottom: 1rem;
            display: flex;
            align-items: center;
            gap: 0.75rem;
            cursor: pointer;
            transition: var(--transition);
        }

            .copy-address-section:hover {
                background: linear-gradient(145deg, #e0f2fe, #bae6fd);
                transform: translateY(-1px);
                box-shadow: var(--shadow-sm);
            }

            .copy-address-section input[type="checkbox"] {
                width: 18px;
                height: 18px;
                margin: 0;
                cursor: pointer;
                accent-color: var(--accent);
            }

            .copy-address-section label {
                font-weight: 600;
                color: var(--primary);
                cursor: pointer;
                margin: 0;
                display: flex;
                align-items: center;
                gap: 0.5rem;
            }

                .copy-address-section label::before {
                    content: '📋';
                    font-size: 1.1rem;
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
        .address-section {
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
            .address-grid {
                grid-template-columns: 1fr;
                gap: 1rem;
            }

            .address-section {
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

        /* Enhanced multi-line textarea */
        .form-control[type="textarea"], 
        .form-control textarea,
        textarea.form-control {
            min-height: 80px;
            resize: vertical;
            font-family: inherit;
            line-height: 1.5;
        }

        /* Improved dropdown styling */
        .form-control option {
            padding: 0.5rem;
        }

        /* Address type indicators */
        .address-type-indicator {
            position: absolute;
            top: 1rem;
            right: 1rem;
            background: rgba(59, 130, 246, 0.1);
            color: var(--accent);
            padding: 0.25rem 0.75rem;
            border-radius: var(--radius-sm);
            font-size: 0.8rem;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        .address-section.permanent .address-type-indicator {
            background: rgba(34, 197, 94, 0.1);
            color: var(--success);
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
                                <li>
                                    <a href="ApplicationRelation.aspx" data-step="4" data-label="Parent/Guardian" title="Parent/Guardian"></a>
                                </li>
                                <li class="current">
                                    <a href="#" data-step="5" data-label="Address" title="Address"></a>
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
                                    <a href="ApplicationRelation.aspx" data-step="3" data-label="Parent/Guardian" title="Parent/Guardian"></a>
                                </li>
                                <li class="current">
                                    <a href="#" data-step="4" data-label="Address" title="Address"></a>
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

                <asp:UpdatePanel ID="updatePanelAddress" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <!-- Address Information -->
                        <div class="address-grid">
                            <!-- Present Address -->
                            <div class="address-section present">
                                <div class="address-type-indicator">Current</div>
                                <h4>
                                    <i class="fas fa-map-marker-alt"></i>
                                    Present Address
                                </h4>
                                
                                <table class="form-table">
                                    <tr>
                                        <td>Address <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtPresentAddress" runat="server" CssClass="form-control"
                                                TextMode="MultiLine" Rows="3" placeholder="Enter your current residential address"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Division <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlPresentDivision" runat="server" CssClass="form-control"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlPresentDivision_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>District <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlPresentDistrict" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Upazila</td>
                                        <td>
                                            <asp:TextBox ID="txtPresentUpazila" runat="server" CssClass="form-control" placeholder="Enter upazila/thana"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Country <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlPresentCountry" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Post Code <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtPresentPostCode" runat="server" CssClass="form-control" placeholder="Enter postal code"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Telephone No.</td>
                                        <td>
                                            <asp:TextBox ID="txtPresentPhone" runat="server" CssClass="form-control" placeholder="Enter landline number (optional)"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <!-- Permanent Address -->
                            <div class="address-section permanent">
                                <div class="address-type-indicator">Home</div>
                                <h4>
                                    <i class="fas fa-home"></i>
                                    Permanent Address
                                </h4>
                                
                                <!-- Copy Address Option -->
                                <div class="copy-address-section">
                                    <asp:CheckBox ID="CheckBoxSameAsPresentAddress" runat="server" 
                                        OnCheckedChanged="CheckBoxSameAsPresentAddress_CheckedChanged" AutoPostBack="true" />
                                    <label for="<%= CheckBoxSameAsPresentAddress.ClientID %>">
                                        Same as Present Address
                                    </label>
                                </div>
                                
                                <table class="form-table">
                                    <tr>
                                        <td>Address <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtPermanentAddress" runat="server" CssClass="form-control"
                                                TextMode="MultiLine" Rows="3" placeholder="Enter your permanent home address"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Division <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlPermanentDivision" runat="server" CssClass="form-control"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlPermanentDivision_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>District <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlPermanentDistrict" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Upazila</td>
                                        <td>
                                            <asp:TextBox ID="txtPermanentUpazila" runat="server" CssClass="form-control" placeholder="Enter upazila/thana"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Country <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlPermanentCountry" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Post Code <span class="spanAsterisk">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtPermanentPostCode" runat="server" CssClass="form-control" placeholder="Enter postal code"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Telephone No.</td>
                                        <td>
                                            <asp:TextBox ID="txtPermanentPhone" runat="server" CssClass="form-control" placeholder="Enter landline number (optional)"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                        <!-- Message Panel -->
                        <asp:Panel ID="messagePanel_Address" runat="server" Visible="false">
                            <div class="helper-message">
                                <asp:Label ID="lblMessageAddress" runat="server" Text=""></asp:Label>
                            </div>
                        </asp:Panel>

                        <!-- Action Buttons -->
                        <div class="action-buttons">
                            <asp:Button ID="btnSave_Address" runat="server" Text="Save & Next"
                                CssClass="btn btn-primary" OnClick="btnSave_Address_Click"
                                OnClientClick="return validateAddress();" Style="margin-right: 1rem;">
                            </asp:Button>

                            <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                                CssClass="btn btn-primary" OnClick="btnNext_Click">
                            </asp:Button>

                            <span id="validationMsg" class="validationErrorMsg"></span>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave_Address" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>