<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Declarationform.aspx.cs" Inherits="Admission.Admission.Candidate.Declarationform" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-sm-12 col-md-12 col-lg-12">
            <div class="panel panel-info">
                <div class="panel-heading  text-center" style="background-color: black">
                    <h4 style="color: #c4bfbf">Declaration</h4>
                </div>
                <div class="panel-body">
                   <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <div class="button" style="margin-top:27px;">
                                <asp:HyperLink ID="HyperLink1" style="border-radius:4px;" NavigateUrl="~/Admission/Candidate/upDocumentv2.aspx?ecat=4"
                                    runat="server" CssClass="btn btn-success">
                                        <span class="glyphicon glyphicon-chevron-left"></span> Preview &nbsp;
                                </asp:HyperLink>
                            </div>
                        </div>
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <div class="button" style="margin-top:27px;">
                                <asp:HyperLink ID="HyperLink2" style="margin-left:438px;border-radius:4px;" NavigateUrl="~/Admission/Candidate/upDocumentv2.aspx?ecat=4"
                                    runat="server" CssClass="btn btn-warning">Pay Online&nbsp;</asp:HyperLink>  
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
