<%@ Page Title="Admission Unit Programs" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdmissionUnitPrograms.aspx.cs" Inherits="Admission.Admission.AdmissionUnitPrograms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <%--delete message not working :(--%>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>Batches for Admission Subject</h4>
                </div>
                <div class="panel-body" style="margin-bottom: -30px;">


                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>

                    <table style="width: 100%" class="table table-hover table-condensed">


                        <tr>
                            <td style="text-align: left; width: 10%; font-weight: bold">Subject</td>
                            <td style="text-align: left; width: 45%">
                                <asp:UpdatePanel ID="updatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlAdmissionUnit" runat="server" Width="100%"></asp:DropDownList>
                                    </ContentTemplate>
                                    <%--<Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                    </Triggers>--%>
                                </asp:UpdatePanel>
                            </td>
                            <td style="text-align: left; width: 45%">
                                <asp:CompareValidator ID="ddlAdmissionUnitCompare" runat="server"
                                    ControlToValidate="ddlAdmissionUnit" Display="Dynamic"
                                    ErrorMessage="School/Faculty is required" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="gr1"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Program</td>
                            <td style="text-align: left;">
                                <asp:UpdatePanel ID="updatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlProgram" runat="server" Width="100%"
                                            OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <%--<Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                    </Triggers>--%>
                                </asp:UpdatePanel>
                            </td>
                            <td style="text-align: left;">
                                <asp:CompareValidator ID="ddlProgramCompare" runat="server"
                                    ControlToValidate="ddlProgram" Display="Dynamic"
                                    ErrorMessage="Program is required" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="gr1"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Category</td>
                            <td style="text-align: left;">
                                <asp:UpdatePanel ID="updatePanelEducationCategory" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlEducationCategory" runat="server" Width="100%">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <%--<Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                    </Triggers>--%>
                                </asp:UpdatePanel>
                            </td>
                            <td style="text-align: left;">
                                <asp:CompareValidator ID="CompareValidator1" runat="server"
                                    ControlToValidate="ddlEducationCategory" Display="Dynamic"
                                    ErrorMessage="Education category is required" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="gr1"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Session</td>
                            <td style="text-align: left;">
                                <asp:UpdatePanel ID="updatePanel1" runat="server" Visible="true">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlSession" runat="server" Width="100%"
                                            OnSelectedIndexChanged="ddlSession_SelectedIndexChanged"
                                            AutoPostBack="true">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <%--<Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlProgram" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                    </Triggers>--%>
                                </asp:UpdatePanel>
                            </td>
                            <td style="text-align: left;">
                                <asp:CompareValidator ID="ddlSessionCompare" runat="server"
                                    ControlToValidate="ddlSession" Display="Dynamic"
                                    ErrorMessage="Session is required" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="gr1"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Batch</td>
                            <td style="text-align: left;">
                                <asp:UpdatePanel ID="updatePanel2" runat="server" Visible="true">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlBatch" runat="server" Width="100%"></asp:DropDownList>
                                    </ContentTemplate>
                                    <%--<Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlSession" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                    </Triggers>--%>
                                </asp:UpdatePanel>
                            </td>
                            <td style="text-align: left;">
                                <asp:CompareValidator ID="ddlBatchCompare" runat="server"
                                    ControlToValidate="ddlBatch" Display="Dynamic"
                                    ErrorMessage="Batch is required" ForeColor="Crimson"
                                    ValueToCompare="-1" Operator="NotEqual" ValidationGroup="gr1"></asp:CompareValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Is Active</td>
                            <td style="text-align: left;">
                                <asp:UpdatePanel ID="updatePanel5" runat="server" Visible="true">
                                    <ContentTemplate>
                                        <asp:CheckBox ID="ckbxIsActive" runat="server" />
                                    </ContentTemplate>
                                    <%--<Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                    </Triggers>--%>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="gr1"
                                    CssClass="btn btn-success"
                                    OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-default"
                                    OnClick="btnClear_Click" />

                                <asp:Button ID="btnInActiveAll" runat="server" Text="In-Active All" 
                                    style="float:right;" 
                                    CssClass="btn btn-danger" 
                                    OnClientClick="return confirm('Are you sure, you want to In-Active All')"
                                    OnClick="btnInActiveAll_Click"/>

                            </td>
                            <%--<td></td>
                            <td></td>--%>
                        </tr>
                    </table>
                </div>
                <%-- END PANEL-BODY --%>
            </div>
            <%-- END PANEL-DEFAULT --%>
        </div>
        <%-- END COL-MD-12 --%>
    </div>
    <%-- END ROW 1--%>
    <%-- ---------------------------------------------------------------------------------------------------------------------------------- --%>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-body" style="margin-bottom: -25px;">
                    <asp:UpdatePanel ID="updatePanelListView" runat="server">
                        <ContentTemplate>
                            <div class="row" style="margin-bottom: 1%;margin-left:1%">
                                Records: &nbsp;
                        <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                            </div>

                            <%--OnItemDeleting="" 
                                OnItemUpdating=""  
                                OnItemDataBound="" 
                                OnItemCommand="" 
                                OnPagePropertiesChanging=""--%>
                            <div class="row" style="overflow-x:scroll;overflow-y:scroll">
                            <asp:ListView ID="lvAdmissionUnitProgram" runat="server"
                                OnItemDataBound="lvAdmissionUnitProgram_ItemDataBound"
                                OnItemCommand="lvAdmissionUnitProgram_ItemCommand"
                                OnItemDeleting="lvAdmissionUnitProgram_ItemDeleting"
                                OnItemUpdating="lvAdmissionUnitProgram_ItemUpdating">
                                <LayoutTemplate>
                                    <table id="tbl"
                                        class="table table-hover table-condensed table-striped"
                                        style="width: 100%; text-align: left">
                                        <tr runat="server" style="background-color: #1387de; color: white;">
                                            <th runat="server">SL#</th>
                                            <th runat="server">Faculty Name</th>
                                            <th runat="server">Program</th>
                                            <th runat="server">Category</th>
                                            <th runat="server">Session</th>
                                            <th runat="server">Batch</th>
                                            <th runat="server">Active?</th>
                                            <th></th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder" />
                                    </table>
                                    <%--<asp:DataPager runat="server" ID="DataPager" PageSize="20">
                                        <Fields>
                                            <asp:NumericPagerField
                                                PreviousPageText="<--"
                                                NextPageText="-->" />
                                        </Fields>
                                    </asp:DataPager>--%>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr runat="server">
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblSerial" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblAdmissionUnitName" runat="server" />.
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblProgramName" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblEducationCategory" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblSession" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblBatch" runat="server" />
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
                        </ContentTemplate>
                        <%--<Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>--%>
                    </asp:UpdatePanel>
                </div>
                <%-- END PANEL-BODY --%>
            </div>
            <%-- END PANEL-DAFAULT --%>
        </div>
        <%-- END COL-MD-12 --%>
    </div>
    <%-- END ROW 2 --%>
    <%-- ---------------------------------------------------------------------------------------------------------------------------------- --%>
</asp:Content>
