<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForeignCandidateHome.aspx.cs" Inherits="Admission.Admission.Candidate.ForeignCandidateHome" %>




<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <script src="../../Scripts/jquery-3.1.1.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(document).ready(function () {
                $('.fadeInDiv1').fadeIn(250);
            });
            $(document).ready(function () {
                $('.fadeInDiv2').delay(200).fadeIn(250);
            });
            $(document).ready(function () {
                $('.fadeInDiv3').delay(200).fadeIn(250);
            });
        });
    </script>

    <style>
        .blink_me {
            animation: blinker 1s linear infinite;
        }

        @keyframes blinker {
            50% {
                opacity: 0;
            }
        }

        
        .modalBackground {
            background-color: #2a2d2a;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
            <div class="row col-lg-12 col-md-12 col-sm-12">
                <br />
                <div class="col-lg-3 col-md-3 col-sm-3">
                    <asp:Button ID="btnApplicationForm" runat="server" Text="Fill Up Application Form" CssClass="btn btn-primary btn-lg btn-block" OnClick="btnApplicationForm_Click"
                        Font-Size="11" Font-Bold="true" />
                </div>

                <div class="col-lg-6 col-md-6 col-sm-6">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <strong>Basic Info</strong>
                        </div>
                        <div class="panel-body">

                            <div class="row">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    Name : 
                                </div>
                                <div class="col-lg-8 col-md-8 col-sm-8">
                                    <asp:Label ID="lblCandidateName" runat="server"></asp:Label>

                                </div>
                            </div>

                            <div class="row" style="margin-top: 5px">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    Email Address :
                                </div>
                                <div class="col-lg-8 col-md-8 col-sm-8">
                                    <asp:Label ID="lblEmail" runat="server"></asp:Label>

                                </div>
                            </div>

                            <div class="row" style="margin-top: 5px">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    Final Submit :
                                </div>
                                <div class="col-lg-8 col-md-8 col-sm-8">
                                    <asp:Label ID="lblFinalSubmit" runat="server"></asp:Label>

                                </div>
                            </div>

                        </div>
                    </div>

                </div>

                <div class="col-lg-3 col-md-3 col-sm-3">
                    <div class="panel panel-info">
                        <div class="panel-heading">
                            <strong>Candidate Photo</strong>
                            <br />
                            <img runat="server" id="imgCtrl" src="" height="155" width="145" alt="Please upload your photo."
                                style="font-size: x-small; color: crimson; text-align: justify;" />
                        </div>
                    </div>
                </div>

            </div>


            <div class="panel panel-default" id="DivProgramAdd" runat="server">
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
                                       <b><%# Container.DataItemIndex + 1 %></b>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Program Name" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblName" Text='<%#Eval("ProgramName") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
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

                            <asp:TemplateField HeaderText="Payment Status" HeaderStyle-CssClass="header-center">
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:Label runat="server" ID="lblPaymentStatus" Text='<%#Eval("PaymentStatus") %>' ForeColor="Black" Font-Bold="false"></asp:Label>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="File Information" HeaderStyle-CssClass="header-center">
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

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div style="padding: 5px; text-align: center">
                                        <asp:LinkButton ID="lnkRemove" runat="server" CssClass="btn-danger btn-sm" CommandArgument='<%# Eval("ProgramPriorityId") %>' Visible='<%# Eval("PaymentStatus")==null ||Eval("PaymentStatus").ToString()==""?true:false  %>' OnClick="lnkRemove_Click" OnClientClick="return confirm('Are you sure you want to delete?');">Remove</asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                        </Columns>
                        <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" />

                    </asp:GridView>

                    <br />

                    <div class="panel-body">

                        <h3><b>Add More Program Bellow: </b> (For each added program, you have to pay 25$)</h3>
                        <br />
                        <div class="row">
                            <div class="col-sm-6 col-md-6 col-lg-6">
                                <div class="form-group">
                                    <label>Session<span class="spanAsterisk">*</span></label>
                                    <asp:DropDownList ID="ddlSession" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:CompareValidator ID="ddlSessionCompare" runat="server"
                                        ControlToValidate="ddlSession" ErrorMessage="required"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-6 col-md-6 col-lg-6">
                                <div class="form-group">
                                    <label>Name of Desired Faculty<span class="spanAsterisk">*</span></label>
                                    <asp:DropDownList ID="ddlAdmissionUnit" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlAdmissionUnit_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:CompareValidator ID="ddlAdmissionUnitCompare" runat="server"
                                        ControlToValidate="ddlAdmissionUnit" ErrorMessage="required"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-6 col-md-6 col-lg-6">
                                <div class="form-group">
                                    <label>Name of Desired Program<span class="spanAsterisk">*</span></label>
                                    <asp:DropDownList ID="ddlProgram" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:CompareValidator ID="ddlProgramCompare" runat="server"
                                        ControlToValidate="ddlProgram" ErrorMessage="required"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual" ValidationGroup="SUBMIT"></asp:CompareValidator>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <div class="form-group">

                                    <asp:Button ID="btnAddProgram" ValidationGroup="SUBMIT" Style="border-radius: 4px;" runat="server" Text="Add Program" OnClick="btnAddProgram_Click" CssClass="btn btn-primary"></asp:Button>


                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>


        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="btndeposite" />
        </Triggers>

    </asp:UpdatePanel>

    
    <div class="col-md-15 col-lg-12">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>

                <asp:Button ID="Button3" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Button3" PopupControlID="Panel3"
                    BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="Panel3" Style="display: none; padding: 5px;" BackColor="White" Width="40%">


                    <div class="panel panel-default">
                        <div class="panel-body">

                            <div class="row col-lg-12 col-md-12 col-sm-12" style="text-align: center; font-weight: bold">
                             
                                <asp:Label ID="lblNotGivenField" runat="server" Text="Program added. Please complete payment(Bank Transfer) and update payment information." ForeColor="Blue"></asp:Label>
                            </div>
                            <hr />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button runat="server" ID="Button5" Text="Ok" OnClick="Button5_Click" CssClass="btn-danger btn-sm" Style="display: inline-block; width: 100%; height: 38px; text-align: center; font-size: 17px;" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                        </div>
                    </div>


                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>



