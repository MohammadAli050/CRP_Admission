<%@ Page Title="SSC - HSC Passing Year Setup" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdmissionSSCHSCPassingYearSetup.aspx.cs" Inherits="Admission.Admission.Office.AdmissionSSCHSCPassingYearSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <style>
        input[type="checkbox"] {
            width: 20px;
            height: 20px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanelAll" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <h2>SSC - HSC Passing Year Setup</h2>
                    <%--<hr />--%>
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
                    <div class="panel panel-default">
                        <div class="panel-body">

                            <div class="row">
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <label>Exam Type<span class="spanAsterisk">*</span></label>
                                        <asp:DropDownList ID="ddlExamType" runat="server" CssClass="form-control" Width="100%">
                                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">SSC</asp:ListItem>
                                            <asp:ListItem Value="2">HSC</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="ddlNationality_ComV" runat="server"
                                            ControlToValidate="ddlExamType"
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
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <label>Start Year:<span class="spanAsterisk">*</span></label>
                                        <asp:TextBox ID="txtStarPassingYear" runat="server" type="number" CssClass="form-control" Width="100%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="txtPlaceOfBirth_ReqV" runat="server"
                                            ControlToValidate="txtStarPassingYear"
                                            ErrorMessage="Required"
                                            ForeColor="Crimson"
                                            Display="Dynamic"
                                            Font-Size="9pt"
                                            Font-Bold="true"
                                            ValidationGroup="gr1">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-sm-4 col-md-4 col-lg-4">
                                    <div class="form-group">
                                        <label>End Year:<span class="spanAsterisk">*</span></label>
                                        <asp:TextBox ID="txtEndPassingYear" runat="server" type="number" CssClass="form-control" Width="100%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="txtEndPassingYear"
                                            ErrorMessage="Required"
                                            ForeColor="Crimson"
                                            Display="Dynamic"
                                            Font-Size="9pt"
                                            Font-Bold="true"
                                            ValidationGroup="gr1">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12 col-md-12 col-lg-12">
                                    <div class="form-group">
                                        <label>Is-Active</label>
                                        <asp:CheckBox ID="ckbxIsActive" runat="server" />
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12 col-md-12 col-lg-12">
                                    <asp:LinkButton ID="btnSave" runat="server"
                                        ValidationGroup="gr1" OnClick="btnSave_Click"
                                        CssClass="btn btn-success">
                                                    Create Setup
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


            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <div class="label label-info">
                        Records:&nbsp;<asp:Label ID="lblCount" runat="server"></asp:Label>
                    </div>
                    <div style="margin-top: 6px;">

                        <asp:ListView ID="lvSSCHSCPassingYearSetup" runat="server"
                            OnItemDataBound="lvSSCHSCPassingYearSetup_ItemDataBound"
                            OnItemCommand="lvSSCHSCPassingYearSetup_ItemCommand"
                            OnItemDeleting="lvSSCHSCPassingYearSetup_ItemDeleting"
                            OnItemUpdating="lvSSCHSCPassingYearSetup_ItemUpdating">
                            <LayoutTemplate>
                                <table id="tblSSCHSCPassingYearSetup"
                                    class="table table-bordered table-hover table-condensed table-striped"
                                    style="width: 100%; text-align: center">
                                    <tr runat="server" style="background-color: #1387de; color: white;">
                                        <th runat="server" style="text-align: center;">SL#</th>
                                        <th runat="server" style="text-align: center;">Exam Type</th>
                                        <th runat="server" style="text-align: center;">Start Year</th>
                                        <th runat="server" style="text-align: center;">End Year</th>
                                        <th runat="server" style="text-align: center;">Is Active</th>
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
                                    <td valign="middle" align="center" class="">
                                        <asp:Label ID="lblExamTypeName" runat="server" />
                                    </td>
                                    <td valign="middle" align="center" class="">
                                        <asp:Label ID="lblStartYear" runat="server" />
                                    </td>
                                    <td valign="middle" align="center" class="">
                                        <asp:Label ID="lblEndYear" runat="server" />
                                    </td>
                                    <td valign="middle" align="center" class="">
                                        <asp:Label ID="lblIsActive" runat="server" />
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

        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
