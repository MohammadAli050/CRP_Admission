<%@ Page Title="Admission" Language="C#" MasterPageFile="~/SiteV2.Master" AutoEventWireup="true" CodeBehind="HomeOld.aspx.cs" Inherits="Admission.Admission.HomeOld" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS_ContentPlaceHolder" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="JS_ContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder" runat="server">



    <!-- ======= Hero Section ======= -->
    <section id="hero" class="d-flex align-items-center">
        <div class="container" data-aos="zoom-out" data-aos-delay="100">
            <h1>Welcome to <span>BUP Admission</span></h1>
            <h2>Excellence Through knowledge</h2>
            <div class="d-flex">
                <a class="btn-get-started scrollto" onclick="Apply()" style="cursor: pointer;">Get Started to Apply</a>
                <a href="https://www.youtube.com/watch?v=TpGPec0cJL0" target="_blank" class="glightbox btn-watch-video"><i class="bi bi-play-circle"></i><span>Watch Video How to Apply</span></a>
            </div>
        </div>
    </section>
    <!-- End Hero -->

    <div class="container">
        <!-- ======= Featured Services Section ======= -->
        <section id="featured-services" class="featured-services">
            <div class="container" data-aos="fade-up">
                <div style="text-align: center;">
                    <h2 style="font-size: 38px; padding-bottom: 40px;">Apply for the programs</h2>
                </div>
                <div class="row">
                    <div class="col-md-6 col-lg-3 d-flex align-items-stretch mb-5 mb-lg-0" style="text-align: center;">
                        <div class="icon-box w-100" data-aos="fade-up" data-aos-delay="100">
                            <div class="icon"><i class="fas fa-book-reader"></i></div>
                            <h4 class="title">
                                <asp:HyperLink ID="hlBachelorProgram" runat="server"
                                    NavigateUrl="~/Admission/Candidate/SelectProgramV4.aspx?ecat=4">
                                        Bachelors Program
                                </asp:HyperLink>
                            </h4>
                        </div>
                    </div>

                    <div class="col-md-6 col-lg-3 d-flex align-items-stretch mb-5 mb-lg-0" style="text-align: center;">
                        <div class="icon-box w-100" data-aos="fade-up" data-aos-delay="200">
                            <div class="icon"><i class="fas fa-book-reader"></i></div>
                            <h4 class="title">
                                <asp:HyperLink ID="hlMastersProgram" runat="server"
                                    NavigateUrl="~/Admission/Candidate/SelectProgram.aspx?ecat=6">
                                        Masters Program
                                </asp:HyperLink>
                            </h4>
                        </div>
                    </div>

                    <div class="col-md-6 col-lg-3 d-flex align-items-stretch mb-5 mb-lg-0" style="text-align: center;">
                        <div class="icon-box w-100" data-aos="fade-up" data-aos-delay="300">
                            <div class="icon"><i class="fas fa-book-reader"></i></div>
                            <h4 class="title">
                                <asp:HyperLink ID="hlCertificateCourse" runat="server"
                                    NavigateUrl="~/Admission/Candidate/SelectProgramPhd.aspx">
                                        MPhil & PhD Programs
                                </asp:HyperLink>
                            </h4>
                        </div>
                    </div>

                    <div class="col-md-6 col-lg-3 d-flex align-items-stretch mb-5 mb-lg-0" style="text-align: center;">
                        <div class="icon-box w-100" data-aos="fade-up" data-aos-delay="400">
                            <div class="icon"><i class="fas fa-book-reader"></i></div>
                            <h4 class="title">
                                <asp:HyperLink ID="hlPostgraduateDiploma" runat="server"
                                    NavigateUrl="~/Admission/PostgraduateDiploma/SelectPostgraduateDiplomaProgram.aspx?ecat=1">
                                    Postgraduate Diploma
                                </asp:HyperLink>
                            </h4>
                        </div>
                    </div>

                </div>

            </div>
        </section>
        <!-- End Featured Services Section -->


        <!-- ======= Frequently Asked Questions Section ======= -->
        <section id="faq" class="faq section-bg" style="padding: 25px 0;">
            <div class="container" data-aos="fade-up">

                <div class="section-title">
                    <h2>Contact</h2>
                    <h3><span>Help Line: 09666 790 790</span></h3>
                    <p>From 0930 hours to 1700 hours</p>
                </div>

            </div>
        </section>
        <!-- End Frequently Asked Questions Section -->


        <!-- Info & Video -->
        <section>
            <div class="row">
                <div class="col-sm-6 col-md-6 col-lg-6">
                    <h4>Bangladesh University of Professionals (BUP)</h4>
                    <strong>Address: </strong>Mirpur Cantonment, Dhaka-1216
                    <br />
                    <strong>Telephone: </strong>88-02-8000368, PABX 8000261-4
                    <br />
                    <strong>Fax: </strong>8000443
                </div>
                <div class="col-sm-6 col-md-6 col-lg-6">
                    <div>
                        <iframe width="100%" height="250" src="https://www.youtube.com/embed/TpGPec0cJL0" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
                    </div>
                </div>
            </div>
        </section>
        <!-- End Featured Services Section -->


    </div>

</asp:Content>
