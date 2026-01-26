<%@ Page Title="Property Setup" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdmissionPropertySetup.aspx.cs" Inherits="Admission.Admission.Office.AdmissionPropertySetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <script src="../../Scripts/jquery-3.1.1.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });


        function openCollapse() {
            //alert('Hello World')
            $('#clickCollaps').click()
        }

    </script>

    <style type="text/css">
        input[type="checkbox"]{
            width: 20px !important;
            height: 20px !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <h3>Property Setup</h3>
                    <hr style="margin: 5px 0;" />
                </div>
            </div>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <asp:Panel ID="messagePanel" runat="server" Visible="false">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <div class="panel panel-primary">
                        <a id="clickCollaps" data-toggle="collapse" href="#collapse1">
                            <div class="panel-heading bg-info" style="border-bottom: 1px solid #337ab7;">
                                <h4 class="panel-title"><i class="fas fa-angle-double-down"></i>&nbsp;<strong>Create Property Setup</strong></h4>
                            </div>
                        </a>
                        <div id="collapse1" class="panel-collapse collapse">
                            <div class="panel-body">

                                <div class="row">
                                    <div class="col-sm-6 col-md-6 col-lg-6">
                                        <div class="form-group">
                                            <label><strong>Education Category <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                            <asp:DropDownList ID="ddlEducationCategory" runat="server" Width="100%" CssClass="form-control"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddlEducationCategory_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="CompareValidator2" runat="server"
                                                ControlToValidate="ddlEducationCategory"
                                                ErrorMessage="Required"
                                                Font-Bold="true"
                                                Font-Size="9pt"
                                                ForeColor="Crimson"
                                                Display="Dynamic"
                                                ValueToCompare="-1"
                                                Operator="NotEqual"
                                                ValidationGroup="gr1"></asp:CompareValidator>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-6">
                                        <div class="form-group">
                                            <label><strong>Property Type <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                            <asp:DropDownList ID="ddlPropertyType" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                            <asp:CompareValidator ID="ddlNationality_ComV" runat="server"
                                                ControlToValidate="ddlPropertyType"
                                                ErrorMessage="Required"
                                                Font-Bold="true"
                                                Font-Size="9pt"
                                                ForeColor="Crimson"
                                                Display="Dynamic"
                                                ValueToCompare="-1"
                                                Operator="NotEqual"
                                                ValidationGroup="gr1"></asp:CompareValidator>
                                        </div>
                                    </div>
                                </div>

                                <asp:Panel ID="panelProgramDDL" runat="server" Visible="false">
                                    <div class="row">
                                        <div class="col-sm-12 col-md-12 col-lg-12">
                                            <div class="form-group">
                                                <label><strong>Program <span style="color: crimson; font-weight: bold;">*</span></strong></label>
                                                <asp:DropDownList ID="ddlProgram" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                                <asp:CompareValidator ID="CompareValidator1" runat="server"
                                                    ControlToValidate="ddlProgram"
                                                    ErrorMessage="Required"
                                                    Font-Bold="true"
                                                    Font-Size="9pt"
                                                    ForeColor="Crimson"
                                                    Display="Dynamic"
                                                    ValueToCompare="-1"
                                                    Operator="NotEqual"
                                                    ValidationGroup="gr1"></asp:CompareValidator>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>

                                <div class="row">
                                    <div class="col-sm-6 col-md-6 col-lg-6">
                                        <div class="form-group">
                                            <label><strong>Is-Visible:</strong></label>
                                            <asp:CheckBox ID="cbIsVisible" runat="server"></asp:CheckBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-6 col-md-6 col-lg-6">
                                        <asp:LinkButton ID="btnSave" runat="server"
                                            ValidationGroup="gr1" OnClick="btnSave_Click"
                                            CssClass="btn btn-success">
                                                    Create Setup
                                        </asp:LinkButton>
                                        &nbsp;
                                        &nbsp;
                                        <asp:LinkButton ID="btnClear" runat="server"
                                            OnClick="btnClear_Click"
                                            CssClass="btn btn-default">
                                            Clear
                                        </asp:LinkButton>
                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>




            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-body" style="margin-bottom: -25px;">

                            <div class="row" style="margin-bottom: 1%;">
                                Records: &nbsp;
                                <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                            </div>

                            <asp:ListView ID="lvPropertySetup" runat="server"
                                OnItemDataBound="lvPropertySetup_ItemDataBound"
                                OnItemCommand="lvPropertySetup_ItemCommand"
                                OnItemDeleting="lvPropertySetup_ItemDeleting"
                                OnItemUpdating="lvPropertySetup_ItemUpdating">
                                <LayoutTemplate>
                                    <table id="tbl"
                                        class="table table-bordered table-hover table-condensed table-striped"
                                        style="width: 100%; text-align: left">
                                        <tr runat="server" style="background-color: #1387de; color: white;">
                                            <th runat="server">SL#</th>
                                            <th runat="server" style="text-align: center;">Education Category</th>
                                            <th runat="server">Property Type</th>
                                            <th runat="server">Program Name</th>
                                            <th runat="server" style="text-align: center;">Is-Visible</th>
                                            <th runat="server" style="text-align: center;">Action</th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder" />
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr runat="server">
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblSerial" runat="server" />.
                                        </td>
                                        <td valign="middle" align="center" class="">
                                            <asp:Label ID="lblEducationCategoryName" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblPropertyTypeName" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblProgramName" runat="server" />
                                        </td>
                                        <td valign="middle" align="center" class="" style="text-align: center;">
                                            <asp:Label ID="lblIsVisible" runat="server" />                                         
                                            <asp:CheckBox ID="cbListIsVisible" runat="server" CssClass="cbListIsVisibleClass"
                                                AutoPostBack="true" OnCheckedChanged="cbListIsVisible_CheckedChanged"/>
                                            <asp:HiddenField ID="hfcbIsVisibleID" runat="server" />
                                        </td>

                                        <td valign="middle" align="center" class="">

                                            <asp:LinkButton CssClass="" ID="lnkEdit" runat="server"
                                                data-toggle="tooltip" title="Edit Setup"
                                                class="btn btn-info btn-sm"><i class="far fa-edit"></i></asp:LinkButton>
                                            &nbsp;
                                            |
                                            &nbsp;
                                            <asp:LinkButton CssClass="" ID="lnkDelete" class="btn btn-danger btn-sm"
                                                data-toggle="tooltip" title="Delete Setup"
                                                OnClientClick="return confirm('Are you Confirm you want to Delete?');"
                                                runat="server"><i class="far fa-trash-alt"></i></asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <div class="alert alert-warning" role="alert" style="text-align: center">No item to display.</div>
                                </EmptyDataTemplate>
                            </asp:ListView>

                        </div>
                        <%-- END PANEL-BODY --%>
                    </div>
                    <%-- END PANEL-DAFAULT --%>
                </div>
                <%-- END COL-MD-12 --%>
            </div>
            <%-- END ROW 2 --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
