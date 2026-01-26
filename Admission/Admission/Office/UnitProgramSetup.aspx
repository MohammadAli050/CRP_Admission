<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="UnitProgramSetup.aspx.cs" Inherits="Admission.Admission.Office.UnitProgramSetup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading"><h4>Details for Admission Units</h4></div>
                <div class="panel-body" style="margin-bottom: -30px;">
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                    <table style="width: 100%" class="table table-condensed">
                        <tr>
                            <td style="text-align: left; width: 15%; font-weight: bold">Admission Unit</td>
                            <td style="text-align: left; width: 35%">
                                <asp:DropDownList ID="ddlAdmissionUnit" runat="server" Width="85%"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlAdmissionUnitCompare" runat="server"
                                    ControlToValidate="ddlAdmissionUnit" ErrorMessage="*" Font-Size="14pt" Font-Bold="true"
                                    ForeColor="Crimson" Display="Dynamic" ValueToCompare="-1" Operator="NotEqual"
                                    ValidationGroup="gr1"></asp:CompareValidator>
                            </td>
                            <td style="text-align: left; width: 15%; font-weight:bold">Intake</td>
                            <td style="text-align: left; width: 35%">
                                <asp:TextBox ID="txtIntake" runat="server" Width="85%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Class Start</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtClassStart" runat="server" Width="85%"></asp:TextBox>
                            </td>
                            <td style="text-align: left; font-weight: bold">Method Of Application</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtMethodOfApplication" runat="server" Width="85%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Number Of Seat</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtNoOfSeat" runat="server" Width="50%" TextMode="Number"></asp:TextBox>
                            </td>
                            <td style="text-align: left; font-weight: bold">Duration (Year)</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDurationYear" runat="server" Width="50%" TextMode="Number"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Duration (Semester)</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtDurationSemester" runat="server" Width="50%" TextMode="Number"></asp:TextBox>
                            </td>
                            <td style="text-align: left; font-weight: bold">Total Credit</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtTotalCredit" runat="server" Width="50%" TextMode="Number"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Total Course Fee</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtTotalCourseFee" runat="server" Width="50%" TextMode="Number"></asp:TextBox>
                            </td>
                            <td style="text-align: left; font-weight: bold">Admission Eligibility</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtAdmissionEligibility" runat="server" TextMode="MultiLine"
                                    Row="5" Width="85%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Admission Test Syllabus</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtAdmissionSyllabus" runat="server" TextMode="MultiLine"
                                    Row="5" Width="85%"></asp:TextBox>
                            </td>
                            <td style="text-align: left; font-weight: bold">Weightage</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtWeightage" runat="server" TextMode="MultiLine"
                                    Row="5" Width="85%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Exam Type</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtExamType" runat="server" TextMode="MultiLine"
                                    Row="5" Width="85%"></asp:TextBox>
                            </td>
                            <td style="text-align: left; font-weight: bold">Contact Info</td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtContactInfo" runat="server" TextMode="MultiLine"
                                    Row="5" Width="85%"></asp:TextBox>
                            </td>
                        </tr>
                        <%--<tr>
                            <td style="text-align: left; font-weight: bold">Admission Form File</td>
                            <td style="text-align: left;">
                                <asp:FileUpload ID="admissionFormFile" runat="server" />
                            </td>
                            <td style="text-align: left; font-weight: bold">Information File</td>
                            <td style="text-align: left;">
                                <asp:FileUpload ID="infoFile" runat="server"/>
                            </td>
                        </tr>--%>
                        <tr>
                            <td style="text-align: left; font-weight: bold">Admission Form File</td>
                            <td style="text-align: left;">
                                <asp:FileUpload ID="admissionFormFile" runat="server" />
                            </td>
                            <td style="text-align: left; font-weight: bold"></td>
                            <td style="text-align: left;">
                                <asp:FileUpload ID="infoFile" runat="server"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Button ID="btnSave" runat="server" Text="Save"  ValidationGroup="gr1"
                                    OnClick="btnSave_Click"/>
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btnClearAdmin"
                                    OnClick="btnClear_Click"/>
                            </td>
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
                    <div class="row" style="margin-bottom: 1%;">
                        Records: &nbsp;
                        <asp:Label ID="lblCount" runat="server" CssClass="badge"></asp:Label>
                    </div>
                    <%--<asp:UpdatePanel ID="updatePanelListView" runat="server">
                        <ContentTemplate>--%>
                    <%--OnItemDeleting="" 
                                OnItemUpdating=""  
                                OnItemDataBound="" 
                                OnItemCommand="" 
                                OnPagePropertiesChanging=""--%>
                    <asp:ListView ID="lvUnitPrograms" runat="server"
                        OnItemDataBound="lvUnitPrograms_ItemDataBound"
                        OnItemCommand="lvUnitPrograms_ItemCommand"
                        OnItemUpdating="lvUnitPrograms_ItemUpdating"
                        OnItemDeleting="lvUnitPrograms_ItemDeleting">
                        <LayoutTemplate>
                            <table id="tblUnitPrograms"
                                class="table table-hover table-condensed table-striped"
                                style="width: 100%; text-align: left">
                                <tr runat="server" style="background-color: #1387de; color: white;">
                                    <th runat="server">Sl#</th>
                                    <th runat="server">Unit Name</th>
                                    <th></th>
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
                                    <asp:Label ID="lblUnitName" runat="server" />
                                </td>
                                <td valign="middle" align="left" class="">
                                    <asp:LinkButton ID="lnkUploadInfoFile" runat="server">Upload Information</asp:LinkButton>&nbsp;
                                    <%--|
                                    &nbsp;<asp:LinkButton ID="lnkUploadForm" runat="server">Upload Form</asp:LinkButton>--%>
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
                    <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
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
