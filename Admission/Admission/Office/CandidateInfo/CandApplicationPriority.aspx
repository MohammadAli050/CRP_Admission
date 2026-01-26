<%@ Page Title="Admin - Application Priority" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CandApplicationPriority.aspx.cs" Inherits="Admission.Admission.Office.CandidateInfo.CandApplicationPriority" %>

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
        .priority-section {
            background: white;
            border-radius: var(--radius-lg);
            padding: 2.5rem;
            margin-bottom: 2.5rem;
            box-shadow: var(--shadow-md);
            border: 1px solid rgba(59, 130, 246, 0.1);
            position: relative;
            overflow: hidden;
        }

            .priority-section::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                height: 5px;
                background: linear-gradient(90deg, var(--accent), var(--secondary));
                border-radius: var(--radius-lg) var(--radius-lg) 0 0;
            }

            .priority-section h4 {
                color: var(--primary);
                margin-bottom: 2rem;
                font-weight: 700;
                font-size: 2.5rem;
                display: flex;
                align-items: center;
                gap: 1rem;
            }

                .priority-section h4::before {
                    content: '';
                    width: 48px;
                    height: 48px;
                    background: linear-gradient(135deg, var(--accent), var(--secondary));
                    border-radius: 50%;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    flex-shrink: 0;
                    background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='white' viewBox='0 0 24 24'%3E%3Cpath d='M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-1 17.93c-3.95-.49-7-3.85-7-7.93 0-.62.08-1.21.21-1.79L9 15v1c0 1.1.9 2 2 2v1.93zm6.9-2.54c-.26-.81-1-1.39-1.9-1.39h-1v-3c0-.55-.45-1-1-1H8v-2h2c.55 0 1-.45 1-1V7h2c1.1 0 2-.9 2-2v-.41c2.93 1.19 5 4.06 5 7.41 0 2.08-.8 3.97-2.1 5.39z'/%3E%3C/svg%3E");
                    background-size: 24px;
                    background-repeat: no-repeat;
                    background-position: center;
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

        /* Filter Form */
        .filter-form-modern {
            background: white;
            border-radius: var(--radius-lg);
            padding: 2rem;
            margin-bottom: 2rem;
            box-shadow: var(--shadow-md);
            border: 1px solid rgba(59, 130, 246, 0.1);
        }

        .filter-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 1.5rem;
            align-items: end;
        }

        .filter-item label {
            display: block;
            font-weight: 600;
            color: var(--dark);
            margin-bottom: 0.75rem;
            font-size: 1.4rem;
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

        .btn-info-modern {
            background: linear-gradient(145deg, #0ea5e9, #0284c7);
            color: white;
            box-shadow: var(--shadow-sm);
        }

            .btn-info-modern:hover {
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
            width: 100%;
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
                font-size: 1.4rem;
            }

            .table-modern tbody tr:hover {
                background: var(--light);
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

        .alert-warning-modern {
            background: linear-gradient(145deg, #fef3c7, #fde68a);
            color: var(--warning);
            border-left: 5px solid var(--warning);
            text-align: center;
            padding: 2rem;
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

        /* Link Buttons */
        .link-btn-modern {
            background: none;
            border: none;
            color: var(--accent);
            font-weight: 600;
            cursor: pointer;
            text-decoration: none;
            transition: var(--transition);
            padding: 0.5rem 1rem;
            border-radius: var(--radius-sm);
        }

            .link-btn-modern:hover {
                background: rgba(59, 130, 246, 0.1);
                text-decoration: underline;
            }

        .link-btn-danger {
            color: var(--danger);
        }

            .link-btn-danger:hover {
                background: rgba(220, 38, 38, 0.1);
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

        .priority-section {
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
            .priority-section h4 {
                font-size: 1.35rem;
            }

            .filter-grid {
                grid-template-columns: 1fr;
            }
        }

        @media (max-width: 768px) {
            .admin-container {
                padding: 1.5rem;
            }

            .priority-section {
                padding: 2rem;
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

            .priority-section h4 {
                font-size: 1.25rem;
            }

            .btn-modern {
                padding: 0.875rem 1.75rem;
                font-size: 1rem;
            }
        }

        @media (max-width: 576px) {
            .admin-container {
                padding: 1rem;
            }

            .priority-section {
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

            .btn-modern {
                padding: 0.75rem 1.5rem;
                font-size: 0.95rem;
                width: 100%;
                justify-content: center;
            }
        }

        #MainContent_lblMessage_Masters {
            color: red;
            font-size: 18px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel_Filter" runat="server">
        <ContentTemplate>

            <div class="admin-container">

                <!-- Progress Navigation -->
                <div class="progress-nav">
                    <nav>
                        <ol class="breadcrumb-modern">
                            <li>
                                <asp:HyperLink ID="hrefAppBasic" runat="server" data-step="1" data-label="Basic Info" title="Basic Info"></asp:HyperLink>
                            </li>
                            <li class="active">
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
                            <li>
                                <asp:HyperLink ID="hrefAppDeclaration" runat="server" data-step="8" data-label="Declaration" title="Declaration"></asp:HyperLink>
                            </li>
                        </ol>
                    </nav>
                </div>

                <!-- Program Priority Section -->
                <div class="priority-section">
                    <h4>
                        <i class="fas fa-list-ol"></i>
                        Program Priority Selection
                    </h4>

                    <div class="alert-modern">
                        <span class="spanAsterisk">*</span> Please note that there is no restriction for Admin while adding a priority.
                   
                    </div>

                    <!-- Message Panel -->
                    <asp:Panel ID="Panel_Master" runat="server">
                        <asp:Label ID="lblMessage_Masters" runat="server"></asp:Label>
                    </asp:Panel>

                    <!-- Filter Form -->
                    <asp:Panel ID="Panel_GridView" runat="server">
                        <div class="filter-form-modern">
                            <div class="filter-grid">
                                <div class="filter-item">
                                    <label for="ddlFaculty">Faculty <span class="spanAsterisk">*</span></label>
                                    <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control-modern"
                                        OnSelectedIndexChanged="ddlFaculty_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="ddlFacultyComV" runat="server"
                                        ControlToValidate="ddlFaculty" ValueToCompare="-1" Operator="NotEqual"
                                        ErrorMessage="Required" ForeColor="Crimson" Display="Dynamic"
                                        ValidationGroup="gr1" CssClass="validationErrorMsg"></asp:CompareValidator>
                                </div>

                                <div class="filter-item">
                                    <label for="ddlProgram">Program <span class="spanAsterisk">*</span></label>
                                    <asp:DropDownList ID="ddlProgram" runat="server" CssClass="form-control-modern"
                                        OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:CompareValidator ID="ddlProgramComV" runat="server"
                                        ControlToValidate="ddlProgram" ValueToCompare="-1" Operator="NotEqual"
                                        ErrorMessage="Required" ForeColor="Crimson" Display="Dynamic"
                                        ValidationGroup="gr1" CssClass="validationErrorMsg"></asp:CompareValidator>
                                </div>

                                <div class="filter-item" style="visibility: hidden;">
                                    <label for="ddlChoice">Choice No.</label>
                                    <asp:DropDownList ID="ddlChoice" runat="server" CssClass="form-control-modern"></asp:DropDownList>
                                </div>

                                <div class="filter-item">
                                    <label>&nbsp;</label>
                                    <asp:Button ID="btnSave" runat="server" Text="Add Program" OnClientClick="this.value = 'Adding.Please wait ....'; this.disabled = true;" UseSubmitBehavior="false"
                                        OnClick="btnSave_Click" ValidationGroup="gr1"
                                        CssClass="btn-modern btn-info-modern" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <!-- Program Priority List -->
                    <asp:ListView ID="lvProgramPriority" runat="server"
                        OnItemDataBound="lvProgramPriority_ItemDataBound"
                        OnItemCommand="lvProgramPriority_ItemCommand"
                        GroupItemCount="1"
                        GroupPlaceholderID="groupPlaceHolder"
                        ItemPlaceholderID="itemPlaceHolder">
                        <LayoutTemplate>
                            <table class="table-modern">
                                <thead>
                                    <tr>
                                        <th>SL#</th>
                                        <th>Faculty Name</th>
                                        <th>Program Name</th>
                                        <th>Choice</th>
                                        <th style="text-align: right;">Actions</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder runat="server" ID="groupPlaceHolder"></asp:PlaceHolder>
                                </tbody>
                            </table>
                        </LayoutTemplate>
                        <GroupTemplate>
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                        </GroupTemplate>
                        <ItemTemplate>
                            <tr runat="server">
                                <td>
                                    <asp:Label ID="lblSerial" runat="server" Visible="false" />
                                    <%# Container.DataItemIndex + 1 %>.
                                </td>
                                <td>
                                    <asp:Label ID="lblUnitName" runat="server" />
                                </td>
                                <td>
                                    <asp:Label ID="lblProgramName" runat="server" />
                                </td>
                                <td>
                                    <asp:Label ID="lblChoice" runat="server" />
                                </td>
                                <td style="text-align: right;">
                                    <asp:LinkButton ID="lnkRemove" runat="server"
                                        CssClass="link-btn-modern link-btn-danger"
                                        CommandName="Remove">Remove</asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="alert-warning-modern" role="alert">
                                No program choices added. Please add program choices using the form above.
                           
                            </div>
                        </EmptyDataTemplate>
                    </asp:ListView>

                    <!-- Next Button -->
                    <div style="text-align: center; margin-top: 2rem;">
                        <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                            CssClass="btn-modern btn-primary-modern" />
                    </div>
                </div>

            </div>
            <%-- end admin-container --%>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
