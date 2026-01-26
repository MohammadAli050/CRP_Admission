<%@ Page Title="Change User Password" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="UserPasswordChange.aspx.cs" Inherits="Admission.Admission.Office.UserPasswordChange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../Content/formStyle.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-6">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Change Password</strong>
                </div>
                <div class="panel-body">
                    <table class="table_form table_fullwidth">
                        <tr>
                            <td class="style_td" style="width: 25%">Username
                            </td>
                            <td style="width: 75%">
                                <asp:Label ID="lblUserName" runat="server" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Old Password <span class="asteriskColor">*</span></td>
                            <td>
                                <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password" required="required" Width="50%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">New Password <span class="asteriskColor">*</span></td>
                            <td>
                                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" required="required" Width="50%"></asp:TextBox><br />
                                <asp:RegularExpressionValidator runat="server" ID="mobileReg"
                                    ValidationGroup="SUBMIT"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Bold="False"
                                    ErrorMessage="Password must be at least 4 characters, no more than 12 characters, and must include at least one upper case letter, one lower case letter, and one numeric digit."
                                    ControlToValidate="txtNewPassword"
                                    ValidationExpression="^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{4,12}$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Retype Password <span class="asteriskColor">*</span></td>
                            <td>
                                <asp:TextBox ID="txtNewPasswordRetype" runat="server" TextMode="Password" required="required" Width="50%"></asp:TextBox>
                                <%--<asp:CompareValidator ID="passwordComV" runat="server" 
                                    ControlToCompare="txtNewPassword"
                                    ControlToValidate="txtNewPasswordRetype" 
                                    Operator="Equal" 
                                    ErrorMessage="Password does not match."
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    ValidationGroup="SUBMIT"></asp:CompareValidator>--%>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                    <asp:Button ID="btnSubmit" runat="server" ValidationGroup="SUBMIT" Text="Update"
                        OnClick="btnSubmit_Click"/>
                    <br /><br />
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
