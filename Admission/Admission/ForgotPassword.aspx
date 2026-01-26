<%@ Page Title="Forgot Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Admission.Admission.ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <style type="text/css">
        .imageBtn {
            margin-top: 8%;
        }

        .table_form td {
            padding-top: 3px;
            padding-bottom: 3px;
            padding-left: 1px;
            padding-right: 1px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-6 col-md-offset-3">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Retrieve Password <span style="color:crimson"> (For International Students Type Only Email Address)</span></strong>
                </div>
                <div class="panel-body">
                    
                    <asp:Panel ID="messagePanel" runat="server" Visible="false" >
                        <%--<div class="alert alert-danger" role="alert">--%>
                        <asp:Label ID="lblMessage" runat="server" Text="" Visible="true"></asp:Label>
                        <%--</div>--%>
                    </asp:Panel>

                    <table style="width: 100%; text-align: left"
                        class="table_form">
                        <tr>
                            <td style="width: 28%; font-weight: bold">Payment ID:</td>
                            <td style="width: 72%">
                                <asp:TextBox ID="txtPaymentID" runat="server" CssClass="form-control" ></asp:TextBox>
                               <%-- <asp:RequiredFieldValidator ID="paymentReq" runat="server" Font-Size="9pt"
                                    ControlToValidate="txtPaymentID" ErrorMessage="Payment ID required."
                                    Display="Dynamic" ValidationGroup="gr1" ForeColor="Crimson"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">Candidate Email:</td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="emailReq" runat="server" Font-Size="9pt"
                                    ControlToValidate="txtEmail" ErrorMessage="Email required."
                                    Display="Dynamic" ValidationGroup="gr1" ForeColor="Crimson"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">Candidate SMS Phone:</td>
                            <td>
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control"></asp:TextBox>
                               <%-- <asp:RequiredFieldValidator ID="phoneReq" runat="server" Font-Size="9pt"
                                    ControlToValidate="txtEmail" ErrorMessage="SMS Phone required."
                                    Display="Dynamic" ValidationGroup="gr1" ForeColor="Crimson"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">Candidate Date of Birth:</td>
                            <td>
                                <asp:TextBox ID="txtDob" runat="server" CssClass="form-control"
                                    Placeholder="dd/mm/yyyy"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="ceTxtDob" runat="server" TargetControlID="txtDob"
                                    Format="dd/MM/yyyy" />
                                <%--<asp:RequiredFieldValidator ID="dobReq" runat="server" Font-Size="9pt"
                                    ControlToValidate="txtEmail" ErrorMessage="Date of Birth required."
                                    Display="Dynamic" ValidationGroup="gr1" ForeColor="Crimson"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ImageButton ID="btnReLoadCaptcha" runat="server"
                                    Height="30" Width="30" ToolTip="Reload captcha"
                                    CssClass="float-right imageBtn"
                                    ImageUrl="~/Images/AppImg/reload6.png"
                                    OnClick="btnReLoadCaptcha_Click" />
                            </td>
                            <td>
                                <asp:UpdatePanel ID="updatePanel1" runat="server">
                                    <ContentTemplate>
                                        <img runat="server" id="imgCtrl" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger
                                            ControlID="btnReLoadCaptcha"
                                            EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">Enter Captcha: 
                            </td>
                            <td>
                                <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="captchaReq" runat="server" Font-Size="9pt"
                                    ControlToValidate="txtCaptcha" ErrorMessage="Captcha required."
                                    Display="Dynamic" ValidationGroup="gr1" ForeColor="Crimson"></asp:RequiredFieldValidator>
                                <asp:Panel ID="captchaMsg" runat="server" Visible="false">
                                    <asp:Label ID="lblCaptcha" runat="server"
                                        CssClass="text-warning"
                                        Text="Sorry your text and image didn't match. Please try again."></asp:Label>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td  style="text-align: center">
                                <asp:Button ID="btnSubmit" runat="server" Text="SUBMIT" ValidationGroup="gr1"
                                    Width="100%" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                            </td>
                        </tr>
                    </table>

                </div>
            </div>
        </div>
    </div>

</asp:Content>
