<%@ Page Title="Application Form - Program Priority/Choice" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApplicationPriorityS.aspx.cs" Inherits="Admission.Admission.Candidate.ApplicationPriorityS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <style>

        /* Modern Program Priority Form Styling */
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

        /* Program Selection Section */
        .program-section {
            background: white;
            border-radius: var(--radius-lg);
            padding: 2rem;
            margin-bottom: 2rem;
            box-shadow: var(--shadow-md);
            border: 1px solid rgba(59, 130, 246, 0.1);
            position: relative;
            overflow: hidden;
        }

            .program-section::before {
                content: '';
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                height: 4px;
                background: linear-gradient(90deg, var(--accent), var(--secondary));
                border-radius: var(--radius-lg) var(--radius-lg) 0 0;
            }

            .program-section h4 {
                color: var(--primary);
                margin-bottom: 1.5rem;
                font-weight: 600;
                font-size: 1.25rem;
                display: flex;
                align-items: center;
                gap: 0.75rem;
            }

                .program-section h4::before {
                    content: '';
                    width: 40px;
                    height: 40px;
                    background: linear-gradient(135deg, var(--accent), var(--secondary));
                    border-radius: 50%;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    flex-shrink: 0;
                    background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='white' viewBox='0 0 24 24'%3E%3Cpath d='M7 13h10v-2H7v2zM4 19h16v2H4v-2zm0-6h16v2H4v-2zm0-6h16v2H4V7z'/%3E%3C/svg%3E");
                    background-size: 20px;
                    background-repeat: no-repeat;
                    background-position: center;
                }

        .program-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
            gap: 1.5rem;
            margin-bottom: 1.5rem;
        }

        .filter-panel {
            background: linear-gradient(145deg, #f0fbff, #e1f5fe);
            border-radius: var(--radius-md);
            padding: 1.5rem;
            border: 1px solid rgba(59, 130, 246, 0.2);
            transition: var(--transition);
        }

            .filter-panel:hover {
                transform: translateY(-2px);
                box-shadow: var(--shadow-md);
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

        .btn-danger {
            background: linear-gradient(145deg, var(--danger), #dc2626);
            color: white;
        }

            .btn-danger:hover {
                transform: translateY(-1px);
                box-shadow: 0 4px 12px rgba(239, 68, 68, 0.3);
            }

        /* Priority List Styling */
        .priority-list {
            background: white;
            border-radius: var(--radius-lg);
            padding: 2rem;
            margin-bottom: 2rem;
            box-shadow: var(--shadow-md);
            border: 1px solid rgba(59, 130, 246, 0.1);
        }

            .priority-list h4 {
                color: var(--primary);
                margin-bottom: 1.5rem;
                font-weight: 600;
                font-size: 1.25rem;
                display: flex;
                align-items: center;
                gap: 0.75rem;
            }

                .priority-list h4::before {
                    content: '';
                    width: 40px;
                    height: 40px;
                    background: linear-gradient(135deg, var(--success), #10b981);
                    border-radius: 50%;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    flex-shrink: 0;
                    background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='white' viewBox='0 0 24 24'%3E%3Cpath d='M9 16.17L4.83 12l-1.42 1.41L9 19 21 7l-1.41-1.41z'/%3E%3C/svg%3E");
                    background-size: 20px;
                    background-repeat: no-repeat;
                    background-position: center;
                }

        .priority-item {
            background: linear-gradient(145deg, #f8fafc, #f1f5f9);
            border-radius: var(--radius-md);
            padding: 1rem 1.5rem;
            margin-bottom: 0.75rem;
            border-left: 4px solid var(--accent);
            display: flex;
            justify-content: space-between;
            align-items: center;
            transition: var(--transition);
        }

            .priority-item:hover {
                transform: translateX(5px);
                box-shadow: var(--shadow-sm);
            }

        .priority-info {
            flex: 1;
        }

        .priority-actions {
            display: flex;
            gap: 0.5rem;
        }

        /* Not Eligible Programs Section */
        .not-eligible-section {
            background: white;
            border-radius: var(--radius-lg);
            padding: 2rem;
            margin-top: 2rem;
            box-shadow: var(--shadow-md);
            border: 1px solid rgba(239, 68, 68, 0.1);
        }

            .not-eligible-section h3 {
                color: var(--danger);
                margin-bottom: 1.5rem;
                font-weight: 600;
                font-size: 1.25rem;
                display: flex;
                align-items: center;
                gap: 0.75rem;
            }

                .not-eligible-section h3::before {
                    content: '';
                    width: 40px;
                    height: 40px;
                    background: linear-gradient(135deg, var(--danger), #dc2626);
                    border-radius: 50%;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    flex-shrink: 0;
                    background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='white' viewBox='0 0 24 24'%3E%3Cpath d='M19 6.41L17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12z'/%3E%3C/svg%3E");
                    background-size: 20px;
                    background-repeat: no-repeat;
                    background-position: center;
                }

        .note-banner {
            background: linear-gradient(145deg, #fef2f2, #fee2e2);
            color: var(--danger);
            padding: 1rem;
            border-radius: var(--radius-md);
            border: 1px solid rgba(239, 68, 68, 0.2);
            text-align: center;
            margin: 1.5rem 0;
            font-weight: 600;
        }

            .note-banner span {
                color: var(--accent);
            }

        .action-buttons {
            text-align: center;
            padding: 2rem;
            background: white;
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-md);
            margin-top: 2rem;
        }

        .validationErrorMsg {
            color: var(--danger);
            font-size: 0.9rem;
            display: block;
            margin-top: 0.5rem;
        }

        .spanAsterisk {
            color: var(--danger);
            font-weight: bold;
            margin-left: 3px;
        }

        /* Animation for form elements */
        .program-section, .priority-list, .not-eligible-section {
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
            .program-grid {
                grid-template-columns: 1fr;
                gap: 1rem;
            }

            .program-section, .priority-list, .not-eligible-section {
                padding: 1.5rem;
            }

            .priority-item {
                flex-direction: column;
                align-items: flex-start;
                gap: 1rem;
            }

            .priority-actions {
                align-self: flex-end;
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
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="application-container">
        <!-- Progress Navigation -->
        <div class="progress-nav">
            <nav>
                <ol class="cd-breadcrumb">
                    <li>
                        <a href="ApplicationBasic.aspx" data-step="1" data-label="Basic" title="Basic"></a>
                    </li>
                    <li>
                        <a href="ApplicationEducation.aspx" data-step="2" data-label="Education" title="Education"></a>
                    </li>
                    <li class="current">
                        <a href="#" data-step="3" data-label="Program Priority" title="Program Priority"></a>
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

        <asp:UpdatePanel ID="UpdatePanel_Filter" runat="server">
            <ContentTemplate>
                <!-- Message Panel -->
                <asp:Panel ID="Panel_Master" runat="server">
                    <div class="helper-message" style="display: <%= lblMessage_Masters.Text != "" ? "block" : "none" %>">
                        <asp:Label ID="lblMessage_Masters" runat="server"></asp:Label>
                    </div>
                </asp:Panel>

                <!-- Program Selection Section -->
                <div class="program-section">
                    <h4>
                        <i class="fas fa-list-ol"></i>
                        Select Program Priority
                    </h4>
                    
                    <asp:Panel ID="Panel_GridView" runat="server">
                        <div class="program-grid">
                            <!-- Faculty Selection -->
                            <div class="filter-panel">
                                <label style="display: block; margin-bottom: 0.5rem; font-weight: 600; color: var(--dark);">
                                    Faculty <span class="spanAsterisk">*</span>
                                </label>
                                <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control"
                                    OnSelectedIndexChanged="ddlFaculty_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:CompareValidator ID="ddlFacultyComV" runat="server"
                                    ControlToValidate="ddlFaculty" ValueToCompare="-1" Operator="NotEqual"
                                    ErrorMessage="Faculty selection is required" ForeColor="Crimson" Display="Dynamic"
                                    ValidationGroup="gr1" CssClass="validationErrorMsg"></asp:CompareValidator>
                            </div>

                            <!-- Program Selection -->
                            <div class="filter-panel">
                                <label style="display: block; margin-bottom: 0.5rem; font-weight: 600; color: var(--dark);">
                                    Program <span class="spanAsterisk">*</span>
                                </label>
                                <asp:DropDownList ID="ddlProgram" runat="server" CssClass="form-control"
                                    OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:CompareValidator ID="ddlProgramComV" runat="server"
                                    ControlToValidate="ddlProgram" ValueToCompare="-1" Operator="NotEqual"
                                    ErrorMessage="Program selection is required" ForeColor="Crimson" Display="Dynamic"
                                    ValidationGroup="gr1" CssClass="validationErrorMsg"></asp:CompareValidator>
                            </div>

                            <!-- Choice Number (Hidden by default) -->
                            <div class="filter-panel" style="display: <%= ddlChoice.Visible ? "block" : "none" %>">
                                <label style="display: block; margin-bottom: 0.5rem; font-weight: 600; color: var(--dark);">
                                    Choice Number
                                </label>
                                <asp:DropDownList ID="ddlChoice" runat="server" CssClass="form-control" Visible="false"></asp:DropDownList>
                            </div>
                        </div>

                        <!-- Add Button -->
                        <div style="text-align: center; margin-top: 1.5rem;">
                            <asp:Button ID="btnSave" runat="server" Text="Add Program" CssClass="btn btn-info" 
                                OnClientClick="this.value = 'Adding...'; this.disabled = true;" UseSubmitBehavior="false"
                                Visible="false" OnClick="btnSave_Click" ValidationGroup="gr1" />
                        </div>
                    </asp:Panel>
                </div>

                <!-- Note Banner -->
                <div class="note-banner">
                    <b>N.B: Please add all program priorities then the <span>Next</span> button will be visible</b>
                </div>

                <!-- Priority List Section -->
                <div class="priority-list">
                    <h4>
                        <i class="fas fa-check-circle"></i>
                        Your Program Priorities
                    </h4>

                    <asp:ListView ID="lvProgramPriority" runat="server"
                        OnItemDataBound="lvProgramPriority_ItemDataBound"
                        OnItemCommand="lvProgramPriority_ItemCommand"
                        GroupItemCount="1"
                        GroupPlaceholderID="groupPlaceHolder"
                        ItemPlaceholderID="itemPlaceHolder">
                        <LayoutTemplate>
                            <div class="priority-container">
                                <asp:PlaceHolder runat="server" ID="groupPlaceHolder"></asp:PlaceHolder>
                            </div>
                        </LayoutTemplate>
                        <GroupTemplate>
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                        </GroupTemplate>
                        <ItemTemplate>
                            <div class="priority-item">
                                <div class="priority-info">
                                    <asp:Label ID="lblSerial" runat="server" Visible="false" />
                                    <strong><asp:Label ID="lblUnitName" runat="server" /></strong>
                                    <asp:Label ID="lblProgramName" runat="server" />
                                    <div style="margin-top: 0.25rem;">
                                        <small style="color: var(--accent); font-weight: 600;">
                                            Priority: <asp:Label ID="lblChoice" runat="server" />
                                        </small>
                                    </div>
                                </div>
                                <div class="priority-actions">
                                    <asp:LinkButton CssClass="btn btn-danger btn-sm" ID="lnkRemove" runat="server" CommandName="Remove">
                                        <i class="fas fa-trash"></i> Remove
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="alert alert-warning" role="alert" style="text-align: center; padding: 2rem;">
                                <i class="fas fa-info-circle" style="margin-right: 0.5rem;"></i>
                                No program priorities added yet. Please select faculty and program above to add your choices.
                            </div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </div>

                <!-- Action Buttons -->
                <div class="action-buttons">
                    <asp:Button ID="btnNext" runat="server" Text="Save & Next" 
                        CssClass="btn btn-primary" OnClick="btnNext_Click" />
                </div>

                <!-- Not Eligible Programs Section -->
                <div class="not-eligible-section" runat="server" id="divNotEligible">
                    <h3>
                        <i class="fas fa-exclamation-triangle"></i>
                        Not Eligible Program List
                    </h3>
                    
                    <asp:GridView runat="server" ID="gvNotEligibleProgramList" AutoGenerateColumns="False" AllowPaging="false"
                        Width="100%" ShowHeader="true" CssClass="table table-bordered" GridLines="None">
                        <HeaderStyle BackColor="#0D2D62" ForeColor="White" Height="40" Font-Bold="True" />
                        <RowStyle BackColor="#f8fafc" />
                        <AlternatingRowStyle BackColor="#ffffff" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL#">
                                <ItemTemplate>
                                    <b><%# Container.DataItemIndex + 1 %></b>
                                </ItemTemplate>
                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Program">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblProgram" Text='<%#Eval("ProgramName") +" (" +Eval("ShortCode") +")" %>' 
                                        ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" Width="50%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Reason">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblReason" Text='<%#Eval("Attribute3") %>' 
                                        ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" Width="45%" />
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle Height="45px" VerticalAlign="Middle" HorizontalAlign="Left" />
                    </asp:GridView>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>