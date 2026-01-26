<%@ Page Title="Generate Test Roll" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="TestRollGeneration.aspx.cs" Inherits="Admission.Admission.Office.TestRollGeneration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/formStyle.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Generate Test Roll</h4>
                </div>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updatePanel_Filter" runat="server">
                        <ContentTemplate>
                            
                            <div>
                                <p style="color: crimson; font-weight: bold">
                                    Please make sure that Unit Code 2 in Admission Faculty Setup is available for the selected faculty.
                                    If not present, then test roll will be generated without issues at first. But adding more candidates
                                    for test roll generation will not be successfull.
                                </p>
                            </div>

                            <asp:Panel ID="messagePanel" runat="server">
                                <asp:Label ID="lblMessage" runat="server"></asp:Label>
                            </asp:Panel>
                            
                            <table class="table_form table_fullwidth">
                                <tr>
                                    <td class="style_td" style="width: 10%">Faculty</td>
                                    <td style="width: 40%">
                                        <asp:DropDownList ID="ddlAdmUnit" runat="server" Width="70%" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlAdmUnit_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:CompareValidator ID="ddlAdmUnitComV" runat="server" ControlToValidate="ddlAdmUnit"
                                            ValueToCompare="-1" Display="Dynamic" ErrorMessage="Required" ForeColor="Crimson"
                                            Font-Size="Medium" Operator="NotEqual" ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                    <td class="style_td style_td_secondCol" style="width: 10%">Session</td>
                                    <td style="width: 40%">
                                        <asp:DropDownList ID="ddlSession" runat="server" Width="70%"></asp:DropDownList>
                                        <asp:CompareValidator ID="ddlSessionComV" runat="server" ControlToValidate="ddlSession"
                                            ValueToCompare="-1" Display="Dynamic" ErrorMessage="Required" ForeColor="Crimson"
                                            Font-Size="Medium" Operator="NotEqual" ValidationGroup="gr1"></asp:CompareValidator>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Button ID="btnGenerate" runat="server" Text="Generate Test Roll"
                                ValidationGroup="gr1" OnClick="btnGenerate_Click" />
                            &nbsp;
                            <asp:Button runat="server" ID="btnView" Text="View" OnClick="btnView_Click" />
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
