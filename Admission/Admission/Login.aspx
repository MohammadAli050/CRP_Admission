<%@ Page Title="Admission - Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Admission.Admission.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style>
        :root {
            /* BHPI Institutional Palette */
            --primary: #8a151b;        /* BHPI Red */
            --secondary: #006a4e;      /* BHPI Green */
            --accent: #00897b;         
            --accent-light: #e0f2f1;
            --success: #10b981;
            --warning: #f59e0b;
            --danger: #dc2626;
            --light: #f8faf9;
            --dark: #1a1a1a;
            --gray: #6B7280;
            --gray-light: #F3F4F6;
            --transition: all 0.3s cubic-bezier(0.25, 0.8, 0.25, 1);
            --shadow-sm: 0 1px 3px rgba(0,0,0,0.12);
            --shadow-md: 0 4px 6px rgba(0,0,0,0.1);
            --shadow-lg: 0 10px 25px rgba(0,0,0,0.1);
            --shadow-xl: 0 20px 40px rgba(0,0,0,0.1);
            --radius-md: 4px;
            --radius-lg: 8px;
            --radius-xl: 12px;
        }

        /* Login Container */
        .login-container {
            min-height: 70vh;
            display: flex;
            align-items: center;
            justify-content: center;
            position: relative;
        }

        /* Login Card */
        .login-card {
            background: white;
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-xl);
            border: 1px solid rgba(0,0,0,0.05);
            overflow: hidden;
            width: 100%;
            max-width: 450px;
            position: relative;
        }

        .login-card::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            height: 5px;
            background: linear-gradient(90deg, var(--primary), var(--secondary));
            z-index: 10;
        }

        /* Login Header */
        .login-header {
            background: #ffffff;
            color: var(--primary);
            padding: 2.5rem 2rem 1.5rem;
            text-align: center;
            position: relative;
            border-bottom: 1px solid #eee;
        }

        .login-title {
            font-size: 1.8rem;
            font-weight: 700;
            margin: 0;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 0.75rem;
            text-transform: uppercase;
            letter-spacing: 1px;
        }

        .login-subtitle {
            font-size: 0.9rem;
            color: var(--secondary);
            margin-top: 0.5rem;
            font-weight: 500;
            text-transform: uppercase;
        }

        .login-body {
            padding: 2rem;
        }

        /* Form Groups */
        .form-group {
            margin-bottom: 1.5rem;
            position: relative;
        }

        .form-label {
            display: block;
            font-weight: 600;
            color: #444;
            margin-bottom: 0.5rem;
            font-size: 0.9rem;
        }

        .form-control-wrapper {
            position: relative;
        }

        .form-control-icon {
            position: absolute;
            left: 1rem;
            top: 50%;
            transform: translateY(-50%);
            color: var(--primary);
            font-size: 1rem;
            z-index: 2;
        }

        .form-control {
            width: 100%;
            padding: 0.8rem 1rem 0.8rem 2.8rem;
            border: 1px solid #dcdcdc;
            border-radius: var(--radius-md);
            font-size: 1rem;
            transition: var(--transition);
            background: #fff;
        }

        .form-control:focus {
            outline: none;
            border-color: var(--secondary);
            box-shadow: 0 0 0 3px rgba(0, 106, 78, 0.1);
        }

        /* Captcha Section */
        .captcha-section {
            background: #f9f9f9;
            border-radius: var(--radius-md);
            padding: 1.25rem;
            margin-bottom: 1.5rem;
            border: 1px solid #eee;
        }

        .captcha-display {
            display: flex;
            align-items: center;
            gap: 1rem;
            margin-bottom: 1rem;
        }

        .captcha-image {
            border: 1px solid #ddd;
            background: #fff;
            padding: 2px;
        }

        .captcha-reload {
            background: var(--secondary);
            border: none;
            border-radius: 4px;
            width: 40px;
            height: 40px;
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            transition: var(--transition);
        }

        .captcha-reload:hover {
            background: var(--primary);
            transform: rotate(90deg);
        }

        /* Submit Button */
        .submit-btn {
            width: 100%;
            background: var(--primary);
            border: none;
            border-radius: var(--radius-md);
            color: white;
            padding: 0.9rem;
            font-size: 1rem;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 1px;
            cursor: pointer;
            transition: var(--transition);
        }

        .submit-btn:hover {
            background: #6d0f14;
            box-shadow: var(--shadow-md);
        }

        /* Forgot Password */
        .forgot-password {
            text-align: center;
            padding-top: 1.2rem;
            margin-top: 1rem;
            border-top: 1px solid #f0f0f0;
        }

        .forgot-password a {
            color: var(--secondary);
            text-decoration: none;
            font-weight: 600;
            font-size: 0.9rem;
        }

        .forgot-password a:hover {
            color: var(--primary);
        }

        /* Error Messages */
        .error-message {
            background: #fff5f5;
            color: var(--danger);
            border: 1px solid #feb2b2;
            border-radius: var(--radius-md);
            padding: 0.8rem;
            margin-bottom: 1.5rem;
            font-size: 0.85rem;
            display: flex;
            align-items: center;
            gap: 0.5rem;
        }

        .field-error {
            color: var(--danger);
            font-size: 0.75rem;
            margin-top: 0.3rem;
            display: block;
        }

        .captcha-error {
            background: #fffaf0;
            color: #c05621;
            border: 1px solid #feebc8;
            padding: 0.6rem;
            font-size: 0.8rem;
            margin-top: 0.5rem;
        }

        /* Animations */
        .fade-in { animation: fadeInUp 0.6s ease forwards; }
        @keyframes fadeInUp {
            from { opacity: 0; transform: translateY(20px); }
            to { opacity: 1; transform: translateY(0); }
        }

        .btn-loading {
            position: relative;
            pointer-events: none;
            opacity: 0.8;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="login-container">
        <div class="login-card fade-in">
            <div class="login-header">
                <h1 class="login-title">
                    <i class="fas fa-lock"></i>
                    <span>Portal Login</span>
                </h1>
                <p class="login-subtitle">BHPI Admission Management</p>
            </div>

            <div class="login-body">
                <asp:Panel ID="messagePanel" runat="server" Visible="false" CssClass="error-message">
                    <i class="fas fa-exclamation-circle"></i>
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </asp:Panel>

                <div class="form-group">
                    <label class="form-label">Username / Mobile</label>
                    <div class="form-control-wrapper">
                        <i class="form-control-icon fas fa-user-md"></i>
                        <asp:TextBox ID="txtUserName" runat="server" 
                            CssClass="form-control" 
                            placeholder="Enter username"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="userNameReq" runat="server" 
                        ControlToValidate="txtUserName" 
                        ErrorMessage="Username is required."
                        Display="Dynamic" 
                        ValidationGroup="gr1" 
                        CssClass="field-error"></asp:RequiredFieldValidator>
                </div>

                <div class="form-group">
                    <label class="form-label">Security Password</label>
                    <div class="form-control-wrapper">
                        <i class="form-control-icon fas fa-key"></i>
                        <asp:TextBox ID="txtPassword" runat="server"
                            TextMode="Password" 
                            CssClass="form-control" 
                            placeholder="Enter password"
                            onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="passwordReq" runat="server" 
                        ControlToValidate="txtPassword" 
                        ErrorMessage="Password is required."
                        Display="Dynamic" 
                        ValidationGroup="gr1" 
                        CssClass="field-error"></asp:RequiredFieldValidator>
                </div>

                <div class="captcha-section">
                    <div class="captcha-display">
                        <asp:UpdatePanel ID="updatePanel1" runat="server">
                            <ContentTemplate>
                                <img runat="server" id="imgCtrl" class="captcha-image" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnReLoadCaptcha" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                        
                        <asp:ImageButton ID="btnReLoadCaptcha" runat="server"
                            CssClass="captcha-reload"
                            ImageUrl="data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='white' viewBox='0 0 24 24'%3E%3Cpath d='M12 4V1L8 5l4 4V6c3.31 0 6 2.69 6 6 0 1.01-.25 1.97-.7 2.8l1.46 1.46C19.54 15.03 20 13.57 20 12c0-4.42-3.58-8-8-8zm0 14c-3.31 0-6-2.69-6-6 0-1.01.25-1.97.7-2.8L5.24 7.74C4.46 8.97 4 10.43 4 12c0 4.42 3.58 8 8 8v3l4-4-4-4v3z'/%3E%3C/svg%3E"
                            ToolTip="Reload captcha"
                            OnClick="btnReLoadCaptcha_Click" />
                    </div>

                    <div class="form-group" style="margin-bottom: 0;">
                        <label class="form-label">Verification Code</label>
                        <asp:TextBox ID="txtCaptcha" runat="server" 
                            CssClass="form-control" 
                            style="padding-left: 1rem;"
                            placeholder="Type characters above"
                            onkeydown="return (event.keyCode!=13);" 
                            AutoCompleteType="Disabled"></asp:TextBox>
                        
                        <asp:RequiredFieldValidator ID="captchaReq" runat="server" 
                            ControlToValidate="txtCaptcha" 
                            ErrorMessage="Captcha code is required."
                            Display="Dynamic" 
                            ValidationGroup="gr1" 
                            CssClass="field-error"></asp:RequiredFieldValidator>
                        
                        <asp:Panel ID="captchaMsg" runat="server" Visible="false" CssClass="captcha-error">
                            <asp:Label ID="lblCaptcha" runat="server"
                                Text="Verification failed. Please try again."></asp:Label>
                        </asp:Panel>
                    </div>
                </div>

                <asp:Button ID="btnSubmit" runat="server" 
                    Text="Sign In" 
                    ValidationGroup="gr1"
                    CssClass="submit-btn" 
                    OnClick="btnSubmit_Click"/>

                <div class="forgot-password">
                    <asp:HyperLink ID="hrefForgotPassword" runat="server" 
                        NavigateUrl="~/Admission/ForgotPassword.aspx">
                        <i class="fas fa-question-circle"></i> Forgot Password?
                    </asp:HyperLink>
                </div>
            </div>
        </div>
    </div>

    <script>
        // Simple loading state on click
        function WebForm_OnSubmit() {
            if (typeof (ValidatorOnSubmit) == "function" && ValidatorOnSubmit() == false) return false;
            var btn = document.getElementById('<%= btnSubmit.ClientID %>');
            if (btn) {
                btn.value = "Authenticating...";
                btn.classList.add('btn-loading');
            }
            return true;
        }
    </script>
</asp:Content>