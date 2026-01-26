<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PostgraduateDiplomaPurchaseNotification.aspx.cs" Inherits="Admission.Admission.PostgraduateDiploma.PostgraduateDiplomaPurchaseNotification" %>



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
                        <tr style="display: none;">
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
                                <%--<asp:Button ID="btnSubmit" runat="server" Text="Pay Now" Visible="true"
                                    CssClass="btn btn-primary" OnClick="btnSubmit_Click"/> &nbsp;&nbsp;
                                <asp:Button ID="btnPayLater" runat="server" Text="Pay Later" Visible="true"
                                    CssClass="btn btn-primary" OnClick="btnSubmitPayLater_Click"/> &nbsp;&nbsp;--%>
                                <asp:Button ID="btnPayLater" runat="server" Text="Go to Home" Visible="true"
                                    CssClass="btn btn-primary" OnClick="btnSubmitPayLater_Click"/> &nbsp;&nbsp;
                            </td>
                        </tr>
                    </table>
                    
                </div>
            </div>
        </div>
    </div>

</asp:Content>

