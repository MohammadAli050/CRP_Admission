<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="PaymentUpdateV2.aspx.cs" Inherits="Admission.Admission.Office.Reports.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../../Content/formStyle.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            width: 15%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong style="color: tomato">Payment Update</strong>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-3 col-md-3 col-sm-3">
                                    <asp:FileUpload runat="server" ID="fuExcel" Width="100%" CssClass="btn btn-primary" />

                                </div>
                                <div class="col-lg-3 col-md-3 col-sm-3">
                                    <asp:Button runat="server" ID="btnUpload" Width="50%" CssClass="btn btn-info" Text="Upload" ValidationGroup="gr1"
                                        OnClick="btnUpload_Click" />
                                </div>
                                <div class="col-lg-2 col-md-2 col-sm-2">
                                    <asp:Button runat="server" ID="btnDownload" Width="50%" CssClass="btn btn-success" Text="View" OnClick="btnDownload_Click" />
                                </div>

                            </div>                           
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnUpload" />
                        </Triggers>
                    </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
