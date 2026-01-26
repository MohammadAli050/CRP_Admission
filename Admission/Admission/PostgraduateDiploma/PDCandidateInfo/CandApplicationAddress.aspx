<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="CandApplicationAddress.aspx.cs" Inherits="Admission.Admission.PostgraduateDiploma.PDCandidateInfo.CandApplicationAddress" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <script src="../../Candidate/Scripts/AddressValidation.js"></script>
    <link href="../../../Content/ApplicationForm.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


        <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading panelHeaderHeight">
                    <ol class="breadcrumb">
                        <li><asp:HyperLink ID="hrefAppBasic" runat="server">Basic Info</asp:HyperLink></li>
                        <%--<li><asp:HyperLink ID="hrefAppPriority" runat="server">Program Priority</asp:HyperLink></li>--%>
                        <li><asp:HyperLink ID="hrefAppEducation" runat="server" >Education</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppRelation" runat="server">Parent/Guardian</asp:HyperLink></li>
                        <li class="active"><asp:HyperLink ID="hrefAppAddress" runat="server" Enabled="false">Address</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppAdditional" runat="server">Additional/Work Experience</asp:HyperLink></li>
                        <%--<li><asp:HyperLink ID="hrefAppFinGuar" runat="server">Financial Guarantor</asp:HyperLink></li>--%>
                        <li><asp:HyperLink ID="hrefAppAttachment" runat="server">Upload Photo</asp:HyperLink></li>
                    </ol>
                </div>
                <%-- end panel heading --%>
                <div class="panel-body">


                    
                    <asp:UpdatePanel ID="updatePanelAddress" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>

                            <div class="row">
                                <div class="col-md-12">

                                    <div class="col-md-6">
                                        <div class="panel panel-default">
                                            <div class="panel-heading style_thead">
                                                Present/Mailing Address
                                            </div>
                                            <div class="panel-body panelBody_edu_marginBottom">
                                                <table style="width: 100%" class="table table-condensed">
                                                    <tr>
                                                        <td style="width: 31%" class="style_td">Mailing Address <span class="spanAsterisk">*</span><br />
                                                            (Postal)</td>
                                                        <td style="width: 69%">
                                                            <asp:TextBox ID="txtPresentAddress" runat="server" Width="100%" CssClass="form-control"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Divsion <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlPresentDivision" runat="server" Width="85%" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlPresentDivision_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">District <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlPresentDistrict" runat="server" Width="85%" CssClass="form-control" Enabled="false"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Upazila</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPresentUpazila" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Country <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlPresentCountry" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Post Code <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtPresentPostCode" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td class="style_td">Mobile</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPresentMobile" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Email</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPresentEmail" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td class="style_td">Telephone No.</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPresentPhone" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <%-- PRESENT ADDRESS --%>
                                    <div class="col-md-6">
                                        <div class="panel panel-default">
                                            <div class="panel-heading style_thead">
                                                Permanent Address
                                            </div>
                                            <div class="panel-body panelBody_edu_marginBottom">
                                                <table style="width: 100%" class="table table-condensed">
                                                    <tr>
                                                        <td style="width: 31%" class="style_td">Mailing Address <span class="spanAsterisk">*</span><br />
                                                            (Postal)</td>
                                                        <td style="width: 69%">
                                                            <asp:TextBox ID="txtPermanentAddress" runat="server" Width="100%" CssClass="form-control"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Divsion <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlPermanentDivision" runat="server" Width="85%" CssClass="form-control"
                                                                AutoPostBack="true" OnSelectedIndexChanged="ddlPermanentDivision_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">District <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlPermanentDistrict" runat="server" Width="85%" CssClass="form-control" Enabled="false"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Upazila</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPermanentUpazila" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Country <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlPermanentCountry" runat="server" Width="85%" CssClass="form-control"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Post Code <span class="spanAsterisk">*</span></td>
                                                        <td>
                                                            <asp:TextBox ID="txtPermanentPostCode" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td class="style_td">Mobile</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPermanentMobile" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="style_td">Email</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPermanentEmail" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td class="style_td">Telephone No.</td>
                                                        <td>
                                                            <asp:TextBox ID="txtPermanentPhone" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <%-- PERMANENT ADDRESS --%>
                                </div>
                                <%-- END COL-MD-12 --%>
                            </div>
                            <%-- END ROW --%>

                            <asp:Panel ID="messagePanel_Address" runat="server">
                                <asp:Label ID="lblMessageAddress" runat="server" Text=""></asp:Label>
                            </asp:Panel>

                            <asp:Button ID="btnSave_Address" runat="server" Text="Save"
                                CssClass="btn btn-primary" OnClick="btnSave_Address_Click"/>

                            <asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                                CssClass="btn btn-primary"/>

                            <span id="validationMsg" class="validationErrorMsg"></span>

                        </ContentTemplate>
                    </asp:UpdatePanel>



                </div>
                <%-- end panel body --%>
            </div>
            <%-- end panel default --%>
        </div>
        <%-- end col-md-12 --%>
    </div>
    <%-- end row --%>





</asp:Content>
