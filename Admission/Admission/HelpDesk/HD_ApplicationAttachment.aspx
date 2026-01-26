<%@ Page Title="" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="HD_ApplicationAttachment.aspx.cs" Inherits="Admission.Admission.HelpDesk.HD_ApplicationAttachment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
    <link href="../../Content/ApplicationForm.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-default">
                <div class="panel-heading panelHeaderHeight">
                    <ol class="breadcrumb">
                        <li><asp:HyperLink ID="hrefAppBasic" runat="server">Basic Info</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppPriority" runat="server">Program Priority</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppEducation" runat="server">Education</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppRelation" runat="server">Parent/Guardian</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppAddress" runat="server">Address</asp:HyperLink></li>
                        <li><asp:HyperLink ID="hrefAppAdditional" runat="server">Additional/Work Experience</asp:HyperLink></li>
                        <%--<li><asp:HyperLink ID="hrefAppFinGuar" runat="server">Financial Guarantor</asp:HyperLink></li>--%>
                        <li class="active"><asp:HyperLink ID="hrefAppAttachment" runat="server">Upload Photo</asp:HyperLink></li>
                    </ol>
                </div>
                <%-- end panel heading --%>
                <div class="panel-body">

                    <asp:Panel ID="messagePanel" runat="server">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        <br />
                    </asp:Panel>

                    <div class="row">
                        <div class="col-md-12">

                            <div class="alert alert-info" style="margin-bottom: 0px; padding: 5px;">
                                <strong>Note:</strong>
                                <ul style="font-weight: bold;">
                                    <li>Please Upload your Photo and Signature Image</li>
                                    <li>Your Photo size should be 150KB</li>
                                    <li>Your Signature Image size should be 150KB</li>
                                </ul>
                            </div>
                            <br />


                            <div class="col-md-4">
                                <div class="panel panel-default">
                                    <div class="panel-heading style_thead">
                                        Photo
                                    </div>
                                    <div class="panel-body">
                                        <%--<asp:UpdatePanel ID="updPanelPhotoUpload" runat="server" ChildrenAsTriggers="true">
                                            <ContentTemplate>--%>
                                        <div style="text-align: center">
                                            <asp:Image ID="ImagePhoto" runat="server"
                                                ImageUrl="~/Images/AppImg/user7.jpg"
                                                Width="154" Height="154" />
                                        </div>
                                        <div style="color: crimson; font-weight:bold;"><sup>File size: 150KB</sup></div>
                                        <%--<asp:FileUpload ID="FileUploadPhoto" runat="server" AllowMultiple="false" accept="image/*" />
                                        <asp:RegularExpressionValidator ID="rexvPhoto" runat="server" ControlToValidate="FileUploadPhoto"
                                            ErrorMessage="Only .jpg, .png, and .jpeg" Display="Dynamic" ForeColor="Crimson"
                                            ValidationExpression="(.*\.([Jj][Pp][Gg])|.*\.([pP][nN][gG])|.*\.([Jj][Pp][Ee][Gg])$)"></asp:RegularExpressionValidator>
                                        <asp:Button ID="btnUploadPhoto" runat="server"
                                            Text="Upload Photo"
                                            Style="display: none"
                                            OnClick="btnUploadPhoto_Click" />--%>
                                        <asp:Panel ID="messagePanel_Photo" runat="server">
                                            <asp:Label ID="lblMessagePhoto" runat="server"></asp:Label>
                                        </asp:Panel>
                                        <%--</ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnUploadPhoto" />
                                            </Triggers>
                                        </asp:UpdatePanel>--%>
                                    </div>
                                </div>
                            </div>
                            <%-- PHOTO --%>
                            <div class="col-md-4">
                                <div class="panel panel-default">
                                    <div class="panel-heading style_thead">
                                        Signature
                                    </div>
                                    <div class="panel-body">
                                        <%--<asp:UpdatePanel ID="updPnlSignatureUpload" runat="server">
                                            <ContentTemplate>--%>
                                        <label>Upload signature with full name.</label>
                                        <div style="text-align: center">
                                            <asp:Image ID="ImageSignature" runat="server"
                                                ImageUrl="~/Images/AppImg/sign2.png"
                                                Width="256" Height="128" />
                                        </div>
                                        <div style="color: crimson; font-weight:bold;"><sup>File size: 150KB</sup></div>
                                        <%--<asp:FileUpload ID="FileUploadSignature" runat="server" AllowMultiple="false" accept="image/*" />
                                        <asp:RegularExpressionValidator ID="rexvSignature" runat="server" ControlToValidate="FileUploadSignature"
                                            ErrorMessage="Only .jpg, .png, and .jpeg" Display="Dynamic" ForeColor="Crimson"
                                            ValidationExpression="(.*\.([Jj][Pp][Gg])|.*\.([pP][nN][gG])|.*\.([Jj][Pp][Ee][Gg])$)"></asp:RegularExpressionValidator>
                                        <asp:Button ID="btnUploadSignature" runat="server"
                                            Text="Upload signature"
                                            Style="display: none"
                                            OnClick="btnUploadSignature_Click" />--%>
                                        <asp:Panel ID="messagePanel_Sign" runat="server">
                                            <asp:Label ID="lblMessageSign" runat="server"></asp:Label>
                                        </asp:Panel>
                                        <%--</ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnUploadSignature" />
                                            </Triggers>
                                        </asp:UpdatePanel>--%>
                                    </div>
                                </div>
                            </div>
                            <%-- SIGNATURE --%>

                        </div>
                        <%-- END COL-MD-12 --%>
                    </div>
                    <%-- END ROW --%>

                    <%--<asp:Button ID="btnNext" runat="server" Text="Next" Visible="false"
                        CssClass="btn btn-primary" />--%>

                </div>
                <%-- end panel body --%>
            </div>
            <%-- end panel default --%>
        </div>
        <%-- end col-md-12 --%>
    </div>
    <%-- end row --%>


</asp:Content>
