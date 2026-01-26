<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ProgramWiseTotalSeatSetup.aspx.cs" Inherits="Admission.Admission.Office.ProgramWiseTotalSeatSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.css" rel="stylesheet" type="text/css" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Program Wise Total Seat Setup</h4>
                </div>
            </div>
        </div>

        <hr />

        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel panel-body">
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Session</strong></label>
                                <asp:DropDownList ID="ddlSession" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlSession_CV" runat="server"
                                    ControlToValidate="ddlSession"
                                    Display="Dynamic"
                                    ErrorMessage="Session is required"
                                    ForeColor="Crimson"
                                    ValueToCompare="-1"
                                    Font-Size="9pt"
                                    Operator="NotEqual"
                                    ValidationGroup="gr1">
                                </asp:CompareValidator>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Faculty</strong></label>
                                <asp:DropDownList ID="ddlFaculty" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFaculty_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Program</strong></label>
                                <asp:DropDownList ID="ddlProgram" Width="100%" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-1 col-md-1 col-lg-1">
                            <div class="form-group">
                                <asp:Button ID="btnLoad" runat="server" Text="Load" CssClass="btn btn-info"
                                    OnClick="btnLoad_Click"
                                    Style="width: 100%; margin-top: 24px;" />
                            </div>
                        </div>
                        <div class="col-sm-1 col-md-1 col-lg-1">
                            <div class="form-group">
                                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-success"
                                    OnClick="btnAdd_Click"
                                    Style="width: 100%; margin-top: 24px;" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <hr />

        <div class="col-md-12" id="pnlAddUpdate" runat="server" visible="false">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Session</strong></label>
                                <asp:DropDownList ID="ddlSessionAddUpdate" runat="server" Width="100%" OnSelectedIndexChanged="ddlSessionAddUpdate_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlSessionAddUpdate_CV" runat="server"
                                    ControlToValidate="ddlSessionAddUpdate"
                                    Display="Dynamic"
                                    ErrorMessage="Session is required"
                                    ForeColor="Crimson"
                                    ValueToCompare="-1"
                                    Font-Size="9pt"
                                    Operator="NotEqual"
                                    ValidationGroup="gr1">
                                </asp:CompareValidator>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Faculty</strong></label>
                                <asp:DropDownList ID="ddlFacultyAddUpdate" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFacultyAddUpdate_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Program</strong></label>
                                <asp:DropDownList ID="ddlProgramAddUpdate" runat="server" Width="100%" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Total Seat</strong></label>
                                <input id="txtTotalSeat" runat="server" class="form-control" width="100%" />
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>From Merit</strong></label>
                                <input id="txtMeritSeat" runat="server" class="form-control" width="100%" />
                            </div>
                        </div>
                        <div class="col-sm-1 col-md-1 col-lg-1">
                            <div class="form-group">
                                <asp:Button ID="btnAddUpdate" runat="server" Text="Save" CssClass="btn btn-success"
                                    OnClick="btnAddUpdate_Click"
                                    Style="width: 100%; margin-top: 24px;" />
                            </div>
                        </div>
                        <div class="col-sm-1 col-md-1 col-lg-1">
                            <div class="form-group">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger"
                                    OnClick="btnCancel_Click"
                                    Style="width: 100%; margin-top: 24px;" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <hr />

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-primary">
                <div class="panel-body">
                    <div class="row" style="overflow-x: scroll; overflow-y: scroll">
                        <asp:ListView ID="lvTotalSeatsetup" runat="server"
                            OnItemDataBound="lvTotalSeatsetup_ItemDataBound"
                            OnItemCommand="lvTotalSeatsetup_ItemCommand"
                            OnItemDeleting="lvTotalSeatsetup_ItemDeleting"
                            OnItemUpdating="lvTotalSeatsetup_ItemUpdating">
                            <LayoutTemplate>
                                <table id="tbl"
                                    class="table table-hover table-condensed table-striped table-bordered"
                                    style="width: 100%; text-align: left">
                                    <tr runat="server" style="background-color: #1387de; color: white; font-size: small">
                                        <th>SL#</th>
                                        <th>Session</th>
                                        <th>Faculty</th>
                                        <th>Program</th>
                                        <th>Total Seats</th>
                                        <th>From Merit</th>
                                        <th>Remaining</th>
                                        <th>Action</th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder" />
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr runat="server" style="font-size: smaller">
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblSerial" runat="server" />.
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblSession" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblFaculty" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblProgram" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblTotalSeat" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblMeritSeat" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblRemaining" runat="server" />
                                    </td>
                                    <td valign="middle" align="right" class="">

                                        <asp:LinkButton CssClass="" ID="lnkEdit" runat="server">Edit</asp:LinkButton>
                                        |                      
                                        <asp:LinkButton CssClass="" ID="lnkDelete"
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
                </div>
            </div>
        </div>
    </div>

</asp:Content>
