<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Listofdocument.aspx.cs" Inherits="Admission.Admission.Candidate.Listofdocument" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" id="divFinalSubmit" runat="server" style="text-align:center;font-size:30px">
        <b style="color:red;font-weight:bold">You have already final submitted your application</b>
    </div>

    <div class="row" id="divMain" runat="server" style="margin-top:10px">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="panel panel-default">
                <div class="panel-body">

                <h4 style="font-size:20px; font-weight:bold; text-align:center;">LIST OF DOCUMENTS TO BE UPLOADED</h4>
                    <br />
                <p style="color: crimson; font-size:16px; font-weight:bold;"
                runat="server">
                <b>Enclosure (Upload certified scanned copy or photocopy):</b></p>             
                <ul>
                    <li> Academic Certificates and Records.</li>
                    <li> Testimonial from your School and College.</li>
                    <li> A recent photograph (passport size with white background).</li>
                    <li> A complete Curriculum Vitae (Within one page).</li>
                    <li> Result of IELTS/equivalent/similar examination or certificate indicating very good command - both written and verbal- of   English language from your School/College.</li>
                    <li> A recent national hospital certificate or a certificate from the relevant recognized health authority about health status and any infectious diseases for public health security.</li>
                    <li> A certified copy of valid passport.</li>
                    <li> A national identity card (If any) or any valid document provided by your Govt.</li>
                    <li> Police Clearance from your country of residence.</li>
                    <li> Application (to respected Vice Chancellor, BUP) for Scholarship/ Financial Aid explaining the reasons for requesting.</li>
                    <li> Deposit slip of Application Fees (Non-refundable)
                        <br />
                     Bank Transfer </li>
                  </ul>
                    <p style="font-weight:bold; font-size: 16px;">  Note: </p>
                    
                    <ul>
                        <li>Only complete documentation will be considered for further processing. Inaccurate, incomplete or illegible application forms will not be considered.  Certificates in ser 1,4,5,&8 must contain mobile and email address of the signatories. <b> Visit Web: </b>www.bup.edu.bd</li>
                        <li>Proof of financial solvency from Bank (Parents/Guardians - last 06 months), or any other encashable investment certificate.</li>
                    </ul>


                   <br /> 
                

                    <div class="row">
            <div class="col-lg-2 col-md-2 col-sm-2">
                 <asp:HyperLink ID="HyperLink1" Style="text-align: left; border-radius: 4px;  background-color: #000000a6; color: white;" NavigateUrl="~/Admission/Candidate/academicInfoAppForm.aspx?ecat=4"
                                    runat="server" CssClass="btn btn-light">
                                        <span class="glyphicon glyphicon-chevron-left"></span> Back &nbsp;
                                </asp:HyperLink>
            </div>

            <div class="col-lg-8 col-md-8 col-sm-8">
                </div>

            <div class="col-lg-2 col-md-2 col-sm-2" style="text-align:right;">
                <asp:Button ID="btnSave" Style="margin-left: 78px; border-radius: 4px; width: 85px;" runat="server" Text="Next" OnClick="btnSave_Click" CssClass="btn btn-primary"></asp:Button>
            </div>
            </div>
                    </div>
                </div>
        </div>


        

    </div>
</asp:Content>
