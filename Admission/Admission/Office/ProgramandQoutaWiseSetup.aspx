<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="ProgramandQoutaWiseSetup.aspx.cs" Inherits="Admission.Admission.Office.ProgramandQoutaWiseSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.css" rel="stylesheet" type="text/css" />

    <style>
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Program and Quota Wise Total Seat Setup</h4>
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
                    </div>
                </div>
            </div>
        </div>

    </div>

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
                                        <th>From Quota</th>
                                        <th>Add/Update</th>
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
                                        <asp:ListBox ID="lbQouta" runat="server"></asp:ListBox>
                                    </td>
                                    <td>
                                        <asp:LinkButton CssClass="btn btn-primary" ID="lnkAddUpdate" runat="server">Quota Seat</asp:LinkButton>
                                    </td>
                                    <td valign="middle" align="right" class="">

                                        <asp:LinkButton CssClass="" ID="lnkEdit" runat="server" Visible="false">Edit</asp:LinkButton>
                                                      
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

    <asp:Button ID="btnAddSharedProg" runat="server" Style="display: none"></asp:Button>
    <ajaxToolkit:ModalPopupExtender
        ID="ModalPopupExtender2"
        runat="server"
        TargetControlID="btnAddSharedProg"
        PopupControlID="shrdPnPopUp"
        CancelControlID="btnShrdCancel"
        BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Panel ID="shrdPnPopUp" runat="server" Style="display: none;" BackColor="White">
        <div style="width: 1200px; padding: 5px; margin: 5px;">
            <fieldset style="padding: 10px; margin: 5px; border-color: red;">
                <div style="text-align: center">
                    <h4 style="color: red; font-size: large; border-bottom: 1px solid; padding-bottom: 5px;"><b>Quota Seat Setup</b></h4>
                </div>
                <asp:HiddenField ID="hdnProgramSeatId" runat="server" Value="" />
                <div class="row">
                    <div runat="server" class="col-md-12" id="div1">
                        <label class="label-width-popupbox">Msg:</label>
                        <asp:Label runat="server" ID="lblSharedMsg" Style="width: 500px; color: red" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-3">
                            <span>Freedom Fighter :</span>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtFreedomFighter" runat="server" Text="0"></asp:TextBox>
                        </div>
                    </div>
                    <br />

                    <div class="col-md-12">
                        <div class="col-md-3">
                            <span>Special Quota :</span>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtSpecialQuota" runat="server" Text="0"></asp:TextBox>
                        </div>
                    </div>
                    <br />

                    <div class="col-md-12">
                        <div class="col-md-3">
                            <span>Tribal :</span>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtTribal" runat="server" Text="0"></asp:TextBox>
                        </div>
                    </div>

                    
                    <br />

                    <div class="col-md-12">
                        <div class="col-md-3">
                            <span>Person with Disability (Physical) :</span>
                        </div>
                        <div class="col-md-3">
                            <asp:TextBox ID="txtDisable" runat="server" Text="0"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-3">

                        <asp:Button ID="btnAddUpdateQouta" runat="server" Text="Add/Update" Width="100%" Font-Bold="true" class="btn btn-info" OnClick="btnAddUpdateQouta_Click" />
                    </div>
                </div>
                <br />

                <div class="row">
                    <div class="col-md-3">

                        <asp:Button ID="btnShrdCancel" runat="server" Width="100%" Font-Bold="true" class="btn btn-danger" Text="Cancel" />
                    </div>
                </div>
            </fieldset>
        </div>
    </asp:Panel>


</asp:Content>
