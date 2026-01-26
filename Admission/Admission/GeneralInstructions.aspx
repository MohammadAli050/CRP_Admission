<%@ Page Title="General Instructions" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GeneralInstructions.aspx.cs" Inherits="Admission.Admission.GeneralInstructions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            
            <asp:Repeater ID="lvInsR" runat="server" Visible="false">
                <ItemTemplate>
                    <table id="tbl" class="table table-responsive table-bordered" style="width: 100%">
                        <tr>
                            <th class="active" style="width: 50%"><%# Eval("Title") %></th>
                            <th class="active" style="width: 50%"><%# Eval("Title_BEN") %></th>
                        </tr>
                        <tr>
                            <td><%# Eval("Description") %></td>
                            <td><%# Eval("Description_BEN") %></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>
            <div>
                <iframe src="../ApplicationDocs/Payment_Procedure.pdf" width="100%" height="750px">This browser does not support PDFs. Please download the PDF to view it: <a href="../ApplicationDocs/Payment_Procedure.pdf">Download PDF</a>
                </iframe>
            </div>
        </div>
    </div>


</asp:Content>
