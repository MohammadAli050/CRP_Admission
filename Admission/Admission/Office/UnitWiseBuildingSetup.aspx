<%@ Page Title="Faculty Wise Building Priority Setup" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="UnitWiseBuildingSetup.aspx.cs" Inherits="Admission.Admission.Office.UnitWiseBuildingSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Faculty/Program Wise Building Priority Setup</h4>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanel_Filter" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </asp:Panel>
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td class="style_td" style="width: 10%">Faculty/Program</td>
                                    <td style="width: 40%">
                                        <asp:DropDownList ID="ddlAdmUnit" runat="server" Width="90%" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlAdmUnit_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="style_td" style="width: 10%">Campus</td>
                                    <td style="width: 40%">
                                        <asp:DropDownList ID="ddlCampus" runat="server" Width="90%"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" />&nbsp;&nbsp;
                                    <asp:Button ID="btnGenerate" runat="server" Text="Generate" Font-Bold="true" ForeColor="Crimson" OnClick="btnGenerate_Click" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <%-- END ROW 1 --%>


    <asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>

            <div>
                <div>
                    <div>
                        <strong>
                            <asp:Label ID="lblType" runat="server"></asp:Label>
                            &nbsp; : &nbsp;
                            Records 
                        </strong>-
                        <asp:Label ID="lblCount" runat="server" CssClass="label label-info"></asp:Label>
                    </div>
                    <br />
                    <div>

                        <asp:Panel ID="gridviewPanel" runat="server">
                            <asp:GridView ID="gvProgBuildingPrior" runat="server"
                                CssClass="table table-responsive table-hover table-bordered"
                                AutoGenerateColumns="false" Width="100%">
                                <HeaderStyle BackColor="#1387de" ForeColor="White" Font-Size="Smaller" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SL" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate><%# Container.DataItemIndex + 1 %>.</ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Font-Size="Small" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Venue Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProgramCampusPriorityId" runat="server" Visible="false" Text='<%#Eval("ProgramCampusPriorityID") %>' Value='<%#Eval("ProgramCampusPriorityID") %>'></asp:Label>
                                            <asp:Label ID="lblVenueName" runat="server" ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Faculty/Program Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnitName" runat="server" Text='<%#Eval("AdmissionUnitName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Campus Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCampusName" runat="server" Text='<%#Eval("CampusName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Building Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblBuildingName" runat="server" Text='<%#Eval("BuildingName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Priority">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="hfBuildingPriority" Visible="false" Text='<%#Eval("BuildingPriority") %>' Value='<%#Eval("BuildingPriority") %>' />
                                            <asp:DropDownList runat="server" ID="ddlBuildingPriority" Width="100px"></asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ToolTip="Save" ID="lnkEdit" Width="80px"
                                                CommandArgument='<%#Eval("ID") %>'
                                                OnClick="lnkEdit_Click" Text="Save" OnClientClick="return confirm('Are you sure want to change priority?');">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Data Found.
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
