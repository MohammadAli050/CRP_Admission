<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelectCertificateProgram.aspx.cs" Inherits="Admission.Admission.CertificateCandidate.SelectCertificateProgram" %>





<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%--<tr runat="server" style="background-color: #1387de; color: white;">
                                    <th runat="server"></th>
                                    <th runat="server"></th>
                                    <th runat="server"></th>
                                    <th runat="server"></th>
                                    <th runat="server"></th>
                                    <th></th>
                                </tr>--%>
    <%--<asp:DataPager runat="server" ID="DataPager" PageSize="20">
                                        <Fields>
                                            <asp:NumericPagerField
                                                PreviousPageText="<--"
                                                NextPageText="-->" />
                                        </Fields>
                                    </asp:DataPager>--%>
    <%--&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkApply" runat="server" CssClass="btn btn-primary">APPLY</asp:LinkButton>--%>



    <div class="row">
        <div class="col-md-12">



           <%-- <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Select 
                        <asp:Label ID="lblEducationCat" runat="server"></asp:Label>
                    </strong>
                </div>
                <div class="panel-body">

                    <asp:ListView ID="lvAdmSetup" runat="server"
                        OnItemDataBound="lvAdmSetup_ItemDataBound"
                        OnItemCommand="lvAdmSetup_ItemCommand">
                        <LayoutTemplate>
                            <table id="tblAdmSetup"
                                class="table table-hover table-condensed table-striped"
                                style="width: 100%; text-align: left">
                                
                                <th runat="server">School Name</th>
                                <th runat="server">Start Date</th>
                                <th runat="server">End Date</th>
                                <th runat="server">Fee</th>
                                <th runat="server"></th>
                                <tr runat="server" id="itemPlaceholder" />
                            </table>
                            
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server">
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblUnitName" runat="server" />
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblStartDate" runat="server" />
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblEndDate" runat="server" />
                                </td>
                                <td valign="middle" align="left" class="">BDT.
                                    <asp:Label ID="lblFee" runat="server" />
                                </td>
                                <td valign="middle" align="right" class="">

                                    <asp:LinkButton ID="lnkViewDetails" runat="server" CssClass="btn btn-info btn-sm">APPLY</asp:LinkButton>
                                    
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="alert alert-warning" role="alert" style="text-align: center">No program(s) opened for admission.</div>
                        </EmptyDataTemplate>
                    </asp:ListView>

                </div>
               
            </div>--%>






                            <div class="col-md-8">


                                <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Select 
                        <asp:Label ID="lblEducationCat" runat="server"></asp:Label>
                    </strong>
                </div>
                <div class="panel-body">

                    <asp:ListView ID="lvAdmSetup" runat="server"
                        OnItemDataBound="lvAdmSetup_ItemDataBound"
                        OnItemCommand="lvAdmSetup_ItemCommand">
                        <LayoutTemplate>
                            <table id="tblAdmSetup"
                                class="table table-hover table-condensed table-striped"
                                style="width: 100%; text-align: left">
                                
                                <th runat="server">School Name</th>
                                <th runat="server">Start Date</th>
                                <th runat="server">End Date</th>
                                <th runat="server">Fee</th>
                                <th runat="server"></th>
                                <tr runat="server" id="itemPlaceholder" />
                            </table>
                            
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server">
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblUnitName" runat="server" />
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblStartDate" runat="server" />
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblEndDate" runat="server" />
                                </td>
                                <td valign="middle" align="left" class="">BDT.
                                    <asp:Label ID="lblFee" runat="server" />
                                </td>
                                <td valign="middle" align="right" class="">

                                    <asp:LinkButton ID="lnkViewDetails" runat="server" CssClass="btn btn-info btn-sm">APPLY</asp:LinkButton>
                                    
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="alert alert-warning" role="alert" style="text-align: center">No program(s) opened for admission.</div>
                        </EmptyDataTemplate>
                    </asp:ListView>

                </div>
               
            </div>




                            </div>
                           
            
                            <%-- <div class="col-md-4">
                                <div class="panel panel-default">
                                    <div class="panel-heading style_thead">
                                        I have a Payment ID <br /> but did not complete my payment
                                    </div>
                                    <div class="panel-body panelBody_edu_marginBottom">
                                        <table style="width: 100%" class="table table-condensed table-striped">
                                            <tr>
                                                <td style="width: 20%" class="style_td">Payment ID</td>
                                                <td>
                                                    <asp:TextBox ID="txtPaymentId" runat="server" CssClass="form-control" onkeydown = "return (event.keyCode!=13);"></asp:TextBox>
                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" Display="Dynamic"
                                                        ControlToValidate="txtPaymentId" ErrorMessage="Required" ForeColor="Crimson" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="Button1" runat="server" Text="Go For Payment" OnClick="btnNext_Click" ValidationGroup="gr1" Class="btn btn-primary" />
                                                    <br />
                                                    <asp:Label ID="lblOLevelResult" runat="server" Text="" Style="font-weight: bold; color: crimson;"></asp:Label>
                                                </td>
                                            </tr>

                                        </table>
                                    </div>
                                </div>
                            </div>--%>























        </div>
        <%-- END COL-MD-6 --%>
    </div>
    <%-- END ROW 1 --%>
    <%-- ------------------------------------------------------------------------------------------------------------------- --%>

<%--    <asp:Panel ID="panelOfflineMasters" runat="server" Visible="false">
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <strong>MPCHRS & MDHSM</strong>
                    </div>
                    <div class="panel-body">
                        <table class="table table-hover table-responsive" style="width: 100%; text-align: left">--%>
                            <%--<tr>
                                <td>
                                    <strong>Master of Peace, Conflict and Human Rights Studies (MPCHRS)</strong>
                                </td>
                                <td>
                                    <a href="../../ApplicationDocs/MPCHRS.pdf" target="_blank">View Details</a>
                                </td>
                                <td>
                                    <a href="../../ApplicationDocs/Application Form- MPCHRS 2017-2018.pdf" target="_blank">Download Application Form</a>
                                </td>
                                <td></td>
                            </tr>--%>
                            <%--<tr>
                                <td>
                                    <p><strong>Masters in Disaster and Human Security Management (MDHSM)</strong></p>
                                    <p>
                                        <u>Students admission fee</u> : 1000 tk <br />
                                        <u>Bank Acc Name</u> : DDHSM, BUP <br />
                                        <u>Bank Acc Number</u> : 0028-0210007542 <br />
                                        <i>After depositing 1000 tk to bank, student will fill up the application form and will 
                                        submit attached copies of all certificate, transcript, testimonial with the application 
                                        form to the admission office.</i>
                                    </p>
                                </td>
                                <td>
                                    <a href="../../ApplicationDocs/MDHSM_circular.pdf" target="_blank">View Details</a><br /><br />
                                    <a href="../../ApplicationDocs/MDHSM_Jan2018_AdmissionNotice.pdf" target="_blank">View Admission Notice</a>
                                </td>
                                <td>
                                    <a href="../../ApplicationDocs/MDHSM_Application_Form.pdf" target="_blank">Download Application Form</a>
                                </td>
                                <td>
                                    <a href="../../ApplicationDocs/MDHSM_Admit card_2.pdf" target="_blank">Admit Card</a>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>--%>

    <div class="row">
        <div class="col-md-12">
            Please visit
            <asp:HyperLink NavigateUrl="http://www.bup.edu.bd" Target="_blank" runat="server">BUP Official Website </asp:HyperLink>
            for more information about Undergraduate and Graduate programs.
        </div>
    </div>

</asp:Content>
