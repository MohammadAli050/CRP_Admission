<%@ Page Title="District For Seat Plan Setup" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="DistrictForSeatPlanSetup.aspx.cs" Inherits="Admission.Admission.Office.DistrictForSeatPlanSetup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Venue Setup</h4>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePnael_Filter" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </asp:Panel>

                            <div class="row">
                                <div class="col-sm-6 col-md-6 col-lg-6">
                                    <label><strong>District</strong></label>
                                    <asp:DropDownList ID="ddlDistrict" runat="server" CssClass="form-control" Width="100%">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="ReqiredFieldValidator1" runat="server"
                                            Display="Dynamic" Font-Size="9pt" ForeColor="Crimson"
                                            ControlToValidate="ddlDistrict" ErrorMessage="Required"
                                            ValidationGroup="gr1">
                                        </asp:RequiredFieldValidator>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2">
                                    <label><strong>District Number</strong></label>
                                    <asp:TextBox ID="txtDistrictNumber" runat="server" CssClass="form-control" Width="100%" TextMode="Number"></asp:TextBox>
                                </div>
                                 <div class="col-sm-2 col-md-2 col-lg-2">
                                    <label><strong>Priority</strong></label>
                                    <asp:TextBox ID="txtDistrictPriority" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                           
                            <div class="row">
                                <div class="col-lg-2 col-sm-2 col-md-2">
                                    <label><strong>Is Active?</strong></label>
                                    <asp:CheckBox ID="ckbxIsActive" runat="server" />
                                </div>

                            </div>

                            <div class="row">
                                <div class="col-lg-1 col-sm-1 col-md-1">
                           
                            <asp:Button ID="btnSubmit" Text="Save" runat="server" CssClass="btn-success btn-sm"  OnClick="btnSubmit_Click" ValidationGroup="gr1"/>
                            &nbsp;
                            <asp:Label ID="lblMessageSave" runat="server" ></asp:Label>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <%-- END PANEL-BODY --%>
            </div>
        </div>
        <%-- END COL-MD-12 --%>
    </div>
    <%-- END ROW 1 --%>

    <asp:UpdatePanel ID="UpdatePanel_Lv" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="row" style="margin-top:1%; margin-left: 1%">
                            Records: &nbsp;
                                <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                        </div>
                        <div class="panel-body">

                            <asp:ListView ID="lvCampus" runat="server"
                                OnItemDataBound="lvCampus_ItemDataBound"
                                OnItemCommand="lvCampus_ItemCommand"
                                OnItemDeleting="lvCampus_ItemDeleting"
                                OnItemUpdating="lvCampus_ItemUpdating">
                                <LayoutTemplate>
                                    <table id="tbl"
                                        class="table table-hover table-condensed table-striped"
                                        style="width: 100%; text-align: left">
                                        <tr runat="server" style="background-color: #1387de; color: white;">
                                            <th runat="server">SL#</th>
                                            <th runat="server">District Name</th>
                                            <th runat="server">Number</th>
                                            <th runat="server">Priority</th>
                                            <th runat="server">Active?</th>
                                            <th></th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder" />
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr runat="server" style="">
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblSerial" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblDistrictName" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblDistrictNumber" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblDistrictPriority" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblIsActive" runat="server" />
                                        </td>
                                        <td valign="middle" align="right" class="">

                                            <asp:LinkButton CssClass="" ID="lnkEdit" runat="server" Visible="false">Edit</asp:LinkButton>
                                            <%--| --%>                     
                                        <asp:LinkButton CssClass="btn-danger btn-sm" ID="lnkDelete" 
                                            OnClientClick="return confirm('Are you Confirm you want to Delete?');"
                                            runat="server">Delete</asp:LinkButton>
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
                </div>
                <%-- END COL-MD-12 --%>
            </div>
            <%-- END ROW 2 --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
