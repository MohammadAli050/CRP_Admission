<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PaymentInformation.aspx.cs" Inherits="Admission.Admission.Candidate.PaymentInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .header-center {
            padding: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-sm-12 col-md-12 col-lg-12">
            <div class="panel panel-info">
                <div class="panel-heading  text-center" style="background-color: black">
                    <h4 style="color: #c4bfbf">Program Wise Payment Information</h4>
                </div>




                <div class="panel-body">


                    <div class="row" id="divPayment" runat="server" style="margin-left: 5px">

                        <div class="row">
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <h4 style="text-align: left; font-weight: bold;">BUP Foreign Account Details:</h4>
                                <ul>
                                    <li>Account Title: <b>BUP General Fund</b></li>
                                    <li>Account Number: <b>0028-0320000091</b></li>
                                    <li>Branch: <b>Mirpur</b></li>
                                    <li>Bank Name: <b>Trust Bank Limited</b></li>
                                    <li>Routing Number: <b>240262987</b></li>
                                    <li>SWIFT: <b>TTBLBDDH0028</b></li>
                                </ul>
                            </div>
                        </div>
                        <br />
                        <asp:HiddenField ID="hdnProgramPriorityId" runat="server" />

                        <div class="row">
                            <div class="col-lg-8 col-md-8 col-sm-8">
                                <div class="form-group">
                                    <label>Program Name <span style="color: #ff6c00; font-size: 8pt; font-weight: bold;"></span></label>
                                    <asp:TextBox ID="txtProgram" runat="server" CssClass="form-control" placeholder="Program Name" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-lg-8 col-md-8 col-sm-8">
                                <div class="form-group">
                                    <label>Bank Name <span style="color: #ff6c00; font-size: 8pt; font-weight: bold;"></span></label>
                                    <asp:TextBox ID="BankName" runat="server" CssClass="form-control" placeholder="Bank Name"></asp:TextBox>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-lg-8 col-md-8 col-sm-8">
                                <div class="form-group">

                                    <label>Transition ID <span style="color: #ff6c00; font-size: 8pt; font-weight: bold;"></span></label>
                                    <asp:TextBox ID="TransID" runat="server" CssClass="form-control" placeholder="Transition ID"></asp:TextBox>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-lg-8 col-md-8 col-sm-8">
                                <div class="form-group">

                                    <label>Payment Date</label>
                                    <asp:TextBox ID="PaymentDate" runat="server" Width="100%" CssClass="form-control" placeholder="dd/MM/yyyy"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalenderExtender_DOB" runat="server"
                                        TargetControlID="PaymentDate" Format="dd/MM/yyyy" />


                                    <span id="txtDateOfPaymentValidateMassage" runat="server" style="font-weight: bold; color: crimson;"></span>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-lg-8 col-md-8 col-sm-8">
                                <p style="font-size: 15px; font-weight: bold;">Upload Application fee deposit slip(PDF, JPEG)</p>
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <asp:FileUpload ID="FileUploadDeposite" runat="server" accept=".jpg,.jpeg,.pdf" CssClass="btn btn-secondary" Width="100%" Style="margin-bottom: 5px; border-radius: 5px; background-color: gainsboro" ClientIDMode="Static" />

                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <asp:Button ID="btndeposite" runat="server" CssClass="btn btn-info" Width="100%" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btndeposite_Click" Text="Update Information"
                                        ClientIDMode="Static" CausesValidation="false"></asp:Button>
                                </div>
                                <div class="col-lg-2 col-md-2 col-sm-2">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Width="100%" Style="display: inline-block; text-align: center; font-size: 14px; border-radius: 4px;" Font-Bold="true" OnClick="btnCancel_Click" Text="Cancel"></asp:Button>
                                </div>
                            </div>
                        </div>

                    </div>


                    <br />

                    <asp:GridView runat="server" ID="GridViewProgramList" AutoGenerateColumns="False" AllowPaging="false" GridLines="None"
                        PagerSettings-Mode="NumericFirstLast" Width="100%"
                        PagerStyle-Font-Bold="true" PagerStyle-Font-Size="Larger"
                        ShowHeader="true">
                        <HeaderStyle BackColor="#91CDE0" ForeColor="Black" />
                        <RowStyle BackColor="#ecf0f0" />
                        <AlternatingRowStyle BackColor="#ffffff" />
                        <Columns>

                            <asp:TemplateField HeaderText="SL" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblSL" Text='<%#Eval("SL") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Program Name" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblName" Text='<%#Eval("ProgramName") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Priority" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblPriority" Text='<%#Eval("Priority") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Session" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblSession" Text='<%#Eval("Session") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblAmount" Text='<%#Eval("Amount") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Payment Status" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblPaymentStatus" Text='<%#Eval("PaymentStatus") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Deposit Slip" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:HyperLink runat="server" CssClass="btn btn-primary" Text="View" ID="lblFileView" Font-Bold="true" ForeColor="White" Font-Underline="true" Font-Size="14px" Style="border-radius: 4px;" Target="_blank" NavigateUrl='<%#Eval("FilePath") %>' Visible='<%#Eval("FilePath")==null ? false : true %>'> Deposit Slip </asp:HyperLink>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Bank Name" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblBankName" Text='<%#Eval("BankName") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Transaction Id" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblTransactionId" Text='<%#Eval("TransactionId") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Payment Date" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblDate" Text='<%#Eval("PaymentDate") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div style="padding: 5px; text-align: center">
                                        <asp:LinkButton ID="lnkUpdate" runat="server" CssClass="btn-info btn-sm" CommandArgument='<%# Eval("ProgramPriorityId") %>' OnClick="lnkUpdate_Click">Update</asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <%--                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div style="padding: 5px; text-align: center">
                                                <asp:LinkButton ID="lnkRemove" runat="server" CssClass="btn-danger btn-sm" CommandArgument='<%# Eval("ID") %>' OnClick="lnkRemove_Click" OnClientClick="return confirm('Are you sure you want to delete?');">Remove</asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>--%>
                        </Columns>
                        <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" />

                    </asp:GridView>




                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <div class="button" style="margin-top: 27px;">
                                <asp:HyperLink ID="HyperLink1" Style="border-radius: 4px; background-color: #000000a6; color: white;" NavigateUrl="~/Admission/Candidate/foreignStudentApplicationDeclaration.aspx?ecat=4"
                                    runat="server" CssClass="btn btn-light">
                                        <span class="glyphicon glyphicon-chevron-left"></span> Back &nbsp;
                                </asp:HyperLink>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

        </div>
    </div>






</asp:Content>
