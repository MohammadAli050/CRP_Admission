<%@ Page Title="Program Priority" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="HD_ApplicationPriority.aspx.cs" Inherits="Admission.Admission.HelpDesk.HD_ApplicationPriority" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../../Content/ApplicationForm.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading panelHeaderHeight">
                    <ol class="breadcrumb">
                        <li>
                            <asp:HyperLink ID="hrefAppBasic" runat="server">Basic Info</asp:HyperLink></li>
                        <li class="active">
                            <asp:HyperLink ID="hrefAppPriority" runat="server">Program Priority</asp:HyperLink></li>
                        <li>
                            <asp:HyperLink ID="hrefAppEducation" runat="server">Education</asp:HyperLink></li>
                        <li>
                            <asp:HyperLink ID="hrefAppRelation" runat="server">Parent/Guardian</asp:HyperLink></li>
                        <li>
                            <asp:HyperLink ID="hrefAppAddress" runat="server">Address</asp:HyperLink></li>
                        <li>
                            <asp:HyperLink ID="hrefAppAdditional" runat="server">Additional/Work Experience</asp:HyperLink></li>
                        <%--<li><asp:HyperLink ID="hrefAppFinGuar" runat="server">Financial Guarantor</asp:HyperLink></li>--%>
                        <li>
                            <asp:HyperLink ID="hrefAppAttachment" runat="server">Upload Photo</asp:HyperLink></li>
                    </ol>
                </div>
                <%-- end panel heading --%>
                <div class="panel-body">

                    <%--===================New Code===============--%>
                    <asp:UpdatePanel ID="UpdatePanel_Filter" runat="server">
                        <ContentTemplate>

                            <asp:Panel ID="Panel_Master" runat="server">
                                <asp:Label ID="lblMessage_Masters" runat="server"></asp:Label>
                            </asp:Panel>

                            <%--<div style="margin-bottom: 15px;">
                                <span class="spanAsterisk">Please note that there is no restriction for Admin while adding a priority.</span>
                            </div>--%>

                            <asp:Panel ID="Panel_GridView" runat="server">

                                <table class="table table-responsive table-hover table-striped table-condensed">
                                    <tr>
                                        <td style="vertical-align: middle; width: 5%">Faculty</td>
                                        <td style="width: 25%">
                                            <asp:DropDownList ID="ddlFaculty" runat="server" Width="75%" CssClass="form-control"
                                                OnSelectedIndexChanged="ddlFaculty_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="ddlFacultyComV" runat="server"
                                                ControlToValidate="ddlFaculty" ValueToCompare="-1" Operator="NotEqual"
                                                ErrorMessage="Required" ForeColor="Crimson" Display="Dynamic"
                                                ValidationGroup="gr1"></asp:CompareValidator>
                                        </td>
                                        <td style="vertical-align: middle; width: 5%">Program</td>
                                        <td>
                                            <asp:DropDownList ID="ddlProgram" runat="server" Width="75%" CssClass="form-control"
                                                OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:CompareValidator ID="ddlProgramComV" runat="server"
                                                ControlToValidate="ddlProgram" ValueToCompare="-1" Operator="NotEqual"
                                                ErrorMessage="Required" ForeColor="Crimson" Display="Dynamic"
                                                ValidationGroup="gr1"></asp:CompareValidator>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:DropDownList ID="ddlChoice" runat="server" Width="55%" CssClass="form-control" Visible="false"></asp:DropDownList>
                                            
                                        </td>
                                        <td style="vertical-align: middle;">
                                            <asp:Button ID="btnSave" runat="server" Text="Add" OnClick="btnSave_Click" ValidationGroup="gr1"  CssClass="btn btn-info"/>
                                        </td>
                                    </tr>
                                </table>

                            </asp:Panel>

                            <asp:ListView ID="lvProgramPriority" runat="server"
                                OnItemDataBound="lvProgramPriority_ItemDataBound"
                                OnItemCommand="lvProgramPriority_ItemCommand"
                                GroupItemCount="1"
                                GroupPlaceholderID="groupPlaceHolder"
                                ItemPlaceholderID="itemPlaceHolder">
                                <LayoutTemplate>
                                    <table class="table table-hover table-condensed table-striped" style="width: 100%; text-align: left">
                                        <asp:PlaceHolder runat="server" ID="groupPlaceHolder"></asp:PlaceHolder>
                                    </table>
                                </LayoutTemplate>
                                <GroupTemplate>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                </GroupTemplate>
                                <ItemTemplate>
                                    <%# AddGroupingHeader() %></h4>
                                        
                                    <tr runat="server">
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblSerial" runat="server" Visible="false" />
                                        </td>
                                        <%--<td valign="middle" align="left" class="">
                                            <asp:Label ID="lblUnitName" runat="server" />
                                        </td>--%>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblProgramName" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblChoice" runat="server" />
                                        </td>
                                        <td valign="middle" align="right" class="">
                                            <asp:LinkButton ID="lnkRemove" runat="server" CssClass="btn-danger btn-sm" OnClientClick="return confirm('Are you sure you want to Remove this Program ?');">Remove</asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <div class="alert alert-warning" role="alert" style="text-align: center">No item to display. Please add program choices.</div>
                                </EmptyDataTemplate>
                            </asp:ListView>


                            <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false" CssClass="btn btn-primary" />

                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <%--===================End New Code===============--%>











                    <%--===================Old Code===============--%>
                    <%--<asp:Panel ID="Panel_Master" runat="server">
                        <asp:Label ID="lblMessage_Masters" runat="server"></asp:Label>
                    </asp:Panel>

                    <asp:Panel ID="Panel_GridView" runat="server">

                        <asp:UpdatePanel ID="updatePanel_ListView" runat="server">
                            <ContentTemplate>
                                <asp:ListView ID="lvProgramPriority" runat="server"
                                    OnItemDataBound="lvProgramPriority_ItemDataBound"
                                    OnItemCommand="lvProgramPriority_ItemCommand"
                                    OnItemDeleting="lvProgramPriority_ItemDeleting"
                                    OnItemUpdating="lvProgramPriority_ItemUpdating"
                                    GroupItemCount="1"
                                    GroupPlaceholderID="groupPlaceHolder"
                                    ItemPlaceholderID="itemPlaceHolder">
                                    <LayoutTemplate>
                                        <table class="table table-hover table-condensed table-striped" style="width: 100%; text-align: left">
                                            <asp:PlaceHolder runat="server" ID="groupPlaceHolder"></asp:PlaceHolder>
                                        </table>
                                    </LayoutTemplate>
                                    <GroupTemplate>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
                                    </GroupTemplate>
                                    <ItemTemplate>
                                        <%# AddGroupingHeader() %></h4>
                                        
                                    <tr runat="server">
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblSerial" runat="server" Visible="false" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblProgramName" runat="server" />
                                        </td>
                                        <td valign="middle" align="left" class="">
                                            <asp:Label ID="lblChoice" runat="server" />
                                        </td>
                                        <td valign="middle" align="right" class="">
                                            <asp:LinkButton CssClass="" ID="lnkEdit" runat="server" Visible="false">Edit</asp:LinkButton>
                                        </td>
                                    </tr>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <div class="alert alert-warning" role="alert" style="text-align: center">No item to display. Please add program choices.</div>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </asp:Panel>--%>
                    <%--===================End Old Code===============--%>
                </div>
                <%-- end panel body --%>
            </div>
            <%-- end panel default --%>
        </div>
        <%-- end col-md-12 --%>
    </div>
    <%-- end row --%>
</asp:Content>
