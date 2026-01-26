<%@ Page Title="Office - Enroll Candidate" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="EnrollCandidateV2.aspx.cs" Inherits="Admission.Admission.Office.EnrollCandidateV2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        /* Modern UI Enhancements */
        body {
            background-color: #f8f9fa;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        .card {
            border: none;
            border-radius: 10px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
            margin-bottom: 1.5rem;
        }

        .card-header {
            background-color: #ffffff;
            border-bottom: 1px solid #eeeeee;
            padding: 1.25rem;
            border-radius: 10px 10px 0 0 !important;
        }

            .card-header h4 {
                margin: 0;
                color: #2c3e50;
                font-weight: 600;
                font-size: 1.1rem;
                text-transform: uppercase;
                letter-spacing: 0.5px;
            }

        .section-icon {
            color: #3498db;
            margin-right: 10px;
        }

        .form-label {
            font-weight: 600;
            color: #555;
            font-size: 0.85rem;
            margin-bottom: 5px;
            display: block;
        }

        /* Overlay Loader */
        #divProgress {
            background: rgba(255, 255, 255, 0.8);
            backdrop-filter: blur(4px);
            width: 100%;
            height: 100%;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .btn-load {
            padding: 8px 30px;
            font-weight: 500;
            transition: all 0.3s;
        }

        .academic-info-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 15px;
        }

        /* Helper for the ID styling requested */
        .tableStrongColor {
            color: #1a237e;
            font-weight: bold;
        }
    </style>

    <script type="text/javascript">
        function InProgress() {
            var panelProg = document.getElementById('divProgress');
            panelProg.style.display = 'flex';
        }

        function onComplete() {
            var panelProg = document.getElementById('divProgress');
            panelProg.style.display = 'none';
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="divProgress" style="display: none; z-index: 2000; position: fixed; top: 0; left: 0;">
        <div class="text-center">
            <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="120px" Width="120px" />
            <p class="mt-2 fw-bold text-primary">Processing Request...</p>
        </div>
    </div>

    <div class="container-fluid py-4">

        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>

                <div class="card border-primary">
                    <div class="card-body p-4">
                        <div class="row align-items-end">
                            <div class="col-lg-4 col-md-6">
                                <label class="form-label">Payment ID</label>
                                <asp:TextBox ID="txtPaymentId" runat="server" CssClass="form-control form-control-lg" placeholder="Enter PaymentID"></asp:TextBox>
                            </div>
                            <div class="col-lg-2 col-md-3">
                                <br />
                                <asp:Button ID="btnLoad" runat="server" Text="Fetch Data" CssClass="btn btn-primary form-control btn-load" OnClick="btnLoad_Click" />
                            </div>

                            <div class="col-lg-2 col-md-3" runat="server" id="divViewButton">
                                <br />
                                <asp:HiddenField ID="hdnCId" runat="server" Value="0" />
                                <asp:LinkButton ID="lnkViewFullForm" runat="server" Text="View Full Form" OnClick="lnkViewFullForm_Click"
                                    CssClass="btn btn-info form-control btn-load" />
                            </div>

                        </div>
                    </div>
                </div>

                <div class="card">
                    <div class="card-header d-flex align-items-center" style="background-color: #a1bff5">
                        <h4>Personal Details</h4>
                    </div>
                    <div class="card-body">
                        <div style="padding: 10px">
                            <div class="row">
                                <div class="col-lg-3 col-md-6">
                                    <span class="form-label">Full Name</span>
                                    <div class="p-2 border-bottom bg-light text-muted small">
                                        <asp:Label ID="lblFullName" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-6">
                                    <span class="form-label">Email Address</span>
                                    <div class="p-2 border-bottom bg-light text-muted small">
                                        <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-6">
                                    <span class="form-label">Phone Number</span>
                                    <div class="p-2 border-bottom bg-light text-muted small">
                                        <asp:Label ID="lblPhone" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-6">
                                    <span class="form-label">Gender</span>
                                    <div class="p-2 border-bottom bg-light text-muted small">
                                        <asp:Label ID="lblGender" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <hr />
                            <div class="row">
                                <div class="col-lg-3 col-md-6">
                                    <span class="form-label">DOB</span>
                                    <div class="p-2 border-bottom bg-light text-muted small">
                                        <asp:Label ID="lblDOB" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-6">
                                    <span class="form-label">Guardian Phone</span>
                                    <div class="p-2 border-bottom bg-light text-muted small">
                                        <asp:Label ID="lblGuardianPhone" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">

                    <div class="col-lg-6 col-md-6 col-sm-6">

                        <div class="card">
                            <div class="card-header d-flex align-items-center" style="background-color: #a1bff5">
                                <h4>SSC / O-Level / Equivalent</h4>
                            </div>
                            <div class="card-body">
                                <div style="padding: 10px">
                                    <div class="row">
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Institute</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblSSCInstitute" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Board</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblSSCBoard" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Exam Type</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblSSCExamType" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Group</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblSSCGroup" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Division/Class</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblSSCDivClass" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">GPA</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblSSCgpa" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Passing Year</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblSSCYear" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-6 col-md-6 col-sm-6">
                        <div class="card">
                            <div class="card-header d-flex align-items-center" style="background-color: #a1bff5">
                                <h4>HSC / A-Level / Equivalent</h4>
                            </div>
                            <div class="card-body">
                                <div style="padding: 10px">
                                    <div class="row">
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Institute</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblHSCInstitute" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Board</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblHSCBoard" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Exam Type</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblHSCExamType" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Group</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblHSCGroup" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Division/Class</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblHSCDivClass" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">GPA</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblHSCgpa" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Passing Year</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblHSCYear" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="row">

                    <div class="col-lg-6 col-md-6 col-sm-6">

                        <div class="card">
                            <div class="card-header d-flex align-items-center" style="background-color: #a1bff5">
                                <h4>Bachelor</h4>
                            </div>
                            <div class="card-body">
                                <div style="padding: 10px">
                                    <div class="row">
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Institute</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblBInstitute" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Program/degree</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblBProgram" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Others</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblBOthers" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Division/Class</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblBdivClass" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">CGPA</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblBcgpa" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Passing Year</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblBYear" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-6 col-md-6 col-sm-6">
                        <div class="card">
                            <div class="card-header d-flex align-items-center" style="background-color: #a1bff5">
                                <h4>Masters</h4>
                            </div>
                            <div class="card-body">
                                <div style="padding: 10px">
                                    <div class="row">
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Institute</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblMInstitute" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Program/degree</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblMProgram" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Others</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblMOthers" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Division/Class</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblMdivClass" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">CGPA</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblMcgpa" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-lg-3 col-md-6">
                                            <span class="form-label">Passing Year</span>
                                            <div class="p-2 border-bottom bg-light text-muted small">
                                                <asp:Label ID="lblMyear" runat="server" Text=""></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


                <div class="row">

                    <div class="col-lg-6 col-md-6 col-sm-6">
                        <div class="card">
                            <div class="card-header d-flex align-items-center" style="background-color: #a1bff5">
                                <h4>Photo</h4>
                            </div>
                            <div class="card-body">
                                <div style="padding: 10px">
                                    <div class="row">
                                        <asp:Image ID="ImagePhoto" runat="server"
                                            ImageUrl="~/Images/AppImg/user7.jpg"
                                            Width="154" Height="154" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-6 col-md-6 col-sm-6">
                        <div class="card">
                            <div class="card-header d-flex align-items-center" style="background-color: #a1bff5">
                                <h4>Signature</h4>
                            </div>
                            <div class="card-body">
                                <div style="padding: 10px">
                                    <div class="row">
                                        <asp:Image ID="ImageSignature" runat="server"
                                            ImageUrl="~/Images/AppImg/sign2.png"
                                            Width="256" Height="128" />
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>


                <div class="card">
                    <div class="card-header d-flex align-items-center" style="background-color: #a1bff5">
                        <h4>Program Information</h4>
                    </div>
                    <div class="card-body">
                        <div style="padding: 10px">
                            <div class="row">
                                <div class="col-lg-3 col-md-6">
                                    <span class="form-label">Program</span>
                                    <div class="p-2 border-bottom bg-light text-muted small">
                                        <asp:Label ID="lblProgram" runat="server" Text="" Font-Bold="true"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-6">
                                    <span class="form-label">Batch</span>
                                    <div class="p-2 border-bottom bg-light text-muted small">
                                        <asp:Label ID="lblBatch" runat="server" Text="" Font-Bold="true"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-6">
                                    <span class="form-label">Student ID</span>
                                    <div class="p-2 border-bottom bg-light text-muted small">
                                        <asp:Label ID="lblStudentID" runat="server" Text="" Font-Bold="true"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-lg-3 col-md-6">
                                    <br />
                                    <asp:Button ID="btnEnroll" runat="server" Text="Enroll to ERP" CssClass="btn btn-danger form-control btn-load" OnClick="btnEnroll_Click" />

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="UpdatePanel" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnLoad" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction AnimationTarget="btnLoad" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
