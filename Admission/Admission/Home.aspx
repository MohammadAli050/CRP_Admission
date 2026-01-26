<%@ Page Title="Admission" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Admission.Admission.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script src="../Scripts/jquery-3.1.1.js"></script>
    <script src="../Scripts/Custom/fadeIn.js"></script>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&family=Poppins:wght@600;700&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css">

    <style type="text/css">
        body {
            font-family: 'Roboto', sans-serif;
            background-color: #f4f7f6;
            color: #333;
            min-height: 100vh;
        }

        .main-container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 40px 20px;
        }

        /* --- Page Title --- */
        .page-title {
            text-align: center;
            font-family: 'Poppins', sans-serif;
            font-size: 2.5rem;
            font-weight: 700;
            color: #8a151b; /* BHPI Red */
            margin-bottom: 50px;
            text-transform: uppercase;
            letter-spacing: 1px;
        }

        /* --- Apply Cards Section --- */
        .apply-section {
            background: #ffffff;
            border-radius: 15px;
            padding: 35px;
            margin-bottom: 30px;
            box-shadow: 0 4px 20px rgba(0,0,0,0.08);
            border-top: 5px solid #006a4e; /* BHPI Green */
        }

        .section-title {
            text-align: left;
            font-size: 1.8rem;
            font-weight: 700;
            margin-bottom: 30px;
            color: #006a4e;
            border-bottom: 2px solid #f0f0f0;
            padding-bottom: 10px;
        }

        .apply-card {
            background: #f8f9fa;
            padding: 15px 25px;
            margin-bottom: 15px;
            border-radius: 10px;
            transition: all 0.3s ease;
            display: flex;
            align-items: center;
            border-left: 5px solid #8a151b;
            text-decoration: none;
        }

        .apply-card:hover {
            transform: translateX(10px);
            background: #8a151b;
            border-left-color: #006a4e;
        }

        .apply-card .card-icon {
            font-size: 1.5rem;
            margin-right: 20px;
            color: #8a151b;
            transition: color 0.3s ease;
        }

        .apply-card:hover .card-icon, 
        .apply-card:hover a {
            color: #ffffff !important;
        }

        .apply-card a {
            color: #333;
            font-weight: 500;
            text-decoration: none;
            font-size: 1.1rem;
            flex-grow: 1;
        }

        /* --- Notices Section --- */
        .notices-section {
            background: #ffffff;
            border-radius: 15px;
            padding: 0;
            box-shadow: 0 4px 20px rgba(0,0,0,0.08);
            overflow: hidden;
            border-top: 5px solid #8a151b;
        }

        .notices-section .panel-heading {
            font-size: 1.4rem;
            font-weight: 700;
            color: #ffffff;
            background: #8a151b;
            padding: 15px 20px;
            display: flex;
            align-items: center;
        }

        .notices-section .panel-heading i {
            margin-right: 10px;
        }

        .notices-section .panel-body {
            padding: 10px 20px;
            max-height: 500px;
            overflow-y: auto;
        }

        #tblNotices td {
            padding: 15px 0;
            border-bottom: 1px solid #eee;
        }

        #tblNotices a {
            text-decoration: none;
            color: #006a4e;
            font-weight: 600;
        }

        #tblNotices a:hover {
            color: #8a151b;
        }

        /* --- Footer Info --- */
        .footer-info {
            margin-top: 60px;
            padding: 40px;
            background: #006a4e;
            color: #ffffff;
            border-radius: 15px;
            box-shadow: 0 10px 30px rgba(0, 106, 78, 0.2);
        }

        .footer-info h3 {
            color: #ffffff;
            font-weight: 700;
            margin-bottom: 20px;
            border-bottom: 2px solid rgba(255,255,255,0.2);
            padding-bottom: 10px;
        }

        .footer-info li {
            margin-bottom: 12px;
            color: #e0e0e0;
            font-size: 0.95rem;
        }

        .footer-info strong {
            color: #ffcc00; /* Contrast color for labels */
        }

        /* --- Alerts --- */
        .alert-danger {
            background-color: #fff5f5;
            border-left: 5px solid #8a151b;
            color: #8a151b;
            border-radius: 8px;
        }

        /* --- Animations --- */
        .fadeInDiv {
            animation: fadeIn 0.8s ease-in;
        }

        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(20px); }
            to { opacity: 1; transform: translateY(0); }
        }

        @media (max-width: 768px) {
            .page-title { font-size: 1.8rem; }
            .col-md-7, .col-md-5 { width: 100%; }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="main-container">

    <h1 class="page-title fadeInDiv">BHPI Admission Portal</h1>

    <asp:Panel ID="panelImportantNotice" runat="server" Visible="false">
        <asp:ListView ID="lvImportantNotices" runat="server" OnItemDataBound="lvImportantNotices_ItemDataBound">
            <ItemTemplate>
                <div class="alert alert-danger fadeInDiv" role="alert">
                    <h4 class="alert-heading" style="font-weight: 700;">
                        <i class="fas fa-exclamation-circle"></i> 
                        <asp:Label ID="lblNoticeTitle" runat="server" />
                    </h4>
                    <hr>
                    <asp:Label ID="lblNoticeDetails" runat="server" />
                </div>
            </ItemTemplate>
        </asp:ListView>
    </asp:Panel>
    
    <asp:Panel ID="panel_Maintanence" runat="server" Visible="false">
    </asp:Panel>

    <div class="row fadeInDiv">
        <div class="col-md-7">
            <div class="apply-section">
                <h2 class="section-title">Admission Programs</h2>
                
                <div class="apply-card">
                    <div class="card-icon"><i class="fas fa-user-graduate"></i></div>
                    <asp:HyperLink ID="hrefApplyUndergrad" NavigateUrl="~/Admission/Candidate/SelectProgramV3.aspx?ecat=4" runat="server">
                        Undergraduate Programs
                    </asp:HyperLink>
                    <i class="fas fa-chevron-right text-muted"></i>
                </div>
                
                <div class="apply-card">
                    <div class="card-icon"><i class="fas fa-graduation-cap"></i></div>
                    <asp:HyperLink ID="hrefApplyGrad" NavigateUrl="~/Admission/Candidate/SelectProgramV3.aspx?ecat=6" runat="server">
                        Graduate Programs
                    </asp:HyperLink>
                    <i class="fas fa-chevron-right text-muted"></i>
                </div>
            </div>
        </div>

        <div class="col-md-5">
            <div class="notices-section">
                <div class="panel-heading">
                    <i class="fas fa-bullhorn"></i>
                    <strong>Latest Notices</strong>
                </div>
                <div class="panel-body">
                    <asp:ListView ID="lvNotice" runat="server" OnItemDataBound="lvNotice_ItemDataBound" OnItemCommand="lvNotice_ItemCommand">
                        <LayoutTemplate>
                            <table id="tblNotices" style="width: 100%;">
                                <tr runat="server" id="itemPlaceholder" />
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server">
                                <td>
                                    <i class="far fa-file-alt" style="color: #8a151b; margin-right: 8px;"></i>
                                    <asp:LinkButton ID="lbNoticeTitle" runat="server" Enabled="false" />
                                    <asp:HyperLink ID="hrefExternalUrl" runat="server" Target="_blank" Text="[View Details]"></asp:HyperLink>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div style="padding: 20px; text-align: center; color: #666;">No current notices.</div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </div>
            </div>
        </div>
    </div>

    <div class="row fadeInDiv">
        <div class="col-md-12">
            <div class="footer-info">
                <div class="row">
                    <div class="col-md-8">
                        <h3><asp:Label ID="lblInsName" runat="server"></asp:Label></h3>
                        <ul class="list-unstyled">
                            <li><strong><i class="fas fa-map-marker-alt"></i> Address: </strong><asp:Label ID="lblInsAddress" runat="server"></asp:Label></li>
                            <li><strong><i class="fas fa-phone"></i> Telephone: </strong><asp:Label ID="lblInsTel1" runat="server"></asp:Label></li>
                            <li style="display: none;"><strong>Mobile: </strong><asp:Label ID="lblInsMobile" runat="server"></asp:Label></li>
                            <li><strong><i class="fas fa-fax"></i> Fax: </strong><asp:Label ID="lblInsFax" runat="server"></asp:Label></li>
                        </ul>
                    </div>
                    <div class="col-md-4 text-right" style="opacity: 0.2; font-size: 5rem;">
                        <i class="fas fa-hospital-symbol"></i>
                    </div>
                </div>
            </div>
        </div>
    </div>

</div>
</asp:Content>