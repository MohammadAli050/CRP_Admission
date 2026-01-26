<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PaymentCheckInterface.aspx.cs" Inherits="Admission.Admission.Office.Reports.PaymentCheckInterface" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>Unpaid Student List</strong>
                </div>
                <div class="panel-body">
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>

                    <table class="table_form">

                        <tr>

                            <td>Upload File (Excel)<asp:FileUpload runat="server" ID="fuExcel" />
                                &nbsp; &nbsp;
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnUpload" Text="Upload" ValidationGroup="gr1"
                                    OnClick="btnUpload_Click" />
                            </td>
                        </tr>


                    </table>
                    <asp:Panel ID="PnlRegisteredCourse" runat="server" Width="100%" Wrap="False">
                        <div class=" panel panel-default pp" style="">
                            <div class="table-responsive " style="border-radius: 8px; overflow: scroll">
                                <asp:GridView ID="gvRegisteredCourse" runat="server" AutoGenerateColumns="False" TabIndex="6" CssClass="table-bordered"
                                    Width="100%" CellPadding="4" ShowFooter="True" ForeColor="#333333" GridLines="None">
                                    <FooterStyle BackColor="#434352" ForeColor="White" Height="30" Font-Bold="True" />
                                    <HeaderStyle BackColor="#101257" Font-Bold="True" ForeColor="White" Height="40" />
                                    <AlternatingRowStyle BackColor="#BBD6E6" />
                                    <%--<PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle Height="25px" BackColor="#E3EAEB" />--%>
                                    <Columns>
                                        <asp:TemplateField HeaderText="SL" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <b><%# Container.DataItemIndex + 1 %></b>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="Term" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate><asp:Label runat="server" ID="lblCourseStatus" Font-Bold="True" Text='<%#Eval("SemesterNo") %>' /></ItemTemplate>
                                </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="PaymentId" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblPaymentId" Font-Bold="True" Text='<%#Eval("PaymentId") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                       
                                        <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblAmount" Font-Bold="True" Text='<%#Eval("Amount") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                    </Columns>
                                    <EditRowStyle BackColor="#7C6F57" />
                                    <EmptyDataTemplate>
                                        <b>No Data Found !</b>
                                    </EmptyDataTemplate>
                                    <%-- <RowStyle CssClass="rowCss" />
                            <HeaderStyle CssClass="tableHead" />--%>
                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>




</asp:Content>

