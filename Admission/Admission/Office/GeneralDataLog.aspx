<%@ Page Title="General Data Log" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="GeneralDataLog.aspx.cs" Inherits="Admission.Admission.Office.GeneralDataLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <script type="text/javascript">

        function InProgress() {
            var panelProg = $get('divProgress');
            panelProg.style.display = '';
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/AppImg/t1.gif" Height="250px" Width="250px" />
    </div>

    <br />


    <asp:UpdatePanel ID="updatePanelAll" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <h3>Log View</h3>
                    <%--<hr style="margin-top: 0px; margin-bottom: 10px;" />--%>
                </div>
            </div>

            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>PaymentId</strong></label>
                                <asp:TextBox ID="txtPaymentId" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>User</strong></label>
                                <asp:TextBox ID="txtUser" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Start Date</strong></label>
                                <asp:TextBox ID="txtStartDate" runat="server" Width="100%" CssClass="form-control" placeholder="dd/mm/yyyy"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="txtStartDate" />
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>End Date</strong></label>
                                <asp:TextBox ID="txtEndDate" runat="server" Width="100%" CssClass="form-control" placeholder="dd/mm/yyyy"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="txtEndDate" />
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <br />
                            <asp:Button ID="btnLoad" runat="server" CssClass="btn btn-info btn-sm" Text="Load" OnClick="btnLoad_Click" style="width:100%;margin-top: 5px;"/>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <br />
                            <asp:Button ID="btnClear" runat="server" CssClass="btn btn-default btn-sm" Text="Clear" OnClick="btnClear_Click" style="width:100%;margin-top: 5px;"/>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-12 col-md-12 col-lg-12">
                            <asp:Panel ID="messagePanel" runat="server" Visible="false">
                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>


            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-info">
                        <div class="panel-body">


                            <asp:Panel ID="listViewPanel" runat="server">

                                <asp:ListView ID="lvAdmSetup" runat="server"
                                    OnItemDataBound="lvAdmSetup_ItemDataBound"
                                    OnItemCommand="lvAdmSetup_ItemCommand"
                                    OnItemDeleting="lvAdmSetup_ItemDeleting"
                                    OnItemUpdating="lvAdmSetup_ItemUpdating">
                                    <LayoutTemplate>
                                        <table id="tbl"
                                            class="table table-hover table-condensed table-striped table-bordered"
                                            style="width: 100%;">
                                            <tr runat="server" style="background-color: #1387de; color: white; font-size: small">
                                                <%--<th runat="server" style="text-align: center">SL#</th>--%>
                                                <th runat="server" style="text-align: center">Date</th>
                                                <th runat="server" style="text-align: center">User</th>
                                                <th runat="server" style="text-align: center">Event Name</th>
                                                <th runat="server" style="text-align: center">Old Data</th>
                                                <th runat="server" style="text-align: center">New Data</th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder" />
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr runat="server" style="font-size: small">
                                            <%--<td valign="middle" align="middle" class="">
                                                <asp:Label ID="lblSerial" runat="server" />
                                            </td>--%>
                                            <td valign="middle" align="left" class="">
                                                <asp:Label ID="lblDate" runat="server" />
                                            </td>
                                            <td valign="middle" align="middle" class="">
                                                <asp:Label ID="lblUser" runat="server" />
                                            </td>
                                            <td valign="middle" align="middle" class="">
                                                <asp:Label ID="lblEventName" runat="server" />
                                            </td>
                                            <td valign="middle" align="middle" class="">
                                                <asp:Label ID="lblOldData" runat="server" />
                                            </td>
                                            <td valign="middle" align="middle" class="">
                                                <asp:Label ID="lblNewData" runat="server" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <div class="alert alert-warning" role="alert" style="text-align: center">No item to display.</div>
                                    </EmptyDataTemplate>
                                </asp:ListView>


                            </asp:Panel>


                        </div>
                    </div>
                </div>
            </div>



        </ContentTemplate>
    </asp:UpdatePanel>



        <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="updatePanelAll" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnLoad" Enabled="false" />
                    <EnableAction AnimationTarget="btnClear" Enabled="false" />
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction AnimationTarget="btnLoad" Enabled="true" />
                    <EnableAction AnimationTarget="btnClear" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>
</asp:Content>
