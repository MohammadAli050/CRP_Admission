<%@ Page Title="Seat Plan Generation" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="SeatPlanGeneration.aspx.cs" Inherits="Admission.Admission.Office.SeatPlanGeneration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Generate Seat Plan</h4>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanel_Filter" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div>
                                <p style="color: crimson; font-weight: bold">
                                    Please note that Program/Faculty wise Campus, Building and Room Priorities must be created
                                    before generating seat plan for a particular faculty and session. Else seat plan will not be
                                    generated.
                                </p>
                            </div>

                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </asp:Panel>
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td class="style_td" style="width: 5%">Faculty/Program</td>
                                    <td style="width: 25%">
                                        <asp:DropDownList ID="ddlAdmUnit" runat="server"></asp:DropDownList>
                                    </td>
                                    <td class="style_td" style="width: 8%">Session</td>
                                    <td style="width: 20%">
                                        <asp:DropDownList ID="ddlSession" runat="server"></asp:DropDownList>
                                    </td>
                                    <td class="style_td" style="width: 8%">Venue</td>
                                    <td style="width: 10%">
                                        <asp:DropDownList ID="ddlDistrict" runat="server"></asp:DropDownList>
                                    </td>
                                    <td style="width: 30%">
                                        <asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" />&nbsp;&nbsp;
                                    <asp:Button ID="btnGenerate" runat="server" Text="Generate" Font-Bold="true" ForeColor="Crimson" OnClick="btnGenerate_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

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

                            <asp:GridView ID="gvSeatPlan" runat="server"
                                CssClass="table table-responsive table-hover table-bordered table-condensed"
                                AutoGenerateColumns="false" Width="100%">
                                <HeaderStyle BackColor="#1387de" ForeColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SL" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate><%# Container.DataItemIndex + 1 %>.</ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="unitName" HeaderText="Faculty/Program" />
                                    <asp:BoundField DataField="roomNameNumber" HeaderText="Room Name & No." />
                                    <asp:BoundField DataField="startRoll" HeaderText="Start Roll" />
                                    <asp:BoundField DataField="endRoll" HeaderText="End Roll" />
                                    <asp:BoundField DataField="floorNo" HeaderText="Floor No." />
                                    <asp:BoundField DataField="capacity" HeaderText="Capacity" />
                                    <asp:BoundField DataField="priority" HeaderText="Priority" />
                                    <asp:BoundField DataField="buildingNameNumber" HeaderText="Building Name & No." />
                                    <asp:BoundField DataField="campusName" HeaderText="Campus Name" />
                                </Columns>
                                <EmptyDataTemplate>
                                    No data found.
                                </EmptyDataTemplate>
                            </asp:GridView>

                        </div>
                        <%-- END PANEL-BODY --%>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
