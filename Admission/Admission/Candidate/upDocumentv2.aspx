<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="upDocumentv2.aspx.cs" Inherits="Admission.Admission.Candidate.upDocumentv2" %>
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
                            <p style="font-size: 18px;">9. Upload National ID Card/Equivalent (PDF, JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                              <asp:FileUpload ID="FileUploadNID" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />
                              
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                            <asp:Button ID="btnNID" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius:4px;" Font-Bold="true" OnClick="btnNID_Click" Text="Upload"
                                                                                ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                              <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblnational" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>
                            
                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">12. Upload Health Certificate (PDF,JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                              <asp:FileUpload ID="FileUploadHealth" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />
                             
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                            <asp:Button ID="btnHealth" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius:4px;" Font-Bold="true" OnClick="btnHealth_Click" Text="Upload"
                                                                                ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                              <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblhealth" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>


                        </div>

                    </div>
               
                    <br />
                    <br />
                    <br />

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">10. Upload Police Clearance Certificate (JPEG, PDF)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                              <asp:FileUpload ID="FileUploadPolice" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />
                    
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                            <asp:Button ID="btnPolice" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius:4px;" Font-Bold="true" OnClick="btnPolice_Click" Text="Upload"
                                                                                ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                              <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblpolice" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>

                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">13. Upload Application for scholarship/financial aid (PDF, JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                              <asp:FileUpload ID="FileUploadDeposit" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />
                                
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                                <asp:Button ID="btnDeposit" runat="server" CssClass="btn btn-info" Style="display: inline-block;text-align: center; font-size: 14px; border-radius:4px;" Font-Bold="true" OnClick="btnDeposit_Click" Text="Upload"
                                                                                ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                              <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lbldeposite" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>
                        </div>

                    </div>


                    <br />
                    <br />
                    <br />

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">11. Upload Scholarship/Grant Application (PDF, JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                              <asp:FileUpload ID="FileUploadGrant" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />
                                
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                            <asp:Button ID="btnGrant" runat="server" CssClass="btn btn-info" Style="display: inline-block; text-align: center; font-size: 14px; border-radius:4px;" Font-Bold="true" OnClick="btnGrant_Click" Text="Upload"
                                                                                ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                              <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lblscholarhip" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>

                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <p style="font-size: 18px;">14. Upload Testimonial from your School and College (PDF, JPEG)</p>
                            <div class="col-lg-6 col-md-6 col-sm-6">
                              <asp:FileUpload ID="FileUploadTestimonial" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro;" ClientIDMode="Static" />
                                

                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4" style="width: 22%;">

                            <asp:Button ID="btnTestimonial" runat="server" CssClass="btn btn-info" Style="display: inline-block;text-align: center; font-size: 14px; border-radius:4px;" Font-Bold="true" OnClick="btnTestimonial_Click" Text="Upload"
                                                                                ClientIDMode="Static" CausesValidation="false"></asp:Button>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                              <asp:HyperLink runat="server" CssClass="btn btn-success" Text="View" ID="lbltestimonial" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank"></asp:HyperLink>

                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <div class="col-lg-9 col-md-9 col-sm-9">
                            <div class="button" style="margin-top:27px;">
                                <asp:HyperLink ID="HyperLink1" style="border-radius:4px; background-color: #000000a6; color: white;" NavigateUrl="~/Admission/Candidate/UploadDocumentsv1.aspx?ecat=4"
                                    runat="server" CssClass="btn btn-light">
                                        <span class="glyphicon glyphicon-chevron-left"></span> Back &nbsp;
                                </asp:HyperLink>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3" style="margin-top:27px;">
                            <asp:Button ID="bttnSave" ValidationGroup="SUBMIT" style="margin-left:138px;border-radius:4px; width:85px;" runat="server" Text="Save" OnClick="bttnSave_Click" CssClass="btn btn-primary"></asp:Button>
                        </div>
                    </div>

                </div>


            </div>
        </div>
    </div>
</asp:Content>
