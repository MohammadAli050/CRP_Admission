<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CandidateByPassPurchaseNotification.aspx.cs" Inherits="Admission.Admission.Office.CandidateByPass.CandidateByPassPurchaseNotification" %>





<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>
                        Name: &nbsp;<asp:Label ID="lblCandidateName" runat="server"></asp:Label>
                    </h4>
                </div>
                <div class="panel-body">
                    <table class="table table-condensed">
                        <tr class="success">
                            <td style="font-weight: bold; width: 20%">
                                <h4>Payment ID</h4>
                            </td>
                            <td style="font-weight: bolder; color: crimson">
                                <h4>
                                    <asp:Label ID="lblPaymentID" runat="server"></asp:Label>
                                </h4>
                            </td>
                        </tr>

                         <tr class="danger">
                            <td style="font-weight: bold; width: 20%">
                                <h4>User ID</h4>
                            </td>
                            <td style="font-weight: bolder; color: green">
                                <h4>
                                    <asp:Label ID="lblUserId" runat="server"></asp:Label>
                                </h4>
                            </td>
                        </tr>

                         <tr class="danger">
                            <td style="font-weight: bold; width: 20%">
                                <h4>Password</h4>
                            </td>
                            <td style="font-weight: bolder; color: green">
                                <h4>
                                    <asp:Label ID="lblPassword" runat="server"></asp:Label>
                                </h4>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <h5>Amount</h5>
                            </td>
                            <td>
                                <h5>
                                    <asp:Label ID="lblAmount" runat="server" Text="0 BDT"></asp:Label> <span> + Charge</span>
                                </h5>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h5>Faculty</h5>
                            </td>
                            <td>
                                <h5>
                                    <asp:Label ID="lblPrograms" runat="server"></asp:Label>
                                </h5>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="color: brown; font-size: 15pt; font-weight: bold;">
                                <asp:Label ID="lblNote" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>

                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:Button ID="btnSubmit_Bkash" runat="server" Text="Pay By BKash"
                                    CssClass="btn btn-primary" OnClick="btnSubmit_Bkash_Click" Visible="true"/> &nbsp;&nbsp;
                                <asp:Button ID="btnSubmit" runat="server" Text="Pay Now" Visible="true"
                                    CssClass="btn btn-primary" OnClick="btnSubmit_Click"/> &nbsp;&nbsp;
                                <%--<asp:Button ID="btnPayLater" runat="server" Text="Pay Later" Visible="true"
                                    CssClass="btn btn-primary" OnClick="btnSubmitPayLater_Click"/> &nbsp;&nbsp;--%>
                                <%--<asp:Button ID="btnSubmit_Fpg" runat="server" Text ="Pay By Foster" Visible="true"
                                    CssClass="btn btn-primary" OnClick="btnSubmit_Fpg_Click" />--%>
                            </td>
                        </tr>
                    </table>
                    
                </div>
            </div>
        </div>
    </div>

</asp:Content>

