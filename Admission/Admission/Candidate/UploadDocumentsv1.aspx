<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadDocumentsv1.aspx.cs" Inherits="Admission.Admission.Candidate.UploadDocumentsv1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <div class="row" id="divFinalSubmit" runat="server" style="text-align:center;font-size:30px">
        <b style="color:red;font-weight:bold">You have already final submitted your application</b>
    </div>

    <div class="row" id="divMain" runat="server" style="margin-top:10px">
        <div class="col-sm-12 col-md-12 col-lg-12">
            <div class="panel panel-info">
                <div class="panel-heading  text-center" style="background-color: black">
                    <h4 style="color: #c4bfbf">Upload Documents</h4>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">1. Upload Your Recent Photo (Passport size JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadPhoto" runat="server" accept=".jpg,.jpeg,.png" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />
                            </div>
                             
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnUploadPhoto" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnUploadPhoto_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblPhotoURL" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>
                            </div>
                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">5. Upload Curriculum Vitae (PDF, JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadCV" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />   
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnCVUpload" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnCVUpload_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblcurriculamURL" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;"  Target="_blank"></asp:HyperLink>
                            </div>

                        </div>
                    </div>

                    <br />
                    <br />
                    <br />

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">2. Upload Your Signature (JPEG, PNG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadSignature" runat="server" accept=".jpg,.jpeg,.png" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />
                             
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnSignature" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnSignature_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblupsign" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>
                            </div>

                        </div>
                        
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">6. Upload English Language Proficiency Certificate (PDF, JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadEnglishCertificate" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />
       
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnEnglishCertificate" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnEnglishCertificate_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblEnglish" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>

                        </div>

                    </div>

                    <br />
                    <br />
                    <br />

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">3. Upload Secondary School Certificate/O-Level/ Equivalent  Diploma (PDF, JPEG)</p>

                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadAcademic" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnAcademic" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnAcademic_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblacademic" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>

                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">7. Upload Higher Secondary Certificate/A-Level/ Equivalent  Diploma (PDF, JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadMedical" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />
                                <br />

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnHsc" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnHsc_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>


                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblmedicalcertificate" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>

                        </div>
                    </div>

                    <br />
                    <br />
                    <br />

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">4. Upload Bachelor/honors certificate (for masters only)(PDF, JPEG)</p>

                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadBechlor" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnbachelor" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnbachelor_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblbachelorcer" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>

                        </div>

                        <div class="col-sm-6 col-md-6 col-lg-6">
                             <p style="font-size: 18px;">8. Upload a certified copy of valid passport(PDF, JPEG)</p>

                            <div class="col-lg-6 col-md-6 col-sm-6">
                                <asp:FileUpload ID="FileUploadPassport" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnpassport" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnpassport_Click" Text="Upload"
                                    ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblPassport" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>                   
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <div class="button" style="margin-top: 27px;">
                                <asp:HyperLink ID="HyperLink1" Style="border-radius: 4px; background-color: #000000a6; color: white;" NavigateUrl="~/Admission/Candidate/academicinfoAppForm.aspx?ecat=4"
                                    runat="server" CssClass="btn btn-light">
                                        <span class="glyphicon glyphicon-chevron-left"></span> Back &nbsp;
                                </asp:HyperLink>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3" style="margin-top: 27px;">
                            <asp:Button ID="bttnsave" ValidationGroup="SUBMIT" Style="margin-left: 138px; border-radius: 4px; width: 85px;" runat="server" Text="Save" OnClick="bttnsave_Click" CssClass="btn btn-primary"></asp:Button>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    

</asp:Content>
