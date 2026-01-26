<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true"
    CodeBehind="Institute.aspx.cs" Inherits="Admission.Admission.Admin.Institute" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <style type="text/css">
        .labelColumn {
            width: 12%;
        }

        .controlColumn {
            width: 38%;
            padding-bottom: 3px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row body-content">

        <div class="row">
            <div class="col-md-12">
                <asp:UpdatePanel ID="updatePanelMsg" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="row">
            <div class="col-md-10 col-md-offset-1">
                <div class="panel panel-default">
                    <div class="panel-heading">Institute</div>
                    <div class="panel-body">
                        <table style="width: 100%;" class="table table-condensed">
                            <tr>
                                <td class="labelColumn">Institute Name</td>
                                <td class="controlColumn">
                                    <asp:TextBox ID="txtInstituteName" runat="server" Width="100%"></asp:TextBox>
                                </td>
                                <td class="labelColumn">Short Name:</td>
                                <td class="controlColumn">
                                    <asp:TextBox ID="txtShortName" runat="server" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top">Address</td>
                                <td class="controlColumn">
                                    <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="4"
                                        Width="100%" ></asp:TextBox>
                                </td>
                                <td>Post Code</td>
                                <td>
                                    <asp:TextBox ID="txtPostCode" runat="server" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Telephone 1</td>
                                <td class="controlColumn">
                                    <asp:TextBox ID="txtTelephone1" runat="server" Width="100%"></asp:TextBox>
                                </td>
                                <td>Telephone 2</td>
                                <td>
                                    <asp:TextBox ID="txtTelephone2" runat="server" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Fax</td>
                                <td class="controlColumn">
                                    <asp:TextBox ID="txtFax" runat="server" Width="100%"></asp:TextBox>
                                </td>
                                <td>Mobile</td>
                                <td>
                                    <asp:TextBox ID="txtMobile" runat="server" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Email</td>
                                <td class="controlColumn">
                                    <asp:TextBox ID="txtEmail" runat="server" Width="100%" TextMode="Email"></asp:TextBox>
                                </td>
                                <td>Website</td>
                                <td>
                                    <asp:TextBox ID="txtWebsite" runat="server" Width="100%" TextMode="Url"></asp:TextBox>
                                </td>
                            </tr>
                            <%--<tr>
                                <td>Logo</td>
                                <td>
                                    <asp:FileUpload ID="fileUploadLogo" runat="server" />
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Banner</td>
                                <td>
                                    <asp:FileUpload ID="fileUploadBanner" runat="server" />
                                </td>
                                <td></td>
                                <td></td>
                            </tr>--%>
                            <tr>
                                <td style="padding-top: 8px;">
                                    <asp:Button ID="btnSave" runat="server" Text="Save"
                                        CssClass="btn btn-info" OnClick="btnSave_Click"/>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <%-- -------------------------------------------------------------------------------------------------------------------- --%>
        <div class="row">
            <div class="col-md-10 col-md-offset-1">
                <asp:UpdatePanel ID="updatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:ListView ID="lvInstituteList" runat="server"
                            OnItemDataBound="lvInstituteList_ItemDataBound"
                            OnItemDeleting="lvInstituteList_ItemDeleting"
                            OnItemUpdating="lvInstituteList_ItemUpdating"
                            OnItemCommand="lvInstituteList_ItemCommand">
                            <LayoutTemplate>
                                <table runat="server" id="tblInstituteList" style="width:100%; text-align: left; font-size: 9pt;"
                                     class="table table-hover">
                                    <tr runat="server" style="background-color: dimgrey">
                                        <th runat="server">Name</th>
                                        <th runat="server">Short Name</th>
                                        <th runat="server">Address</th>
                                        <th runat="server">Post Code</th>
                                        <th runat="server">Telephone1</th>
                                        <th runat="server">Telephone2</th>
                                        <th runat="server">Fax</th>
                                        <th runat="server">Mobile</th>
                                        <th></th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceHolder"></tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr runat="server">
                                    <td style="vertical-align: middle; text-align:left" class="">
                                        <asp:Label ID="lblName" runat="server"></asp:Label>
                                    </td>
                                    <td style="vertical-align: middle; text-align:left" class="">
                                        <asp:Label ID="lblShortName" runat="server"></asp:Label>
                                    </td>
                                    <td style="vertical-align: middle; text-align:left" class="">
                                        <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                    </td>
                                    <td style="vertical-align: middle; text-align:left" class="">
                                        <asp:Label ID="lblPostCode" runat="server"></asp:Label>
                                    </td>
                                    <td style="vertical-align: middle; text-align:left" class="">
                                        <asp:Label ID="lblTel1" runat="server"></asp:Label>
                                    </td>
                                    <td style="vertical-align: middle; text-align:left" class="">
                                        <asp:Label ID="lblTel2" runat="server"></asp:Label>
                                    </td>
                                    <td style="vertical-align: middle; text-align:left" class="">
                                        <asp:Label ID="lblFax" runat="server"></asp:Label>
                                    </td>
                                    <td style="vertical-align: middle; text-align:left" class="">
                                        <asp:Label ID="lblMobile" runat="server"></asp:Label>
                                    </td>
                                    <td style="vertical-align: middle; text-align:left" class="">
                                        <asp:LinkButton ID="lnkUpload" runat="server">Upload</asp:LinkButton> | 
                                        <asp:LinkButton ID="lnkEdit" runat="server">Edit</asp:LinkButton> | 
                                        <asp:LinkButton ID="lnkDelete" runat="server">Delete</asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <div class="alert alert-warning" role="alert">
                                    No Item to display.
                                </div>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </ContentTemplate>
                    <%--<Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                    </Triggers>--%>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

</asp:Content>
