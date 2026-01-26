<%@ Page Title="Select Programs" Language="C#" MasterPageFile="~/SiteV2.Master" AutoEventWireup="true" CodeBehind="SelectProgramV4.aspx.cs" Inherits="Admission.Admission.Candidate.SelectProgramV4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CSS_ContentPlaceHolder" runat="server">

    <style>
        input[type=checkbox] {
            width: 20px;
            height: 20px;
        }


        @media only screen and (max-width: 600px) {
            .horizontalScrollTable {
                display: block;
                overflow-x: auto;
                white-space: nowrap;
            }
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="JS_ContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <asp:UpdatePanel runat="server" ID="upanel1">
        <ContentTemplate>
            <br />
            <div class="container">

                <div class="row">
                    <div class="col-sm-8 col-md-8 col-lg-8">
                        <div class="card">
                            <div class="card-header" style="text-align: center;">
                                <h4 class="card-title mb-0">Bachelors Program</h4>
                            </div>
                            <div class="card-body" style="padding-left: 5px; padding-right: 5px;">
                                <asp:ListView ID="lvAdmSetup" runat="server"
                                    OnItemDataBound="lvAdmSetup_ItemDataBound">
                                    <LayoutTemplate>
                                        <table id="tblAdmSetup"
                                            class="table table-hover table-condensed table-striped horizontalScrollTable"
                                            style="width: 100%; text-align: left">
                                            <th runat="server"></th>
                                            <th runat="server">Faculty</th>
                                            <th runat="server">Application
                                                <br />
                                                Start Date</th>
                                            <th runat="server">Application
                                                <br />
                                                End Date</th>
                                            <th runat="server">Fee</th>
                                            <tr runat="server" id="itemPlaceholder" />
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr runat="server">
                                            <td>
                                                <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" OnCheckedChanged="ckbxSelectedSchool_CheckedChanged" />
                                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("ID") %>' />
                                                <asp:HiddenField ID="HiddenField2" runat="server" Value='<%# Eval("AdmissionUnitID") %>' />
                                                <asp:HiddenField ID="HiddenField3" runat="server" Value='<%# Eval("AcaCalID") %>' />
                                                <asp:HiddenField ID="HiddenField4" runat="server" Value='<%# Eval("Fee") %>' />
                                            </td>
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
                                        </tr>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <div class="alert alert-warning" role="alert" style="text-align: center">No program(s) opened for admission.</div>
                                    </EmptyDataTemplate>
                                </asp:ListView>

                                <div class="card">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-sm-8 col-md-8 col-lg-8" style="font-weight: bold;">
                                                <p class="mb-0">
                                                    Number of Selected Faculty(s):
                                                    <asp:Label ID="lblNoOfSelSchool" runat="server" Text="0" Style="color: crimson;"></asp:Label>
                                                </p>
                                                <p class="mb-0">
                                                    Total Fees:
                                                    <asp:Label ID="lblTotal" runat="server" Text="0" Style="color: crimson;"></asp:Label>
                                                </p>
                                            </div>
                                            <div class="col-sm-4 col-md-4 col-lg-4" style="text-align: right;">
                                                <%--<asp:Button ID="btnApply1" runat="server" Text="APPLY" Style="width:247px;margin-top:5px" OnClick="btnApply_Click" CssClass="btn btn-success" />--%>
                                                <asp:LinkButton ID="btnApply" runat="server" OnClick="btnApply_Click" CssClass="btn btn-success mt-1"><i class="fas fa-share-square"></i>&nbsp;APPLY</asp:LinkButton>
                                            </div>
                                        </div>

                                    </div>
                                </div>



                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4 col-md-4 col-lg-4">

                        <div class="card">
                            <div class="card-header" style="text-align: center;">
                                <h4 class="mb-0" style="font-size: 20px;">I have a Payment ID
                                <br />
                                    but did not complete my payment
                                </h4>
                            </div>
                            <div class="card-body">

                                <div class="row">
                                    <div class="col-sm-4 col-md-4 col-lg-4 pr-0">
                                        Payment ID
                                    </div>
                                    <div class="col-sm-8 col-md-8 col-lg-8">
                                        <asp:TextBox ID="txtPaymentId" runat="server" CssClass="form-control" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" Display="Dynamic"
                                            ControlToValidate="txtPaymentId" ErrorMessage="Required" ForeColor="Crimson" ValidationGroup="gr1"></asp:RequiredFieldValidator>
                                        <br />
                                        <asp:LinkButton ID="btnGoForPayment" runat="server" OnClick="btnGoForPayment_Click" ValidationGroup="gr1" Class="btn btn-info w-100"><i class="fas fa-paper-plane"></i>&nbsp;Go For Payment</asp:LinkButton>
                                        <br />
                                        <asp:Label ID="lblOLevelResult" runat="server" Text="" Style="font-weight: bold; color: crimson;"></asp:Label>
                                    </div>
                                </div>

                            </div>
                        </div>

                    </div>
                </div>


                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12">

                        <ul class="mt-3" style="padding-left: 20px;">
                            <li>Candidates having combination of SSC/HSC and O/A Level (for example, SSC & A level or O level & HSC) and International Baccalaureate are requested to contact with BUP admission helpline. 
                            </li>
                            <li>Please visit
                                <asp:HyperLink NavigateUrl="http://www.bup.edu.bd" Target="_blank" runat="server">BUP Official Website </asp:HyperLink>
                                for more information about Undergraduate and Graduate programs.
                            </li>
                        </ul>

                    </div>
                </div>


            </div>




            <br />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
