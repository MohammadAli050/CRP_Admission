<%@ Page Title="Admission Setup Of MPhil And PhD Programs" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdmissionSetupOfMPhilAndPhdPrograms.aspx.cs" Inherits="Admission.Admission.Office.AdmissionSetupOfMPhilAndPhdPrograms" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    
    <asp:UpdatePanel ID="updatePanelAll" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <h3>Admission Setup Of MPhil And PhD Programs</h3>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <asp:Panel ID="panelMasters" runat="server" >
                                <div class="form-group">
                                    <label><strong>Title <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="txtPlaceOfBirth_ReqV" runat="server"
                                    ControlToValidate="txtTitle"
                                    ErrorMessage="Required"
                                    ForeColor="Crimson"
                                    Display="Dynamic"
                                    Font-Size="9pt"
                                    Font-Bold="true"
                                    ValidationGroup="UploadDoc">
                                </asp:RequiredFieldValidator>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <div class="form-group">
                                <label><strong>File Type <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                <asp:DropDownList ID="ddlFileType" runat="server" CssClass="form-control" Width="100%">
                                    <asp:ListItem Value="-1">-Select-</asp:ListItem>
                                    <asp:ListItem Value="1">Notice</asp:ListItem>
                                    <asp:ListItem Value="2">Form</asp:ListItem>
                                    <asp:ListItem Value="3">Preferred Fields or Areas of Research</asp:ListItem>
                                </asp:DropDownList>
                                <asp:CompareValidator ID="CompareValidator1" runat="server"
                                        ControlToValidate="ddlFileType" ErrorMessage="Required"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual" ValidationGroup="UploadDoc"></asp:CompareValidator>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <strong>File Upload: <span style="color: darkred;">(Docx,Pdf)</span></strong>
                            <asp:FileUpload ID="FileUploadDocument" runat="server" />
                            <%--<asp:RegularExpressionValidator ID="rexvPhoto" runat="server" ControlToValidate="FileUploadDocument"
                                ErrorMessage="Docx,Pdf" Display="Dynamic" ForeColor="Crimson"
                                ></asp:RegularExpressionValidator>--%>
                            <%--ValidationExpression="(.*\.([Pp][Dd][Ff])$)"--%>
                        </div>
                        <div class="col-sm-6 col-md-6 col-lg-6">

                            <asp:Button ID="btnUploadFile" Text="Upload" runat="server"
                                OnClick="btnUploadFile_Click"
                                CssClass="btn btn-default"
                                Style="width: 185px; margin-top: 5px;"
                                ValidationGroup="UploadDoc" />

                        </div>
                    </div>


                </div>
            </div>


            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-info">
                        <div class="panel-body">


                            <asp:Panel ID="listViewPanel" runat="server" style="overflow-x:scroll; overflow-y:scroll">

                                <asp:ListView ID="lvAdmSetup" runat="server"
                                    OnItemDataBound="lvAdmSetup_ItemDataBound"
                                    OnItemCommand="lvAdmSetup_ItemCommand"
                                    OnItemDeleting="lvAdmSetup_ItemDeleting"
                                    OnItemUpdating="lvAdmSetup_ItemUpdating">
                                    <LayoutTemplate>
                                        <table id="tbl"
                                            class="table table-hover table-condensed table-striped table-bordered"
                                            style="width: 100%;">
                                            <tr runat="server" style="background-color: #1387de; color: white; font-size: small">
                                                <th runat="server" style="text-align: center">SL#</th>
                                                <th runat="server">Title</th>
                                                <th runat="server" style="text-align: center">File Type</th>
                                                <th runat="server" style="text-align: center">
                                                    Serial No <br />
                                                    <asp:Button ID="btnSaveSerialNo" runat="server" Text="Save" CssClass="btn btn-default btn-sm"
                                                        style="padding: 2px 20px;border-radius: 11px;" 
                                                        OnClick="btnSaveSerialNo_Click"/>
                                                </th>
                                                <th runat="server" style="text-align: center">View</th>
                                                <th runat="server" style="text-align: center">Delete</th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder" />
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr runat="server" style="font-size: small">
                                            <td valign="middle" align="middle" class="">
                                                <asp:Label ID="lblSerial" runat="server" />
                                                <asp:Label ID="lblId" runat="server" Visible="false" />
                                            </td>
                                            <td valign="middle" align="left" class="">
                                                <asp:Label ID="lblTitle" runat="server" />
                                            </td>
                                            <td valign="middle" align="middle" class="">
                                                <asp:Label ID="lblFileType" runat="server" />
                                            </td>
                                            <td valign="middle" align="middle" class="">
                                                <asp:DropDownList ID="ddlSerialNo" runat="server"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlSerialNo_SelectedIndexChanged"></asp:DropDownList>
                                                <br />
                                                <asp:Label ID="lblDDLMessage" runat="server" Text=""/>
                                            </td>
                                            <td valign="middle" align="middle" class="">
                                                <asp:HyperLink ID="hlViewFile" runat="server"></asp:HyperLink>
                                            </td>
                                            <td valign="middle" align="middle" class="">
                                                <asp:LinkButton ID="lnkDelete" runat="server" CssClass="btn btn-danger" >Delete</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <div class="alert alert-warning" role="alert" style="text-align: center">No item to display.</div>
                                    </EmptyDataTemplate>
                                </asp:ListView>


                            </asp:Panel>


                        </div>
                    </div>
                </div>
            </div>




        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUploadFile" />
        </Triggers>
    </asp:UpdatePanel>



</asp:Content>
