<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MassageAfterAdmissionClosed.aspx.cs" Inherits="Admission.Admission.Candidate.MassageAfterAdmissionClosed" %>








<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">


    <style type="text/css">
        .style_td {
            font-weight: bold;
            text-align: left;
            font-size: 9pt;
        }

        .style_td1 {
            font-weight: bold;
            text-align: right;
            font-size: 9pt;
        }

        .style_td_secondCol {
            border-left: dotted;
            border-color: gray;
            border-width: 1px;
        }

        .spanAsterisk {
            color: crimson;
            font-size: 12pt;
        }

        .style_thead {
            text-align: center;
            /*background-color: lightgrey;*/
            font-family: Calibri;
            font-size: 12pt;
            font-weight: bold;
        }

        .panelBody_edu_marginBottom {
            margin-bottom: -3%;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="upanel1">
        <ContentTemplate>
             <div class="row">
                        <div class="col-md-12">

                            <%--<div class="col-md-2"></div>--%>

                            <div class="col-md-12">
                                <div class="panel-heading style_thead">
                                    <asp:Label ID="Label1" runat="server" Text="Important Notice:" style="font-size: 28px;color: crimson;"></asp:Label>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-body panelBody_edu_marginBottom">
                                        <table style="width: 100%" class="table table-condensed table-striped">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label2" runat="server" Text="1. Use black pen only." style="font-family: Calibri;font-size: 20px;"></asp:Label><br />
                                                    <asp:Label ID="Label3" runat="server" Text="2. Use of cell phone, calculator, watch and other electronic gadget in exam hall is strictly prohibited." style="font-family: Calibri;font-size: 20px;"></asp:Label><br />
                                                    <asp:Label ID="Label5" runat="server" Text="3. Bring colour print out of ADMIT CARD." style="font-family: Calibri;font-size: 20px;"></asp:Label><br />
                                                    <asp:Label ID="Label4" runat="server" Text="4. Consider road condition and traffic jam at Mirpur-12 to reach exam venue in time." style="font-family: Calibri;font-size: 20px;"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                            </tr>

                                        </table>
                                    </div>
                                </div>

                            </div>
                            <%--<div class="col-md-2"></div>--%>
                        </div>
                        <%-- END COL-MD-12 --%>
                    </div>
        </ContentTemplate>
    </asp:UpdatePanel>


    <div class="row">
        <div class="col-md-12">
            <%--<div class="col-md-2"></div>--%>
            <div class="col-md-8">
            Please visit
            <asp:HyperLink NavigateUrl="http://www.bup.edu.bd" Target="_blank" runat="server">BUP Official Website </asp:HyperLink>
            for more information about Undergraduate and Graduate programs.
            </div>
            <%--<div class="col-md-2"></div>--%>
         </div>
    </div>

</asp:Content>