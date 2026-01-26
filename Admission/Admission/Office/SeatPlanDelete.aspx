<%@ Page Title="Seat Plan Delete" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="SeatPlanDelete.aspx.cs" Inherits="Admission.Admission.Office.SeatPlanDelete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-xs-1 col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Delete Seat Plan</h4>
                </div>
                <div class="panel-body">

                    <asp:UpdatePanel ID="updatePanelFilter" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="panelMessage" runat="server">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </asp:Panel>
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td class="style_td" style="width: 10%">Faculty</td>
                                    <td style="width: 35%">
                                        <asp:DropDownList ID="ddlAdmissionUnit" runat="server"></asp:DropDownList>
                                        <asp:CompareValidator ID="ddlAdmissionUnitComV" runat="server" ControlToValidate="ddlAdmissionUnit"
                                            ErrorMessage="Required" ForeColor="Crimson" Font-Size="Small"
                                            ValueToCompare="-1" Operator="NotEqual" Display="Dynamic"
                                            ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                    <td class="style_td_secondCol" style="width: 10%">Session</td>
                                    <td style="width: 35%">
                                        <asp:DropDownList ID="ddlSession" runat="server" Width="35%"></asp:DropDownList>
                                        <asp:CompareValidator ID="ddlSessionComV" runat="server" ControlToValidate="ddlSession"
                                            ErrorMessage="Required" ForeColor="Crimson" Font-Size="Small"
                                            ValueToCompare="-1" Operator="NotEqual" Display="Dynamic"
                                            ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                    <td style="width: 10%">
                                        <asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>
    <%-- END ROW 1 --%>

    <asp:UpdatePanel ID="updatePanel_GridView" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            Records:
                            <asp:Label ID="lblCount" runat="server"></asp:Label>
                        </div>
                        <div class="panel-body">
                            <div>
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" ForeColor="Crimson"
                                    OnClientClick="return confirm('Are you sure you want to Delete?');"
                                    OnClick="btnDelete_Click" /> &nbsp;
                                <asp:Label ID="lblDeleteMsg" runat="server"></asp:Label>
                            </div>
                            <div>
                                <asp:GridView ID="gvProgramRoomPriority" runat="server"
                                    CssClass="table table-responsive table-hover table-bordered table-condensed"
                                    AutoGenerateColumns="false" Width="100%">
                                    <HeaderStyle BackColor="#1387de" ForeColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="SL" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate><%# Container.DataItemIndex + 1 %>.</ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="AdmUnitName" HeaderText="Faculty/Program" />
                                        <asp:BoundField DataField="StartRoll" HeaderText="Start Roll" />
                                        <asp:BoundField DataField="EndRoll" HeaderText="End Roll" />
                                        <asp:BoundField DataField="BuildingName" HeaderText="Building Name" />
                                        <asp:BoundField DataField="RoomName" HeaderText="Room Name" />
                                        <asp:BoundField DataField="Priority" HeaderText="Priority" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No data found.
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                            <br />
                            <div>
                                <asp:Button ID="btnDeleteTr" runat="server" Text="Delete" ForeColor="Crimson"
                                    OnClientClick="return confirm('Are you sure you want to Delete?');"
                                    OnClick="btnDeleteTr_Click" /> &nbsp;
                                <asp:Label ID="lblDeleteTrMsg" runat="server"></asp:Label>
                            </div>
                            <div>
                                <asp:GridView ID="gvTestRoll" runat="server"
                                    CssClass="table table-responsive table-hover table-bordered table-condensed"
                                    AutoGenerateColumns="false" Width="100%">
                                    <HeaderStyle BackColor="#1387de" ForeColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="SL" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate><%# Container.DataItemIndex + 1 %>.</ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="unitName" HeaderText="Faculty/Program" />
                                        <asp:BoundField DataField="candidateName" HeaderText="Candidate Name" />
                                        <asp:BoundField DataField="candidatePhone" HeaderText="Candidate Phone" />
                                        <asp:BoundField DataField="testRoll" HeaderText="Test Roll" />
                                        <asp:BoundField DataField="programRoomPriorID" HeaderText="PRP" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No data found.
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                        <%-- END PANEL-BODY --%>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
