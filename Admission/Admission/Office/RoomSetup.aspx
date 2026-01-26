<%@ Page Title="Room Setup" Language="C#" MasterPageFile="~/SiteAdmin.Master"
    AutoEventWireup="true" CodeBehind="RoomSetup.aspx.cs" Inherits="Admission.Admission.Office.RoomSetup"%>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../Content/formStyle.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Room Setup</h4>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePnael_Filter" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </asp:Panel>
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td class="style_td" style="width: 10%">Room Name</td>
                                    <td style="width: 40%">
                                        <asp:TextBox ID="txtRoomName" runat="server" Width="90%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="txtRoomNameRFV" runat="server" ControlToValidate="txtRoomName"
                                            ErrorMessage="Required" ForeColor="Crimson" Display="Dynamic" ValidationGroup="gr1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="style_td style_td_secondCol" style="width: 10%">Building</td>
                                    <td style="width: 40%">
                                        <asp:DropDownList ID="ddlBuilding" runat="server" Width="90%"></asp:DropDownList>
                                        <asp:CompareValidator ID="ddlBuildingComV" runat="server" ControlToValidate="ddlBuilding"
                                            ValueToCompare="-1" Operator="NotEqual" ErrorMessage="Required" ForeColor="Crimson"
                                            Font-Size="Larger" Display="Dynamic" ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td ">Room Number</td>
                                    <td>
                                        <asp:TextBox ID="txtRoomNumber" runat="server" Width="70%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="txtRoomNumberRFV" runat="server" ControlToValidate="txtRoomNumber"
                                            ErrorMessage="Required" ForeColor="Crimson" Display="Dynamic" ValidationGroup="gr1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    <td class="style_td style_td_secondCol">Priority</td>
                                    <td>
                                        <asp:TextBox ID="txtPriority" runat="server" TextMode="Number" Width="70%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="txtPriorityRFV" runat="server" ControlToValidate="txtPriority"
                                            ErrorMessage="Required" ForeColor="Crimson" Display="Dynamic" ValidationGroup="gr1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td ">Floor Number</td>
                                    <td>
                                        <asp:TextBox ID="txtFloorNumber" runat="server" Width="70%"></asp:TextBox>
                                    </td>
                                    <td class="style_td style_td_secondCol">Room Capacity</td>
                                    <td>
                                        <asp:TextBox ID="txtRoomCapacity" runat="server" TextMode="Number" Width="70%"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="txtRoomCapacityRFV" runat="server" ControlToValidate="txtRoomCapacity"
                                            ErrorMessage="Required" ForeColor="Crimson" Display="Dynamic" ValidationGroup="gr1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style_td">Is Active?</td>
                                    <td>
                                        <asp:CheckBox ID="ckbxIsActive" runat="server" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                            <asp:Button ID="btnSubmit" Text="Save" runat="server"
                                ValidationGroup="gr1" OnClick="btnSubmit_Click" />
                            &nbsp;
                            <asp:Label ID="lblMessageSave" runat="server"></asp:Label>

                            <div class="row">
                                <div class="col-md-12">
                                    <table>
                                        <tr>
                                            <td class="style_td" style="width: 10%">Upload File</td>
                                            <td style="width: 40%">
                                                <asp:FileUpload ID="FileUploadRoom" runat="server" />
                                            </td>
                                            <td style="width: 40%">
                                                <asp:Button ID="btnUploadFile" runat="server" Text="Upload" OnClick="btnUploadFile_Click" />
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div>
                                <asp:GridView ID="GridView1" runat="server"></asp:GridView>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnUploadFile" />
                        </Triggers>
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
                        <div class="row" style="margin-top: 1%; margin-left: 1%">
                            Records: &nbsp;
                                <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                        </div>
                        <div class="panel-body">
                        <div class="row" style="overflow-x:scroll;overflow-y:scroll">

                            <asp:ListView ID="lvRoom" runat="server"
                                OnItemDataBound="lvRoom_ItemDataBound"
                                OnItemCommand="lvRoom_ItemCommand"
                                OnItemDeleting="lvRoom_ItemDeleting"
                                OnItemUpdating="lvRoom_ItemUpdating">
                                <LayoutTemplate>
                                    <table id="tbl"
                                        class="table table-hover table-condensed table-striped"
                                        style="width: 100%; text-align: left">
                                        <tr runat="server" style="background-color: #1387de; color: white;">
                                            <th runat="server">SL#</th>
                                            <th runat="server">Room Name</th>
                                            <th runat="server">Building Name</th>
                                            <th runat="server">Room #</th>
                                            <th runat="server">Floor #</th>
                                            <th runat="server">Capacity</th>
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
                                            <asp:Label ID="lblRoomName" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblBuildingName" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblRoomNumber" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblFloorNumber" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblCapacity" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblPriority" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblIsActive" runat="server" />
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
                        <%-- END PANEL-BODY --%>
                    </div>
                </div>
                <%-- END COL-MD-12 --%>
            </div>
            <%-- END ROW 2 --%>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
