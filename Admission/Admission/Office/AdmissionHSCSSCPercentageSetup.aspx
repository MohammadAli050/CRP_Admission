<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdmissionHSCSSCPercentageSetup.aspx.cs" Inherits="Admission.Admission.Office.AdmissionHSCSSCPercentageSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.0/sweetalert.min.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:UpdatePanel ID="updatePanelAll" runat="server">
    <ContentTemplate>
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h4>SSC and HSC Marks Percentage Setup</h4>
                </div>
            </div>
        </div>
        <hr />
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-body">
                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                    <div class="row">
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Session</strong></label>
                                <asp:DropDownList ID="ddlSession" runat="server" Width="100%" CssClass="form-control" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
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
                                <asp:DropDownList ID="ddlFaculty" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-3 col-md-3 col-lg-3">
                            <div class="form-group">
                                <label><strong>Exam Type</strong></label>
                                <asp:DropDownList ID="ddlExamType" Width="100%" CssClass="form-control" runat="server" Enabled="true">
                                    <asp:ListItem Enabled="true" Text="-All-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="SSC" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="HSC" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="O Level" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="A Level" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="Dakhil" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Alim" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="Diploma" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="SSC (Vocational)" Value="12"></asp:ListItem>
                                    <asp:ListItem Text="HSC (Vocational)" Value="13"></asp:ListItem>
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
                        <div class="col-sm-1 col-md-1 col-lg-1">
                            <div class="form-group">
                                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-success"
                                    OnClick="btnAdd_Click"
                                    Style="width: 100%; margin-top: 24px;" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <hr />
        <div class="col-md-12" id="pnlAddUpdate" runat="server" visible="false">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Session</strong></label>
                                <asp:DropDownList ID="ddlSessionAddUpdate" runat="server" Width="100%" OnSelectedIndexChanged="ddlSessionAddUpdate_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                <asp:CompareValidator ID="ddlSessionAddUpdate_CV" runat="server"
                                    ControlToValidate="ddlSessionAddUpdate"
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
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Faculty</strong></label>
                                <asp:DropDownList ID="ddlFacultyAddUpdate" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                         <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Exam Type</strong></label>
                                <asp:DropDownList ID="ddlExamTypeAddUpdate" Width="100%" CssClass="form-control" runat="server" Enabled="true">
                                    <asp:ListItem Enabled="true" Text="-All-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="SSC" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="HSC" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="O Level" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="A Level" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="Dakhil" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Alim" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="Diploma" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="SSC (Vocational)" Value="12"></asp:ListItem>
                                    <asp:ListItem Text="HSC (Vocational)" Value="13"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>




                   <%--     <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Exam Type</strong></label>
                                <asp:DropDownList ID="ddlExamTypeAddUpdate" runat="server" Width="100%" CssClass="form-control">
                                    <asp:ListItem Text="-All-" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="SSC" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="HSC" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="O Level" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="A Level" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="Dakhil" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Alim" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="Diploma" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="SSC (Vocational)" Value="12"></asp:ListItem>
                                    <asp:ListItem Text="HSC (Vocational)" Value="13"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>--%>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Mark Percentage</strong></label>
                                <input id="txtPercentage" runat="server" class="form-control" width="100%" />
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <label><strong>Number of Subjects</strong></label>
                                <input id="txtNumberSubject" runat="server" class="form-control" width="100%" value ="0" />
                            </div>
                        </div>
                        <div class="col-sm-2 col-md-2 col-lg-2">
                            <div class="form-group">
                                <asp:Button ID="btnAddUpdate" runat="server" Text="Save" CssClass="btn btn-success"
                                    OnClick="btnAddUpdate_Click"
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
                        <asp:ListView ID="lvPercentSetup" runat="server"
                            OnItemDataBound="lvPercentSetup_ItemDataBound"
                            OnItemCommand="lvPercentSetup_ItemCommand"
                            OnItemDeleting="lvPercentSetup_ItemDeleting"
                            OnItemUpdating="lvPercentSetup_ItemUpdating">
                            <LayoutTemplate>
                                <table id="tbl"
                                    class="table table-hover table-condensed table-striped table-bordered"
                                    style="width: 100%; text-align: left">
                                    <tr runat="server" style="background-color: #1387de; color: white; font-size: small">
                                        <th>SL#</th>
                                        <th>Session</th>
                                        <th>Faculty</th>
                                        <th>Exam Type</th>
                                        <th>Marks Percentage</th>
                                        <th>Number of Subjects</th>
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
                                        <asp:Label ID="lblExamType" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblPercentage" runat="server" />
                                    </td>
                                    <td valign="middle" align="left" class="">
                                        <asp:Label ID="lblNoSubject" runat="server" />
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
            </div>
        </div>
    </div>
     </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
