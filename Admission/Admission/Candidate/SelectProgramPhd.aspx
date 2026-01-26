<%@ Page Title="MPhil & PhD" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelectProgramPhd.aspx.cs" Inherits="Admission.Admission.Candidate.SelectProgramPhd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">
        <div class="col-sm-12 col-md-12 col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading"><strong>MPhil & PhD</strong></div>
                <div class="panel-body">

                    <asp:ListView ID="lvAdmSetup" runat="server"
                        OnItemDataBound="lvAdmSetup_ItemDataBound">
                        <LayoutTemplate>
                            <table id="tbl"
                                class="table "
                                style="width: 100%;">
                                <tr runat="server" id="itemPlaceholder" />
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server" style="font-size: small">
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblTitle" runat="server" />
                                </td>
                                <td valign="middle" align="middle" class="">
                                    <asp:HyperLink ID="hlViewFile" runat="server"></asp:HyperLink>
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













    <%--<div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <strong>
                        <asp:Label ID="lblEducationCat" runat="server" Text="MPhil & PhD"></asp:Label>
                    </strong>
                </div>
                <div class="panel-body">

                    <table class="table table-hover table-responsive" style="width: 100%; text-align: left">
                        <tr>
                            <td>MPhil & PhD Admission Notice (2020-2021)</td>
                            <td>

                                <asp:HyperLink ID="hrefMPhilPhDAdmissionNotice" runat="server"
                                    CssClass="btn btn-info" Text="View Details"
                                    Target="_blank" NavigateUrl="~/ApplicationDocs/MPhilPhDAdmissionNotice.pdf"></asp:HyperLink>--%>

                                <%--<a href="http://bup.edu.bd/assets/uploads/mphil_phd_program/1536121784MPhil%20and%20PhD%20Admission%20Ad_Web_2018-2019.pdf" target="_blank" class="btn btn-info">View Details</a>--%>
                            <%--</td>--%>
                            <%--<td>
                                <a href="../../ApplicationDocs/Application Form_ MPhil & PhD.pdf" target="_blank">Download Application Form</a>
                            </td>--%>
                            <%--<td class="alert alert-warning">
                                No programs opened for admission.
                            </td>--%>
                        <%--</tr>
						<tr>
                            <td>Preferred Fields/Areas of Research</td>
                            <td>
                                <asp:HyperLink ID="hrefPreferredFieldsAreasofResearch" runat="server"
                                    CssClass="btn btn-info" Text="View Details"
                                    Target="_blank" NavigateUrl="~/ApplicationDocs/PreferredFieldsAreasofResearch.pdf"></asp:HyperLink>--%>


                                <%--<a href="http://bup.edu.bd/assets/uploads/mphil_phd_program/1536120501Preferred%20Fields%20Areas%20of%20Research.pdf" target="_blank" class="btn btn-info">View Details</a>--%>
                            <%--</td>--%>
                            <%--<td>
                                <a href="../../ApplicationDocs/Application Form_ MPhil & PhD.pdf" target="_blank">Download Application Form</a>
                            </td>--%>
                            <%--<td class="alert alert-warning">
                                No programs opened for admission.
                            </td>--%>
                        <%--</tr>
                        <tr>
                            <td>Application Form for MPhil & PhD Admission (2020-2021)</td>
                            <td>


                                <asp:HyperLink ID="hrefApplicationFormforMPhilPhDAdmission" runat="server"
                                    CssClass="btn btn-info" Text="View Details"
                                    Target="_blank" NavigateUrl="~/ApplicationDocs/ApplicationFormforMPhilPhDAdmission.pdf"></asp:HyperLink>--%>


                                <%--<a href="http://bup.edu.bd/assets/uploads/mphil_phd_program/1536120481Application%20Form_%20MPhil%20and%20PhD.pdf" target="_blank" class="btn btn-info">View Details</a>--%>
                            <%--</td>--%>
                            <%--<td>
                                <a href="../../ApplicationDocs/Application Form_ MPhil & PhD.pdf" target="_blank">Download Application Form</a>
                            </td>--%>
                            <%--<td class="alert alert-warning">
                                No programs opened for admission.
                            </td>--%>
                       <%-- </tr>
                        
                    </table>

                </div>--%>
                <%-- END PANEL-BODY --%>
            <%--</div>--%>
            <%-- END PANEL-DEFAULT --%>
        <%--</div>--%>
        <%-- END COL-MD-6 --%>
    <%--</div>--%>
    <%-- END ROW 1 --%>
</asp:Content>
