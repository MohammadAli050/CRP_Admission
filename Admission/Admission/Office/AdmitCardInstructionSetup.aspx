<%@ Page Title="AdmitCard Instruction Setup" Language="C#"
    MasterPageFile="~/SiteAdmin.Master"
    AutoEventWireup="true"
    CodeBehind="AdmitCardInstructionSetup.aspx.cs"
    Inherits="Admission.Admission.Office.AdmitCardInstructionSetup"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <script src="../../Scripts/jquery-3.1.1.min.js"></script>
    <script src="../../Content/CKEditor/ckeditor.js"></script>

    <style>
        input[type="checkbox"] {
            width: 20px;
            height: 20px;
        }
    </style>

    <script>

        function openCollapse() {
            //$('#collapse1').removeClass("panel-collapse collapse");
            //$('#collapse1').addClass("panel-collapse collapse in");
            ////aria-expanded="false"
            //$('#collapse1').removeAttr("aria-expanded");
            //$('#collapse1').attr("aria-expanded", "true");

            //alert("HEllo");
            $("#clickCollaps").click();
        }

        //$(document).ready(function () {
        //    assignDataInCollapse();
        //});

        function assignCKEditorInTextField() {
            //alert(dataT);
            //console.log(dataT);

            //$("#MainContent_txtNoticeDetails").html(dataT);

            CKEDITOR.replace('<%=txtNoticeDetails.ClientID%>');
        }

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%--    <asp:UpdatePanel ID="UpdatePanelAll" runat="server">
        <ContentTemplate>--%>

    <%--<script>
                Sys.Application.add_load(assignCKEditorInTextField);
            </script>--%>

    <div class="row">
        <div class="col-sm-12 col-md-12 col-lg-12">
            <h2>AdmitCard Instruction Setup</h2>
            <hr />
        </div>
    </div>



    <div class="row">
        <div class="col-sm-12 col-md-12 col-lg-12">
            <asp:Panel ID="messagePanel" runat="server" Visible="false">
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            </asp:Panel>
        </div>
    </div>

    <div class="row" style="margin-bottom: 20px;">
        <div class="col-sm-12 col-md-12 col-lg-12">
            <div class="panel panel-default">
                
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-5">
                            <div class="form-group">
                                <label for="ddlSubCategory">Session:</label>
                                <asp:DropDownList ID="ddlSession" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlAdmissionUnit_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="form-group">
                                <label for="ddlCategory">School:</label>
                                <asp:DropDownList ID="ddlAdmissionUnit" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlAdmissionUnit_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-2" style="padding-top: 25px;">
                            <%--<asp:LinkButton ID="btnLoad" runat="server" CssClass="btn btn-primary" OnClick="btnLoad_Click">
                            Load
                        </asp:LinkButton>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-sm-12 col-md-12 col-lg-12">
            <div class="panel panel-primary">
                <a id="clickCollaps" data-toggle="collapse" href="#collapse1">
                    <div class="panel-heading bg-info" style="border-bottom: 1px solid #337ab7;">
                        <h4 class="panel-title"><i class="fas fa-angle-double-down"></i>&nbsp;<strong>Create Instruction</strong></h4>
                    </div>
                </a>
                <div id="collapse1" class="panel-collapse collapse">
                    <div class="panel-body">

                        <div class="row">
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <div class="form-group">
                                    <label>Instruction:<span class="spanAsterisk">*</span></label>
                                    <asp:TextBox ID="txtNoticeDetails" runat="server" TextMode="MultiLine"></asp:TextBox>
                                    <script type="text/javascript" lang="javascript">
                                        CKEDITOR.replace('<%=txtNoticeDetails.ClientID%>');
                                    </script>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-12 col-md-12 col-lg-12">
                                <asp:LinkButton ID="btnSave" runat="server"
                                    ValidationGroup="gr1" OnClick="btnSave_Click"
                                    CssClass="btn btn-success">
                                                    Create Instruction
                                </asp:LinkButton>

                                <asp:LinkButton ID="btnClear" runat="server"
                                    OnClick="btnClear_Click"
                                    CssClass="btn btn-default" Style="margin-left: 5px;">Clear</asp:LinkButton>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-sm-12 col-md-12 col-lg-12">
            <div class="label label-info">
                Records:&nbsp;<asp:Label ID="lblCount" runat="server"></asp:Label>
            </div>
            <div style="margin-top: 6px; overflow-x: scroll;">
                <asp:ListView ID="lvNotices" runat="server"
                    OnItemDataBound="lvNotices_ItemDataBound"
                    OnItemCommand="lvNotices_ItemCommand"
                    OnItemDeleting="lvNotices_ItemDeleting"
                    OnItemUpdating="lvNotices_ItemUpdating">
                    <LayoutTemplate>
                        <table id="tblNotices"
                            class="table table-bordered table-hover table-condensed table-striped"
                            style="width: 100%; text-align: center">
                            <tr runat="server" style="background-color: #1387de; color: white;">
                                <th runat="server" style="text-align: center;">SL#</th>
                                <th runat="server">School</th>
                                <th runat="server">Admit Card Instruction</th>
                                <th runat="server" style="text-align: center;">Action</th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder" />
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr runat="server">
                            <td valign="middle" align="center" class="">
                                <asp:Label ID="lblSerial" runat="server"
                                    Text='<%# Container.DataItemIndex + 1 %>' />.
                            </td>
                            <td valign="middle" align="left" class="">
                                <asp:Label ID="lblSchool" runat="server" />
                            </td>
                            <td valign="middle" align="left" class="">
                                <asp:Label ID="lblNoticeDetails" runat="server" />
                            </td>
                            <td valign="middle" align="center" class="">
                                <asp:LinkButton CssClass="" ID="lnkEdit" runat="server">Edit</asp:LinkButton>
                                |
                            <asp:LinkButton CssClass="" ID="lnkDelete"
                                OnClientClick="return confirm('Confirm you want to Delete?');"
                                runat="server">Delete</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <div class="alert alert-warning" role="alert" style="text-align: center">No item to display.</div>
                    </EmptyDataTemplate>
                </asp:ListView>
            </div>
        </div>
    </div>



    <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
