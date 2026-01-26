<%@ Page Title="Program Information" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProgramInformation.aspx.cs" Inherits="Admission.Admission.Candidate.ProgramInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">

    <style type="text/css">
        .style_table {
            border-left: lightgray;
            border-left-style: solid;
            border-left-width: 1px;
            border-right: lightgray;
            border-right-style: solid;
            border-right-width: 1px;
            border-bottom: lightgray;
            border-bottom-style: solid;
            border-bottom-width: 1px;
        }

            /*.style_firstTd{
            border-right: solid;
            border-right-color: lightgray;
            border-right-width: 1px;
        }*/

            .style_table tr td:first-child {
                border-right: solid;
                border-right-color: lightgray;
                border-right-width: 1px;
            }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-md-12">

            <div class="panel panel-default">

                <div class="panel-heading">
                    <asp:Label ID="lblUnitName" runat="server" Text=""></asp:Label>
                </div>
                <div class="panel-body">
                    <asp:ListView ID="lvPrograms" runat="server"
                        OnItemDataBound="lvPrograms_ItemDataBound">
                        <LayoutTemplate>
                            <table id="tbl"
                                class="table table-hover table-condensed table-striped"
                                style="width: 100%; text-align: left">
                                <tr runat="server" style="background-color: #1387de; color: white; font-size: medium">
                                    <%--<th runat="server">SL#</th>--%>
                                    <th runat="server">Subject you are applying for</th>
                                    <th></th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder" />
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server" style="font-size: medium">
                                <%--<td valign="middle" align="left" class="">
                                    <asp:Label ID="lblSerial" runat="server" />.
                                </td>--%>
                                <td valign="middle" align="left" class="">
                                    <asp:Label ID="lblPrograms" runat="server" />.
                                </td>
                                <td valign="middle" align="right" class=""></td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="alert alert-warning" role="alert" style="text-align: center">No item to display.</div>
                        </EmptyDataTemplate>
                    </asp:ListView>
                    <table>
                        <tr>
                            <td>
                                <br />
                                <asp:Button ID="btnApply" runat="server" Text="Next"
                                    CssClass="btn btn-primary" OnClick="btnApply_Click" />
                            </td>
                        </tr>
                    </table>
                </div>

            </div>

        </div>
    </div>


    <div class="row">
        <div class="col-md-12">
            Please visit <asp:HyperLink NavigateUrl="https://www.bigm.edu.bd/" Target="_blank" runat="server">BIGM Official Website </asp:HyperLink> 
            for more information about MBA programs.
        </div>
    </div>

</asp:Content>
