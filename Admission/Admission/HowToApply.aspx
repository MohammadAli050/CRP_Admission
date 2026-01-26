<%@ Page Title="How To Apply" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HowToApply.aspx.cs" Inherits="Admission.Admission.HowToApply" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%--    <div class="row">
        <h4>Coming soon.</h4>
    </div>--%>

    <div class="row" style="display: none;">
        <div class="col-md-12">
            <asp:Repeater ID="lvInsHta" runat="server" Visible="false">
                <ItemTemplate>
                    <table id="tbl" class="table table-responsive table-bordered" style="width: 100%">
                        <tr>
                            <th class="active"><%# Eval("Title_ENG") %></th>
                        </tr>
                        <tr>
                            <td><%# Eval("Description_ENG") %></td>
                        </tr>
                        <tr>
                            <th class="active"><%# Eval("Title_BEN") %></th>
                        </tr>
                        <tr>
                            <td><%# Eval("Description_BEN") %></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>

        </div>
    </div>

    <div>
        <iframe src="../ApplicationDocs/ApplicationSubmissionProces.pdf" width="100%" height="750px">This browser does not support PDFs. Please download the PDF to view it: <a href="../ApplicationDocs/ApplicationSubmissionProcess.pdf">Download PDF</a>
        </iframe>
    </div>


    <%--  <div class="row">
        <div class="col-sm-12 col-md-12 col-lg-12 text-center">
            <div>
                <iframe width="560" height="315" src="https://www.youtube.com/embed/TpGPec0cJL0" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
            </div>
        </div>
    </div> --%>
</asp:Content>
