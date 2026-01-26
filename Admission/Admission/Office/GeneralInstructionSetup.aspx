<%@ Page Title="General Instructions Setup" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="GeneralInstructionSetup.aspx.cs" Inherits="Admission.Admission.Office.GeneralInstructionSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <link href="../../Content/formStyle.css" rel="stylesheet" />


    <script type="text/javascript">
        function Count() {
            var i = document.getElementById("MainContent_txtGETitle").value.length;
            document.getElementById("MainContent_display").innerHTML = 1000 - i;
        }

        function Count_Ben() {
            var i = document.getElementById("MainContent_txtGETitleBen").value.length;
            document.getElementById("MainContent_display_ben").innerHTML = 1000 - i;
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>General Instructions Setup</strong>
                </div>
                <div class="panel-body">

                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>

                    <table class="table_form table_fullwidth">
                        <tr>
                            <td class="style_td" style="width: 10%">Title</td>
                            <td style="width: 90%">
                                <asp:TextBox ID="txtGETitle" runat="server" Width="85%" MaxLength="1000"
                                    Rows="2" onkeyup="Count()" TextMode="MultiLine"></asp:TextBox>
                                <br />
                                <span style="color: grey">Characters left:
                                    <asp:Label ID="display" runat="server">1000</asp:Label></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Description</td>
                            <td>
                                <asp:TextBox ID="txtGEDesc" runat="server" TextMode="MultiLine" Rows="5" Width="85%"></asp:TextBox>
                                <ajaxToolkit:HtmlEditorExtender ID="txtGEDescHEE" runat="server" TargetControlID="txtGEDesc"></ajaxToolkit:HtmlEditorExtender>
                                <span style="color: crimson">Note: For security reason, please do not use hyperlink or any link to outside website address.</span>
                            </td>
                        </tr>
                         <tr>
                            <td class="style_td" style="width: 10%">Title (Bengali)</td>
                            <td style="width: 90%">
                                <asp:TextBox ID="txtGETitleBen" runat="server" Width="85%" MaxLength="1000"
                                    Rows="2" onkeyup="Count_Ben()" TextMode="MultiLine"></asp:TextBox>
                                <br />
                                <span style="color: grey">Characters left:
                                    <asp:Label ID="display_ben" runat="server">1000</asp:Label></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="style_td">Description (Bengali)</td>
                            <td>
                                <asp:TextBox ID="txtGEDescBen" runat="server" TextMode="MultiLine" Rows="5" Width="85%"></asp:TextBox>
                                <ajaxToolkit:HtmlEditorExtender ID="txtGEDescBenHEE" runat="server" TargetControlID="txtGEDescBen"></ajaxToolkit:HtmlEditorExtender>
                                <span style="color: crimson">Note: For security reason, please do not use hyperlink or any link to outside website address.</span>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="btnSave" runat="server" Text="Save"
                        OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-body" style="margin-bottom: 0px;">
                    <div class="row" style="margin-bottom: 1%;">
                        Records: &nbsp;
                        <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                    </div>
                    <asp:ListView ID="lvData" runat="server"
                        OnItemDataBound="lvData_ItemDataBound"
                        OnItemCommand="lvData_ItemCommand"
                        OnItemDeleting="lvData_ItemDeleting"
                        OnItemUpdating="lvData_ItemUpdating"
                        OnPagePropertiesChanging="lvData_PagePropertiesChanging">
                        <LayoutTemplate>
                            <table id="tbl"
                                class="table table-hover table-condensed table-striped"
                                style="width: 100%; text-align: left">
                                <tr runat="server" style="background-color: #1387de; color: white;">
                                    <th runat="server">SL#</th>
                                    <th runat="server">Title</th>
                                    <th></th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder" />
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server">
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblSerial" runat="server" />.
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblTitle" runat="server" />
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
                    <div class="pagerStyle">
                        <br />
                        <asp:DataPager runat="server" ID="lvDataPager"
                            PagedControlID="lvData" PageSize="15">
                            <Fields>
                                <asp:NextPreviousPagerField PreviousPageText="<<" FirstPageText="First" ShowPreviousPageButton="true"
                                    ShowFirstPageButton="true" ShowNextPageButton="false" ShowLastPageButton="false"
                                    ButtonCssClass="btn btn-default" RenderNonBreakingSpacesBetweenControls="false" RenderDisabledButtonsAsLabels="false" />
                                <asp:NumericPagerField ButtonType="Link" CurrentPageLabelCssClass="btn btn-primary disabled" RenderNonBreakingSpacesBetweenControls="false"
                                    NumericButtonCssClass="btn btn-default" ButtonCount="10" NextPageText="..." NextPreviousButtonCssClass="btn btn-default" />
                                <asp:NextPreviousPagerField NextPageText=">>" LastPageText="Last" ShowNextPageButton="true"
                                    ShowLastPageButton="true" ShowPreviousPageButton="false" ShowFirstPageButton="false"
                                    ButtonCssClass="btn btn-default" RenderNonBreakingSpacesBetweenControls="false" RenderDisabledButtonsAsLabels="false" />
                            </Fields>
                        </asp:DataPager>
                    </div>
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
