<%@ Page Title="Admission Setup Of Other MSC Programs" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdmissionSetupOfOtherMSCPrograms.aspx.cs" Inherits="Admission.Admission.Office.AdmissionSetupOfOtherMSCPrograms" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <asp:UpdatePanel ID="updatePanelAll" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <h3>Admission Setup Of Other MSC Programs</h3>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-6 col-md-6 col-lg-6">
                            <asp:Panel ID="panelMasters" runat="server" >
                                <div class="form-group">
                                    <label><strong>Program <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                    <asp:DropDownList ID="ddlProgram" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                    <asp:CompareValidator ID="CompareValidator2" runat="server"
                                        ControlToValidate="ddlProgram" ErrorMessage="Required"
                                        Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                        ValueToCompare="-1" Operator="NotEqual" ValidationGroup="UploadDoc"></asp:CompareValidator>
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
                            <strong>File Upload: <span style="color: darkred;">(Only .pdf)</span></strong>
                            <asp:FileUpload ID="FileUploadDocument" runat="server" />
                            <asp:RegularExpressionValidator ID="rexvPhoto" runat="server" ControlToValidate="FileUploadDocument"
                                ErrorMessage="Only .pdf" Display="Dynamic" ForeColor="Crimson"
                                ValidationExpression="(.*\.([Pp][Dd][Ff])$)"></asp:RegularExpressionValidator>
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


                            <asp:Panel ID="listViewPanel" runat="server" style="overflow-x:scroll;overflow-y:scroll">

                                <asp:ListView ID="lvProgramPriority" runat="server"
                                    OnItemDataBound="lvProgramPriority_ItemDataBound"
                                    OnItemCommand="lvProgramPriority_ItemCommand"
                                    GroupItemCount="1"
                                    GroupPlaceholderID="groupPlaceHolder"
                                    ItemPlaceholderID="itemPlaceHolder">
                                    <LayoutTemplate>
                                        <table class="table table-hover table-condensed table-striped" style="width: 100%; text-align: left; margin-top: 10px">
                                            <asp:PlaceHolder runat="server" ID="groupPlaceHolder"></asp:PlaceHolder>
                                        </table>
                                    </LayoutTemplate>
                                    <GroupTemplate>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                    </GroupTemplate>
                                    <ItemTemplate>
                                        <%# AddGroupingHeader() %>
                                        <tr runat="server">
                                            <td valign="middle" align="left" class="">
                                                <asp:Label ID="lblSerial" runat="server" />
                                            </td>
                                            <td valign="middle" align="left" class="">
                                                <asp:Label ID="lblFileTypeName" runat="server" />
                                            </td>
                                            <td valign="middle" align="right" class="">
                                                <asp:HyperLink ID="hlBtn" runat="server"></asp:HyperLink>
                                            </td>
                                            <td valign="middle" align="right" class="">
                                                <asp:LinkButton CssClass="btn btn-danger" ID="lbDelete" runat="server">Delete</asp:LinkButton>
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
